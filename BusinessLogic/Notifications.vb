Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Notifications

#region "Variables"

    Protected mNotificationID As long
    Protected mNotificationType As long
    Protected mReceiverID As long
    Protected mDocumentID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDueDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mResponseRequired As boolean
    Protected mStatus As boolean
    Protected mNotification As string
    Protected mResponse As string

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

    Public  Property NotificationID() As long
        Get
		return mNotificationID
        End Get
        Set(ByVal value As long)
		mNotificationID = value
        End Set
    End Property

    Public  Property NotificationType() As long
        Get
		return mNotificationType
        End Get
        Set(ByVal value As long)
		mNotificationType = value
        End Set
    End Property

    Public  Property ReceiverID() As long
        Get
		return mReceiverID
        End Get
        Set(ByVal value As long)
		mReceiverID = value
        End Set
    End Property

    Public  Property DocumentID() As long
        Get
		return mDocumentID
        End Get
        Set(ByVal value As long)
		mDocumentID = value
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

    Public  Property DueDate() As string
        Get
		return mDueDate
        End Get
        Set(ByVal value As string)
		mDueDate = value
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

    Public  Property ResponseRequired() As boolean
        Get
		return mResponseRequired
        End Get
        Set(ByVal value As boolean)
		mResponseRequired = value
        End Set
    End Property

    Public  Property Status() As boolean
        Get
		return mStatus
        End Get
        Set(ByVal value As boolean)
		mStatus = value
        End Set
    End Property

    Public  Property Notification() As string
        Get
		return mNotification
        End Get
        Set(ByVal value As string)
		mNotification = value
        End Set
    End Property

    Public  Property Response() As string
        Get
		return mResponse
        End Get
        Set(ByVal value As string)
		mResponse = value
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

    NotificationID = 0
    mNotificationType = 0
    mReceiverID = 0
    mDocumentID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDueDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mResponseRequired = FALSE
    mStatus = FALSE
    mNotification = ""
    mResponse = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mNotificationID) 

    End Function 

    Public Overridable Function Retrieve(ByVal NotificationID As Long) As Boolean 

        Dim sql As String 

        If NotificationID > 0 Then 
            sql = "SELECT * FROM tblNotifications WHERE NotificationID = " & NotificationID
        Else 
            sql = "SELECT * FROM tblNotifications WHERE NotificationID = " & mNotificationID
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

                log.Error("Notifications not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetNotifications() As System.Data.DataSet

        Return GetNotifications(mNotificationID)

    End Function

    Public Overridable Function GetNotifications(ByVal NotificationID As Long) As DataSet

        Dim sql As String

        If NotificationID > 0 Then
            sql = "SELECT * FROM tblNotifications WHERE NotificationID = " & NotificationID
        Else
            sql = "SELECT * FROM tblNotifications WHERE NotificationID = " & mNotificationID
        End If

        Return GetNotifications(sql)

    End Function

    Public Overridable Function GetNotificationsByUserSender(ByVal UserID As Long, ByVal Status As Long, ByVal NotificationType As Long) As DataSet

        Dim sql As String

        If UserID > 0 Then
            sql = "SELECT * FROM tblNotifications WHERE CreatedBy = " & UserID & " AND [Status] = " & Status & " AND NotificationType = " & NotificationType
        Else
            sql = "SELECT * FROM tblNotifications WHERE CreatedBy = " & mCreatedBy & " AND [Status] = " & Status & " AND NotificationType = " & NotificationType
        End If

        Return GetNotifications(sql)

    End Function

    Protected Overridable Function GetNotifications(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mNotificationID = Catchnull(.Item("NotificationID"), 0)
            mNotificationType = Catchnull(.Item("NotificationType"), 0)
            mReceiverID = Catchnull(.Item("ReceiverID"), 0)
            mDocumentID = Catchnull(.Item("DocumentID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDueDate = Catchnull(.Item("DueDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mResponseRequired = Catchnull(.Item("ResponseRequired"), False)
            mStatus = Catchnull(.Item("Status"), False)
            mNotification = Catchnull(.Item("Notification"), "")
            mResponse = Catchnull(.Item("Response"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@NotificationID", DBType.Int32, mNotificationID)
        db.AddInParameter(cmd, "@NotificationType", DBType.Int32, mNotificationType)
        db.AddInParameter(cmd, "@ReceiverID", DBType.Int32, mReceiverID)
        db.AddInParameter(cmd, "@DocumentID", DBType.Int32, mDocumentID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DueDate", DBType.String, mDueDate)
        db.AddInParameter(cmd, "@ResponseRequired", DBType.Boolean, mResponseRequired)
        db.AddInParameter(cmd, "@Status", DBType.Boolean, mStatus)
        db.AddInParameter(cmd, "@Notification", DBType.String, mNotification)
        db.AddInParameter(cmd, "@Response", DBType.String, mResponse)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Notifications")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mNotificationID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblNotifications SET Deleted = 1 WHERE NotificationID = " & mNotificationID) 
        Return Delete("DELETE FROM tblNotifications WHERE NotificationID = " & mNotificationID)

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