Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ReportSumbissionTracking

#region "Variables"

    Protected mReportSubmissionTrackingID As long
    Protected mFileID As long
    Protected mOrganizationID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mExpectedDate As string
    Protected mActualSubmissionDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mSubmissionStatus As boolean
    Protected mReportTitle As String
    Protected mAuthor As String
    Protected mComments As String

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

    Public Property ReportSubmissionTrackingID() As Long
        Get
            Return mReportSubmissionTrackingID
        End Get
        Set(ByVal value As Long)
            mReportSubmissionTrackingID = value
        End Set
    End Property

    Public Property FileID() As Long
        Get
            Return mFileID
        End Get
        Set(ByVal value As Long)
            mFileID = value
        End Set
    End Property

    Public Property OrganizationID() As Long
        Get
            Return mOrganizationID
        End Get
        Set(ByVal value As Long)
            mOrganizationID = value
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

    Public Property ExpectedDate() As String
        Get
            Return mExpectedDate
        End Get
        Set(ByVal value As String)
            mExpectedDate = value
        End Set
    End Property

    Public Property ActualSubmissionDate() As String
        Get
            Return mActualSubmissionDate
        End Get
        Set(ByVal value As String)
            mActualSubmissionDate = value
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

    Public Property SubmissionStatus() As Boolean
        Get
            Return mSubmissionStatus
        End Get
        Set(ByVal value As Boolean)
            mSubmissionStatus = value
        End Set
    End Property

    Public Property ReportTitle() As String
        Get
            Return mReportTitle
        End Get
        Set(ByVal value As String)
            mReportTitle = value
        End Set
    End Property

    Public Property Author() As String
        Get
            Return mAuthor
        End Get
        Set(ByVal value As String)
            mAuthor = value
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

        ReportSubmissionTrackingID = 0
        mFileID = 0
        mOrganizationID = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mExpectedDate = ""
        mActualSubmissionDate = ""
        mCreatedDate = ""
        mUpdatedDate = ""
        mSubmissionStatus = False
        mReportTitle = ""
        mAuthor = ""
        mComments = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mReportSubmissionTrackingID)

    End Function

    Public Overridable Function Retrieve(ByVal ReportSubmissionTrackingID As Long) As Boolean

        Dim sql As String

        If ReportSubmissionTrackingID > 0 Then
            sql = "SELECT * FROM tblReportSubmissionTracking WHERE ReportSubmissionTrackingID = " & ReportSubmissionTrackingID
        Else
            sql = "SELECT * FROM tblReportSubmissionTracking WHERE ReportSubmissionTrackingID = " & mReportSubmissionTrackingID
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

                log.Error("ReportSumbissionTracking not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetReportSumbissionTracking() As System.Data.DataSet

        Return GetReportSumbissionTracking(mReportSubmissionTrackingID)

    End Function

    Public Overridable Function GetReportSumbissionTracking(ByVal ReportSubmissionTrackingID As Long) As DataSet

        Dim sql As String

        If ReportSubmissionTrackingID > 0 Then
            sql = "SELECT * FROM tblReportSubmissionTracking WHERE ReportSubmissionTrackingID = " & ReportSubmissionTrackingID
        Else
            sql = "SELECT * FROM tblReportSubmissionTracking WHERE ReportSubmissionTrackingID = " & mReportSubmissionTrackingID
        End If

        Return GetReportSumbissionTracking(sql)

    End Function

    Public Overridable Function GetReportSumbissionTracking(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mReportSubmissionTrackingID = Catchnull(.Item("ReportSubmissionTrackingID"), 0)
            mFileID = Catchnull(.Item("FileID"), 0)
            mOrganizationID = Catchnull(.Item("OrganizationID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mExpectedDate = Catchnull(.Item("ExpectedDate"), "")
            mActualSubmissionDate = Catchnull(.Item("ActualSubmissionDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mSubmissionStatus = Catchnull(.Item("SubmissionStatus"), False)
            mReportTitle = Catchnull(.Item("ReportTitle"), "")
            mAuthor = Catchnull(.Item("Author"), "")
            mComments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ReportSubmissionTrackingID", DBType.Int32, mReportSubmissionTrackingID)
        db.AddInParameter(cmd, "@FileID", DBType.Int32, mFileID)
        db.AddInParameter(cmd, "@OrganizationID", DBType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ExpectedDate", DBType.String, mExpectedDate)
        db.AddInParameter(cmd, "@ActualSubmissionDate", DBType.String, mActualSubmissionDate)
        db.AddInParameter(cmd, "@SubmissionStatus", DBType.Boolean, mSubmissionStatus)
        db.AddInParameter(cmd, "@ReportTitle", DBType.String, mReportTitle)
        db.AddInParameter(cmd, "@Author", DBType.String, mAuthor)
        db.AddInParameter(cmd, "@Comments", DBType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ReportSumbissionTracking")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mReportSubmissionTrackingID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblReportSubmissionTracking SET Deleted = 1 WHERE ReportSubmissionTrackingID = " & mReportSubmissionTrackingID) 
        Return Delete("DELETE FROM tblReportSubmissionTracking WHERE ReportSubmissionTrackingID = " & mReportSubmissionTrackingID)

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