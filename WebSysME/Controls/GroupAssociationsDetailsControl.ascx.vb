Imports BusinessLogic

Partial Class GroupAssociationsDetailsControl
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

                LoadGroupAssociations(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadGroupAssociations(ByVal GroupAssociationID As Long) As Boolean

        Try

            Dim objGroupAssociations As New GroupAssociations(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGroupAssociations

                If .Retrieve(GroupAssociationID) Then

                    txtGroupAssociationID.Text = .GroupAssociationID
                    txtAssociation.Text = .Association
                    txtDescription.Text = .Description

                    ShowMessage("GroupAssociations loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadGroupAssociations: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objGroupAssociations As New GroupAssociations(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGroupAssociations

                .GroupAssociationID = IIf(IsNumeric(txtGroupAssociationID.Text), txtGroupAssociationID.Text, 0)
                .Association = txtAssociation.Text
                .Description = txtDescription.Text

                If .Save Then

                    If Not IsNumeric(txtGroupAssociationID.Text) OrElse Trim(txtGroupAssociationID.Text) = 0 Then txtGroupAssociationID.Text = .GroupAssociationID
                    ShowMessage("GroupAssociations saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save Group Associations", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtGroupAssociationID.Text = ""
        txtAssociation.Text = ""
        txtDescription.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub
End Class

