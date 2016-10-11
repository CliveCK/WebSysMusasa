Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class CashDonation

#region "Variables"

    Protected mCashDonationID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mFundraisingDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mAmountReceived As decimal
    Protected mReceivedFrom As String
    Protected mPurpose As String
    Protected mReceiptNo As String

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

    Public  Property CashDonationID() As long
        Get
		return mCashDonationID
        End Get
        Set(ByVal value As long)
		mCashDonationID = value
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

    Public  Property FundraisingDate() As string
        Get
		return mFundraisingDate
        End Get
        Set(ByVal value As string)
		mFundraisingDate = value
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

    Public  Property AmountReceived() As decimal
        Get
		return mAmountReceived
        End Get
        Set(ByVal value As decimal)
		mAmountReceived = value
        End Set
    End Property

    Public  Property ReceivedFrom() As string
        Get
		return mReceivedFrom
        End Get
        Set(ByVal value As string)
		mReceivedFrom = value
        End Set
    End Property

    Public Property Purpose() As String
        Get
            Return mPurpose
        End Get
        Set(ByVal value As String)
            mPurpose = value
        End Set
    End Property

    Public Property ReceiptNo() As String
        Get
            Return mReceiptNo
        End Get
        Set(ByVal value As String)
            mReceiptNo = value
        End Set
    End Property

#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long) 

        mObjectUserID = ObjectUserID 
        mConnectionName = ConnectionName 
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub 

#End Region 

Public Sub Clear()  

    CashDonationID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mFundraisingDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mAmountReceived = 0.0
    mReceivedFrom = ""
        mPurpose = ""
        mReceiptNo = ""

    End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCashDonationID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CashDonationID As Long) As Boolean 

        Dim sql As String 

        If CashDonationID > 0 Then 
            sql = "SELECT * FROM tblCashDonations WHERE CashDonationID = " & CashDonationID
        Else 
            sql = "SELECT * FROM tblCashDonations WHERE CashDonationID = " & mCashDonationID
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

                log.error("CashDonation not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCashDonation() As System.Data.DataSet

        Return GetCashDonation(mCashDonationID)

    End Function

    Public Overridable Function GetCashDonation(ByVal CashDonationID As Long) As DataSet

        Dim sql As String

        If CashDonationID > 0 Then
            sql = "SELECT * FROM tblCashDonations WHERE CashDonationID = " & CashDonationID
        Else
            sql = "SELECT * FROM tblCashDonations WHERE CashDonationID = " & mCashDonationID
        End If

        Return GetCashDonation(sql)

    End Function

    Public Overridable Function GetCashDonation(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mCashDonationID = Catchnull(.Item("CashDonationID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mFundraisingDate = Catchnull(.Item("FundraisingDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mAmountReceived = Catchnull(.Item("AmountReceived"), 0.0)
            mReceivedFrom = Catchnull(.Item("ReceivedFrom"), "")
            mPurpose = Catchnull(.Item("Purpose"), "")
            mReceiptNo = Catchnull(.Item("ReceiptNo"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@CashDonationID", DBType.Int32, mCashDonationID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@FundraisingDate", DBType.String, mFundraisingDate)
        db.AddInParameter(cmd, "@AmountReceived", DbType.Decimal, mAmountReceived)
        db.AddInParameter(cmd, "@ReceivedFrom", DBType.String, mReceivedFrom)
        db.AddInParameter(cmd, "@Purpose", DbType.String, mPurpose)
        db.AddInParameter(cmd, "@ReceiptNo", DbType.String, mReceiptNo)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_CashDonation")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mCashDonationID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblCashDonations SET Deleted = 1 WHERE CashDonationID = " & mCashDonationID) 
        Return Delete("DELETE FROM tblCashDonations WHERE CashDonationID = " & mCashDonationID)

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