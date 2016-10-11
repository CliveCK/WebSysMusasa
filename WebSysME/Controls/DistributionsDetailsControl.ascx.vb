Imports BusinessLogic

Partial Class DistributionsDetailsControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

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

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboDistributionType

                .DataSource = objLookup.Lookup("luDistributionTypes", "DistributionTypeID", "Description").Tables(0)
                .DataValueField = "DistributionTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboOrganization

                .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name").Tables(0)
                .DataValueField = "OrganizationID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboCommodity

                .DataSource = objLookup.Lookup("luCommodities", "CommodityID", "Description").Tables(0)
                .DataValueField = "CommodityID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboUnits

                .DataSource = objLookup.Lookup("luCommodityUnits", "CommodityUnitID", "Description").Tables(0)
                .DataValueField = "CommodityUnitID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadDistributions(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadDistributions(ByVal DistributionID As Long) As Boolean

        Try

            Dim objDistributions As New Distributions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objDistributions

                If .Retrieve(DistributionID) Then

                    txtDistributionID1.Text = .DistributionID
                    If Not IsNothing(cboDistributionType.Items.FindByValue(.DistributionTypeID)) Then cboDistributionType.SelectedValue = .DistributionTypeID
                    If Not IsNothing(cboOrganization.Items.FindByValue(.OrganizationID)) Then cboOrganization.SelectedValue = .OrganizationID
                    If Not IsNothing(cboCommodity.Items.FindByValue(.OrganizationID)) Then cboCommodity.SelectedValue = .CommodityID
                    If Not IsNothing(cboUnits.Items.FindByValue(.UnitID)) Then cboUnits.SelectedValue = .UnitID
                    txtName.Text = .Name
                    If Not .DistributionDate = "" Then radDate.SelectedDate = .DistributionDate
                    txtDescription.Text = .Description
                    txtLocation.Text = .Location
                    txtQuantityPerBen.Text = .QuantityPerBeneficiary


                    ShowMessage("Distributions loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadDistributions: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function Save() As Boolean

        Try

            Dim objDistributions As New Distributions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objDistributions

                .DistributionID = IIf(IsNumeric(txtDistributionID1.Text), txtDistributionID1.Text, 0)
                If cboDistributionType.SelectedIndex > 0 Then .DistributionTypeID = cboDistributionType.SelectedValue
                If cboOrganization.SelectedIndex > 0 Then .OrganizationID = cboOrganization.SelectedValue
                If cboCommodity.SelectedIndex > 0 Then .CommodityID = cboCommodity.SelectedValue
                If cboUnits.SelectedIndex > 0 Then .UnitID = cboUnits.SelectedValue
                .Name = txtName.Text
                If radDate.SelectedDate.HasValue Then .DistributionDate = radDate.SelectedDate
                .Description = txtDescription.Text
                .Location = txtLocation.Text
                .QuantityPerBeneficiary = txtQuantityPerBen.Text

                If .Save Then

                    If Not IsNumeric(txtDistributionID1.Text) OrElse Trim(txtDistributionID1.Text) = 0 Then txtDistributionID1.Text = .DistributionID
                    LoadDistributions(.DistributionID)
                    ShowMessage("Distribution saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtDistributionID1.Text = ""
        If Not IsNothing(cboDistributionType.Items.FindByValue("")) Then
            cboDistributionType.SelectedValue = ""
        ElseIf Not IsNothing(cboDistributionType.Items.FindByValue(0)) Then
            cboDistributionType.SelectedValue = 0
        Else
            cboDistributionType.SelectedIndex = -1
        End If
        If Not IsNothing(cboOrganization.Items.FindByValue("")) Then
            cboOrganization.SelectedValue = ""
        ElseIf Not IsNothing(cboOrganization.Items.FindByValue(0)) Then
            cboOrganization.SelectedValue = 0
        Else
            cboOrganization.SelectedIndex = -1
        End If
        If Not IsNothing(cboCommodity.Items.FindByValue("")) Then
            cboCommodity.SelectedValue = ""
        ElseIf Not IsNothing(cboCommodity.Items.FindByValue(0)) Then
            cboCommodity.SelectedValue = 0
        Else
            cboCommodity.SelectedIndex = -1
        End If
        txtName.Text = ""
        radDate.Clear()
        txtDescription.Text = ""
        txtLocation.Text = ""
        txtQuantityPerBen.Text = ""

    End Sub

    Private Sub lnkBeneficiaries_Click(sender As Object, e As EventArgs) Handles lnkBeneficiaries.Click

        If IsNumeric(txtDistributionID1.Text) Then
            Response.Redirect("~/DistributionBeneficiaryPage.aspx?id=" & objUrlEncoder.Encrypt(txtDistributionID1.Text))
        Else
            ShowMessage("Please save Distribution first!...", MessageTypeEnum.Error)
        End If

    End Sub
End Class

