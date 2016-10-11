Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class StrategicPlans

#region "Variables"

    Protected mStrategicPlanID As long
    Protected mOrganizationID As long
    Protected mFromYear As long
    Protected mToYear As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDAte As string
    Protected mUpdatedDate As string
    Protected mPlanID As string
    Protected mName As string
    Protected mSummary As string

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

    Public  Property StrategicPlanID() As long
        Get
		return mStrategicPlanID
        End Get
        Set(ByVal value As long)
		mStrategicPlanID = value
        End Set
    End Property

    Public  Property OrganizationID() As long
        Get
		return mOrganizationID
        End Get
        Set(ByVal value As long)
		mOrganizationID = value
        End Set
    End Property

    Public  Property FromYear() As long
        Get
		return mFromYear
        End Get
        Set(ByVal value As long)
		mFromYear = value
        End Set
    End Property

    Public  Property ToYear() As long
        Get
		return mToYear
        End Get
        Set(ByVal value As long)
		mToYear = value
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

    Public  Property PlanID() As string
        Get
		return mPlanID
        End Get
        Set(ByVal value As string)
		mPlanID = value
        End Set
    End Property

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
        End Set
    End Property

    Public  Property Summary() As string
        Get
		return mSummary
        End Get
        Set(ByVal value As string)
		mSummary = value
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

    StrategicPlanID = 0
    mOrganizationID = 0
    mFromYear = 0
    mToYear = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDAte = ""
    mUpdatedDate = ""
    mPlanID = ""
    mName = ""
    mSummary = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mStrategicPlanID) 

    End Function 

    Public Overridable Function Retrieve(ByVal StrategicPlanID As Long) As Boolean 

        Dim sql As String 

        If StrategicPlanID > 0 Then 
            sql = "SELECT * FROM tblStrategicPlans WHERE StrategicPlanID = " & StrategicPlanID
        Else 
            sql = "SELECT * FROM tblStrategicPlans WHERE StrategicPlanID = " & mStrategicPlanID
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

                log.error("StrategicPlans not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetStrategicPlans() As System.Data.DataSet

        Return GetStrategicPlans(mStrategicPlanID)

    End Function

    Public Overridable Function GetStrategicPlans(ByVal StrategicPlanID As Long) As DataSet

        Dim sql As String

        If StrategicPlanID > 0 Then
            sql = "SELECT * FROM tblStrategicPlans WHERE StrategicPlanID = " & StrategicPlanID
        Else
            sql = "SELECT * FROM tblStrategicPlans WHERE StrategicPlanID = " & mStrategicPlanID
        End If

        Return GetStrategicPlans(sql)

    End Function

    Public Overridable Function GetStrategicPlans(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mStrategicPlanID = Catchnull(.Item("StrategicPlanID"), 0)
            mOrganizationID = Catchnull(.Item("OrganizationID"), 0)
            mFromYear = Catchnull(.Item("FromYear"), 0)
            mToYear = Catchnull(.Item("ToYear"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDAte = Catchnull(.Item("CreatedDAte"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mPlanID = Catchnull(.Item("PlanID"), "")
            mName = Catchnull(.Item("Name"), "")
            mSummary = Catchnull(.Item("Summary"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@StrategicPlanID", DBType.Int32, mStrategicPlanID)
        db.AddInParameter(cmd, "@OrganizationID", DBType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@FromYear", DBType.Int32, mFromYear)
        db.AddInParameter(cmd, "@ToYear", DBType.Int32, mToYear)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@PlanID", DBType.String, mPlanID)
        db.AddInParameter(cmd, "@Name", DBType.String, mName)
        db.AddInParameter(cmd, "@Summary", DBType.String, mSummary)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_StrategicPlans")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mStrategicPlanID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblStrategicPlans SET Deleted = 1 WHERE StrategicPlanID = " & mStrategicPlanID) 
        Return Delete("DELETE FROM tblStrategicPlans WHERE StrategicPlanID = " & mStrategicPlanID)

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