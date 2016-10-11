Imports Universal.CommonFunctions
Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class SubOfficesDetailsControl
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

            If Not IsNothing(Request.QueryString("id")) Then

                Dim objOrganization As New BusinessLogic.Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                lblOrgName.Text = Catchnull(objOrganization.GetOrganization(objUrlEncoder.Decrypt(Request.QueryString("id"))).Tables(0).Rows(0)("Name"), "No Name")
                LoadGrid(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadSubOffices(ByVal SubOfficeID As Long) As Boolean

        Try

            Dim objSubOffices As New SubOffices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSubOffices

                If .Retrieve(SubOfficeID) Then

                    txtSubOfficeID.Text = .SubOfficeID
                    txtOrganizationID.Text = .OrganizationID
                    txtContactNo.Text = .ContactNo
                    txtFax.Text = .Fax
                    txtName.Text = .Name
                    txtEmail.Text = .Email
                    txtPhysicalAddress.Text = .PhysicalAddress

                    ShowMessage("SubOffices loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadSubOffices: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub LoadGrid(OrganizationID)

        Dim objSubOffices As New SubOffices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radSubListing

            .DataSource = objSubOffices.GetSubOffices("SELECT * FROM tblSubOffices where OrganizationID  = " & OrganizationID).Tables(0)
            .DataBind()

            ViewState("SubOff") = .DataSource

        End With

    End Sub

    Private Sub radSubListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radSubListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radSubListing.Items(index)

            Select Case e.CommandName

                Case "View"

                    LoadSubOffices(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End Select

        End If

    End Sub

    Public Function Save() As Boolean

        Try

            Dim objSubOffices As New SubOffices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSubOffices

                .SubOfficeID = IIf(IsNumeric(txtSubOfficeID.Text), txtSubOfficeID.Text, 0)
                If Not IsNothing(Request.QueryString("id")) AndAlso IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("id"))) Then .OrganizationID = objUrlEncoder.Decrypt(Request.QueryString("id")) Else ShowMessage("No Organization. Error!", MessageTypeEnum.Error) : Exit Function
                .ContactNo = IIf(IsNumeric(txtContactNo.Text), txtContactNo.Text, 0)
                .Fax = IIf(IsNumeric(txtFax.Text), txtFax.Text, 0)
                .Name = txtName.Text
                .Email = txtEmail.Text
                .PhysicalAddress = txtPhysicalAddress.Text

                If .Save Then

                    If Not IsNumeric(txtSubOfficeID.Text) OrElse Trim(txtSubOfficeID.Text) = 0 Then txtSubOfficeID.Text = .SubOfficeID
                    LoadGrid(.OrganizationID)
                    ShowMessage("SubOffices saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error while saving...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtSubOfficeID.Text = ""
        txtOrganizationID.Text = ""
        txtContactNo.Text = 0
        txtFax.Text = 0
        txtName.Text = ""
        txtEmail.Text = ""
        txtPhysicalAddress.Text = ""

    End Sub

    Private Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click

        Response.Redirect("~/OurOrganization.aspx?id=" & Request.QueryString("id"))

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub radSubListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radSubListing.NeedDataSource

        radSubListing.DataSource = DirectCast(ViewState("SubOff"), DataTable)

    End Sub
End Class

