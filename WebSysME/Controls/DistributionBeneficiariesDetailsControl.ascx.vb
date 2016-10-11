Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class DistributionBeneficiariesDetailsControl
    Inherits System.Web.UI.UserControl

    Dim ds As DataSet
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageTypeEnum)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageTypeEnum)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            If Not IsNothing(Request.QueryString("id")) AndAlso IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("id"))) Then

                Dim objLookup As New BusinessLogic.CommonFunctions

                With cboDistribution

                    .DataSource = objLookup.Lookup("tblDistributions", "DistributionID", "Name", , "DistributionID = " & Request.QueryString("id")).Tables(0)
                    .DataValueField = "TrainingID"
                    .DataTextField = "Name"
                    .DataBind()

                End With

                With cboBeneficiaryType

                    .DataSource = objLookup.Lookup("luBeneficiaryTypes", "BeneficiaryTypeID", "Description").Tables(0)
                    .DataValueField = "BeneficiaryTypeID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                    .SelectedIndex = 0

                End With

                With cboUnit

                    .DataSource = objLookup.Lookup("luCommodityUnits", "CommodityUnitID", "Description").Tables(0)
                    .DataValueField = "CommodityUnitID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                    .SelectedIndex = 0

                End With

                With cboCommodity

                    .DataSource = objLookup.Lookup("luCommodities", "CommodityID", "Description").Tables(0)
                    .DataValueField = "CommodityID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                    .SelectedIndex = 0

                End With

            End If

        End If

    End Sub

    Protected Function GetSelectedBeneficiaryIDs() As String

        Dim BeneficiaryIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radBeneficiaries.SelectedItems
            BeneficiaryIDArray.Add(gridRow.Item("ObjectID").Text.ToString())
        Next

        Return String.Join(",", BeneficiaryIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Beneficiary() As String = GetSelectedBeneficiaryIDs.Split(",")

        If Beneficiary.Length > 0 Then

            For i As Long = 0 To Beneficiary.Length - 1

                Dim objDistributionBeneficiaries As New BusinessLogic.DistributionBeneficiaries(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objDistributionBeneficiaries

                    .BeneficiaryTypeID = cboBeneficiaryType.SelectedValue
                    .DistributionID = Request.QueryString("id")
                    .BeneficiaryID = Beneficiary(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub LoadGrid()

        Dim sql As String = DertemineBeneficiaryTypeSQL(cboBeneficiaryType.SelectedItem.Text)
        ViewState("DistributionBeneficiaries") = Nothing

        If sql <> "" Then

            With radBeneficiaries

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()

                ViewState("DistributionBeneficiaries") = .DataSource

            End With

        End If

    End Sub

    Public Function DertemineBeneficiaryTypeSQL(ByVal Type As String) As String

        Dim sql As String = ""

        Select Case Type

            Case "Community"
                sql = "SELECT CommunityID As ObjectID, Name, Description FROM tblCommunities"

            Case "Group"
                sql = "SELECT GroupID As ObjectID, GroupName, Description FROM tblGroups"

            Case "Organization"
                sql = "SELECT OrganizationID As ObjectID, Name FROM tblOrganization"

            Case "School"
                sql = "SELECT SchoolID As ObjectID, Name FROM tblSchools"

            Case "HealthCenter"
                sql = "SELECT HealthCenterID as ObjectID, Name, Description FROM tblHealthCenters"

            Case "Household"
                sql = "SELECT BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries WHERE Suffix = 1"

            Case "Individual"
                sql = "SELECT BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries"

        End Select

        Return sql

    End Function

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If Map() Then

            Dim sql As String = "SELECT * FROM tblDistributionBeneficiaries WHERE DistributionID = " & Request.QueryString("id") & " AND BeneficiaryTypeID =" & cboBeneficiaryType.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

            LoadGrid()
            ShowMessage("Distribution Beneficiaries saved successfully...", MessageTypeEnum.Information)

        Else

            ShowMessage("Failed to save beneficiaries...", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadDistributionBeneficiaries(ByVal DistributionBeneficiaryID As Long) As Boolean

        Try

            Dim objDistributionBeneficiaries As New DistributionBeneficiaries(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objDistributionBeneficiaries

                If .Retrieve(DistributionBeneficiaryID) Then

                    txtDistributionBeneficiaryID.Text = .DistributionBeneficiaryID
                    If Not IsNothing(cboDistribution.Items.FindByValue(.DistributionID)) Then cboDistribution.SelectedValue = .DistributionID
                    If Not IsNothing(cboBeneficiaryType.Items.FindByValue(.BeneficiaryTypeID)) Then cboBeneficiaryType.SelectedValue = .BeneficiaryTypeID
                    If Not IsNothing(cboCommodity.Items.FindByValue(.CommodityID)) Then cboCommodity.SelectedValue = .CommodityID
                    If Not IsNothing(cboUnit.Items.FindByValue(.UnitID)) Then cboUnit.SelectedValue = .UnitID
                    txtQuantity.Text = .Quantity

                    ShowMessage("DistributionBeneficiaries loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadDistributionBeneficiaries: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtDistributionBeneficiaryID.Text = ""
        If Not IsNothing(cboDistribution.Items.FindByValue("")) Then
            cboDistribution.SelectedValue = ""
        ElseIf Not IsNothing(cboDistribution.Items.FindByValue(0)) Then
            cboDistribution.SelectedValue = 0
        Else
            cboDistribution.SelectedIndex = -1
        End If
        If Not IsNothing(cboBeneficiaryType.Items.FindByValue("")) Then
            cboBeneficiaryType.SelectedValue = ""
        ElseIf Not IsNothing(cboBeneficiaryType.Items.FindByValue(0)) Then
            cboBeneficiaryType.SelectedValue = 0
        Else
            cboBeneficiaryType.SelectedIndex = -1
        End If
        If Not IsNothing(cboCommodity.Items.FindByValue("")) Then
            cboCommodity.SelectedValue = ""
        ElseIf Not IsNothing(cboCommodity.Items.FindByValue(0)) Then
            cboCommodity.SelectedValue = 0
        Else
            cboCommodity.SelectedIndex = -1
        End If
        If Not IsNothing(cboUnit.Items.FindByValue("")) Then
            cboUnit.SelectedValue = ""
        ElseIf Not IsNothing(cboUnit.Items.FindByValue(0)) Then
            cboUnit.SelectedValue = 0
        Else
            cboUnit.SelectedIndex = -1
        End If
        txtQuantity.Text = 0.0

    End Sub

    Private Sub radBeneficiaries_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radBeneficiaries.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radBeneficiaries.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objAttendants As New BusinessLogic.DistributionBeneficiaries(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objAttendants

                        .DistributionID = Request.QueryString("id")
                        .BeneficiaryID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radBeneficiaries_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radBeneficiaries.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

            If ds.Tables(0).Select("BeneficiaryID = " & gridItem("ObjectID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already selected..."
                btnImage.Visible = True

            End If

        End If

    End Sub

    Private Sub radBeneficiaries_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radBeneficiaries.NeedDataSource

        radBeneficiaries.DataSource = DirectCast(ViewState("DistributionBeneficiaries"), DataTable)

    End Sub

    Private Sub cboBeneficiaryType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBeneficiaryType.SelectedIndexChanged

        LoadGrid()

    End Sub

    Private Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click

        Response.Redirect("~/DistributionsPage.aspx?id=" & Request.QueryString("id"))

    End Sub
End Class

