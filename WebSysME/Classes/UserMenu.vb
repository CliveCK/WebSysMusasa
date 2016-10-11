Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class UserMenu

#Region "Variables"

    Private db As Database
    Private mConnectionName As String

#End Region

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String)

        mConnectionName = ConnectionName
        Dim factory As DatabaseProviderFactory = New DatabaseProviderFactory()
        db = factory.Create(ConnectionName)

    End Sub


#End Region

#Region "Methods"

    Public Function GetContextMenu(ByVal ContextMenu As String, ByVal UseZeroParentValues As Boolean) As DataSet

        Dim sql As String = "SELECT [MenuID], [MenuName], " & _
        IIf(UseZeroParentValues, "ISNULL([ParentID],0)", "[ParentID]") & _
        " AS [ParentID], [URL], [DrawMenu], [MenuType], [OrderIndex] FROM luMenu WHERE [DrawMenu] = 1 AND [MenuType] = " & "'" & ContextMenu & "' ORDER BY ParentID, OrderIndex"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetContextMenu(ByVal UseZeroParentValues As Boolean) As DataSet

        Dim sql As String = "SELECT [MenuID], [MenuName], " & _
        IIf(UseZeroParentValues, "ISNULL([ParentID],0)", "[ParentID]") & _
        " AS [ParentID], [URL], [DrawMenu], [MenuType], [OrderIndex] FROM luMenu WHERE [DrawMenu] = 1 ORDER BY ParentID, OrderIndex"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetContextMenu(ByVal UseZeroParentValues As Boolean, ByVal ExcludeContextMenuCriteria As String) As DataSet

        Dim sql As String = "SELECT [MenuID], [MenuName], " & _
        IIf(UseZeroParentValues, "ISNULL([ParentID],0)", "[ParentID]") & _
        " AS [ParentID], [URL], [DrawMenu], [MenuType], [OrderIndex] FROM luMenu WHERE [DrawMenu] = 1 "

        Dim Criteria As String() = ExcludeContextMenuCriteria.Split(",")

        If Criteria.Length > 0 Then
            ExcludeContextMenuCriteria = ""
            For i As Long = 0 To Criteria.Length - 1

                ExcludeContextMenuCriteria &= "'" & Criteria(i) & "'"

            Next

            sql &= " AND ContextMenu NOT IN (" & ExcludeContextMenuCriteria & ") "

        End If

        sql &= " ORDER BY ParentID, OrderIndex"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    ' This will get all menu items that are in lumenu irrespective of whether they should be drawn on the main menu or not
    Public Function GetAllContextMenu(ByVal UseZeroParentValues As Boolean) As DataSet

        Dim sql As String = "SELECT [MenuID], [MenuName], " & _
        IIf(UseZeroParentValues, "ISNULL([ParentID],0)", "[ParentID]") & _
        " AS [ParentID], [URL],[DrawMenu], [ContextMenu], [OrderIndex] FROM luMenu ORDER BY ParentID, OrderIndex"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

End Class
