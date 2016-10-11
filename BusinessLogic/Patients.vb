Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Patients

#Region "Variables"

    Protected mPatientID As Long
    Protected mPatientNo As String
    Protected mReligionID As Long
    Protected mMedicalAidTypeID As Long
    Protected mStatusID As Long
    Protected mOrphanhoodID As long
    Protected mClosestHealthCenterID As long
    Protected mSchoolID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As Long
    Protected mDOB As String
    Protected mDOD As String
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mFirstName As string
    Protected mSurname As string
    Protected mMaritalStatus As String
    Protected mSex As String
    Protected mTelephone As String

    Protected db As Database 
    Protected mConnectionName As String 
    Protected mObjectUserID As Long

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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

    Public Property PatientID() As Long
        Get
            Return mPatientID
        End Get
        Set(ByVal value As Long)
            mPatientID = value
        End Set
    End Property

    Public Property PatientNo() As String
        Get
            Return mPatientNo
        End Get
        Set(ByVal value As String)
            mPatientNo = value
        End Set
    End Property

    Public  Property ReligionID() As long
        Get
		return mReligionID
        End Get
        Set(ByVal value As long)
		mReligionID = value
        End Set
    End Property

    Public Property MedicalAidTypeID() As Long
        Get
            Return mMedicalAidTypeID
        End Get
        Set(ByVal value As Long)
            mMedicalAidTypeID = value
        End Set
    End Property

    Public Property StatusID() As Long
        Get
            Return mStatusID
        End Get
        Set(ByVal value As Long)
            mStatusID = value
        End Set
    End Property

    Public  Property OrphanhoodID() As long
        Get
		return mOrphanhoodID
        End Get
        Set(ByVal value As long)
		mOrphanhoodID = value
        End Set
    End Property

    Public  Property ClosestHealthCenterID() As long
        Get
		return mClosestHealthCenterID
        End Get
        Set(ByVal value As long)
		mClosestHealthCenterID = value
        End Set
    End Property

    Public  Property SchoolID() As long
        Get
		return mSchoolID
        End Get
        Set(ByVal value As long)
		mSchoolID = value
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

    Public Property DOB() As String
        Get
            Return mDOB
        End Get
        Set(ByVal value As String)
            mDOB = value
        End Set
    End Property

    Public Property DOD() As String
        Get
            Return mDOD
        End Get
        Set(ByVal value As String)
            mDOD = value
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

    Public  Property FirstName() As string
        Get
		return mFirstName
        End Get
        Set(ByVal value As string)
		mFirstName = value
        End Set
    End Property

    Public  Property Surname() As string
        Get
		return mSurname
        End Get
        Set(ByVal value As string)
		mSurname = value
        End Set
    End Property

    Public  Property MaritalStatus() As string
        Get
		return mMaritalStatus
        End Get
        Set(ByVal value As string)
		mMaritalStatus = value
        End Set
    End Property

    Public Property Sex() As String
        Get
            Return mSex
        End Get
        Set(ByVal value As String)
            mSex = value
        End Set
    End Property

    Public Property Telephone() As String
        Get
            Return mTelephone
        End Get
        Set(ByVal value As String)
            mTelephone = value
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

        PatientID = 0
        PatientNo = ""
        mReligionID = 0
        mMedicalAidTypeID = 0
        mStatusID = 0
        mOrphanhoodID = 0
    mClosestHealthCenterID = 0
    mSchoolID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
        mDOB = ""
        mDOD = ""
        mCreatedDate = ""
    mUpdatedDate = ""
    mFirstName = ""
    mSurname = ""
    mMaritalStatus = ""
        mSex = ""
        mTelephone = ""

    End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mPatientID) 

    End Function 

    Public Overridable Function Retrieve(ByVal PatientID As Long) As Boolean 

        Dim sql As String 

        If PatientID > 0 Then 
            sql = "SELECT * FROM tblPatients WHERE PatientID = " & PatientID
        Else 
            sql = "SELECT * FROM tblPatients WHERE PatientID = " & mPatientID
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

                log.error("Patients not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetPatients() As System.Data.DataSet

        Return GetPatients(mPatientID)

    End Function

    Public Function GetAllPatients() As DataSet

        Dim sql As String

        sql = "select P.*, D.Description As Diagnosis, PS.Description As PatientStatus,R.Description As Religion, O.Description As Orphanhood, 
                PS.Description as PatientStatus, E.Priority, H.Name As ClosestHealthCenter, M.Description As MedicalAidType "
        sql &= " from tblPatients P "
        sql &= "left outer join luReligion R on P.ReligionID = R.ReligionID "
        sql &= "left outer join luMedicalAidTypes M on M.MedicalAidTypeID = P.MedicalAidTypeID "
        sql &= "left outer join tblOrphanhood O on O.ObjectID = P.OrphanhoodID "
        sql &= "left outer join tblEligibility E on E.PatientID = P.PatientID "
        sql &= "left outer join tblHealthCenters H on H.HealthCenterID = P.ClosestHealthCenterID "
        sql &= "left outer join luPatientStatus PS on PS.PatientStatusID = P.StatusID "
        sql &= "left outer join tblMedicalHistory MD on MD.PatientID = P.PatientID "
        sql &= "left outer join luDiagnosis D on MD.Diagnosis = D.DiagnosisID"

        Return GetPatients(sql)

    End Function

    Public Overridable Function GetPatients(ByVal PatientID As Long) As DataSet

        Dim sql As String

        If PatientID > 0 Then
            sql = "SELECT * FROM tblPatients WHERE PatientID = " & PatientID
        Else
            sql = "SELECT * FROM tblPatients WHERE PatientID = " & mPatientID
        End If

        Return GetPatients(sql)

    End Function

    Protected Overridable Function GetPatients(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mPatientID = Catchnull(.Item("PatientID"), 0)
            mPatientNo = Catchnull(.Item("PatientNo"), 0)
            mReligionID = Catchnull(.Item("ReligionID"), 0)
            mMedicalAidTypeID = Catchnull(.Item("MedicalAidTypeID"), 0)
            mStatusID = Catchnull(.Item("StatusID"), 0)
            mOrphanhoodID = Catchnull(.Item("OrphanhoodID"), 0)
            mClosestHealthCenterID = Catchnull(.Item("ClosestHealthCenterID"), 0)
            mSchoolID = Catchnull(.Item("SchoolID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDOB = Catchnull(.Item("DOB"), "")
            mDOD = Catchnull(.Item("DOD"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mFirstName = Catchnull(.Item("FirstName"), "")
            mSurname = Catchnull(.Item("Surname"), "")
            mMaritalStatus = Catchnull(.Item("MaritalStatus"), "")
            mSex = Catchnull(.Item("Sex"), "")
            mTelephone = Catchnull(.Item("TelephoneNo"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@PatientID", DbType.Int32, mPatientID)
        db.AddInParameter(cmd, "@PatientNo", DbType.String, mPatientNo)
        db.AddInParameter(cmd, "@ReligionID", DBType.Int32, mReligionID)
        db.AddInParameter(cmd, "@MedicalAidTypeID", DbType.Int32, mMedicalAidTypeID)
        db.AddInParameter(cmd, "@StatusID", DbType.Int32, mStatusID)
        db.AddInParameter(cmd, "@OrphanhoodID", DBType.Int32, mOrphanhoodID)
        db.AddInParameter(cmd, "@ClosestHealthCenterID", DBType.Int32, mClosestHealthCenterID)
        db.AddInParameter(cmd, "@SchoolID", DBType.Int32, mSchoolID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DOB", DbType.String, mDOB)
        db.AddInParameter(cmd, "@DOD", DbType.String, mDOD)
        db.AddInParameter(cmd, "@FirstName", DBType.String, mFirstName)
        db.AddInParameter(cmd, "@Surname", DBType.String, mSurname)
        db.AddInParameter(cmd, "@MaritalStatus", DBType.String, mMaritalStatus)
        db.AddInParameter(cmd, "@Sex", DbType.String, mSex)
        db.AddInParameter(cmd, "@TelephoneNo", DbType.String, mTelephone)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Patients")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mPatientID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblPatients SET Deleted = 1 WHERE PatientID = " & mPatientID) 
        Return Delete("DELETE FROM tblPatients WHERE PatientID = " & mPatientID)

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

#end region

End Class