Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Appointments

#region "Variables"

    Protected mID As long
    Protected mUserID As long
    Protected mRoomID As long
    Protected mRecurrenceParentID As long
    Protected mActivityID As long
    Protected mStart As string
    Protected mEnd As string
    Protected mPercentComplete As decimal
    Protected mCompleted As string
    Protected mSubject As string
    Protected mRecurrenceRule As string
    Protected mAnnotations As string
    Protected mDescription As string
    Protected mReminder As string
    Protected mLastModified As string

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

    Public  Property ID() As long
        Get
		return mID
        End Get
        Set(ByVal value As long)
		mID = value
        End Set
    End Property

    Public  Property UserID() As long
        Get
		return mUserID
        End Get
        Set(ByVal value As long)
		mUserID = value
        End Set
    End Property

    Public  Property RoomID() As long
        Get
		return mRoomID
        End Get
        Set(ByVal value As long)
		mRoomID = value
        End Set
    End Property

    Public  Property RecurrenceParentID() As long
        Get
		return mRecurrenceParentID
        End Get
        Set(ByVal value As long)
		mRecurrenceParentID = value
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

    Public  Property Start() As string
        Get
		return mStart
        End Get
        Set(ByVal value As string)
		mStart = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return mEnd
        End Get
        Set(ByVal value As String)
            mEnd = value
        End Set
    End Property

    Public  Property PercentComplete() As decimal
        Get
		return mPercentComplete
        End Get
        Set(ByVal value As decimal)
		mPercentComplete = value
        End Set
    End Property

    Public  Property Completed() As string
        Get
		return mCompleted
        End Get
        Set(ByVal value As string)
		mCompleted = value
        End Set
    End Property

    Public  Property Subject() As string
        Get
		return mSubject
        End Get
        Set(ByVal value As string)
		mSubject = value
        End Set
    End Property

    Public  Property RecurrenceRule() As string
        Get
		return mRecurrenceRule
        End Get
        Set(ByVal value As string)
		mRecurrenceRule = value
        End Set
    End Property

    Public  Property Annotations() As string
        Get
		return mAnnotations
        End Get
        Set(ByVal value As string)
		mAnnotations = value
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

    Public  Property Reminder() As string
        Get
		return mReminder
        End Get
        Set(ByVal value As string)
		mReminder = value
        End Set
    End Property

    Public  Property LastModified() As string
        Get
		return mLastModified
        End Get
        Set(ByVal value As string)
		mLastModified = value
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

    ID = 0
    mUserID = 0
    mRoomID = 0
    mRecurrenceParentID = 0
    mActivityID = 0
    mStart = ""
    mEnd = ""
    mPercentComplete = 0.0
    mCompleted = ""
    mSubject = ""
    mRecurrenceRule = ""
    mAnnotations = ""
    mDescription = ""
    mReminder = ""
    mLastModified = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ID As Long) As Boolean 

        Dim sql As String 

        If ID > 0 Then 
            sql = "SELECT * FROM Appointments WHERE ID = " & ID
        Else 
            sql = "SELECT * FROM Appointments WHERE ID = " & mID
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

                log.error("Appointments not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetAppointments() As System.Data.DataSet

        Return GetAppointments(mID)

    End Function

    Public Overridable Function GetAppointments(ByVal ID As Long) As DataSet

        Dim sql As String

        If ID > 0 Then
            sql = "SELECT * FROM Appointments WHERE ID = " & ID
        Else
            sql = "SELECT * FROM Appointments WHERE ID = " & mID
        End If

        Return GetAppointments(sql)

    End Function

    Protected Overridable Function GetAppointments(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mID = Catchnull(.Item("ID"), 0)
            mUserID = Catchnull(.Item("UserID"), 0)
            mRoomID = Catchnull(.Item("RoomID"), 0)
            mRecurrenceParentID = Catchnull(.Item("RecurrenceParentID"), 0)
            mActivityID = Catchnull(.Item("ActivityID"), 0)
            mStart = Catchnull(.Item("Start"), "")
            mEnd = Catchnull(.Item("End"), "")
            mPercentComplete = Catchnull(.Item("PercentComplete"), 0.0)
            mCompleted = Catchnull(.Item("Completed"), "")
            mSubject = Catchnull(.Item("Subject"), "")
            mRecurrenceRule = Catchnull(.Item("RecurrenceRule"), "")
            mAnnotations = Catchnull(.Item("Annotations"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mReminder = Catchnull(.Item("Reminder"), "")
            mLastModified = Catchnull(.Item("LastModified"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ID", DBType.Int32, mID)
        db.AddInParameter(cmd, "@UserID", DBType.Int32, mUserID)
        db.AddInParameter(cmd, "@RoomID", DBType.Int32, mRoomID)
        db.AddInParameter(cmd, "@RecurrenceParentID", DBType.Int32, mRecurrenceParentID)
        db.AddInParameter(cmd, "@ActivityID", DBType.Int32, mActivityID)
        db.AddInParameter(cmd, "@Start", DBType.String, mStart)
        db.AddInParameter(cmd, "@End", DBType.String, mEnd)
        db.AddInParameter(cmd, "@PercentComplete", DbType.Decimal, mPercentComplete)
        db.AddInParameter(cmd, "@Completed", DBType.String, mCompleted)
        db.AddInParameter(cmd, "@Subject", DBType.String, mSubject)
        db.AddInParameter(cmd, "@RecurrenceRule", DBType.String, mRecurrenceRule)
        db.AddInParameter(cmd, "@Annotations", DBType.String, mAnnotations)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)
        db.AddInParameter(cmd, "@Reminder", DBType.String, mReminder)
        db.AddInParameter(cmd, "@LastModified", DBType.String, mLastModified)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Appointments")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE Appointments SET Deleted = 1 WHERE ID = " & mID) 
        Return Delete("DELETE FROM Appointments WHERE ID = " & mID)

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