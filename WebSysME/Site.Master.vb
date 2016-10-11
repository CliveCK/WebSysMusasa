Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class SiteMaster
    Inherits MasterPage
    Private Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Private Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Private _antiXsrfTokenValue As String
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

    Protected Sub Page_Init(sender As Object, e As EventArgs)
        ' The code below helps to protect against XSRF attacks
        'Dim requestCookie = Request.Cookies(AntiXsrfTokenKey)
        'Dim requestCookieGuidValue As Guid
        'If requestCookie IsNot Nothing AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue) Then
        '    ' Use the Anti-XSRF token from the cookie
        '    _antiXsrfTokenValue = requestCookie.Value
        '    Page.ViewStateUserKey = _antiXsrfTokenValue
        'Else
        '    ' Generate a new Anti-XSRF token and save to the cookie
        '    _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
        '    Page.ViewStateUserKey = _antiXsrfTokenValue

        '    Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With { _
        '         .HttpOnly = True, _
        '         .Value = _antiXsrfTokenValue _
        '    }
        '    If FormsAuthentication.RequireSSL AndAlso Request.IsSecureConnection Then
        '        responseCookie.Secure = True
        '    End If
        '    Response.Cookies.[Set](responseCookie)
        'End If

        'AddHandler Page.PreLoad, AddressOf master_Page_PreLoad
    End Sub

    Protected Sub master_Page_PreLoad(sender As Object, e As EventArgs)
        'If Not IsPostBack Then
        '    ' Set Anti-XSRF token
        '    ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
        '    ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, [String].Empty)
        'Else
        '    ' Validate the Anti-XSRF token
        '    If DirectCast(ViewState(AntiXsrfTokenKey), String) <> _antiXsrfTokenValue OrElse DirectCast(ViewState(AntiXsrfUserNameKey), String) <> (If(Context.User.Identity.Name, [String].Empty)) Then
        '        Throw New InvalidOperationException("Validation of Anti-XSRF token failed.")
        '    End If
        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            hypUser.Text = CookiesWrapper.thisUserFullName

            With lblOrg

                .Text = Replace(CookiesWrapper.thisConnectionName, "WebSys", "")
                .ForeColor = Drawing.Color.DarkGreen
                .Font.Bold = True

            End With

            With lblOrgInfor

                If CookiesWrapper.thisUserName <> "Admin" Then

                    If CookiesWrapper.StaffID > 0 Then

                        Dim objStaffMembers As New BusinessLogic.StaffMember(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                        If objStaffMembers.Retrieve(CookiesWrapper.StaffID) Then

                            .Text = "<b>Organization:</b> " & objStaffMembers.Organization & "  <b>Position:</b> " & objStaffMembers.StaffPosition

                        End If

                    Else

                        .Text = "Account not linked to your staff profile! Contact your Administrator..."
                        .ForeColor = Drawing.Color.DarkRed

                    End If

                End If


            End With

            With radmMainMenu

                .DataSource = db.ExecuteDataSet(CommandType.Text, "Select * FROM luMenu WHERE MenuType = 'MainMenu' AND DrawMenu = 1 ORDER By ParentID, OrderIndex")
                .DataTextField = "MenuName"
                .DataValueField = "MenuID"
                .DataFieldID = "MenuID"
                .DataFieldParentID = "ParentID"
                .DataNavigateUrlField = "URL"
                .DataBind()

            End With

        End If

    End Sub

    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        'Context.GetOwinContext().Authentication.SignOut()
    End Sub

    Private Sub cmdLogout_Click(sender As Object, e As EventArgs) Handles cmdLogout.Click

        Dim myUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If CookiesWrapper.thisLogID <> 0 AndAlso CookiesWrapper.thisLogID <> -1 Then

            myUser.LogID = CookiesWrapper.thisLogID
            myUser.SaveUserLog(CookiesWrapper.thisUserID)

        End If

        FormsAuthentication.SignOut()

        CookiesWrapper.ClearCookies()

        Session.Abandon()

        Dim authCookie As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, "")
        authCookie.Expires = DateTime.Now.AddYears(-1)
        Response.Cookies.Add(authCookie)

        Dim sessionCookie As HttpCookie = New HttpCookie("ASP.NET_SessionId", "")
        sessionCookie.Expires = DateTime.Now.AddYears(-1)
        Response.Cookies.Add(sessionCookie)

        FormsAuthentication.RedirectToLoginPage()

        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

    End Sub

    Private Function HasMenuPermission(ByVal MenuID As Integer) As Boolean

        Dim ds As DataSet = GetMenuRights()

        If ds.Tables(0).Rows.Count > 0 Then
            ' Search for row
            Dim row As DataRow() = ds.Tables(0).Select("MenuID=" & MenuID)
            If row.Length > 0 Then

                Return True

            Else : Return False

            End If

        Else : Return False

        End If

    End Function

    Private Function GetMenuRights() As DataSet
        Try

            Dim objMenulevelPermisions As New SysPermissionsManager.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            Return objMenulevelPermisions.GetSelectedUserMenuRights(CookiesWrapper.thisUserID)

        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Private Sub radmMainMenu_ItemDataBound(sender As Object, e As Telerik.Web.UI.RadMenuEventArgs) Handles radmMainMenu.ItemDataBound

        Try

            If CookiesWrapper.thisUserName.ToLower <> "admin" Then

                Dim item As Telerik.Web.UI.RadMenuItem = e.Item
                Dim dataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
                Dim MenuID As Integer = dataRow("MenuID")

                If HasMenuPermission(MenuID) Then
                    item.Enabled = True
                Else
                    item.Enabled = False
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub
End Class