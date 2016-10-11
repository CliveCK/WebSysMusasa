Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class StrategicPlanDetails

#region "Variables"

    Protected mStrategicPlanDetailID As long
    Protected mStrategicPlanID As long
    Protected mObjectiveID As long
    Protected mActivityID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDAte As string
    Protected mUpdatedDate As string
    Protected mPlanYear As string
    Protected mPlanQuarter As String
    Protected mPlanMonth As String
    Protected mMilestone As string

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

    Public  Property StrategicPlanDetailID() As long
        Get
		return mStrategicPlanDetailID
        End Get
        Set(ByVal value As long)
		mStrategicPlanDetailID = value
        End Set
    End Property

    Public  Property StrategicPlanID() As long
        Get
		return mStrategicPlanID
        End Get
        Set(ByVal value As long)
		mStrategicPlanID = value
        End Set
    End Property

    Public  Property ObjectiveID() As long
        Get
		return mObjectiveID
        End Get
        Set(ByVal value As long)
		mObjectiveID = value
        End Set
    End Property

    Public  Property ActivityID() As long
        Get
		return mActivityID
        End Get
        Set(ByVal value As long)
		mActivityID = value
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

    Public  Property CreatedDAte() As string
        Get
		return mCreatedDAte
        End Get
        Set(ByVal value As string)
		mCreatedDAte = value
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

    Public  Property PlanYear() As string
        Get
		return mPlanYear
        End Get
        Set(ByVal value As string)
		mPlanYear = value
        End Set
    End Property

    Public  Property PlanQuarter() As string
        Get
		return mPlanQuarter
        End Get
        Set(ByVal value As string)
		mPlanQuarter = value
        End Set
    End Property

    Public Property PlanMonth() As String
        Get
            Return mPlanMonth
        End Get
        Set(ByVal value As String)
            mPlanMonth = value
        End Set
    End Property

    Public  Property Milestone() As string
        Get
		return mMilestone
        End Get
        Set(ByVal value As string)
		mMilestone = value
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

    StrategicPlanDetailID = 0
    mStrategicPlanID = 0
    mObjectiveID = 0
    mActivityID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDAte = ""
    mUpdatedDate = ""
    mPlanYear = ""
        mPlanQuarter = ""
        mPlanMonth = ""
    mMilestone = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mStrategicPlanDetailID) 

    End Function 

    Public Overridable Function Retrieve(ByVal StrategicPlanDetailID As Long) As Boolean 

        Dim sql As String 

        If StrategicPlanDetailID > 0 Then 
            sql = "SELECT * FROM tblStrategicPlanDetails WHERE StrategicPlanDetailID = " & StrategicPlanDetailID
        Else 
            sql = "SELECT * FROM tblStrategicPlanDetails WHERE StrategicPlanDetailID = " & mStrategicPlanDetailID
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

                log.error("StrategicPlanDetails not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetStrategicPlanDetails() As System.Data.DataSet

        Return GetStrategicPlanDetails(mStrategicPlanDetailID)

    End Function

    Public Overridable Function GetStrategicPlanDetails(ByVal StrategicPlanDetailID As Long) As DataSet

        Dim sql As String

        If StrategicPlanDetailID > 0 Then
            sql = "SELECT * FROM tblStrategicPlanDetails WHERE StrategicPlanDetailID = " & StrategicPlanDetailID
        Else
            sql = "SELECT * FROM tblStrategicPlanDetails WHERE StrategicPlanDetailID = " & mStrategicPlanDetailID
        End If

        Return GetStrategicPlanDetails(sql)

    End Function

    Public Overridable Function GetStrategicPlanDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mStrategicPlanDetailID = Catchnull(.Item("StrategicPlanDetailID"), 0)
            mStrategicPlanID = Catchnull(.Item("StrategicPlanID"), 0)
            mObjectiveID = Catchnull(.Item("ObjectiveID"), 0)
            mActivityID = Catchnull(.Item("ActivityID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDAte = Catchnull(.Item("CreatedDAte"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mPlanYear = Catchnull(.Item("PlanYear"), "")
            mPlanQuarter = Catchnull(.Item("PlanQuarter"), "")
            mPlanMonth = Catchnull(.Item("PlanMonth"), "")
            mMilestone = Catchnull(.Item("Milestone"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@StrategicPlanDetailID", DBType.Int32, mStrategicPlanDetailID)
        db.AddInParameter(cmd, "@StrategicPlanID", DBType.Int32, mStrategicPlanID)
        db.AddInParameter(cmd, "@ObjectiveID", DBType.Int32, mObjectiveID)
        db.AddInParameter(cmd, "@ActivityID", DBType.Int32, mActivityID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@PlanYear", DBType.String, mPlanYear)
        db.AddInParameter(cmd, "@PlanQuarter", DBType.String, mPlanQuarter)
        db.AddInParameter(cmd, "@Milestone", DBType.String, mMilestone)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_StrategicPlanDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mStrategicPlanDetailID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblStrategicPlanDetails SET Deleted = 1 WHERE StrategicPlanDetailID = " & mStrategicPlanDetailID) 
        Return Delete("DELETE FROM tblStrategicPlanDetails WHERE StrategicPlanDetailID = " & mStrategicPlanDetailID)

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