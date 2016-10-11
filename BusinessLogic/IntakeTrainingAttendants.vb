Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class IntakeTrainingAttendants

#region "Variables"

    Protected mIntakeTrainingAttendantID As long
    Protected mIntakeID As long
    Protected mAttendantID As long
    Protected mTrainingStatusID As long
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

    Public  Property IntakeTrainingAttendantID() As long
        Get
		return mIntakeTrainingAttendantID
        End Get
        Set(ByVal value As long)
		mIntakeTrainingAttendantID = value
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

    Public  Property AttendantID() As long
        Get
		return mAttendantID
        End Get
        Set(ByVal value As long)
		mAttendantID = value
        End Set
    End Property

    Public  Property TrainingStatusID() As long
        Get
		return mTrainingStatusID
        End Get
        Set(ByVal value As long)
		mTrainingStatusID = value
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

    IntakeTrainingAttendantID = 0
    mIntakeID = 0
    mAttendantID = 0
    mTrainingStatusID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mIntakeTrainingAttendantID) 

    End Function 

    Public Overridable Function Retrieve(ByVal IntakeTrainingAttendantID As Long) As Boolean 

        Dim sql As String 

        If IntakeTrainingAttendantID > 0 Then 
            sql = "SELECT * FROM tblIntakeTrainingAttendants WHERE IntakeTrainingAttendantID = " & IntakeTrainingAttendantID
        Else 
            sql = "SELECT * FROM tblIntakeTrainingAttendants WHERE IntakeTrainingAttendantID = " & mIntakeTrainingAttendantID
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

                log.Error("IntakeTrainingAttendants not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetIntakeTrainingAttendants() As System.Data.DataSet

        Return GetIntakeTrainingAttendants(mIntakeTrainingAttendantID)

    End Function

    Public Overridable Function GetIntakeTrainingAttendants(ByVal IntakeTrainingAttendantID As Long) As DataSet

        Dim sql As String

        If IntakeTrainingAttendantID > 0 Then
            sql = "SELECT * FROM tblIntakeTrainingAttendants WHERE IntakeTrainingAttendantID = " & IntakeTrainingAttendantID
        Else
            sql = "SELECT * FROM tblIntakeTrainingAttendants WHERE IntakeTrainingAttendantID = " & mIntakeTrainingAttendantID
        End If

        Return GetIntakeTrainingAttendants(sql)

    End Function

    Protected Overridable Function GetIntakeTrainingAttendants(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mIntakeTrainingAttendantID = Catchnull(.Item("IntakeTrainingAttendantID"), 0)
            mIntakeID = Catchnull(.Item("IntakeID"), 0)
            mAttendantID = Catchnull(.Item("AttendantID"), 0)
            mTrainingStatusID = Catchnull(.Item("TrainingStatusID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@IntakeTrainingAttendantID", DbType.Int32, mIntakeTrainingAttendantID)
        db.AddInParameter(cmd, "@IntakeID", DbType.Int32, mIntakeID)
        db.AddInParameter(cmd, "@AttendantID", DbType.Int32, mAttendantID)
        db.AddInParameter(cmd, "@TrainingStatusID", DbType.Int32, mTrainingStatusID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_IntakeTrainingAttendants")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mIntakeTrainingAttendantID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblIntakeTrainingAttendants SET Deleted = 1 WHERE IntakeTrainingAttendantID = " & mIntakeTrainingAttendantID) 
        Return Delete("DELETE FROM tblIntakeTrainingAttendants WHERE IntakeTrainingAttendantID = " & mIntakeTrainingAttendantID)

    End Function

    Public Overridable Function DeleteEntries() As Boolean

        'Return Delete("UPDATE tblIntakeTrainingAttendants SET Deleted = 1 WHERE IntakeTrainingAttendantID = " & mIntakeTrainingAttendantID) 
        Return Delete("DELETE FROM tblIntakeTrainingAttendants WHERE IntakeID = " & mIntakeID & " AND AttendantID = " & mAttendantID)

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