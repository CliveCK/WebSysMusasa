Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class CommonFunctions

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

    Public ReadOnly Property Lookup(ByVal tableName As String, Optional ByVal valueColumn As String = "", Optional ByVal descriptionColumn As String = "", Optional ByVal orderColumn As String = "", Optional ByVal filterRows As String = "", Optional ByVal DISTINCT As Boolean = False, Optional ByVal CFdb As Database = Nothing) As System.Data.DataSet

        Get

            If IsNothing(CFdb) Then CFdb = db

            If valueColumn.Trim = "" Then valueColumn = "ID"
            If descriptionColumn.Trim = "" Then descriptionColumn = "Description"

            Dim strSQL As String

            If ((orderColumn = "") And (DISTINCT = False)) Then DISTINCT = True

            If DISTINCT Then
                strSQL = "SELECT DISTINCT " & valueColumn & IIf(valueColumn = descriptionColumn, "", ", [" & descriptionColumn & "]") & " FROM " & tableName & ""
            Else
                strSQL = "SELECT " & valueColumn & IIf(valueColumn = descriptionColumn, "", ", [" & descriptionColumn & "]") & " FROM " & tableName & ""
            End If

            If filterRows.Trim <> "" Then strSQL &= " WHERE " & filterRows

            If orderColumn.Trim <> "" Then
                strSQL &= " ORDER BY " & orderColumn
            Else
                strSQL &= " ORDER BY " & descriptionColumn
            End If

            Return CFdb.ExecuteDataSet(CommandType.Text, strSQL)

        End Get

    End Property

    Public ReadOnly Property GetStaffByType(ByVal StaffType As String) As DataSet
        Get
            Dim sql As String = "Select * from tblStaffMembers S inner join luStaffPosition P on S.PositionID = P.PositionID where P.Description = '" & StaffType & "'"

            Return db.ExecuteDataSet(CommandType.Text, sql)
        End Get
    End Property

    Public ReadOnly Property GetShelter() As DataSet
        Get
            Dim sql As String = "Select S.* from tblSubOffices S inner join tblOrganization O on O.OrganizationID = S.OrganizationID "
            sql &= "inner join luOrganizationTypes T on T.OrganizationTypeID = O.OrganizationTypeID where T.Description = 'Self'"

            Return db.ExecuteDataSet(CommandType.Text, sql)
        End Get
    End Property

    Public ReadOnly Property SqlExec(ByVal Sql As String) As DataSet
        Get

            Return db.ExecuteDataSet(CommandType.Text, Sql)

        End Get
    End Property

    Public ReadOnly Property Lookup2(ByVal tableName As String, Optional ByVal valueColumn As String = "", Optional ByVal descriptionColumn As String = "", Optional ByVal orderColumn As String = "", Optional ByVal filterRows As String = "", Optional ByVal DISTINCT As Boolean = False, Optional ByVal CFdb As Database = Nothing) As System.Data.DataSet

        Get

            If IsNothing(CFdb) Then CFdb = db

            If valueColumn.Trim = "" Then valueColumn = "ID"
            If descriptionColumn.Trim = "" Then descriptionColumn = "Description"

            Dim strSQL As String

            If ((orderColumn = "") And (DISTINCT = False)) Then DISTINCT = True

            If DISTINCT Then
                strSQL = "SELECT DISTINCT " & valueColumn & IIf(valueColumn = descriptionColumn, "", ", " & descriptionColumn & "") & " FROM " & tableName & ""
            Else
                strSQL = "SELECT " & valueColumn & IIf(valueColumn = descriptionColumn, "", ", " & descriptionColumn & "") & " FROM " & tableName & ""
            End If

            If filterRows.Trim <> "" Then strSQL &= " WHERE " & filterRows

            If orderColumn.Trim <> "" Then
                strSQL &= " ORDER BY " & orderColumn
            Else
                strSQL &= " ORDER BY " & descriptionColumn
            End If

            Return CFdb.ExecuteDataSet(CommandType.Text, strSQL)

        End Get

    End Property

End Class
