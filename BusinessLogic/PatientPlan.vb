Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class PatientPlan

#region "Variables"

    Protected mPatientPlanID As long
    Protected mPatientID As long
    Protected mNeed As long
    Protected mAction As long
    Protected mServiceProvider As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mPlanDate As String
    Protected mActualDate As String
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mCost As decimal
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

    Public  Property PatientPlanID() As long
        Get
		return mPatientPlanID
        End Get
        Set(ByVal value As long)
		mPatientPlanID = value
        End Set
    End Property

    Public  Property PatientID() As long
        Get
		return mPatientID
        End Get
        Set(ByVal value As long)
		mPatientID = value
        End Set
    End Property

    Public  Property Need() As long
        Get
		return mNeed
        End Get
        Set(ByVal value As long)
		mNeed = value
        End Set
    End Property

    Public  Property Action() As long
        Get
		return mAction
        End Get
        Set(ByVal value As long)
		mAction = value
        End Set
    End Property

    Public  Property ServiceProvider() As long
        Get
		return mServiceProvider
        End Get
        Set(ByVal value As long)
		mServiceProvider = value
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

    Public  Property PlanDate() As string
        Get
		return mPlanDate
        End Get
        Set(ByVal value As string)
		mPlanDate = value
        End Set
    End Property

    Public Property ActualDate() As String
        Get
            Return mActualDate
        End Get
        Set(ByVal value As String)
            mActualDate = value
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

    Public  Property Cost() As decimal
        Get
		return mCost
        End Get
        Set(ByVal value As decimal)
		mCost = value
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

    PatientPlanID = 0
    mPatientID = 0
    mNeed = 0
    mAction = 0
    mServiceProvider = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mPlanDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mCost = 0.0
        mComments = ""
        mActualDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mPatientPlanID) 

    End Function 

    Public Overridable Function Retrieve(ByVal PatientPlanID As Long) As Boolean 

        Dim sql As String 

        If PatientPlanID > 0 Then 
            sql = "SELECT * FROM tblPatientPlan WHERE PatientPlanID = " & PatientPlanID
        Else 
            sql = "SELECT * FROM tblPatientPlan WHERE PatientPlanID = " & mPatientPlanID
        End If 

        Return Retrieve(sql) 

    End Function

    Public Overridable Function RetrieveByPatient(ByVal PatientID As Long) As Boolean

        Dim sql As String

        If PatientID > 0 Then
            sql = "SELECT * FROM tblPatientPlan WHERE PatientID = " & PatientID
        Else
            sql = "SELECT * FROM tblPatientPlan WHERE PatientID = " & mPatientID
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

                log.error("PatientPlan not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetPatientPlan() As System.Data.DataSet

        Return GetPatientPlan(mPatientPlanID)

    End Function

    Public Function GetPatientPlanDetails(ByVal PatientID As Long) As System.Data.DataSet

        Dim sql As String

        sql = "SELECT PatientPlanID, PatientID, PlanDate, ActualDate, NA.Description As [Action], O.Name As ServiceProvider, Cost, Comments, NS.Description As Need, NC.Description AS NeedCategory "
        sql &= "FROM tblPatientPlan P left outer join luNeedServices NS "
        sql &= "on NS.NeedServiceID = P.Need left outer join luNeedCategory NC on NC.NeedCategoryID = NS.NeedCategoryID "
        sql &= "left outer join tblOrganization O on O.OrganizationID = P.ServiceProvider "
        sql &= "left outer join luNeedAction NA on NA.NeedActionID = P.Action "
        sql &= "WHERE PatientID = " & PatientID

        Return GetPatientPlan(sql)

    End Function

    Public Overridable Function GetPatientPlan(ByVal PatientPlanID As Long) As DataSet

        Dim sql As String

        If PatientPlanID > 0 Then
            sql = "SELECT * FROM tblPatientPlan WHERE PatientPlanID = " & PatientPlanID
        Else
            sql = "SELECT * FROM tblPatientPlan WHERE PatientPlanID = " & mPatientPlanID
        End If

        Return GetPatientPlan(sql)

    End Function

    Public Overridable Function GetPatientPlan(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mPatientPlanID = Catchnull(.Item("PatientPlanID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mNeed = Catchnull(.Item("Need"), 0)
            mAction = Catchnull(.Item("Action"), 0)
            mServiceProvider = Catchnull(.Item("ServiceProvider"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mPlanDate = Catchnull(.Item("PlanDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mCost = Catchnull(.Item("Cost"), 0.0)
            mComments = Catchnull(.Item("Comments"), "")
            mActualDate = Catchnull(.Item("ActualDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@PatientPlanID", DBType.Int32, mPatientPlanID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@Need", DBType.Int32, mNeed)
        db.AddInParameter(cmd, "@Action", DBType.Int32, mAction)
        db.AddInParameter(cmd, "@ServiceProvider", DBType.Int32, mServiceProvider)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@PlanDate", DBType.String, mPlanDate)
        db.AddInParameter(cmd, "@Cost", DbType.Decimal, mCost)
        db.AddInParameter(cmd, "@Comments", DbType.String, mComments)
        db.AddInParameter(cmd, "@ActualDate", DbType.String, mActualDate)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_PatientPlan")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mPatientPlanID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblPatientPlan SET Deleted = 1 WHERE PatientPlanID = " & mPatientPlanID) 
        Return Delete("DELETE FROM tblPatientPlan WHERE PatientPlanID = " & mPatientPlanID)

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