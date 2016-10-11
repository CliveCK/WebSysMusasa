Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class DepartmentalPlan

#region "Variables"

    Protected mDepartmentPlanID As long
    Protected mOrganizationPlanID As long
    Protected mYear As long
    Protected mPeriod As long
    Protected mStatusID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mActivityCategory As string
    Protected mActivity As string
    Protected mComments As string

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

    Public  Property DepartmentPlanID() As long
        Get
		return mDepartmentPlanID
        End Get
        Set(ByVal value As long)
		mDepartmentPlanID = value
        End Set
    End Property

    Public  Property OrganizationPlanID() As long
        Get
		return mOrganizationPlanID
        End Get
        Set(ByVal value As long)
		mOrganizationPlanID = value
        End Set
    End Property

    Public  Property Year() As long
        Get
		return mYear
        End Get
        Set(ByVal value As long)
		mYear = value
        End Set
    End Property

    Public  Property Period() As long
        Get
		return mPeriod
        End Get
        Set(ByVal value As long)
		mPeriod = value
        End Set
    End Property

    Public  Property StatusID() As long
        Get
		return mStatusID
        End Get
        Set(ByVal value As long)
		mStatusID = value
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

    Public  Property ActivityCategory() As string
        Get
		return mActivityCategory
        End Get
        Set(ByVal value As string)
		mActivityCategory = value
        End Set
    End Property

    Public  Property Activity() As string
        Get
		return mActivity
        End Get
        Set(ByVal value As string)
		mActivity = value
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

    DepartmentPlanID = 0
    mOrganizationPlanID = 0
    mYear = 0
    mPeriod = 0
    mStatusID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mActivityCategory = ""
    mActivity = ""
    mComments = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mDepartmentPlanID) 

    End Function 

    Public Overridable Function Retrieve(ByVal DepartmentPlanID As Long) As Boolean 

        Dim sql As String 

        If DepartmentPlanID > 0 Then 
            sql = "SELECT * FROM tblDepartmentalPlan WHERE DepartmentPlanID = " & DepartmentPlanID
        Else 
            sql = "SELECT * FROM tblDepartmentalPlan WHERE DepartmentPlanID = " & mDepartmentPlanID
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

                log.Warn("DepartmentalPlan not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Function RetrieveAll() As DataSet

        Dim sql As String = "SELECT * FROM tblDepartmentalPlan"

        Return GetDepartmentalPlan(sql)

    End Function


    Public Overridable Function GetDepartmentalPlan() As System.Data.DataSet

        Return GetDepartmentalPlan(mDepartmentPlanID)

    End Function

    Public Overridable Function GetDepartmentalPlan(ByVal DepartmentPlanID As Long) As DataSet

        Dim sql As String

        If DepartmentPlanID > 0 Then
            sql = "SELECT * FROM tblDepartmentalPlan WHERE DepartmentPlanID = " & DepartmentPlanID
        Else
            sql = "SELECT * FROM tblDepartmentalPlan WHERE DepartmentPlanID = " & mDepartmentPlanID
        End If

        Return GetDepartmentalPlan(sql)

    End Function

    Protected Overridable Function GetDepartmentalPlan(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mDepartmentPlanID = Catchnull(.Item("DepartmentPlanID"), 0)
            mOrganizationPlanID = Catchnull(.Item("OrganizationPlanID"), 0)
            mYear = Catchnull(.Item("Year"), 0)
            mPeriod = Catchnull(.Item("Period"), 0)
            mStatusID = Catchnull(.Item("StatusID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mActivityCategory = Catchnull(.Item("ActivityCategory"), "")
            mActivity = Catchnull(.Item("Activity"), "")
            mComments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@DepartmentPlanID", DBType.Int32, mDepartmentPlanID)
        db.AddInParameter(cmd, "@OrganizationPlanID", DBType.Int32, mOrganizationPlanID)
        db.AddInParameter(cmd, "@Year", DBType.Int32, mYear)
        db.AddInParameter(cmd, "@Period", DBType.Int32, mPeriod)
        db.AddInParameter(cmd, "@StatusID", DBType.Int32, mStatusID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ActivityCategory", DBType.String, mActivityCategory)
        db.AddInParameter(cmd, "@Activity", DBType.String, mActivity)
        db.AddInParameter(cmd, "@Comments", DBType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_DepartmentalPlan")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mDepartmentPlanID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblDepartmentalPlan SET Deleted = 1 WHERE DepartmentPlanID = " & mDepartmentPlanID) 
        Return Delete("DELETE FROM tblDepartmentalPlan WHERE DepartmentPlanID = " & mDepartmentPlanID)

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