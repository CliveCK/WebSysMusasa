Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class CounsellingSessionActivities

#region "Variables"

    Protected mClientSessionActivityID As long
    Protected mBeneficiaryID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mActivityDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mActivity As string
    Protected mDescription As string
    Protected mOutcome As string
    Protected mRemarks As string

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

    Public  Property ClientSessionActivityID() As long
        Get
		return mClientSessionActivityID
        End Get
        Set(ByVal value As long)
		mClientSessionActivityID = value
        End Set
    End Property

    Public  Property BeneficiaryID() As long
        Get
		return mBeneficiaryID
        End Get
        Set(ByVal value As long)
		mBeneficiaryID = value
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

    Public  Property ActivityDate() As string
        Get
		return mActivityDate
        End Get
        Set(ByVal value As string)
		mActivityDate = value
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

    Public  Property Activity() As string
        Get
		return mActivity
        End Get
        Set(ByVal value As string)
		mActivity = value
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

    Public  Property Outcome() As string
        Get
		return mOutcome
        End Get
        Set(ByVal value As string)
		mOutcome = value
        End Set
    End Property

    Public  Property Remarks() As string
        Get
		return mRemarks
        End Get
        Set(ByVal value As string)
		mRemarks = value
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

    ClientSessionActivityID = 0
    mBeneficiaryID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mActivityDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mActivity = ""
    mDescription = ""
    mOutcome = ""
    mRemarks = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mClientSessionActivityID) 

    End Function

    Public Overridable Function Retrieve(ByVal ClientSessionActivityID As Long) As Boolean

        Dim sql As String

        If ClientSessionActivityID > 0 Then
            sql = "SELECT * FROM tblCounsellingSessionActivities WHERE ClientSessionActivityID = " & ClientSessionActivityID
        Else
            sql = "SELECT * FROM tblCounsellingSessionActivities WHERE ClientSessionActivityID = " & mClientSessionActivityID
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

                log.Error("CounsellingSessionActivities not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCounsellingSessionActivities() As System.Data.DataSet

        Return GetCounsellingSessionActivities(mClientSessionActivityID)

    End Function

    Public Overridable Function GetCounsellingSessionActivities(ByVal ClientSessionActivityID As Long) As DataSet

        Dim sql As String

        If ClientSessionActivityID > 0 Then
            sql = "SELECT * FROM tblCounsellingSessionActivities WHERE ClientSessionActivityID = " & ClientSessionActivityID
        Else
            sql = "SELECT * FROM tblCounsellingSessionActivities WHERE ClientSessionActivityID = " & mClientSessionActivityID
        End If

        Return GetCounsellingSessionActivities(sql)

    End Function

    Public Overridable Function GetCounsellingSessionActivitiesByBeneficiaryID(ByVal BeneficiaryID As Long) As DataSet

        Dim sql As String

        sql = "SELECT * FROM tblCounsellingSessionActivities WHERE BeneficiaryID = " & BeneficiaryID

        Return GetCounsellingSessionActivities(sql)

    End Function

    Protected Overridable Function GetCounsellingSessionActivities(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mClientSessionActivityID = Catchnull(.Item("ClientSessionActivityID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mActivityDate = Catchnull(.Item("ActivityDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mActivity = Catchnull(.Item("Activity"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mOutcome = Catchnull(.Item("Outcome"), "")
            mRemarks = Catchnull(.Item("Remarks"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ClientSessionActivityID", DbType.Int32, mClientSessionActivityID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ActivityDate", DbType.String, mActivityDate)
        db.AddInParameter(cmd, "@Activity", DbType.String, mActivity)
        db.AddInParameter(cmd, "@Description", DbType.String, mDescription)
        db.AddInParameter(cmd, "@Outcome", DbType.String, mOutcome)
        db.AddInParameter(cmd, "@Remarks", DbType.String, mRemarks)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_CounsellingSessionActivities")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mClientSessionActivityID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblCounsellingSessionActivities SET Deleted = 1 WHERE ClientSessionActivityID = " & mClientSessionActivityID) 
        Return Delete("DELETE FROM tblCounsellingSessionActivities WHERE ClientSessionActivityID = " & mClientSessionActivityID)

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