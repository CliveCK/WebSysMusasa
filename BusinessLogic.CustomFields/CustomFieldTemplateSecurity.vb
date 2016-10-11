Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class CustomFieldTemplateSecurity

    Private ObjectUserID As Long
    Private ConnectionName As String
    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database

#Region "Variables"

    Private mUserID As Integer
    Private mTemplateID As Integer
    Private mErrorMessage As String

#End Region

#Region "Public Properties"

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return mErrorMessage
        End Get
    End Property

#End Region

#Region "Constructors"

    Sub New(ByVal ConnectionName As String, UserID As Long)

        Me.ConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)
        Me.ObjectUserID = UserID

    End Sub

#End Region

#Region "Methods"

    Public Function GetTemplatesWithoutPermissions() As DataSet

        Dim sql As String = "SELECT * FROM CustomField_Templates WHERE TemplateID NOT IN (SELECT TemplateID FROM CustomField_TemplateSecurity)"

        Return DataHelper.ExecuteDataset(db, sql, "TemplatesWithoutPermissions")

    End Function

    Public Function GetTemplatesAccessedThroughGroups(ByVal UserID As Long) As DataSet

        Dim sql As String = ""

        sql &= "SELECT CustomField_Templates.*  " & vbCrLf
        sql &= "FROM CustomField_Templates " & vbCrLf
        sql &= "	INNER JOIN CustomField_TemplateSecurity ON CustomField_TemplateSecurity.TemplateID = CustomField_Templates.TemplateSecurityID AND CustomField_TemplateSecurity.GroupID IS NOT NULL " & vbCrLf
        sql &= "	INNER JOIN UserGroups ON CustomField_TemplateSecurity.GroupID = UserGroups.GroupID " & vbCrLf
        sql &= "WHERE UserGroups.[UserID] = " & UserID & vbCrLf

        Return DataHelper.ExecuteDataset(db, sql, "TemplatesAccessedThroughGroups")

    End Function

    Public Function GrantUserSecurity(ByVal UserID As Long, ByVal TemplateID As Long, ByVal SecurityType As CustomFieldSecurityType) As Boolean

        Dim sql As String = ""

        'If there is no security, then we INSERT
        'If there is security, we leave intact, BUT upgrade all the read security entries IIF we are setting write security

        sql &= "declare @EditAllowed bit, @TemplateID int; " & vbCrLf
        sql &= "select @EditAllowed = " & CInt(SecurityType) & ", @TemplateID = " & TemplateID & ", @UserID = " & UserID & vbCrLf
        sql &= vbCrLf
        sql &= "insert into [CustomField_TemplateSecurity]([TemplateID],[UserID],[EditAllowed])" & vbCrLf
        sql &= "select @TemplateID, @UserID, @EditAllowed" & vbCrLf
        sql &= "where not exists (select * from [CustomField_TemplateSecurity] where [TemplateID] = @TemplateID and [UserID] = @UserID) " & vbCrLf

        If SecurityType = CustomFieldSecurityType.Write Then

            sql &= vbCrLf
            sql &= "update [CustomField_TemplateSecurity] set [EditAllowed] = 1"
            sql &= "where [TemplateID] = @TemplateID and [UserID] = @UserID and [EditAllowed] < @EditAllowed" & vbCrLf

        End If

        Return DataHelper.ExecuteNonQuery(db, sql)

    End Function

    Public Function GrantGroupSecurity(ByVal GroupID As Long, ByVal TemplateID As Long, ByVal SecurityType As CustomFieldSecurityType) As Boolean

        Dim sql As String = ""

        'If there is no security, then we INSERT
        'If there is security, we leave intact, BUT upgrade all the read security entries IIF we are setting write security

        sql &= "declare @EditAllowed bit, @TemplateID int; " & vbCrLf
        sql &= "select @EditAllowed = " & CInt(SecurityType) & ", @TemplateID = " & TemplateID & ", @GroupID = " & GroupID & vbCrLf
        sql &= vbCrLf
        sql &= "insert into [CustomField_TemplateSecurity]([TemplateID],[GroupID],[EditAllowed])" & vbCrLf
        sql &= "select @TemplateID, @GroupID, @EditAllowed" & vbCrLf
        sql &= "where not exists (select * from [CustomField_TemplateSecurity] where [TemplateID] = @TemplateID and [GroupID] = @GroupID) " & vbCrLf

        If SecurityType = CustomFieldSecurityType.Write Then

            sql &= vbCrLf
            sql &= "update [CustomField_TemplateSecurity] set [EditAllowed] = 1"
            sql &= "where [TemplateID] = @TemplateID and [GroupID] = @GroupID and [EditAllowed] < @EditAllowed" & vbCrLf

        End If

        Return DataHelper.ExecuteNonQuery(db, sql)

    End Function

    Public Function RevokeBulkGroupSecurity(ByVal TemplateID As Long, ByVal GroupIDs As String, ByVal SecurityType As CustomFieldSecurityType) As Boolean

        If GroupIDs <> "" Then
            Return DataHelper.ExecuteNonQuery(db, "DELETE FROM CustomField_TemplateSecurity WHERE TemplateID = " & TemplateID & " AND GroupID IN (" & GroupIDs & ") AND EditAllowed = " & CInt(SecurityType))
        Else
            Return True
        End If

    End Function

    Public Function RevokeBulkUserSecurity(ByVal TemplateID As Long, ByVal UserIDs As String, ByVal SecurityType As CustomFieldSecurityType) As Boolean

        If UserIDs <> "" Then
            Return DataHelper.ExecuteNonQuery(db, "DELETE FROM CustomField_TemplateSecurity WHERE TemplateID = " & TemplateID & " AND UserID IN (" & UserIDs & ") AND EditAllowed = " & CInt(SecurityType))
        Else
            Return True
        End If

    End Function

    Public Function AssignBulkGroupSecurity(ByVal TemplateID As Long, ByVal GroupIDs As String, ByVal SecurityType As CustomFieldSecurityType) As Boolean

        Dim sql As String = ""

        If GroupIDs = "" Then Return True

        'If there is no security, then we INSERT
        'If there is security, we leave intact, BUT upgrade all the read security entries IIF we are setting write security

        sql &= "declare @EditAllowed bit, @TemplateID int; " & vbCrLf
        sql &= "select @EditAllowed = " & CInt(SecurityType) & ", @TemplateID = " & TemplateID & vbCrLf
        sql &= vbCrLf
        sql &= "insert into [CustomField_TemplateSecurity]([TemplateID],[GroupID],[EditAllowed],[UserID])" & vbCrLf
        sql &= "select @TemplateID, [GroupID], @EditAllowed, 0" & vbCrLf
        sql &= "from groups where [GroupID] in (0" & GroupIDs & ") " & vbCrLf
        sql &= "    and not exists (select * from [CustomField_TemplateSecurity] where [TemplateID] = @TemplateID and [CustomField_TemplateSecurity].[GroupID] = groups.[GroupID]) " & vbCrLf

        If SecurityType = CustomFieldSecurityType.Write Then

            sql &= vbCrLf
            sql &= "update [CustomField_TemplateSecurity] set [EditAllowed] = 1" & vbCrLf
            sql &= "where [TemplateID] = @TemplateID and [GroupID] in (0" & GroupIDs & ") and [EditAllowed] < @EditAllowed" & vbCrLf

        End If

        Return DataHelper.ExecuteNonQuery(db, sql)

    End Function

    Public Function AssignBulkUserSecurity(ByVal TemplateID As Long, ByVal UserIDs As String, ByVal SecurityType As CustomFieldSecurityType) As Boolean

        Dim sql As String = ""

        If UserIDs = "" Then Return True

        'If there is no security, then we INSERT
        'If there is security, we leave intact, BUT upgrade all the read security entries IIF we are setting write security

        sql &= "declare @EditAllowed bit, @TemplateID int; " & vbCrLf
        sql &= "select @EditAllowed = " & CInt(SecurityType) & ", @TemplateID = " & TemplateID & ";" & vbCrLf
        sql &= vbCrLf
        sql &= "insert into [CustomField_TemplateSecurity]([TemplateID],[UserID],[EditAllowed],[GroupID])" & vbCrLf
        sql &= "select @TemplateID, [UserID], @EditAllowed, 0" & vbCrLf
        sql &= "from users where [UserID] in (0" & UserIDs & ") " & vbCrLf
        sql &= "    and not exists (select * from [CustomField_TemplateSecurity] where [TemplateID] = @TemplateID and [CustomField_TemplateSecurity].[UserID] = users.[UserID]) " & vbCrLf

        If SecurityType = CustomFieldSecurityType.Write Then

            sql &= vbCrLf
            sql &= "update [CustomField_TemplateSecurity] set [EditAllowed] = 1"
            sql &= "where [TemplateID] = @TemplateID and [UserID] in (0" & UserIDs & ") and [EditAllowed] < @EditAllowed" & vbCrLf

        End If

        Return DataHelper.ExecuteNonQuery(db, sql)

    End Function

    Public Function RevokeUserSecurity(ByVal UserID As Long, ByVal TemplateID As Long) As Boolean

        Dim sql As String = "DELETE FROM CustomField_TemplateSecurity WHERE TemplateID = " & TemplateID & " AND UserID = " & UserID

        Return DataHelper.ExecuteNonQuery(db, sql)

    End Function

    Public Function RevokeGroupSecurity(ByVal GroupID As Long, ByVal TemplateID As Long) As Boolean

        Dim sql As String = "DELETE FROM CustomField_TemplateSecurity WHERE TemplateID = " & TemplateID & " AND GroupID = " & GroupID

        Return DataHelper.ExecuteNonQuery(db, sql)

    End Function

    Public Function GetPermittedUsers(ByVal TemplateID As Long, ByVal SecurityType As CustomFieldSecurityType) As DataSet

        Dim sql As String = ""

        sql &= "SELECT Users.[UserID] AS [ID], Users.Username + CASE WHEN CustomField_TemplateSecurity.EditAllowed > " & CInt(SecurityType) & " THEN ' [*]' ELSE '' END  AS [Name], 'User' AS UserType " & vbCrLf
        sql &= "FROM Users INNER JOIN CustomField_TemplateSecurity ON Users.[UserID] = CustomField_TemplateSecurity.[UserID] " & vbCrLf
        sql &= "WHERE CustomField_TemplateSecurity.TemplateID = " & TemplateID & " AND CustomField_TemplateSecurity.EditAllowed >= " & CInt(SecurityType) & " ORDER BY [Name]"

        Return DataHelper.ExecuteDataset(db, sql)

    End Function

    Public Function GetPermittedGroups(ByVal TemplateID As Long, ByVal SecurityType As CustomFieldSecurityType) As DataSet

        Dim sql As String = ""

        sql &= "SELECT Groups.[GroupID] AS [ID], Groups.GroupName  + CASE WHEN CustomField_TemplateSecurity.EditAllowed > " & CInt(SecurityType) & " THEN ' [*]' ELSE '' END  AS [Name], 'Group' AS UserType " & vbCrLf
        sql &= "FROM Groups INNER JOIN CustomField_TemplateSecurity ON Groups.[GroupID] = CustomField_TemplateSecurity.[GroupID] " & vbCrLf
        sql &= "WHERE CustomField_TemplateSecurity.TemplateID = " & TemplateID & " AND CustomField_TemplateSecurity.EditAllowed >= " & CInt(SecurityType) & " ORDER BY [Name]"

        Return DataHelper.ExecuteDataset(db, sql)

    End Function

    Public Function GetDeniedUsers(ByVal TemplateID As Long, ByVal SecurityType As CustomFieldSecurityType) As DataTable

        Dim sql As String = ""

        sql &= "SELECT Users.[UserID] AS [ID], Users.Username AS [Name], 'User' as [UserType] " & vbCrLf
        sql &= "FROM Users WHERE NOT EXISTS (SELECT CustomField_TemplateSecurity.TemplateSecurityID FROM CustomField_TemplateSecurity WHERE CustomField_TemplateSecurity.[UserID] = Users.[UserID] AND TemplateID = " & TemplateID & " AND CustomField_TemplateSecurity.EditAllowed = " & CInt(SecurityType) & ") ORDER BY [Name]"

        Return DataHelper.ExecuteDataTable(db, sql)

    End Function

    Public Function GetDeniedGroups(ByVal TemplateID As Long, ByVal SecurityType As CustomFieldSecurityType) As DataTable

        Dim sql As String = ""

        sql &= "SELECT Groups.[GroupID] AS [ID], Groups.GroupName AS [Name], 'Group' as [UserType] " & vbCrLf
        sql &= "FROM Groups WHERE NOT EXISTS (SELECT CustomField_TemplateSecurity.TemplateID FROM CustomField_TemplateSecurity WHERE CustomField_TemplateSecurity.[GroupID] = Groups.[GroupID] AND TemplateID = " & TemplateID & " AND CustomField_TemplateSecurity.EditAllowed = " & CInt(SecurityType) & ") ORDER BY [Name]"

        Return DataHelper.ExecuteDataTable(db, sql)

    End Function

#End Region

End Class
