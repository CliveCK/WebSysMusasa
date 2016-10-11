Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions

Public Class Projects

#Region "Variables"

    Protected mProject As Long
    Protected mProgramID As Long
    Protected mProjectCode As String
    Protected mKeyChangePromiseID As Long
    Protected mProjectManager As Long
    Protected mTargetedNoOfBeneficiaries As Long
    Protected mActualBeneficiaries As Long
    Protected mCreatedBy As Long
    Protected mUpdatedBy As Long
    Protected mStartDate As String
    Protected mFinalEvlDate As String
    Protected mEndDate As String
    Protected mCreatedDate As String
    Protected mUpdatedDate As String
    Protected mProjectBudget As Decimal
    Protected mName As String
    Protected mStrategicObjectiveID As Long
    Protected mAcronym As String
    Protected mProjectDocument As String
    Protected mFinalGStatement As String
    Protected mObjective As String
    Protected mBenDescription As String
    Protected mStakeholderDescription As String

    Protected db As Database
    Protected mConnectionName As String
    Protected mObjectUserID As Long

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

    Public Property Project() As Long
        Get
            Return mProject
        End Get
        Set(ByVal value As Long)
            mProject = value
        End Set
    End Property

    Public Property ProgrameID As Long
        Get
            Return mProgramID
        End Get
        Set(ByVal value As Long)
            mProgramID = value
        End Set
    End Property

    Public Property ProjectCode() As String
        Get
            Return mProjectCode
        End Get
        Set(ByVal value As String)
            mProjectCode = value
        End Set
    End Property

    Public Property KeyChangePromiseID() As Long
        Get
            Return mKeyChangePromiseID
        End Get
        Set(ByVal value As Long)
            mKeyChangePromiseID = value
        End Set
    End Property

    Public Property ProjectManager() As Long
        Get
            Return mProjectManager
        End Get
        Set(ByVal value As Long)
            mProjectManager = value
        End Set
    End Property

    Public Property TargetedNoOfBeneficiaries() As Long
        Get
            Return mTargetedNoOfBeneficiaries
        End Get
        Set(ByVal value As Long)
            mTargetedNoOfBeneficiaries = value
        End Set
    End Property

    Public Property ActualBeneficiaries() As Long
        Get
            Return mActualBeneficiaries
        End Get
        Set(ByVal value As Long)
            mActualBeneficiaries = value
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

    Public Property StartDate() As String
        Get
            Return mStartDate
        End Get
        Set(ByVal value As String)
            mStartDate = value
        End Set
    End Property

    Public Property FinalEvlDate() As String
        Get
            Return mFinalEvlDate
        End Get
        Set(ByVal value As String)
            mFinalEvlDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return mEndDate
        End Get
        Set(ByVal value As String)
            mEndDate = value
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

    Public Property ProjectBudget() As Decimal
        Get
            Return mProjectBudget
        End Get
        Set(ByVal value As Decimal)
            mProjectBudget = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Public Property StrategicObjectiveID() As Long
        Get
            Return mStrategicObjectiveID
        End Get
        Set(ByVal value As Long)
            mStrategicObjectiveID = value
        End Set
    End Property

    Public Property Acronym() As String
        Get
            Return mAcronym
        End Get
        Set(ByVal value As String)
            mAcronym = value
        End Set
    End Property

    Public Property ProjectDocument() As String
        Get
            Return mProjectDocument
        End Get
        Set(ByVal value As String)
            mProjectDocument = value
        End Set
    End Property

    Public Property FinalGStatement() As String
        Get
            Return mFinalGStatement
        End Get
        Set(ByVal value As String)
            mFinalGStatement = value
        End Set
    End Property

    Public Property Objective() As String
        Get
            Return mObjective
        End Get
        Set(ByVal value As String)
            mObjective = value
        End Set
    End Property

    Public Property BenDescription() As String
        Get
            Return mBenDescription
        End Get
        Set(ByVal value As String)
            mBenDescription = value
        End Set
    End Property

    Public Property StakeholderDescription() As String
        Get
            Return mStakeholderDescription
        End Get
        Set(ByVal value As String)
            mStakeholderDescription = value
        End Set
    End Property

#End Region

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub

