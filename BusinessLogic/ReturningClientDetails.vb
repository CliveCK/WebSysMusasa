Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ReturningClientDetails

#region "Variables"

    Protected mReturningClientDetailID As Long
    Protected mBeneficiaryID As Long
    Protected mHowManyTimes As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateOfLastAbuse As string
    Protected mNextAppointmentDate As string
    Protected mSessionDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mAbusedAfterVisit As string
    Protected mAbusedHow As string
    Protected mHasPreviousAbuseContinued As string
    Protected mHasNewAbuseStarted As string
    Protected mWhatKindOfAbuse As string
    Protected mIsNewAbuseLinkedToVisit As string
    Protected mActionToBeTakenByClient As string
    Protected mActionByCounsellor As string
    Protected mReportIssuedBy As string
    Protected mPermanentInjuries As string
    Protected mIssuesFromPreviousSession As string
    Protected mFeedbackFromLastSession As string
    Protected mNewIssuesRaised As string
    Protected mClientOptions As string
    Protected mWasReportMadeToPolice As string
    Protected mNameOfPoliceStation As string
    Protected mOfficer As string
    Protected mWasOfficerHelpful As string
    Protected mIfNoWhy As string
    Protected mAnyMedicalReport As string

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

    Public  Property ReturningClientDetailID() As long
        Get
		return mReturningClientDetailID
        End Get
        Set(ByVal value As long)
		mReturningClientDetailID = value
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

    Public  Property HowManyTimes() As long
        Get
		return mHowManyTimes
        End Get
        Set(ByVal value As long)
		mHowManyTimes = value
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

    Public  Property DateOfLastAbuse() As string
        Get
		return mDateOfLastAbuse
        End Get
        Set(ByVal value As string)
		mDateOfLastAbuse = value
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

    Public  Property SessionDate() As string
        Get
		return mSessionDate
        End Get
        Set(ByVal value As string)
		mSessionDate = value
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

    Public  Property AbusedAfterVisit() As string
        Get
		return mAbusedAfterVisit
        End Get
        Set(ByVal value As string)
		mAbusedAfterVisit = value
        End Set
    End Property

    Public  Property AbusedHow() As string
        Get
		return mAbusedHow
        End Get
        Set(ByVal value As string)
		mAbusedHow = value
        End Set
    End Property

    Public  Property HasPreviousAbuseContinued() As string
        Get
		return mHasPreviousAbuseContinued
        End Get
        Set(ByVal value As string)
		mHasPreviousAbuseContinued = value
        End Set
    End Property

    Public  Property HasNewAbuseStarted() As string
        Get
		return mHasNewAbuseStarted
        End Get
        Set(ByVal value As string)
		mHasNewAbuseStarted = value
        End Set
    End Property

    Public  Property WhatKindOfAbuse() As string
        Get
		return mWhatKindOfAbuse
        End Get
        Set(ByVal value As string)
		mWhatKindOfAbuse = value
        End Set
    End Property

    Public  Property IsNewAbuseLinkedToVisit() As string
        Get
		return mIsNewAbuseLinkedToVisit
        End Get
        Set(ByVal value As string)
		mIsNewAbuseLinkedToVisit = value
        End Set
    End Property

    Public  Property ActionToBeTakenByClient() As string
        Get
		return mActionToBeTakenByClient
        End Get
        Set(ByVal value As string)
		mActionToBeTakenByClient = value
        End Set
    End Property

    Public  Property ActionByCounsellor() As string
        Get
		return mActionByCounsellor
        End Get
        Set(ByVal value As string)
		mActionByCounsellor = value
        End Set
    End Property

    Public  Property ReportIssuedBy() As string
        Get
		return mReportIssuedBy
        End Get
        Set(ByVal value As string)
		mReportIssuedBy = value
        End Set
    End Property

    Public  Property PermanentInjuries() As string
        Get
		return mPermanentInjuries
        End Get
        Set(ByVal value As string)
		mPermanentInjuries = value
        End Set
    End Property

    Public  Property IssuesFromPreviousSession() As string
        Get
		return mIssuesFromPreviousSession
        End Get
        Set(ByVal value As string)
		mIssuesFromPreviousSession = value
        End Set
    End Property

    Public  Property FeedbackFromLastSession() As string
        Get
		return mFeedbackFromLastSession
        End Get
        Set(ByVal value As string)
		mFeedbackFromLastSession = value
        End Set
    End Property

    Public  Property NewIssuesRaised() As string
        Get
		return mNewIssuesRaised
        End Get
        Set(ByVal value As string)
		mNewIssuesRaised = value
        End Set
    End Property

    Public  Property ClientOptions() As string
        Get
		return mClientOptions
        End Get
        Set(ByVal value As string)
		mClientOptions = value
        End Set
    End Property

    Public  Property WasReportMadeToPolice() As string
        Get
		return mWasReportMadeToPolice
        End Get
        Set(ByVal value As string)
		mWasReportMadeToPolice = value
        End Set
    End Property

    Public  Property NameOfPoliceStation() As string
        Get
		return mNameOfPoliceStation
        End Get
        Set(ByVal value As string)
		mNameOfPoliceStation = value
        End Set
    End Property

    Public  Property Officer() As string
        Get
		return mOfficer
        End Get
        Set(ByVal value As string)
		mOfficer = value
        End Set
    End Property

    Public  Property WasOfficerHelpful() As string
        Get
		return mWasOfficerHelpful
        End Get
        Set(ByVal value As string)
		mWasOfficerHelpful = value
        End Set
    End Property

    Public  Property IfNoWhy() As string
        Get
		return mIfNoWhy
        End Get
        Set(ByVal value As string)
		mIfNoWhy = value
        End Set
    End Property

    Public  Property AnyMedicalReport() As string
        Get
		return mAnyMedicalReport
        End Get
        Set(ByVal value As string)
		mAnyMedicalReport = value
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

    ReturningClientDetailID = 0
        mBeneficiaryID = 0
        mHowManyTimes = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateOfLastAbuse = ""
    mNextAppointmentDate = ""
    mSessionDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mAbusedAfterVisit = ""
    mAbusedHow = ""
    mHasPreviousAbuseContinued = ""
    mHasNewAbuseStarted = ""
    mWhatKindOfAbuse = ""
    mIsNewAbuseLinkedToVisit = ""
    mActionToBeTakenByClient = ""
    mActionByCounsellor = ""
    mReportIssuedBy = ""
    mPermanentInjuries = ""
    mIssuesFromPreviousSession = ""
    mFeedbackFromLastSession = ""
    mNewIssuesRaised = ""
    mClientOptions = ""
    mWasReportMadeToPolice = ""
    mNameOfPoliceStation = ""
    mOfficer = ""
    mWasOfficerHelpful = ""
    mIfNoWhy = ""
    mAnyMedicalReport = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mReturningClientDetailID) 

    End Function

    Public Overridable Function Retrieve(ByVal BeneficiaryID As Long) As Boolean

        Dim sql As String

        If BeneficiaryID > 0 Then
            sql = "SELECT * FROM tblReturningClientDetails WHERE BeneficiaryID = " & BeneficiaryID
        Else
            sql = "SELECT * FROM tblReturningClientDetails WHERE ReturningClientDetailID = " & mBeneficiaryID
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

                log.Error("ReturningClientDetails not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetReturningClientDetails() As System.Data.DataSet

        Return GetReturningClientDetails(mReturningClientDetailID)

    End Function

    Public Overridable Function GetReturningClientDetails(ByVal ReturningClientDetailID As Long) As DataSet

        Dim sql As String

        If ReturningClientDetailID > 0 Then
            sql = "SELECT * FROM tblReturningClientDetails WHERE ReturningClientDetailID = " & ReturningClientDetailID
        Else
            sql = "SELECT * FROM tblReturningClientDetails WHERE ReturningClientDetailID = " & mReturningClientDetailID
        End If

        Return GetReturningClientDetails(sql)

    End Function

    Protected Overridable Function GetReturningClientDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mReturningClientDetailID = Catchnull(.Item("ReturningClientDetailID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mHowManyTimes = Catchnull(.Item("HowManyTimes"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateOfLastAbuse = Catchnull(.Item("DateOfLastAbuse"), "")
            mNextAppointmentDate = Catchnull(.Item("NextAppointmentDate"), "")
            mSessionDate = Catchnull(.Item("SessionDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mAbusedAfterVisit = Catchnull(.Item("AbusedAfterVisit"), "")
            mAbusedHow = Catchnull(.Item("AbusedHow"), "")
            mHasPreviousAbuseContinued = Catchnull(.Item("HasPreviousAbuseContinued"), "")
            mHasNewAbuseStarted = Catchnull(.Item("HasNewAbuseStarted"), "")
            mWhatKindOfAbuse = Catchnull(.Item("WhatKindOfAbuse"), "")
            mIsNewAbuseLinkedToVisit = Catchnull(.Item("IsNewAbuseLinkedToVisit"), "")
            mActionToBeTakenByClient = Catchnull(.Item("ActionToBeTakenByClient"), "")
            mActionByCounsellor = Catchnull(.Item("ActionByCounsellor"), "")
            mReportIssuedBy = Catchnull(.Item("ReportIssuedBy"), "")
            mPermanentInjuries = Catchnull(.Item("PermanentInjuries"), "")
            mIssuesFromPreviousSession = Catchnull(.Item("IssuesFromPreviousSession"), "")
            mFeedbackFromLastSession = Catchnull(.Item("FeedbackFromLastSession"), "")
            mNewIssuesRaised = Catchnull(.Item("NewIssuesRaised"), "")
            mClientOptions = Catchnull(.Item("ClientOptions"), "")
            mWasReportMadeToPolice = Catchnull(.Item("WasReportMadeToPolice"), "")
            mNameOfPoliceStation = Catchnull(.Item("NameOfPoliceStation"), "")
            mOfficer = Catchnull(.Item("Officer"), "")
            mWasOfficerHelpful = Catchnull(.Item("WasOfficerHelpful"), "")
            mIfNoWhy = Catchnull(.Item("IfNoWhy"), "")
            mAnyMedicalReport = Catchnull(.Item("AnyMedicalReport"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ReturningClientDetailID", DbType.Int32, mReturningClientDetailID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@HowManyTimes", DbType.Int32, mHowManyTimes)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateOfLastAbuse", DbType.String, mDateOfLastAbuse)
        db.AddInParameter(cmd, "@NextAppointmentDate", DbType.String, mNextAppointmentDate)
        db.AddInParameter(cmd, "@SessionDate", DbType.String, mSessionDate)
        db.AddInParameter(cmd, "@AbusedAfterVisit", DbType.String, mAbusedAfterVisit)
        db.AddInParameter(cmd, "@AbusedHow", DbType.String, mAbusedHow)
        db.AddInParameter(cmd, "@HasPreviousAbuseContinued", DbType.String, mHasPreviousAbuseContinued)
        db.AddInParameter(cmd, "@HasNewAbuseStarted", DbType.String, mHasNewAbuseStarted)
        db.AddInParameter(cmd, "@WhatKindOfAbuse", DbType.String, mWhatKindOfAbuse)
        db.AddInParameter(cmd, "@IsNewAbuseLinkedToVisit", DbType.String, mIsNewAbuseLinkedToVisit)
        db.AddInParameter(cmd, "@ActionToBeTakenByClient", DbType.String, mActionToBeTakenByClient)
        db.AddInParameter(cmd, "@ActionByCounsellor", DbType.String, mActionByCounsellor)
        db.AddInParameter(cmd, "@ReportIssuedBy", DbType.String, mReportIssuedBy)
        db.AddInParameter(cmd, "@PermanentInjuries", DbType.String, mPermanentInjuries)
        db.AddInParameter(cmd, "@IssuesFromPreviousSession", DbType.String, mIssuesFromPreviousSession)
        db.AddInParameter(cmd, "@FeedbackFromLastSession", DbType.String, mFeedbackFromLastSession)
        db.AddInParameter(cmd, "@NewIssuesRaised", DbType.String, mNewIssuesRaised)
        db.AddInParameter(cmd, "@ClientOptions", DbType.String, mClientOptions)
        db.AddInParameter(cmd, "@WasReportMadeToPolice", DbType.String, mWasReportMadeToPolice)
        db.AddInParameter(cmd, "@NameOfPoliceStation", DbType.String, mNameOfPoliceStation)
        db.AddInParameter(cmd, "@Officer", DbType.String, mOfficer)
        db.AddInParameter(cmd, "@WasOfficerHelpful", DbType.String, mWasOfficerHelpful)
        db.AddInParameter(cmd, "@IfNoWhy", DbType.String, mIfNoWhy)
        db.AddInParameter(cmd, "@AnyMedicalReport", DbType.String, mAnyMedicalReport)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ReturningClientDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mReturningClientDetailID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblReturningClientDetails SET Deleted = 1 WHERE ReturningClientDetailID = " & mReturningClientDetailID) 
        Return Delete("DELETE FROM tblReturningClientDetails WHERE ReturningClientDetailID = " & mReturningClientDetailID)

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