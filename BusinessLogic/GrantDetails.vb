Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GrantDetails

#region "Variables"

    Protected mGrantDetailID As long
    Protected mProjectID As long
    Protected mDonorID As long
    Protected mKeyChangePromiseID As long
    Protected mContractCurrencyID As long
    Protected mContractDurationInMonths As long
    Protected mNumberOfReports As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mContractStartDate As string
    Protected mContractEndDate As string
    Protected mNewContractEndDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mExtensionGranted As boolean
    Protected mContractValueInCurrency As decimal
    Protected mContractValueInUSD As decimal
    Protected mContractValueInGBP As decimal
    Protected mTotalProjectCosts As decimal
    Protected mTotalAdminCosts As decimal
    Protected mVATInfo As string
    Protected mNatureOfExtension As string
    Protected mDonorRequirements As string

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

    Public  Property GrantDetailID() As long
        Get
		return mGrantDetailID
        End Get
        Set(ByVal value As long)
		mGrantDetailID = value
        End Set
    End Property

    Public  Property ProjectID() As long
        Get
		return mProjectID
        End Get
        Set(ByVal value As long)
		mProjectID = value
        End Set
    End Property

    Public  Property DonorID() As long
        Get
		return mDonorID
        End Get
        Set(ByVal value As long)
		mDonorID = value
        End Set
    End Property

    Public  Property KeyChangePromiseID() As long
        Get
		return mKeyChangePromiseID
        End Get
        Set(ByVal value As long)
		mKeyChangePromiseID = value
        End Set
    End Property

    Public  Property ContractCurrencyID() As long
        Get
		return mContractCurrencyID
        End Get
        Set(ByVal value As long)
		mContractCurrencyID = value
        End Set
    End Property

    Public  Property ContractDurationInMonths() As long
        Get
		return mContractDurationInMonths
        End Get
        Set(ByVal value As long)
		mContractDurationInMonths = value
        End Set
    End Property

    Public  Property NumberOfReports() As long
        Get
		return mNumberOfReports
        End Get
        Set(ByVal value As long)
		mNumberOfReports = value
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

    Public  Property ContractStartDate() As string
        Get
		return mContractStartDate
        End Get
        Set(ByVal value As string)
		mContractStartDate = value
        End Set
    End Property

    Public  Property ContractEndDate() As string
        Get
		return mContractEndDate
        End Get
        Set(ByVal value As string)
		mContractEndDate = value
        End Set
    End Property

    Public  Property NewContractEndDate() As string
        Get
		return mNewContractEndDate
        End Get
        Set(ByVal value As string)
		mNewContractEndDate = value
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

    Public  Property ExtensionGranted() As boolean
        Get
		return mExtensionGranted
        End Get
        Set(ByVal value As boolean)
		mExtensionGranted = value
        End Set
    End Property

    Public  Property ContractValueInCurrency() As decimal
        Get
		return mContractValueInCurrency
        End Get
        Set(ByVal value As decimal)
		mContractValueInCurrency = value
        End Set
    End Property

    Public  Property ContractValueInUSD() As decimal
        Get
		return mContractValueInUSD
        End Get
        Set(ByVal value As decimal)
		mContractValueInUSD = value
        End Set
    End Property

    Public  Property ContractValueInGBP() As decimal
        Get
		return mContractValueInGBP
        End Get
        Set(ByVal value As decimal)
		mContractValueInGBP = value
        End Set
    End Property

    Public  Property TotalProjectCosts() As decimal
        Get
		return mTotalProjectCosts
        End Get
        Set(ByVal value As decimal)
		mTotalProjectCosts = value
        End Set
    End Property

    Public  Property TotalAdminCosts() As decimal
        Get
		return mTotalAdminCosts
        End Get
        Set(ByVal value As decimal)
		mTotalAdminCosts = value
        End Set
    End Property

    Public  Property VATInfo() As string
        Get
		return mVATInfo
        End Get
        Set(ByVal value As string)
		mVATInfo = value
        End Set
    End Property

    Public  Property NatureOfExtension() As string
        Get
		return mNatureOfExtension
        End Get
        Set(ByVal value As string)
		mNatureOfExtension = value
        End Set
    End Property

    Public  Property DonorRequirements() As string
        Get
		return mDonorRequirements
        End Get
        Set(ByVal value As string)
		mDonorRequirements = value
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

    GrantDetailID = 0
    mProjectID = 0
    mDonorID = 0
    mKeyChangePromiseID = 0
    mContractCurrencyID = 0
    mContractDurationInMonths = 0
    mNumberOfReports = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mContractStartDate = ""
    mContractEndDate = ""
    mNewContractEndDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mExtensionGranted = FALSE
    mContractValueInCurrency = 0.0
    mContractValueInUSD = 0.0
    mContractValueInGBP = 0.0
    mTotalProjectCosts = 0.0
    mTotalAdminCosts = 0.0
    mVATInfo = ""
    mNatureOfExtension = ""
    mDonorRequirements = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGrantDetailID) 

    End Function

    Public Overridable Function Retrieve(ByVal GrantDetailID As Long) As Boolean

        Dim sql As String

        If GrantDetailID > 0 Then
            sql = "SELECT * FROM tblGrantDetails WHERE GrantDetailID = " & GrantDetailID
        Else
            sql = "SELECT * FROM tblGrantDetails WHERE GrantDetailID = " & mGrantDetailID
        End If

        Return Retrieve(sql)

    End Function

    Public Overridable Function RetrieveAll() As DataSet

        Dim sql As String

        sql = "SELECT *, P.Name as ProjectTitle, D.Name as Donor, K.KeyChangePromiseNo as KCP FROM tblGrantDetails G inner join tblProjects P on G.ProjectID = P.Project "
        sql &= " left outer join tblKeyChangePromises K on K.KeyChangePromiseID = G.KeyChangePromiseID "
        sql &= " left outer join luDonor D on D.DonorID = G.DonorID"

        Return GetGrantDetails(sql)

    End Function

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean 

        Try 

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql) 

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then 

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0)) 

                dsRetrieve = Nothing 
                Return True 

            Else

                log.Error("GrantDetails not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGrantDetails() As System.Data.DataSet

        Return GetGrantDetails(mGrantDetailID)

    End Function

    Public Overridable Function GetGrantDetails(ByVal GrantDetailID As Long) As DataSet

        Dim sql As String

        If GrantDetailID > 0 Then
            sql = "SELECT * FROM tblGrantDetails WHERE GrantDetailID = " & GrantDetailID
        Else
            sql = "SELECT * FROM tblGrantDetails WHERE GrantDetailID = " & mGrantDetailID
        End If

        Return GetGrantDetails(sql)

    End Function

    Protected Overridable Function GetGrantDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGrantDetailID = Catchnull(.Item("GrantDetailID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mDonorID = Catchnull(.Item("DonorID"), 0)
            mKeyChangePromiseID = Catchnull(.Item("KeyChangePromiseID"), 0)
            mContractCurrencyID = Catchnull(.Item("ContractCurrencyID"), 0)
            mContractDurationInMonths = Catchnull(.Item("ContractDurationInMonths"), 0)
            mNumberOfReports = Catchnull(.Item("NumberOfReports"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mContractStartDate = Catchnull(.Item("ContractStartDate"), "")
            mContractEndDate = Catchnull(.Item("ContractEndDate"), "")
            mNewContractEndDate = Catchnull(.Item("NewContractEndDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mExtensionGranted = Catchnull(.Item("ExtensionGranted"), False)
            mContractValueInCurrency = Catchnull(.Item("ContractValueInCurrency"), 0.0)
            mContractValueInUSD = Catchnull(.Item("ContractValueInUSD"), 0.0)
            mContractValueInGBP = Catchnull(.Item("ContractValueInGBP"), 0.0)
            mTotalProjectCosts = Catchnull(.Item("TotalProjectCosts"), 0.0)
            mTotalAdminCosts = Catchnull(.Item("TotalAdminCosts"), 0.0)
            mVATInfo = Catchnull(.Item("VATInfo"), "")
            mNatureOfExtension = Catchnull(.Item("NatureOfExtension"), "")
            mDonorRequirements = Catchnull(.Item("DonorRequirements"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GrantDetailID", DbType.Int32, mGrantDetailID)
        db.AddInParameter(cmd, "@ProjectID", DbType.Int32, mProjectID)
        db.AddInParameter(cmd, "@DonorID", DbType.Int32, mDonorID)
        db.AddInParameter(cmd, "@KeyChangePromiseID", DbType.Int32, mKeyChangePromiseID)
        db.AddInParameter(cmd, "@ContractCurrencyID", DbType.Int32, mContractCurrencyID)
        db.AddInParameter(cmd, "@ContractDurationInMonths", DbType.Int32, mContractDurationInMonths)
        db.AddInParameter(cmd, "@NumberOfReports", DbType.Int32, mNumberOfReports)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ContractStartDate", DbType.String, mContractStartDate)
        db.AddInParameter(cmd, "@ContractEndDate", DbType.String, mContractEndDate)
        db.AddInParameter(cmd, "@NewContractEndDate", DbType.String, mNewContractEndDate)
        db.AddInParameter(cmd, "@ExtensionGranted", DbType.Boolean, mExtensionGranted)
        db.AddInParameter(cmd, "@ContractValueInCurrency", DbType.Decimal, mContractValueInCurrency)
        db.AddInParameter(cmd, "@ContractValueInUSD", DbType.Decimal, mContractValueInUSD)
        db.AddInParameter(cmd, "@ContractValueInGBP", DbType.Decimal, mContractValueInGBP)
        db.AddInParameter(cmd, "@TotalProjectCosts", DbType.Decimal, mTotalProjectCosts)
        db.AddInParameter(cmd, "@TotalAdminCosts", DbType.Decimal, mTotalAdminCosts)
        db.AddInParameter(cmd, "@VATInfo", DbType.String, mVATInfo)
        db.AddInParameter(cmd, "@NatureOfExtension", DbType.String, mNatureOfExtension)
        db.AddInParameter(cmd, "@DonorRequirements", DbType.String, mDonorRequirements)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GrantDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGrantDetailID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGrantDetails SET Deleted = 1 WHERE GrantDetailID = " & mGrantDetailID) 
        Return Delete("DELETE FROM tblGrantDetails WHERE GrantDetailID = " & mGrantDetailID)

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