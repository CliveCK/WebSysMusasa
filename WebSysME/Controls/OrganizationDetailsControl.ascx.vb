Imports BusinessLogic

Partial Class OrganizationDetailsControl
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

            With cboOrganizationType

                .DataSource = objLookup.Lookup("luOrganizationTypes", "OrganizationTypeID", "Description").Tables(0)
                .DataValueField = "OrganizationTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadOrganization(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadOrganization(ByVal OrganizationID As Long) As Boolean

        Try

            Dim objOrganization As New Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objOrganization

                If .Retrieve(OrganizationID) Then

                    txtOrganizationID.Text = .OrganizationID
                    If Not IsNothing(cboOrganizationType.Items.FindByValue(.OrganizationTypeID)) Then cboOrganizationType.SelectedValue = .OrganizationTypeID
                    txtLongitude.Text = .Longitude
                    txtLatitude.Text = .Latitude
                    txtContactNo.Text = .ContactNo
                    txtCellPhoneNo.Text = .CellPhoneNo
                    txtName.Text = .Name
                    txtDescription.Text = .Description
                    txtContactName.Text = .ContactName
                    txtEmail.Text = .Email
                    txtAddress.Text = .Address
                    txtWebsiteAddress.Text = .WebsiteAddress

                    lnkSubOffices.Visible = True
                    ShowMessage("Organization loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Organization", MessageTypeEnum.Error)
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

            Dim objOrganization As New Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objOrganization

                .OrganizationID = IIf(IsNumeric(txtOrganizationID.Text), txtOrganizationID.Text, 0)
                .WardID = 0
                If cboOrganizationType.SelectedIndex > -1 Then .OrganizationTypeID = cboOrganizationType.SelectedValue
                If IsNumeric(txtLongitude.Text) Then .Longitude = txtLongitude.Text
                If IsNumeric(txtLatitude.Text) Then .Latitude = txtLatitude.Text
                .ContactNo = txtContactNo.Text
                .CellPhoneNo = txtCellPhoneNo.Text
                .Name = txtName.Text
                .Description = txtDescription.Text
                .ContactName = txtContactName.Text
                .Address = txtAddress.Text
                .Email = txtEmail.Text
                .WebsiteAddress = txtWebsiteAddress.Text

                If .Save Then

                    If Not IsNumeric(txtOrganizationID.Text) OrElse Trim(txtOrganizationID.Text) = 0 Then txtOrganizationID.Text = .OrganizationID
                    LoadOrganization(.OrganizationID)
                    lnkSubOffices.Visible = True
                    ShowMessage("Organization saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error during saving...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtOrganizationID.Text = ""
        If Not IsNothing(cboOrganizationType.Items.FindByValue("")) Then
            cboOrganizationType.SelectedValue = ""
        ElseIf Not IsNothing(cboOrganizationType.Items.FindByValue(0)) Then
            cboOrganizationType.SelectedValue = 0
        Else
            cboOrganizationType.SelectedIndex = -1
        End If
        txtLongitude.Text = 0.0
        txtLatitude.Text = 0.0
        txtContactNo.Text = ""
        txtCellPhoneNo.Text = ""
        txtName.Text = ""
        txtDescription.Text = ""
        txtContactName.Text = ""
        txtAddress.Text = ""
        txtEmail.Text = ""
        txtWebsiteAddress.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub lnkSubOffices_Click(sender As Object, e As EventArgs) Handles lnkSubOffices.Click

        Response.Redirect("~/SubOfficesPage.aspx?id=" & objUrlEncoder.Encrypt(txtOrganizationID.Text))

    End Sub
End Class

