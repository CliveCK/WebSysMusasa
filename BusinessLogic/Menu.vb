Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class Menu

    Protected db As Database
    Protected mConnectionName As String
    Protected mObjectUserID As Long

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub

    Public Function GetContextMenu() As DataSet

        Dim sql As String

        sql = "SELECT * FROM luMenu where MenuType = 'Context'"
        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

End Class
