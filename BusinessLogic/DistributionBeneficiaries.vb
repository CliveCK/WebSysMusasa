Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class DistributionBeneficiaries

#region "Variables"

    Protected mDistributionBeneficiaryID As long
    Protected mDistributionID As long
    Protected mBeneficiaryTypeID As long
    Protected mBeneficiaryID As long
    Protected mCommodityID As long
    Protected mUnitID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mQuantity As decimal

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

    Public  Property DistributionBeneficiaryID() As long
        Get
		return mDistributionBeneficiaryID
        End Get
        Set(ByVal value As long)
		mDistributionBeneficiaryID = value
        End Set
    End Property

    Public  Property DistributionID() As long
        Get
		return mDistributionID
        End Get
        Set(ByVal value As long)
		mDistributionID = value
        End Set
    End Property

    Public  Property BeneficiaryTypeID() As long
        Get
		return mBeneficiaryTypeID
        End Get
        Set(ByVal value As long)
		mBeneficiaryTypeID = value
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

    Public  Property CommodityID() As long
        Get
		return mCommodityID
        End Get
        Set(ByVal value As long)
		mCommodityID = value
        End Set
    End Property

    Public  Property UnitID() As long
        Get
		return mUnitID
        End Get
        Set(ByVal value As long)
		mUnitID = value
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

    Public  Property Quantity() As decimal
        Get
		return mQuantity
        End Get
        Set(ByVal value As decimal)
		mQuantity = value
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

    DistributionBeneficiaryID = 0
    mDistributionID = 0
    mBeneficiaryTypeID = 0
    mBeneficiaryID = 0
    mCommodityID = 0
    mUnitID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mQuantity = 0.0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mDistributionBeneficiaryID) 

    End Function 

    Public Overridable Function Retrieve(ByVal DistributionBeneficiaryID As Long) As Boolean 

        Dim sql As String 

        If DistributionBeneficiaryID > 0 Then 
            sql = "SELECT * FROM tblDistributionBeneficiaries WHERE DistributionBeneficiaryID = " & DistributionBeneficiaryID
        Else 
            sql = "SELECT * FROM tblDistributionBeneficiaries WHERE DistributionBeneficiaryID = " & mDistributionBeneficiaryID
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

                log.Warn("DistributionBeneficiaries not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetDistributionBeneficiaries() As System.Data.DataSet

        Return GetDistributionBeneficiaries(mDistributionBeneficiaryID)

    End Function

    Public Overridable Function GetDistributionBeneficiaries(ByVal DistributionBeneficiaryID As Long) As DataSet

        Dim sql As String

        If DistributionBeneficiaryID > 0 Then
            sql = "SELECT * FROM tblDistributionBeneficiaries WHERE DistributionBeneficiaryID = " & DistributionBeneficiaryID
        Else
            sql = "SELECT * FROM tblDistributionBeneficiaries WHERE DistributionBeneficiaryID = " & mDistributionBeneficiaryID
        End If

        Return GetDistributionBeneficiaries(sql)

    End Function

    Protected Overridable Function GetDistributionBeneficiaries(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mDistributionBeneficiaryID = Catchnull(.Item("DistributionBeneficiaryID"), 0)
            mDistributionID = Catchnull(.Item("DistributionID"), 0)
            mBeneficiaryTypeID = Catchnull(.Item("BeneficiaryTypeID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mCommodityID = Catchnull(.Item("CommodityID"), 0)
            mUnitID = Catchnull(.Item("UnitID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mQuantity = Catchnull(.Item("Quantity"), 0.0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@DistributionBeneficiaryID", DBType.Int32, mDistributionBeneficiaryID)
        db.AddInParameter(cmd, "@DistributionID", DBType.Int32, mDistributionID)
        db.AddInParameter(cmd, "@BeneficiaryTypeID", DBType.Int32, mBeneficiaryTypeID)
        db.AddInParameter(cmd, "@BeneficiaryID", DBType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@CommodityID", DBType.Int32, mCommodityID)
        db.AddInParameter(cmd, "@UnitID", DBType.Int32, mUnitID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Quantity", DbType.Decimal, mQuantity)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_DistributionBeneficiaries")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mDistributionBeneficiaryID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblDistributionBeneficiaries SET Deleted = 1 WHERE DistributionBeneficiaryID = " & mDistributionBeneficiaryID) 
        Return Delete("DELETE FROM tblDistributionBeneficiaries WHERE DistributionBeneficiaryID = " & mDistributionBeneficiaryID)

    End Function

    Public Function DeleteEntries() As Boolean

        Return Delete("DELETE FROM tblDistributionBeneficiaries WHERE DistributionID = " & mDistributionID & " AND BeneficiaryID = " & mBeneficiaryID)

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