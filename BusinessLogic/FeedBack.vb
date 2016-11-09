Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class FeedBack

#region "Variables"

    Protected mFeedbackID As long
    Protected mAge As long
    Protected mNatureOfProblemID As long
    Protected mAssistanceID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateCompleted As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mOfficeName As string
    Protected mSex As string
    Protected mOtherProblem As string
    Protected mOtherAssistance As string
    Protected mSatisfiedOutcomeOfCase As string
    Protected mExplainWhyCase As string
    Protected mRecommendations As string
    Protected mWouldRecommend As string
    Protected mOtherChallenges As string
    Protected mExpectation As string
    Protected mExpectationFulfilled As string
    Protected mExplainExpectation As string
    Protected mFeelSafe As string
    Protected mExplainFeelSafe As string
    Protected mSatisfiedWithService As string
    Protected mExplaiWhyService As string
    Protected mHelpInOtherAreas As string
    Protected mHowHelpful As string
    Protected mChallengesWithOrganization As string
    Protected mChallengesWithServiceDelivery As string

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

    Public  Property FeedbackID() As long
        Get
		return mFeedbackID
        End Get
        Set(ByVal value As long)
		mFeedbackID = value
        End Set
    End Property

    Public  Property Age() As long
        Get
		return mAge
        End Get
        Set(ByVal value As long)
		mAge = value
        End Set
    End Property

    Public  Property NatureOfProblemID() As long
        Get
		return mNatureOfProblemID
        End Get
        Set(ByVal value As long)
		mNatureOfProblemID = value
        End Set
    End Property

    Public  Property AssistanceID() As long
        Get
		return mAssistanceID
        End Get
        Set(ByVal value As long)
		mAssistanceID = value
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

    Public  Property DateCompleted() As string
        Get
		return mDateCompleted
        End Get
        Set(ByVal value As string)
		mDateCompleted = value
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

    Public  Property OfficeName() As string
        Get
		return mOfficeName
        End Get
        Set(ByVal value As string)
		mOfficeName = value
        End Set
    End Property

    Public  Property Sex() As string
        Get
		return mSex
        End Get
        Set(ByVal value As string)
		mSex = value
        End Set
    End Property

    Public  Property OtherProblem() As string
        Get
		return mOtherProblem
        End Get
        Set(ByVal value As string)
		mOtherProblem = value
        End Set
    End Property

    Public  Property OtherAssistance() As string
        Get
		return mOtherAssistance
        End Get
        Set(ByVal value As string)
		mOtherAssistance = value
        End Set
    End Property

    Public  Property SatisfiedOutcomeOfCase() As string
        Get
		return mSatisfiedOutcomeOfCase
        End Get
        Set(ByVal value As string)
		mSatisfiedOutcomeOfCase = value
        End Set
    End Property

    Public  Property ExplainWhyCase() As string
        Get
		return mExplainWhyCase
        End Get
        Set(ByVal value As string)
		mExplainWhyCase = value
        End Set
    End Property

    Public  Property Recommendations() As string
        Get
		return mRecommendations
        End Get
        Set(ByVal value As string)
		mRecommendations = value
        End Set
    End Property

    Public  Property WouldRecommend() As string
        Get
		return mWouldRecommend
        End Get
        Set(ByVal value As string)
		mWouldRecommend = value
        End Set
    End Property

    Public  Property OtherChallenges() As string
        Get
		return mOtherChallenges
        End Get
        Set(ByVal value As string)
		mOtherChallenges = value
        End Set
    End Property

    Public  Property Expectation() As string
        Get
		return mExpectation
        End Get
        Set(ByVal value As string)
		mExpectation = value
        End Set
    End Property

    Public  Property ExpectationFulfilled() As string
        Get
		return mExpectationFulfilled
        End Get
        Set(ByVal value As string)
		mExpectationFulfilled = value
        End Set
    End Property

    Public  Property ExplainExpectation() As string
        Get
		return mExplainExpectation
        End Get
        Set(ByVal value As string)
		mExplainExpectation = value
        End Set
    End Property

    Public  Property FeelSafe() As string
        Get
		return mFeelSafe
        End Get
        Set(ByVal value As string)
		mFeelSafe = value
        End Set
    End Property

    Public  Property ExplainFeelSafe() As string
        Get
		return mExplainFeelSafe
        End Get
        Set(ByVal value As string)
		mExplainFeelSafe = value
        End Set
    End Property

    Public  Property SatisfiedWithService() As string
        Get
		return mSatisfiedWithService
        End Get
        Set(ByVal value As string)
		mSatisfiedWithService = value
        End Set
    End Property

    Public  Property ExplaiWhyService() As string
        Get
		return mExplaiWhyService
        End Get
        Set(ByVal value As string)
		mExplaiWhyService = value
        End Set
    End Property

    Public  Property HelpInOtherAreas() As string
        Get
		return mHelpInOtherAreas
        End Get
        Set(ByVal value As string)
		mHelpInOtherAreas = value
        End Set
    End Property

    Public  Property HowHelpful() As string
        Get
		return mHowHelpful
        End Get
        Set(ByVal value As string)
		mHowHelpful = value
        End Set
    End Property

    Public  Property ChallengesWithOrganization() As string
        Get
		return mChallengesWithOrganization
        End Get
        Set(ByVal value As string)
		mChallengesWithOrganization = value
        End Set
    End Property

    Public  Property ChallengesWithServiceDelivery() As string
        Get
		return mChallengesWithServiceDelivery
        End Get
        Set(ByVal value As string)
		mChallengesWithServiceDelivery = value
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

    FeedbackID = 0
    mAge = 0
    mNatureOfProblemID = 0
    mAssistanceID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateCompleted = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mOfficeName = ""
    mSex = ""
    mOtherProblem = ""
    mOtherAssistance = ""
    mSatisfiedOutcomeOfCase = ""
    mExplainWhyCase = ""
    mRecommendations = ""
    mWouldRecommend = ""
    mOtherChallenges = ""
    mExpectation = ""
    mExpectationFulfilled = ""
    mExplainExpectation = ""
    mFeelSafe = ""
    mExplainFeelSafe = ""
    mSatisfiedWithService = ""
    mExplaiWhyService = ""
    mHelpInOtherAreas = ""
    mHowHelpful = ""
    mChallengesWithOrganization = ""
    mChallengesWithServiceDelivery = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mFeedbackID) 

    End Function 

    Public Overridable Function Retrieve(ByVal FeedbackID As Long) As Boolean 

        Dim sql As String 

        If FeedbackID > 0 Then 
            sql = "SELECT * FROM tblFeeback WHERE FeedbackID = " & FeedbackID
        Else 
            sql = "SELECT * FROM tblFeeback WHERE FeedbackID = " & mFeedbackID
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

                log.Error("FeedBack not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetFeedBack() As System.Data.DataSet

        Return GetFeedBack(mFeedbackID)

    End Function

    Public Overridable Function GetFeedBack(ByVal FeedbackID As Long) As DataSet

        Dim sql As String

        If FeedbackID > 0 Then
            sql = "SELECT * FROM tblFeeback WHERE FeedbackID = " & FeedbackID
        Else
            sql = "SELECT * FROM tblFeeback WHERE FeedbackID = " & mFeedbackID
        End If

        Return GetFeedBack(sql)

    End Function

    Protected Overridable Function GetFeedBack(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mFeedbackID = Catchnull(.Item("FeedbackID"), 0)
            mAge = Catchnull(.Item("Age"), 0)
            mNatureOfProblemID = Catchnull(.Item("NatureOfProblemID"), 0)
            mAssistanceID = Catchnull(.Item("AssistanceID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateCompleted = Catchnull(.Item("DateCompleted"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mOfficeName = Catchnull(.Item("OfficeName"), "")
            mSex = Catchnull(.Item("Sex"), "")
            mOtherProblem = Catchnull(.Item("OtherProblem"), "")
            mOtherAssistance = Catchnull(.Item("OtherAssistance"), "")
            mSatisfiedOutcomeOfCase = Catchnull(.Item("SatisfiedOutcomeOfCase"), "")
            mExplainWhyCase = Catchnull(.Item("ExplainWhyCase"), "")
            mRecommendations = Catchnull(.Item("Recommendations"), "")
            mWouldRecommend = Catchnull(.Item("WouldRecommend"), "")
            mOtherChallenges = Catchnull(.Item("OtherChallenges"), "")
            mExpectation = Catchnull(.Item("Expectation"), "")
            mExpectationFulfilled = Catchnull(.Item("ExpectationFulfilled"), "")
            mExplainExpectation = Catchnull(.Item("ExplainExpectation"), "")
            mFeelSafe = Catchnull(.Item("FeelSafe"), "")
            mExplainFeelSafe = Catchnull(.Item("ExplainFeelSafe"), "")
            mSatisfiedWithService = Catchnull(.Item("SatisfiedWithService"), "")
            mExplaiWhyService = Catchnull(.Item("ExplaiWhyService"), "")
            mHelpInOtherAreas = Catchnull(.Item("HelpInOtherAreas"), "")
            mHowHelpful = Catchnull(.Item("HowHelpful"), "")
            mChallengesWithOrganization = Catchnull(.Item("ChallengesWithOrganization"), "")
            mChallengesWithServiceDelivery = Catchnull(.Item("ChallengesWithServiceDelivery"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@FeedbackID", DbType.Int32, mFeedbackID)
        db.AddInParameter(cmd, "@Age", DbType.Int32, mAge)
        db.AddInParameter(cmd, "@NatureOfProblemID", DbType.Int32, mNatureOfProblemID)
        db.AddInParameter(cmd, "@AssistanceID", DbType.Int32, mAssistanceID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateCompleted", DbType.String, mDateCompleted)
        db.AddInParameter(cmd, "@OfficeName", DbType.String, mOfficeName)
        db.AddInParameter(cmd, "@Sex", DbType.String, mSex)
        db.AddInParameter(cmd, "@OtherProblem", DbType.String, mOtherProblem)
        db.AddInParameter(cmd, "@OtherAssistance", DbType.String, mOtherAssistance)
        db.AddInParameter(cmd, "@SatisfiedOutcomeOfCase", DbType.String, mSatisfiedOutcomeOfCase)
        db.AddInParameter(cmd, "@ExplainWhyCase", DbType.String, mExplainWhyCase)
        db.AddInParameter(cmd, "@Recommendations", DbType.String, mRecommendations)
        db.AddInParameter(cmd, "@WouldRecommend", DbType.String, mWouldRecommend)
        db.AddInParameter(cmd, "@OtherChallenges", DbType.String, mOtherChallenges)
        db.AddInParameter(cmd, "@Expectation", DbType.String, mExpectation)
        db.AddInParameter(cmd, "@ExpectationFulfilled", DbType.String, mExpectationFulfilled)
        db.AddInParameter(cmd, "@ExplainExpectation", DbType.String, mExplainExpectation)
        db.AddInParameter(cmd, "@FeelSafe", DbType.String, mFeelSafe)
        db.AddInParameter(cmd, "@ExplainFeelSafe", DbType.String, mExplainFeelSafe)
        db.AddInParameter(cmd, "@SatisfiedWithService", DbType.String, mSatisfiedWithService)
        db.AddInParameter(cmd, "@ExplaiWhyService", DbType.String, mExplaiWhyService)
        db.AddInParameter(cmd, "@HelpInOtherAreas", DbType.String, mHelpInOtherAreas)
        db.AddInParameter(cmd, "@HowHelpful", DbType.String, mHowHelpful)
        db.AddInParameter(cmd, "@ChallengesWithOrganization", DbType.String, mChallengesWithOrganization)
        db.AddInParameter(cmd, "@ChallengesWithServiceDelivery", DbType.String, mChallengesWithServiceDelivery)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_FeedBack")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mFeedbackID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblFeeback SET Deleted = 1 WHERE FeedbackID = " & mFeedbackID) 
        Return Delete("DELETE FROM tblFeeback WHERE FeedbackID = " & mFeedbackID)

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