#End Region

    Public Sub Clear()

        Project = 0
        mProjectCode = ""
        mKeyChangePromiseID = 0
        mProjectManager = 0
        mTargetedNoOfBeneficiaries = 0
        mActualBeneficiaries = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mStartDate = ""
        mFinalEvlDate = ""
        mEndDate = ""
        mCreatedDate = ""
        mUpdatedDate = ""
        mProjectBudget = 0.0
        mName = ""
        mStrategicObjectiveID = 0
        mAcronym = ""
        mProjectDocument = ""
        mFinalGStatement = ""
        mObjective = ""
        mBenDescription = ""
        mStakeholderDescription = ""
        mProgramID = 0

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mProject)

    End Function

    Public Overridable Function Retrieve(ByVal ProjectID As Long) As Boolean

        Dim sql As String

        If ProjectID > 0 Then
            sql = "SELECT * FROM tblProjects WHERE Project = " & ProjectID
        Else
            sql = "SELECT * FROM tblProjects WHERE Project = " & mProject
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

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetProjects() As System.Data.DataSet

        Return GetProjects(mProject)

    End Function

    Public Overridable Function GetProjects(ByVal Project As Long) As DataSet

        Dim sql As String
        Dim objOrganization As New Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim Criteria = IIf(CookiesWrapper.OrganizationID = objOrganization.GetParentOrganizationID(), "", " AND CreatedBy not in (" & objOrganization.GetPermittedUsersByOrganization(CookiesWrapper.OrganizationID) & ")")

        If Project > 0 Then
            sql = "SELECT * FROM tblProjects WHERE Project = " & Project & Criteria
        Else
            sql = "SELECT * FROM tblProjects WHERE Project = " & mProject & Criteria
        End If

        Return GetProjects(sql)

    End Function

    Public Overridable Function GetProjects(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAllProjects(ByVal StatusID As Int16) As DataSet

        Dim objOrganization As New Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim Criteria = IIf(CookiesWrapper.OrganizationID = objOrganization.GetParentOrganizationID(), "", " WHERE CreatedBy not in (" & objOrganization.GetPermittedUsersByOrganization(CookiesWrapper.OrganizationID) & ")")

        Dim sql As String = "SELECT * FROM tblProjects" & Criteria 'where IsActive = " & StatusID

        Return GetProjects(sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mProject = Catchnull(.Item("Project"), 0)
            mProjectCode = Catchnull(.Item("ProjectCode"), "")
            mKeyChangePromiseID = Catchnull(.Item("KeyChangePromiseID"), 0)
            mProgramID = Catchnull(.Item("Program"), 0)
            mProjectManager = Catchnull(.Item("ProjectManager"), 0)
            mTargetedNoOfBeneficiaries = Catchnull(.Item("TargetedNoOfBeneficiaries"), 0)
            mActualBeneficiaries = Catchnull(.Item("ActualBeneficiaries"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mStartDate = Catchnull(.Item("StartDate"), "")
            mFinalEvlDate = Catchnull(.Item("FinalEvlDate"), "")
            mEndDate = Catchnull(.Item("EndDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mProjectBudget = Catchnull(.Item("ProjectBudget"), 0.0)
            mName = Catchnull(.Item("Name"), "")
            mStrategicObjectiveID = Catchnull(.Item("StrategicObjectiveID"), 0)
            mAcronym = Catchnull(.Item("Acronym"), "")
            mProjectDocument = Catchnull(.Item("ProjectDocument"), "")
            mFinalGStatement = Catchnull(.Item("FinalGStatement"), "")
            mObjective = Catchnull(.Item("Objective"), "")
            mBenDescription = Catchnull(.Item("BenDescription"), "")
            mStakeholderDescription = Catchnull(.Item("StakeholderDescription"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@Project", DbType.Int32, mProject)
        db.AddInParameter(cmd, "@ProgramID", DbType.Int32, mProgramID)
        db.AddInParameter(cmd, "@ProjectCode", DbType.String, mProjectCode)
        db.AddInParameter(cmd, "@KeyChangePromiseID", DbType.Int32, mKeyChangePromiseID)
        db.AddInParameter(cmd, "@ProjectManager", DbType.Int32, mProjectManager)
        db.AddInParameter(cmd, "@TargetedNoOfBeneficiaries", DbType.Int32, mTargetedNoOfBeneficiaries)
        db.AddInParameter(cmd, "@ActualBeneficiaries", DbType.Int32, mActualBeneficiaries)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@StartDate", DbType.String, mStartDate)
        db.AddInParameter(cmd, "@FinalEvlDate", DbType.String, mFinalEvlDate)
        db.AddInParameter(cmd, "@EndDate", DbType.String, mEndDate)
        db.AddInParameter(cmd, "@ProjectBudget", DbType.Decimal, mProjectBudget)
        db.AddInParameter(cmd, "@Name", DbType.String, mName)
        db.AddInParameter(cmd, "@StrategicObjectiveID", DbType.Int32, mStrategicObjectiveID)
        db.AddInParameter(cmd, "@Acronym", DbType.String, mAcronym)
        db.AddInParameter(cmd, "@ProjectDocument", DbType.String, mProjectDocument)
        db.AddInParameter(cmd, "@FinalGStatement", DbType.String, mFinalGStatement)
        db.AddInParameter(cmd, "@Objective", DbType.String, mObjective)
        db.AddInParameter(cmd, "@BenDescription", DbType.String, mBenDescription)
        db.AddInParameter(cmd, "@StakeholderDescription", DbType.String, mStakeholderDescription)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Projects")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mProject = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProjects SET Deleted = 1 WHERE Project = " & mProject) 
        Return Delete("DELETE FROM tblProjects WHERE Project = " & mProject)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            Return False

        End Try

    End Function

#End Region

#End Region

End Class