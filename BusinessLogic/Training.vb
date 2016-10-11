Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Training

#region "Variables"

    Protected mTrainingID As long
    Protected mTrainingTypeID As Long
    Protected mFromDate As String
    Protected mToDate As String
    Protected mCreatedBy As long
    Protected mUpdatedBy As Long
    Protected mOrganizationID As Long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mName As string
    Protected mDescription As string
    Protected mLocation As String
    Protected mFacilitators As String

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

    Public  Property TrainingID() As long
        Get
		return mTrainingID
        End Get
        Set(ByVal value As long)
		mTrainingID = value
        End Set
    End Property

    Public  Property TrainingTypeID() As long
        Get
		return mTrainingTypeID
        End Get
        Set(ByVal value As long)
		mTrainingTypeID = value
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

    Public Property OrganizationID() As Long
        Get
            Return mOrganizationID
        End Get
        Set(ByVal value As Long)
            mOrganizationID = value
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

    Public Property FromDate() As String
        Get
            Return mFromDate
        End Get
        Set(ByVal value As String)
            mFromDate = value
        End Set
    End Property

    Public Property ToDate() As String
        Get
            Return mToDate
        End Get
        Set(ByVal value As String)
            mToDate = value
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

    Public Property Facilitators() As String
        Get
            Return mFacilitators
        End Get
        Set(ByVal value As String)
            mFacilitators = value
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

    Public  Property Location() As string
        Get
		return mLocation
        End Get
        Set(ByVal value As string)
		mLocation = value
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

    TrainingID = 0
    mTrainingTypeID = 0
    mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mOrganizationID = 0
    mCreatedDate = ""
    mUpdatedDate = ""
        mName = ""
        mFromDate = ""
        mToDate = ""
    mDescription = ""
        mLocation = ""
        mFacilitators = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mTrainingID) 

    End Function 

    Public Overridable Function Retrieve(ByVal TrainingID As Long) As Boolean 

        Dim sql As String 

        If TrainingID > 0 Then 
            sql = "SELECT * FROM tblTrainings WHERE TrainingID = " & TrainingID
        Else 
            sql = "SELECT * FROM tblTrainings WHERE TrainingID = " & mTrainingID
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

                log.Warn("Training not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Function RetriveAll()

        Dim sql As String = "SELECT * FROM tblTrainings"

        Return GetTraining(sql)

    End Function

    Public Overridable Function GetTraining() As System.Data.DataSet

        Return GetTraining(mTrainingID)

    End Function

    Public Overridable Function GetTraining(ByVal TrainingID As Long) As DataSet

        Dim sql As String

        If TrainingID > 0 Then
            sql = "SELECT * FROM tblTrainings WHERE TrainingID = " & TrainingID
        Else
            sql = "SELECT * FROM tblTrainings WHERE TrainingID = " & mTrainingID
        End If

        Return GetTraining(sql)

    End Function

    Public Overridable Function GetTraining(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mTrainingID = Catchnull(.Item("TrainingID"), 0)
            mTrainingTypeID = Catchnull(.Item("TrainingTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mOrganizationID = Catchnull(.Item("OrganizationID"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mName = Catchnull(.Item("Name"), "")
            mFromDate = Catchnull(.Item("FromDate"), "")
            mToDate = Catchnull(.Item("ToDate"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mLocation = Catchnull(.Item("Location"), "")
            mFacilitators = Catchnull(.Item("Facilitator"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@TrainingID", DBType.Int32, mTrainingID)
        db.AddInParameter(cmd, "@TrainingTypeID", DBType.Int32, mTrainingTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@OrganizationID", DbType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@Name", DbType.String, mName)
        db.AddInParameter(cmd, "@FromDate", DbType.String, mFromDate)
        db.AddInParameter(cmd, "@ToDate", DbType.String, mToDate)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)
        db.AddInParameter(cmd, "@Location", DbType.String, mLocation)
        db.AddInParameter(cmd, "@Facilitator", DbType.String, mFacilitators)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Training")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mTrainingID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblTrainings SET Deleted = 1 WHERE TrainingID = " & mTrainingID) 
        Return Delete("DELETE FROM tblTrainings WHERE TrainingID = " & mTrainingID)

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