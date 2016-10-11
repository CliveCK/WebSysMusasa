Imports Universal.CommonFunctions
Imports System.Web.UI.WebControls
Imports System.Collections.Specialized
Imports System.Collections.Generic
Imports Telerik.Web.UI
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web.UI.HtmlControls
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Public Class CustomFieldsManager

    Public Class CustomFieldFilter

        Public FieldName As String
        Public TemplateID As String

        Public FilterValue As String
        Public FullTextOption As String

        Public Overrides Function ToString() As String

            Return GetConditions("CustomField_Grid_" & TemplateID, FilterValue, FullTextOption:=FullTextOption)

        End Function

        Public Sub New(ByVal FullTextOption As String, ByVal TemplateID As Long, FieldName As String, FilterValue As String)
            Me.FullTextOption = FullTextOption
        End Sub

    End Class

    Public Shared MAX_FIELD_LENGTH_NOTES As Integer = 4000

    Public ErrorMessage As String
    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database
    Private ConnectionName As String

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Event CustomAction(ByVal ID As String, ByVal FieldName As String, ByVal Combo As Telerik.Web.UI.RadComboBox)

    Public Property ObjectUserID As Long

    Sub New(ByVal ConnectionName As String, UserID As Long)

        Me.ConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)
        Me.ObjectUserID = UserID

    End Sub

    Property EnableClientValidationScripts As Boolean

    Public Function Get_All_Custom_Fields(ByVal ObjectType As Char, Optional ByVal OnlyWithValues As Boolean = False, Optional ByVal SearchFieldsOnly As Boolean = False, Optional ByVal ApplySecurity As Boolean = True, Optional ByVal ApplyActivationRules As Boolean = False, Optional ByVal ActivationType As String = "", Optional ByVal Activator As String = "", Optional ByVal ShowInResultsFieldsOnly As Boolean = False) As DataTable

        Dim sql As String = ""

        Dim SecurityFields As String = String.Empty
        If ApplySecurity Then
            If ApplyActivationRules Then
                SecurityFields = ", ISNULL( dbo.fnCustomField_ActivateTemplate( SecMatrix.TemplateID, '" & ActivationType & "', " & Activator & " ), SecMatrix.EditAllowed) {0} " & vbCrLf
            Else
                SecurityFields = ", SecMatrix.EditAllowed" & vbCrLf
            End If
        Else
            SecurityFields = "" & vbCrLf
        End If

        sql &= "SELECT PF.FieldName, PF.FieldType, SecMatrix.TemplateName, ISNULL(PT.DisplayIndex, MIN(PF.DisplayIndex)) AS DisplayIndex, PT.Required, PT.ShowInResults, PT.LoadOnDemand, PT.HelpNotes, MIN(PF.ObjectDataID) AS ObjectDataID" & String.Format(SecurityFields, "EditAllowed") & "  " & vbCrLf
        sql &= "FROM CustomField_ObjectData PF  " & vbCrLf
        sql &= "	INNER JOIN CustomField_Fields PT ON PF.FieldName = PT.FieldName  " & vbCrLf
        If ApplySecurity Then sql &= "    INNER JOIN fnCustomField_GetAllowedTemplates(" & ObjectUserID & ") SecMatrix ON SecMatrix.TemplateID = PT.TemplateID " & vbCrLf
        sql &= "WHERE ObjectType='" & ObjectType & "' " & vbCrLf
        sql &= IIf(OnlyWithValues, "    AND (PF.FieldValue1 IS NOT NULL AND LTRIM(RTRIM(PF.FieldValue1))<>'')", "") & vbCrLf
        sql &= IIf(SearchFieldsOnly, "    AND PT.Search = 1", "") & vbCrLf
        sql &= IIf(ShowInResultsFieldsOnly, "    AND PT.ShowInResults = 1", "") & vbCrLf
        sql &= "GROUP BY PF.FieldName, PF.FieldType, SecMatrix.TemplateName, PT.DisplayIndex, PT.Required, PT.ShowInResults, PT.LoadOnDemand, PT.HelpNotes" & String.Format(SecurityFields, "") & " " & vbCrLf
        sql &= "ORDER BY SecMatrix.TemplateName, DisplayIndex " & vbCrLf

        Return DataHelper.ExecuteDatatable(db, sql, "AllCustomFields")

    End Function

    Public Function Get_All_Used_Custom_Fields(
        ByVal ObjectType As Char,
        Optional ByVal SearchFieldsOnly As Boolean = False,
        Optional ByVal ApplySecurity As Boolean = True,
        Optional ByVal ShowInResultsFieldsOnly As Boolean = False
    ) As DataTable

        Dim SecurityFields As String = String.Empty
        If ApplySecurity Then
            SecurityFields = ", SecMatrix.EditAllowed"
        Else
            SecurityFields = ""
        End If

        Dim sql As String = ""
        sql &= "SELECT PT.FieldName, PT.FieldType, SecMatrix.TemplateName, PT.DisplayIndex, PT.Required, PT.ShowInResults, PT.CustomFieldID" & String.Format(SecurityFields, "EditAllowed") & "  " & vbCrLf
        sql &= "FROM CustomField_Fields PT " & vbCrLf
        If ApplySecurity Then sql &= "    INNER JOIN fnCustomField_GetAllowedTemplates(" & ObjectUserID & ") SecMatrix ON SecMatrix.TemplateID = PT.TemplateID " & vbCrLf
        sql &= "WHERE PT.TemplateID IN (SELECT U.TemplateID FROM CustomField_ObjectTemplates U WHERE ObjectType='" & ObjectType & "') " & vbCrLf
        sql &= IIf(SearchFieldsOnly, "    AND PT.Search = 1" & vbCrLf, "")
        sql &= IIf(ShowInResultsFieldsOnly, "    AND PT.ShowInResults = 1" & vbCrLf, "")
        sql &= "ORDER BY PT.TemplateID, DisplayIndex " & vbCrLf

        Return db.ExecuteDataSet(CommandType.Text, sql).Tables("AllProjectFields")

    End Function

    Private Function Get_Custom_Fields(ByVal ID As Long, ByVal ObjectType As String, Optional ByVal ApplySecurity As Boolean = True, Optional ByVal ApplyActivationRules As Boolean = False, Optional ByVal ActivationType As String = "", Optional ByVal Activator As String = "") As DataTable

        Dim sql As String = ""

        Dim SecurityFields As String = String.Empty
        If ApplySecurity Then
            If ApplyActivationRules Then
                SecurityFields = ", ISNULL( dbo.fnCustomField_ActivateTemplate( SecMatrix.TemplateID, '" & ActivationType & "', " & Activator & " ), SecMatrix.EditAllowed) {0} " & vbCrLf
            Else
                SecurityFields = ", SecMatrix.EditAllowed" & vbCrLf
            End If
        Else
            SecurityFields = "" & vbCrLf
        End If

        'sql = "SELECT FieldName,ISNULL(FieldValue1,'') AS FieldValue1,FieldType,DisplayIndex FROM CustomField_ObjectData WHERE ObjectType='P' AND (ObjectID = " & projectID & ") ORDER BY DisplayIndex"
        sql &= "SELECT PF.FieldName,  ISNULL(FieldValue1,'') AS FieldValue1, ISNULL(PT.FieldType, PF.FieldType) FieldType, CFT.TemplateName, CFT.TemplateType, PT.DisplayIndex, PT.Required, ISNULL(PT.NewLine,0) AS NewLine, ISNULL(PT.LoadOnDemand,0) AS LoadOnDemand, PT.HelpNotes, PF.CustomFieldID AS CustomFieldID, MIN(PF.ObjectDataID) AS ObjectDataID " & String.Format(SecurityFields, "EditAllowed") & vbCrLf
        sql &= "FROM CustomField_Templates CFT " & vbCrLf
        sql &= "	INNER JOIN CustomField_Fields PT ON CFT.TemplateID = PT.TemplateID " & vbCrLf
        sql &= "	LEFT OUTER JOIN CustomField_ObjectData PF ON PF.CustomFieldID = PT.CustomFieldID " & vbCrLf
        If ApplySecurity Then sql &= "    LEFT OUTER JOIN fnCustomField_GetAllowedTemplates(" & ObjectUserID & ") SecMatrix ON SecMatrix.TemplateID = PT.TemplateID " & vbCrLf
        sql &= "WHERE ObjectType='" & ObjectType & "' AND (ObjectID = " & ID & ") " & vbCrLf
        sql &= "GROUP BY PF.FieldName, ISNULL(FieldValue1,''), ISNULL(PT.FieldType, PF.FieldType), CFT.TemplateName, CFT.TemplateType, PT.DisplayIndex, PT.Required, ISNULL(PT.NewLine,0), ISNULL(PT.LoadOnDemand,0), PT.HelpNotes, PF.CustomFieldID" & String.Format(SecurityFields, "") & " " & vbCrLf
        sql &= "ORDER BY CFT.TemplateName, PT.DisplayIndex " & vbCrLf

        Return DataHelper.ExecuteDatatable(db, sql, "ProjectFields" & ID)

    End Function

    Public Function GetStatusTemplates(ByVal ID As Long, ByVal ObjectType As String, ByVal ObjectStatusID As Long, Optional ByVal TemplateObjectType As String = "", Optional ByVal ApplySecurity As Boolean = True, Optional ByVal ApplyActivationRules As Boolean = False, Optional ByVal ActivationType As String = "", Optional ByVal Activator As String = "") As DataSet

        If TemplateObjectType = "" Then TemplateObjectType = ObjectType

        Dim sql As New System.Text.StringBuilder
        Dim SecurityFields As String = String.Empty
        If ApplySecurity Then
            If ApplyActivationRules Then
                SecurityFields = ", ISNULL( dbo.fnCustomField_ActivateTemplate( SecMatrix.TemplateID, '" & ActivationType & "', " & Activator & " ), SecMatrix.EditAllowed) {0} "
            Else
                SecurityFields = ", SecMatrix.EditAllowed"
            End If
        Else
            SecurityFields = "" & vbCrLf
        End If

        sql.AppendLine("SELECT CFT.TemplateName, CFT.TemplateType, CFT.TemplateID, CFT.Comments " & String.Format(SecurityFields, "EditAllowed"))
        sql.AppendLine("FROM CustomField_Templates CFT ")
        sql.AppendLine("	INNER JOIN CustomField_Automation ST ON ST.TemplateID = CFT.TemplateID ")
        If ApplySecurity Then sql.AppendLine("    LEFT OUTER JOIN fnCustomField_GetAllowedTemplates(" & ObjectUserID & ") SecMatrix ON SecMatrix.TemplateID = CFT.TemplateID ")
        sql.AppendLine("WHERE ST.AutomatorType = '" & TemplateObjectType & "' AND AutomatorID = " & ObjectStatusID)
        sql.AppendLine("GROUP BY CFT.TemplateName, CFT.TemplateType, CFT.TemplateID, CFT.Comments " & String.Format(SecurityFields, "") & " ")
        sql.AppendLine("ORDER BY CFT.TemplateName  ")

        Return DataHelper.ExecuteDataset(db, sql.ToString, "RowsAffected")

    End Function

    'TODO: Custom Fields: Apply Edit Permissions where this method is called/used
    Public Function Get_Templates(
        Optional ByVal ApplySecurity As Boolean = False,
        Optional ByVal ApplyActivationRules As Boolean = False,
        Optional ByVal ActivationType As String = "",
        Optional ByVal Activator As String = "",
        Optional ByVal UsedTemplatesOnly As Boolean = False,
        Optional ByVal ObjectID As Long = 0,
        Optional ByVal ObjectType As String = ""
    ) As DataTable
        '
        Dim strSQL As String = "SELECT CFT.TemplateName, CFT.TemplateType, CFT.TemplateID, CFT.Comments "
        If ApplySecurity Then
            If ApplyActivationRules Then
                strSQL &= ", ISNULL( dbo.fnCustomField_ActivateTemplate( SecMatrix.TemplateID, '" & ActivationType & "', " & Activator & " ), SecMatrix.EditAllowed) EditAllowed " & vbCrLf
            Else
                strSQL &= ", SecMatrix.EditAllowed" & vbCrLf
            End If
        Else
            strSQL &= "" & vbCrLf
        End If

        strSQL &= "FROM CustomField_Templates CFT" & vbCrLf
        If ApplySecurity Then strSQL &= "    INNER JOIN fnCustomField_GetAllowedTemplates(" & ObjectUserID & ") SecMatrix ON SecMatrix.TemplateID = CFT.TemplateID " & vbCrLf
        If UsedTemplatesOnly Then strSQL &= String.Format("    INNER JOIN CustomField_ObjectTemplates U ON CFT.TemplateID = U.TemplateID AND U.ObjectType = '{0}' AND U.ObjectID = {1}", ObjectType, ObjectID) & vbCrLf
        strSQL &= "ORDER BY CFT.TemplateName"

        Return DataHelper.ExecuteDatatable(db, strSQL, "prjTemplates")

    End Function

    Public Function GetCustomFieldsList(ByVal CustomFieldIDs As List(Of Long)) As List(Of String)

        Dim Fields As New List(Of String)

        Try

            If CustomFieldIDs IsNot Nothing AndAlso CustomFieldIDs.Count > 0 Then

                Dim strSQL As String = "SELECT FieldName FROM CustomField_Fields WHERE CustomFieldID IN (0" & Join(CustomFieldIDs.Select(Function(id) id.ToString).ToArray, ", ") & ") ORDER BY DisplayIndex"
                Dim ds As DataSet = DataHelper.ExecuteDataset(db, strSQL)

                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    For Each dr As DataRow In ds.Tables(0).Rows
                        Fields.Add(Catchnull(dr("FieldName"), String.Empty))
                    Next

                End If

                Return Fields.Distinct.Where(Function(col) Not String.IsNullOrWhiteSpace(col)).ToList

            End If

        Catch ex As Exception
            Log.Error(ex)
        End Try

        Return Fields

    End Function

    Public Function RenameTemplate(ByVal OldName, ByVal NewName) As Boolean

        Try

            Dim sql As String = String.Empty
            sql &= "DECLARE @NewName varchar(255), @OldName varchar(255); " & vbCrLf
            sql &= "SELECT @NewName = '" & NewName.replace("'", "''") & "', @OldName = '" & OldName.replace("'", "''") & "'; " & vbCrLf
            sql &= "UPDATE CustomField_Templates SET TemplateName = @NewName WHERE TemplateName = @OldName;" & vbCrLf

            db.ExecuteNonQuery(CommandType.Text, sql)
            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            Log.Error(ex)
            Return False

        End Try

    End Function

    Public Function GetStatusTemplateCustomFields(ByVal ID As Long, ByVal ObjectType As String, ByVal ObjectStatusID As Long, Optional ByVal TemplateObjectType As String = "", Optional ByVal ApplySecurity As Boolean = True, Optional ByVal ApplyActivationRules As Boolean = False, Optional ByVal ActivationType As String = "", Optional ByVal Activator As String = "") As DataSet

        If TemplateObjectType = "" Then TemplateObjectType = ObjectType

        Dim sql As New System.Text.StringBuilder
        Dim SecurityFields As String = String.Empty
        If ApplySecurity Then
            If ApplyActivationRules Then
                SecurityFields = ", ISNULL( dbo.fnCustomField_ActivateTemplate( SecMatrix.TemplateID, '" & ActivationType & "', " & Activator & " ), SecMatrix.EditAllowed) {0} "
            Else
                SecurityFields = ", SecMatrix.EditAllowed"
            End If
        Else
            SecurityFields = "" & vbCrLf
        End If

        'sql = "SELECT FieldName,ISNULL(FieldValue1,'') AS FieldValue1,FieldType,DisplayIndex FROM CustomField_ObjectData WHERE ObjectType='P' AND (ObjectID = " & projectID & ") ORDER BY DisplayIndex"
        sql.AppendLine("SELECT PT.FieldName,  NULL AS FieldValue1, 0 As ObjectDataID, PT.FieldType, SecMatrix.TemplateName, PT.DisplayIndex, PT.Required, ISNULL(PT.NewLine,0) AS NewLine, ISNULL(PT.LoadOnDemand,0) AS LoadOnDemand, PT.HelpNotes, PT.CustomFieldID, 0 AS CustomFieldID " & String.Format(SecurityFields, "EditAllowed"))
        sql.AppendLine("FROM CustomField_Fields PT ")
        sql.AppendLine("	LEFT OUTER JOIN CustomField_Automation ST ON ST.TemplateID = PT.TemplateID ")
        If ApplySecurity Then
            sql.AppendLine("    LEFT OUTER JOIN fnCustomField_GetAllowedTemplates(" & ObjectUserID & ") SecMatrix ON SecMatrix.TemplateID = PT.TemplateID ")
        Else
            sql.AppendLine("    INNER JOIN CustomField_Templates SecMatrix ON SecMatrix.TemplateID = PT.TemplateID ")
        End If
        sql.AppendLine("WHERE ST.AutomatorType = '" & TemplateObjectType & "' AND AutomatorID = " & ObjectStatusID)
        sql.AppendLine("GROUP BY PT.FieldName, PT.FieldType, SecMatrix.TemplateName, PT.DisplayIndex, PT.Required, ISNULL(PT.NewLine,0), ISNULL(PT.LoadOnDemand,0), PT.HelpNotes, PT.CustomFieldID" & String.Format(SecurityFields, "") & " ")
        sql.AppendLine("ORDER BY SecMatrix.TemplateName, PT.DisplayIndex ")

        Return DataHelper.ExecuteDataset(db, sql.ToString, "RowsAffected")

    End Function

    ''' <summary>
    ''' This method creates status templates for objects (Project, Contacts, Locations, or Documents) based on 
    ''' rules defined against some of the object's properties such as Type and Status. The rules state
    ''' what custom fields must be added to it when it's property has a certain value.
    ''' </summary>
    ''' <param name="ObjectID">The ID of the object. For projects, use Projects.ObjectID, 
    ''' for contacts use Contactss.ContactID and for documents, use Documents.ID</param>
    ''' <param name="ObjectType">The type identifier for the object in question. For Locations use L, Projects use P, for Documents use D and for contacts, use C.</param>
    ''' <param name="AutomatorID">The status ID against whose rules we are going to apply. This must be the numeric value from the database and NOT the name</param>
    ''' <param name="AutomatorType">Identifies what the ObjectStatusID refers to. For Project Type use PT, for Project Status use PS, for Contact Type use CT, for Contact Status use CS, for Document Type use DT, for Location Type use LT and for Document Status use DS.</param>
    ''' <param name="RowsAffected">This returns the actual number of rows affected by the changes.</param>
    ''' <returns></returns>
    ''' <remarks>A rowCount of less than 0 (zero) indicates an error occured during processing.</remarks>
    Public Function UpdateObjectWithStatusTemplates(
        ByVal ObjectID As Long,
        ByVal ObjectType As String,
        ByVal AutomatorID As Long,
        ByVal AutomatorType As AutomatorTypes,
        Optional ByRef RowsAffected As Long = 0
    ) As Boolean

        Dim sql As New System.Text.StringBuilder

        Try

            Dim AutomatorTypeIdentifier As String = String.Empty

            Select Case AutomatorType
                Case AutomatorTypes.Project
                    AutomatorTypeIdentifier = "P"
                Case AutomatorTypes.HealthCenter
                    AutomatorTypeIdentifier = "H"
                Case AutomatorTypes.School
                    AutomatorTypeIdentifier = "S"
                Case AutomatorTypes.Organization
                    AutomatorTypeIdentifier = "O"
                Case AutomatorTypes.Community
                    AutomatorTypeIdentifier = "C"
                Case AutomatorTypes.Group
                    AutomatorTypeIdentifier = "G"
                Case AutomatorTypes.Household
                    AutomatorTypeIdentifier = "HH"
            End Select

            sql.AppendLine("INSERT INTO [CustomField_ObjectTemplates] (TemplateID, ObjectID, ObjectType, CreatedBy)")
            sql.AppendLine("SELECT TemplateID, " & ObjectID & ", '" & ObjectType & "', " & ObjectUserID)
            sql.AppendLine("FROM CustomField_Automation")
            sql.AppendLine("WHERE AutomatorType = '" & AutomatorTypeIdentifier & "' AND AutomatorID = " & AutomatorID)
            sql.AppendLine("    AND NOT EXISTS (SELECT * FROM CustomField_ObjectTemplates WHERE TemplateID = CustomField_Automation.TemplateID AND ObjectID = " & ObjectID & " AND ObjectType = '" & ObjectType & "');")
            sql.AppendLine("SELECT @@ROWCOUNT;")

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql.ToString)

            RowsAffected = -1
            Dim ds As DataSet = db.ExecuteDataSet(cmd)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso Not IsDBNull(ds.Tables(0).Rows(0)(0)) AndAlso IsNumeric(ds.Tables(0).Rows(0)(0)) Then
                RowsAffected = ds.Tables(0).Rows(0)(0)
            End If

            If RowsAffected > 0 Then
                UpdateObjectsUsingTemplate(ObjectType, ObjectID)
            End If

            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            Log.Error(sql.ToString, ex)
            Return False

        End Try

    End Function

    ''' <summary>
    ''' Checks for objects that are not using ALL the fields in the templates they have been assigned.
    ''' If any such objects exist, it inserts the missing fields. The method should be executed
    ''' whenever existing (and used templates) are modified.
    ''' </summary>
    ''' <param name="ObjectType"></param>
    ''' <param name="ObjectID"></param>
    ''' <returns></returns>
    ''' <remarks>Method performs very badly in a web environment on large databases due to
    ''' browser timeouts. It might be advisable to run this method in alternative environments
    ''' such as SQL Server Management Studio, a desktop app or a windows service</remarks>
    Public Function UpdateObjectsUsingTemplate(
        Optional ByVal ObjectType As String = "",
        Optional ByVal ObjectID As Long = 0
    ) As Boolean

        'TODOL: Implement ObjectType

        Select Case ObjectType.Trim
            Case "P", "C", "S", "H", "O", "G", "HH"
                ObjectType = ObjectType.Trim
            Case Else
                'invalid entity type. we'll clear and just do for everything
                ObjectType = String.Empty
        End Select

        Try

            Dim sql As New System.Text.StringBuilder
            sql.AppendLine(";WITH UsedTemplateFields AS (")
            sql.AppendLine("")

            If String.IsNullOrEmpty(ObjectType) OrElse ObjectType.Equals("C") Then

                sql.AppendLine("")
                sql.AppendLine("	SELECT DISTINCT T.CustomFieldID, CFT.TemplateName, F.ObjectID, F.FieldName, T.FieldType, T.DisplayIndex, F.ObjectType ")
                sql.AppendLine("	FROM CustomField_ObjectTemplates U")
                sql.AppendLine("		INNER JOIN CustomField_Templates CFT ON U.TemplateID = CFT.TemplateID AND CFT.TemplateType != 'Grid'")
                sql.AppendLine("		INNER JOIN tblCommunities P ON U.ObjectID = P.CommunityID AND U.ObjectType = 'C'")
                sql.AppendLine("		INNER JOIN CustomField_ObjectData F ON F.ObjectID = P.CommunityID AND F.ObjectType = 'C'")
                sql.AppendLine("		INNER JOIN CustomField_Fields T ON F.CustomFieldID = T.CustomFieldID AND U.TemplateID = T.TemplateID")
                sql.AppendLine("	WHERE CFT.TemplateName IS NOT NULL")
                If ObjectID > 0 Then sql.AppendLine("	AND P.CommunityID = " & ObjectID)
                sql.AppendLine("")

            End If

            If String.IsNullOrEmpty(ObjectType) OrElse ObjectType.Equals("H") Then

                If String.IsNullOrEmpty(ObjectType) Then sql.AppendLine("	UNION")

                sql.AppendLine("")
                sql.AppendLine("	SELECT DISTINCT T.CustomFieldID, CFT.TemplateName, F.ObjectID, F.FieldName, T.FieldType, T.DisplayIndex, F.ObjectType ")
                sql.AppendLine("	FROM CustomField_ObjectTemplates U")
                sql.AppendLine("		INNER JOIN CustomField_Templates CFT ON U.TemplateID = CFT.TemplateID AND CFT.TemplateType != 'Grid'")
                sql.AppendLine("		INNER JOIN tblCustomFields_Objects L ON U.ObjectID = L.ObjectID AND U.ObjectType = 'H'")
                sql.AppendLine("		INNER JOIN CustomField_ObjectData F ON F.ObjectID = L.ObjectID AND F.ObjectType = 'H'")
                sql.AppendLine("		INNER JOIN CustomField_Fields T ON F.CustomFieldID = T.CustomFieldID AND U.TemplateID = T.TemplateID")
                sql.AppendLine("	WHERE CFT.TemplateName IS NOT NULL")
                If ObjectID > 0 Then sql.AppendLine("	AND L.ObjectID = " & ObjectID & " AND L.Code = 'H'")
                sql.AppendLine("")

            End If

            If String.IsNullOrEmpty(ObjectType) OrElse ObjectType.Equals("HH") Then

                If String.IsNullOrEmpty(ObjectType) Then sql.AppendLine("	UNION")

                sql.AppendLine("")
                sql.AppendLine("	SELECT DISTINCT T.CustomFieldID, CFT.TemplateName, F.ObjectID, F.FieldName, T.FieldType, T.DisplayIndex, F.ObjectType ")
                sql.AppendLine("	FROM CustomField_ObjectTemplates U")
                sql.AppendLine("		INNER JOIN CustomField_Templates CFT ON U.TemplateID = CFT.TemplateID AND CFT.TemplateType != 'Grid'")
                sql.AppendLine("		INNER JOIN tblCustomFields_Objects L ON U.ObjectID = L.ObjectID AND U.ObjectType = 'HH'")
                sql.AppendLine("		INNER JOIN CustomField_ObjectData F ON F.ObjectID = L.ObjectID AND F.ObjectType = 'HH'")
                sql.AppendLine("		INNER JOIN CustomField_Fields T ON F.CustomFieldID = T.CustomFieldID AND U.TemplateID = T.TemplateID")
                sql.AppendLine("	WHERE CFT.TemplateName IS NOT NULL")
                If ObjectID > 0 Then sql.AppendLine("	AND L.ObjectID = " & ObjectID & " AND L.Code = 'HH'")
                sql.AppendLine("")

            End If

            If String.IsNullOrEmpty(ObjectType) OrElse ObjectType.Equals("O") Then

                If String.IsNullOrEmpty(ObjectType) Then sql.AppendLine("	UNION")

                sql.AppendLine("")
                sql.AppendLine("	SELECT DISTINCT T.CustomFieldID, CFT.TemplateName, F.ObjectID, F.FieldName, T.FieldType, T.DisplayIndex, F.ObjectType ")
                sql.AppendLine("	FROM CustomField_ObjectTemplates U")
                sql.AppendLine("		INNER JOIN CustomField_Templates CFT ON U.TemplateID = CFT.TemplateID AND CFT.TemplateType != 'Grid'")
                sql.AppendLine("		INNER JOIN tblCustomFields_Objects L ON U.ObjectID = L.ObjectID AND U.ObjectType = 'O'")
                sql.AppendLine("		INNER JOIN CustomField_ObjectData F ON F.ObjectID = L.ObjectID AND F.ObjectType = 'O'")
                sql.AppendLine("		INNER JOIN CustomField_Fields T ON F.CustomFieldID = T.CustomFieldID AND U.TemplateID = T.TemplateID")
                sql.AppendLine("	WHERE CFT.TemplateName IS NOT NULL")
                If ObjectID > 0 Then sql.AppendLine("	AND L.ObjectID = " & ObjectID & " AND L.Code = 'O'")
                sql.AppendLine("")

            End If

            If String.IsNullOrEmpty(ObjectType) OrElse ObjectType.Equals("P") Then

                If String.IsNullOrEmpty(ObjectType) Then sql.AppendLine("	UNION")

                sql.AppendLine("")
                sql.AppendLine("	SELECT DISTINCT T.CustomFieldID, CFT.TemplateName, F.ObjectID, F.FieldName, T.FieldType, T.DisplayIndex, F.ObjectType ")
                sql.AppendLine("	FROM CustomField_ObjectTemplates U")
                sql.AppendLine("		INNER JOIN CustomField_Templates CFT ON U.TemplateID = CFT.TemplateID AND CFT.TemplateType != 'Grid'")
                sql.AppendLine("		INNER JOIN tblCustomFields_Objects P ON U.ObjectID = P.ObjectID AND U.ObjectType = 'P'")
                sql.AppendLine("		LEFT OUTER JOIN CustomField_ObjectData F ON F.ObjectID = P.ObjectID AND F.ObjectType = 'P'")
                sql.AppendLine("		INNER JOIN CustomField_Fields T ON F.CustomFieldID = T.CustomFieldID AND U.TemplateID = T.TemplateID")
                sql.AppendLine("	WHERE CFT.TemplateName IS NOT NULL")
                If ObjectID > 0 Then sql.AppendLine("	AND P.ObjectID = " & ObjectID & " AND P.Code = 'P'")
                sql.AppendLine("")

            End If

            If String.IsNullOrEmpty(ObjectType) OrElse ObjectType.Equals("S") Then

                If String.IsNullOrEmpty(ObjectType) Then sql.AppendLine("	UNION")

                sql.AppendLine("")
                sql.AppendLine("	SELECT DISTINCT T.CustomFieldID, CFT.TemplateName, F.ObjectID, F.FieldName, T.FieldType, T.DisplayIndex, F.ObjectType ")
                sql.AppendLine("	FROM CustomField_ObjectTemplates U")
                sql.AppendLine("		INNER JOIN CustomField_Templates CFT ON U.TemplateID = CFT.TemplateID AND CFT.TemplateType != 'Grid'")
                sql.AppendLine("		INNER JOIN tblCustomFields_Objects L ON U.ObjectID = L.ObjectID AND U.ObjectType = 'S'")
                sql.AppendLine("		INNER JOIN CustomField_ObjectData F ON F.ObjectID = L.ObjectID AND F.ObjectType = 'S'")
                sql.AppendLine("		INNER JOIN CustomField_Fields T ON F.CustomFieldID = T.CustomFieldID AND U.TemplateID = T.TemplateID")
                sql.AppendLine("	WHERE CFT.TemplateName IS NOT NULL")
                If ObjectID > 0 Then sql.AppendLine("	AND L.ObjectID = " & ObjectID & " AND L.Code = 'S'")
                sql.AppendLine("")

            End If

            If String.IsNullOrEmpty(ObjectType) OrElse ObjectType.Equals("G") Then

                If String.IsNullOrEmpty(ObjectType) Then sql.AppendLine("	UNION")

                sql.AppendLine("")
                sql.AppendLine("	SELECT DISTINCT T.CustomFieldID, CFT.TemplateName, F.ObjectID, F.FieldName, T.FieldType, T.DisplayIndex, F.ObjectType ")
                sql.AppendLine("	FROM CustomField_ObjectTemplates U")
                sql.AppendLine("		INNER JOIN CustomField_Templates CFT ON U.TemplateID = CFT.TemplateID AND CFT.TemplateType != 'Grid'")
                sql.AppendLine("		INNER JOIN tblCustomFields_Objects L ON U.ObjectID = L.ObjectID AND U.ObjectType = 'S'")
                sql.AppendLine("		INNER JOIN CustomField_ObjectData F ON F.ObjectID = L.ObjectID AND F.ObjectType = 'S'")
                sql.AppendLine("		INNER JOIN CustomField_Fields T ON F.CustomFieldID = T.CustomFieldID AND U.TemplateID = T.TemplateID")
                sql.AppendLine("	WHERE CFT.TemplateName IS NOT NULL")
                If ObjectID > 0 Then sql.AppendLine("	AND L.ObjectID = " & ObjectID & " AND L.Code = 'G'")
                sql.AppendLine("")

            End If

            sql.AppendLine("), AllFields AS (")
            sql.AppendLine("	SELECT DISTINCT T.CustomFieldID, CFT.TemplateName, U.ObjectID, T.FieldName, T.FieldType, T.DisplayIndex, U.ObjectType ObjectType")
            sql.AppendLine("	FROM CustomField_ObjectTemplates U")
            sql.AppendLine("		INNER JOIN CustomField_Templates CFT ON U.TemplateID = CFT.TemplateID AND CFT.TemplateType != 'Grid'")
            sql.AppendLine("		LEFT OUTER JOIN CustomField_Fields T ON U.TemplateID = T.TemplateID")

            If Not String.IsNullOrEmpty(ObjectType) Then
                sql.AppendLine("	WHERE U.ObjectType = '" & ObjectType & "'")
                If ObjectID > 0 Then sql.AppendLine("	AND U.ObjectID = " & ObjectID)
            End If

            sql.AppendLine(")")
            sql.AppendLine("INSERT INTO CustomField_ObjectData (CustomFieldID, ObjectID, FieldName, FieldType, FieldValue1, DisplayIndex, ObjectType, CreatedBy)")
            sql.AppendLine("SELECT CustomFieldID, ObjectID, FieldName, FieldType, FieldValue1, DisplayIndex, ObjectType, " & ObjectUserID & " FROM (")
            sql.AppendLine("	SELECT CustomFieldID, ObjectID, FieldName, FieldType, NULL FieldValue1, DisplayIndex, ObjectType FROM AllFields  ")
            sql.AppendLine("	EXCEPT")
            sql.AppendLine("	SELECT CustomFieldID, ObjectID, FieldName, FieldType, NULL, DisplayIndex, ObjectType FROM UsedTemplateFields")
            sql.AppendLine("	")
            sql.AppendLine(") AS NewFields WHERE FieldName IS NOT NULL")

            db.ExecuteNonQuery(CommandType.Text, sql.ToString)
            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            Log.Error(ex)
            Return False

        End Try

    End Function

    ''' <summary>
    ''' Get a dataset containing all the custom fields linked to a template
    ''' </summary>
    ''' <param name="TemplateID">the TemplateID for which to get fields</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTemplateFields(ByVal TemplateID As Long) As DataTable

        Dim sql As String = "SELECT CustomFieldID, TemplateName, FieldName, ISNULL(FieldType, 'Text') AS FieldType, ISNULL(DisplayIndex,0) AS DisplayIndex, ISNULL(Search, 0) AS Search, ISNULL(NewLine, 0) AS NewLine, ISNULL(Required, 0) AS Required, ISNULL(ShowInResults, 0) AS ShowInResults, ISNULL(LoadOnDemand, 0) LoadOnDemand, HelpNotes FROM CustomField_Fields F INNER JOIN CustomField_Templates T ON F.TemplateID = T.TemplateID WHERE F.TemplateID = " & TemplateID
        Return DataHelper.ExecuteDataTable(db, sql, "TemplateFields")

    End Function

    Public Function SaveTemplateField(
        ByVal CustomFieldID As Long,
        ByVal TemplateID As Long,
        ByVal FieldName As String,
        ByVal FieldType As String,
        ByVal DisplayIndex As Byte,
        ByVal Search As Boolean,
        ByVal Required As Boolean,
        ByVal NewLine As Boolean,
        ByVal ShowInResults As Boolean,
        ByVal LoadOnDemand As Boolean,
        ByVal HelpNotes As String
    ) As Boolean

        Try

            If CustomFieldID = 0 And FieldName = "" Then Return True

            Dim sql As String = ""

            If CustomFieldID = 0 Then

                sql &= "INSERT INTO [CustomField_Fields](TemplateID,FieldName, FieldType, DisplayIndex, Search, Required, NewLine, ShowInResults, LoadOnDemand, HelpNotes, CreatedBy) " & vbCrLf
                sql &= "VALUES (@TemplateID, @FieldName, @FieldType, @DisplayIndex, @Search, @Required, @NewLine, @ShowInResults, @LoadOnDemand, @HelpNotes, @UpdatedBy) " & vbCrLf

            Else

                sql &= "UPDATE [CustomField_Fields] SET " & vbCrLf
                sql &= "	   [TemplateID] = @TemplateID " & vbCrLf
                sql &= "      ,[FieldName] = @FieldName " & vbCrLf
                sql &= "      ,[FieldType] = @FieldType " & vbCrLf
                sql &= "      ,[DisplayIndex] = @DisplayIndex " & vbCrLf
                sql &= "      ,[Search] = @Search " & vbCrLf
                sql &= "      ,[Required] = @Required " & vbCrLf
                sql &= "      ,[NewLine] = @NewLine " & vbCrLf
                sql &= "      ,[ShowInResults] = @ShowInResults " & vbCrLf
                sql &= "      ,[LoadOnDemand] = @LoadOnDemand " & vbCrLf
                sql &= "      ,[HelpNotes] = @HelpNotes " & vbCrLf
                sql &= "      ,[UpdatedBy] = @UpdatedBy " & vbCrLf
                sql &= "      ,[UpdatedDate] = getdate() " & vbCrLf
                sql &= " WHERE CustomFieldID = @CustomFieldID " & vbCrLf
                sql &= " " & vbCrLf

            End If

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)

            db.AddInParameter(cmd, "@TemplateID", DbType.Int32, TemplateID)
            db.AddInParameter(cmd, "@FieldName", DbType.String, FieldName)
            db.AddInParameter(cmd, "@FieldType", DbType.String, FieldType)
            db.AddInParameter(cmd, "@DisplayIndex", DbType.Int32, DisplayIndex)
            db.AddInParameter(cmd, "@Search", DbType.Boolean, Search)
            db.AddInParameter(cmd, "@Required", DbType.Boolean, Required)
            db.AddInParameter(cmd, "@NewLine", DbType.Boolean, NewLine)
            db.AddInParameter(cmd, "@ShowInResults", DbType.Boolean, ShowInResults)
            db.AddInParameter(cmd, "@LoadOnDemand", DbType.Boolean, LoadOnDemand)
            db.AddInParameter(cmd, "@HelpNotes", DbType.String, HelpNotes)
            db.AddInParameter(cmd, "@CustomFieldID", DbType.Int32, CustomFieldID)
            db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, objectuserid)

            If Not db.ExecuteNonQuery(cmd) > 0 Then
                Return False
            End If

            If CustomFieldID = 0 Then
                CreateGridTemplateField(FieldName)
            End If

            Return True

        Catch ex As Exception

            Log.Error(ex)
            Return False

        End Try

    End Function

    Public Function CreateGridTemplateField(ByVal FieldName As String) As Boolean

        Dim sql As String = String.Empty

        Try

            sql &= "declare @sql varchar(4000) " & vbCrLf
            sql &= "  " & vbCrLf
            sql &= ";with CFieldTypes(FieldType, DB_Type) as ( " & vbCrLf
            sql &= "	select 'Checkbox', 'varchar(5)' " & vbCrLf
            sql &= "	union select 'Combo', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'Country', 'varchar(100)' " & vbCrLf
            sql &= "	union select 'ContactList', 'varchar(255)' " & vbCrLf
            sql &= "	union select 'Currency', 'varchar(25)' " & vbCrLf
            sql &= "	union select 'ContactsEntityList', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'ContactsLookup', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'ProjectsLookup', 'varchar(4000)' " & vbCrLf
            sql &= "	--union select 'DocumentsChecklist', 'varchar(4000)' " & vbCrLf
            sql &= "	--union select 'DocumentsTable', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'ContactsEntityLookup', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'FreeText', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'FollowUpDate', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'LocationIndexLookup', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'Date', 'datetime' " & vbCrLf
            sql &= "	union select 'DateTime', 'datetime' " & vbCrLf
            sql &= "	union select 'Duplicate', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'MultiValue', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'Notes', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'Numeric', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'Money', 'currency' " & vbCrLf
            sql &= "	union select 'Rating', 'varchar(10)' " & vbCrLf
            sql &= "	union select 'Text', 'varchar(4000)' " & vbCrLf
            sql &= "	union select 'Time', 'datetime' " & vbCrLf
            sql &= "	union select 'UserLookup', 'varchar(100)' " & vbCrLf
            sql &= "	union select 'CustomDataLookup', 'varchar(4000)' " & vbCrLf
            sql &= ") " & vbCrLf
            sql &= "select @sql = " & vbCrLf
            sql &= "'if not exists (select * from sys.tables where [name]=''CustomField_Grid_' + cast(T.TemplateID as varchar) + ''') " & vbCrLf
            sql &= "begin " & vbCrLf
            sql &= "	create table CustomField_Grid_' + cast(T.TemplateID as varchar) + ' ( " & vbCrLf
            sql &= "		DataID int identity(1,1) not null, " & vbCrLf
            sql &= "		ObjectID int not null, " & vbCrLf
            sql &= "		ObjectType varchar(5) not null, " & vbCrLf
            sql &= "		EntryDate datetime not null default(getdate()), " & vbCrLf
            sql &= "		EntryPerson int not null, " & vbCrLf
            sql &= "		CField_' + cast(F.CustomFieldID as varchar) + ' ' + FT.DB_Type + ' null " & vbCrLf
            sql &= "	) " & vbCrLf
            sql &= "  " & vbCrLf
            sql &= "  alter table CustomField_Grid_' + cast(T.TemplateID as varchar) + ' add constraint PK_CustomField_Grid_' + cast(T.TemplateID as varchar) + ' primary key clustered (DataID) " & vbCrLf
            sql &= "  with( statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on) on [primary] " & vbCrLf
            sql &= "  " & vbCrLf
            sql &= "  create nonclustered index [NCIX_CustomField_Grid_' + cast(T.TemplateID as varchar) + '_ObjectType_EntityID] ON [CustomField_Grid_' + cast(T.TemplateID as varchar) + ']([ObjectType] ASC, [ObjectID] ASC) " & vbCrLf
            sql &= "  with (pad_index = off, statistics_norecompute = off, sort_in_tempdb = off, drop_existing = off, online = off, allow_row_locks = on, allow_page_locks = on, fillfactor = 80) " & vbCrLf
            sql &= "  " & vbCrLf
            sql &= "end " & vbCrLf
            sql &= "else if not exists (select * from sys.columns where [name]=''CField_' + cast(F.CustomFieldID as varchar) + ''' and object_id=object_id(''CustomField_Grid_' + cast(T.TemplateID as varchar) + ''')) " & vbCrLf
            sql &= "	exec(''alter table CustomField_Grid_' + cast(T.TemplateID as varchar) + ' add CField_' + cast(F.CustomFieldID as varchar) + ' ' + FT.DB_Type + ' null'') ' " & vbCrLf
            sql &= "from CustomField_Templates T " & vbCrLf
            sql &= "	inner join CustomField_Fields F on T.TemplateID = F.TemplateID   " & vbCrLf
            sql &= "	inner join CFieldTypes FT on F.FieldType = FT.FieldType  " & vbCrLf
            sql &= "where T.TemplateType='Grid' and F.FieldName = '" & FieldName & "' " & vbCrLf
            sql &= vbCrLf
            sql &= "if @sql is not null" & vbCrLf
            sql &= "    exec (@sql);"
            sql &= vbCrLf
            'sql &= "if @sql is not null"

            Return db.ExecuteNonQuery(CommandType.Text, sql)

        Catch ex As Exception

            Log.Error(sql, ex)
            Return False

        End Try

    End Function

    Public Function Get_Custom_Field_Value(ByVal ID As Long, ByVal ObjectType As String, ByVal FieldName As String, Optional ByVal UseSmartCaching As Boolean = False) As String

        Static FieldValueCache As New Hashtable

        Dim dr As DataRow = Nothing

        If UseSmartCaching Then

            Dim ds As DataSet = TryCast(FieldValueCache(String.Format("{0}:{1}", ID, ObjectType)), DataSet)

            If ds Is Nothing Then

                'We do NOT have the custom fields for this object loaded already.
                'We will get the data from the database

                FieldValueCache = New Hashtable 'Reinitialise cache. We can't keep the data too long otherwise we increase risk of stale data

                Dim strSQL As String = "SELECT FieldName, ISNULL(FieldValue1,'') AS FieldValue1 " &
                    "FROM  CustomField_ObjectData " &
                    "WHERE ObjectType='" & ObjectType & "' AND (ObjectID = " & ID & ") AND FieldType <> 'Duplicate'" &
                    "ORDER BY FieldName"

                FieldValueCache(String.Format("{0}:{1}", ID, ObjectType)) = DataHelper.ExecuteDataset(db, strSQL)
                ds = TryCast(FieldValueCache(String.Format("{0}:{1}", ID, ObjectType)), DataSet)

            End If

            'We now have the values for this field already loaded, so we dont make a database call. 
            'We will just use the cached datatable. This will result in a performance boost
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                Dim drFields() As DataRow = ds.Tables(0).Select("FieldName = '" & FieldName.Replace("'", "''") & "'")
                If drFields IsNot Nothing AndAlso drFields.Length > 0 Then
                    dr = drFields(0)
                End If

            End If

        Else

            Dim strSQL As String = String.Empty
            strSQL &= "SELECT ISNULL(FieldValue1,'') AS FieldValue1 " & _
                "FROM  CustomField_ObjectData WHERE ObjectType='" & ObjectType & "' AND " & _
                "(ObjectID = " & ID & ") AND (FieldName = '" & FieldName.Replace("'", "''") & "') AND FieldType <> 'Duplicate'"

            Dim ds As DataSet = DataHelper.ExecuteDataset(db, strSQL, "CustomFieldValue" & FieldName)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                dr = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)
            End If

        End If

        If dr IsNot Nothing Then
            Return Catchnull(dr("FieldValue1"), String.Empty)
        Else
            Return String.Empty
        End If

    End Function

    Public Function Get_Custom_Field_Values(ByVal ObjectType As String, ByVal FieldName As String, ByVal FieldType As String, Optional ByVal ForSearch As Boolean = False) As DataTable

        'Lookup
        Dim strSQL As String = ""
        Dim sql As New System.Text.StringBuilder

        Select Case FieldType.ToLower

            Case "freetext"

                Return Nothing

            Case "numeric"

                Return Nothing 'strSQL = "SELECT DISTINCT CAST(NULLIF(LTRIM(RTRIM(FieldValue)),'') AS float) AS FieldValue FROM CustomField_ObjectData WHERE ObjectType = '" & ObjectType & "' AND ISNUMERIC(NULLIF(LTRIM(RTRIM(FieldValue)),'')) = 1 AND (FieldName = '" & FieldName.Replace("'", "''") & "') GROUP BY FieldValue ORDER BY FieldValue ASC"

            Case "multivalue", "contactslookup", "projectslookup", "locationindexlookup", "contactsentitylookup", "customdatalookup"

                sql.AppendLine(String.Format("SELECT FieldValue, FieldName FROM CustomField_DataLookup WHERE FieldName = '{0}' OR FieldName = '{0}::Status' OR FieldName = '{0}::Type' ", FieldName.Replace("'", "''")))
                If ForSearch Then sql.Append("OR FieldName = '" & FieldName.Replace("'", "''") & ".Search'")
                strSQL = sql.ToString

            Case "contactlist"

                sql.AppendLine("IF EXISTS(SELECT FieldValue FROM CustomField_DataLookup WHERE FieldName = '" & FieldName.Replace("'", "''") & "')")
                sql.AppendLine("	SELECT P.ContactID, ISNULL([Root],'') + ISNULL(' > ' + CASE P.Parent WHEN '-Root-' THEN NULL ELSE P.Parent END, '') AS FieldValue  ")
                sql.AppendLine("	FROM Contacts P  ")
                sql.AppendLine("	WHERE P.Description IN (SELECT FieldValue FROM CustomField_DataLookup WHERE FieldName = '" & FieldName.Replace("'", "''") & "')")
                sql.AppendLine("	UNION SELECT NULL, '' ")
                sql.AppendLine("	ORDER BY FieldValue ")
                sql.AppendLine("ELSE ")
                sql.AppendLine("	SELECT P.ContactID, ISNULL([Root],'') + ISNULL(' > ' + CASE P.Parent WHEN '-Root-' THEN NULL ELSE P.Parent END, '') AS FieldValue  ")
                sql.AppendLine("	FROM Contacts P  ")
                sql.AppendLine("	UNION SELECT NULL, '' ")
                sql.AppendLine("	ORDER BY FieldValue ")
                strSQL = sql.ToString

            Case "contactsentitylist"

                sql.AppendLine("IF EXISTS(SELECT FieldValue FROM CustomField_DataLookup WHERE FieldName = '" & FieldName.Replace("'", "''") & "')")
                sql.AppendLine("	SELECT P.ContactID, ISNULL([Root],'') AS FieldValue  ")
                sql.AppendLine("	FROM Contacts P  ")
                sql.AppendLine("	WHERE P.Parent = '-Root-' ")
                sql.AppendLine("        AND P.Description IN (SELECT FieldValue FROM CustomField_DataLookup WHERE FieldName = '" & FieldName.Replace("'", "''") & "')")
                sql.AppendLine("	UNION SELECT NULL, '' ")
                sql.AppendLine("	ORDER BY FieldValue ")
                sql.AppendLine("ELSE ")
                sql.AppendLine("	SELECT P.ContactID, ISNULL([Root],'') AS FieldValue  ")
                sql.AppendLine("	FROM Contacts P  ")
                sql.AppendLine("		INNER JOIN Lookup_Root R ON P.Root_ID = R.Root_ID ")
                sql.AppendLine("	WHERE P.Parent = '-Root-' ")
                sql.AppendLine("	UNION SELECT NULL, '' ")
                sql.AppendLine("	ORDER BY FieldValue ")
                strSQL = sql.ToString

            Case "currency"

                sql.AppendLine("SELECT CAST(CurrencyID AS varchar) CurrencyID, [Description] + ISNULL(' (' + Symbol + ')', '') FieldValue FROM Lookup_Currency ")
                sql.AppendLine("UNION ")
                sql.AppendLine("SELECT NULL AS CurrencyID, '' AS FieldValue FROM Lookup_Currency ")
                sql.AppendLine("ORDER BY FieldValue ")

                strSQL = sql.ToString

            Case "country"

                sql.AppendLine("SELECT '' AS FieldValue FROM Lookup_Countries")
                sql.AppendLine("UNION SELECT Country_Name AS FieldValue FROM Lookup_Countries")
                sql.AppendLine("ORDER BY FieldValue")
                strSQL = sql.ToString

            Case "userlookup"

                sql.AppendLine("IF EXISTS (SELECT * FROM CustomField_DataLookup WHERE FieldName = '" & FieldName.Replace("'", "''") & "')")
                sql.AppendLine("	SELECT [UserName], UserGroups.[UserID] AS FieldValue")
                sql.AppendLine("    FROM Users")
                sql.AppendLine("		INNER JOIN UserGroups ON UserGroups.UserID = Users.UserID")
                sql.AppendLine("		INNER JOIN Groups ON UserGroups.GroupID = Groups.GroupID")
                sql.AppendLine("		CROSS JOIN (SELECT FieldValue AS [Group] FROM CustomField_DataLookup WHERE FieldName = '" & FieldName.Replace("'", "''") & "') AS GroupsList")
                sql.AppendLine("	WHERE Groups.Group_Name LIKE GroupsList.[Group]")
                sql.AppendLine("    ORDER BY [UserName]")
                sql.AppendLine("ELSE")
                sql.AppendLine("	SELECT [UserName], [UserID] AS FieldValue FROM Users UNION SELECT '', 0 ")
                sql.AppendLine("    ORDER BY [UserName]")
                strSQL = sql.ToString

            Case Else

                sql.AppendLine("IF EXISTS (SELECT * FROM CustomField_DataLookup WHERE FieldName = '" & FieldName.Replace("'", "''") & "')")
                sql.AppendLine("	SELECT FieldValue FROM CustomField_DataLookup WHERE FieldName = '" & FieldName.Replace("'", "''") & "'")
                sql.AppendLine("	UNION")
                sql.AppendLine("	SELECT ISNULL(FieldValue1,'') AS FieldValue ")
                sql.AppendLine("	FROM CustomField_ObjectData")
                sql.AppendLine("	WHERE ObjectType = '" & ObjectType & "' ")
                sql.AppendLine("		AND (FieldName = '" & FieldName & "') ")
                sql.AppendLine("	GROUP BY FieldValue1 ")
                sql.AppendLine("	ORDER BY FieldValue ASC ")
                sql.AppendLine("ELSE")
                sql.AppendLine("	SELECT ISNULL(FieldValue1,'') AS FieldValue ")
                sql.AppendLine("	FROM CustomField_ObjectData ")
                sql.AppendLine("	WHERE ObjectType = '" & ObjectType & "' ")
                sql.AppendLine("		AND (FieldName = '" & FieldName.Replace("'", "''") & "') ")
                sql.AppendLine("	GROUP BY FieldValue1 ")
                sql.AppendLine("    ORDER BY FieldValue ASC")
                strSQL = sql.ToString

        End Select

        Return DataHelper.ExecuteDataset(db, strSQL, "fieldValues" & CleanupFilename(FieldName)).Tables(0)

    End Function

    Public Function Get_Custom_Field_Control_Loaded(
        ByVal ReadOnly_Template As Boolean,
        ByVal ObjectDataID As Long,
        ByVal CustomFieldID As Long,
        ByVal ObjectID As Long,
        ByVal ObjectType As String,
        ByVal FieldName As String,
        ByVal FieldType As String,
        ByVal ForSearch As Boolean,
        ByVal DisplayDateFormat As String,
        ByVal LoadOnDemand As Boolean
    ) As System.Web.UI.Control

        'returns a control pre loaded with the correct value
        Dim value As String = Get_Custom_Field_Value(ObjectID, ObjectType, FieldName, True)
        Dim myObject As Web.UI.WebControls.WebControl = Get_Custom_Field_Control(ObjectID, ReadOnly_Template, ObjectDataID, CustomFieldID, ObjectType, FieldName, FieldType, ForSearch, DisplayDateFormat, LoadOnDemand)

        Return SetFieldValue(value, myObject, FieldType, DisplayDateFormat, LoadOnDemand)

    End Function

    Public Function SetFieldValue(
        ByVal value As String,
        ByRef myObject As Web.UI.WebControls.WebControl,
        ByVal FieldType As String,
        ByVal DisplayDateFormat As String,
        ByVal LoadOnDemand As Boolean
    ) As System.Web.UI.Control

        If TypeOf myObject Is System.Web.UI.WebControls.DropDownList Then

            Dim o As Web.UI.WebControls.DropDownList = CType(myObject, Web.UI.WebControls.DropDownList)
            o.SelectedIndex = o.Items.IndexOf(o.Items.FindByValue(value))

        ElseIf TypeOf myObject Is Telerik.Web.UI.RadComboBox Then

            Dim o As Telerik.Web.UI.RadComboBox = CType(myObject, Telerik.Web.UI.RadComboBox)

            Select Case FieldType

                Case "contactslookup", "projectslookup", "locationindexlookup", "contactsentitylookup", "userlookup", "customdatalookup"

                    o.Items.Add(New Telerik.Web.UI.RadComboBoxItem(value, value))
                    o.SelectedIndex = o.Items.IndexOf(o.Items.FindItemByValue(value, True))

                Case "currency"

                    If o.Items.FindItemByValue(value, True) IsNot Nothing Then
                        o.SelectedIndex = o.Items.IndexOf(o.Items.FindItemByValue(value, True))
                    ElseIf o.Items.FindItemByText(value, True) IsNot Nothing Then
                        o.SelectedIndex = o.Items.IndexOf(o.Items.FindItemByText(value, True))
                    End If

                Case Else

                    Dim itm As RadComboBoxItem = o.Items.FindItemByValue(value, True)
                    If itm Is Nothing AndAlso Not String.IsNullOrWhiteSpace(value) Then
                        'o.Items.Add(New RadComboBoxItem(value, value))
                        'itm = o.Items.FindItemByValue(value, True)
                        o.Text = value
                    End If
                    o.SelectedIndex = o.Items.IndexOf(o.Items.FindItemByValue(value, True))

            End Select

        ElseIf TypeOf myObject Is Telerik.Web.UI.RadListBox Then

            Dim o As Telerik.Web.UI.RadListBox = CType(myObject, Telerik.Web.UI.RadListBox)
            Dim values() As String = value.Split(Chr(255))
            For Each v As String In values
                If v <> "" Then
                    Dim itm As Telerik.Web.UI.RadListBoxItem = o.FindItemByValue(v)
                    If itm IsNot Nothing Then
                        itm.Selected = True
                        itm.Checked = True
                        o.ReorderToIndex(itm.Index, 0)
                    End If
                End If
            Next

        ElseIf TypeOf myObject Is Telerik.Web.UI.RadTimePicker Then

            Dim o As Telerik.Web.UI.RadTimePicker = CType(myObject, Telerik.Web.UI.RadTimePicker)
            If IsDate(Today.ToString("yyyy-MM-dd") & " " & value) Then o.SelectedDate = CDate(Today.ToString("yyyy-MM-dd") & " " & value)

        ElseIf TypeOf myObject Is Telerik.Web.UI.RadDateTimePicker Then

            Dim o As Telerik.Web.UI.RadDateTimePicker = CType(myObject, Telerik.Web.UI.RadDateTimePicker)
            If IsDate(value) Then o.SelectedDate = CDate(value)

        ElseIf TypeOf myObject Is Telerik.Web.UI.RadDatePicker Then

            Dim o As Telerik.Web.UI.RadDatePicker = CType(myObject, Telerik.Web.UI.RadDatePicker)
            If IsDate(value) Then o.SelectedDate = CDate(value)

        ElseIf TypeOf myObject Is Telerik.Web.UI.RadRating Then

            Dim o As Telerik.Web.UI.RadRating = CType(myObject, Telerik.Web.UI.RadRating)
            o.Value = value

        ElseIf TypeOf myObject Is System.Web.UI.WebControls.Label Then

            Dim o As System.Web.UI.WebControls.Label = CType(myObject, System.Web.UI.WebControls.Label)
            o.Text = value

        ElseIf TypeOf myObject Is Telerik.Web.UI.RadTextBox Then

            Dim o As Telerik.Web.UI.RadTextBox = CType(myObject, Telerik.Web.UI.RadTextBox)
            o.Text = value

        ElseIf TypeOf myObject Is Telerik.Web.UI.RadNumericTextBox Then

            Dim o As Telerik.Web.UI.RadNumericTextBox = CType(myObject, Telerik.Web.UI.RadNumericTextBox)
            Dim d As Double = 0.0 : If Double.TryParse(value, d) Then o.Value = d

        ElseIf TypeOf myObject Is System.Web.UI.WebControls.Table Then

            'Do nothing with this one.


        ElseIf TypeOf myObject Is System.Web.UI.WebControls.Panel Then

            'Do nothing with this one.

        Else
            Dim o As Web.UI.WebControls.TextBox = CType(myObject, Web.UI.WebControls.TextBox)
            o.Text = value

        End If

        myObject.Attributes("CFValue") = value

        Return myObject

    End Function

    Public Shared Sub SetDateFormat(ByRef dtp As Telerik.Web.UI.RadDatePicker, DisplayDateFormat As String)

        dtp.DateInput.DateFormat = DisplayDateFormat
        dtp.DateInput.DisplayDateFormat = DisplayDateFormat
        dtp.DateInput.MinDate = New Date(1500, 1, 1)
        dtp.MinDate = New Date(1500, 1, 1)

        Dim holiday As New Telerik.Web.UI.RadCalendarDay
        holiday.Date = Today
        holiday.IsSelectable = True
        holiday.IsToday = True
        holiday.ItemStyle.BackColor = Drawing.Color.Yellow
        dtp.Calendar.SpecialDays.Add(holiday)

    End Sub

    Public Function LoadCustomFieldsPanel(
        ByRef Page As Web.UI.Page,
        ByVal ObjectID As Long,
        ByVal ObjectType As String,
        ByVal DisplayDateFormat As String,
        Optional ByVal ApplySecurity As Boolean = True,
        Optional ByVal ApplyActivationRules As Boolean = True
    ) As Telerik.Web.UI.RadPanelBar

        Dim CFPanelBar As New Telerik.Web.UI.RadPanelBar With {
            .Width = Unit.Percentage(98), .BorderWidth = Unit.Pixel(0),
            .BorderStyle = BorderStyle.None, .ExpandMode = PanelBarExpandMode.MultipleExpandedItems
        }

        Return LoadCustomFieldsPanel(Page, CFPanelBar, ObjectID, ObjectType, DisplayDateFormat, ApplySecurity, ApplyActivationRules)

    End Function

    Public Function LoadCustomFieldsPanel(
        ByRef Page As Web.UI.Page,
        ByRef CFPanelBar As RadPanelBar,
        ByVal ObjectID As Long,
        ByVal ObjectType As String,
        ByVal DisplayDateFormat As String,
        Optional ByVal ApplySecurity As Boolean = True,
        Optional ByVal ApplyActivationRules As Boolean = True
    ) As Telerik.Web.UI.RadPanelBar

        Dim TemplatesTable As DataTable = Get_Templates(
            ApplySecurity, ApplyActivationRules, ObjectType, ObjectID,
            True, ObjectID, ObjectType
        )
        Dim FieldsTable As DataTable = Get_Custom_Fields(ObjectID, ObjectType, ApplySecurity, ApplyActivationRules, ObjectType, ObjectID)

        Return LoadCustomFieldsPanel(CFPanelBar, TemplatesTable, FieldsTable, Page, ObjectID, ObjectType, DisplayDateFormat, ApplySecurity, ApplyActivationRules)

    End Function

    Public Function LoadStatusTemplateCustomFieldsPanel(
        ByRef Page As Web.UI.Page,
        ByVal ObjectID As Long,
        ByVal ObjectType As String,
        ByVal DisplayDateFormat As String,
        ByVal ActivatorID As Long,
        Optional ByVal ActivatorType As String = "",
        Optional ByVal ApplySecurity As Boolean = True,
        Optional ByVal ApplyActivationRules As Boolean = True
    ) As Telerik.Web.UI.RadPanelBar

        Dim CFPanelBar As New Telerik.Web.UI.RadPanelBar With {
            .Width = Unit.Percentage(98), .BorderWidth = Unit.Pixel(0),
            .BorderStyle = BorderStyle.None, .ExpandMode = PanelBarExpandMode.MultipleExpandedItems
        }

        Return LoadStatusTemplateCustomFieldsPanel(Page, CFPanelBar, ObjectID, ObjectType, DisplayDateFormat, ActivatorID, ActivatorType, ApplySecurity, ApplyActivationRules)

    End Function

    Public Function LoadStatusTemplateCustomFieldsPanel(
        ByRef Page As Web.UI.Page,
        ByRef CFPanelBar As RadPanelBar,
        ByVal ObjectID As Long,
        ByVal ObjectType As String,
        ByVal DisplayDateFormat As String,
        ByVal ActivatorID As Long,
        Optional ByVal ActivatorType As String = "",
        Optional ByVal ApplySecurity As Boolean = True,
        Optional ByVal ApplyActivationRules As Boolean = True
    ) As Telerik.Web.UI.RadPanelBar

        'Dim TemplatesTable As DataTable = Get_Templates(False, False, String.Empty, String.Empty, False, ObjectID, ObjectType)
        Dim TemplatesTable As DataTable = GetStatusTemplates(ObjectID, ObjectType, ActivatorID, ActivatorType, ApplySecurity, ApplyActivationRules, String.Empty, String.Empty).Tables(0)
        Dim FieldsTable As DataTable = Nothing

        Dim ds As DataSet = GetStatusTemplateCustomFields(ObjectID, ObjectType, ActivatorID, ActivatorType)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then FieldsTable = ds.Tables(0)

        Return LoadCustomFieldsPanel(CFPanelBar, TemplatesTable, FieldsTable, Page, ObjectID, ObjectType, DisplayDateFormat, ApplySecurity, ApplyActivationRules)

    End Function

    Private Function LoadCustomFieldsPanel(
        ByRef CFPanelBar As RadPanelBar,
        ByVal TemplatesTable As DataTable,
        ByRef FieldsTable As DataTable,
        ByRef Page As Web.UI.Page,
        ByVal ObjectID As Long,
        ByVal ObjectType As String,
        ByVal DisplayDateFormat As String,
        Optional ByVal ApplySecurity As Boolean = True,
        Optional ByVal ApplyActivationRules As Boolean = True,
        Optional ByVal NumberOfVirtualColumns As Long = 3
    ) As Telerik.Web.UI.RadPanelBar

        Dim NumberOfCFColumns As Long = NumberOfVirtualColumns * 2 '(1 cell for label, 1 cell for control)

        Dim LabelWidth As Unit = Unit.Percentage(100 * (1 / NumberOfVirtualColumns) * 0.4) '13 - we reserve 40% of a virtual column to the label
        Dim ControlCellWidth As Unit = Unit.Percentage(100 * (1 / NumberOfVirtualColumns) * 0.6) '20 - we reserve 60% of a virtual column to the label

        Dim ErrorsOccured As Boolean = False
        Dim TemplateFieldsCompletionStatus As String = String.Empty

        EnableClientValidationScripts = My.Computer.FileSystem.FileExists(Web.HttpContext.Current.Server.MapPath(String.Format("~/Clients/{0}/Scripts/{0}.dw.customfields.validation.js", ConnectionName.ToLower)))

        Dim tblCustomFields As New Table

        If FieldsTable IsNot Nothing Then

            Dim tblRow As New TableRow
            Dim TemplateName As String = ""
            Dim PreviousFieldType As String = ""

            Dim CFPanelBarItem As Telerik.Web.UI.RadPanelItem = Nothing

            For Each drTemplate As DataRow In TemplatesTable.Rows

                TemplateName = Catchnull(drTemplate("TemplateName"), "")

                While tblRow.Cells.Count < NumberOfCFColumns AndAlso tblRow.Cells.Count > 0

                    Dim cell As New TableCell()
                    tblRow.Cells.Add(cell)
                    cell.Text = " "

                End While

                'Add the template header
                CFPanelBarItem = New Telerik.Web.UI.RadPanelItem With {
                        .PreventCollapse = False,
                        .Expanded = True,
                        .Value = TemplateName,
                        .Text = TemplateName
                    }
                CFPanelBarItem.Font.Bold = True

                If Catchnull(drTemplate("TemplateType"), "Standard") = "Grid" Then

                    Dim CFPanelBarItem2 As New Telerik.Web.UI.RadPanelItem
                    CFPanelBarItem2.Controls.Add(LoadCustomFieldGrid(ObjectID, ObjectType, drTemplate("TemplateID"), TemplateName, DisplayDateFormat))
                    CFPanelBarItem.Items.Add(CFPanelBarItem2)

                    CFPanelBar.Items.Add(CFPanelBarItem)

                    Continue For

                End If

                tblRow = New TableRow
                tblCustomFields = New Table With {
                    .EnableViewState = True,
                    .CellPadding = 0,
                    .CellSpacing = 1,
                    .Width = Unit.Percentage(100)
                }

                'Set flag indicating whether all fields on template are filled in
                Dim BlankTemplateFields = FieldsTable.Select("(FieldValue1 IS NULL OR FieldValue1 <> '') AND TemplateName = '" & TemplateName & "'")
                Dim AllTemplateFields = FieldsTable.Select("TemplateName = '" & TemplateName & "'")

                If (BlankTemplateFields.Length = 0) Then
                    TemplateFieldsCompletionStatus = "NoneComplete"
                    CFPanelBarItem.ForeColor = Drawing.Color.OrangeRed  'DarkRed, DarkGreen
                ElseIf BlankTemplateFields.Length < AllTemplateFields.Length Then
                    TemplateFieldsCompletionStatus = "PartiallyComplete"
                    CFPanelBarItem.ForeColor = Drawing.Color.MediumBlue  'DarkRed, DarkGreen
                ElseIf BlankTemplateFields.Length = AllTemplateFields.Length Then
                    TemplateFieldsCompletionStatus = "AllComplete"
                    CFPanelBarItem.ForeColor = Drawing.Color.SlateGray 'DarkRed, DarkGreen
                End If

                CFPanelBarItem.Attributes("CompletionStatus") = TemplateFieldsCompletionStatus

                tblRow = New TableRow

                If TemplatesTable IsNot Nothing AndAlso TemplatesTable.Select(String.Format("TemplateName = '{0}' AND [Comments] IS NOT NULL", TemplateName.Replace("'", "''"))).Length > 0 Then

                    Try

                        Dim Comment As String = Catchnull(TemplatesTable.Select(String.Format("TemplateName = '{0}' AND [Comments] IS NOT NULL", TemplateName.Replace("'", "''")))(0)("Comments"), String.Empty)

                        Dim templatecommentcell As New TableCell()
                        tblRow.Cells.Add(templatecommentcell)

                        With templatecommentcell

                            .BackColor = Drawing.Color.WhiteSmoke
                            .ColumnSpan = NumberOfCFColumns
                            '.Font.Bold = True
                            .Style("padding-top") = Unit.Pixel(6).ToString
                            .Style("padding-bottom") = Unit.Pixel(6).ToString
                            .CssClass = "CFComment"

                            Dim lblComment As New Label
                            lblComment.Text = Comment

                            .Controls.Add(lblComment)

                        End With

                        tblCustomFields.Rows.Add(tblRow)

                    Catch ex As Exception

                        Log.Error(ex)

                    End Try

                End If

                tblRow = New TableRow

                PreviousFieldType = ""

                'New row for each template, add a telerik collapse expand with template name, add table for each template, add controls into this table
                Dim dwTemplateRows As New DataView(FieldsTable) With {
                        .RowFilter = String.Format("TemplateName = '{0}'", Catchnull(drTemplate("TemplateName"), "")),
                        .Sort = "DisplayIndex, FieldName"
                    }

                For i As Integer = 0 To dwTemplateRows.Count - 1

                    Dim StartNewLine As Boolean = False
                    Dim drCustomField As DataRowView = dwTemplateRows(i)

                    Try

                        If Catchnull(drCustomField("NewLine"), False) OrElse
                            Catchnull(drCustomField("FieldType"), "").ToString.ToLower = "notes" OrElse
                             Catchnull(drCustomField("FieldType"), "").ToString.ToLower = "documentstable" OrElse
                             Catchnull(drCustomField("FieldType"), "").ToString.ToLower = "documentschecklist" Then

                            Select Case PreviousFieldType

                                Case "contactslookup", "projectslookup", "locationindexlookup", "contactsentitylookup", "contactlist", "contactsentitylist", "customdatalookup"

                                    tblRow.Cells(tblRow.Cells.Count - 1).ColumnSpan = NumberOfCFColumns - tblRow.Cells.Count
                                    StartNewLine = True

                                Case Else

                                    While tblRow.Cells.Count < NumberOfCFColumns

                                        Dim cell As New TableCell()
                                        tblRow.Cells.Add(cell)
                                        cell.Text = " "

                                    End While

                            End Select

                        End If

                        PreviousFieldType = Catchnull(drCustomField("FieldType"), "").ToString.ToLower

                        If tblRow.Cells.Count >= NumberOfCFColumns OrElse StartNewLine Then
                            tblRow = New TableRow
                        End If

                        Dim tblCell As New TableCell

                        If Catchnull(drCustomField("FieldType"), "").ToString.ToLower <> "documentstable" Then

                            Dim lbl As New Label() With {.Text = Catchnull(drCustomField("FieldName"), "")}
                            lbl.Attributes("ReqStatus") = "0"
                            lbl.ToolTip = Catchnull(drCustomField("HelpNotes"), "")
                            tblCell.Controls.Add(lbl)

                            tblCell.Width = LabelWidth ' Unit.Pixel(95)
                            tblCell.VerticalAlign = VerticalAlign.Top
                            tblRow.Cells.Add(tblCell)

                        End If

                        tblCell = New TableCell
                        Dim ctrl As WebControl = Get_Custom_Field_Control_Loaded(
                            Catchnull(drCustomField("EditAllowed"), 1) = 0,
                            Catchnull(drCustomField("ObjectDataID"), 0),
                            Catchnull(drCustomField("CustomFieldID"), 0),
                            ObjectID, ObjectType,
                            Catchnull(drCustomField("FieldName"), ""),
                            Catchnull(drCustomField("FieldType"), ""),
                            False, DisplayDateFormat,
                            Catchnull(drCustomField("LoadOnDemand"), True)
                        )

                        If TypeOf ctrl Is Telerik.Web.UI.RadDatePicker AndAlso Catchnull(drCustomField("FieldType"), "Text").ToString.ToLower.Equals("date") Then

                            SetDateFormat(ctrl, DisplayDateFormat)

                        End If

                        tblCell.VerticalAlign = VerticalAlign.Top
                        tblCell.Controls.Add(ctrl)

                        'myObject = tblCell.Controls(0)
                        'tblCell.Width = ControlCellWidth

                        If Catchnull(drCustomField("Required"), False) Then

                            'Dim rqval As New RequiredFieldValidator
                            'rqval.ControlToValidate = ctrl.ClientID
                            'rqval.Text = "*"
                            'rqval.ErrorMessage = "A value is required for: " & IIf(TemplateName = "", "", TemplateName & " > ") & drCustomField("FieldName")
                            'tblCell.Controls.Add(rqval)

                            ctrl.BackColor = Drawing.Color.LavenderBlush

                            If TypeOf ctrl Is RadDatePicker Then

                                CType(ctrl, RadDatePicker).DateInput.BackColor = Drawing.Color.LavenderBlush

                            End If

                        End If

                        tblRow.Cells.Add(tblCell)

                        'Dim fcnt As Integer = 1
                        If dwTemplateRows.Count.Equals(1) Then
                            tblCell = New TableCell
                            tblCell.Width = LabelWidth
                            tblCell.VerticalAlign = VerticalAlign.Top
                            tblRow.Cells.Add(tblCell)
                            tblCell = New TableCell
                            tblCell.Width = ControlCellWidth
                            tblCell.VerticalAlign = VerticalAlign.Top
                            tblRow.Cells.Add(tblCell)
                        End If

                        tblCustomFields.Rows.Add(tblRow)

                        If Catchnull(drCustomField("FieldType"), "").ToString.ToLower = "notes" Then
                            tblCell.ColumnSpan = NumberOfCFColumns - 1 '(the 1 is used by the label)
                            tblRow = New TableRow
                        ElseIf Catchnull(drCustomField("FieldType"), "").ToString.ToLower = "documentstable" Then
                            tblCell.ColumnSpan = NumberOfCFColumns
                            tblRow = New TableRow
                        Else
                            tblCell.Width = ControlCellWidth
                        End If

                    Catch ex As Exception

                        Log.Error(ex)
                        ErrorsOccured = True

                    End Try

                    If CFPanelBarItem IsNot Nothing Then CFPanelBarItem.Font.Bold = True

                Next 'Field

                If CFPanelBarItem IsNot Nothing Then

                    Dim CFPanelBarItem2 As New Telerik.Web.UI.RadPanelItem
                    CFPanelBarItem2.Controls.Add(tblCustomFields)
                    CFPanelBarItem.Items.Add(CFPanelBarItem2)

                    CFPanelBar.Items.Add(CFPanelBarItem)

                End If

            Next 'Template

        End If

        If ErrorsOccured Then ErrorMessage = "Errors occured loading some custom fields."

        If Not Page.ClientScript.IsClientScriptIncludeRegistered("jQuery") Then _
           Page.ClientScript.RegisterClientScriptInclude("jQuery", WebHelper.UrlResolver.ResolveToFullUrl("~/content/scripts/jquery-1.11.1.min.js"))

        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFContactDetailsRequesting", CustomFieldsManager.GetContactsLookupScript, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFProjectDetailsRequesting", CustomFieldsManager.GetProjectsLookupScript, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFRequestingOnDemandData", CustomFieldsManager.OnCFRequestingOnDemandData, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFLocationRequesting", CustomFieldsManager.GetLocationIndexLookupScript, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFEntityRequesting", CustomFieldsManager.GetEntityLookupScript, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFContactDetailsRequesting", CustomFieldsManager.GetContactsLookupScript, True)

        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "CFValidationScriptP", CustomFieldsManager.GetCFValidationScript(ObjectType, ConnectionName), True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "CFValidationScriptD", CustomFieldsManager.GetCFValidationScript("D", ConnectionName), True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "CFValidationScriptC", CustomFieldsManager.GetCFValidationScript("C", ConnectionName), True)

        If EnableClientValidationScripts AndAlso Not Page.ClientScript.IsClientScriptIncludeRegistered("dw.customfields.validation") Then
            Page.ClientScript.RegisterClientScriptInclude("dw.customfields.validation", Page.ResolveClientUrl(String.Format("~/Clients/{0}/Scripts/{0}.dw.customfields.validation.js", ConnectionName.ToLower)))
            Page.ClientScript.RegisterClientScriptInclude("global.dw.customfields.validation", Page.ResolveClientUrl(String.Format("~/Scripts/dw.customfields.validation.js", ConnectionName.ToLower)))
        End If

        Try

            Dim objDefaultSettings As New UserSettingsManager(ConnectionName, ObjectUserID)
            'Dim CollapseTemplates As Boolean = CBool(objDefaultSettings.GetSetting("GlobalSettings", 0, "CustomFields.ViewSettings.DefaultExpand." & ObjectType, False, True))

            'If CollapseTemplates Then CFPanelBar.CollapseAllItems()

            Dim CFExpand As String = objDefaultSettings.GetSetting("DefaultSettings", ObjectUserID, "CustomFields.ViewSettings.DefaultExpand", "ExpandAll", True)
            If CFExpand.ToLower.Equals("collapseall") Then CFPanelBar.CollapseAllItems()

        Catch ex As Exception

            Log.Error(ex)

        End Try

        Return CFPanelBar

    End Function

    Public Shared Function GetCFValidationScript(ByVal ObjectType As String, ByVal CompanyLogin As String) As String

        Dim script As String = ""
        Dim clientValidationScriptFxnName As String = CompanyLogin.ToLower & "_CF_" & ObjectType & "_Val"
        Dim clientValidationInitialisationScriptFxnName As String = CompanyLogin.ToLower & "_CF_" & ObjectType & "_Initialize_Validation"

        'for radcomboboxes
        script &= "function CF_" & ObjectType & "_Val_Changing(sender, args) { " & vbCrLf
        script &= "     " & String.Format("if(typeof {0}_Changing == 'function') {{ return {0}_Changing(sender, args); }} else {{ args.set_cancel(false); return true; }}", clientValidationScriptFxnName) & vbCrLf
        script &= "} " & vbCrLf
        script &= vbCrLf

        script &= "function CF_" & ObjectType & "_Val_Changed(sender, args) { " & vbCrLf
        script &= "     " & String.Format("if(typeof {0}_Changed == 'function') {{ return {0}_Changed(sender, args); }} else return true;", clientValidationScriptFxnName) & vbCrLf
        script &= "} " & vbCrLf
        script &= vbCrLf

        script &= "$(document).ready(function () { " & vbCrLf
        script &= vbCrLf
        script &= "    //we need to call all the initialisation functions here. " & vbCrLf
        script &= "    " & String.Format("if(typeof {0} == 'function') {{ return {0}(); }}", clientValidationInitialisationScriptFxnName) & vbCrLf
        script &= vbCrLf
        script &= "}); " & vbCrLf

        Return script

    End Function

    Public Shared Function GetLocationIndexLookupScript() As String

        Dim script As String = ""
        script &= "function OnCFLocationIndexRequesting(sender, eventArgs) " & vbCrLf
        script &= "{ " & vbCrLf
        script &= "	if (sender.get_text().length < 3) " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    eventArgs.set_cancel(true); " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "	else " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    var context = eventArgs.get_context(); " & vbCrLf
        script &= "	    context[""FilterString""] = eventArgs.get_text(); " & vbCrLf
        script &= "	    if(sender._attributes._data.TypeFilter) context[""TypeFilter""] = sender._attributes._data.TypeFilter; " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "} " & vbCrLf

        Return script

    End Function

    Public Shared Function GetEntityLookupScript() As String

        Dim script As String = ""
        script &= "function OnCFEntityDetailsRequesting(sender, eventArgs) " & vbCrLf
        script &= "{ " & vbCrLf
        script &= "	if (sender.get_text().length < 3) " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    eventArgs.set_cancel(true); " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "	else " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    var context = eventArgs.get_context(); " & vbCrLf
        script &= "	    context[""FilterString""] = eventArgs.get_text(); " & vbCrLf
        script &= "	    if(sender._attributes._data.TypeFilter) context[""TypeFilter""] = sender._attributes._data.TypeFilter; " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "} " & vbCrLf

        Return script

    End Function

    Public Shared Function OnCFRequestingOnDemandData() As String

        Dim script As String = ""

        script &= "function OnCFRequestingOnDemandData(sender, eventArgs) " & vbCrLf
        script &= "{ " & vbCrLf
        script &= "	if (sender.get_items().get_count() > 1) " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    eventArgs.set_cancel(true); " & vbCrLf
        script &= "	} else { " & vbCrLf
        script &= "	    var context = eventArgs.get_context(); " & vbCrLf
        script &= "	    context[""FieldName""] = sender._attributes._data.FieldName; " & vbCrLf
        script &= "	    context[""FieldType""] = sender._attributes._data.FieldType; " & vbCrLf
        script &= "	    context[""ObjectType""] = sender._attributes._data.CF_OBJ_TYPE; " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "} " & vbCrLf

        Return script

    End Function

    Public Shared Function GetProjectsLookupScript() As String

        Dim script As String = ""
        script &= "function OnCFProjectDetailsRequesting(sender, eventArgs) " & vbCrLf
        script &= "{ " & vbCrLf
        script &= "	if (sender.get_text().length < 3) " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    eventArgs.set_cancel(true); " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "	else " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    var context = eventArgs.get_context(); " & vbCrLf
        script &= "	    context[""FilterString""] = eventArgs.get_text(); " & vbCrLf
        script &= "	    if(sender._attributes._data.TypeFilter) context[""TypeFilter""] = sender._attributes._data.TypeFilter; " & vbCrLf
        script &= "	    if(sender._attributes._data.StatusFilter) context[""StatusFilter""] = sender._attributes._data.StatusFilter; " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "} " & vbCrLf
        script &= vbCrLf

        Return script

    End Function

    Public Shared Function GetContactsLookupScript() As String

        Dim script As String = ""
        script &= "function OnCFContactDetailsRequesting(sender, eventArgs) " & vbCrLf
        script &= "{ " & vbCrLf
        script &= "	if (sender.get_text().length < 3) " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    eventArgs.set_cancel(true); " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "	else " & vbCrLf
        script &= "	{ " & vbCrLf
        script &= "	    var context = eventArgs.get_context(); " & vbCrLf
        script &= "	    context[""FilterString""] = eventArgs.get_text(); " & vbCrLf
        script &= "	    if(sender._attributes._data.TypeFilter) context[""TypeFilter""] = sender._attributes._data.TypeFilter; " & vbCrLf
        script &= "	} " & vbCrLf
        script &= "} " & vbCrLf
        script &= vbCrLf
        script &= "function OnCFClientBlurHandler(sender, eventArgs) { " & vbCrLf
        script &= "    var textInTheCombo = sender.get_text(); " & vbCrLf
        script &= "    var item = sender.findItemByText(textInTheCombo); " & vbCrLf
        script &= "    //if there is no item with that text " & vbCrLf
        script &= "    if (!item) { " & vbCrLf
        script &= "        sender.set_text(""""); " & vbCrLf
        script &= "        //setTimeout(function () { " & vbCrLf
        script &= "            //var inputElement = sender.get_inputDomElement(); " & vbCrLf
        script &= "            //inputElement.focus(); " & vbCrLf
        script &= "	       //}, 20); " & vbCrLf
        script &= "    } " & vbCrLf
        script &= "    return true; " & vbCrLf
        script &= "} " & vbCrLf

        Return script

    End Function

    Public Function Get_Custom_Field_Control(
        ByVal ObjectID As Long,
        ByVal ReadOnly_Template As Boolean,
        ByVal ObjectDataID As Long,
        ByVal CustomFieldID As Long,
        ByVal ObjectType As String,
        ByVal FieldName As String,
        ByVal FieldType As String,
        ByVal ForSearch As Boolean,
        ByVal DisplayDateFormat As String,
        ByVal LoadOnDemand As Boolean
    ) As System.Web.UI.Control

        Static control_id As Long = 0 : control_id += 1

        If FieldType.ToLower = "date" Then

            If Not ForSearch Then

                Dim rdp As New Telerik.Web.UI.RadDatePicker

                rdp.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
                rdp.MinDate = New Date(1600, 1, 1)
                rdp.MaxDate = New Date(2200, 12, 31)
                rdp.Attributes("FieldName") = FieldName
                rdp.Attributes("FieldType") = FieldType
                rdp.Attributes("ObjectDataID") = ObjectDataID
                rdp.Attributes("CF_OBJ_ID") = ObjectID
                rdp.Attributes("CF_OBJ_TYPE") = ObjectType

                If Not ForSearch Then
                    With rdp
                        .Enabled = Not ReadOnly_Template
                        .Attributes("ROTemplate") = ReadOnly_Template
                    End With
                End If

                SetDateFormat(rdp, DisplayDateFormat)

                Return rdp

            Else

                Dim rdpStart As New Telerik.Web.UI.RadDatePicker

                With rdpStart

                    .ID = "txtCustom" & FieldName.Replace(" ", "_") & "_Start_" & control_id
                    .MinDate = New Date(1600, 1, 1)
                    .MaxDate = New Date(2200, 12, 31)
                    .Attributes("FieldName") = FieldName
                    .Attributes("FieldType") = FieldType
                    .Attributes("ObjectDataID") = ObjectDataID
                    .Attributes("CF_OBJ_ID") = ObjectID
                    .Attributes("CF_OBJ_TYPE") = ObjectType
                    .Attributes("Range") = "Start"
                    .Width = New System.Web.UI.WebControls.Unit(120, Web.UI.WebControls.UnitType.Pixel)

                End With

                Dim rdpEnd As New Telerik.Web.UI.RadDatePicker

                With rdpEnd

                    .ID = "txtCustom" & FieldName.Replace(" ", "_") & "_End_" & control_id
                    .MinDate = New Date(1600, 1, 1)
                    .MaxDate = New Date(2200, 12, 31)
                    .Attributes("FieldName") = FieldName
                    .Attributes("FieldType") = FieldType
                    .Attributes("CF_OBJ_ID") = ObjectID
                    .Attributes("CF_OBJ_TYPE") = ObjectType
                    .Attributes("ObjectDataID") = ObjectDataID
                    .Attributes("Range") = "End"
                    .Width = New System.Web.UI.WebControls.Unit(120, Web.UI.WebControls.UnitType.Pixel)

                End With

                Dim dateSearch As New System.Web.UI.WebControls.Table
                With dateSearch
                    .ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
                    .Attributes("FieldName") = FieldName
                    .Attributes("FieldType") = FieldType
                    .Attributes("CF_OBJ_ID") = ObjectID
                    .Attributes("CF_OBJ_TYPE") = ObjectType
                    .Attributes("ObjectDataID") = ObjectDataID
                End With

                SetDateFormat(rdpStart, DisplayDateFormat)
                SetDateFormat(rdpEnd, DisplayDateFormat)

                Dim tr As New System.Web.UI.WebControls.TableRow

                Dim td As New System.Web.UI.WebControls.TableCell
                td.Controls.Add(rdpStart)
                tr.Cells.Add(td)

                td = New System.Web.UI.WebControls.TableCell
                td.Text = "to"
                tr.Cells.Add(td)

                td = New System.Web.UI.WebControls.TableCell
                td.Controls.Add(rdpEnd)
                tr.Cells.Add(td)

                dateSearch.Rows.Add(tr)

                Return dateSearch

            End If

        ElseIf FieldType.ToLower = "datetime" Then

            Dim rdtp As New Telerik.Web.UI.RadDateTimePicker

            rdtp.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
            rdtp.MinDate = New Date(1600, 1, 1)
            rdtp.MaxDate = New Date(2200, 12, 31)
            rdtp.DateInput.DateFormat = "dd/MM/yyyy HH:mm"
            rdtp.DateInput.DisplayDateFormat = rdtp.DateInput.DateFormat
            rdtp.TimeView.Interval = New TimeSpan(0, 30, 0)
            rdtp.TimeView.HeaderText = ""
            rdtp.TimeView.Columns = 4
            rdtp.Attributes("FieldName") = FieldName
            rdtp.Attributes("FieldType") = FieldType
            rdtp.Attributes("CF_OBJ_ID") = ObjectID
            rdtp.Attributes("CF_OBJ_TYPE") = ObjectType
            rdtp.Attributes("ObjectDataID") = ObjectDataID

            If Not ForSearch Then
                With rdtp
                    .Enabled = Not ReadOnly_Template
                    .Attributes("ROTemplate") = ReadOnly_Template
                End With
            End If

            Return rdtp

        ElseIf FieldType.ToLower = "ContactsAction" Then

            Dim cbo As RadComboBox = Get_Custom_Field_Control(ObjectID, False, ObjectDataID, CustomFieldID, ObjectType, FieldName, "contactslookup", ForSearch, DisplayDateFormat, LoadOnDemand)
            Dim cmd As New Button With {
                .ID = "cmdCustom" & FieldName.Replace(" ", "_") & "_" & control_id,
                .Text = "GO"
            }
            cmd.Attributes("cfActionID") = ObjectID
            cmd.Attributes("cfActionCombo") = cbo.ClientID
            cmd.Attributes("cfActionFieldName") = FieldName

            AddHandler cmd.Click, Sub(s, ev)

                                      Dim b As Button = s
                                      Dim c As RadComboBox = TryCast(b.Parent.Controls(0), Telerik.Web.UI.RadComboBox)
                                      Dim a As System.Web.UI.AttributeCollection = b.Attributes

                                      RaiseEvent CustomAction(a("cfActionID"), a("cfActionFieldName"), c)

                                  End Sub

            Dim pnl As New Panel
            pnl.Controls.Add(cbo)
            pnl.Controls.Add(cmd)
            Return pnl

        ElseIf FieldType.ToLower = "documentstable" Then

            'This gets a list of documents linked to this item
            'Settings:
            ' StaticColumns
            ' CustomFieldColumns
            ' DocTypeFilter
            ' LinkType: Defines the filter criteria for the documents

            Dim sql As String = String.Empty
            sql &= "WITH Settings AS ( " & vbCrLf
            sql &= "	SELECT FieldName + ':' + Settings.[Name] AS SettingName " & vbCrLf
            sql &= "	FROM CustomField_Fields, (	 " & vbCrLf
            sql &= "		SELECT 'StaticColumns' As [Name] " & vbCrLf
            sql &= "		UNION SELECT 'CustomFieldColumns' " & vbCrLf
            sql &= "        UNION SELECT 'ReadOnlyColumns' " & vbCrLf
            sql &= "		UNION SELECT 'DocTypeFilter' " & vbCrLf
            sql &= "		UNION SELECT 'LinkType'  " & vbCrLf
            sql &= "	) AS Settings " & vbCrLf
            sql &= "	WHERE FieldType = 'DocumentsTable' AND FieldName = '" & FieldName & "' " & vbCrLf
            sql &= ") " & vbCrLf
            sql &= "SELECT V.DataLookupID, S.SettingName, V.FieldValue1  " & vbCrLf
            sql &= "FROM Settings S  " & vbCrLf
            sql &= "	LEFT JOIN CustomField_DataLookup V ON REPLACE(V.FieldName, '#CF_DOC_TBL:', '') = S.SettingName " & vbCrLf

            Dim ds As DataSet = DataHelper.ExecuteDataset(db, sql)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then

                Dim tbl As New Web.UI.WebControls.Table
                tbl.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
                tbl.Attributes("FieldName") = FieldName
                tbl.Attributes("FieldType") = FieldType
                tbl.Attributes("CF_OBJ_ID") = ObjectID
                tbl.Attributes("CF_OBJ_TYPE") = ObjectType
                tbl.Attributes("ObjectDataID") = ObjectDataID

                Dim t As DataTable = ds.Tables(0)
                Dim cols As DataColumnCollection = t.Columns
                Dim dr() As DataRow

                Dim StaticColumns As List(Of String) = New List(Of String)
                dr = t.Select("SettingName='" & FieldName.Trim & ":StaticColumns" & "' AND (FieldValue1 IS NOT NULL OR FieldValue1 <> '')")
                If dr.Length > 0 AndAlso Not String.IsNullOrEmpty(CStr(Catchnull(dr(0)("FieldValue1"), "", True))) Then StaticColumns.AddRange(CStr(Catchnull(dr(0)("FieldValue1"), "", True)).Split("|"))

                Dim CustomFieldColumns As List(Of String) = New List(Of String)
                dr = t.Select("SettingName='" & FieldName.Trim & ":CustomFieldColumns" & "' AND (FieldValue1 IS NOT NULL OR FieldValue1 <> '')")
                If dr.Length > 0 AndAlso Not String.IsNullOrEmpty(CStr(Catchnull(dr(0)("FieldValue1"), "", True))) Then CustomFieldColumns.AddRange(CStr(Catchnull(dr(0)("FieldValue1"), "", True)).Split("|"))

                Dim DocTypeFilter As List(Of String) = New List(Of String)
                dr = t.Select("SettingName='" & FieldName.Trim & ":DocTypeFilter" & "' AND (FieldValue1 IS NOT NULL OR FieldValue1 <> '')")
                If dr.Length > 0 AndAlso Not String.IsNullOrEmpty(CStr(Catchnull(dr(0)("FieldValue1"), "", True))) Then DocTypeFilter.AddRange(CStr(Catchnull(dr(0)("FieldValue1"), "", True)).Split("|"))

                Dim ReadOnlyColumns As List(Of String) = New List(Of String)
                dr = t.Select("SettingName='" & FieldName.Trim & ":ReadOnlyColumns" & "' AND (FieldValue1 IS NOT NULL OR FieldValue1 <> '')")
                If dr.Length > 0 AndAlso Not String.IsNullOrEmpty(CStr(Catchnull(dr(0)("FieldValue1"), "", True))) Then ReadOnlyColumns.AddRange(CStr(Catchnull(dr(0)("FieldValue1"), "", True)).Split("|"))

                Dim LinkType As String = "P"

                sql = "SELECT D.ID AS ID, " & vbCrLf

                sql &= IIf(StaticColumns.Count > 0, Join(StaticColumns.ToArray, ",") & vbCrLf, String.Empty)
                sql &= IIf(CustomFieldColumns.Count > 0, IIf(StaticColumns.Count > 0, ", ", String.Empty) & Join(CustomFieldColumns.ToArray, ","), String.Empty) & vbCrLf
                sql &= "FROM vwDocumentCustomFields D " & vbCrLf

                Select Case LinkType
                    Case "P"
                        sql &= "    INNER JOIN vwProjectCustomFields P ON P.ObjectID = D.WField2_ID "
                End Select

                If DocTypeFilter.Count > 0 Then sql &= "WHERE D.Doc_Type IN ('" & Join(DocTypeFilter.ToArray, "','") & "') AND P.ObjectID = " & ObjectID

                sql &= vbCrLf & vbCrLf & vbCrLf

                sql &= "SELECT D.ID, PT.LoadOnDemand, PF.* " & vbCrLf
                sql &= "FROM CustomField_ObjectData PF " & vbCrLf
                sql &= "	INNER JOIN Documents D ON D.ID = PF.ObjectID " & vbCrLf
                sql &= "	INNER JOIN CustomField_Fields PT ON PT.CustomFieldID = PF.CustomFieldID " & vbCrLf
                sql &= "WHERE PF.ObjectType = 'D'  " & vbCrLf
                sql &= "	AND D.Doc_Type IN ('" & Join(DocTypeFilter.ToArray, "','") & "') AND D.WField2_ID = " & ObjectID & vbCrLf
                sql &= "	AND PF.FieldName IN ('" & Join(CustomFieldColumns.ToArray, "','").Replace("D.[", "").Replace("[", "").Replace("]", "") & "') " & vbCrLf

                Dim dsCF As DataSet = DataHelper.ExecuteDataset(db, sql)

                If dsCF IsNot Nothing AndAlso dsCF.Tables.Count > 1 Then

                    Dim r As New Web.UI.WebControls.TableRow

                    Dim c As New TableCell()
                    c.ColumnSpan = dsCF.Tables(0).Columns.Count - 1
                    c.Text = FieldName
                    c.BackColor = Drawing.Color.LightGray
                    r.Cells.Add(c)
                    tbl.Rows.Add(r)

                    r = New Web.UI.WebControls.TableRow
                    For Each col As DataColumn In dsCF.Tables(0).Columns

                        If col.ColumnName = "ID" Then Continue For

                        c = New TableCell()
                        c.Text = col.ColumnName
                        r.Cells.Add(c)

                    Next
                    tbl.Rows.Add(r)

                    Dim dt_control_id As Long = 0
                    For Each fld As DataRow In dsCF.Tables(0).Rows

                        r = New Web.UI.WebControls.TableRow

                        If dsCF.Tables(0).Rows.Count \ 2 = 1 Then
                            r.BackColor = Drawing.Color.WhiteSmoke
                        End If

                        For Each col As DataColumn In dsCF.Tables(0).Columns

                            If col.ColumnName = "ID" Then Continue For

                            dt_control_id += 1
                            c = New TableCell()

                            If ReadOnlyColumns.Contains(col.ColumnName) OrElse
                                 ReadOnlyColumns.Contains("[" & col.ColumnName & "]") OrElse
                                 ReadOnlyColumns.Contains("D.[" & col.ColumnName & "]") Then

                                Dim lbl As New System.Web.UI.WebControls.Label
                                lbl.Text = Catchnull(fld(col), "")
                                c.Controls.Add(lbl)

                            ElseIf StaticColumns.Contains(col.ColumnName) OrElse
                                StaticColumns.Contains("[" & col.ColumnName & "]") OrElse
                                StaticColumns.Contains("D.[" & col.ColumnName & "]") Then

                                Dim txt As New Telerik.Web.UI.RadTextBox

                                txt.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id & "_" & col.ColumnName.Replace(" ", "_") & dt_control_id
                                txt.Attributes("FieldName") = col.ColumnName
                                txt.Attributes("FieldType") = FieldType
                                txt.TextMode = Web.UI.WebControls.TextBoxMode.SingleLine
                                txt.Rows = 1
                                txt.Width = System.Web.UI.WebControls.Unit.Percentage(90)
                                txt.Attributes("ObjectDataID") = ObjectDataID
                                txt.InputType = Telerik.Web.UI.Html5InputType.Text
                                txt.MaxLength = 500
                                txt.Skin = "Metro"

                                txt.Text = Catchnull(fld(col), "")

                                c.Controls.Add(txt)

                            ElseIf CustomFieldColumns.Contains(col.ColumnName) OrElse
                                CustomFieldColumns.Contains("[" & col.ColumnName & "]") OrElse
                                CustomFieldColumns.Contains("D.[" & col.ColumnName & "]") Then

                                Dim lbl As New System.Web.UI.WebControls.Label
                                lbl.Text = Catchnull(fld(col.ColumnName), "")
                                c.Controls.Add(lbl)

                                Dim CFields() As DataRow = dsCF.Tables(1).Select(String.Format("ObjectID = {0} AND FieldName = '{1}'", fld("ID"), col.ColumnName))

                                If CFields.Length > 0 Then

                                    Dim cf As DataRow = CFields(0)

                                    Dim ctrl As Object = Get_Custom_Field_Control_Loaded(
                                        ReadOnly_Template,
                                        cf("ObjectDataID"),
                                        cf("CustomFieldID"),
                                        cf("ObjectID"),
                                        "D",
                                        col.ColumnName,
                                        cf("FieldType"),
                                        ForSearch,
                                        DisplayDateFormat,
                                        Catchnull(cf("LoadOnDemand"), True)
                                    )
                                    If TypeOf ctrl Is Telerik.Web.ISkinnableControl Then
                                        CType(ctrl, Telerik.Web.ISkinnableControl).Skin = "Metro"
                                    End If

                                    c.Controls.Add(ctrl)

                                End If

                            End If

                            r.Cells.Add(c)

                        Next

                        tbl.Rows.Add(r)

                    Next

                End If

                Return tbl

                Select Case LinkType
                    Case "T", "Transmittal"
                    Case "P", "Project"
                        sql &= "WField2_ID = " & ObjectID
                    Case "C", "Contact"
                End Select

            End If

        ElseIf FieldType.ToLower = "time" Then

            Dim rtp As New Telerik.Web.UI.RadTimePicker

            rtp.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
            rtp.DateInput.DateFormat = "HH:mm"
            rtp.DateInput.DisplayDateFormat = "HH:mm"
            rtp.TimeView.Interval = New TimeSpan(0, 30, 0)
            rtp.TimeView.HeaderText = ""
            rtp.TimeView.Columns = 4
            rtp.Attributes("FieldName") = FieldName
            rtp.Attributes("FieldType") = FieldType
            rtp.Attributes("CF_OBJ_ID") = ObjectID
            rtp.Attributes("CF_OBJ_TYPE") = ObjectType
            rtp.Attributes("ObjectDataID") = ObjectDataID

            If Not ForSearch Then
                With rtp
                    .Enabled = Not ReadOnly_Template
                    .Attributes("ROTemplate") = ReadOnly_Template
                End With
            End If

            Return rtp

        ElseIf FieldType.ToLower = "multivalue" Then

            Dim tblValues As DataTable = Get_Custom_Field_Values(ObjectType, FieldName, FieldType)

            Dim returnRadListBox As New Telerik.Web.UI.RadListBox
            returnRadListBox.ID = "lstCustom" & FieldName.Replace(" ", "") & "_" & control_id
            returnRadListBox.Attributes("FieldName") = FieldName
            returnRadListBox.Attributes("FieldType") = FieldType
            returnRadListBox.Attributes("CF_OBJ_ID") = ObjectID
            returnRadListBox.Attributes("CF_OBJ_TYPE") = ObjectType
            returnRadListBox.Attributes("ObjectDataID") = ObjectDataID
            returnRadListBox.SelectionMode = Telerik.Web.UI.ListBoxSelectionMode.Multiple
            returnRadListBox.Items.Clear()
            returnRadListBox.Height = 80
            returnRadListBox.Width = System.Web.UI.WebControls.Unit.Percentage(80)

            If Not ForSearch Then
                With returnRadListBox
                    .Enabled = Not ReadOnly_Template
                    .Attributes("ROTemplate") = ReadOnly_Template
                End With
            End If

            Dim myItem As New Telerik.Web.UI.RadListBoxItem

            For i As Integer = 0 To tblValues.Rows.Count - 1

                myItem = New Telerik.Web.UI.RadListBoxItem
                myItem.Text = Catchnull(tblValues.Rows(i).Item("FieldValue1"), "")
                myItem.Value = Catchnull(tblValues.Rows(i).Item("FieldValue1"), "")
                returnRadListBox.Items.Add(myItem)

            Next

            returnRadListBox.Items.Add(New Telerik.Web.UI.RadListBoxItem("", ""))

            Return returnRadListBox

        ElseIf FieldType.ToLower = "duplicate" Then

            Dim lbl As New System.Web.UI.WebControls.Label

            lbl.ID = "lblCustom" & FieldName.Replace(" ", "_") & "_" & control_id
            lbl.Attributes("FieldName") = FieldName
            lbl.Attributes("FieldType") = FieldType
            lbl.Attributes("CF_OBJ_ID") = ObjectID
            lbl.Attributes("CF_OBJ_TYPE") = ObjectType
            lbl.Attributes("ObjectDataID") = ObjectDataID

            Return lbl

        ElseIf FieldType.ToLower = "notes" Then

            Dim txt As New Telerik.Web.UI.RadTextBox

            txt.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
            txt.Attributes("FieldName") = FieldName
            txt.Attributes("FieldType") = FieldType
            txt.TextMode = Web.UI.WebControls.TextBoxMode.MultiLine
            txt.Rows = 2
            txt.Width = System.Web.UI.WebControls.Unit.Percentage(90)
            txt.Attributes("CF_OBJ_ID") = ObjectID
            txt.Attributes("CF_OBJ_TYPE") = ObjectType
            txt.Attributes("ObjectDataID") = ObjectDataID
            txt.InputType = Telerik.Web.UI.Html5InputType.Text
            txt.MaxLength = MAX_FIELD_LENGTH_NOTES

            Try

                'It was agreed to lock down all notes fields for AHP on 01/Nov/2013
                If ConnectionName.ToLower.Equals("ahp") And Not ForSearch Then
                    txt.ReadOnly = True
                End If

            Catch ex As Exception

            End Try

            If Not ForSearch Then
                With txt
                    .Enabled = Not ReadOnly_Template
                    .Attributes("ROTemplate") = ReadOnly_Template
                End With
            End If

            Return txt

        ElseIf FieldType.ToLower = "rating" Then

            Dim rr As New Telerik.Web.UI.RadRating

            rr.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
            rr.Attributes("FieldName") = FieldName
            rr.Attributes("FieldType") = FieldType
            rr.Width = System.Web.UI.WebControls.Unit.Percentage(90)
            rr.Attributes("CF_OBJ_ID") = ObjectID
            rr.Attributes("CF_OBJ_TYPE") = ObjectType
            rr.Attributes("ObjectDataID") = ObjectDataID

            If Not ForSearch Then
                With rr
                    .Enabled = Not ReadOnly_Template
                    .Attributes("ROTemplate") = ReadOnly_Template
                End With
            End If

            Return rr

        ElseIf FieldType.ToLower = "contactslookup" OrElse
            FieldType.ToLower = "projectslookup" OrElse
            FieldType.ToLower = "locationindexlookup" OrElse
            FieldType.ToLower = "contactsentitylookup" OrElse
            FieldType.ToLower = "customdatalookup" Then

            Dim cbo As New Telerik.Web.UI.RadComboBox
            Dim tblValues As DataTable = Get_Custom_Field_Values(ObjectType, FieldName, FieldType, ForSearch)

            cbo.ID = "cboCustom" & FieldName.Replace(" ", "") & "_" & control_id
            cbo.Attributes("FieldName") = FieldName
            cbo.Attributes("FieldType") = FieldType
            cbo.Attributes("CF_OBJ_ID") = ObjectID
            cbo.Attributes("CF_OBJ_TYPE") = ObjectType
            cbo.Attributes("ObjectDataID") = ObjectDataID
            cbo.Items.Clear()
            cbo.Width = System.Web.UI.WebControls.Unit.Percentage(90)
            cbo.EnableLoadOnDemand = True
            cbo.AllowCustomText = False
            cbo.ChangeTextOnKeyBoardNavigation = False

            If Not ForSearch Then
                With cbo
                    .Enabled = Not ReadOnly_Template
                    .Attributes("ROTemplate") = ReadOnly_Template
                End With
            End If

            If tblValues IsNot Nothing Then

                Dim values As String = String.Empty

                'Default to types filter
                Dim vw As New DataView(tblValues)
                vw.RowFilter = String.Format("FieldName = '{0}' OR FieldName = '{0}::Type'", FieldName.Replace("'", "''"))
                For i As Integer = 0 To vw.Count - 1
                    values &= IIf(values = String.Empty, "", "|") & Catchnull(vw(i)("FieldValue1"), "")
                Next
                cbo.Attributes("TypeFilter") = values

                values = String.Empty
                vw.RowFilter = String.Format("FieldName = '{0}::Status'", FieldName.Replace("'", "''"))
                For i As Integer = 0 To vw.Count - 1
                    values &= IIf(values = String.Empty, "", "|") & Catchnull(vw(i)("FieldValue1"), "")
                Next
                cbo.Attributes("StatusFilter") = values

            End If

            cbo.WebServiceSettings.Path = "~/CustomFields/DataService.asmx"

            If FieldType.ToLower = "contactslookup" Then

                cbo.OnClientItemsRequesting = "OnCFContactDetailsRequesting"
                cbo.WebServiceSettings.Method = "GetContacts"

            ElseIf FieldType.ToLower = "projectslookup" Then

                cbo.OnClientItemsRequesting = "OnCFProjectDetailsRequesting"
                cbo.WebServiceSettings.Method = "GetProjects"

            ElseIf FieldType.ToLower = "locationindexlookup" Then

                cbo.OnClientItemsRequesting = "OnCFLocationIndexRequesting"
                cbo.WebServiceSettings.Method = "GetLocationIndex"

            ElseIf FieldType.ToLower = "customdatalookup" Then

                cbo.OnClientItemsRequesting = "OnCFCustomDataRequesting"
                cbo.WebServiceSettings.Method = "GetCustomData"

            ElseIf FieldType.ToLower = "contactsentitylookup" Then

                cbo.OnClientItemsRequesting = "OnCFEntityDetailsRequesting"
                cbo.WebServiceSettings.Method = "GetContactsEntity"

            End If

            cbo.OnClientBlur = "OnCFClientBlurHandler" 'to prevent items not from the Docwize lists

            Return cbo

        ElseIf FieldType.ToLower = "money" Or FieldType.ToLower = "numeric" Then

            If Not ForSearch Then

                Dim txt As New Telerik.Web.UI.RadNumericTextBox

                txt.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
                txt.Attributes("FieldName") = FieldName
                txt.Attributes("FieldType") = FieldType
                txt.Attributes("CF_OBJ_ID") = ObjectID
                txt.Attributes("CF_OBJ_TYPE") = ObjectType
                txt.Attributes("ObjectDataID") = ObjectDataID
                txt.Width = System.Web.UI.WebControls.Unit.Percentage(80)
                Select Case FieldType.ToLower
                    Case "money"
                        txt.Type = Telerik.Web.UI.NumericType.Currency
                    Case "numeric"
                        txt.Type = Telerik.Web.UI.NumericType.Number
                End Select
                txt.ShowSpinButtons = True
                txt.IncrementSettings.InterceptArrowKeys = True
                txt.EnabledStyle.HorizontalAlign = Web.UI.WebControls.HorizontalAlign.Right

                Dim cs As New CustomFieldSettingValue(Get_Custom_Field_Values(ObjectType, FieldName, FieldType))
                If cs.GetNumericSetting(FieldName & ".MaxValue") Then
                    If txt.Value.HasValue Then txt.MaxValue = Math.Max(cs.NumericValue, txt.Value.Value)
                    txt.MaxValue = cs.NumericValue
                End If
                If cs.GetNumericSetting(FieldName & ".MinValue") Then
                    If txt.Value.HasValue Then txt.MinValue = Math.Min(cs.NumericValue, txt.Value.Value)
                    txt.MinValue = cs.NumericValue
                End If

                With txt.NumberFormat
                    .DecimalSeparator = "."
                    .GroupSeparator = ","
                    .GroupSizes = 3
                    .KeepTrailingZerosOnFocus = True
                End With

                txt.Enabled = Not ReadOnly_Template
                txt.Attributes("ROTemplate") = ReadOnly_Template

                Return txt

            Else

                Dim txtStart As New Telerik.Web.UI.RadNumericTextBox

                With txtStart

                    .ID = "txtCustom" & FieldName.Replace(" ", "_") & "_Start_" & control_id
                    .Attributes("FieldName") = FieldName
                    .Attributes("FieldType") = FieldType
                    .Attributes("CF_OBJ_ID") = ObjectID
                    .Attributes("CF_OBJ_TYPE") = ObjectType
                    .Attributes("ObjectDataID") = ObjectDataID
                    .Attributes("Range") = "Start"
                    .Width = New System.Web.UI.WebControls.Unit(120, Web.UI.WebControls.UnitType.Pixel)
                    Select Case FieldType.ToLower
                        Case "money"
                            .Type = Telerik.Web.UI.NumericType.Currency
                        Case "numeric"
                            .Type = Telerik.Web.UI.NumericType.Number
                    End Select
                    .ShowSpinButtons = True
                    .IncrementSettings.InterceptArrowKeys = True
                    .EnabledStyle.HorizontalAlign = Web.UI.WebControls.HorizontalAlign.Right

                    With .NumberFormat
                        .DecimalSeparator = "."
                        .GroupSeparator = ","
                        .GroupSizes = 3
                        .KeepTrailingZerosOnFocus = True
                    End With

                End With

                Dim txtEnd As New Telerik.Web.UI.RadNumericTextBox

                With txtEnd

                    .ID = "txtCustom" & FieldName.Replace(" ", "_") & "_End_" & control_id
                    .Attributes("FieldName") = FieldName
                    .Attributes("FieldType") = FieldType
                    .Attributes("CF_OBJ_ID") = ObjectID
                    .Attributes("CF_OBJ_TYPE") = ObjectType
                    .Attributes("ObjectDataID") = ObjectDataID
                    .Attributes("Range") = "End"
                    .Width = New System.Web.UI.WebControls.Unit(120, Web.UI.WebControls.UnitType.Pixel)
                    Select Case FieldType.ToLower
                        Case "money"
                            .Type = Telerik.Web.UI.NumericType.Currency
                        Case "numeric"
                            .Type = Telerik.Web.UI.NumericType.Number
                    End Select
                    .ShowSpinButtons = True
                    .IncrementSettings.InterceptArrowKeys = True
                    .EnabledStyle.HorizontalAlign = Web.UI.WebControls.HorizontalAlign.Right

                    With .NumberFormat
                        .DecimalSeparator = "."
                        .GroupSeparator = ","
                        .GroupSizes = 3
                        .KeepTrailingZerosOnFocus = True
                    End With

                End With

                Dim numericSearch As New System.Web.UI.WebControls.Table
                With numericSearch
                    .ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
                    .Attributes("FieldName") = FieldName
                    .Attributes("FieldType") = FieldType
                    .Attributes("CF_OBJ_ID") = ObjectID
                    .Attributes("CF_OBJ_TYPE") = ObjectType
                    .Attributes("ObjectDataID") = ObjectDataID
                End With

                Dim tr As New System.Web.UI.WebControls.TableRow

                Dim td As New System.Web.UI.WebControls.TableCell
                td.Controls.Add(txtStart)
                tr.Cells.Add(td)

                td = New System.Web.UI.WebControls.TableCell
                td.Text = "to"
                tr.Cells.Add(td)

                td = New System.Web.UI.WebControls.TableCell
                td.Controls.Add(txtEnd)
                tr.Cells.Add(td)

                numericSearch.Rows.Add(tr)

                Return numericSearch

            End If

        Else

            Dim tblValues As DataTable = Get_Custom_Field_Values(ObjectType, FieldName, FieldType)

            If tblValues IsNot Nothing AndAlso (tblValues.Rows.Count > 0 OrElse FieldType.ToLower = "checkbox" OrElse FieldType.ToLower = "currency") Then

                Dim cbo As New Telerik.Web.UI.RadComboBox
                cbo.AllowCustomText = ForSearch OrElse ( _
                    FieldType.ToLower <> "currency" AndAlso _
                    FieldType.ToLower <> "combo" AndAlso _
                    FieldType.ToLower <> "userlookup" AndAlso _
                    FieldType.ToLower <> "country" AndAlso _
                    FieldType.ToLower <> "contactsentitylist" AndAlso _
                    FieldType.ToLower <> "contactlist" AndAlso _
                    FieldType.ToLower <> "checkbox" _
                )
                cbo.ID = "cboCustom" & FieldName.Replace(" ", "") & "_" & control_id
                cbo.Attributes("FieldName") = FieldName
                cbo.Attributes("FieldType") = FieldType
                cbo.Attributes("CF_OBJ_ID") = ObjectID
                cbo.Attributes("CF_OBJ_TYPE") = ObjectType
                cbo.Attributes("ObjectDataID") = ObjectDataID
                cbo.Items.Clear()
                cbo.Width = System.Web.UI.WebControls.Unit.Percentage(80)
                cbo.ChangeTextOnKeyBoardNavigation = False
                cbo.MaxHeight = 300

                'Setup for LoadOnDemand
                cbo.EnableAutomaticLoadOnDemand = LoadOnDemand 'Not cbo.AllowCustomText

                If LoadOnDemand Then

                    cbo.EnableLoadOnDemand = True
                    cbo.EnableItemCaching = True

                    cbo.OnClientItemsRequesting = "OnCFRequestingOnDemandData"
                    cbo.OnClientBlur = "OnCFClientBlurHandler" 'to prevent items not from the Docwize lists

                    With cbo.WebServiceSettings
                        .Method = "GetCustomFieldDataItems"
                        .Path = "~/CustomFields/DataService.asmx"
                    End With

                    If Not ForSearch Then
                        With cbo
                            .Enabled = Not ReadOnly_Template
                            .Attributes("ROTemplate") = ReadOnly_Template
                        End With
                    End If

                Else

                    Dim myItem As New Telerik.Web.UI.RadComboBoxItem

                    For i As Integer = 0 To tblValues.Rows.Count - 1

                        myItem = New Telerik.Web.UI.RadComboBoxItem

                        If FieldType = "userlookup" Then
                            myItem.Text = Catchnull(tblValues.Rows(i).Item("Username"), "")
                        Else
                            myItem.Text = Catchnull(tblValues.Rows(i).Item("FieldValue1"), "")
                        End If

                        myItem.Value = Catchnull(tblValues.Rows(i).Item("FieldValue1"), "")

                        cbo.Items.Add(myItem)

                    Next

                End If

                'Dim myItem As New Telerik.Web.UI.RadComboBoxItem

                'If FieldType.ToLower = "checkbox" Then

                '    cbo.Items.Add(New Telerik.Web.UI.RadComboBoxItem("YES", "YES"))
                '    cbo.Items.Add(New Telerik.Web.UI.RadComboBoxItem("NO", "NO"))
                '    cbo.Items.Add(New Telerik.Web.UI.RadComboBoxItem("N/A", "N/A"))

                'ElseIf FieldType.ToLower = "currency" Then

                '    For i As Integer = 0 To tblValues.Rows.Count - 1

                '        myItem = New Telerik.Web.UI.RadComboBoxItem
                '        myItem.Text = Catchnull(tblValues.Rows(i).Item("FieldValue1"), "")
                '        myItem.Value = Catchnull(tblValues.Rows(i).Item("CurrencyID"), "")
                '        cbo.Items.Add(myItem)

                '    Next

                'ElseIf FieldType.ToLower = "userlookup" Then

                '    For i As Integer = 0 To tblValues.Rows.Count - 1

                '        myItem = New Telerik.Web.UI.RadComboBoxItem
                '        myItem.Text = Catchnull(tblValues.Rows(i).Item("UserName"), "")
                '        myItem.Value = Catchnull(tblValues.Rows(i).Item("FieldValue1"), "")
                '        cbo.Items.Add(myItem)

                '    Next

                'Else

                '    For i As Integer = 0 To tblValues.Rows.Count - 1

                '        myItem = New Telerik.Web.UI.RadComboBoxItem
                '        myItem.Text = Catchnull(tblValues.Rows(i).Item("FieldValue1"), "")
                '        myItem.Value = Catchnull(tblValues.Rows(i).Item("FieldValue1"), "")
                '        cbo.Items.Add(myItem)

                '    Next

                'End If

                If (FieldType.ToLower = "combo" OrElse FieldType.ToLower = "checkbox") Then

                    If EnableClientValidationScripts Then
                        cbo.OnClientSelectedIndexChanged = "CF_" & ObjectType & "_Val_Changed"
                        cbo.OnClientSelectedIndexChanging = "CF_" & ObjectType & "_Val_Changing"
                    End If

                    'If cbo.FindItemByValue("") Is Nothing Then
                    '    cbo.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", ""))
                    '    cbo.SelectedValue = ""
                    'End If

                End If

                Return cbo

            Else

                Dim txt As New System.Web.UI.WebControls.TextBox

                txt.ID = "txtCustom" & FieldName.Replace(" ", "_") & "_" & control_id
                txt.Attributes("FieldName") = FieldName
                txt.Attributes("FieldType") = FieldType
                txt.Attributes("CF_OBJ_ID") = ObjectID
                txt.Attributes("CF_OBJ_TYPE") = ObjectType
                txt.Attributes("ObjectDataID") = ObjectDataID
                txt.Width = System.Web.UI.WebControls.Unit.Percentage(80)

                If Not ForSearch Then
                    With txt
                        .Enabled = Not ReadOnly_Template
                        .Attributes("ROTemplate") = ReadOnly_Template
                    End With
                End If

                Return txt

            End If

        End If

        Return Nothing

    End Function

    Public Function SaveCustomFields(ByVal ControlsPanel As RadPanelBar, Optional ID As Long = 0, Optional ObjectType As String = "") As Boolean

        SaveCustomFields = True

        If ControlsPanel IsNot Nothing Then

            Dim items As New List(Of RadPanelItem)
            For Each itm As RadPanelItem In ControlsPanel.Items
                items.Add(itm)
            Next

            Dim sw As New System.Diagnostics.Stopwatch
            sw.Start()

            'For Each pnlItem As RadPanelItem In ControlsPanel.Items
            Parallel.ForEach(
                items,
                Sub(pnlItem As RadPanelItem)

                    If pnlItem IsNot Nothing AndAlso
                        pnlItem.Items.Count > 0 AndAlso
                        TypeOf pnlItem.Items(0).Controls(0) Is Table Then

                        Dim ControlsTable As System.Web.UI.WebControls.Table = pnlItem.Items(0).Controls(0)
                        SaveCustomFields = SaveCustomFields And SaveCustomFields(ControlsTable, ID, ObjectType)

                    End If

                End Sub
            )
            'Next

            sw.Stop()
            Log.DebugFormat("Custom fields save took {0}s ", sw.Elapsed.TotalSeconds)

        End If

    End Function

    Public Function FindControlValue(ByVal ControlsPanel As RadPanelBar, FieldName As String)

        If ControlsPanel IsNot Nothing Then

            For Each pnlItem As RadPanelItem In ControlsPanel.Items

                If pnlItem IsNot Nothing AndAlso
                    pnlItem.Items.Count > 0 AndAlso
                    TypeOf pnlItem.Items(0).Controls(0) Is Table Then

                    Dim ControlsTable As System.Web.UI.WebControls.Table = pnlItem.Items(0).Controls(0)
                    Dim ctrl = FindControlValue(ControlsTable, FieldName)

                    If ctrl.Control IsNot Nothing Then

                        Return ctrl

                    End If

                End If

            Next

        End If

        Return New With {.Control = Nothing, .Value = String.Empty}

    End Function

    Public Function FindControlValue(ByVal ControlsTable As System.Web.UI.WebControls.Table, FieldName As String)

        Dim FieldValue As String = String.Empty

        If Not IsNothing(ControlsTable) Then

            For i As Integer = 0 To ControlsTable.Rows.Count - 1

                For j As Integer = 1 To 3

                    Try

                        If ControlsTable.Rows(i).Cells.Count > (j * 2) - 1 AndAlso ControlsTable.Rows(i).Cells((j * 2) - 1).Controls.Count > 0 Then

                            Dim myControl As System.Web.UI.WebControls.WebControl = ControlsTable.Rows(i).Cells((j * 2) - 1).Controls(0)
                            If FieldName.Equals(myControl.Attributes("FieldName")) Then

                                Return New With {.Control = myControl, .Value = GetControlValue(myControl)}

                            End If

                        End If

                    Catch ex As Exception

                        Me.ErrorMessage &= ex.Message
                        Log.Error(ex)

                    End Try

                Next

            Next

        End If

        Return New With {.Control = Nothing, .Value = String.Empty}

    End Function

    Public Function SaveCustomFields(ByVal ControlsTable As System.Web.UI.WebControls.Table, Optional CF_OBJ_ID As Long = 0, Optional CF_ObjectType As String = "") As Boolean

        SaveCustomFields = True

        Try

            Dim ObjectID As String = ""
            Dim ObjectType As String = ""
            Dim ObjectDataID As String = ""
            Dim CustomFieldID As String = ""
            Dim FieldName As String = ""
            Dim FieldValue As String = ""
            Dim FieldType As String = ""
            Dim myControl As System.Web.UI.WebControls.WebControl

            If Not IsNothing(ControlsTable) Then

                For i As Integer = 0 To ControlsTable.Rows.Count - 1

                    For j As Integer = 1 To 3

                        Try

                            If ControlsTable.Rows(i).Cells.Count > (j * 2) - 1 AndAlso ControlsTable.Rows(i).Cells((j * 2) - 1).Controls.Count > 0 Then

                                FieldValue = String.Empty
                                myControl = ControlsTable.Rows(i).Cells((j * 2) - 1).Controls(0)
                                FieldName = myControl.Attributes("FieldName")
                                FieldType = myControl.Attributes("FieldType")
                                ObjectID = myControl.Attributes("CF_OBJ_ID")
                                If Not IsNumeric(ObjectID) OrElse CLng(ObjectID) = 0 Then ObjectID = CF_OBJ_ID
                                ObjectType = myControl.Attributes("CF_OBJ_TYPE")
                                If String.IsNullOrWhiteSpace(ObjectID) Then ObjectType = CF_ObjectType
                                ObjectDataID = myControl.Attributes("ObjectDataID")
                                CustomFieldID = myControl.Attributes("CustomFieldID")

                                Dim ReadOnlyTemplate As Boolean = False
                                Boolean.TryParse(myControl.Attributes("ROTemplate"), ReadOnlyTemplate)
                                If ReadOnlyTemplate Then Continue For

                                FieldValue = GetControlValue(myControl)

                                'Check if value has changed. If not, then don't save, skip to next
                                If myControl.Attributes("CFValue") IsNot Nothing AndAlso myControl.Attributes("CFValue") = FieldValue Then

                                    'Move onto next field. This will avoid hitting the database
                                    Continue For

                                End If

                                'Check of dit al klaar bestaan
                                Dim fieldExists As Boolean = (IsNumeric(ObjectDataID) AndAlso CInt(ObjectDataID) > 0)

                                If Not fieldExists Then

                                    Dim tempTable As DataTable = DataHelper.ExecuteDataTable(db, "SELECT COUNT(*) as theCount FROM  CustomField_ObjectData WHERE ObjectType='" & ObjectType & "' GROUP BY ObjectID, FieldName HAVING (ObjectID = " & ObjectID & ") AND (FieldName = '" & FieldName.Replace("'", "''") & "')", "fieldNameValue" & FieldName)
                                    fieldExists = tempTable IsNot Nothing AndAlso tempTable.Rows.Count > 0 AndAlso (Catchnull(tempTable.Rows(0).Item(0), 0) > 0)

                                End If

                                Dim strCustomSql As String

                                If fieldExists Then
                                    If (IsNumeric(ObjectDataID) AndAlso CInt(ObjectDataID) > 0) Then
                                        strCustomSql = "UPDATE CustomField_ObjectData SET FieldValue1 = '" & FieldValue.Replace("'", "''").Trim & "' WHERE ObjectDataID = " & ObjectDataID
                                    Else
                                        strCustomSql = "UPDATE CustomField_ObjectData SET FieldValue1 = '" & FieldValue.Replace("'", "''").Trim & "' WHERE ObjectType='" & ObjectType & "' AND (ObjectID = " & ObjectID & ") AND (FieldName = '" & FieldName.Replace("'", "''") & "')"
                                    End If
                                Else
                                    'NOTE, EK INSERT NIE HIER NUWE FIELDS NIE, AANVAAR DIE GOED MOET AL DAAR WEES
                                    strCustomSql = "if exists (select * from CustomField_ObjectData where ObjectType='" & ObjectType & "' and ObjectID = " & ObjectID & " and FieldName = '" & FieldName.Replace("'", "''") & "' )" & vbCrLf
                                    strCustomSql &= "    update CustomField_ObjectData set FieldValue1 = '" & FieldValue.Replace("'", "''").Trim & "' where ObjectType='" & ObjectType & "' and ObjectID = " & ObjectID & " and FieldName = '" & FieldName.Replace("'", "''") & "'" & vbCrLf
                                    strCustomSql &= "else " & vbCrLf
                                    strCustomSql &= "    insert into CustomField_ObjectData(ObjectID, FieldValue1, FieldName, FieldType, DisplayIndex, ObjectType, CustomFieldID, CreatedDate, CreatedBy)" & vbCrLf
                                    strCustomSql &= "    select " & ObjectID & ", '" & FieldValue.Replace("'", "''").Trim & "', FieldName, FieldType, DisplayIndex, '" & ObjectType & "', CustomFieldID, getdate(), " & ObjectUserID & " from CustomField_Fields where FieldName = '" & FieldName.Replace("'", "''") & "'" & vbCrLf

                                End If

                                strCustomSql = strCustomSql.Replace("''", "NULL")

                                Try
                                    Dim cmd As DbCommand = db.GetSqlStringCommand(strCustomSql)
                                    cmd.CommandTimeout = 120
                                    db.ExecuteNonQuery(cmd)
                                Catch ex As Exception
                                    Log.Error(strCustomSql, ex)
                                    SaveCustomFields = False
                                    ErrorMessage &= ex.Message
                                End Try

                            End If

                        Catch ex As Exception

                            Me.ErrorMessage &= ex.Message
                            SaveCustomFields = False
                            Log.Error(ex)

                        End Try

                    Next

                Next

                'AFTER SAVING, WE RELOAD THE "DUPLICATE" FIELDS
                For i As Integer = 0 To ControlsTable.Rows.Count - 1

                    For j As Integer = 1 To 3

                        Try

                            If ControlsTable.Rows(i).Cells.Count > (j * 2) - 1 AndAlso ControlsTable.Rows(i).Cells((j * 2) - 1).Controls.Count > 0 Then

                                myControl = ControlsTable.Rows(i).Cells((j * 2) - 1).Controls(0)
                                FieldName = myControl.Attributes("FieldName")
                                FieldType = myControl.Attributes("FieldType")

                                If TypeOf myControl Is System.Web.UI.WebControls.Label Then
                                    CType(myControl, System.Web.UI.WebControls.Label).Text = Get_Custom_Field_Value(ObjectID, ObjectType, FieldName, False)
                                End If

                            End If

                        Catch ex As Exception

                            Me.ErrorMessage &= ex.Message
                            SaveCustomFields = False
                            Log.Error(ex)

                        End Try

                    Next

                Next

            End If

        Catch ex As Exception

            ErrorMessage &= ex.Message
            Log.Error(ex)

        End Try

    End Function

    Public Function UpdateCustomField(ByVal ID As Long, ByVal ObjectType As String, ByVal FieldName As String, ByVal FieldValue As String) As Boolean

        Dim sql As String = String.Empty
        sql &= "if exists (select * from CustomField_ObjectData where ObjectType='" & ObjectType & "' and ObjectID = " & ID & " and FieldName = '" & FieldName.Replace("'", "''") & "' )" & vbCrLf
        sql &= "    update CustomField_ObjectData set FieldValue1 = '" & FieldValue.Replace("'", "''").Trim & "' where ObjectType='" & ObjectType & "' and ObjectID = " & ID & " and FieldName = '" & FieldName.Replace("'", "''") & "'" & vbCrLf
        sql &= "else " & vbCrLf
        sql &= "    insert into CustomField_ObjectData(ObjectID, FieldValue1, FieldName, FieldType, DisplayIndex, ObjectType, CustomFieldID)" & vbCrLf
        sql &= "    select " & ID & ", '" & FieldValue.Replace("'", "''").Trim & "', FieldName, FieldType, DisplayIndex, '" & ObjectType & "', CustomFieldID from CustomField_Fields where FieldName = '" & FieldName.Replace("'", "''") & "'" & vbCrLf

        Try

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            cmd.CommandText = 120
            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            Log.Error(String.Format("Data Save Failure: ID={0}, ObjectType={1}, FieldName={2}, FieldValue={3}", ID, ObjectType, FieldName, FieldValue) & vbCrLf & ex.ToString)
            Return False

        End Try

    End Function

    Public Function AddCustomField(ByVal ObjectID As Long, ByVal ObjectType As String, ByVal FieldName As String, ByVal FieldValue As String) As Boolean

        Dim sql As String = String.Empty
        sql &= "insert into CustomField_ObjectData(ObjectID, FieldValue1, FieldName, FieldType, DisplayIndex, ObjectType, CustomFieldID)" & vbCrLf
        sql &= "select " & ObjectID & ", '" & FieldValue.Replace("'", "''").Trim & "', FieldName, FieldType, DisplayIndex, '" & ObjectType & "', CustomFieldID from CustomField_Fields where FieldName = '" & FieldName.Replace("'", "''") & "'" & vbCrLf

        Try

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            cmd.CommandText = 120
            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            Log.Error(String.Format("Data Save Failure: ID={0}, ObjectType={1}, FieldName={2}, FieldValue={3}", ObjectID, ObjectType, FieldName, FieldValue) & vbCrLf & ex.ToString)
            Return False

        End Try

    End Function

    Private Function ToSqlDate(ByVal val As String) As String

        If IsDate(val) Then
            Return CDate(val).ToString("yyyy-MM-dd")
        Else
            Return val
        End If

    End Function

    Public Function AssignCustomFieldTemplates(ByVal templateslist As String, ByVal ID As Long, ByVal ObjectType As String) As Boolean

        Try

            Dim templates() As String = templateslist.Split(";")

            Dim ErrorOccurred As Boolean = False
            For Each template As String In templates

                If template.Trim <> "" Then

                    Dim sql As String = ""
                    sql &= "INSERT INTO [CustomField_ObjectTemplates]([ID],[ObjectType],[TemplateName]) " & vbCrLf
                    sql &= "SELECT TOP 1 " & ID & ", '" & ObjectType.Replace("'", "''") & "', [TemplateName] FROM CustomField_Fields WHERE TemplateName = '" & template.Replace("'", "''") & "' " & vbCrLf
                    sql &= "AND NOT EXISTS (SELECT * FROM CustomField_ObjectTemplates WHERE [ID]=" & ID & " AND [ObjectType] = '" & ObjectType.Replace("'", "''") & "' AND [TemplateName] = CustomField_Fields.[TemplateName])"
                    sql &= "; SELECT SCOPE_IDENTITY();" & vbCrLf

                    Dim ds As DataSet = DataHelper.ExecuteDataset(db, sql)

                    If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                        If Catchnull(ds.Tables(0).Rows(0)(0), 0) <= 0 Then

                            ErrorOccurred = True

                        End If

                    End If

                End If

            Next

            Return Not ErrorOccurred

        Catch ex As Exception

            Log.Error(ex)
            Return False

        End Try

    End Function

    Public Function RemoveTemplateFromObject(ByVal TemplateID As Long, ByVal ObjectID As Long, ByVal ObjectType As String) As Boolean

        Dim sql As String = "DELETE FROM CustomField_ObjectData WHERE ObjectID = " & ObjectID & " AND ObjectType = '" & ObjectType.Replace("'", "''") & "' AND CustomFieldID IN (SELECT [CustomFieldID] FROM CustomField_Fields WHERE TemplateID = " & TemplateID & ");" & vbCrLf
        sql &= "DELETE FROM CustomField_ObjectTemplates WHERE [ObjectID] = " & ObjectID & " AND [ObjectType] = '" & ObjectType.Replace("'", "''") & "' AND [TemplateID] = " & TemplateID

        Try

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            Log.Error(ex)
            Me.ErrorMessage = ex.Message
            Return False

        End Try

    End Function

    Public Function ClearAllValues(ByVal ID As Long, ByVal ObjectType As String) As Boolean

        Try

            Dim sql As String = "DELETE FROM CustomField_ObjectData WHERE ObjectType = '" & ObjectType & "' AND (ObjectID = " & ID & ")"
            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            Log.Error(ex)
            Me.ErrorMessage = ex.Message
            Return False

        End Try

        Return True

    End Function

    Public Function RenameTemplateField(ByVal CustomFieldID As Long, ByVal NewFieldName As String) As Boolean

        Try

            'rename the field lookups, the field names, and where the field has been used before, and the project status validation rules
            Dim sql As String = String.Empty
            sql &= "DECLARE @oldfldname varchar(1000), @newfldname varchar(1000); " & vbCrLf
            sql &= "SELECT @newfldname = '" & NewFieldName.Replace("'", "''") & "';  " & vbCrLf
            sql &= "SELECT @oldfldname = FieldName FROM CustomField_Fields WHERE CustomFieldID = " & CustomFieldID & ";  " & vbCrLf
            sql &= "UPDATE CustomField_DataLookup SET FieldName = LTRIM(RTRIM(@newfldname)) WHERE FieldName = @oldfldname; " & vbCrLf
            sql &= "UPDATE CustomField_ObjectData SET FieldName = @newfldname WHERE CustomFieldID = " & CustomFieldID & vbCrLf
            sql &= "UPDATE CustomField_Fields SET FieldName = @newfldname WHERE CustomFieldID = " & CustomFieldID & vbCrLf

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            Log.Error(ex)
            Me.ErrorMessage = ex.Message
            Return False

        End Try

        Return True

    End Function

    Public Function ChangeTemplateFieldType(ByVal CustomFieldID As Long, ByVal NewType As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, "UPDATE CustomField_ObjectData SET FieldType = '" & NewType.Replace("'", "''") & "' WHERE CustomFieldID = " & CustomFieldID)
            db.ExecuteNonQuery(CommandType.Text, "UPDATE CustomField_Fields SET FieldType = '" & NewType.Replace("'", "''") & "' WHERE CustomFieldID = " & CustomFieldID)
            Return True

        Catch ex As Exception

            Log.Error(ex)
            Me.ErrorMessage = ex.Message
            Return False

        End Try

        Return True

    End Function

    Private Sub AddAnd(ByRef SQLWhere As String)
        If Len(SQLWhere.Trim) > 0 Then
            SQLWhere = SQLWhere & " AND "
        End If
    End Sub

    Private Sub AddOr(ByRef SQLWhere As String)
        If Len(SQLWhere.Trim) > 0 Then
            SQLWhere = SQLWhere & " OR "
        End If
    End Sub

    Public Function GetSearchFields(ByRef SearchFields As String, ByVal controlsTable As Web.UI.WebControls.Table) As String

        If controlsTable IsNot Nothing Then

            For i As Integer = 0 To controlsTable.Rows.Count - 1

                Dim myControl As New Object

                Dim NumberOfColumns As Integer = controlsTable.Rows(i).Cells.Count / 2

                For k As Integer = 1 To NumberOfColumns

                    myControl = controlsTable.Rows(i).Cells((k * 2) - 1).Controls(0)

                    If TypeOf myControl Is System.Web.UI.WebControls.DropDownList Then

                        If Len(Trim(myControl.SelectedValue)) > 0 Then
                            SearchFields &= IIf(SearchFields = "", "", ",") & "[" & myControl.attributes("fieldName").ToString & "] "
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadListBox Then

                        Dim lst As Telerik.Web.UI.RadListBox = CType(myControl, Telerik.Web.UI.RadListBox)

                        For Each itm As Telerik.Web.UI.RadListBoxItem In lst.SelectedItems

                            If Len(Trim(itm.Text)) > 0 Then
                                SearchFields &= IIf(SearchFields = "", "", ", ") & "[" & myControl.attributes("fieldName").ToString & "]"
                                Exit For
                            End If

                        Next

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadComboBox Then

                        Dim cbo As Telerik.Web.UI.RadComboBox = CType(myControl, Telerik.Web.UI.RadComboBox)

                        If Len(Trim(myControl.SelectedValue)) > 0 Then
                            SearchFields &= IIf(SearchFields = "", "", ",") & "[" & myControl.attributes("fieldName").ToString & "] "
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadDatePicker Then

                        Dim d As Telerik.Web.UI.RadDatePicker = myControl
                        If d.SelectedDate.HasValue Then
                            SearchFields &= IIf(SearchFields = "", "", ",") & "[" & myControl.attributes("fieldName").ToString & "] "
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadDateTimePicker Then

                        Dim d As Telerik.Web.UI.RadDateTimePicker = myControl
                        If d.SelectedDate.HasValue Then
                            SearchFields &= IIf(SearchFields = "", "", ",") & "[" & myControl.attributes("fieldName").ToString & "] "
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadTimePicker Then

                        Dim d As Telerik.Web.UI.RadTimePicker = myControl
                        If d.SelectedDate.HasValue Then
                            SearchFields &= IIf(SearchFields = "", "", ",") & "[" & myControl.attributes("fieldName").ToString & "] "
                        End If

                    ElseIf TypeOf myControl Is System.Web.UI.WebControls.Table Then

                        Dim d As System.Web.UI.WebControls.Table = myControl
                        Dim rv As RangeValues = GetRangeValues(myControl.attributes("fieldName").ToString, myControl.attributes("fieldType").ToString, d)

                        If rv.HasValues Then
                            SearchFields &= IIf(SearchFields = "", "", ",") & "[" & myControl.attributes("fieldName").ToString & "] "
                        End If

                    Else

                        If Len(Trim(myControl.Text)) > 0 Then
                            SearchFields &= IIf(SearchFields = "", "", ",") & "[" & myControl.attributes("fieldName").ToString & "] "
                        End If

                    End If

                Next

            Next

        End If

        Return SearchFields

    End Function

    Public Function GetSearchCriteria(ByRef SQLWhere As String, ByVal controlsTable As Web.UI.WebControls.Table, Optional ByVal SearchCFieldsTable As String = "SearchCFields") As String

        If controlsTable IsNot Nothing Then

            For i As Integer = 0 To controlsTable.Rows.Count - 1

                Dim myControl As New Object

                Dim NumberOfColumns As Integer = controlsTable.Rows(i).Cells.Count / 2

                For k As Integer = 1 To NumberOfColumns

                    myControl = controlsTable.Rows(i).Cells((k * 2) - 1).Controls(0)

                    If TypeOf myControl Is System.Web.UI.WebControls.DropDownList Then

                        If Len(Trim(myControl.SelectedValue)) > 0 Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & "(" & GetConditions(SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", myControl.SelectedValue) & ") "
                        End If

                    ElseIf TypeOf myControl Is System.Web.UI.WebControls.Table _
                        AndAlso CType(myControl, System.Web.UI.WebControls.WebControl).Attributes("FieldType").ToLower = "date" Then

                        Dim tbl As System.Web.UI.WebControls.Table = myControl
                        Dim FieldName As String = tbl.Attributes("FieldName")

                        Dim rdtpStart As Telerik.Web.UI.RadDatePicker = Nothing
                        Dim rdtpEnd As Telerik.Web.UI.RadDatePicker = Nothing

                        'txtCustom" & FieldName.Replace(" ", "_") & "_End_" & id
                        For Each row As System.Web.UI.WebControls.TableRow In tbl.Rows

                            For Each cell As System.Web.UI.WebControls.TableCell In row.Cells

                                For Each ctrl As System.Web.UI.Control In cell.Controls

                                    If TypeOf ctrl Is Telerik.Web.UI.RadDatePicker Then

                                        If ctrl.ID.StartsWith("txtCustom" & FieldName.Replace(" ", "_") & "_End_") Then
                                            rdtpEnd = ctrl
                                        ElseIf ctrl.ID.StartsWith("txtCustom" & FieldName.Replace(" ", "_") & "_Start_") Then
                                            rdtpStart = ctrl
                                        End If

                                    End If

                                Next

                            Next

                        Next

                        Dim StartDate As String = String.Empty, EndDate As String = String.Empty
                        If rdtpStart IsNot Nothing AndAlso rdtpStart.SelectedDate.HasValue Then
                            StartDate = rdtpStart.SelectedDate.Value.ToString("yyyy-MM-dd")
                        End If

                        If rdtpEnd IsNot Nothing AndAlso rdtpEnd.SelectedDate.HasValue Then
                            EndDate = rdtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd")
                        End If

                        FieldName = "[" & FieldName & "]"

                        Dim crit As String = String.Empty
                        If Not String.IsNullOrEmpty(StartDate) And Not String.IsNullOrEmpty(EndDate) Then
                            crit &= " (" & SearchCFieldsTable & "." & FieldName & " >= '" & StartDate & " 00:00'"
                            crit &= " AND " & SearchCFieldsTable & "." & FieldName & " <= '" & EndDate & " 23:59:59')"
                        ElseIf Not String.IsNullOrEmpty(StartDate) Then
                            crit &= "" & SearchCFieldsTable & "." & FieldName & " >= '" & StartDate & " 00:00'"
                        ElseIf Not String.IsNullOrEmpty(EndDate) Then
                            crit &= "" & SearchCFieldsTable & "." & FieldName & " <= '" & EndDate & " 23:59:59'"
                        End If

                        If Not String.IsNullOrEmpty(crit) Then
                            AddAnd(SQLWhere)
                            SQLWhere &= "(" & crit & ") "
                        End If

                    ElseIf TypeOf myControl Is System.Web.UI.WebControls.Table _
                        AndAlso CType(myControl, System.Web.UI.WebControls.WebControl).Attributes("FieldType").ToLower = "numeric" Then

                        Dim tbl As System.Web.UI.WebControls.Table = myControl
                        Dim FieldName As String = tbl.Attributes("FieldName")

                        Dim txtStart As System.Web.UI.WebControls.TextBox = Nothing
                        Dim txtEnd As System.Web.UI.WebControls.TextBox = Nothing

                        'txtCustom" & FieldName.Replace(" ", "_") & "_End_" & id
                        For Each row As System.Web.UI.WebControls.TableRow In tbl.Rows

                            For Each cell As System.Web.UI.WebControls.TableCell In row.Cells

                                For Each ctrl As System.Web.UI.Control In cell.Controls

                                    If TypeOf ctrl Is System.Web.UI.WebControls.TextBox Then

                                        If ctrl.ID.StartsWith("txtCustom" & FieldName.Replace(" ", "_") & "_End_") Then
                                            txtEnd = ctrl
                                        ElseIf ctrl.ID.StartsWith("txtCustom" & FieldName.Replace(" ", "_") & "_Start_") Then
                                            txtStart = ctrl
                                        End If

                                    End If

                                Next

                            Next

                        Next

                        Dim StartRange As String = String.Empty, EndRange As String = String.Empty
                        If txtStart IsNot Nothing AndAlso IsNumeric(txtStart.Text.Replace(",", "").Replace(" ", "")) Then
                            StartRange = txtStart.Text.Replace(",", "").Replace(" ", "")
                        End If

                        If txtEnd IsNot Nothing AndAlso IsNumeric(txtEnd.Text.Replace(",", "").Replace(" ", "")) Then
                            EndRange = txtEnd.Text.Replace(",", "").Replace(" ", "")
                        End If

                        FieldName = "[" & FieldName & "]"

                        Dim crit As String = String.Empty
                        If IsNumeric(StartRange) And IsNumeric(EndRange) Then
                            crit = " ( " & SearchCFieldsTable & "." & FieldName & " >= " & StartRange
                            crit &= " AND " & SearchCFieldsTable & "." & FieldName & " <= " & EndRange & " )"
                        ElseIf IsNumeric(StartRange) Then
                            crit = SearchCFieldsTable & "." & FieldName & " >= " & StartRange
                        ElseIf IsNumeric(EndRange) Then
                            crit = SearchCFieldsTable & "." & FieldName & " <= " & EndRange
                        End If

                        If Not String.IsNullOrEmpty(crit) Then
                            AddAnd(SQLWhere)
                            SQLWhere &= "(" & crit & ") "
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadListBox Then

                        Dim lst As Telerik.Web.UI.RadListBox = CType(myControl, Telerik.Web.UI.RadListBox)
                        Dim lstcrit As String = ""

                        For Each itm As Telerik.Web.UI.RadListBoxItem In lst.SelectedItems
                            If Len(Trim(itm.Text)) > 0 Then
                                AddOr(lstcrit)
                                lstcrit = lstcrit & "(" & GetConditions(SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", itm.Text) & ") "
                            End If
                        Next

                        If lstcrit <> "" Then
                            SQLWhere &= "(" & lstcrit & ")"
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadComboBox _
                        AndAlso CType(myControl, System.Web.UI.WebControls.WebControl).Attributes("FieldType").ToLower = "combo" Then

                        Dim cbo As Telerik.Web.UI.RadComboBox = CType(myControl, Telerik.Web.UI.RadComboBox)

                        If Len(Trim(cbo.Text)) > 0 Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & "(" & SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "] = '" & cbo.Text.Replace("'", "''") & "') "
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadComboBox Then

                        Dim cbo As Telerik.Web.UI.RadComboBox = CType(myControl, Telerik.Web.UI.RadComboBox)

                        If Len(Trim(cbo.Text)) > 0 Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & "(" & GetConditions(SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", cbo.Text) & ") "
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadDatePicker Then

                        Dim d As Telerik.Web.UI.RadDatePicker = myControl
                        If d.SelectedDate.HasValue Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & String.Format("(CAST(NULLIF(LTRIM(RTRIM([{0}])),'') AS datetime) >= '{1} 00:00:00' AND CAST(NULLIF(LTRIM(RTRIM([{0}])),'') AS datetime) <= '{1} 23:59:59')", SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", ToSqlDate(d.SelectedDate.Value))
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadDateTimePicker Then

                        Dim d As Telerik.Web.UI.RadDateTimePicker = myControl
                        If d.SelectedDate.HasValue Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & String.Format("(CAST(NULLIF(LTRIM(RTRIM([{0}])),'') AS datetime) >= '{1} 00:00:00' AND CAST(NULLIF(LTRIM(RTRIM([{0}])),'') AS datetime) <= '{1} 23:59:59')", SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", ToSqlDate(d.SelectedDate.Value))
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadTimePicker Then

                        Dim d As Telerik.Web.UI.RadTimePicker = myControl
                        If d.SelectedDate.HasValue Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & String.Format("(CAST(NULLIF(LTRIM(RTRIM([{0}])),'') AS datetime) >= '{1}' AND CAST(NULLIF(LTRIM(RTRIM([{0}])),'') AS datetime) <= '{1}')", SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", d.SelectedDate.Value.ToString("HH:mm:ss"))
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadTextBox Then

                        If Len(Trim(myControl.Text)) > 0 Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & "(" & GetConditions(SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", myControl.text) & ") "
                        End If

                    ElseIf TypeOf myControl Is Telerik.Web.UI.RadNumericTextBox Then

                        If Len(Trim(CType(myControl, Telerik.Web.UI.RadNumericTextBox).Value)) > 0 Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & "(" & GetConditions(SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", myControl.text) & ") "
                        End If

                    Else

                        If Len(Trim(myControl.Text)) > 0 Then
                            AddAnd(SQLWhere)
                            SQLWhere = SQLWhere & "(" & GetConditions(SearchCFieldsTable & ".[" & myControl.attributes("fieldName").ToString & "]", myControl.text) & ") "
                        End If

                    End If

                Next

            Next

        End If

        Return SQLWhere

    End Function

    Private dsCustomFieldValuesTable As DataSet = Nothing

    Public Function LoadCustomFields(ByVal ID As Long, ByVal ObjectType As String) As Boolean

        Dim sql As String = "SELECT * FROM CustomField_ObjectData WHERE ObjectID = " & ID & " AND ObjectType = '" & ObjectType & "'"
        dsCustomFieldValuesTable = DataHelper.ExecuteDataset(db, sql)

    End Function

    Public Function HasCustomField(ByVal FieldName As String) As Boolean

        If dsCustomFieldValuesTable IsNot Nothing Then

            Dim dt As DataTable = dsCustomFieldValuesTable.Tables(0)
            Dim rows() As DataRow = dt.Select("FieldName = '" & FieldName.Replace("'", "''") & "'")

            Return rows.Length > 0

        Else

            Return False

        End If

    End Function

    Public Function GetCustomFieldValue(ByVal FieldName As String, Optional ByVal [DefaultValue] As String = "") As String

        If dsCustomFieldValuesTable IsNot Nothing Then

            Dim dt As DataTable = dsCustomFieldValuesTable.Tables(0)
            Dim rows() As DataRow = dt.Select("FieldName = '" & FieldName.Replace("'", "''") & "'")

            If rows.Length <= 0 OrElse IsDBNull(rows(0)("FieldValue1")) Then
                Return [DefaultValue]
            Else
                Return rows(0)("FieldValue1")
            End If

        Else

            Return [DefaultValue]

        End If

    End Function

    Private Function GetRangeValues(ByVal FieldName As String, ByVal FieldType As String, ByVal tbl As System.Web.UI.WebControls.Table) As RangeValues

        Dim StartRange As String = String.Empty, EndRange As String = String.Empty

        For Each row As System.Web.UI.WebControls.TableRow In tbl.Rows

            For Each cell As System.Web.UI.WebControls.TableCell In row.Cells

                For Each ctrl As System.Web.UI.Control In cell.Controls

                    If TypeOf ctrl Is Telerik.Web.UI.RadDatePicker Then

                        If ctrl.ID.StartsWith("txtCustom" & FieldName.Replace(" ", "_") & "_End_") Then

                            Dim dtp As Telerik.Web.UI.RadDatePicker = ctrl
                            If dtp.SelectedDate.HasValue Then EndRange = dtp.SelectedDate.Value.ToString("yyyy-MM-dd")

                        ElseIf ctrl.ID.StartsWith("txtCustom" & FieldName.Replace(" ", "_") & "_Start_") Then

                            Dim dtp As Telerik.Web.UI.RadDatePicker = ctrl
                            If dtp.SelectedDate.HasValue Then StartRange = dtp.SelectedDate.Value.ToString("yyyy-MM-dd")

                        End If

                    End If

                    If TypeOf ctrl Is System.Web.UI.WebControls.TextBox Then

                        If ctrl.ID.StartsWith("txtCustom" & FieldName.Replace(" ", "_") & "_End_") Then

                            Dim txt As System.Web.UI.WebControls.TextBox = ctrl
                            If Not String.IsNullOrEmpty(txt.Text) Then EndRange = txt.Text

                        ElseIf ctrl.ID.StartsWith("txtCustom" & FieldName.Replace(" ", "_") & "_Start_") Then

                            Dim txt As System.Web.UI.WebControls.TextBox = ctrl
                            If Not String.IsNullOrEmpty(txt.Text) Then EndRange = txt.Text

                        End If

                    End If

                Next

            Next

        Next

        Dim rv As New RangeValues
        rv.StartValue = StartRange
        rv.EndValue = EndRange
        Return rv

    End Function

    Private Class RangeValues

        Public EndValue As String
        Public StartValue As String

        Public Function HasValues() As Boolean

            Return Not (String.IsNullOrEmpty(EndValue) And String.IsNullOrEmpty(StartValue))

        End Function

    End Class

    Public Function MoveCustomFieldValue(ByVal ObjectType As String, ByVal ObjectID As Long, ByVal FromField As String, ByVal ToField As String, ByVal Clear_From_FieldValue As Boolean) As Boolean

        Dim sql As String = String.Empty
        sql &= "DECLARE @ObjectType varchar(10), @ObjectID int, @From_FieldName varchar(255), @To_FieldName varchar(255), @Clear_From_FieldValue bit " & vbCrLf
        sql &= "SELECT @ObjectType = '" & ObjectType & "', @ObjectID = " & ObjectID & ", @From_FieldName = '" & FromField.Replace("'", "''") & "', @To_FieldName = '" & ToField.Replace("'", "''") & "', @Clear_From_FieldValue = " & IIf(Clear_From_FieldValue, "1", "0") & vbCrLf
        sql &= vbCrLf
        sql &= "UPDATE CustomField_ObjectData SET "
        sql &= "    FieldValue1 = ( " & vbCrLf
        sql &= "		SELECT TOP 1 FieldValue1 " & vbCrLf
        sql &= "		FROM CustomField_ObjectData  " & vbCrLf
        sql &= "		WHERE ObjectType = @ObjectType AND ObjectID = @ObjectID AND FieldName = @From_FieldName " & vbCrLf
        sql &= "	) " & vbCrLf
        sql &= "FROM CustomField_ObjectData  " & vbCrLf
        sql &= "WHERE ObjectType = @ObjectType AND ObjectID = @ObjectID AND FieldName = @To_FieldName " & vbCrLf
        sql &= vbCrLf
        sql &= "IF @Clear_From_FieldValue = 1 " & vbCrLf
        sql &= "	UPDATE CustomField_ObjectData SET  " & vbCrLf
        sql &= "		FieldValue1 = NULL " & vbCrLf
        sql &= "	WHERE ObjectType = @ObjectType AND ObjectID = @ObjectID AND FieldName = @From_FieldName " & vbCrLf
        sql &= vbCrLf

        Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
        db.ExecuteNonQuery(cmd)
        Return True

    End Function

    Public Function CloneCustomFields(SourceID As Integer, ObjectType As String, DestinationID As Integer) As Boolean

        Dim sql As String = String.Empty
        sql &= "DECLARE @ObjectType varchar(5), @SourceID int, @DestinationID int " & vbCrLf
        sql &= vbCrLf
        sql &= "SELECT @ObjectType = '{0}', @SourceID = {1}, @DestinationID = {2} " & vbCrLf
        sql &= vbCrLf
        sql &= "--Copy the fields, only where they dont exist " & vbCrLf
        sql &= "INSERT INTO CustomField_ObjectData(ObjectID, FieldName, FieldValue1, FieldType, DisplayIndex, ObjectType, CustomFieldID) " & vbCrLf
        sql &= "SELECT @DestinationID, FieldName, FieldValue1, FieldType, DisplayIndex, ObjectType, CustomFieldID  " & vbCrLf
        sql &= "FROM CustomField_ObjectData  " & vbCrLf
        sql &= "WHERE ObjectType = @ObjectType AND ObjectID = @SourceID " & vbCrLf
        sql &= "     AND NOT EXISTS ( " & vbCrLf
        sql &= "          SELECT * FROM CustomField_ObjectData P  " & vbCrLf
        sql &= "          WHERE P.ObjectType = CustomField_ObjectData.ObjectType AND P.ObjectID = @DestinationID  " & vbCrLf
        sql &= "               AND P.FieldName = CustomField_ObjectData.FieldName " & vbCrLf
        sql &= "     ) " & vbCrLf
        sql &= vbCrLf
        sql &= "--Copy the template records, only where they dont exist " & vbCrLf
        sql &= "INSERT INTO CustomField_ObjectTemplates(ID, ObjectType, TemplateName) " & vbCrLf
        sql &= "SELECT @DestinationID, ObjectType, TemplateName " & vbCrLf
        sql &= "FROM CustomField_ObjectTemplates " & vbCrLf
        sql &= "WHERE ObjectType = @ObjectType AND ID = @SourceID " & vbCrLf
        sql &= "EXCEPT " & vbCrLf
        sql &= "SELECT ID, ObjectType, TemplateName " & vbCrLf
        sql &= "FROM CustomField_ObjectTemplates " & vbCrLf
        sql &= "WHERE ObjectType = @ObjectType AND ID = @DestinationID " & vbCrLf

        sql = String.Format(sql, ObjectType, SourceID, DestinationID)

        Try

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            Return False

        End Try

    End Function

    Private Function GetControlValue(myControl As WebControl, Optional ByRef HasChanged As Boolean = True) As String

        Dim FieldValue As String = String.Empty

        If TypeOf myControl Is System.Web.UI.WebControls.DropDownList Then

            With CType(myControl, System.Web.UI.WebControls.DropDownList)
                If .SelectedIndex > -1 Then FieldValue = .SelectedValue
            End With

        ElseIf TypeOf myControl Is Telerik.Web.UI.RadTimePicker Then

            Dim dtp As Telerik.Web.UI.RadTimePicker = myControl
            If dtp.SelectedDate.HasValue Then FieldValue = dtp.SelectedDate.Value.ToString("HH:mm:ss")

        ElseIf TypeOf myControl Is Telerik.Web.UI.RadDateTimePicker Then

            Dim dtp As Telerik.Web.UI.RadDateTimePicker = myControl
            If dtp.SelectedDate.HasValue Then FieldValue = dtp.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")

        ElseIf TypeOf myControl Is Telerik.Web.UI.RadDatePicker Then

            Dim dtp As Telerik.Web.UI.RadDatePicker = myControl
            If dtp.SelectedDate.HasValue Then FieldValue = ToSqlDate(dtp.SelectedDate.Value)

        ElseIf TypeOf myControl Is System.Web.UI.WebControls.TextBox Then

            Dim txt As Web.UI.WebControls.TextBox = myControl
            FieldValue = txt.Text

        ElseIf TypeOf myControl Is Telerik.Web.UI.RadTextBox Then

            Dim txt As Telerik.Web.UI.RadTextBox = myControl
            FieldValue = txt.Text

        ElseIf TypeOf myControl Is Telerik.Web.UI.RadNumericTextBox Then

            Dim txt As Telerik.Web.UI.RadNumericTextBox = myControl
            If txt.Value.HasValue Then FieldValue = txt.Value

        ElseIf TypeOf myControl Is Telerik.Web.UI.RadListBox Then

            'Telerik.Web.UI.RadListBox
            Dim lst As Telerik.Web.UI.RadListBox = myControl
            Dim values As String = ""
            For Each itm As Telerik.Web.UI.RadListBoxItem In lst.SelectedItems
                If itm.Value <> "" Then values &= IIf(values = "", "", Chr(255)) & itm.Value
            Next
            FieldValue = values

        ElseIf TypeOf myControl Is System.Web.UI.WebControls.Label Then

            FieldValue = String.Empty

        Else

            Dim cbo As Telerik.Web.UI.RadComboBox = CType(myControl, Telerik.Web.UI.RadComboBox)
            Select Case cbo.Attributes("FieldType")

                Case "currency"

                    If cbo.SelectedIndex > -1 Then

                        FieldValue = cbo.SelectedItem.Value

                    ElseIf cbo.SelectedValue IsNot Nothing Then

                        FieldValue = cbo.SelectedValue

                    Else

                        FieldValue = String.Empty

                    End If

                Case "locationindexlookup", "userlookup" ', "contactslookup", "contactsentitylookup", "contactlist", "contactsentitylist"

                    If cbo.SelectedIndex > -1 Then

                        FieldValue = cbo.SelectedItem.Text

                    ElseIf cbo.SelectedValue IsNot Nothing Then

                        FieldValue = cbo.SelectedValue

                    Else

                        FieldValue = String.Empty

                    End If

                Case Else

                    FieldValue = cbo.Text

            End Select

        End If

        If myControl.Attributes("CFValue") IsNot Nothing AndAlso myControl.Attributes("CFValue") = FieldValue Then

            'Move onto next field. This will avoid hitting the database
            HasChanged = False

        End If

        Return FieldValue

    End Function

    Public Function GetCustomFieldGridColumns(CFTemplateID As Long) As DataSet

        Dim sql As String = String.Empty
        sql &= "select distinct c.name ColumnName, pt.* " & vbCrLf
        sql &= "from sys.columns c " & vbCrLf
        sql &= "	left join CustomField_Fields pt on c.name = 'CField_' + cast(PT.CustomFieldID as varchar) " & vbCrLf
        sql &= "where object_id=object_id('CustomField_Grid_" & CFTemplateID & "') " & vbCrLf
        sql &= "	and (c.name in ('DataID','ObjectID','ObjectType','EntryDate','EntryPerson') or c.name like 'CField_%')" & vbCrLf

        Return DataHelper.ExecuteDataset(db, sql)

    End Function

    Public Function Get_Custom_Field_Grid_Data(ByVal ObjectID As Long, ByVal ObjectType As String, ByVal CFTemplateID As Long) As DataTable

        Dim sql As String = String.Format("select * from CustomField_Grid_{0} where ObjectID = {1} and ObjectType = '{2}'", CFTemplateID, ObjectID, ObjectType)
        Return DataHelper.ExecuteDataset(db, sql).Tables(0)

    End Function

    Public Function Get_Custom_Field_Grid_Data(ByVal CFTemplateID As Long, ByVal ObjectType As String) As DataTable

        Dim sql As String = String.Format("select * from CustomField_Grid_{0} where ObjectType = '{1}'", CFTemplateID, ObjectType)
        Return DataHelper.ExecuteDataset(db, sql).Tables(0)

    End Function

    Public Function LoadCustomFieldGrid(ByVal ObjectID As Long, ByVal ObjectType As String, ByVal CFTemplateID As Long, ByVal CFTemplateName As String, ByVal DisplayDateFormat As String) As RadGrid

        Dim grid As New RadGrid With {
            .AutoGenerateColumns = False,
            .AllowAutomaticInserts = False,
            .AllowAutomaticUpdates = False,
            .AllowAutomaticDeletes = False,
            .ShowGroupPanel = True,
            .ClientIDMode = Web.UI.ClientIDMode.Predictable,
            .Width = Unit.Percentage(100)
        }
        '.AutoGenerateEditColumn = True,
        '.AutoGenerateDeleteColumn = True,

        With grid.MasterTableView

            .AutoGenerateColumns = False
            .AllowAutomaticInserts = False
            .AllowAutomaticUpdates = False
            .AllowAutomaticDeletes = False
            .AllowFilteringByColumn = True
            '.EditMode = GridEditMode.InPlace
            .CommandItemDisplay = GridCommandItemDisplay.Top
            .InsertItemPageIndexAction = GridInsertItemPageIndexAction.ShowItemOnCurrentPage

            With .EditFormSettings
                .ColumnNumber = 3
                .FormTableStyle.Width = Unit.Percentage(100)
                .FormMainTableStyle.Width = Unit.Percentage(100)
            End With

        End With

        grid.Columns.Add(
            New GridEditCommandColumn With {
                .ButtonType = GridButtonColumnType.ImageButton,
                .UniqueName = "EditCommandColumn"
            })

        grid.Attributes("CFTemplateID") = CFTemplateID
        grid.MasterTableView.DataKeyNames = New String() {"DataID", "ObjectID", "ObjectType"}

        'Add the column names
        Dim dsColumns As DataSet = GetCustomFieldGridColumns(CFTemplateID)
        For Each col As DataRow In dsColumns.Tables(0).Rows

            Dim ColumnName As String = col("ColumnName")

            If Catchnull(col("CustomFieldID"), 0) = 0 Then

                Dim boundColumn1 As New GridBoundColumn() With {
                    .DataField = ColumnName,
                    .UniqueName = ColumnName,
                    .HeaderText = Catchnull(col("FieldName"), ColumnName),
                    .Display = False,
                    .ReadOnly = True
                }

                Select Case ColumnName
                    Case "ObjectID"
                        boundColumn1.DefaultInsertValue = ObjectID
                    Case "ObjectType"
                        boundColumn1.DefaultInsertValue = ObjectType
                    Case "EntryPerson"
                        boundColumn1.DefaultInsertValue = ObjectUserID
                End Select

                grid.MasterTableView.Columns.Add(boundColumn1)

            Else

                Dim tc As New GridTemplateColumn() With {
                    .ItemTemplate = New CFGridTemplate() With {
                            .ColumnName = ColumnName,
                            .CustomFieldID = Catchnull(col("CustomFieldID"), 0),
                            .FieldName = Catchnull(col("FieldName"), String.Empty)
                        },
                    .EditItemTemplate = New CFGridEditTemplate() With {
                            .CustomFields = Me,
                            .ColumnName = Catchnull(col("ColumnName"), String.Empty),
                            .CustomFieldID = Catchnull(col("CustomFieldID"), 0),
                            .FieldName = Catchnull(col("FieldName"), String.Empty),
                            .FieldType = Catchnull(col("FieldType"), String.Empty),
                            .ObjectID = ObjectID,
                            .DataID = 0,
                            .ObjectType = ObjectType,
                            .DisplayDateFormat = DisplayDateFormat,
                            .LoadOnDemand = col("LoadOnDemand")
                        },
                    .HeaderText = Catchnull(col("FieldName"), col("ColumnName")),
                    .DataField = ColumnName,
                    .UniqueName = ColumnName.Replace(" ", "_"),
                    .GroupByExpression = String.Format("Group By {0}", ColumnName, .HeaderText)
                }

                grid.MasterTableView.Columns.Add(tc)

            End If

        Next

        grid.Columns.Add(
            New GridButtonColumn With {
                .Text = "Delete",
                .CommandName = "Delete",
                .ButtonType = GridButtonColumnType.ImageButton,
                .ConfirmText = "Are you sure you want to delete this entry?"
            })

        grid.AllowPaging = False
        grid.AllowSorting = True
        grid.ClientSettings.Scrolling.AllowScroll = True
        grid.ClientSettings.Scrolling.SaveScrollPosition = True
        grid.ClientSettings.Scrolling.ScrollHeight = Unit.Pixel(200)
        grid.ClientSettings.EnableRowHoverStyle = True
        grid.ClientSettings.EnableAlternatingItems = True
        grid.ClientSettings.AllowDragToGroup = True
        grid.GroupingSettings.CaseSensitive = False
        grid.GroupingSettings.ShowUnGroupButton = True

        'Dim CommandItems As New RadGridHeaderTemplate()
        'CommandItems.InstantiateIn(grid.MasterTableView)

        AddHandler grid.NeedDataSource, Sub(source As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
                                            grid.DataSource = Get_Custom_Field_Grid_Data(ObjectID, ObjectType, CFTemplateID)
                                        End Sub

        AddHandler grid.ItemCommand, AddressOf grid_ItemCommand
        AddHandler grid.UpdateCommand, AddressOf grid_UpdateCommand
        AddHandler grid.InsertCommand, AddressOf grid_InsertCommand
        AddHandler grid.DeleteCommand, AddressOf grid_DeleteCommand

        Return grid

    End Function

    Private Sub grid_DeleteCommand(sender As Object, e As GridCommandEventArgs)

        'Log.Warn("grid_DeleteCommand: " & e.CommandName)

        Dim itm As GridDataItem = TryCast(e.Item, GridDataItem)
        Dim sql As String = String.Format("DELETE FROM CustomField_Grid_{0} WHERE DataID = {1}", sender.Attributes("CFTemplateID"), itm.GetDataKeyValue("DataID").ToString)

        Try

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            db.ExecuteNonQuery(cmd)

        Catch ex As Exception
            Log.Error(ex)
        End Try

    End Sub

    Private Sub grid_InsertCommand(sender As Object, e As GridCommandEventArgs)

        'Log.Warn("grid_InsertCommand: " & e.CommandName)

        Try

            Dim editedItem As GridEditFormInsertItem = TryCast(e.Item, GridEditFormInsertItem)

            Dim cfValues As New Hashtable()
            e.Item.OwnerTableView.ExtractValuesFromItem(cfValues, editedItem)

            For Each cf As String In cfValues.Keys
                Log.Warn(String.Format("{0} = {1}", cf, cfValues(cf)))
            Next

            cfValues.Add("ObjectID", DirectCast(editedItem("ObjectID").Controls(0), System.Web.UI.WebControls.TextBox).Text)
            cfValues.Add("ObjectType", DirectCast(editedItem("ObjectType").Controls(0), System.Web.UI.WebControls.TextBox).Text)
            cfValues.Add("EntryPerson", DirectCast(editedItem("EntryPerson").Controls(0), System.Web.UI.WebControls.TextBox).Text)

            Dim cols As String = String.Empty
            Dim vals As String = String.Empty

            For Each cf As String In cfValues.Keys

                Dim value As String = cfValues(cf)
                If String.IsNullOrWhiteSpace(value) Then value = String.Empty

                cols &= String.Format("{0}{1}", IIf(String.IsNullOrWhiteSpace(cols), "", ", "), cf)
                vals &= String.Format("{0}NULLIF('{1}','')", IIf(String.IsNullOrWhiteSpace(vals), "", ", "), value.Replace("'", "''"))

            Next

            Dim sql As String = String.Format(
                "INSERT INTO CustomField_Grid_{0} ({1})" & vbCrLf & "VALUES({2})",
                sender.Attributes("CFTemplateID"),
                cols,
                vals
            )

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            db.ExecuteNonQuery(cmd)

        Catch ex As Exception
            Log.Error(ex)
        End Try

    End Sub

    Private Sub grid_ItemCommand(sender As Object, e As GridCommandEventArgs)

        'Log.Warn("grid_ItemCommand: " & e.CommandName)

    End Sub

    Private Sub grid_UpdateCommand(sender As Object, e As GridCommandEventArgs)

        'Log.Warn("grid_UpdateCommand: " & e.CommandName)

        Try

            Dim editedItem As GridEditableItem = TryCast(e.Item, GridEditableItem)

            Dim cfValues As New Hashtable()
            e.Item.OwnerTableView.ExtractValuesFromItem(cfValues, editedItem)

            Dim sql As String = String.Empty
            sql &= String.Format("UPDATE CustomField_Grid_{0} SET", sender.Attributes("CFTemplateID")) & vbCrLf

            Dim crit As String = String.Empty
            For Each cf As String In cfValues.Keys

                Dim value As String = cfValues(cf)
                If String.IsNullOrWhiteSpace(value) Then value = String.Empty

                crit &= String.Format("{0}{1} = NULLIF('{2}','')" & vbCrLf, IIf(String.IsNullOrWhiteSpace(crit), "     ", "    ,"), cf, value.Replace("'", "''"))

            Next

            sql &= crit
            sql &= "WHERE DataID = " & editedItem.GetDataKeyValue("DataID").ToString

            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            db.ExecuteNonQuery(cmd)

        Catch ex As Exception
            Log.Error(ex)
        End Try

    End Sub

    Private Class RadGridHeaderTemplate
        Implements System.Web.UI.ITemplate

        Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Sub New()
        End Sub

        Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn

            Try

                Dim div As HtmlGenericControl = New HtmlGenericControl("div")
                div.Attributes.Add("class", "cmdItem")

                Dim addShiftButton As LinkButton = New LinkButton With {.ID = "btnAddShift", .CommandName = "AddShift", .OnClientClick = "return fireCommand('AddShift', '');"}
                Dim addShiftButtonImage As HtmlGenericControl = New HtmlGenericControl("img")
                addShiftButtonImage.Attributes.Add("alt", "")
                addShiftButtonImage.Attributes.Add("src", "../../Images/Icons/AddRecord.png")
                addShiftButton.Controls.Add(addShiftButtonImage)
                addShiftButton.Text = "Add Shift"

                Dim exportCSVButton As LinkButton = New LinkButton With {.ID = "btnExportCSV", .CommandName = "Export CSV", .OnClientClick = "return exportGrid('CSV');"}
                Dim exportCSVButtonImage As HtmlGenericControl = New HtmlGenericControl("img")
                exportCSVButtonImage.Attributes.Add("alt", "")
                exportCSVButtonImage.Attributes.Add("src", "../../Images/Icons/ExportCSV.png")
                exportCSVButton.Controls.Add(exportCSVButtonImage)
                exportCSVButton.Text = "Export to CSV"

                Dim manageShiftColumnButton As LinkButton = New LinkButton With {.ID = "btnManageShiftColumns", .CommandName = "ManageShiftColumns", .OnClientClick = "return fireCommand('ManageShiftColumns', '');"}
                Dim manageShiftColumnButtonImage As HtmlGenericControl = New HtmlGenericControl("img")
                manageShiftColumnButtonImage.Attributes.Add("alt", "")
                manageShiftColumnButtonImage.Attributes.Add("src", "../../Images/Icons/Columns.png")
                manageShiftColumnButton.Controls.Add(manageShiftColumnButtonImage)
                manageShiftColumnButton.Text = "Manage Shift Columns"

                Dim manageJobColumnButton As LinkButton = New LinkButton With {.ID = "btnManageJobColumns", .CommandName = "ManageJobColumns", .OnClientClick = "return fireCommand('ManageJobColumns', '');"}
                Dim manageJobColumnButtonImage As HtmlGenericControl = New HtmlGenericControl("img")
                manageJobColumnButtonImage.Attributes.Add("alt", "")
                manageJobColumnButtonImage.Attributes.Add("src", "../../Images/Icons/Columns.png")
                manageJobColumnButton.Controls.Add(manageJobColumnButtonImage)
                manageJobColumnButton.Text = "Manage Job Columns"

                div.Controls.Add(addShiftButton)
                div.Controls.Add(exportCSVButton)
                div.Controls.Add(manageShiftColumnButton)
                div.Controls.Add(manageJobColumnButton)

                container.Controls.Add(div)

            Catch ex As Exception
                Log.Error(ex)
            End Try

        End Sub

    End Class

    Public Class CFGridTemplate
        Implements Web.UI.ITemplate

        Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Property CustomFieldID As Long
        Public Property FieldName As String
        Public Property ColumnName As String

        Protected lbl As Label

        Public Sub InstantiateIn(container As Web.UI.Control) Implements Web.UI.ITemplate.InstantiateIn

            lbl = New Label With {
                    .ID = String.Format("cflbl_{0}_{1}", CustomFieldID, FieldName)
                }
            AddHandler lbl.DataBinding, AddressOf lbl_DataBinding

            container.Controls.Add(lbl)

        End Sub

        Sub lbl_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

            Try

                Dim lbl As Label = DirectCast(sender, Label)
                Dim container As GridDataItem = DirectCast(lbl.NamingContainer, GridDataItem)
                Dim dr As DataRowView = DirectCast(container.DataItem, DataRowView)

                lbl.Text = Catchnull(dr(ColumnName), String.Empty)

            Catch ex As Exception
                Log.Error(ex)
            End Try

        End Sub

    End Class

    Public Class CFGridEditTemplate
        Implements Web.UI.ITemplate, Web.UI.IBindableTemplate

        Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Property CustomFields As CustomFieldsManager

        Public Property ObjectID As String = String.Empty
        Public Property DataID As Long
        Public Property ObjectType As String = String.Empty

        Public Property CustomFieldID As Long
        Public Property FieldName As String = String.Empty
        Public Property ColumnName As String = String.Empty
        Public Property FieldType As String = String.Empty
        Public Property DisplayDateFormat As String = String.Empty
        Public Property LoadOnDemand As String = String.Empty

        Protected ctrl As PlaceHolder

        Public Sub InstantiateIn(container As Web.UI.Control) Implements Web.UI.ITemplate.InstantiateIn

            ctrl = New PlaceHolder With {
                    .ID = String.Format("cfph_{0}_{1}", CustomFieldID, FieldName)
                }
            AddHandler ctrl.DataBinding, AddressOf ctrl_DataBinding

            Dim c As WebControl = CustomFields.Get_Custom_Field_Control(
               ObjectID:=ObjectID,
               ReadOnly_Template:=False,
               ObjectDataID:=DataID,
               CustomFieldID:=CustomFieldID,
               ObjectType:=ObjectType,
               FieldName:=FieldName,
               FieldType:=FieldType,
               ForSearch:=False,
               DisplayDateFormat:=DisplayDateFormat,
               LoadOnDemand:=LoadOnDemand
           )

            ctrl.Controls.Add(c)
            container.Controls.Add(ctrl)

        End Sub

        Sub ctrl_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

            Try

                Dim ph As PlaceHolder = DirectCast(sender, PlaceHolder)
                Dim container As GridEditFormItem = DirectCast(ph.NamingContainer, GridEditFormItem)

                If TypeOf container.DataItem Is DataRowView Then

                    Dim dr As DataRowView = DirectCast(container.DataItem, DataRowView)
                    CustomFields.SetFieldValue(Catchnull(dr(ColumnName), String.Empty), ctrl.Controls(0), FieldType, DisplayDateFormat, LoadOnDemand)

                End If

            Catch ex As Exception
                Log.Error(ex)
            End Try

        End Sub

        Public Function ExtractValues(container As Web.UI.Control) As IOrderedDictionary Implements Web.UI.IBindableTemplate.ExtractValues

            Dim dick As New OrderedDictionary()

            Try

                If ctrl IsNot Nothing AndAlso ctrl.Controls.Count > 0 Then

                    Dim HasChanged As Boolean = False
                    dick.Add(ColumnName, CustomFields.GetControlValue(ctrl.Controls(0), HasChanged))
                    'Log.WarnFormat("Extracting values: {0} [{1}] = {2}", FieldName, ColumnName, dick(ColumnName))

                Else
                    dick.Add(ColumnName, String.Empty)
                    'Log.WarnFormat("Extracting values: {0} [{1}] is empty", FieldName, ColumnName)
                End If

            Catch ex As Exception
                Log.Error(ex)
            End Try

            Return dick

        End Function

    End Class

    Function DeleteTemplate(ByVal TemplateID As Long) As Boolean

        Try

            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_CustomField_DeleteTemplate")
            db.AddInParameter(cmd, "TemplateID", TemplateID)
            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            Log.Error(ex)
            Return False

        End Try

    End Function

    Function RemoveAllAutoApplyRules(ByVal TemplateName As String) As Boolean

        Try

            Dim sql As String = String.Format("DELETE FROM CustomField_Automation WHERE TemplateName = NULLIF('{0}','')", TemplateName.Replace("'", "''"))
            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            Log.Error(ex)
            Return False

        End Try

    End Function

    Function RemoveTemplateFromAllObjects(TemplateID As Long) As Boolean

        Dim sql As String = String.Empty
        sql &= "DELETE FROM CustomField_ObjectData WHERE CustomFieldID IN (SELECT PT.CustomFieldID FROM CustomField_Fields PT WHERE PT.TemplateID = {0});"
        sql &= "DELETE FROM CustomField_ObjectTemplates WHERE TemplateID = {0};"

        Dim cmd As DbCommand = db.GetSqlStringCommand(String.Format(sql, TemplateID))
        cmd.CommandText = 120
        db.ExecuteNonQuery(cmd)
        Return True

    End Function

    Public Shared Function CleanupFilename(ByVal ProposedName As String) As String

        'clean up the file name by taking out anythign thats NOT in one of the following
        '\w      :  alphanumeric char          _   :  underscore
        '        :  space  
        '\.      :  dot                       \-   :  minus or dash
        '\u0028  :  (  open bracket       \u0029   :  ) close bracket
        '\u005B  :  (  open bracket       \u005D   :  ) close bracket
        '\u002C  :  ,  comma              \u0021   :  ! exclamation mark 
        '\u0027  :  '  apostrophe         \u003D   :  ; semicolon
        '\u003B  :  =  equal sign         \u002B   :  + plus sign

        Try

            ProposedName = ProposedName.Replace(vbCrLf, "").Replace(vbTab, "")
            Return System.Text.RegularExpressions.Regex.Replace(ProposedName, _
            "[^\w _\.\-\u0028\u0029\u002C\u0021\u0027\u003D\u003B\u002B\u005D\u005B]", "")

        Catch ex As Exception

            Return ProposedName

        End Try

    End Function

End Class
