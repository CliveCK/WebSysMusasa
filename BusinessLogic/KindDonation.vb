Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class KindDonation

#region "Variables"

    Protected mKindDonationID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mReceivedDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mDonation As string
    Protected mQuantinty As string
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

    Public  Property KindDonationID() As long
        Get
		return mKindDonationID
        End Get
        Set(ByVal value As long)
		mKindDonationID = value
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

    Public  Property ReceivedDate() As string
        Get
		return mReceivedDate
        End Get
        Set(ByVal value As string)
		mReceivedDate = value
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

    Public  Property Donation() As string
        Get
		return mDonation
        End Get
        Set(ByVal value As string)
		mDonation = value
        End Set
    End Property

    Public  Property Quantinty() As string
        Get
		return mQuantinty
        End Get
        Set(ByVal value As string)
		mQuantinty = value
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

    KindDonationID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mReceivedDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mDonation = ""
    mQuantinty = ""
    mReceivedFrom = ""
        mPurpose = ""
        mReceiptNo = ""

    End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mKindDonationID) 

    End Function 

    Public Overridable Function Retrieve(ByVal KindDonationID As Long) As Boolean 

        Dim sql As String 

        If KindDonationID > 0 Then 
            sql = "SELECT * FROM tblKindDonation WHERE KindDonationID = " & KindDonationID
        Else 
            sql = "SELECT * FROM tblKindDonation WHERE KindDonationID = " & mKindDonationID
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

                log.error("KindDonation not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetKindDonation() As System.Data.DataSet

        Return GetKindDonation(mKindDonationID)

    End Function

    Public Overridable Function GetKindDonation(ByVal KindDonationID As Long) As DataSet

        Dim sql As String

        If KindDonationID > 0 Then
            sql = "SELECT * FROM tblKindDonation WHERE KindDonationID = " & KindDonationID
        Else
            sql = "SELECT * FROM tblKindDonation WHERE KindDonationID = " & mKindDonationID
        End If

        Return GetKindDonation(sql)

    End Function

    Public Overridable Function GetKindDonation(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mKindDonationID = Catchnull(.Item("KindDonationID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mReceivedDate = Catchnull(.Item("ReceivedDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mDonation = Catchnull(.Item("Donation"), "")
            mQuantinty = Catchnull(.Item("Quantinty"), "")
            mReceivedFrom = Catchnull(.Item("ReceivedFrom"), "")
            mPurpose = Catchnull(.Item("Purpose"), "")
            mReceiptNo = Catchnull(.Item("ReceiptNo"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@KindDonationID", DBType.Int32, mKindDonationID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ReceivedDate", DBType.String, mReceivedDate)
        db.AddInParameter(cmd, "@Donation", DBType.String, mDonation)
        db.AddInParameter(cmd, "@Quantinty", DBType.String, mQuantinty)
        db.AddInParameter(cmd, "@ReceivedFrom", DBType.String, mReceivedFrom)
        db.AddInParameter(cmd, "@Purpose", DbType.String, mPurpose)
        db.AddInParameter(cmd, "@ReceiptNo", DbType.String, mReceiptNo)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_KindDonation")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mKindDonationID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblKindDonation SET Deleted = 1 WHERE KindDonationID = " & mKindDonationID) 
        Return Delete("DELETE FROM tblKindDonation WHERE KindDonationID = " & mKindDonationID)

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