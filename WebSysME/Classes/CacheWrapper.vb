'Imports SpectrumITS.DataManagement.SQLHeirarchies
'Imports SpectrumITS.MemberManagement.BusinessLogic

''**THIS FILE IS SHARED AMONGST MANY PROJECTS**

Partial Public Class CacheWrapper

    '    Private objCategories As NestedSetManager

    Public Shared Property MainMenuItemsCache() As DataSet

        Get

            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MainMenuItemsCache") Is Nothing Then

                Dim myMenuItems As New UserMenu(CookiesWrapper.thisConnectionName)
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MainMenuItemsCache") = myMenuItems.GetContextMenu("MainMenu", False)

            End If

            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MainMenuItemsCache"), DataSet)

        End Get

        Set(ByVal value As DataSet)

            If IsNothing(value) Then
                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MainMenuItemsCache")
            Else
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MainMenuItemsCache") = value
            End If

        End Set

    End Property

    '    Public Shared Property QuickIconsCache() As DataSet

    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "QuickIconsCache") Is Nothing Then

    '                Dim myQuickIcons As New QuickIconMenuItem(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "QuickIconsCache") = myQuickIcons.GetQuickIcons

    '            End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "QuickIconsCache"), DataSet)

    '        End Get

    '        Set(ByVal value As DataSet)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MainMenuItemsCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MainMenuItemsCache") = value
    '            End If

    '        End Set

    '    End Property

    Public Shared Property MenuItemsCache() As DataSet

        Get

            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuItemsCache") Is Nothing Then
                Dim myMenuItems As New UserMenu(CookiesWrapper.thisConnectionName)
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuItemsCache") = myMenuItems.GetContextMenu(False)

            End If

            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuItemsCache"), DataSet)

        End Get

        Set(ByVal value As DataSet)

            If IsNothing(value) Then
                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MenuItemsCache")
            Else
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuItemsCache") = value
            End If

        End Set

    End Property

    Public Shared Property MenuActionItemsCache() As DataSet

        Get

            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuActionItemsCache") Is Nothing Then
                Dim myMenuItems As New UserMenu(CookiesWrapper.thisConnectionName)
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuActionItemsCache") = myMenuItems.GetContextMenu(False, "MainMenu,Dashboard")

            End If

            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuActionItemsCache"), DataSet)

        End Get

        Set(ByVal value As DataSet)

            If IsNothing(value) Then
                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MenuActionItemsCache")
            Else
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuActionItemsCache") = value
            End If

        End Set

    End Property

    Public Shared Property MenuRightsCache() As DataSet

        Get

            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuRightsCache") Is Nothing Then

                Dim objMenulevelPermisions As New SysPermissionsManager.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuRightsCache") = objMenulevelPermisions.GetSelectedUserMenuRights(CookiesWrapper.thisUserID)

            End If

            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuRightsCache"), DataSet)

        End Get

        Set(ByVal value As DataSet)

            If IsNothing(value) Then
                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MenuRightsCache")
            Else
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuRightsCache") = value
            End If

        End Set

    End Property

    Public Shared Property FunctionalityRightsCache() As DataSet

        Get

            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "FunctionalityRightsCache") Is Nothing Then

                Dim objFunctionalityPermisions As New SysPermissionsManager.Functionality(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "FunctionalityRightsCache") = objFunctionalityPermisions.GetSelectedUserFunctionalityRights(CookiesWrapper.thisUserID)

            End If

            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "FunctionalityRightsCache"), DataSet)

        End Get

        Set(ByVal value As DataSet)

            If IsNothing(value) Then
                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "FunctionalityRightsCache")
            Else
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "FunctionalityRightsCache") = value
            End If

        End Set

    End Property

    Public Shared Property PageRightsCache() As DataSet

        Get

            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "PageRightsCache") Is Nothing Then
                Dim objMenulevelPermisions As New SysPermissionsManager.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "PageRightsCache") = objMenulevelPermisions.GetUserPageRights(CookiesWrapper.thisUserID)

            End If

            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "PageRightsCache"), DataSet)

        End Get

        Set(ByVal value As DataSet)

            If IsNothing(value) Then
                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "PageRightsCache")
            Else
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "PageRightsCache") = value
            End If

        End Set

    End Property

    '    Public Shared Property MailReceipientsCache() As DataSet

    '        Get

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MailReceipientsCache"), DataSet)

    '        End Get

    '        Set(ByVal value As DataSet)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MailReceipientsCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MailReceipientsCache") = value
    '            End If

    '        End Set

    '    End Property

    Public Shared Property ReportsCache() As DataSet

        Get

            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

            Return db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblReports")

        End Get

        Set(ByVal value As DataSet)

            If IsNothing(value) Then
                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "ReportsCache")
            Else
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "ReportsCache") = value
            End If

        End Set

    End Property

    '    Public Shared Property MemberTypeCache() As DataSet

    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MemberTypeCache") Is Nothing Then

    '                Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    '                Dim myMemberTypes As System.Data.DataSet = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM luMemberTypes")

    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MemberTypeCache") = myMemberTypes

    '            End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MemberTypeCache"), DataSet)

    '        End Get

    '        Set(ByVal value As DataSet)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MemberTypeCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MemberTypeCache") = value
    '            End If

    '        End Set

    '    End Property

    '    Public Shared Property BillingGroupCache() As DataSet

    '        Get

    '            'If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "BillingGroupCache") Is Nothing Then

    '            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    '            Dim myBillingGroups As System.Data.DataSet = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM luBillingGroups")

    '            HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "BillingGroupCache") = myBillingGroups

    '            'End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "BillingGroupCache"), DataSet)

    '        End Get

    '        Set(ByVal value As DataSet)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "BillingGroupCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "BillingGroupCache") = value
    '            End If

    '        End Set

    '    End Property

    '    Public Shared Property DistributionGroupsCache() As DataSet

    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "DistributionGroupsCache") Is Nothing Then

    '                Dim objCategories As NestedSetManager = New NestedSetManager(CookiesWrapper.thisConnectionName, "tblCategories", "LeftValue", "RightValue", "CategoryID", "Description", "ParentID", "TreeID", "Code")
    '                Dim myMenuItems As New UserMenu(CookiesWrapper.thisConnectionName)
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "DistributionGroupsCache") = objCategories.GetChildren(Options.Categories.DistributionGroupsID, False)

    '            End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "DistributionGroupsCache"), DataSet)

    '        End Get

    '        Set(ByVal value As DataSet)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "DistributionGroupsCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "DistributionGroupsCache") = value
    '            End If

    '        End Set

    '    End Property

    '    Public Shared Property SegregatedFundsCache() As DataTable

    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "SegregatedFundsCache") Is Nothing Then

    '                Dim objSegregatedFunds As New SegregatedFund(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    '                Dim dt As DataTable = objSegregatedFunds.GetAllSegregatedFunds().Tables(0)

    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "SegregatedFundsCache") = dt

    '            End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "SegregatedFundsCache"), DataTable)

    '        End Get

    '        Set(ByVal value As DataTable)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "SegregatedFundsCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "SegregatedFundsCache") = value
    '            End If

    '        End Set

    '    End Property

    '    Public Shared Property ActiveSegregatedFundsCache() As DataTable

    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "ActiveSegregatedFundsCache") Is Nothing Then

    '                Dim objSegregatedFunds As New SegregatedFund(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    '                Dim dt As DataTable = objSegregatedFunds.GetAllActiveSegregatedFunds().Tables(0)

    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "ActiveSegregatedFundsCache") = dt

    '            End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "ActiveSegregatedFundsCache"), DataTable)

    '        End Get

    '        Set(ByVal value As DataTable)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "ActiveSegregatedFundsCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "ActiveSegregatedFundsCache") = value
    '            End If

    '        End Set

    '    End Property

    '    Public Shared Property CategoriesCache() As DataTable

    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "CategoriesCache") Is Nothing Then

    '                Dim objCategories As NestedSetManager = New NestedSetManager(CookiesWrapper.thisConnectionName, "tblCategories", "LeftValue", "RightValue", "CategoryID", "Description", "ParentID", "TreeID", "Code")

    '                Dim dt As DataTable = objCategories.GetTrees.Tables(0)

    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "CategoriesCache") = dt

    '            End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "CategoriesCache"), DataTable)

    '        End Get

    '        Set(ByVal value As DataTable)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "CategoriesCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "CategoriesCache") = value
    '            End If

    '        End Set

    '    End Property

    '    Public Shared Property MySavedSearchesCache() As DataSet

    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MySavedSearchesCache") Is Nothing Then

    '                Dim myMenuItems As New UserMenu(CookiesWrapper.thisConnectionName)

    '                'HttpContext.Current.Cache(CookiesWrapper.thisConnectionName &"MySavedSearchesCache") = objSavedSearches.GetMySavedSearches(CookiesWrapper.thisUserID)

    '            End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MySavedSearchesCache"), DataSet)

    '        End Get

    '        Set(ByVal value As DataSet)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MySavedSearchesCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MySavedSearchesCache") = value
    '            End If

    '        End Set

    '    End Property

    '    Public Shared Property StatusCache() As DataTable

    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "StatusCache") Is Nothing Then

    '                Dim objCategories As NestedSetManager = New NestedSetManager(CookiesWrapper.thisConnectionName, "tblCategories", "LeftValue", "RightValue", "CategoryID", "Description", "ParentID", "TreeID", "Code")

    '                Dim dt As DataTable = objCategories.GetTrees.Tables(0)

    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "StatusCache") = dt

    '            End If

    '            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "StatusCache"), DataTable)

    '        End Get

    '        Set(ByVal value As DataTable)

    '            If IsNothing(value) Then
    '                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "StatusCache")
    '            Else
    '                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "StatusCache") = value
    '            End If

    '        End Set
    '    End Property

    '    Public Shared ReadOnly Property RegisteredCompany()
    '        Get

    '            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "RegisteredCompany") Is Nothing Then

    '                Dim objLicense As New SpectrumITS.Licensing.License(Options.LicenseFilePath)

    '                Try

    '                    If objLicense.LoadLicenseFile Then

    '                        HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "RegisteredCompany") = objLicense.Company

    '                    Else

    '                        HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "RegisteredCompany") = "UNREGISTERED"

    '                    End If

    '                Catch ex As Exception

    '                    HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "RegisteredCompany") = "UNREGISTERED"

    '                End Try

    '            End If

    '            Return HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "RegisteredCompany")

    '        End Get
    '    End Property

    Public Shared Property MenuItemsAllCache() As DataSet

        ' This cache stores all the menu items that are in the database lunmenu table. 

        Get

            If HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuItemsAllCache") Is Nothing Then
                Dim myMenuItems As New UserMenu(CookiesWrapper.thisConnectionName)
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuItemsAllCache") = myMenuItems.GetAllContextMenu(False)

            End If

            Return CType(HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuItemsAllCache"), DataSet)

        End Get

        Set(ByVal value As DataSet)

            If IsNothing(value) Then
                HttpContext.Current.Cache.Remove(CookiesWrapper.thisConnectionName & "MenuItemsAllCache")
            Else
                HttpContext.Current.Cache(CookiesWrapper.thisConnectionName & "MenuItemsAllCache") = value
            End If

        End Set

    End Property

End Class
