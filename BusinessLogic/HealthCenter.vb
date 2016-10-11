Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class HealthCenter

#region "Variables"

    Protected mHealthCenterID As long
    Protected mWardID As Long
    Protected mProvinceID As Long
    Protected mDistrictID As Long
    Protected mHealthCenterTypeID As long
    Protected mNoOfDoctors As long
    Protected mNoOfNurses As long
    Protected mNoOfBeds As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As String
    Protected mHasAmbulance As Boolean
    Protected mHasMotherShelter As Boolean
    Protected mLongitude As decimal
    Protected mLatitude As decimal
    Protected mName As string
    Protected mDescription As string
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected db As Database 
    Protected mConnectionName As String 
    Protected mObjectUserID As Long 

#end region

#region "Properties"

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

    Public  Property HealthCenterID() As long
        Get
		return mHealthCenterID
        End Get
        Set(ByVal value As long)
		mHealthCenterID = value
        End Set
    End Property

    Public  Property WardID() As long
        Get
		return mWardID
        End Get
        Set(ByVal value As long)
		mWardID = value
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

    Public  Property HealthCenterTypeID() As long
        Get
		return mHealthCenterTypeID
        End Get
        Set(ByVal value As long)
		mHealthCenterTypeID = value
        End Set
    End Property

    Public  Property NoOfDoctors() As long
        Get
		return mNoOfDoctors
        End Get
        Set(ByVal value As long)
		mNoOfDoctors = value
        End Set
    End Property

    Public  Property NoOfNurses() As long
        Get
		return mNoOfNurses
        End Get
        Set(ByVal value As long)
		mNoOfNurses = value
        End Set
    End Property

    Public  Property NoOfBeds() As long
        Get
		return mNoOfBeds
        End Get
        Set(ByVal value As long)
		mNoOfBeds = value
        End Set
    End Property

    Public  Property CreatedBy() As long
        Get
		return mCreatedBy
        End Get
        Set(ByVal value As long)
		mCreatedBy = value
        End Set
    End Property

    Public  Property UpdatedBy() As long
        Get
		return mUpdatedBy
        End Get
        Set(ByVal value As long)
		mUpdatedBy = value
        End Set
    End Property

    Public  Property CreatedDate() As string
        Get
		return mCreatedDate
        End Get
        Set(ByVal value As string)
		mCreatedDate = value
        End Set
    End Property

    Public  Property UpdatedDate() As string
        Get
		return mUpdatedDate
        End Get
        Set(ByVal value As string)
		mUpdatedDate = value
        End Set
    End Property

    Public Property HasAmbulance() As Boolean
        Get
            Return mHasAmbulance
        End Get
        Set(ByVal value As Boolean)
            mHasAmbulance = value
        End Set
    End Property

    Public Property HasMotherShelter() As Boolean
        Get
            Return mHasMotherShelter
        End Get
        Set(ByVal value As Boolean)
            mHasMotherShelter = value
        End Set
    End Property

    Public  Property Longitude() As decimal
        Get
		return mLongitude
        End Get
        Set(ByVal value As decimal)
		mLongitude = value
        End Set
    End Property

    Public  Property Latitude() As decimal
        Get
		return mLatitude
        End Get
        Set(ByVal value As decimal)
		mLatitude = value
        End Set
    End Property

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
        End Set
    End Property

    Public  Property Description() As string
        Get
		return mDescription
        End Get
        Set(ByVal value As string)
		mDescription = value
        End Set
    End Property

#end region

#region "Methods"

#Region "Constructors" 
 
    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long) 

        mObjectUserID = ObjectUserID 
        mConnectionName = ConnectionName 
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub 

#End Region 

