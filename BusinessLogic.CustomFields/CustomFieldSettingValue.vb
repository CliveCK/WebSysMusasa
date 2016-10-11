Imports Universal.CommonFunctions

Public Class CustomFieldSettingValue

    Public Sub New(ByVal tbl As DataTable)
        SettingsTable = tbl
    End Sub

    Public Function GetNumericSetting(ByVal SettingName As String, Optional ByVal SettingFieldName As String = "FieldValue1", Optional ByVal ValueIfNull As String = "") As Boolean

        NumericValue = 0

        If GetSetting(SettingName, SettingFieldName, ValueIfNull) Then

            If IsNumeric(SettingValue) Then
                NumericValue = CDec(SettingValue)
                Return True
            End If

        Else

            Return False

        End If

    End Function

    Public Function GetSetting(ByVal SettingName As String, Optional ByVal SettingFieldName As String = "FieldValue1", Optional ByVal ValueIfNull As String = "") As Boolean

        SettingValue = String.Empty

        If SettingsTable IsNot Nothing AndAlso SettingsTable.Rows.Count > 0 Then

            Dim settings() As DataRow = SettingsTable.Select(String.Format("{0} = '{1}'", SettingFieldName, SettingName))

            If settings.Length > 0 Then

                SettingValue = Catchnull(settings(0)(SettingName), ValueIfNull)
                Return True

            End If

        End If

        Return False

    End Function

    Public Property SettingsTable As DataTable
    Public Property SettingValue As String
    Public Property NumericValue As Decimal

End Class
