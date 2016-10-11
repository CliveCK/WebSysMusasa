Imports System.ComponentModel
Imports System.Web

'**THIS FILE IS SHARED AMONGST MANY PROJECTS**

<Description("Provide access to cookies as if they were properties variables of a class.")> _
Partial Public Class CookiesWrapper

#Region "Variables"

    Private Shared mGotNothing As Boolean

    Private mUserID As Long
    Private mRequest As System.Web.HttpRequest
    Private mResponse As System.Web.HttpResponse
    Private mSearchType As String

    Private Shared NON_CLEARABLE_COOKIES As String = "ConnectionName,UserName,MemberType,SegregatedFundID"

#End Region

#Region "Properties"
    <Description("Check whether last access was to an existing cookie or a default value was returned.")> _
    Public ReadOnly Property GotNothing() As Boolean
        Get
            GotNothing = mGotNothing
        End Get
    End Property

#End Region

#Region "Subscription Cookies"

    <Description("Gets or sets the ProcessID saved into the ProcessID cookie.")> _
    Public Shared Property ProcessID() As Long
        Get
            Return GetCookie("ProcessID", 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("ProcessID", Value)
        End Set
    End Property

    <Description("Gets or sets the CategorySchemeID saved into the CategorySchemeID cookie.")> _
    Public Shared Property CategorySchemeID() As Long
        Get
            Return GetCookie("CategorySchemeID", 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("CategorySchemeID", Value)
        End Set
    End Property

    <Description("Gets or sets the SubscriptionSchemeID saved into the SubscriptionSchemeID cookie.")> _
    Public Shared Property SubscriptionSchemeID() As Long
        Get
            Return GetCookie("SubscriptionSchemeID", 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("SubscriptionSchemeID", Value)
        End Set
    End Property

    <Description("Gets or sets the Subscriptionid saved into the SubscriptionID cookie.")> _
    Public Shared Property SubscriptionID() As Long
        Get
            Return GetCookie("SubscriptionID", 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("SubscriptionID", Value)
        End Set
    End Property

    <Description("Gets or sets the StatementHeaderID saved into the SubscriptionID cookie.")> _
    Public Shared Property StatementHeaderID() As Long
        Get
            Return GetCookie("StatementHeaderID", 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("StatementHeaderID", Value)
        End Set
    End Property

    '<Description("Gets or sets the IsGeneratingBillingSchedule saved into the IsGeneratingBillingSchedule cookie.")> _
    ' Public Shared Property IsGeneratingBillingSchedule() As Boolean
    '    Get
    '        Return GetCookie("IsGeneratingBillingSchedule", False)
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        AddCookie("IsGeneratingBillingSchedule", Value)
    '    End Set
    'End Property

#End Region

#Region "User Cookies"

    <Description("Gets or sets a ApplicationSkin saved into the ApplicationSkin cookie.")> _
    Public Shared Property ApplicationSkin() As String
        Get
            Return GetCookie("ApplicationSkin", "mms")
        End Get
        Set(ByVal Value As String)
            AddCookie("ApplicationSkin", Value)
        End Set
    End Property

#End Region

#Region "User Cookies"

    <Description("Gets or sets a Connection Name saved into the ConnectionName cookie.")> _
    Public Shared Property thisConnectionName() As String
        Get
            Return GetCookie("thisConnectionName", "CustomFields")
        End Get
        Set(ByVal Value As String)
            AddCookie("thisConnectionName", Value)
        End Set
    End Property

    <Description("Gets or sets a Connection Name saved into the ConnectionName cookie.")> _
    Public Shared Property RadTreeSelectedNode() As String
        Get
            Return GetCookie("RadTreeSelectedNode", "CustomFields")
        End Get
        Set(ByVal Value As String)
            AddCookie("RadTreeSelectedNode", Value)
        End Set
    End Property

    <Description("Gets or sets a AdHocMessage saved into the AdHocMessage cookie.")> _
    Public Shared Property AdHocMessage() As String
        Get
            Return GetCookie("AdHocMessage", "CustomFields")
        End Get
        Set(ByVal Value As String)
            AddCookie("AdHocMessage", Value)
        End Set
    End Property

    <Description("Gets or sets the reportid saved into the ReportID cookie.")> _
    Public Shared Property thisReportID() As Long
        Get
            Return GetCookie("thisReportID", "0")
        End Get
        Set(ByVal Value As Long)
            AddCookie("thisReportID", Value)
        End Set
    End Property

    <Description("Gets or sets the OrganizationId saved into the OrganizationID cookie.")> _
    Public Shared Property OrganizationID() As Long
        Get
            Return GetCookie("OrganizationID", "0")
        End Get
        Set(ByVal Value As Long)
            AddCookie("OrganizationID", Value)
        End Set
    End Property

    <Description("Gets or sets the StaffID saved into the StaffID cookie.")> _
    Public Shared Property StaffID() As Long
        Get
            Return GetCookie("StaffID", "0")
        End Get
        Set(ByVal Value As Long)
            AddCookie("StaffID", Value)
        End Set
    End Property

    <Description("Gets or sets a user id saved into the UserID cookie.")> _
    Public Shared Property thisUserID() As Long
        Get
            Return GetCookie("thisUserID", 1)
        End Get
        Set(ByVal Value As Long)
            AddCookie("thisUserID", Value)
        End Set
    End Property

    <Description("Gets or sets a user id saved into the LogID cookie.")> _
    Public Shared Property thisLogID() As Long
        Get
            Return GetCookie("thisLogID", 1)
        End Get
        Set(ByVal Value As Long)
            AddCookie("thisLogID", Value)
        End Set
    End Property

    <Description("Gets or sets a user name saved into the UserFullName cookie.")> _
    Public Shared Property thisUserFullName() As String
        Get
            Return GetCookie("thisUserFullName", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("thisUserFullName", Value)
        End Set
    End Property

    <Description("Gets or sets a user name saved into the UserName cookie.")> _
    Public Shared Property thisUserName() As String
        Get
            Return GetCookie("thisUserName", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("thisUserName", Value)
        End Set
    End Property

    <Description("Gets or sets a User Groups saved into the UserGroups cookie.")> _
    Public Shared Property thisUserGroups() As String
        Get
            Return GetCookie("thisUserGroups", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("thisUserGroups", Value)
        End Set
    End Property

    <Description("Gets or sets a Search type saved into the Searchtype cookie.")> _
    Public Shared Property SearchType() As String
        Get
            Return GetCookie("SearchType", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("SearchType", Value)
        End Set
    End Property

#End Region

#Region "Beneficiary Cookies"

    <Description("Gets or sets the SegregatedFundID saved into the SegregatedFundID cookie.")> _
    Public Shared Property BeneficiaryID() As Long
        Get
            Return GetCookie("BeneficiaryID", 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("BeneficiaryID", Value)
        End Set
    End Property

    <Description("Gets or sets a Member id saved into the MemberID cookie.")> _
    Public Shared Property ParentID() As Long
        Get
            Dim tmp As String = GetCookie("ParentID", 0)
            Return IIf(IsNumeric(tmp), tmp, 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("ParentID", Value)
        End Set
    End Property

    <Description("Gets or sets a category id saved into the CategoryID cookie.")> _
    Public Shared Property CategoryID() As Long
        Get
            Return GetCookie("CategoryID", -1)
        End Get
        Set(ByVal Value As Long)
            AddCookie("CategoryID", Value)
        End Set
    End Property

    <Description("Gets or sets a Member No saved into the MemberNo cookie.")> _
    Public Shared Property MemberNo() As String
        Get
            Return GetCookie("MemberNo", -1)
        End Get
        Set(ByVal Value As String)
            AddCookie("MemberNo", Value)
        End Set
    End Property

    <Description("Gets or sets a member image File path saved into the FilePath cookie.")> _
    Public Shared Property FileFrom() As String
        Get
            Return GetCookie("FileFrom", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("FileFrom", Value)
        End Set
    End Property

    <Description("Gets or sets a MemberEmailAddress path saved into the MemberEmailAddress cookie.")> _
    Public Shared Property MemberEmailAddress() As String
        Get
            Return GetCookie("MemberEmailAddress", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("MemberEmailAddress", Value)
        End Set
    End Property

    <Description("Gets or sets the document URL e.g Birth Certificate, and saved into the DocumentURL cookie.")> _
    Public Shared Property DocumentURL() As String
        Get
            Return GetCookie("DocumentURL", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("DocumentURL", Value)
        End Set
    End Property

    <Description("Gets or sets a member type saved into the MemberType cookie.")> _
    Public Shared Property MemberType() As String
        Get
            Return GetCookie("MemberType", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("MemberType", Value)
        End Set
    End Property

    <Description("Gets or sets a member image File path saved into the FileTo cookie.")> _
    Public Shared Property FileTo() As String
        Get
            Return GetCookie("FileTo", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("FileTo", Value)
        End Set
    End Property

    <Description("Gets or sets a member image File URL saved into the FilePath cookie.")> _
    Public Shared Property FileUrl() As String
        Get
            Return GetCookie("FileUrl", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("FileUrl", Value)
        End Set
    End Property

    <Description("Gets or sets a member status saved into the MemberStatus cookie.")> _
    Public Shared Property MemberStatus() As String
        Get
            Return GetCookie("MemberStatus", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("MemberStatus", Value)
        End Set
    End Property

    <Description("Gets or sets the query for the Last Searched Member saved into the LastMemberSearch cookie.")> _
    Public Shared Property LastMemberSearch() As String
        Get
            Dim s As String = HttpContext.Current.Session("LastMemberSearch")
            If String.IsNullOrEmpty(s) Then
                Return ""
            Else
                Return s
            End If
        End Get
        Set(ByVal Value As String)
            HttpContext.Current.Session("LastMemberSearch") = Value
        End Set
    End Property

    <Description("Gets or sets a member search information saved into the MemberStatus cookie.")> _
    Public Shared Property MemberSearchResults() As String
        Get
            Return GetCookie("MemberSearchResults", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("MemberSearchResults", Value)
        End Set
    End Property

    <Description("Gets or sets the Company ID saved into the CompanyID cookie.")> _
    Public Shared Property CompanyID() As String
        Get
            Return GetCookie("CompanyID", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("CompanyID", Value)
        End Set
    End Property

    <Description("Gets or sets the Provider ID saved into the ProviderID cookie.")> _
    Public Shared Property ProviderID() As String
        Get
            Return GetCookie("ProviderID", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("ProviderID", Value)
        End Set
    End Property

    <Description("Gets or sets the query for the Effective change date saved into the ChangeEffectiveDate cookie.")> _
    Public Shared Property ChangeEffectiveDate() As DateTime
        Get
            Return GetCookie("ChangeEffectiveDate", Now.ToLongDateString)
        End Get
        Set(ByVal Value As DateTime)
            AddCookie("ChangeEffectiveDate", Value)
        End Set
    End Property

#End Region

#Region "Communication Cookies"



#End Region

#Region "Document Cookies"

    <Description("Gets or sets a DocumentID saved into the DocumentID cookie.")> _
    Public Shared Property DocumentID() As Long
        Get
            Dim tmp As String = GetCookie("DocumentID", 0)
            Return IIf(IsNumeric(tmp), tmp, 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("DocumentID", Value)
        End Set
    End Property

#End Region

#Region "ListManagement Cookies"

    <Description("Gets or sets a ListID saved into the ListID cookie.")> _
    Public Shared Property ListID() As Long
        Get
            Dim tmp As String = GetCookie("ListID", 0)
            Return IIf(IsNumeric(tmp), tmp, 0)
        End Get
        Set(ByVal Value As Long)
            AddCookie("ListID", Value)
        End Set
    End Property

#End Region

#Region "Custom Field Cookies"
    <Description("Gets or sets a owner id saved into the OwnerID cookie.")> _
    Public Shared Property OwnerID() As Long
        Get
            Return GetCookie("OwnerID", -1)
        End Get
        Set(ByVal Value As Long)
            AddCookie("OwnerID", Value)
        End Set
    End Property

    <Description("Gets or sets an owner type saved into the OwnerType cookie.")> _
    Public Shared Property OwnerType() As String
        Get
            Return GetCookie("OwnerType", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("OwnerType", Value)
        End Set
    End Property
#End Region

#Region "Reports Cookies"

    <Description("Gets or sets the Report Name saved into the Report Name cookie.")> _
    Public Shared Property ReportName() As String
        Get
            Return GetCookie("ReportName", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("ReportName", Value)
        End Set
    End Property

    <Description("Gets or sets the DiskFileName saved into the DiskFileName cookie.")> _
    Public Shared Property DiskFileName As String
        Get
            Return GetCookie("DiskFileName", "")
        End Get
        Set(ByVal Value As String)
            AddCookie("DiskFileName", Value)
        End Set
    End Property

#End Region

#Region "Private Methods"

    Shared Sub AddCookie(ByVal Key, ByVal Value)

        Dim objCookie As New HttpCookie(Key, Value)

        objCookie.Path = "/"

        Dim rqCookies As HttpCookieCollection = HttpContext.Current.Request.Cookies
        Dim rsCookies As HttpCookieCollection = HttpContext.Current.Response.Cookies

        While rqCookies.Get(Key) IsNot Nothing : rqCookies.Remove(Key) : End While
        rsCookies.Remove(Key)

        If rqCookies.Get(Key) IsNot Nothing Then
            rqCookies.Set(objCookie)
        Else
            rqCookies.Add(objCookie)
        End If

        If rsCookies.Get(Key) IsNot Nothing Then
            rsCookies.Set(objCookie)
        Else
            rsCookies.Add(objCookie)
        End If

    End Sub

    Shared Function GetCookie(ByVal Key, ByVal ValueIfNothing)
        If HttpContext.Current.Request.Cookies(Key) Is Nothing Then
            mGotNothing = True
            GetCookie = ValueIfNothing
        Else
            mGotNothing = False
            GetCookie = HttpContext.Current.Request.Cookies(Key).Value
        End If
    End Function

    Public Shared Sub ClearCookies()

        Try

            For Each objCookie As HttpCookie In HttpContext.Current.Request.Cookies

                If CanClearCookie(objCookie.Name) Then

                    HttpContext.Current.Request.Cookies.Remove(objCookie.Name)
                    HttpContext.Current.Response.Cookies.Remove(objCookie.Name)

                End If

            Next

        Catch : End Try

    End Sub

    Private Shared Function CanClearCookie(ByVal CookieName As String) As Boolean

        Return Not Array.IndexOf(NON_CLEARABLE_COOKIES.Split(","), CookieName) >= 0

    End Function

#End Region

End Class

