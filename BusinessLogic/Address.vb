Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions

Public Class Address

#Region "Variable"

    Protected mAddressID As Long
    Protected mSectionID As Long
    Protected mVillageID As Long
    Protected mStreetID As Long
    Protected mCreatedBy As Long
    Protected mUpdatedBy As Long
    Protected mOwnerID As Long
    Protected mDistrictID As Long
    Protected mWardID As Long
    Protected mCityID As Long
    Protected mSurburbID As Long
    Protected mCreatedDate As String
    Protected mUpdatedDate As String
    Protected mIsUrban As Boolean
    Protected mName As String
    Protected mAddress As String
    Protected mSerialNo As String

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

    Public Property AddressID() As Long
        Get
            Return mAddressID
        End Get
        Set(ByVal value As Long)
            mAddressID = value
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

    Public Property VillageID() As Long
        Get
            Return mVillageID
        End Get
        Set(ByVal value As Long)
            mVillageID = value
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

    Public Property CreatedBy() As Long
        Get
            Return mCreatedBy
        End Get
        Set(ByVal value As Long)
            mCreatedBy = value
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

    Public Property OwnerID() As Long
        Get
            Return mOwnerID
        End Get
        Set(ByVal value As Long)
            mOwnerID = value
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

    Public Property IsUrban() As Boolean
        Get
            Return mIsUrban
        End Get
        Set(ByVal value As Boolean)
            mIsUrban = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return mAddress
        End Get
        Set(ByVal value As String)
            mAddress = value
        End Set
    End Property

    Public Property SerialNo() As String
        Get
            Return mSerialNo
        End Get
        Set(ByVal value As String)
            mSerialNo = value
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

        AddressID = 0
        mSectionID = 0
        mVillageID = 0
        mStreetID = 0
        mDistrictID = 0
        mWardID = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mOwnerID = 0
        mCreatedDate = ""
        mUpdatedDate = ""
        mIsUrban = False
        mName = ""
        mAddress = ""
        mSerialNo = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mAddressID)

    End Function

    Public Overridable Function Retrieve(ByVal OwnerID As Long) As Boolean

        Dim sql As String

        sql = "SELECT * FROM tblAddresses WHERE OwnerID = " & OwnerID

        Return Retrieve(sql)

    End Function

    Public Function RetrieveByPatient(ByVal PatientID As Long) As Boolean

        Dim sql As String

        If PatientID > 0 Then
            sql = "SELECT A.*, W.WardID, D.DistrictID, C.CityID, S1.SuburbID FROM tblAddresses A left outer join tblVillages V on V.VillageID = A.VillageID "
            sql &= "left outer join tblWards W on W.WardID = V.WardID "
            sql &= "left outer join tblDistricts D on D.DistrictID = W.DistrictID "
            sql &= "left outer join tblStreets SR on SR.StreetID = A.StreetID"
            sql &= "left outer join tblSection S on S.SectionID = SR.SectionID "
            sql &= "left outer join tblSuburbs S1 on S1.SuburbID = S.SuburbID "
            sql &= "left outer join tblCities C on C.CityID = S1.CityID WHERE OwnerID = " & PatientID
        Else
            sql = "SELECT * FROM tblAddresses WHERE OwnerID = " & mOwnerID
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

                log.error("Address not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetAddress() As System.Data.DataSet

        Return GetAddress(mAddressID)

    End Function

    Public Overridable Function GetAddress(ByVal AddressID As Long) As DataSet

        Dim sql As String

        If AddressID > 0 Then
            sql = "SELECT * FROM tblAddresses WHERE AddressID = " & AddressID
        Else
            sql = "SELECT * FROM tblAddresses WHERE AddressID = " & mAddressID
        End If

        Return GetAddress(sql)

    End Function

    Protected Overridable Function GetAddress(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mAddressID = Catchnull(.Item("AddressID"), 0)
            mSectionID = Catchnull(.Item("SectionID"), 0)
            mVillageID = Catchnull(.Item("VillageID"), 0)
            mStreetID = Catchnull(.Item("StreetID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mOwnerID = Catchnull(.Item("OwnerID"), 0)
            mCityID = Catchnull(.Item("CityID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mSurburbID = Catchnull(.Item("SuburbID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mIsUrban = Catchnull(.Item("IsUrban"), False)
            mName = Catchnull(.Item("Name"), "")
            mAddress = Catchnull(.Item("Address"), "")
            mSerialNo = Catchnull(.Item("SerialNo"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@AddressID", DBType.Int32, mAddressID)
        db.AddInParameter(cmd, "@SectionID", DBType.Int32, mSectionID)
        db.AddInParameter(cmd, "@VillageID", DBType.Int32, mVillageID)
        db.AddInParameter(cmd, "@StreetID", DbType.Int32, mStreetID)
        db.AddInParameter(cmd, "@DistrictID", DbType.Int32, mDistrictID)
        db.AddInParameter(cmd, "@WardID", DbType.Int32, mWardID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@OwnerID", DBType.Int32, mOwnerID)
        db.AddInParameter(cmd, "@IsUrban", DBType.Boolean, mIsUrban)
        db.AddInParameter(cmd, "@Name", DBType.String, mName)
        db.AddInParameter(cmd, "@Address", DBType.String, mAddress)
        db.AddInParameter(cmd, "@SerialNo", DBType.String, mSerialNo)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Address")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mAddressID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            log.error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblAddresses SET Deleted = 1 WHERE AddressID = " & mAddressID) 
        Return Delete("DELETE FROM tblAddresses WHERE AddressID = " & mAddressID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

#End Region

#End Region

End Class