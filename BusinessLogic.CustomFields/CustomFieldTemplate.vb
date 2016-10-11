Imports Universal.CommonFunctions
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class CustomFieldTemplate

#Region "Variables"

    Private ObjectUserID As Long
    Private ConnectionName As String
    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Properties"

    Public Property ErrorMessage As String

    Public Property TemplateID() As Long
    Public Property CreatedDate() As String
    Public Property TemplateName() As String
    Public Property Comments() As String

#End Region

#Region "Methods"

#Region "Constructors"

    Sub New(ByVal ConnectionName As String, UserID As Long)

        Me.ConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)
        Me.ObjectUserID = UserID

    End Sub

#End Region

    Property TemplateType As String

    Public Sub Clear()

        TemplateID = 0
        CreatedDate = ""
        TemplateName = ""
        Comments = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(TemplateID)

    End Function

    Public Overridable Function RetrieveByName(ByVal TemplateName As String) As Boolean

        Dim sql As String = "SELECT * FROM CustomField_Templates WHERE TemplateName = '" & TemplateName.Replace("'", "''") & "'"

        Return Retrieve(sql)

    End Function

    Public Overridable Function Retrieve(ByVal TemplateID As Long) As Boolean

        Dim sql As String

        If TemplateID > 0 Then
            sql = "SELECT * FROM CustomField_Templates WHERE TemplateID = " & TemplateID
        Else
            sql = "SELECT * FROM CustomField_Templates WHERE TemplateID = " & TemplateID
        End If

        Return Retrieve(sql)

    End Function

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean

        Try

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0))

                dsRetrieve = Nothing
                Return True

            Else

                ErrorMessage = "CustomFieldTemplate not found."
                Log.Debug(ErrorMessage)
                Return False

            End If

        Catch e As Exception

            ErrorMessage = e.Message
            Log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCustomFieldTemplate() As System.Data.DataSet

        Return GetCustomFieldTemplate(TemplateID)

    End Function

    Public Overridable Function GetCustomFieldTemplate(ByVal TemplateID As Long) As DataSet

        Dim sql As String

        If TemplateID > 0 Then
            sql = "SELECT * FROM CustomField_Templates WHERE TemplateID = " & TemplateID
        Else
            sql = "SELECT * FROM CustomField_Templates WHERE TemplateID = " & TemplateID
        End If

        Return GetCustomFieldTemplate(sql)

    End Function

    Protected Overridable Function GetCustomFieldTemplate(ByVal sql As String) As DataSet

        Return DataHelper.ExecuteDataset(db, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            TemplateID = Catchnull(.Item("TemplateID"), 0)
            CreatedDate = Catchnull(.Item("CreatedDate"), "")
            TemplateName = Catchnull(.Item("TemplateName"), "")
            Comments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Function Save() As Boolean

        Dim sql As String = String.Empty

        If String.IsNullOrEmpty(Comments) Then Comments = String.Empty
        If String.IsNullOrEmpty(TemplateName) Then TemplateName = String.Empty

        If TemplateID = 0 Then
            sql &= "INSERT INTO [CustomField_Templates]([TemplateName], [TemplateType], [Comments], CreatedDate, CreatedBy) VALUES ('" & TemplateName.Replace("'", "''") & "', '" & TemplateType.Replace("'", "''") & "', '" & Comments.Replace("'", "''") & "', getdate(), " & ObjectUserID & "); SELECT SCOPE_IDENTITY();" & vbCrLf
        Else
            sql &= "UPDATE [CustomField_Templates] SET [TemplateName]='" & TemplateName.Replace("'", "''") & "', [Comments]='" & Comments.Replace("'", "''") & "' WHERE [TemplateID]=" & TemplateID & "; SELECT " & TemplateID & vbCrLf
        End If

        Try

            Dim ds As DataSet = DataHelper.ExecuteDataset(db, sql)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                TemplateID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            Log.Error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE CustomField_Templates SET Deleted = 1 WHERE TemplateID = " & mTemplateID) 
        Return Delete("DELETE FROM CustomField_Templates WHERE TemplateID = " & TemplateID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            ErrorMessage = e.Message
            Log.Error(e)
            Return False

        End Try

    End Function

#End Region

#End Region

End Class