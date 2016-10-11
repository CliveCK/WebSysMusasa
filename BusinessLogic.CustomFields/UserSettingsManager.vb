
Class UserSettingsManager

    Private ConnectionName As String
    Private ObjectUserID As Long

    Sub New(ConnectionName As String, UserID As Long)

        Me.ConnectionName = ConnectionName
        ObjectUserID = UserID

    End Sub

    Function GetSetting(ServerGroup As String, UserID As Long, SettingGroup As String, SettingName As String, DefaultValue As Object) As String
        Return DefaultValue
    End Function

End Class