Public Sub Clear()  

    HealthCenterID = 0
    mWardID = 0
    mHealthCenterTypeID = 0
    mNoOfDoctors = 0
    mNoOfNurses = 0
    mNoOfBeds = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
        mHasAmbulance = False
        mHasMotherShelter = False
        mLongitude = 0.0
    mLatitude = 0.0
    mName = ""
    mDescription = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mHealthCenterID) 

    End Function 

    Public Overridable Function Retrieve(ByVal HealthCenterID As Long) As Boolean 

        Dim sql As String 

        If HealthCenterID > 0 Then 
            sql = "SELECT * FROM tblHealthCenters H inner join tblWards W on W.WardID  = H.WardID "
            sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
            sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID WHERE HealthCenterID = " & HealthCenterID
        Else 
            sql = "SELECT * FROM tblHealthCenters H inner join tblWards W on W.WardID  = H.WardID "
            sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
            sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID WHERE HealthCenterID = " & mHealthCenterID
        End If 

        Return Retrieve(sql) 

    End Function

    Public Function RetrieveAll(ByVal Criteria As String) As DataSet

        Dim sql As String = "SELECT H.*, P.Name As Province, D.Name as District, D.DistrictID, W.WardID FROM tblHealthCenters H "
        sql &= "inner join tblWards W on W.WardID = H.WardID "
        sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
        sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID  " & Criteria

        Return GetHealthCenter(sql)

    End Function

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean 

        Try 

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql) 

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then 

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0)) 

                dsRetrieve = Nothing 
                Return True 

            Else 

                log.Warn("HealthCenter not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetHealthCenter() As System.Data.DataSet

        Return GetHealthCenter(mHealthCenterID) 

    End Function 

    Public Overridable Function GetHealthCenter(ByVal HealthCenterID As Long) As DataSet 

        Dim sql As String 

        If HealthCenterID > 0 Then 
            sql = "SELECT * FROM tblHealthCenters WHERE HealthCenterID = " & HealthCenterID
        Else 
            sql = "SELECT * FROM tblHealthCenters WHERE HealthCenterID = " & mHealthCenterID
        End If 

        Return GetHealthCenter(sql) 

    End Function

    Public Overridable Function GetHealthCenter(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetCustomFieldsObjectIDByCode(ByVal Code As String) As Long

        Dim sql As String = "SELECT ObjectID FROM tblCustomFields_Objects WHERE Code = '" & Code & "'"

        Return db.ExecuteDataSet(CommandType.Text, sql).Tables(0).Rows(0)(0)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mHealthCenterID = Catchnull(.Item("HealthCenterID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mProvinceID = Catchnull(.Item("ProvinceID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mHealthCenterTypeID = Catchnull(.Item("HealthCenterTypeID"), 0)
            mNoOfDoctors = Catchnull(.Item("NoOfDoctors"), 0)
            mNoOfNurses = Catchnull(.Item("NoOfNurses"), 0)
            mNoOfBeds = Catchnull(.Item("NoOfBeds"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mHasAmbulance = Catchnull(.Item("HasAmbulance"), False)
            mHasMotherShelter = Catchnull(.Item("HasMotherShelter"), False)
            mLongitude = Catchnull(.Item("Longitude"), 0.0)
            mLatitude = Catchnull(.Item("Latitude"), 0.0)
            mName = Catchnull(.Item("Name"), "")
            mDescription = Catchnull(.Item("Description"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@HealthCenterID", DBType.Int32, mHealthCenterID) 
        db.AddInParameter(cmd, "@WardID", DBType.Int32, mWardID) 
        db.AddInParameter(cmd, "@HealthCenterTypeID", DBType.Int32, mHealthCenterTypeID) 
        db.AddInParameter(cmd, "@NoOfDoctors", DBType.Int32, mNoOfDoctors) 
        db.AddInParameter(cmd, "@NoOfNurses", DBType.Int32, mNoOfNurses) 
        db.AddInParameter(cmd, "@NoOfBeds", DBType.Int32, mNoOfBeds) 
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@HasAmbulance", DbType.Boolean, mHasAmbulance)
        db.AddInParameter(cmd, "@HasMotherShelter", DbType.Boolean, mHasMotherShelter)
        db.AddInParameter(cmd, "@Longitude", DbType.Decimal, mLongitude) 
        db.AddInParameter(cmd, "@Latitude", DbType.Decimal, mLatitude) 
        db.AddInParameter(cmd, "@Name", DBType.String, mName) 
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_HealthCenter") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mHealthCenterID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblHealthCenters SET Deleted = 1 WHERE HealthCenterID = " & mHealthCenterID) 
        Return Delete("DELETE FROM tblHealthCenters WHERE HealthCenterID = " & mHealthCenterID) 

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

#end region

End Class