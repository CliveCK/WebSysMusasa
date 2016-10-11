Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Budgets

#region "Variables"

    Protected mBudgetID As long
    Protected mProjectID As long
    Protected mActivityCategoryID As long
    Protected mActivityID As long
    Protected mUnitID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mUnitCost As decimal
    Protected mNumberOfUnits As decimal

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

    Public  Property BudgetID() As long
        Get
		return mBudgetID
        End Get
        Set(ByVal value As long)
		mBudgetID = value
        End Set
    End Property

    Public  Property ProjectID() As long
        Get
		return mProjectID
        End Get
        Set(ByVal value As long)
		mProjectID = value
        End Set
    End Property

    Public  Property ActivityCategoryID() As long
        Get
		return mActivityCategoryID
        End Get
        Set(ByVal value As long)
		mActivityCategoryID = value
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

    Public  Property UnitID() As long
        Get
		return mUnitID
        End Get
        Set(ByVal value As long)
		mUnitID = value
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

    Public  Property UnitCost() As decimal
        Get
		return mUnitCost
        End Get
        Set(ByVal value As decimal)
		mUnitCost = value
        End Set
    End Property

    Public  Property NumberOfUnits() As decimal
        Get
		return mNumberOfUnits
        End Get
        Set(ByVal value As decimal)
		mNumberOfUnits = value
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

    BudgetID = 0
    mProjectID = 0
    mActivityCategoryID = 0
    mActivityID = 0
    mUnitID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mUnitCost = 0.0
    mNumberOfUnits = 0.0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mBudgetID) 

    End Function 

    Public Overridable Function Retrieve(ByVal BudgetID As Long) As Boolean 

        Dim sql As String 

        If BudgetID > 0 Then 
            sql = "SELECT * FROM tblBudgets WHERE BudgetID = " & BudgetID
        Else 
            sql = "SELECT * FROM tblBudgets WHERE BudgetID = " & mBudgetID
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

                log.Error("Budgets not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetBudgets() As System.Data.DataSet

        Return GetBudgets(mBudgetID)

    End Function

    Public Overridable Function GetBudgets(ByVal BudgetID As Long) As DataSet

        Dim sql As String

        If BudgetID > 0 Then
            sql = "SELECT * FROM tblBudgets WHERE BudgetID = " & BudgetID
        Else
            sql = "SELECT * FROM tblBudgets WHERE BudgetID = " & mBudgetID
        End If

        Return GetBudgets(sql)

    End Function

    Public Overridable Function GetBudgetsByProject(ByVal ProjectID As Long) As DataSet

        Dim objOrganization As New Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim Criteria = IIf(CookiesWrapper.OrganizationID = objOrganization.GetParentOrganizationID(), "", " AND B.CreatedBy not in (" & objOrganization.GetPermittedUsersByOrganization(CookiesWrapper.OrganizationID) & ")")
        Dim sql As String

        sql = "SELECT B.*, A.Description As Activity, AC.Description as ActivityCategory, C.Description as Unit FROM tblBudgets B "
        sql &= "inner join tblActivities A on A.ActivityID = B.ActivityID "
        sql &= "inner join tblActivityCategory AC on B.ActivityCategoryID = AC.ActivityCategoryID "
        sql &= "left outer join luCommodities C on C.CommodityID = B.UnitID WHERE ProjectID = " & IIf(ProjectID > 0, ProjectID, mProjectID) & Criteria

        Return GetBudgets(sql)

    End Function

    Public Overridable Function GetBudgets(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mBudgetID = Catchnull(.Item("BudgetID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mActivityCategoryID = Catchnull(.Item("ActivityCategoryID"), 0)
            mActivityID = Catchnull(.Item("ActivityID"), 0)
            mUnitID = Catchnull(.Item("UnitID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mUnitCost = Catchnull(.Item("UnitCost"), 0.0)
            mNumberOfUnits = Catchnull(.Item("NumberOfUnits"), 0.0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@BudgetID", DbType.Int32, mBudgetID)
        db.AddInParameter(cmd, "@ProjectID", DbType.Int32, mProjectID)
        db.AddInParameter(cmd, "@ActivityCategoryID", DbType.Int32, mActivityCategoryID)
        db.AddInParameter(cmd, "@ActivityID", DbType.Int32, mActivityID)
        db.AddInParameter(cmd, "@UnitID", DbType.Int32, mUnitID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@UnitCost", DbType.Decimal, mUnitCost)
        db.AddInParameter(cmd, "@NumberOfUnits", DbType.Decimal, mNumberOfUnits)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Budgets")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mBudgetID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblBudgets SET Deleted = 1 WHERE BudgetID = " & mBudgetID) 
        Return Delete("DELETE FROM tblBudgets WHERE BudgetID = " & mBudgetID)

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