Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Interventions

#region "Variables"

    Protected mInterventionID As long
    Protected mSectorID As Long
    Protected mProjectID As Long
    Protected mBeneficiariesTarget As long
    Protected mActualBenficiaries As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mStartDate As string
    Protected mEndDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mName As string
    Protected mDescription As string
    Protected mDescriptionOfBeneficiaries As string

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

    Public  Property InterventionID() As long
        Get
		return mInterventionID
        End Get
        Set(ByVal value As long)
		mInterventionID = value
        End Set
    End Property

    Public Property ProjectID() As Long
        Get
            Return mProjectID
        End Get
        Set(ByVal value As Long)
            mProjectID = value
        End Set
    End Property

    Public  Property SectorID() As long
        Get
		return mSectorID
        End Get
        Set(ByVal value As long)
		mSectorID = value
        End Set
    End Property

    Public  Property BeneficiariesTarget() As long
        Get
		return mBeneficiariesTarget
        End Get
        Set(ByVal value As long)
		mBeneficiariesTarget = value
        End Set
    End Property

    Public  Property ActualBenficiaries() As long
        Get
		return mActualBenficiaries
        End Get
        Set(ByVal value As long)
		mActualBenficiaries = value
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

    Public  Property StartDate() As string
        Get
		return mStartDate
        End Get
        Set(ByVal value As string)
		mStartDate = value
        End Set
    End Property

    Public  Property EndDate() As string
        Get
		return mEndDate
        End Get
        Set(ByVal value As string)
		mEndDate = value
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

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
        End Set
    End Property

    Public  Property Description() As string
        Get
		return mDescription
        End Get
        Set(ByVal value As string)
		mDescription = value
        End Set
    End Property

    Public  Property DescriptionOfBeneficiaries() As string
        Get
		return mDescriptionOfBeneficiaries
        End Get
        Set(ByVal value As string)
		mDescriptionOfBeneficiaries = value
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

        mInterventionID = 0
        mProjectID = 0
    mSectorID = 0
    mBeneficiariesTarget = 0
    mActualBenficiaries = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mStartDate = ""
    mEndDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mName = ""
    mDescription = ""
    mDescriptionOfBeneficiaries = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mInterventionID) 

    End Function 

    Public Overridable Function Retrieve(ByVal InterventionID As Long) As Boolean 

        Dim sql As String 

        If InterventionID > 0 Then 
            sql = "SELECT * FROM tblInterventions WHERE InterventionID = " & InterventionID
        Else 
            sql = "SELECT * FROM tblInterventions WHERE InterventionID = " & mInterventionID
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

                log.Warn("Interventions not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetInterventions() As System.Data.DataSet

        Return GetInterventions(mInterventionID) 

    End Function 

    Public Overridable Function GetInterventions(ByVal InterventionID As Long) As DataSet 

        Dim sql As String 

        If InterventionID > 0 Then 
            sql = "SELECT * FROM tblInterventions WHERE InterventionID = " & InterventionID
        Else 
            sql = "SELECT * FROM tblInterventions WHERE InterventionID = " & mInterventionID
        End If 

        Return GetInterventions(sql) 

    End Function 

    Public Overridable Function GetInterventions(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mInterventionID = Catchnull(.Item("InterventionID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mSectorID = Catchnull(.Item("SectorID"), 0)
            mBeneficiariesTarget = Catchnull(.Item("BeneficiariesTarget"), 0)
            mActualBenficiaries = Catchnull(.Item("ActualBenficiaries"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mStartDate = Catchnull(.Item("StartDate"), "")
            mEndDate = Catchnull(.Item("EndDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mName = Catchnull(.Item("Name"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mDescriptionOfBeneficiaries = Catchnull(.Item("DescriptionOfBeneficiaries"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@InterventionID", DbType.Int32, mInterventionID)
        db.AddInParameter(cmd, "@ProjectID", DbType.Int32, mProjectID)
        db.AddInParameter(cmd, "@SectorID", DBType.Int32, mSectorID) 
        db.AddInParameter(cmd, "@BeneficiariesTarget", DBType.Int32, mBeneficiariesTarget) 
        db.AddInParameter(cmd, "@ActualBenficiaries", DBType.Int32, mActualBenficiaries) 
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID) 
        db.AddInParameter(cmd, "@StartDate", DBType.String, mStartDate) 
        db.AddInParameter(cmd, "@EndDate", DBType.String, mEndDate) 
        db.AddInParameter(cmd, "@Name", DBType.String, mName) 
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription) 
        db.AddInParameter(cmd, "@DescriptionOfBeneficiaries", DBType.String, mDescriptionOfBeneficiaries) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Interventions") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mInterventionID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblInterventions SET Deleted = 1 WHERE InterventionID = " & mInterventionID) 
        Return Delete("DELETE FROM tblInterventions WHERE InterventionID = " & mInterventionID) 

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