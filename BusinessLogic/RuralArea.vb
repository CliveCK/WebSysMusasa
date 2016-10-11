Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions

Public Class RuralArea

#Region "Variables"

    Private mCountryID As Integer
    Private mCountry As String
    Private mProvinceID As Integer
    Private mProvince As String
    Private mDistrictID As Integer
    Private mTown As String
    Private mTownID As Long
    Private mDistrict As String
    Private mWardID As Integer
    Private mWard As String
    Private mVillageID As Integer
    Private mVillage As String
    Private mObjectUserID As Integer
    Private mConnectionName As String

    Private db As Database

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Properties"

    Public Property CountryID As Integer
        Get
            Return mCountryID
        End Get
        Set(value As Integer)
            mCountryID = value
        End Set
    End Property

    Public Property Country As String
        Get
            Return mCountry
        End Get
        Set(value As String)
            mCountry = value
        End Set
    End Property

    Public Property ProvinceID As Integer
        Get
            Return mProvinceID
        End Get
        Set(value As Integer)
            mProvinceID = value
        End Set
    End Property

    Public Property VillageID As Integer
        Get
            Return mVillageID
        End Get
        Set(value As Integer)
            mVillageID = value
        End Set
    End Property

    Public Property Village As String
        Get
            Return mVillage
        End Get
        Set(value As String)
            mVillage = value
        End Set
    End Property


    Public Property Province As String
        Get
            Return mProvince
        End Get
        Set(value As String)
            mProvince = value
        End Set
    End Property

    Public Property DistrictID As Integer
        Get
            Return mDistrictID
        End Get
        Set(value As Integer)
            mDistrictID = value
        End Set
    End Property

    Public Property District As String
        Get
            Return mDistrict
        End Get
        Set(value As String)
            mDistrict = value
        End Set
    End Property

    Public Property TownID As Integer
        Get
            Return mTownID
        End Get
        Set(value As Integer)
            mTownID = value
        End Set
    End Property

    Public Property Town As String
        Get
            Return mTown
        End Get
        Set(value As String)
            mTown = value
        End Set
    End Property

    Public Property WardID As Integer
        Get
            Return mWardID
        End Get
        Set(value As Integer)
            mWardID = value
        End Set
    End Property

    Public Property Ward As String
        Get
            Return mWard
        End Get
        Set(value As String)
            mWard = value
        End Set
    End Property

#End Region

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub

    Public Function Save(ByVal Target As String) As Boolean

        Dim sql As String = ""

        Select Case Target

            Case "Country"
                sql = "INSERT INTO luCountries (CountryName, CreatedBy, CreatedDate) VALUES ('" & mCountry & "', 1, getdate())"

            Case "Province"
                sql = "INSERT INTO tblProvinces (Name, CountryID, CreatedBy, CreatedDate) VALUES ('" & mProvince & "', " & mCountryID & ",1, getdate())"

            Case "District"
                sql = "INSERT INTO tblDistricts (Name, ProvinceID, CreatedBy, CreatedDate) VALUES ('" & mDistrict & "', " & mProvinceID & " , 1, getdate())"

            Case "Town"
                sql = "INSERT INTO tblTowns (Name, DistrictID, CreatedBy, CreatedDate) VALUES ('" & mTown & "', " & mDistrictID & " , 1, getdate())"

            Case "Ward"
                sql = "INSERT INTO tblWards (Name, DistrictID, TownID, CreatedBy, CreatedDate) VALUES ('" & mWard & "', " & mDistrictID & "," & mTownID & ", 1, getdate())"

            Case "Village"
                sql = "INSERT INTO tblVillages (Name, WardID, CreatedBy, CreatedDate) VALUES ('" & mVillage & "', " & mWardID & ", 1, getdate())"

        End Select

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

        Try

            db.ExecuteNonQuery(cmd)

            Return True

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

    Public Function CheckDistrictByName() As Boolean

        Return Catchnull(db.ExecuteDataSet(CommandType.Text, "Select * from tblDistricts where Name = '" & mDistrict & "'").Tables(0).Rows.Count > 0, False)

    End Function

    Public Function CheckProvinceByName() As Boolean

        Return Catchnull(db.ExecuteDataSet(CommandType.Text, "Select * from tblProvinces where Name = '" & mProvince & "'").Tables(0).Rows.Count > 0, False)

    End Function

End Class
