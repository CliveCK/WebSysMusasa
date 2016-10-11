Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class LocationObjects

#Region "Variables"

    Protected mLocationObjectID As Long
    Protected mObjectID As Long
    Protected mObjectTypeID As Long
    Protected mCountryID As Long
    Protected mCityID As Long
    Protected mSurburbID As Long
    Protected mUpdatedBy As Long
    Protected mStreetID As Long
    Protected mProvinceID As Long
    Protected mDistrictID As Long
    Protected mWardID As Long
    Protected mSectionID As Long
    Protected mCreatedBy As Long
    Protected mCreatedDate As String
    Protected mUpdatedDate As String

    Protected db As Database
    Protected mConnectionName As String
    Protected mObjectUserID As Long

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Properties"

    Public ReadOnly Property Database() As Database
        Get
            Return db
        End Get
    End Property

    Public ReadOnly Property OwnerType() As String
        Get
            Return Me.GetType.Name
        End Get
    End Property

    Public ReadOnly Property ConnectionName() As String
        Get
            Return mConnectionName
        End Get
    End Property

    Public Property LocationObjectID() As Long
        Get
            Return mLocationObjectID
        End Get
        Set(ByVal value As Long)
            mLocationObjectID = value
        End Set
    End Property

    Public Property ObjectID() As Long
        Get
            Return mObjectID
        End Get
        Set(ByVal value As Long)
            mObjectID = value
        End Set
    End Property

    Public Property ObjectTypeID() As Long
        Get
            Return mObjectTypeID
        End Get
        Set(ByVal value As Long)
            mObjectTypeID = value
        End Set
    End Property

    Public Property CountryID() As Long
        Get
            Return mCountryID
        End Get
        Set(ByVal value As Long)
            mCountryID = value
        End Set
    End Property

    Public Property CityID() As Long
        Get
            Return mCityID
        End Get
        Set(ByVal value As Long)
            mCityID = value
        End Set
    End Property

    Public Property SurburbID() As Long
        Get
            Return mSurburbID
        End Get
        Set(ByVal value As Long)
            mSurburbID = value
        End Set
    End Property

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
        End Set
    End Property

    Public Property StreetID() As Long
        Get
            Return mStreetID
        End Get
        Set(ByVal value As Long)
            mStreetID = value
        End Set
    End Property

    Public Property ProvinceID() As Long
        Get
            Return mProvinceID
        End Get
        Set(ByVal value As Long)
            mProvinceID = value
        End Set
    End Property

    Public Property DistrictID() As Long
        Get
            Return mDistrictID
        End Get
        Set(ByVal value As Long)
            mDistrictID = value
        End Set
    End Property

    Public Property WardID() As Long
        Get
            Return mWardID
        End Get
        Set(ByVal value As Long)
            mWardID = value
        End Set
    End Property

    Public Property SectionID() As Long
        Get
            Return mSectionID
        End Get
        Set(ByVal value As Long)
            mSectionID = value
        End Set
    End Property

    Public Property CreatedBy() As Long
        Get
            Return mCreatedBy
        End Get
        Set(ByVal value As Long)
            mCreatedBy = value
        End Set
    End Property

    Public Property CreatedDate() As String
        Get
            Return mCreatedDate
        End Get
        Set(ByVal value As String)
            mCreatedDate = value
        End Set
    End Property

    Public Property UpdatedDate() As String
        Get
            Return mUpdatedDate
        End Get
        Set(ByVal value As String)
            mUpdatedDate = value
        End Set
    End Property

#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub

#End Region

    Public Sub Clear()

        LocationObjectID = 0
        mObjectID = 0
        mObjectTypeID = 0
        mCountryID = 0
        mCityID = 0
        mSurburbID = 0
        mUpdatedBy = 0
        mStreetID = 0
        mProvinceID = 0
        mDistrictID = 0
        mWardID = 0
        mSectionID = 0
        mCreatedBy = mObjectUserID
        mCreatedDate = ""
        mUpdatedDate = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mLocationObjectID)

    End Function

    Public Overridable Function Retrieve(ByVal LocationObjectID As Long) As Boolean

        Dim sql As String

        If LocationObjectID > 0 Then
            sql = "SELECT * FROM tblLocationObjects WHERE LocationObjectID = " & LocationObjectID
        Else
            sql = "SELECT * FROM tblLocationObjects WHERE LocationObjectID = " & mLocationObjectID
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

                log.Error("LocationObjects not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetLocationObjects() As System.Data.DataSet

        Return GetLocationObjects(mLocationObjectID)

    End Function

    Public Overridable Function GetLocationObjects(ByVal LocationObjectID As Long) As DataSet

        Dim sql As String

        If LocationObjectID > 0 Then
            sql = "SELECT * FROM tblLocationObjects WHERE LocationObjectID = " & LocationObjectID
        Else
            sql = "SELECT * FROM tblLocationObjects WHERE LocationObjectID = " & mLocationObjectID
        End If

        Return GetLocationObjects(sql)

    End Function

    Protected Overridable Function GetLocationObjects(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mLocationObjectID = Catchnull(.Item("LocationObjectID"), 0)
            mObjectID = Catchnull(.Item("ObjectID"), 0)
            mObjectTypeID = Catchnull(.Item("ObjectTypeID"), 0)
            mCountryID = Catchnull(.Item("CountryID"), 0)
            mCityID = Catchnull(.Item("CityID"), 0)
            mSurburbID = Catchnull(.Item("SurburbID"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mStreetID = Catchnull(.Item("StreetID"), 0)
            mProvinceID = Catchnull(.Item("ProvinceID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mSectionID = Catchnull(.Item("SectionID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@LocationObjectID", DBType.Int32, mLocationObjectID)
        db.AddInParameter(cmd, "@ObjectID", DBType.Int32, mObjectID)
        db.AddInParameter(cmd, "@ObjectTypeID", DBType.Int32, mObjectTypeID)
        db.AddInParameter(cmd, "@CountryID", DBType.Int32, mCountryID)
        db.AddInParameter(cmd, "@CityID", DBType.Int32, mCityID)
        db.AddInParameter(cmd, "@SurburbID", DBType.Int32, mSurburbID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@StreetID", DBType.Int32, mStreetID)
        db.AddInParameter(cmd, "@ProvinceID", DBType.Int32, mProvinceID)
        db.AddInParameter(cmd, "@DistrictID", DBType.Int32, mDistrictID)
        db.AddInParameter(cmd, "@WardID", DBType.Int32, mWardID)
        db.AddInParameter(cmd, "@SectionID", DBType.Int32, mSectionID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_LocationObjects")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mLocationObjectID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblLocationObjects SET Deleted = 1 WHERE LocationObjectID = " & mLocationObjectID) 
        Return Delete("DELETE FROM tblLocationObjects WHERE LocationObjectID = " & mLocationObjectID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

#End Region

#End Region

End Class