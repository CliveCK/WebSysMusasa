Imports Universal.CommonFunctions
Imports Microsoft.Practices.EnterpriseLibrary.Data

Partial Public Class DataPage
    Inherits System.Web.UI.Page

    Private db As Database
    Private DataLookup As BusinessLogic.CommonFunctions
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim db As Database = New DatabaseProviderFactory().Create("ConnectionString")

        'Select Case Request.QueryString("op")

        '    Case "GetPlainCCContacts"

        '        SearchPlainCCContacts(Request.QueryString("Query"), False)

        '    Case "GetPlainContacts"

        '        SearchPlainContacts(Request.QueryString("Query"), False)

        '    Case "GetContacts"

        '        SearchContacts(Request.QueryString("Query"), False, Request.QueryString("ActionFilter"))

        '    Case "GetProjects"

        '        SearchProjects(Request.QueryString("Query"))

        '    Case "GetFolders"

        '        SearchFolders(Request.QueryString("Query"))

        '    Case "GetLocations"

        '        SearchLocations(Request.QueryString("Query"))

        '    Case "GetLocations_FullDesc"

        '        SearchLocationsByText(Request.QueryString("Query"))

        'End Select

    End Sub

    'Public Sub SearchLocations(ByVal Query As String)

    '    Dim sql As String = ""
    '    Dim crit As String = Universal.CommonFunctions.GetConditions("LI.[Full_Desc]", Query)

    '    Dim secure As Boolean = False
    '    If Request.QueryString("secure") IsNot Nothing Then
    '        Boolean.TryParse(Request.QueryString("secure"), secure)
    '    End If

    '    Dim typeCrit As String = String.Empty
    '    If Request.QueryString("loctype") IsNot Nothing Then
    '        typeCrit = Universal.CommonFunctions.GetConditions("LT.[Name]", Request.QueryString("loctype"))
    '    End If

    '    Dim count As String = 30
    '    If Request.QueryString("count") IsNot Nothing AndAlso IsNumeric(Request.QueryString("count")) Then
    '        count = Request.QueryString("loctype")
    '    End If

    '    sql &= "SELECT TOP " & count & " LI.CategoryID AS [value], LI.Full_Desc AS [caption] " & vbCrLf
    '    sql &= "FROM " & IIf(secure, "fnGetAllowedLocations(" & CookiesWrapper.thisUserID & ")", "Categories") & " LI " & vbCrLf
    '    sql &= "    INNER JOIN luCategoryTypes LT ON LI.[CategoryTypeID] = LT.[CategoryTypeID] " & vbCrLf
    '    If crit <> "" Then sql &= "WHERE " & crit & vbCrLf
    '    If typeCrit <> "" Then sql &= IIf(String.IsNullOrEmpty(crit), "WHERE ", vbTab & "AND ") & typeCrit & vbCrLf
    '    sql &= "ORDER BY ContactID, Full_Desc"

    '    Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

    '    Response.Clear()

    '    Response.Write(DataLookup.GetJSONString(ds.Tables(0)))

    '    Response.End()

    'End Sub

    'Public Sub SearchLocationsByText(ByVal Query As String)

    '    Dim sql As String = ""
    '    Dim crit As String = Universal.CommonFunctions.GetConditions("LI.[Full_Desc]", Query)

    '    Dim typeCrit As String = String.Empty
    '    If Request.QueryString("loctype") IsNot Nothing Then
    '        typeCrit = Universal.CommonFunctions.GetConditions("LT.[Name]", Request.QueryString("loctype"))
    '    End If

    '    Dim secure As Boolean = False
    '    If Request.QueryString("secure") IsNot Nothing Then
    '        Boolean.TryParse(Request.QueryString("secure"), secure)
    '    End If

    '    sql &= "SELECT TOP 150 LI.Full_Desc AS [value], LI.Full_Desc AS [caption] " & vbCrLf
    '    sql &= "FROM " & IIf(secure, "fnGetAllowedLocations(" & CookiesWrapper.thisUserID & ")", "Categories") & " LI " & vbCrLf
    '    sql &= "    INNER JOIN luCategoryTypes LT ON LI.[CategoryTypeID] = LT.[CategoryTypeID] " & vbCrLf
    '    If crit <> "" Then sql &= "WHERE " & crit & vbCrLf
    '    If typeCrit <> "" Then sql &= IIf(String.IsNullOrEmpty(crit), "WHERE ", vbTab & "AND ") & typeCrit & vbCrLf
    '    sql &= "ORDER BY ContactID, Full_Desc"

    '    Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

    '    Response.Clear()

    '    Response.Write(DataLookup.GetJSONString(ds.Tables(0)))

    '    Response.End()

    'End Sub

    'Public Sub SearchFolders(ByVal Query As String)

    '    Dim sql As String = ""
    '    sql &= "SELECT TOP 30 PrgID AS [value], Full_Desc AS [caption] " & vbCrLf
    '    sql &= "FROM fnGetAllowedFolders(" & CookiesWrapper.thisUserID & ") " & vbCrLf
    '    sql &= "WHERE Full_Desc LIKE N'%" & Query.Replace("'", "''") & "%'" & vbCrLf
    '    sql &= "ORDER BY Full_Desc"

    '    Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

    '    Response.Clear()

    '    Response.Write(DataLookup.GetJSONString(ds.Tables(0)))

    '    Response.End()

    'End Sub

    'Public Sub SearchProjects(ByVal Query As String)

    '    Dim sql As String = ""
    '    sql &= "SELECT TOP 30 P.Project_ID AS [value], R1.Root + ' > ' + P.Project_Number AS [caption] " & vbCrLf
    '    sql &= "FROM fnGetAllowedProjects(" & CookiesWrapper.thisUserID & ") P " & vbCrLf
    '    sql &= "	INNER JOIN Contacts R ON P.Client_ID = R.Root_ID AND R.Parent = '-Root-' " & vbCrLf
    '    sql &= "	INNER JOIN Lookup_Root R1 ON R1.Root_ID = R.Root_ID AND R.Parent = '-Root-' " & vbCrLf
    '    sql &= "WHERE R1.[Root] LIKE N'%" & Query.Replace("'", "''") & "%' OR P.Project_Number LIKE N'%" & Query.Replace("'", "''") & "%' " & vbCrLf
    '    sql &= "ORDER BY R1.[Root], P.Project_Number"

    '    Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

    '    Response.Clear()

    '    Response.Write(DataLookup.GetJSONString(ds.Tables(0)))

    '    Response.End()

    'End Sub

    'Public Sub SearchContacts(ByVal Query As String, ByVal OnlyWithEmails As Boolean, ByVal ActionFilter As String)

    '    Dim contact As String, action As String = ""

    '    If Query.Contains(">") Then

    '        Dim s() As String = Query.Split(">")
    '        contact = s(0).Trim
    '        action = s(1).Trim

    '    Else

    '        contact = Query
    '        If Not String.IsNullOrWhiteSpace(ActionFilter) Then action = ActionFilter

    '    End If

    '    If action.Trim = "" Then action = "For Information"

    '    Dim sql As String = ""
    '    sql &= "select top 30 ContactType + '_' + cast(ContactID as varchar(10)) + '_' + isnull(actionrequest, 'For Information') as [value], case ContactType when 'E' then [Parent] + isnull(' - ' + Initials, '') + isnull(' (' + lower(Email) + ')', '') when 'I' then [Parent] end + ' - ' + isnull(actionrequest, 'For Information') AS [caption] from (" & vbCrLf
    '    sql &= "    select distinct lp.Initials, r.[Root] AS [Root], p.ContactID, r.Root_ID, case when p.[Parent]='-Root-' then [Root] else  ISNULL([Root] + ' -> ', '') + p.[Parent] end Parent, p.Email, p.IDNumber, 'E' as ContactType from Contacts p  " & vbCrLf
    '    sql &= "    	inner join lookup_root r on p.root_id=r.root_id " & vbCrLf
    '    sql &= "    	inner join Contacts lp on lp.root_id = r.root_id and lp.parent='-Root-' " & vbCrLf
    '    'sql &= "    where p.Parent <> '-Root-' " & vbCrLf
    '    sql &= " union " & vbCrLf
    '    sql &= "    select distinct null as Initials, ' Docwize ' + ISNULL('- ' + Department_Name,'Users') as [Root], " & vbCrLf
    '    sql &= "    	[User_ID] AS ContactID, NULL AS Root_ID, Username AS Parent, Email, null AS IDNumber, 'I' as ContactType " & vbCrLf
    '    sql &= "    from users " & vbCrLf
    '    sql &= "        left outer join lookup_departments on users.department_id = lookup_departments.department_id " & vbCrLf
    '    sql &= "    where Users.Active = 1" & vbCrLf
    '    sql &= ") as contacts "
    '    sql &= ", lookup_actionrequests where actionrequest like N'%" & action.Replace("'", "''") & "%'  --and [Parent] <> '-Root-' " & vbCrLf
    '    sql &= IIf(OnlyWithEmails, "and NULLIF(RTRIM(LTRIM(Email)),'') IS NOT NULL ", "")

    '    Dim crit As String = ""
    '    If Query <> "" Then

    '        crit = "(" & vbCrLf
    '        crit &= vbTab & " parent like N'%" & contact.Replace("'", "''") & "%' " & vbCrLf
    '        crit &= vbTab & " or [root] like N'%" & contact.Replace("'", "''") & "%' " & vbCrLf
    '        crit &= vbTab & " or [Email] like N'%" & contact.Replace("'", "''") & "%' " & vbCrLf
    '        crit &= vbTab & " or idnumber like N'%" & contact.Replace("'", "''") & "%' " & vbCrLf
    '        crit &= ")" & vbCrLf

    '    End If

    '    sql &= IIf(crit = "", "", "and " & crit) & "ORDER BY contacts.[Root], contacts.[Parent]"

    '    Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

    '    Response.Clear()

    '    Response.Write(DataLookup.GetJSONString(ds.Tables(0)))

    '    Response.End()

    'End Sub

    'Public Sub SearchPlainContacts(ByVal Query As String, ByVal OnlyWithEmails As Boolean)

    '    Dim contact As String = Query

    '    Dim sql As String = ""
    '    sql &= "select distinct top 30 p.ContactID as [value], " & vbCrLf
    '    sql &= "	case when p.[Parent] = '-Root-' then r.Root else ISNULL(r.[Root] + ' -> ', '') + p.Parent end AS [caption] " & vbCrLf
    '    sql &= "from Contacts p   " & vbCrLf
    '    sql &= "	inner join lookup_root r on p.root_id=r.root_id  " & vbCrLf
    '    sql &= "	inner join Contacts lp on lp.root_id = r.root_id and lp.parent='-Root-'  " & vbCrLf

    '    Dim crit As String = ""
    '    If Query <> "" Then crit = " (p.parent like N'%" & contact.Replace("'", "''") & "%' OR r.[Root] like N'%" & contact.Replace("'", "''") & "%' OR P.[Email] like N'%" & contact.Replace("'", "''") & "%' OR p.[IDNumber] like N'%" & contact.Replace("'", "''") & "%')"

    '    sql &= IIf(crit = "", "", "and " & crit) & "ORDER BY [caption]"

    '    Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

    '    Response.Clear()

    '    Response.Write(DataLookup.GetJSONString(ds.Tables(0)))

    '    Response.End()

    'End Sub

    Public Sub SearchPlainCCContacts(ByVal Query As String, ByVal OnlyWithEmails As Boolean)

        Dim contact As String = Query

        Dim sql As String = ""
        sql &= "select distinct top 30 'ContID_' + CAST(p.ContactID as varchar) as [value],  " & vbCrLf
        sql &= "    case when p.Parent='-Root-' then r.[Root] else ISNULL(r.[Root] + ' -> ', '') + p.Parent end AS [caption]  " & vbCrLf
        sql &= "from Contacts p   " & vbCrLf
        sql &= "	inner join lookup_root r on p.root_id=r.root_id  " & vbCrLf
        sql &= "	inner join Contacts lp on lp.root_id = r.root_id and lp.parent='-Root-'  " & vbCrLf

        Dim crit As String = ""
        If Query <> "" Then crit = " (p.parent like N'%" & contact.Replace("'", "''") & "%' OR r.[Root] like N'%" & contact.Replace("'", "''") & "%' OR r.[Root] like N'%" & contact.Replace("'", "''") & "%' OR p.[IDNumber] like N'%" & contact.Replace("'", "''") & "%')"

        sql &= IIf(crit = "", "", "and " & crit) & "ORDER BY [caption]"

        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        Response.Clear()

        'Response.Write(DataLookup.GetJSONString(ds.Tables(0)))

        Response.End()

    End Sub

End Class