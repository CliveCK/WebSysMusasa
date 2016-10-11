Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class MicroFinanceRepayments

#region "Variables"

    Protected mMicroFinanceRepaymentID As long
    Protected mMicrofinanceClientDetailID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateRepayed As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mAmountRepayed As decimal

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

    Public  Property MicroFinanceRepaymentID() As long
        Get
		return mMicroFinanceRepaymentID
        End Get
        Set(ByVal value As long)
		mMicroFinanceRepaymentID = value
        End Set
    End Property

    Public  Property MicrofinanceClientDetailID() As long
        Get
		return mMicrofinanceClientDetailID
        End Get
        Set(ByVal value As long)
		mMicrofinanceClientDetailID = value
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

    Public  Property DateRepayed() As string
        Get
		return mDateRepayed
        End Get
        Set(ByVal value As string)
		mDateRepayed = value
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

    Public  Property AmountRepayed() As decimal
        Get
		return mAmountRepayed
        End Get
        Set(ByVal value As decimal)
		mAmountRepayed = value
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

    MicroFinanceRepaymentID = 0
    mMicrofinanceClientDetailID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateRepayed = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mAmountRepayed = 0.0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mMicroFinanceRepaymentID) 

    End Function 

    Public Overridable Function Retrieve(ByVal MicroFinanceRepaymentID As Long) As Boolean 

        Dim sql As String 

        If MicroFinanceRepaymentID > 0 Then 
            sql = "SELECT * FROM tblMicroFinanceRepayments WHERE MicroFinanceRepaymentID = " & MicroFinanceRepaymentID
        Else 
            sql = "SELECT * FROM tblMicroFinanceRepayments WHERE MicroFinanceRepaymentID = " & mMicroFinanceRepaymentID
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

                log.Error("MicroFinanceRepayments not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetMicroFinanceRepayments() As System.Data.DataSet

        Return GetMicroFinanceRepayments(mMicroFinanceRepaymentID)

    End Function

    Public Overridable Function GetMicroFinanceRepayments(ByVal MicrofinanceClientDetailID As Long) As DataSet

        Dim sql As String

        If MicrofinanceClientDetailID > 0 Then
            sql = "SELECT * FROM tblMicroFinanceRepayments WHERE MicrofinanceClientDetailID = " & MicrofinanceClientDetailID
        Else
            sql = "SELECT * FROM tblMicroFinanceRepayments WHERE MicrofinanceClientDetailID = " & mMicrofinanceClientDetailID
        End If

        Return GetMicroFinanceRepayments(sql)

    End Function

    Protected Overridable Function GetMicroFinanceRepayments(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mMicroFinanceRepaymentID = Catchnull(.Item("MicroFinanceRepaymentID"), 0)
            mMicrofinanceClientDetailID = Catchnull(.Item("MicrofinanceClientDetailID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateRepayed = Catchnull(.Item("DateRepayed"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mAmountRepayed = Catchnull(.Item("AmountRepayed"), 0.0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@MicroFinanceRepaymentID", DbType.Int32, mMicroFinanceRepaymentID)
        db.AddInParameter(cmd, "@MicrofinanceClientDetailID", DbType.Int32, mMicrofinanceClientDetailID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateRepayed", DbType.String, mDateRepayed)
        db.AddInParameter(cmd, "@AmountRepayed", DbType.Decimal, mAmountRepayed)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_MicroFinanceRepayments")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mMicroFinanceRepaymentID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblMicroFinanceRepayments SET Deleted = 1 WHERE MicroFinanceRepaymentID = " & mMicroFinanceRepaymentID) 
        Return Delete("DELETE FROM tblMicroFinanceRepayments WHERE MicroFinanceRepaymentID = " & mMicroFinanceRepaymentID)

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