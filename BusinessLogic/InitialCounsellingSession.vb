Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class InitialCounsellingSession

#region "Variables"

    Protected mInitialCounsellingSessionID As Long
    Protected mBeneficiaryID As Long
    Protected mEmploymentStatus As String
    Protected mNextOfKin As String
    Protected mReferredBy As String
    Protected mProblemCategoryID As long
    Protected mHowManyTimes As long
    Protected mLawyerID As long
    Protected mShelterID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mSessionDate As string
    Protected mNextAppointmentDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mReferredToLaywer As boolean
    Protected mReferredToShelter As boolean
    Protected mPresentingProblem As string
    Protected mOther As string
    Protected mCaseReported As string
    Protected mWhereWasProblemReported As string
    Protected mChallengesFaced As string
    Protected mMedicalReport As string
    Protected mDurationOfSession As string
    Protected mIssuedBy As string
    Protected mProblemsExperienced As string
    Protected mClientExpectations As string
    Protected mOtherOptionAvailable As string
    Protected mReferral As string
    Protected mCarePlan As string

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

    Public  Property InitialCounsellingSessionID() As long
        Get
		return mInitialCounsellingSessionID
        End Get
        Set(ByVal value As long)
		mInitialCounsellingSessionID = value
        End Set
    End Property

    Public Property BeneficiaryID() As Long
        Get
            Return mBeneficiaryID
        End Get
        Set(ByVal value As Long)
            mBeneficiaryID = value
        End Set
    End Property

    Public  Property ProblemCategoryID() As long
        Get
		return mProblemCategoryID
        End Get
        Set(ByVal value As long)
		mProblemCategoryID = value
        End Set
    End Property

    Public  Property HowManyTimes() As long
        Get
		return mHowManyTimes
        End Get
        Set(ByVal value As long)
		mHowManyTimes = value
        End Set
    End Property

    Public  Property LawyerID() As long
        Get
		return mLawyerID
        End Get
        Set(ByVal value As long)
		mLawyerID = value
        End Set
    End Property

    Public  Property ShelterID() As long
        Get
		return mShelterID
        End Get
        Set(ByVal value As long)
		mShelterID = value
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

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
        End Set
    End Property

    Public Property EmploymentStatus() As String
        Get
            Return mEmploymentStatus
        End Get
        Set(ByVal value As String)
            mEmploymentStatus = value
        End Set
    End Property

    Public Property NextOfkin() As String
        Get
            Return mNextOfKin
        End Get
        Set(ByVal value As String)
            mNextOfKin = value
        End Set
    End Property

    Public Property ReferredBy() As String
        Get
            Return mReferredBy
        End Get
        Set(ByVal value As String)
            mReferredBy = value
        End Set
    End Property

    Public  Property SessionDate() As string
        Get
		return mSessionDate
        End Get
        Set(ByVal value As string)
		mSessionDate = value
        End Set
    End Property

    Public  Property NextAppointmentDate() As string
        Get
		return mNextAppointmentDate
        End Get
        Set(ByVal value As string)
		mNextAppointmentDate = value
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

    Public  Property ReferredToLaywer() As boolean
        Get
		return mReferredToLaywer
        End Get
        Set(ByVal value As boolean)
		mReferredToLaywer = value
        End Set
    End Property

    Public  Property ReferredToShelter() As boolean
        Get
		return mReferredToShelter
        End Get
        Set(ByVal value As boolean)
		mReferredToShelter = value
        End Set
    End Property

    Public  Property PresentingProblem() As string
        Get
		return mPresentingProblem
        End Get
        Set(ByVal value As string)
		mPresentingProblem = value
        End Set
    End Property

    Public  Property Other() As string
        Get
		return mOther
        End Get
        Set(ByVal value As string)
		mOther = value
        End Set
    End Property

    Public  Property CaseReported() As string
        Get
		return mCaseReported
        End Get
        Set(ByVal value As string)
		mCaseReported = value
        End Set
    End Property

    Public  Property WhereWasProblemReported() As string
        Get
		return mWhereWasProblemReported
        End Get
        Set(ByVal value As string)
		mWhereWasProblemReported = value
        End Set
    End Property

    Public  Property ChallengesFaced() As string
        Get
		return mChallengesFaced
        End Get
        Set(ByVal value As string)
		mChallengesFaced = value
        End Set
    End Property

    Public  Property MedicalReport() As string
        Get
		return mMedicalReport
        End Get
        Set(ByVal value As string)
		mMedicalReport = value
        End Set
    End Property

    Public  Property DurationOfSession() As string
        Get
		return mDurationOfSession
        End Get
        Set(ByVal value As string)
		mDurationOfSession = value
        End Set
    End Property

    Public  Property IssuedBy() As string
        Get
		return mIssuedBy
        End Get
        Set(ByVal value As string)
		mIssuedBy = value
        End Set
    End Property

    Public  Property ProblemsExperienced() As string
        Get
		return mProblemsExperienced
        End Get
        Set(ByVal value As string)
		mProblemsExperienced = value
        End Set
    End Property

    Public  Property ClientExpectations() As string
        Get
		return mClientExpectations
        End Get
        Set(ByVal value As string)
		mClientExpectations = value
        End Set
    End Property

    Public  Property OtherOptionAvailable() As string
        Get
		return mOtherOptionAvailable
        End Get
        Set(ByVal value As string)
		mOtherOptionAvailable = value
        End Set
    End Property

    Public  Property Referral() As string
        Get
		return mReferral
        End Get
        Set(ByVal value As string)
		mReferral = value
        End Set
    End Property

    Public  Property CarePlan() As string
        Get
		return mCarePlan
        End Get
        Set(ByVal value As string)
		mCarePlan = value
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

    InitialCounsellingSessionID = 0
        mBeneficiaryID = 0
        mProblemCategoryID = 0
        mEmploymentStatus = ""
        mNextOfKin = ""
        mReferredBy = ""
        mHowManyTimes = 0
    mLawyerID = 0
    mShelterID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mSessionDate = ""
    mNextAppointmentDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mReferredToLaywer = FALSE
    mReferredToShelter = FALSE
    mPresentingProblem = ""
    mOther = ""
    mCaseReported = ""
    mWhereWasProblemReported = ""
    mChallengesFaced = ""
    mMedicalReport = ""
    mDurationOfSession = ""
    mIssuedBy = ""
    mProblemsExperienced = ""
    mClientExpectations = ""
    mOtherOptionAvailable = ""
    mReferral = ""
    mCarePlan = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mInitialCounsellingSessionID) 

    End Function

    Public Overridable Function Retrieve(ByVal BeneficiaryID As Long) As Boolean

        Dim sql As String

        If BeneficiaryID > 0 Then
            sql = "SELECT * FROM tblInitialCounsellingSession WHERE BeneficiaryID = " & BeneficiaryID
        Else
            sql = "SELECT * FROM tblInitialCounsellingSession WHERE InitialCounsellingSessionID = " & mBeneficiaryID
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

                log.Error("InitialCounsellingSession not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetInitialCounsellingSession() As System.Data.DataSet

        Return GetInitialCounsellingSession(mInitialCounsellingSessionID)

    End Function

    Public Function GetBenDetails(ByVal UseCriteria As Boolean, ByVal StaffID As Long) As DataSet

        Dim Criteria As String = ""

        If UseCriteria Then

            Criteria = " AND CounsellorID = " & StaffID

        End If

        Dim sql As String = "Select DISTINCT B.*, D.Name As District, W.Name As Ward, ISNULL(S.UserFirstName, '') + ISNULL(S.UserSurname, '') AS AssignedBy, CAST(C.CreatedDate As Date) as DateAssigned from tblBeneficiaries B "
        sql &= "Left outer join tblAddresses A on A.OwnerID = B.BeneficiaryID  "
        sql &= "left outer join tblDistricts D on D.DistrictID = A.DistrictID "
        sql &= "Left outer join tblWards W on W.WardID = A.WardID  "
        sql &= "left outer join tblClientDetails C on C.BeneficiaryID = B.BeneficiaryID AND ReferredToCounsellor = 1 " & Criteria & " "
        sql &= "Left outer join tblUsers S on S.UserID = C.CreatedBy "
        sql &= "where B.BeneficiaryID in (Select BeneficiaryID from tblInitialCounsellingSession) OR "
        sql &= "B.BeneficiaryID in (Select BeneficiaryID from tblClientDetails where ReferredToCounsellor = 1 " & Criteria & " ) "
        sql &= "OR B.BeneficiaryID in (Select BeneficiaryID from tblReturningClientDetails) "

        Return GetInitialCounsellingSession(sql)

    End Function

    Public Overridable Function GetInitialCounsellingSession(ByVal InitialCounsellingSessionID As Long) As DataSet

        Dim sql As String

        If InitialCounsellingSessionID > 0 Then
            sql = "SELECT * FROM tblInitialCounsellingSession WHERE InitialCounsellingSessionID = " & InitialCounsellingSessionID
        Else
            sql = "SELECT * FROM tblInitialCounsellingSession WHERE InitialCounsellingSessionID = " & mInitialCounsellingSessionID
        End If

        Return GetInitialCounsellingSession(sql)

    End Function

    Protected Overridable Function GetInitialCounsellingSession(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mInitialCounsellingSessionID = Catchnull(.Item("InitialCounsellingSessionID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mProblemCategoryID = Catchnull(.Item("ProblemCategoryID"), 0)
            mHowManyTimes = Catchnull(.Item("HowManyTimes"), 0)
            mLawyerID = Catchnull(.Item("LawyerID"), 0)
            mShelterID = Catchnull(.Item("ShelterID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mSessionDate = Catchnull(.Item("SessionDate"), "")
            mEmploymentStatus = Catchnull(.Item("EmploymentStatus"), "")
            mNextOfKin = Catchnull(.Item("NextOfKin"), "")
            mReferredBy = Catchnull(.Item("ReferredBy"), "")
            mNextAppointmentDate = Catchnull(.Item("NextAppointmentDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mReferredToLaywer = Catchnull(.Item("ReferredToLaywer"), False)
            mReferredToShelter = Catchnull(.Item("ReferredToShelter"), False)
            mPresentingProblem = Catchnull(.Item("PresentingProblem"), "")
            mOther = Catchnull(.Item("Other"), "")
            mCaseReported = Catchnull(.Item("CaseReported"), "")
            mWhereWasProblemReported = Catchnull(.Item("WhereWasProblemReported"), "")
            mChallengesFaced = Catchnull(.Item("ChallengesFaced"), "")
            mMedicalReport = Catchnull(.Item("MedicalReport"), "")
            mDurationOfSession = Catchnull(.Item("DurationOfSession"), "")
            mIssuedBy = Catchnull(.Item("IssuedBy"), "")
            mProblemsExperienced = Catchnull(.Item("ProblemsExperienced"), "")
            mClientExpectations = Catchnull(.Item("ClientExpectations"), "")
            mOtherOptionAvailable = Catchnull(.Item("OtherOptionAvailable"), "")
            mReferral = Catchnull(.Item("Referral"), "")
            mCarePlan = Catchnull(.Item("CarePlan"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@InitialCounsellingSessionID", DbType.Int32, mInitialCounsellingSessionID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@EmploymentStatus", DbType.String, mEmploymentStatus)
        db.AddInParameter(cmd, "@NextOfKin", DbType.String, mNextOfKin)
        db.AddInParameter(cmd, "@ReferredBy", DbType.String, mReferredBy)
        db.AddInParameter(cmd, "@ProblemCategoryID", DbType.Int32, mProblemCategoryID)
        db.AddInParameter(cmd, "@HowManyTimes", DbType.Int32, mHowManyTimes)
        db.AddInParameter(cmd, "@LawyerID", DbType.Int32, mLawyerID)
        db.AddInParameter(cmd, "@ShelterID", DbType.Int32, mShelterID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@SessionDate", DbType.String, mSessionDate)
        db.AddInParameter(cmd, "@NextAppointmentDate", DbType.String, mNextAppointmentDate)
        db.AddInParameter(cmd, "@ReferredToLaywer", DbType.Boolean, mReferredToLaywer)
        db.AddInParameter(cmd, "@ReferredToShelter", DbType.Boolean, mReferredToShelter)
        db.AddInParameter(cmd, "@PresentingProblem", DbType.String, mPresentingProblem)
        db.AddInParameter(cmd, "@Other", DbType.String, mOther)
        db.AddInParameter(cmd, "@CaseReported", DbType.String, mCaseReported)
        db.AddInParameter(cmd, "@WhereWasProblemReported", DbType.String, mWhereWasProblemReported)
        db.AddInParameter(cmd, "@ChallengesFaced", DbType.String, mChallengesFaced)
        db.AddInParameter(cmd, "@MedicalReport", DbType.String, mMedicalReport)
        db.AddInParameter(cmd, "@DurationOfSession", DbType.String, mDurationOfSession)
        db.AddInParameter(cmd, "@IssuedBy", DbType.String, mIssuedBy)
        db.AddInParameter(cmd, "@ProblemsExperienced", DbType.String, mProblemsExperienced)
        db.AddInParameter(cmd, "@ClientExpectations", DbType.String, mClientExpectations)
        db.AddInParameter(cmd, "@OtherOptionAvailable", DbType.String, mOtherOptionAvailable)
        db.AddInParameter(cmd, "@Referral", DbType.String, mReferral)
        db.AddInParameter(cmd, "@CarePlan", DbType.String, mCarePlan)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_InitialCounsellingSession")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mInitialCounsellingSessionID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblInitialCounsellingSession SET Deleted = 1 WHERE InitialCounsellingSessionID = " & mInitialCounsellingSessionID) 
        Return Delete("DELETE FROM tblInitialCounsellingSession WHERE InitialCounsellingSessionID = " & mInitialCounsellingSessionID)

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