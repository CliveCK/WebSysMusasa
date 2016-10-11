Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class GroupMembers
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Not IsNothing(Request.QueryString("id")) AndAlso IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("id"))) Then

                Dim objLookup As New BusinessLogic.CommonFunctions

                With cboGroup

                    .DataSource = objLookup.Lookup("tblGroups", "GroupID", "GroupName", , "GroupID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))).Tables(0)
                    .DataValueField = "GroupID"
                    .DataTextField = "GroupName"
                    .DataBind()

                End With

                LoadGrid()

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

                If Request.QueryString("type") = "Committee" Then

                    Dim objDistributionBeneficiaries As New BusinessLogic.GroupComitteAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objDistributionBeneficiaries

                        .GroupID = objUrlEncoder.Decrypt(Request.QueryString("id"))
                        .AttendantID = Beneficiary(i)

                        If Not .Save Then

                            log.Error("Error saving...")
                            Return False

                        End If

                    End With

                Else

                    Dim objDistributionBeneficiaries As New BusinessLogic.HouseholdGroups(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objDistributionBeneficiaries

                        .GroupID = objUrlEncoder.Decrypt(Request.QueryString("id"))
                        .HouseholdID = Beneficiary(i)

                        If Not .Save Then

                            log.Error("Error saving...")
                            Return False

                        End If

                    End With

                End If

            Next

        End If

        Return True

    End Function

    Private Sub LoadGrid()

        Dim sql1 As String = ""

        Dim sql As String = ""

        If Request.QueryString("type") = "Committee" Then

            sql = "Select B.BeneficiaryID as ObjectID, B.FirstName + ' ' + B.Surname As Name, G.GroupName as Group From tblBeneficiaries B "
            sql &= " inner Join tblHouseholdGroups HG On B.BeneficiaryID = HG.HouseholdID inner join tblGroups G on G.GroupID = HG.GroupID"
            sql &= " Where Suffix = 1"

            sql1 = "Select AttendantID as BeneficiaryID, * FROM tblGroupComitteeAttendants WHERE GroupID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))

        Else

            sql = "Select BeneficiaryID as ObjectID, FirstName + ' ' + Surname As Name FROM tblBeneficiaries"

            sql1 = "Select HouseholdID as BeneficiaryID, * FROM tblHouseholdGroups WHERE GroupID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))

        End If

        ds = db.ExecuteDataSet(CommandType.Text, sql1)

        With radBeneficiaries

            .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
            .DataBind()

            ViewState("GroupBeneficiaries") = .DataSource

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Groupd Members saved successfully...", MessageTypeEnum.Information)

        Else

            ShowMessage("Failed To save members...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radBeneficiaries_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radBeneficiaries.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radBeneficiaries.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    If Request.QueryString("type") = "Committee" Then

                        Dim objAttendants As New BusinessLogic.GroupComitteAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                        With objAttendants

                            .GroupID = objUrlEncoder.Decrypt(Request.QueryString("id"))
                            .AttendantID = Server.HtmlDecode(e.CommandArgument)

                            If .DeleteEntries() Then

                                LoadGrid()
                                ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                            End If

                        End With

                    Else

                        Dim objAttendants As New BusinessLogic.GroupComitteAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                        With objAttendants

                            .GroupID = objUrlEncoder.Decrypt(Request.QueryString("id"))
                            .AttendantID = Server.HtmlDecode(e.CommandArgument)

                            If .DeleteBeneficiaryEntries() Then

                                LoadGrid()
                                ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                            End If

                        End With

                    End If

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

        radBeneficiaries.DataSource = DirectCast(ViewState("GroupBeneficiaries"), DataTable)

    End Sub

    Private Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click

        Response.Redirect("~/GroupsDetails.aspx?id=" & Request.QueryString("id"))

    End Sub
End Class

