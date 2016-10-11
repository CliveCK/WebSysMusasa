Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions

Public Class Report

#Region "Variables"

    Protected mReportID As Long
    Protected mCode As String
    Protected mDescription As String
    Protected mLeftValue As Long
    Protected mRightValue As Long
    Protected mParentID As Long
    Protected mTreeID As Long
    Protected mReportName As String
    Protected mIsContainerOnly As Boolean
    Protected mIsSingleSelectionTree As Boolean
    Protected mIsSearchable As Boolean
    Protected mMem2000Match As Long
    Protected mXMLString As String
    Protected mCreatedDate As String
    Protected mCreatedBy As Long
    Protected mUpdatedDate As String
    Protected mUpdatedBy As Long
    Protected mIsSubscriptionBased As Boolean
    Protected mReportActionsXML As String
    Protected mReportData As String
    Protected mIsEditable As Boolean
    Protected mHasParameters As Boolean

    Protected db As Database
    Protected mConnectionName As String
    Protected mObjectUserID As Long

    Public Property ErrorMessage As String = String.Empty
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

    Public Overridable Property ReportID() As Long
        Get
            Return mReportID
        End Get
        Set(ByVal value As Long)
            mReportID = value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return mCode
        End Get
        Set(ByVal value As String)
            mCode = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal value As String)
            mDescription = value
        End Set
    End Property

    Public Property LeftValue() As Long
        Get
            Return mLeftValue
        End Get
        Set(ByVal value As Long)
            mLeftValue = value
        End Set
    End Property

    Public Property RightValue() As Long
        Get
            Return mRightValue
        End Get
        Set(ByVal value As Long)
            mRightValue = value
        End Set
    End Property

    Public Property ParentID() As Long
        Get
            Return mParentID
        End Get
        Set(ByVal value As Long)
            mParentID = value
        End Set
    End Property

    Public Property TreeID() As Long
        Get
            Return mTreeID
        End Get
        Set(ByVal value As Long)
            mTreeID = value
        End Set
    End Property

    Public Property ReportName() As String
        Get
            Return mReportName
        End Get
        Set(ByVal value As String)
            mReportName = value
        End Set
    End Property

    Public Property IsContainerOnly() As Boolean
        Get
            Return mIsContainerOnly
        End Get
        Set(ByVal value As Boolean)
            mIsContainerOnly = value
        End Set
    End Property

    Public Property IsSingleSelectionTree() As Boolean
        Get
            Return mIsSingleSelectionTree
        End Get
        Set(ByVal value As Boolean)
            mIsSingleSelectionTree = value
        End Set
    End Property

    Public Property IsSearchable() As Boolean
        Get
            Return mIsSearchable
        End Get
        Set(ByVal value As Boolean)
            mIsSearchable = value
        End Set
    End Property

    Public Property Mem2000Match() As Long
        Get
            Return mMem2000Match
        End Get
        Set(ByVal value As Long)
            mMem2000Match = value
        End Set
    End Property

    Public Property XMLString() As String
        Get
            Return mXMLString
        End Get
        Set(ByVal value As String)
            mXMLString = value
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

    Public Property CreatedBy() As Long
        Get
            Return mCreatedBy
        End Get
        Set(ByVal value As Long)
            mCreatedBy = value
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

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
        End Set
    End Property

    Public Property IsSubscriptionBased() As Boolean
        Get
            Return mIsSubscriptionBased
        End Get
        Set(ByVal value As Boolean)
            mIsSubscriptionBased = value
        End Set
    End Property

    Public Property ReportActionsXML() As String
        Get
            Return mReportActionsXML
        End Get
        Set(ByVal value As String)
            mReportActionsXML = value
        End Set
    End Property

    Public Property ReportData() As String
        Get
            Return mReportData
        End Get
        Set(ByVal value As String)
            mReportData = value
        End Set
    End Property

    Public Property IsEditable() As Boolean
        Get
            Return mIsEditable
        End Get
        Set(ByVal value As Boolean)
            mIsEditable = value
        End Set
    End Property

    Public Property HasParameters() As Boolean
        Get
            Return mHasParameters
        End Get
        Set(ByVal value As Boolean)
            mHasParameters = value
        End Set
    End Property

#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        Dim factory As DatabaseProviderFactory = New DatabaseProviderFactory()
        db = factory.Create(ConnectionName)

    End Sub

