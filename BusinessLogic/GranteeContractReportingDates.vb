Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GranteeContractReportingDates

#region "Variables"

    Protected mReportDateID As long
    Protected mGrantDetailID As long
    Protected mReportTypeID As Long
    Protected mSubmissionStatusID As Long
    Protected mPersonResponsibleID As Long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mExpectedDate As string
    Protected mDateSubmitted As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mNameOfReport As string
    Protected mComments As string

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

    Public  Property ReportDateID() As long
        Get
		return mReportDateID
        End Get
        Set(ByVal value As long)
		mReportDateID = value
        End Set
    End Property

    Public  Property GrantDetailID() As long
        Get
		return mGrantDetailID
        End Get
        Set(ByVal value As long)
		mGrantDetailID = value
        End Set
    End Property

    Public  Property ReportTypeID() As long
        Get
		return mReportTypeID
        End Get
        Set(ByVal value As long)
		mReportTypeID = value
        End Set
    End Property

    Public Property SubmissionStatusID() As Long
        Get
            Return mSubmissionStatusID
        End Get
        Set(ByVal value As Long)
            mSubmissionStatusID = value
        End Set
    End Property

    Public Property PersonResponsibleID() As Long
        Get
            Return mPersonResponsibleID
        End Get
        Set(ByVal value As Long)
            mPersonResponsibleID = value
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

    Public  Property ExpectedDate() As string
        Get
		return mExpectedDate
        End Get
        Set(ByVal value As string)
		mExpectedDate = value
        End Set
    End Property

    Public  Property DateSubmitted() As string
        Get
		return mDateSubmitted
        End Get
        Set(ByVal value As string)
		mDateSubmitted = value
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

    Public  Property NameOfReport() As string
        Get
		return mNameOfReport
        End Get
        Set(ByVal value As string)
		mNameOfReport = value
        End Set
    End Property

    Public  Property Comments() As string
        Get
		return mComments
        End Get
        Set(ByVal value As string)
		mComments = value
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

    ReportDateID = 0
    mGrantDetailID = 0
    mReportTypeID = 0
        mSubmissionStatusID = 0
        mPersonResponsibleID = 0
        mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mExpectedDate = ""
    mDateSubmitted = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mNameOfReport = ""
    mComments = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mReportDateID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ReportDateID As Long) As Boolean 

        Dim sql As String 

        If ReportDateID > 0 Then 
            sql = "SELECT * FROM tblGranteeContractReportingDates WHERE ReportDateID = " & ReportDateID
        Else 
            sql = "SELECT * FROM tblGranteeContractReportingDates WHERE ReportDateID = " & mReportDateID
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

                log.Error("GranteeContractReportingDates not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGranteeContractReportingDates() As System.Data.DataSet

        Return GetGranteeContractReportingDates(mReportDateID)

    End Function

    Public Overridable Function GetGranteeContractReportingDates(ByVal GranteeDetailID As Long) As DataSet

        Dim sql As String

        If GranteeDetailID > 0 Then
            sql = "SELECT C.*, T.Description As TypeOfReport, S.Description As SubmissionStatus, M.StaffFullName As StaffResponsible FROM [tblGranteeContractReportingDates] C left outer join luContractReportTypes T ON C.ReportTypeID = T.ReportTypeID "
            sql &= "left outer join luSubmissionStatus S on S.SubmissionStatusID = C.SubmissionStatusID "
            sql &= "left outer join tblStaffMembers M on C.PersonResponsibleID = M.StaffID WHERE GrantDetailID = " & GranteeDetailID
        Else
            sql = "SELECT C.*, T.Description As TypeOfReport, S.Description As SubmissionStatus, M.StaffFullName As StaffResponsible FROM [tblGranteeContractReportingDates] C left outer join luContractReportTypes T ON C.ReportTypeID = T.ReportTypeID "
            sql &= "left outer join luSubmissionStatus S on S.SubmissionStatusID = C.SubmissionStatusID "
            sql &= "left outer join tblStaffMembers M On C.PersonResponsibleID = M.StaffID WHERE GrantDetailID = " & mGrantDetailID
        End If

        Return GetGranteeContractReportingDates(sql)

    End Function

    Protected Overridable Function GetGranteeContractReportingDates(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mReportDateID = Catchnull(.Item("ReportDateID"), 0)
            mGrantDetailID = Catchnull(.Item("GrantDetailID"), 0)
            mReportTypeID = Catchnull(.Item("ReportTypeID"), 0)
            mSubmissionStatusID = Catchnull(.Item("SubmissionStatusID"), 0)
            mPersonResponsibleID = Catchnull(.Item("PersonResponsibleID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mExpectedDate = Catchnull(.Item("ExpectedDate"), "")
            mDateSubmitted = Catchnull(.Item("DateSubmitted"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mNameOfReport = Catchnull(.Item("NameOfReport"), "")
            mComments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ReportDateID", DbType.Int32, mReportDateID)
        db.AddInParameter(cmd, "@GrantDetailID", DbType.Int32, mGrantDetailID)
        db.AddInParameter(cmd, "@ReportTypeID", DbType.Int32, mReportTypeID)
        db.AddInParameter(cmd, "@SubmissionStatusID", DbType.Int32, mSubmissionStatusID)
        db.AddInParameter(cmd, "@PersonResponsibleID", DbType.Int32, mPersonResponsibleID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ExpectedDate", DbType.String, mExpectedDate)
        db.AddInParameter(cmd, "@NameOfReport", DbType.String, mNameOfReport)
        db.AddInParameter(cmd, "@Comments", DbType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GranteeContractReportingDates")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mReportDateID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGranteeContractReportingDates Set Deleted = 1 WHERE ReportDateID = " & mReportDateID) 
        Return Delete("DELETE FROM tblGranteeContractReportingDates WHERE ReportDateID = " & mReportDateID)

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