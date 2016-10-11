Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Distributions

#region "Variables"

    Protected mDistributionID As long
    Protected mDistributionTypeID As Long
    Protected mDistributionDate As String
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mName As String
    Protected mOrganizationID As Long
    Protected mDescription As string
    Protected mLocation As String
    Protected mCommodityID As Long
    Protected mUnitID As Long
    Protected mQuantity As Long
    Protected mQuantityPerBeneficiary As Long

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

    Public  Property DistributionID() As long
        Get
		return mDistributionID
        End Get
        Set(ByVal value As long)
		mDistributionID = value
        End Set
    End Property

    Public  Property DistributionTypeID() As long
        Get
		return mDistributionTypeID
        End Get
        Set(ByVal value As long)
		mDistributionTypeID = value
        End Set
    End Property

    Public Property OrganizationID() As Long
        Get
            Return mOrganizationID
        End Get
        Set(ByVal value As Long)
            mOrganizationID = value
        End Set
    End Property

    Public Property UnitID() As Long
        Get
            Return mUnitID
        End Get
        Set(ByVal value As Long)
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

    Public Property DistributionDate() As String
        Get
            Return mDistributionDate
        End Get
        Set(ByVal value As String)
            mDistributionDate = value
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

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
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

    Public  Property Location() As string
        Get
		return mLocation
        End Get
        Set(ByVal value As string)
		mLocation = value
        End Set
    End Property

    Public Property CommodityID() As Long
        Get
            Return mCommodityID
        End Get
        Set(ByVal value As Long)
            mCommodityID = value
        End Set
    End Property

    Public Property Quantity() As Long
        Get
            Return mQuantity
        End Get
        Set(ByVal value As Long)
            mQuantity = value
        End Set
    End Property

    Public Property QuantityPerBeneficiary() As Long
        Get
            Return mQuantityPerBeneficiary
        End Get
        Set(ByVal value As Long)
            mQuantityPerBeneficiary = value
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

    DistributionID = 0
        mDistributionTypeID = 0
        mDistributionDate = ""
        mOrganizationID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mName = ""
    mDescription = ""
        mLocation = ""
        mCommodityID = 0
        mUnitID = 0
        mQuantity = 0
        mQuantityPerBeneficiary = 0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mDistributionID) 

    End Function 

    Public Overridable Function Retrieve(ByVal DistributionID As Long) As Boolean 

        Dim sql As String 

        If DistributionID > 0 Then 
            sql = "SELECT * FROM tblDistributions WHERE DistributionID = " & DistributionID
        Else 
            sql = "SELECT * FROM tblDistributions WHERE DistributionID = " & mDistributionID
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

                log.Warn("Distributions not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetDistributions() As System.Data.DataSet

        Return GetDistributions(mDistributionID)

    End Function

    Public Overridable Function GetDistributions(ByVal DistributionID As Long) As DataSet

        Dim sql As String

        If DistributionID > 0 Then
            sql = "SELECT * FROM tblDistributions WHERE DistributionID = " & DistributionID
        Else
            sql = "SELECT * FROM tblDistributions WHERE DistributionID = " & mDistributionID
        End If

        Return GetDistributions(sql)

    End Function

    Protected Overridable Function GetDistributions(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mDistributionID = Catchnull(.Item("DistributionID"), 0)
            mDistributionTypeID = Catchnull(.Item("DistributionTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mOrganizationID = Catchnull(.Item("OrganizationID"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mName = Catchnull(.Item("Name"), "")
            mDistributionDate = Catchnull(.Item("DistributionDate"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mLocation = Catchnull(.Item("Location"), "")
            mCommodityID = Catchnull(.Item("CommodityID"), 0)
            mUnitID = Catchnull(.Item("UnitID"), 0)
            mQuantity = Catchnull(.Item("Quantity"), 0)
            mQuantityPerBeneficiary = Catchnull(.Item("QuantityPerBeneficiary"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@DistributionID", DBType.Int32, mDistributionID)
        db.AddInParameter(cmd, "@DistributionTypeID", DBType.Int32, mDistributionTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@OrganizationID", DbType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@Name", DbType.String, mName)
        db.AddInParameter(cmd, "@DistributionDate", DbType.String, mDistributionDate)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)
        db.AddInParameter(cmd, "@Location", DbType.String, mLocation)
        db.AddInParameter(cmd, "@CommodityID", DbType.Int32, mCommodityID)
        db.AddInParameter(cmd, "@UnitID", DbType.Int32, mUnitID)
        db.AddInParameter(cmd, "@Quantity", DbType.Int32, mQuantity)
        db.AddInParameter(cmd, "@QuantityPerBeneficiary", DbType.Int32, mQuantityPerBeneficiary)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Distributions")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mDistributionID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblDistributions SET Deleted = 1 WHERE DistributionID = " & mDistributionID) 
        Return Delete("DELETE FROM tblDistributions WHERE DistributionID = " & mDistributionID)

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