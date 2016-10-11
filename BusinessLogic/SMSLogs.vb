Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class SMSLogs 

#region "Variables"

    Protected mSMSLogID As long
    Protected mSenderID As long
    Protected mProcessingCount As long
    Protected mTimeSent As string
    Protected mSMSRequestID As string
    Protected mReceiverAddress As string
    Protected mSMSMessage As string
    Protected mStatus As string
    Protected mDeliveryStatus As string

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

    Public  Property SMSLogID() As long
        Get
		return mSMSLogID
        End Get
        Set(ByVal value As long)
		mSMSLogID = value
        End Set
    End Property

    Public  Property SenderID() As long
        Get
		return mSenderID
        End Get
        Set(ByVal value As long)
		mSenderID = value
        End Set
    End Property

    Public  Property ProcessingCount() As long
        Get
		return mProcessingCount
        End Get
        Set(ByVal value As long)
		mProcessingCount = value
        End Set
    End Property

    Public  Property TimeSent() As string
        Get
		return mTimeSent
        End Get
        Set(ByVal value As string)
		mTimeSent = value
        End Set
    End Property

    Public  Property SMSRequestID() As string
        Get
		return mSMSRequestID
        End Get
        Set(ByVal value As string)
		mSMSRequestID = value
        End Set
    End Property

    Public  Property ReceiverAddress() As string
        Get
		return mReceiverAddress
        End Get
        Set(ByVal value As string)
		mReceiverAddress = value
        End Set
    End Property

    Public  Property SMSMessage() As string
        Get
		return mSMSMessage
        End Get
        Set(ByVal value As string)
		mSMSMessage = value
        End Set
    End Property

    Public  Property Status() As string
        Get
		return mStatus
        End Get
        Set(ByVal value As string)
		mStatus = value
        End Set
    End Property

    Public  Property DeliveryStatus() As string
        Get
		return mDeliveryStatus
        End Get
        Set(ByVal value As string)
		mDeliveryStatus = value
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

    SMSLogID = 0
    mSenderID = 0
    mProcessingCount = 0
    mTimeSent = ""
    mSMSRequestID = ""
    mReceiverAddress = ""
    mSMSMessage = ""
    mStatus = ""
    mDeliveryStatus = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mSMSLogID) 

    End Function 

    Public Overridable Function Retrieve(ByVal SMSLogID As Long) As Boolean 

        Dim sql As String 

        If SMSLogID > 0 Then 
            sql = "SELECT * FROM tblSMSLogs WHERE SMSLogID = " & SMSLogID
        Else 
            sql = "SELECT * FROM tblSMSLogs WHERE SMSLogID = " & mSMSLogID
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

                log.Error("SMSLogs not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetSMSLogs() As System.Data.DataSet

        Return GetSMSLogs(mSMSLogID)

    End Function

    Public Overridable Function GetSMSLogs(ByVal SMSLogID As Long) As DataSet

        Dim sql As String

        If SMSLogID > 0 Then
            sql = "SELECT * FROM tblSMSLogs WHERE SMSLogID = " & SMSLogID
        Else
            sql = "SELECT * FROM tblSMSLogs WHERE SMSLogID = " & mSMSLogID
        End If

        Return GetSMSLogs(sql)

    End Function

    Protected Overridable Function GetSMSLogs(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mSMSLogID = Catchnull(.Item("SMSLogID"), 0)
            mSenderID = Catchnull(.Item("SenderID"), 0)
            mProcessingCount = Catchnull(.Item("ProcessingCount"), 0)
            mTimeSent = Catchnull(.Item("TimeSent"), "")
            mSMSRequestID = Catchnull(.Item("SMSRequestID"), "")
            mReceiverAddress = Catchnull(.Item("ReceiverAddress"), "")
            mSMSMessage = Catchnull(.Item("SMSMessage"), "")
            mStatus = Catchnull(.Item("Status"), "")
            mDeliveryStatus = Catchnull(.Item("DeliveryStatus"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@SMSLogID", DbType.Int32, mSMSLogID)
        db.AddInParameter(cmd, "@SenderID", DbType.Int32, mSenderID)
        db.AddInParameter(cmd, "@ProcessingCount", DbType.Int32, mProcessingCount)
        db.AddInParameter(cmd, "@TimeSent", DbType.String, mTimeSent)
        db.AddInParameter(cmd, "@SMSRequestID", DbType.String, mSMSRequestID)
        db.AddInParameter(cmd, "@ReceiverAddress", DbType.String, mReceiverAddress)
        db.AddInParameter(cmd, "@SMSMessage", DbType.String, mSMSMessage)
        db.AddInParameter(cmd, "@Status", DbType.String, mStatus)
        db.AddInParameter(cmd, "@DeliveryStatus", DbType.String, mDeliveryStatus)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_SMSLogs")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mSMSLogID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblSMSLogs SET Deleted = 1 WHERE SMSLogID = " & mSMSLogID) 
        Return Delete("DELETE FROM tblSMSLogs WHERE SMSLogID = " & mSMSLogID)

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