#End Region

  Public Shadows Sub Clear()

        mReportID = 0
        mCode = ""
        mDescription = ""
        mLeftValue = 0
        mRightValue = 0
        mParentID = 0
        mTreeID = 0
        mReportName = ""
        mIsContainerOnly = False
        mIsSingleSelectionTree = False
        mIsSearchable = False
        mMem2000Match = 0
        mXMLString = ""
        mCreatedDate = ""
        mCreatedBy = mObjectUserID
        mUpdatedDate = ""
        mUpdatedBy = 0
        mIsSubscriptionBased = False
        mReportActionsXML = ""
        mReportData = ""
        mIsEditable = False
        mHasParameters = False

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mReportID)

    End Function

    Public Overridable Function Retrieve(ByVal ReportID As Long) As Boolean

        Dim sql As String

        If ReportID > 0 Then
            sql = "SELECT * FROM tblReports WHERE ReportID = " & ReportID
        Else
            sql = "SELECT * FROM tblReports WHERE ReportID = " & mReportID
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

                ErrorMessage = "Report not found."
                log.Error(ErrorMessage)

                Return False

            End If

        Catch e As Exception

            ErrorMessage = e.Message
            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetReport() As System.Data.DataSet

        Return GetReport(mReportID)

    End Function

    Public Overridable Function GetReport(ByVal ReportID As Long) As DataSet

        Dim sql As String

        If ReportID > 0 Then
            sql = "SELECT * FROM tblReports WHERE ReportID = " & ReportID
        Else
            sql = "SELECT * FROM tblReports WHERE ReportID = " & mReportID
        End If

        Return GetReport(sql)

    End Function

    Protected Overridable Function GetReport(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Overridable Function GetReports() As DataSet

        Dim sql As String = "SELECT * FROM tblReports"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mReportID = Catchnull(.Item("ReportID"), 0)
            mCode = Catchnull(.Item("Code"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mLeftValue = Catchnull(.Item("LeftValue"), 0)
            mRightValue = Catchnull(.Item("RightValue"), 0)
            mParentID = Catchnull(.Item("ParentID"), 0)
            mTreeID = Catchnull(.Item("TreeID"), 0)
            mReportName = Catchnull(.Item("ReportName"), "")
            mIsContainerOnly = Catchnull(.Item("IsContainerOnly"), False)
            mIsSingleSelectionTree = Catchnull(.Item("IsSingleSelectionTree"), False)
            mIsSearchable = Catchnull(.Item("IsSearchable"), False)
            mMem2000Match = Catchnull(.Item("Mem2000Match"), 0)
            mXMLString = Catchnull(.Item("XMLString"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mIsSubscriptionBased = Catchnull(.Item("IsSubscriptionBased"), False)
            mReportActionsXML = Catchnull(.Item("ReportActionsXML"), "")
            mReportData = Catchnull(.Item("ReportData"), "")
            mIsEditable = Catchnull(.Item("IsEditable"), False)
            mHasParameters = Catchnull(.Item("HasParameters"), False)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ReportID", DBType.Int32, mReportID)
        db.AddInParameter(cmd, "@Code", DBType.String, mCode)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)
        db.AddInParameter(cmd, "@LeftValue", DBType.Int32, mLeftValue)
        db.AddInParameter(cmd, "@RightValue", DBType.Int32, mRightValue)
        db.AddInParameter(cmd, "@ParentID", DBType.Int32, mParentID)
        db.AddInParameter(cmd, "@TreeID", DBType.Int32, mTreeID)
        db.AddInParameter(cmd, "@ReportName", DBType.String, mReportName)
        db.AddInParameter(cmd, "@IsContainerOnly", DBType.Boolean, mIsContainerOnly)
        db.AddInParameter(cmd, "@IsSingleSelectionTree", DBType.Boolean, mIsSingleSelectionTree)
        db.AddInParameter(cmd, "@IsSearchable", DBType.Boolean, mIsSearchable)
        db.AddInParameter(cmd, "@Mem2000Match", DBType.Int32, mMem2000Match)
        db.AddInParameter(cmd, "@XMLString", DBType.String, mXMLString)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@IsSubscriptionBased", DBType.Boolean, mIsSubscriptionBased)
        db.AddInParameter(cmd, "@ReportActionsXML", DBType.String, mReportActionsXML)
        db.AddInParameter(cmd, "@ReportData", DBType.String, mReportData)
        db.AddInParameter(cmd, "@IsEditable", DBType.Boolean, mIsEditable)
        db.AddInParameter(cmd, "@HasParameters", DBType.Boolean, mHasParameters)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Report")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If (ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0) Then

                mReportID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblReports SET Deleted = 1 WHERE ReportID = " & mReportID) 
        Return Delete("DELETE FROM tblReports WHERE ReportID = " & mReportID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            ErrorMessage = e.Message
            log.Error(e)
            Return False

        End Try

    End Function

#End Region

#End Region

End Class