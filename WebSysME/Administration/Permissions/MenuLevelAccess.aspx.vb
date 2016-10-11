Imports SysPermissionsManager.Functionality

Partial Public Class MenuLevelAccess
    Inherits System.Web.UI.Page
    Private objUserPermissions As New Global.SysPermissionsManager.Functionality(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.ChangeAccessPermissions) Then

            Response.Redirect("~/RestrictedAccess.aspx?msg=DenyAccess")

        End If

    End Sub



End Class