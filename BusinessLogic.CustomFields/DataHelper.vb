Public Class DataHelper

    Public Shared Property ErrorMessage As String
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Shared Function ExecuteDataset(ByRef db As Microsoft.Practices.EnterpriseLibrary.Data.Database, ByVal sql As String, Optional ByVal Name As String = "") As DataSet

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            If Not String.IsNullOrWhiteSpace(Name) Then ds.Tables(0).TableName = Name
            Return ds
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function ExecuteNonQuery(ByRef db As Microsoft.Practices.EnterpriseLibrary.Data.Database, ByVal sql As String, Optional ByVal Name As String = "") As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, sql)
            Return True

        Catch ex As Exception
            ErrorMessage = ex.Message
            Log.Error(sql, ex)
            Return False
        End Try

    End Function

    Public Shared Function ExecuteDataTable(ByRef db As Microsoft.Practices.EnterpriseLibrary.Data.Database, ByVal sql As String, Optional ByVal Name As String = "") As DataTable

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            If Not String.IsNullOrWhiteSpace(Name) Then ds.Tables(0).TableName = Name
            Return ds.Tables(0)
        Else
            Return Nothing
        End If

    End Function

End Class
