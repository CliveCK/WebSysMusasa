Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class UrbanArea

#Region "Variables"

    Private mCountryID As Integer
    Private mCountry As String
    Private mCityID As Integer
    Private mCity As String
    Private mSuburbID As Integer
    Private mSuburb As String
    Private mSectionID As Integer
    Private mSection As String
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

    Public Property CityID As Integer
        Get
            Return mCityID
        End Get
        Set(value As Integer)
            mCityID = value
        End Set
    End Property

    Public Property City As String
        Get
            Return mCity
        End Get
        Set(value As String)
            mCity = value
        End Set
    End Property

    Public Property SuburbID As Integer
        Get
            Return mSuburbID
        End Get
        Set(value As Integer)
            mSuburbID = value
        End Set
    End Property

    Public Property Suburb As String
        Get
            Return mSuburb
        End Get
        Set(value As String)
            mSuburb = value
        End Set
    End Property

    Public Property SectionID As Integer
        Get
            Return mSectionID
        End Get
        Set(value As Integer)
            mSectionID = value
        End Set
    End Property

    Public Property Section As String
        Get
            Return mSection
        End Get
        Set(value As String)
            mSection = value
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

            Case "City"
                sql = "INSERT INTO tblCities (Name, CountryID, CreatedBy, CreatedDate) VALUES ('" & mCity & "', " & mCountryID & ",1, getdate())"

            Case "Suburb"
                sql = "INSERT INTO tblSuburbs (Name, CityID, CreatedBy, CreatedDate) VALUES ('" & mSuburb & "', " & mCityID & " , 1, getdate())"

            Case "Section"
                sql = "INSERT INTO tblSection (Name, SuburbID, CreatedBy, CreatedDate) VALUES ('" & mSection & "', " & mSuburbID & ", 1, getdate())"

        End Select

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(Sql)

        Try

            db.ExecuteNonQuery(cmd)

            Return True

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

End Class
