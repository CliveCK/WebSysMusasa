Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class CurrencyExchageRates

#region "Variables"

    Protected mCurrencyExchangeRateID As long
    Protected mFromCurrencyID As Long
    Protected mToCurrencyID As Long
    Protected mRate As Decimal
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateEffective As string
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

    Public  Property CurrencyExchangeRateID() As long
        Get
		return mCurrencyExchangeRateID
        End Get
        Set(ByVal value As long)
		mCurrencyExchangeRateID = value
        End Set
    End Property

    Public  Property FromCurrencyID() As long
        Get
		return mFromCurrencyID
        End Get
        Set(ByVal value As long)
		mFromCurrencyID = value
        End Set
    End Property

    Public Property ToCurrencyID() As Long
        Get
            Return mToCurrencyID
        End Get
        Set(ByVal value As Long)
            mToCurrencyID = value
        End Set
    End Property

    Public Property Rate() As Decimal
        Get
            Return mRate
        End Get
        Set(ByVal value As Decimal)
            mRate = value
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

    Public  Property DateEffective() As string
        Get
		return mDateEffective
        End Get
        Set(ByVal value As string)
		mDateEffective = value
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

    CurrencyExchangeRateID = 0
    mFromCurrencyID = 0
        mToCurrencyID = 0
        mRate = 0.0
        mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateEffective = ""
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCurrencyExchangeRateID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CurrencyExchangeRateID As Long) As Boolean 

        Dim sql As String 

        If CurrencyExchangeRateID > 0 Then 
            sql = "SELECT * FROM tblCurrencyExchangeRates WHERE CurrencyExchangeRateID = " & CurrencyExchangeRateID
        Else 
            sql = "SELECT * FROM tblCurrencyExchangeRates WHERE CurrencyExchangeRateID = " & mCurrencyExchangeRateID
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

                log.Error("CurrencyExchageRates not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCurrencyExchageRates() As System.Data.DataSet

        Return GetCurrencyExchageRates(mCurrencyExchangeRateID)

    End Function

    Public Overridable Function GetCurrencyExchageRates(ByVal CurrencyExchangeRateID As Long) As DataSet

        Dim sql As String

        If CurrencyExchangeRateID > 0 Then
            sql = "SELECT * FROM tblCurrencyExchangeRates WHERE CurrencyExchangeRateID = " & CurrencyExchangeRateID
        Else
            sql = "SELECT * FROM tblCurrencyExchangeRates WHERE CurrencyExchangeRateID = " & mCurrencyExchangeRateID
        End If

        Return GetCurrencyExchageRates(sql)

    End Function

    Public Function GetCurrencyIDByTextSymbol(ByVal TextSymbol As String) As Long

        Return Catchnull(GetCurrencyExchageRates("SELECT CurrencyID FROM luCurrency WHERE Description = '" & TextSymbol & "'").Tables(0).Rows(0)(0), 0)

    End Function

    Public Function GetCurrentExchangeRate() As Decimal

        Dim sql As String = "Select TOP 1 Rate from tblCurrencyExchangeRates WHERE FromCurrencyID = " & FromCurrencyID & " AND ToCurrencyID = " & ToCurrencyID
        sql &= " And DateEffective <= getdate() order by DateEffective desc"

        If FromCurrencyID = ToCurrencyID Then

            Return 1.0

        End If

        Return Catchnull(GetCurrencyExchageRates(sql).Tables(0).Rows(0)(0), 0.0)

    End Function

    Public Overridable Function GetCurrencyExchageRates(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mCurrencyExchangeRateID = Catchnull(.Item("CurrencyExchangeRateID"), 0)
            mFromCurrencyID = Catchnull(.Item("FromCurrencyID"), 0)
            mToCurrencyID = Catchnull(.Item("ToCurrencyID"), 0)
            mRate = Catchnull(.Item("Rate"), 0.0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateEffective = Catchnull(.Item("DateEffective"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@CurrencyExchangeRateID", DbType.Int32, mCurrencyExchangeRateID)
        db.AddInParameter(cmd, "@FromCurrencyID", DbType.Int32, mFromCurrencyID)
        db.AddInParameter(cmd, "@ToCurrencyID", DbType.Int32, mToCurrencyID)
        db.AddInParameter(cmd, "@Rate", DbType.Decimal, mRate)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateEffective", DbType.String, mDateEffective)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_CurrencyExchageRates")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mCurrencyExchangeRateID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblCurrencyExchangeRates SET Deleted = 1 WHERE CurrencyExchangeRateID = " & mCurrencyExchangeRateID) 
        Return Delete("DELETE FROM tblCurrencyExchangeRates WHERE CurrencyExchangeRateID = " & mCurrencyExchangeRateID)

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