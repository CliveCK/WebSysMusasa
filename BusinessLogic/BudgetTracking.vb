Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class BudgetTracking

#region "Variables"

    Protected mBudgetTrackinngID As long
    Protected mBudgetID As long
    Protected mBudgetYear As long
    Protected mBudgetMonthID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mBudgetTarget As decimal
    Protected mActual As decimal
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

    Public  Property BudgetTrackinngID() As long
        Get
		return mBudgetTrackinngID
        End Get
        Set(ByVal value As long)
		mBudgetTrackinngID = value
        End Set
    End Property

    Public  Property BudgetID() As long
        Get
		return mBudgetID
        End Get
        Set(ByVal value As long)
		mBudgetID = value
        End Set
    End Property

    Public  Property BudgetYear() As long
        Get
		return mBudgetYear
        End Get
        Set(ByVal value As long)
		mBudgetYear = value
        End Set
    End Property

    Public  Property BudgetMonthID() As long
        Get
		return mBudgetMonthID
        End Get
        Set(ByVal value As long)
		mBudgetMonthID = value
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

    Public  Property BudgetTarget() As decimal
        Get
		return mBudgetTarget
        End Get
        Set(ByVal value As decimal)
		mBudgetTarget = value
        End Set
    End Property

    Public  Property Actual() As decimal
        Get
		return mActual
        End Get
        Set(ByVal value As decimal)
		mActual = value
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

    BudgetTrackinngID = 0
    mBudgetID = 0
    mBudgetYear = 0
    mBudgetMonthID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mBudgetTarget = 0.0
    mActual = 0.0
    mComments = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mBudgetTrackinngID) 

    End Function 

    Public Overridable Function Retrieve(ByVal BudgetTrackinngID As Long) As Boolean 

        Dim sql As String 

        If BudgetTrackinngID > 0 Then 
            sql = "SELECT * FROM tblBudgetTracking WHERE BudgetTrackinngID = " & BudgetTrackinngID
        Else 
            sql = "SELECT * FROM tblBudgetTracking WHERE BudgetTrackinngID = " & mBudgetTrackinngID
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

                log.Error("BudgetTracking not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetBudgetTracking() As System.Data.DataSet

        Return GetBudgetTracking(mBudgetTrackinngID)

    End Function

    Public Overridable Function GetBudgetTracking(ByVal BudgetTrackinngID As Long) As DataSet

        Dim sql As String

        If BudgetTrackinngID > 0 Then
            sql = "SELECT * FROM tblBudgetTracking WHERE BudgetTrackinngID = " & BudgetTrackinngID
        Else
            sql = "SELECT * FROM tblBudgetTracking WHERE BudgetTrackinngID = " & mBudgetTrackinngID
        End If

        Return GetBudgetTracking(sql)

    End Function

    Public Overridable Function GetBudgetTrackingByBudget(ByVal BudgetID As Long) As DataSet

        Dim sql As String

        sql = "select BT.*, A.Description AS Activity, AC.Description AS ActivityCategory, M.Description as BudgetMonth from tblBudgetTracking BT "
        sql &= "inner join tblBudgets B on BT.BudgetID = B.BudgetID "
        sql &= "inner join tblActivities A on A.ActivityID = B.ActivityID "
        sql &= "inner join tblActivityCategory AC on B.ActivityCategoryID = AC.ActivityCategoryID "
        sql &= "left outer join luMonths M on BT.BudgetMonthID = M.MonthID where B.BudgetID = " & IIf(BudgetID > 0, BudgetID, mBudgetID)

        Return GetBudgetTracking(sql)

    End Function

    Protected Overridable Function GetBudgetTracking(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mBudgetTrackinngID = Catchnull(.Item("BudgetTrackinngID"), 0)
            mBudgetID = Catchnull(.Item("BudgetID"), 0)
            mBudgetYear = Catchnull(.Item("BudgetYear"), 0)
            mBudgetMonthID = Catchnull(.Item("BudgetMonthID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mBudgetTarget = Catchnull(.Item("BudgetTarget"), 0.0)
            mActual = Catchnull(.Item("Actual"), 0.0)
            mComments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@BudgetTrackinngID", DbType.Int32, mBudgetTrackinngID)
        db.AddInParameter(cmd, "@BudgetID", DbType.Int32, mBudgetID)
        db.AddInParameter(cmd, "@BudgetYear", DbType.Int32, mBudgetYear)
        db.AddInParameter(cmd, "@BudgetMonthID", DbType.Int32, mBudgetMonthID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@BudgetTarget", DbType.Decimal, mBudgetTarget)
        db.AddInParameter(cmd, "@Actual", DbType.Decimal, mActual)
        db.AddInParameter(cmd, "@Comments", DbType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_BudgetTracking")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mBudgetTrackinngID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblBudgetTracking SET Deleted = 1 WHERE BudgetTrackinngID = " & mBudgetTrackinngID) 
        Return Delete("DELETE FROM tblBudgetTracking WHERE BudgetTrackinngID = " & mBudgetTrackinngID)

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