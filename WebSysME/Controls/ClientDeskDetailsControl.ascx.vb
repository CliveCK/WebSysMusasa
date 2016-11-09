Imports BusinessLogic

Partial Class ClientDeskDetailsControl
    Inherits System.Web.UI.UserControl

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

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            If Not IsNothing(Request.QueryString("id")) Then

                LoadClientDesk(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadClientDesk(ByVal ClientDeskInforID As Long) As Boolean

        Try

            Dim objClientDesk As New ClientDesk(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objClientDesk

                If .Retrieve(ClientDeskInforID) Then

                    txtCleintDeskInforID.Text = .ClientDeskInforID
                    txtAge.Text = .Age
                    txtName.Text = .Name
                    cboSex.SelectedValue = .Sex
                    txtWhereFrom.Text = .WhereFrom
                    txtInformationProvided.Text = .InformationProvided

                    ShowMessage("ClientDesk loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadClientDesk: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objClientDesk As New ClientDesk(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objClientDesk

                .ClientDeskInforID = IIf(IsNumeric(txtCleintDeskInforID.Text), txtCleintDeskInforID.Text, 0)
                .Age = txtAge.Text
                .Name = txtName.Text
                .Sex = cboSex.SelectedValue
                .WhereFrom = txtWhereFrom.Text
                .InformationProvided = txtInformationProvided.Text

                If .Save Then

                    If Not IsNumeric(txtCleintDeskInforID.Text) OrElse Trim(txtCleintDeskInforID.Text) = 0 Then txtCleintDeskInforID.Text = .ClientDeskInforID
                    ShowMessage("ClientDesk saved successfully...", MessageTypeEnum.Information)

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

        txtCleintDeskInforID.Text = ""
        txtAge.Text = 0
        txtName.Text = ""
        cboSex.SelectedValue = ""
        txtWhereFrom.Text = ""
        txtInformationProvided.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub
End Class

