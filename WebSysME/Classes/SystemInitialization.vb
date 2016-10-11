Imports SecurityPolicy.SecurityPolicy

Public Class SystemInitialization

    Shared Sub EnforceUserSecurity()

        Dim objUser As New Global.SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        objSecurityPolicy.RetrieveActiveSecurityPolicy()

        If objUser.Retrieve(CookiesWrapper.thisUserID) Then

            If objSecurityPolicy.PasswordExpires AndAlso objUser.MustChangePassword Then

                HttpContext.Current.RewritePath("~/MyInfo.aspx", False)

            End If

        End If

    End Sub

    Shared Sub EnforceUserMenuSecurity(ByVal RequestedPage As String)

        Dim objUser As New Global.SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If RequestedPage = "" Or RequestedPage = "default.aspx" Then

        Else

            If CookiesWrapper.thisUserID > 0 Then

                If IsNothing(CacheWrapper.MenuRightsCache) Then CacheWrapper.MenuRightsCache.BeginInit()
                If IsNothing(CacheWrapper.PageRightsCache) Then CacheWrapper.PageRightsCache.BeginInit()
                If IsNothing(CacheWrapper.MenuRightsCache) Then CacheWrapper.MenuItemsCache.BeginInit()

                Dim dsRights As DataSet = CacheWrapper.MenuRightsCache
                Dim dsMenuItems As DataSet = CacheWrapper.MenuItemsAllCache
                Dim dsPageRights As DataSet = CacheWrapper.PageRightsCache
                Dim HasRights As Boolean

                If dsMenuItems.Tables(0).Rows.Count > 0 Then

                    Dim row As DataRow() = dsMenuItems.Tables(0).Select("URL LIKE '%" & RequestedPage & "%'")

                    If row.Length > 0 Then
                        If row(0).ItemArray(5) = 1 Then
                            If dsRights.Tables(0).Rows.Count > 0 Then
                                ' Search for row
                                Dim dsRightsRow As DataRow() = dsRights.Tables(0).Select("URL LIKE '%" & RequestedPage & "%'")

                                If dsRightsRow.Length <= 0 Then

                                    HasRights = False
                                    HttpContext.Current.Response.Redirect((HttpContext.Current.Request.Url.AbsoluteUri.Replace((HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).Substring(1), "") + "/RestrictedAccess.aspx?msg=Deny"))

                                Else
                                    HasRights = True
                                End If

                            End If

                            If dsPageRights.Tables(0).Rows.Count > 0 Then
                                ' Search for row
                                Dim dsPageRightsRow As DataRow() = dsPageRights.Tables(0).Select("URL LIKE '%" & RequestedPage & "%'")

                                If dsPageRightsRow.Length <= 0 Then

                                    HasRights = False
                                    HttpContext.Current.Response.Redirect((HttpContext.Current.Request.Url.AbsoluteUri.Replace((HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).Substring(1), "") + "/RestrictedAccess.aspx?msg=Deny"))

                                Else
                                    HasRights = True
                                End If

                            End If
                        End If
                    End If

                End If


            End If
        End If

    End Sub

    Shared Function EnforceUserFunctionalitySecurity(ByVal FuctionalityID As Long) As Boolean

        '   Dim objUser As New Global.SpectrumITS.MemberManagement.BusinessLogic.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Dim objFunctionalityPermisions As New SysPermissionsManager.Functionality(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objFunctionalityPermisions.CheckFunctionalityPermission(CookiesWrapper.thisUserID, FuctionalityID)

    End Function

End Class
