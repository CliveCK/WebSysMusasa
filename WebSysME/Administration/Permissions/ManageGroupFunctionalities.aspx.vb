Imports SysPermissionsManager.Functionality

Partial Public Class ManageGroupFunctionalities
    Inherits System.Web.UI.Page
    Private objUserPermissions As New Global.SysPermissionsManager.Functionality(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.ChangeAccessPermissions) Then

                Response.Redirect("~/RestrictedAccess.aspx?msg=Deny")
                'Dim myEx As New ApplicationException("You are not athorised to view this page details.Contact your administrator")
                'Throw myEx

            End If

        Catch ex As Exception

            lblMessages.Text = ex.Message

        End Try

    End Sub

    Private Sub ReceiveControlMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum) Handles DefaultGroupPermissions1.ReceiveControlMessage, DefaultUserPermissions1.ReceiveControlMessage

        lblMessages.Text = Message
        lblMessages.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

End Class