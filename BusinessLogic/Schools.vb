Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Schools

#region "Variables"

    Protected mSchoolID As long
    Protected mWardID As Long
    Protected mProvinceID As Long
    Protected mDistrictID As Long
    Protected mSchoolTypeID As long
    Protected mStaffCompliment As long
    Protected mNoOfMaleStudents As long
    Protected mNoOfFemaleStudents As long
    Protected mTotalEnrollment As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mLongitude As decimal
    Protected mLatitude As decimal
    Protected mName As string
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

    Public  Property SchoolID() As long
        Get
		return mSchoolID
        End Get
        Set(ByVal value As long)
		mSchoolID = value
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
            Return mWardID
        End Get
        Set(ByVal value As Long)
            mWardID = value
        End Set
    End Property
    Public Property DistrictID() As Long
        Get
            Return mWardID
        End Get
        Set(ByVal value As Long)
            mWardID = value
        End Set
    End Property

    Public  Property SchoolTypeID() As long
        Get
		return mSchoolTypeID
        End Get
        Set(ByVal value As long)
		mSchoolTypeID = value
        End Set
    End Property

    Public  Property StaffCompliment() As long
        Get
		return mStaffCompliment
        End Get
        Set(ByVal value As long)
		mStaffCompliment = value
        End Set
    End Property

    Public  Property NoOfMaleStudents() As long
        Get
		return mNoOfMaleStudents
        End Get
        Set(ByVal value As long)
		mNoOfMaleStudents = value
        End Set
    End Property

    Public  Property NoOfFemaleStudents() As long
        Get
		return mNoOfFemaleStudents
        End Get
        Set(ByVal value As long)
		mNoOfFemaleStudents = value
        End Set
    End Property

    Public  Property TotalEnrollment() As long
        Get
		return mTotalEnrollment
        End Get
        Set(ByVal value As long)
		mTotalEnrollment = value
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

    SchoolID = 0
        mWardID = 0
    mSchoolTypeID = 0
    mStaffCompliment = 0
    mNoOfMaleStudents = 0
    mNoOfFemaleStudents = 0
    mTotalEnrollment = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mLongitude = 0.0
    mLatitude = 0.0
    mName = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mSchoolID) 

    End Function 

    Public Overridable Function Retrieve(ByVal SchoolID As Long) As Boolean 

        Dim sql As String 

        If SchoolID > 0 Then 
            sql = "SELECT C.*, D.DistrictID, P.ProvinceID FROM tblSchools C inner join tblWards W on W.WardID = C.WardID "
            sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
            sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID  WHERE SchoolID = " & SchoolID
        Else 
            sql = "SELECT * FROM tblSchools WHERE SchoolID = " & mSchoolID
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

                log.Warn("Schools not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function

    Public Function RetrieveAll() As DataSet

        Dim sql As String = "SELECT S.*, ST.Description As SchoolType FROM tblSchools S inner join luSchoolTypes ST on S.SchoolTypeID = ST.SchoolTypeID"

        Return GetSchools(sql)

    End Function

    Public Overridable Function GetSchools() As System.Data.DataSet

        Return GetSchools(mSchoolID) 

    End Function 

    Public Overridable Function GetSchools(ByVal SchoolID As Long) As DataSet 

        Dim sql As String 

        If SchoolID > 0 Then 
            sql = "SELECT * FROM tblSchools WHERE SchoolID = " & SchoolID
        Else 
            sql = "SELECT * FROM tblSchools WHERE SchoolID = " & mSchoolID
        End If 

        Return GetSchools(sql) 

    End Function 

    Protected Overridable Function GetSchools(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function 

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mSchoolID = Catchnull(.Item("SchoolID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mProvinceID = Catchnull(.Item("ProvinceID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mSchoolTypeID = Catchnull(.Item("SchoolTypeID"), 0)
            mStaffCompliment = Catchnull(.Item("StaffCompliment"), 0)
            mNoOfMaleStudents = Catchnull(.Item("NoOfMaleStudents"), 0)
            mNoOfFemaleStudents = Catchnull(.Item("NoOfFemaleStudents"), 0)
            mTotalEnrollment = Catchnull(.Item("TotalEnrollment"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mLongitude = Catchnull(.Item("Longitude"), 0.0)
            mLatitude = Catchnull(.Item("Latitude"), 0.0)
            mName = Catchnull(.Item("Name"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@SchoolID", DBType.Int32, mSchoolID) 
        db.AddInParameter(cmd, "@WardID", DBType.Int32, mWardID) 
        db.AddInParameter(cmd, "@SchoolTypeID", DBType.Int32, mSchoolTypeID) 
        db.AddInParameter(cmd, "@StaffCompliment", DBType.Int32, mStaffCompliment) 
        db.AddInParameter(cmd, "@NoOfMaleStudents", DBType.Int32, mNoOfMaleStudents) 
        db.AddInParameter(cmd, "@NoOfFemaleStudents", DBType.Int32, mNoOfFemaleStudents) 
        db.AddInParameter(cmd, "@TotalEnrollment", DBType.Int32, mTotalEnrollment) 
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID) 
        db.AddInParameter(cmd, "@Longitude", DbType.Decimal, mLongitude) 
        db.AddInParameter(cmd, "@Latitude", DbType.Decimal, mLatitude) 
        db.AddInParameter(cmd, "@Name", DBType.String, mName) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Schools") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mSchoolID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblSchools SET Deleted = 1 WHERE SchoolID = " & mSchoolID) 
        Return Delete("DELETE FROM tblSchools WHERE SchoolID = " & mSchoolID) 

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