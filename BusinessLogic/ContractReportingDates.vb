Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ContractReportingDates

#region "Variables"

    Protected mReportDateID As Long
    Protected mGrantDetailID As Long
    Protected mReportTypeID As Long
    Protected mCreatedBy As Long
    Protected mUpdatedBy As Long
    Protected mDateSubmitted As String
    Protected mPersonResponsibleID As Long
    Protected mCreatedDate As String
    Protected mUpdatedDate As String
    Protected mNameOfReport As String
    Protected mComments As String
    Protected mExpectedDate As String
    Protected mStatusID As Long

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

    Public Property ReportDateID() As Long
        Get
            Return mReportDateID
        End Get
        Set(ByVal value As Long)
            mReportDateID = value
        End Set
    End Property

    Public Property GrantDetailID() As Long
        Get
            Return mGrantDetailID
        End Get
        Set(ByVal value As Long)
            mGrantDetailID = value
        End Set
    End Property

    Public Property ReportTypeID() As Long
        Get
            Return mReportTypeID
        End Get
        Set(ByVal value As Long)
            mReportTypeID = value
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

    Public Property DateSubmitted() As String
        Get
            Return mDateSubmitted
        End Get
        Set(ByVal value As String)
            mDateSubmitted = value
        End Set
    End Property

    Public Property ExpectedDate() As String
        Get
            Return mExpectedDate
        End Get
        Set(ByVal value As String)
            mExpectedDate = value
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

    Public Property NameOfReport() As String
        Get
            Return mNameOfReport
        End Get
        Set(ByVal value As String)
            mNameOfReport = value
        End Set
    End Property

    Public Property Comments() As String
        Get
            Return mComments
        End Get
        Set(ByVal value As String)
            mComments = value
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

        ReportDateID = 0
        mGrantDetailID = 0
        mReportTypeID = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mDateSubmitted = ""
        mPersonResponsibleID = 0
        mCreatedDate = ""
        mUpdatedDate = ""
        mNameOfReport = ""
        mComments = ""
        mExpectedDate = ""
        mStatusID = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mReportDateID)

    End Function

    Public Overridable Function Retrieve(ByVal ReportDateID As Long) As Boolean

        Dim sql As String

        If ReportDateID > 0 Then
            sql = "SELECT * FROM tblContractReportingDates WHERE ReportDateID = " & ReportDateID
        Else
            sql = "SELECT * FROM tblContractReportingDates WHERE ReportDateID = " & mReportDateID
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

                log.Error("ContractReportingDates not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetContractReportingDates() As System.Data.DataSet

        Return GetContractReportingDates(mReportDateID)

    End Function

    Public Overridable Function GetContractReportingDates(ByVal GrantDetailID As Long) As DataSet

        Dim sql As String

        If GrantDetailID > 0 Then
            sql = "SELECT C.*, T.Description As TypeOfReport, S.Description As SubmissionStatus, M.StaffFullName As StaffResponsible FROM tblContractReportingDates C left outer join luContractReportTypes T ON C.ReportTypeID = T.ReportTypeID "
            sql &= "left outer join luSubmissionStatus S on S.SubmissionStatusID = C.SubmissionStatusID "
            sql &= "left outer join tblStaffMembers M on C.PersonResponsibleID = M.StaffID WHERE GrantDetailID = " & GrantDetailID
        Else
            sql = "SELECT C.*, T.Description As TypeOfReport, S.Description As SubmissionStatus, M.StaffFullName As StaffResponsible FROM tblContractReportingDates C left outer join luContractReportTypes T ON C.ReportTypeID = T.ReportTypeID "
            sql &= "left outer join luSubmissionStatus S on S.SubmissionStatusID = C.SubmissionStatusID "
            sql &= "left outer join tblStaffMembers M on C.PersonResponsibleID = M.StaffID WHERE GrantDetailID = " & mGrantDetailID
        End If

        Return GetContractReportingDates(sql)

    End Function

    Protected Overridable Function GetContractReportingDates(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mReportDateID = Catchnull(.Item("ReportDateID"), 0)
            mGrantDetailID = Catchnull(.Item("GrantDetailID"), 0)
            mReportTypeID = Catchnull(.Item("ReportTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateSubmitted = Catchnull(.Item("DateSubmitted"), "")
            mPersonResponsibleID = Catchnull(.Item("PersonResponsibleID"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mNameOfReport = Catchnull(.Item("NameOfReport"), "")
            mComments = Catchnull(.Item("Comments"), "")
            mStatusID = Catchnull(.Item("SubmissionStatusID"), "")
            mExpectedDate = Catchnull(.Item("ExpectedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ReportDateID", DbType.Int32, mReportDateID)
        db.AddInParameter(cmd, "@GrantDetailID", DbType.Int32, mGrantDetailID)
        db.AddInParameter(cmd, "@ReportTypeID", DbType.Int32, mReportTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ExpectedDate", DbType.String, mExpectedDate)
        db.AddInParameter(cmd, "@NameOfReport", DbType.String, mNameOfReport)
        db.AddInParameter(cmd, "@SubmissionStatusID", DbType.String, mStatusID)
        db.AddInParameter(cmd, "@PersonResponsibleID", DbType.String, mPersonResponsibleID)
        db.AddInParameter(cmd, "@Comments", DbType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ContractReportingDates")

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

        'Return Delete("UPDATE tblContractReportingDates SET Deleted = 1 WHERE ReportDateID = " & mReportDateID) 
        Return Delete("DELETE FROM tblContractReportingDates WHERE ReportDateID = " & mReportDateID)

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