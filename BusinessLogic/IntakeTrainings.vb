Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class IntakeTrainings

#region "Variables"

    Protected mIntakeTrainingID As long
    Protected mIntakeID As long
    Protected mTrainingID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

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

    Public  Property IntakeTrainingID() As long
        Get
		return mIntakeTrainingID
        End Get
        Set(ByVal value As long)
		mIntakeTrainingID = value
        End Set
    End Property

    Public  Property IntakeID() As long
        Get
		return mIntakeID
        End Get
        Set(ByVal value As long)
		mIntakeID = value
        End Set
    End Property

    Public  Property TrainingID() As long
        Get
		return mTrainingID
        End Get
        Set(ByVal value As long)
		mTrainingID = value
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

    IntakeTrainingID = 0
    mIntakeID = 0
    mTrainingID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mIntakeTrainingID) 

    End Function 

    Public Overridable Function Retrieve(ByVal IntakeTrainingID As Long) As Boolean 

        Dim sql As String 

        If IntakeTrainingID > 0 Then 
            sql = "SELECT * FROM tblIntakeTrainings WHERE IntakeTrainingID = " & IntakeTrainingID
        Else 
            sql = "SELECT * FROM tblIntakeTrainings WHERE IntakeTrainingID = " & mIntakeTrainingID
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

                log.error("IntakeTrainings not found.") 

                Return False 

            End If 

        Catch e As Exception 

            log.error(e) 
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetIntakeTrainings() As System.Data.DataSet

        Return GetIntakeTrainings(mIntakeTrainingID) 

    End Function 

    Public Overridable Function GetIntakeTrainings(ByVal IntakeTrainingID As Long) As DataSet 

        Dim sql As String 

        If IntakeTrainingID > 0 Then 
            sql = "SELECT * FROM tblIntakeTrainings WHERE IntakeTrainingID = " & IntakeTrainingID
        Else 
            sql = "SELECT * FROM tblIntakeTrainings WHERE IntakeTrainingID = " & mIntakeTrainingID
        End If 

        Return GetIntakeTrainings(sql) 

    End Function 

    Protected Overridable Function GetIntakeTrainings(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function 

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mIntakeTrainingID = Catchnull(.Item("IntakeTrainingID"), 0)
            mIntakeID = Catchnull(.Item("IntakeID"), 0)
            mTrainingID = Catchnull(.Item("TrainingID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@IntakeTrainingID", DBType.Int32, mIntakeTrainingID) 
        db.AddInParameter(cmd, "@IntakeID", DBType.Int32, mIntakeID) 
        db.AddInParameter(cmd, "@TrainingID", DBType.Int32, mTrainingID) 
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_IntakeTrainings") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mIntakeTrainingID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblIntakeTrainings SET Deleted = 1 WHERE IntakeTrainingID = " & mIntakeTrainingID) 
        Return Delete("DELETE FROM tblIntakeTrainings WHERE IntakeTrainingID = " & mIntakeTrainingID)

    End Function

    Public Overridable Function DeleteEntries() As Boolean

        'Return Delete("UPDATE tblIntakeTrainings SET Deleted = 1 WHERE IntakeTrainingID = " & mIntakeTrainingID) 
        Return Delete("DELETE FROM tblIntakeTrainings WHERE IntakeID = " & mIntakeID & " AND TrainingID = " & mTrainingID)

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