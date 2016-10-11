Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Telerik.Web.UI
Imports Universal.CommonFunctions
Imports Microsoft.Practices.EnterpriseLibrary.Data

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
<System.Web.Script.Services.ScriptService()> _
Public Class DataService
    Inherits System.Web.Services.WebService

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private db As Database = New DatabaseProviderFactory().Create("ConnectionString")
    Dim SystemOptions As Object

    <WebMethod()> _
    Public Function GetFoldersForSearch(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim sql As String = ""
        sql &= "SELECT  PrgID value,Full_Desc caption FROM [dbo].[fnGetAllowedFolders] (" & CookiesWrapper.thisUserID & ")  F" & vbCrLf

        sql &= "WHERE [Full_Desc] LIKE '%" & Query.Trim.Replace("'", "''") & "%' " & vbCrLf
        sql &= "ORDER BY F.Full_Desc"

        Dim db As Database = New DatabaseProviderFactory().Create("ConnectionString")
        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("caption"), "")
            itemData.Value = Catchnull(dr("value"), "")

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function


    ''' <summary>
    ''' This for search page location Index 
    ''' </summary>
    ''' <param name="context"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetLocationIndexForSearch(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim sql As String = ""
        sql &= "select CategoryID value,  Full_Desc caption from Categories L" & vbCrLf

        sql &= "WHERE [Full_Desc] LIKE '%" & Query.Trim.Replace("'", "''") & "%' " & vbCrLf
        sql &= "ORDER BY L.Full_Desc"

        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("caption"), "")
            itemData.Value = Catchnull(dr("value"), "")

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetFolders(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim sql As String = ""
        sql &= "SELECT TOP 30 Full_Desc AS [value], Full_Desc AS [caption] " & vbCrLf
        sql &= "FROM Lookup_Folders F " & vbCrLf
        sql &= "WHERE [Full_Desc] LIKE '%" & Query.Replace("'", "''") & "%' " & vbCrLf
        sql &= "ORDER BY F.Full_Desc"

        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("caption"), "")
            itemData.Value = Catchnull(dr("value"), "")

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetLocationIndex(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim TypeFilter As String = String.Empty
        If contextDictionary.Keys.Contains("TypeFilter") Then TypeFilter = contextDictionary("TypeFilter").ToString()

        Dim sql As String = ""
        sql &= "SELECT TOP 30 Full_Desc AS [value], Full_Desc AS [caption] " & vbCrLf
        sql &= "FROM Categories L " & vbCrLf

        If TypeFilter <> String.Empty Then
            sql &= "     INNER JOIN dbo.luCategoryTypes LT ON L.CategoryTypeID = LT.CategoryTypeID " & vbCrLf
        End If

        sql &= "WHERE [Full_Desc] LIKE '%" & Query.Replace("'", "''") & "%' " & vbCrLf

        If TypeFilter <> String.Empty Then

            Dim types() As String = TypeFilter.Split("|")
            Dim critType As String = String.Empty

            For Each t As String In types
                If Not String.IsNullOrEmpty(t) Then
                    critType &= IIf(String.IsNullOrEmpty(critType), "", "    OR ") & "LT.Name = '" & t.Replace("'", "''") & "'" & vbCrLf
                End If
            Next

            If critType <> String.Empty Then sql &= " AND (" & vbCrLf & critType & vbCrLf & ") " & vbCrLf

        End If

        sql &= "ORDER BY L.Full_Desc"

        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("caption"), "")
            itemData.Value = Catchnull(dr("value"), "")

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod()>
    Public Function GetDesignation(ByVal context As Object) As RadComboBoxItemData()
        '        On Error GoTo errorHandle
        '        Dim returnDs As New DataSet
        '        Dim strSQL As String = ""
        '        If Designation <> "" Or Designation <> Nothing Then
        '            strSQL = "SELECT Designation FROM Lookup_ContactsDesignation WHERE Designation like '%" & Designation & "%'"
        '        Else
        '            strSQL = "SELECT Designation FROM Lookup_ContactsDesignation"
        '        End If
        '        Return Me.mObjBus.mDB.GetDataSet(strSQL, "designations")

        '        Return returnDs
        '        Exit Function
        'errorHandle:
        '        Me.mError = Err.Description & " cDWClients.ContactDesignationSearch : " & Err.Number
        '        Return Nothing


        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()

        Dim Query As String = String.Empty, RootID As String = String.Empty, ProjectType As String = String.Empty, Root As String = String.Empty, BlankText As String = String.Empty
        Dim sql As String = ""
        sql &= " SELECT Designation_ID as[value], Designation as [caption] FROM Lookup_ContactsDesignation"


        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)
        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("caption"), "")
            itemData.Value = Catchnull(dr("value"), "")

            result.Add(itemData)

        Next

        Return result.ToArray()
    End Function

    <WebMethod()> _
    Public Function GetProjects(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()

        Try

            Dim Query As String = String.Empty, RowCountLimit As String = "15", ParentID As String = String.Empty, RootID As String = String.Empty, ProjectType As String = String.Empty, Root As String = String.Empty, BlankText As String = String.Empty, IncludeClientName As Boolean = True, IncludeProjectStatus As Boolean = False
            Dim StatusFilter As String = String.Empty, TypeFilter As String = String.Empty

            If contextDictionary.ContainsKey("RowCountLimit") Then RowCountLimit = contextDictionary("RowCountLimit").ToString()
            If contextDictionary.ContainsKey("FilterString") Then Query = contextDictionary("FilterString").ToString()
            If contextDictionary.ContainsKey("ParentID") Then ParentID = contextDictionary("ParentID").ToString
            If contextDictionary.ContainsKey("RootID") Then RootID = contextDictionary("RootID").ToString
            If contextDictionary.ContainsKey("Root") Then Root = contextDictionary("Root").ToString
            If contextDictionary.ContainsKey("BlankText") Then BlankText = contextDictionary("BlankText").ToString
            If contextDictionary.ContainsKey("ProjectType") Then ProjectType = contextDictionary("ProjectType").ToString
            If contextDictionary.ContainsKey("TypeFilter") Then TypeFilter = contextDictionary("ProjectType").ToString
            If contextDictionary.ContainsKey("StatusFilter") Then StatusFilter = contextDictionary("ProjectType").ToString
            If contextDictionary.ContainsKey("IncludeClientName") Then IncludeClientName = (contextDictionary("IncludeClientName").ToString.ToLower.Equals("true"))
            If contextDictionary.ContainsKey("IncludeProjectStatus") Then IncludeProjectStatus = contextDictionary("IncludeProjectStatus").ToString

            Dim sql As String = ""
            sql &= "SELECT " & IIf(IsNumeric(RowCountLimit) AndAlso CInt(RowCountLimit) > 0, "TOP " & RowCountLimit, String.Empty) & " P.Project_ID AS [value], " &
                IIf(IncludeClientName, "R1.Root + ' > ' +  ", String.Empty) &
                "P.Project_Number " &
                IIf(IncludeProjectStatus, " + ISNULL(' [' + NULLIF(LTRIM(RTRIM(P.Project_Status)), '') + ']', '')", String.Empty) &
                " AS [caption] " & vbCrLf
            sql &= "FROM Projects P " & vbCrLf
            sql &= "	INNER JOIN Contacts R ON P.Client_ID = R.Root_ID AND R.Parent = '-Root-' " & vbCrLf
            sql &= "	INNER JOIN Lookup_Root R1 ON R1.Root_ID = R.Root_ID AND R.Parent = '-Root-' " & vbCrLf

            Dim crit As String = String.Empty

            If Not String.IsNullOrEmpty(Query) Then
                crit = "R1.[Root] LIKE N'%" & Query.Replace("'", "''") & "%' OR P.Project_Number LIKE N'%" & Query.Replace("'", "''") & "%' " & vbCrLf
            ElseIf Not String.IsNullOrEmpty(RootID) AndAlso IsNumeric(RootID) Then
                crit = "R1.[Root_ID] = " & RootID & vbCrLf
            ElseIf Not String.IsNullOrEmpty(ParentID) AndAlso IsNumeric(ParentID) Then
                crit = "R.[ContactID] = " & ParentID & vbCrLf
            ElseIf Not String.IsNullOrEmpty(Root) Then
                crit = "R1.[Root] = N'" & Root.Replace("'", "''") & "' " & vbCrLf
            End If

            If Not String.IsNullOrEmpty(ProjectType) Then
                crit &= IIf(String.IsNullOrEmpty(crit), "", " AND ") & "P.[Project_Type] = '" & ProjectType.Replace("'", "''") & "'" & vbCrLf
            End If

            If TypeFilter <> String.Empty Then

                Dim types() As String = TypeFilter.Split("|")
                Dim critType As String = String.Empty

                For Each t As String In types
                    If Not String.IsNullOrEmpty(t) Then
                        critType &= IIf(String.IsNullOrEmpty(critType), "", "    OR ") & "P.Project_Type = '" & t.Replace("'", "''") & "'" & vbCrLf
                    End If
                Next

                If critType <> String.Empty Then sql &= " AND (" & vbCrLf & critType & vbCrLf & ") " & vbCrLf

            End If

            If StatusFilter <> String.Empty Then

                Dim statuses() As String = StatusFilter.Split("|")
                Dim critType As String = String.Empty

                For Each t As String In statuses
                    If Not String.IsNullOrEmpty(t) Then
                        critType &= IIf(String.IsNullOrEmpty(critType), "", "    OR ") & "P.Project_Status = '" & t.Replace("'", "''") & "'" & vbCrLf
                    End If
                Next

                If critType <> String.Empty Then sql &= " AND (" & vbCrLf & critType & vbCrLf & ") " & vbCrLf

            End If

            sql &= IIf(String.IsNullOrEmpty(crit), "", "WHERE " & crit & vbCrLf)

            If Not String.IsNullOrEmpty(BlankText) Then
                sql &= "UNION SELECT 0, '" & BlankText.Replace("'", "''") & "'"
            End If
            sql &= "ORDER BY [caption]"


            Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

            For Each dr As DataRow In ds.Tables(0).Rows

                Dim itemData As New RadComboBoxItemData()

                itemData.Text = Catchnull(dr("caption"), "")
                itemData.Value = Catchnull(dr("value"), "")

                result.Add(itemData)

            Next

        Catch ex As Exception
            Log.Error(ex)
        End Try

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetMandatoryDocTypeFields(ByVal DocType As String) As String()

        Dim s As New List(Of String)
        Dim sql As String = "SELECT [FieldValue] FROM [CustomField_DataLookup] WHERE [FieldValue] IS NOT NULL AND [FieldName] = '#MANDATORYDOCDETAILS-" & DocType.Replace("'", "''") & "' ORDER BY [FieldValue]"


        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            s.Add(dr(0))

        Next

        Return s.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetSecureProjects(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim sql As String = ""
        sql &= "SELECT TOP 15 P.Project_ID AS [value], R1.Root + ' > ' + P.Project_Number AS [caption], R1.Root_ID, R.ContactID " & vbCrLf
        sql &= "FROM dbo.fnGetAllowedProjects(" & CookiesWrapper.thisUserID & ") P " & vbCrLf
        sql &= "	INNER JOIN Contacts R ON P.Client_ID = R.Root_ID AND R.Parent = '-Root-' " & vbCrLf
        sql &= "	INNER JOIN Lookup_Root R1 ON R1.Root_ID = R.Root_ID AND R.Parent = '-Root-' " & vbCrLf
        sql &= "WHERE R1.[Root] LIKE N'%" & Query.Replace("'", "''") & "%' OR P.Project_Number LIKE N'%" & Query.Replace("'", "''") & "%' " & vbCrLf
        sql &= "ORDER BY R1.[Root], P.Project_Number"


        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("caption"), "")
            itemData.Value = Catchnull(dr("value"), "")
            itemData.Attributes("dwRootID") = Catchnull(dr("Root_ID"), 0)
            itemData.Attributes("dwParentID") = Catchnull(dr("ContactID"), 0)

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetContactsEntity(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim UseRootID As String = String.Empty
        If contextDictionary.Keys.Contains("TypeFilter") Then UseRootID = contextDictionary("UseRootID")

        Dim TypeFilter As String = String.Empty
        If contextDictionary.Keys.Contains("TypeFilter") Then TypeFilter = contextDictionary("TypeFilter").ToString()

        Dim sql As String = ""
        sql &= "SELECT TOP 50 " & IIf(UseRootID = "TRUE", "P.Root_ID", "P.ContactID") & ", ISNULL([Root],'') + ISNULL(' > ' + CASE P.Parent WHEN '-Root-' THEN NULL ELSE P.Parent END, '')" & IIf(SystemOptions.Contacts.DisplayIDNumberInContactLookups, " + ISNULL(' - (' + P.IDNumber + ISNULL(', ' + NULLIF(P.Email,''), '') + ')', '')", String.Empty) & " AS Contact  " & vbCrLf
        sql &= "FROM Contacts P  " & vbCrLf
        sql &= "	INNER JOIN Lookup_Root R ON P.Root_ID = R.Root_ID " & vbCrLf

        Dim crit As String = ""
        crit = Universal.CommonFunctions.GetConditions("[Root]", Query)
        If crit <> "" Then crit &= " OR "
        crit &= Universal.CommonFunctions.GetConditions("[Parent]", Query)
        If crit <> "" Then crit &= " OR "
        crit &= Universal.CommonFunctions.GetConditions("[Initials]", Query)
        If crit <> "" Then crit &= " OR "
        crit &= Universal.CommonFunctions.GetConditions("[IDNumber]", Query)

        sql &= "WHERE P.Parent = '-Root-' " & IIf(crit = "", "", " AND (" & crit & ")") & vbCrLf

        If TypeFilter <> String.Empty Then

            Dim types() As String = TypeFilter.Split("|")
            Dim critType As String = String.Empty

            For Each t As String In types
                If Not String.IsNullOrEmpty(t) Then
                    critType &= IIf(String.IsNullOrEmpty(critType), "", "    OR ") & "P.Description = '" & t.Replace("'", "''") & "'" & vbCrLf
                End If
            Next

            If critType <> String.Empty Then sql &= " AND (" & vbCrLf & critType & vbCrLf & ") " & vbCrLf

        End If

        sql &= "ORDER BY [Root], [Parent]"


        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("Contact"), "")
            If UseRootID = "TRUE" Then
                itemData.Value = Catchnull(dr("Root_ID"), "")
            Else
                itemData.Value = Catchnull(dr("ContactID"), "")
            End If

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetCustomFieldDataItems(ByVal context As Object) As RadComboBoxItemData()

        Dim result As New List(Of RadComboBoxItemData)()

        Try

            Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
            Dim FieldName As String = contextDictionary("FieldName").ToString()
            Dim FieldType As String = contextDictionary("FieldType").ToString()
            Dim ObjectType As String = contextDictionary("ObjectType").ToString()


            Dim objCF As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

            Dim tblValues As DataTable = objCF.Get_Custom_Field_Values(ObjectType, FieldName, FieldType)

            Dim TextField As String = "FieldValue", ValueField As String = "FieldValue"

            If FieldType.ToLower = "checkbox" Then

                result.Add(New RadComboBoxItemData() With {.Text = "YES", .Value = "YES"})
                result.Add(New RadComboBoxItemData() With {.Text = "NO", .Value = "NO"})
                result.Add(New RadComboBoxItemData() With {.Text = "N/A", .Value = "N/A"})

                Return result.ToArray

            ElseIf FieldType.ToLower = "currency" Then

                TextField = "FieldValue"
                ValueField = "CurrencyID"

            ElseIf FieldType.ToLower = "userlookup" Then

                TextField = "UserName"
                ValueField = "FieldValue"

            End If

            For Each dr As DataRow In tblValues.Rows

                Dim itemData As New RadComboBoxItemData()

                itemData.Text = Catchnull(dr(TextField), "")
                itemData.Value = Catchnull(dr(ValueField), "")

                result.Add(itemData)

            Next

            If (FieldType.ToLower = "combo" OrElse FieldType.ToLower = "checkbox") Then

                Dim itemData As New RadComboBoxItemData()

                itemData.Text = ""
                itemData.Value = ""

                result.Add(itemData)

            End If

        Catch ex As Exception
            Log.Error(ex)
        End Try

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetProjectEntities(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()

        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim UseRootIDAsValue As Boolean = False 'By default, this method uses the ContactID as the Value
        Boolean.TryParse(contextDictionary("UseRootIDAsValue").ToString(), UseRootIDAsValue)

        Dim TypeFilter As String = String.Empty
        If contextDictionary.Keys.Contains("TypeFilter") Then TypeFilter = contextDictionary("TypeFilter").ToString()

        Dim sql As String = ""
        sql &= "SELECT TOP 50 P.ContactID, ISNULL([Root],'') + ISNULL(' > ' + CASE P.Parent WHEN '-Root-' THEN NULL ELSE P.Parent END, '') AS Contact, R.Root_ID  " & vbCrLf
        sql &= "FROM Contacts P  " & vbCrLf
        sql &= "	INNER JOIN Lookup_Root R ON P.Root_ID = R.Root_ID " & vbCrLf

        Dim crit As String = ""
        crit = Universal.CommonFunctions.GetConditions("[Root]", Query)
        If crit <> "" Then crit &= " OR "
        crit &= Universal.CommonFunctions.GetConditions("[Parent]", Query)
        If crit <> "" Then crit &= " OR "
        crit &= Universal.CommonFunctions.GetConditions("[IDNumber]", Query)

        sql &= "WHERE R.Client_Flag = 1 AND P.Parent = '-Root-' " & IIf(crit = "", "", " AND (" & crit & ")") & vbCrLf

        If TypeFilter <> String.Empty Then

            Dim types() As String = TypeFilter.Split("|")
            Dim critType As String = String.Empty

            For Each t As String In types
                If Not String.IsNullOrEmpty(t) Then
                    critType &= IIf(String.IsNullOrEmpty(critType), "", "    OR ") & "P.Description = '" & t.Replace("'", "''") & "'" & vbCrLf
                End If
            Next

            If critType <> String.Empty Then sql &= " AND (" & vbCrLf & critType & vbCrLf & ") " & vbCrLf

        End If

        sql &= "ORDER BY [Root], [Parent]"


        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("Contact"), "")
            If UseRootIDAsValue Then
                itemData.Value = Catchnull(dr("Root_ID"), "")
                itemData.Attributes("ContactID") = Catchnull(dr("ContactID"), "")
            Else
                itemData.Value = Catchnull(dr("ContactID"), "")
                itemData.Attributes("Root_ID") = Catchnull(dr("Root_ID"), "")
            End If


            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetContacts(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim TypeFilter As String = String.Empty
        If contextDictionary.Keys.Contains("TypeFilter") Then TypeFilter = contextDictionary("TypeFilter").ToString()

        Dim sql As String = ""
        sql &= "SELECT TOP 50 P.ContactID, ISNULL([Root],'') + ISNULL(' > ' + CASE P.Parent WHEN '-Root-' THEN NULL ELSE P.Parent END, '')" & IIf(SystemOptions.Contacts.DisplayIDNumberInContactLookups, " + ISNULL(' - (' + P.IDNumber + ISNULL(', ' + NULLIF(P.Email,''), '') + ')', '')", String.Empty) & " AS Contact  " & vbCrLf
        sql &= "FROM Contacts P  " & vbCrLf
        sql &= "	INNER JOIN Lookup_Root R ON P.Root_ID = R.Root_ID " & vbCrLf

        Dim crit As String = ""
        crit = Universal.CommonFunctions.GetConditions("[Root]", Query)
        If crit <> "" Then crit &= " OR "
        crit &= Universal.CommonFunctions.GetConditions("[Parent]", Query)
        If crit <> "" Then crit &= " OR "
        crit &= Universal.CommonFunctions.GetConditions("[IDNumber]", Query)
        crit = "(" & crit & ")"

        If TypeFilter <> String.Empty Then

            Dim types() As String = TypeFilter.Split("|")
            Dim critType As String = String.Empty

            For Each t As String In types
                If Not String.IsNullOrEmpty(t) Then
                    critType &= IIf(String.IsNullOrEmpty(critType), "", "    OR ") & "P.Description = '" & t.Replace("'", "''") & "'" & vbCrLf
                End If
            Next

            If critType <> String.Empty Then crit &= " AND (" & vbCrLf & critType & vbCrLf & ") " & vbCrLf

        End If

        If crit <> "" Then sql &= "WHERE " & crit

        sql &= "ORDER BY [Root], [Parent]"


        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("Contact"), "")
            itemData.Value = Catchnull(dr("ContactID"), "")

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetDocTypesForContactProjectAssign(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim ProjectType As String = contextDictionary("ProjectType").ToString()

        Dim sql As String = "SELECT FieldValue FROM CustomField_DataLookup WHERE FieldName = '#PROJ_TYP.CONTACTS.DOC_TYP." & ProjectType.Replace("'", "''") & "' ORDER BY [FieldValue]"


        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("FieldValue"), "")
            itemData.Value = Catchnull(dr("FieldValue"), "")

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetDocTypes(ByVal context As Object) As RadComboBoxItemData()

        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim result As New List(Of RadComboBoxItemData)()
        Dim Query As String = contextDictionary("FilterString").ToString()

        Dim sql As String = ""
        sql &= "SELECT * FROM ( " & vbCrLf
        sql &= "SELECT    LookUp_DocumentTypes.ParentID, LookUp_DocumentTypes.Doc_Type, Lookup_DocumentTypeSub.SubDocType,   " & vbCrLf
        sql &= "                      ISNULL(LookUp_DocumentTypes.Doc_Type + ' > ' + Lookup_DocumentTypeSub.SubDocType, LookUp_DocumentTypes.Doc_Type)   " & vbCrLf
        sql &= "                      AS DocumentType, CASE ISNULL(CustomFields.ID,0) "
        sql &= " WHEN 0 THEN 0 ELSE 1 END AS HasCustomFields  " & vbCrLf
        sql &= "FROM         LookUp_DocumentTypes LEFT OUTER JOIN  " & vbCrLf
        sql &= "                      Lookup_DocumentTypeSub ON LookUp_DocumentTypes.ParentID = Lookup_DocumentTypeSub.ParentID  " & vbCrLf
        sql &= "			LEFT OUTER JOIN (SELECT DISTINCT DocumentTypeID ID FROM Lookup_DocumentTypeFieldTemplates) AS CustomFields ON CustomFields.ID = LookUp_DocumentTypes.ParentID " & vbCrLf
        sql &= "WHERE ISNULL(LookUp_DocumentTypes.Doc_Type,'')<>'' " & vbCrLf
        sql &= "UNION  " & vbCrLf
        sql &= "SELECT DISTINCT LookUp_DocumentTypes.ParentID, LookUp_DocumentTypes.Doc_Type, NULL AS SubDocType, LookUp_DocumentTypes.Doc_Type AS DocumentType, "
        sql &= "CASE ISNULL(CustomFields.ID,0) "
        sql &= "WHEN 0 THEN 0 ELSE 1 END AS HasCustomFields  " & vbCrLf
        sql &= "FROM         LookUp_DocumentTypes INNER JOIN  " & vbCrLf
        sql &= "                      Lookup_DocumentTypeSub ON LookUp_DocumentTypes.ParentID = Lookup_DocumentTypeSub.ParentID  " & vbCrLf
        sql &= "			LEFT OUTER JOIN (SELECT DISTINCT DocumentTypeID ID FROM Lookup_DocumentTypeFieldTemplates) AS CustomFields ON CustomFields.ID = LookUp_DocumentTypes.ParentID " & vbCrLf
        sql &= "WHERE ISNULL(LookUp_DocumentTypes.Doc_Type,'')<>'' " & vbCrLf
        'sql &= "	SELECT    LookUp_DocumentTypes.ParentID + CASE ISNULL(CustomFields.ID,0) WHEN 0 THEN 0.0 ELSE 0.1 END AS ParentID, " & vbCrLf
        'sql &= "						  ISNULL(LookUp_DocumentTypes.Doc_Type + ' > ' + Lookup_DocumentTypeSub.SubDocType, LookUp_DocumentTypes.Doc_Type)    " & vbCrLf
        'sql &= "						  AS DocumentType " & vbCrLf
        'sql &= "	FROM         LookUp_DocumentTypes LEFT OUTER JOIN   " & vbCrLf
        'sql &= "						  Lookup_DocumentTypeSub ON LookUp_DocumentTypes.ParentID = Lookup_DocumentTypeSub.ParentID   " & vbCrLf
        'sql &= "				LEFT OUTER JOIN (SELECT DISTINCT DocumentTypeID ID FROM Lookup_DocumentTypeFieldTemplates) AS CustomFields ON CustomFields.ID = LookUp_DocumentTypes.ParentID  " & vbCrLf
        'sql &= "	WHERE ISNULL(LookUp_DocumentTypes.Doc_Type,'')<>''  " & vbCrLf
        'sql &= "	UNION   " & vbCrLf
        'sql &= "	SELECT DISTINCT LookUp_DocumentTypes.ParentID + CASE ISNULL(CustomFields.ID,0) WHEN 0 THEN 0.0 ELSE 0.1 END, LookUp_DocumentTypes.Doc_Type AS DocumentType " & vbCrLf
        'sql &= "	FROM         LookUp_DocumentTypes INNER JOIN   " & vbCrLf
        'sql &= "						  Lookup_DocumentTypeSub ON LookUp_DocumentTypes.ParentID = Lookup_DocumentTypeSub.ParentID   " & vbCrLf
        'sql &= "				LEFT OUTER JOIN (SELECT DISTINCT DocumentTypeID ID FROM Lookup_DocumentTypeFieldTemplates) AS CustomFields ON CustomFields.ID = LookUp_DocumentTypes.ParentID  " & vbCrLf
        'sql &= "	WHERE ISNULL(LookUp_DocumentTypes.Doc_Type,'')<>''  " & vbCrLf
        sql &= ") AS DocTypes " & vbCrLf

        Dim crit As String = ""
        If Query <> "" Then crit = Universal.CommonFunctions.GetConditions("[DocTypes.DocumentType]", Query)
        If crit <> "" Then sql &= "WHERE " & crit

        sql &= "ORDER BY [DocumentType]"


        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        For Each dr As DataRow In ds.Tables(0).Rows

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("DocumentType"), "")
            itemData.Value = Catchnull(dr("ParentID") & "." & dr("HasCustomFields") & "," & dr("DocumentType"), "")

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

    <WebMethod(), Script.Services.ScriptMethod()> _
    Public Function LookupProjectStatuses(ByVal context As Object) As RadComboBoxItemData()

        Dim result As New List(Of RadComboBoxItemData)()
        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))

        Dim Query As String = contextDictionary("Text").ToString()


        Dim sql As String = "SELECT Project_Status_ID, Status_Name, Project_Type_ID FROM luProjectStatuses"
        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql, "projectTypes")

        If contextDictionary.Keys.Contains("ProjectType") Then

            Dim ProjectType As String = contextDictionary("ProjectType").ToString()
            ds.Tables(0).DefaultView.RowFilter = String.Format("ISNULL(Project_Type_ID,0)=0 OR Project_Type_ID = {0}", ProjectType)

        End If

        ds.Tables(0).DefaultView.Sort = "Status_Name"

        For Each dr As DataRowView In ds.Tables(0).DefaultView

            Dim itemData As New RadComboBoxItemData()

            itemData.Text = Catchnull(dr("Status_Name"), "")
            itemData.Value = Catchnull(dr("Project_Status_ID"), 0)

            result.Add(itemData)

        Next

        Return result.ToArray()

    End Function

End Class