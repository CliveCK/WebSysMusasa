Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class MicroFinanceClientDetails

#region "Variables"

    Protected mMicrofinanceClientDetailID As long
    Protected mBeneficiaryID As long
    Protected mRepaymentTerm As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateApproved As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mAmountApproved As decimal
    Protected mMonthlyRepayment As decimal

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

    Public  Property MicrofinanceClientDetailID() As long
        Get
		return mMicrofinanceClientDetailID
        End Get
        Set(ByVal value As long)
		mMicrofinanceClientDetailID = value
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

    Public  Property RepaymentTerm() As long
        Get
		return mRepaymentTerm
        End Get
        Set(ByVal value As long)
		mRepaymentTerm = value
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

    Public  Property DateApproved() As string
        Get
		return mDateApproved
        End Get
        Set(ByVal value As string)
		mDateApproved = value
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

    Public  Property AmountApproved() As decimal
        Get
		return mAmountApproved
        End Get
        Set(ByVal value As decimal)
		mAmountApproved = value
        End Set
    End Property

    Public  Property MonthlyRepayment() As decimal
        Get
		return mMonthlyRepayment
        End Get
        Set(ByVal value As decimal)
		mMonthlyRepayment = value
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

    MicrofinanceClientDetailID = 0
    mBeneficiaryID = 0
    mRepaymentTerm = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateApproved = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mAmountApproved = 0.0
    mMonthlyRepayment = 0.0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mMicrofinanceClientDetailID) 

    End Function

    Public Overridable Function Retrieve(ByVal BeneficiaryID As Long) As Boolean

        Dim sql As String

        If BeneficiaryID > 0 Then
            sql = "SELECT * FROM tblMicrofinanceClientDetails WHERE BeneficiaryID = " & BeneficiaryID
        Else
            sql = "SELECT * FROM tblMicrofinanceClientDetails WHERE BeneficiaryID = " & mBeneficiaryID
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

                log.Error("MicroFinanceClientDetails not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetMicroFinanceClientDetails() As System.Data.DataSet

        Return GetMicroFinanceClientDetails(mMicrofinanceClientDetailID)

    End Function

    Public Overridable Function GetMicroFinanceClientDetails(ByVal MicrofinanceClientDetailID As Long) As DataSet

        Dim sql As String

        If MicrofinanceClientDetailID > 0 Then
            sql = "SELECT * FROM tblMicrofinanceClientDetails WHERE MicrofinanceClientDetailID = " & MicrofinanceClientDetailID
        Else
            sql = "SELECT * FROM tblMicrofinanceClientDetails WHERE MicrofinanceClientDetailID = " & mMicrofinanceClientDetailID
        End If

        Return GetMicroFinanceClientDetails(sql)

    End Function

    Protected Overridable Function GetMicroFinanceClientDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mMicrofinanceClientDetailID = Catchnull(.Item("MicrofinanceClientDetailID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mRepaymentTerm = Catchnull(.Item("RepaymentTerm"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateApproved = Catchnull(.Item("DateApproved"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mAmountApproved = Catchnull(.Item("AmountApproved"), 0.0)
            mMonthlyRepayment = Catchnull(.Item("MonthlyRepayment"), 0.0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@MicrofinanceClientDetailID", DbType.Int32, mMicrofinanceClientDetailID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@RepaymentTerm", DbType.Int32, mRepaymentTerm)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateApproved", DbType.String, mDateApproved)
        db.AddInParameter(cmd, "@AmountApproved", DbType.Decimal, mAmountApproved)
        db.AddInParameter(cmd, "@MonthlyRepayment", DbType.Decimal, mMonthlyRepayment)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_MicroFinanceClientDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mMicrofinanceClientDetailID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblMicrofinanceClientDetails SET Deleted = 1 WHERE MicrofinanceClientDetailID = " & mMicrofinanceClientDetailID) 
        Return Delete("DELETE FROM tblMicrofinanceClientDetails WHERE MicrofinanceClientDetailID = " & mMicrofinanceClientDetailID)

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