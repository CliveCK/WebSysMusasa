Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Procument

#region "Variables"

    Protected mProcumentID As long
    Protected mProjectID As long
    Protected mCommodityID As long
    Protected mQuantityRequired As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateRequired As string
    Protected mDateOrdered As string
    Protected mDateRequested As string
    Protected mDateSupplied As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mRequestedBy As string
    Protected mOrderedBy As string

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

    Public  Property ProcumentID() As long
        Get
		return mProcumentID
        End Get
        Set(ByVal value As long)
		mProcumentID = value
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

    Public  Property CommodityID() As long
        Get
		return mCommodityID
        End Get
        Set(ByVal value As long)
		mCommodityID = value
        End Set
    End Property

    Public  Property QuantityRequired() As long
        Get
		return mQuantityRequired
        End Get
        Set(ByVal value As long)
		mQuantityRequired = value
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

    Public  Property DateRequired() As string
        Get
		return mDateRequired
        End Get
        Set(ByVal value As string)
		mDateRequired = value
        End Set
    End Property

    Public  Property DateOrdered() As string
        Get
		return mDateOrdered
        End Get
        Set(ByVal value As string)
		mDateOrdered = value
        End Set
    End Property

    Public  Property DateRequested() As string
        Get
		return mDateRequested
        End Get
        Set(ByVal value As string)
		mDateRequested = value
        End Set
    End Property

    Public  Property DateSupplied() As string
        Get
		return mDateSupplied
        End Get
        Set(ByVal value As string)
		mDateSupplied = value
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

    Public  Property RequestedBy() As string
        Get
		return mRequestedBy
        End Get
        Set(ByVal value As string)
		mRequestedBy = value
        End Set
    End Property

    Public  Property OrderedBy() As string
        Get
		return mOrderedBy
        End Get
        Set(ByVal value As string)
		mOrderedBy = value
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

    ProcumentID = 0
    mProjectID = 0
    mCommodityID = 0
    mQuantityRequired = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateRequired = ""
    mDateOrdered = ""
    mDateRequested = ""
    mDateSupplied = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mRequestedBy = ""
    mOrderedBy = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mProcumentID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ProcumentID As Long) As Boolean 

        Dim sql As String 

        If ProcumentID > 0 Then 
            sql = "SELECT * FROM tblProcument WHERE ProcumentID = " & ProcumentID
        Else 
            sql = "SELECT * FROM tblProcument WHERE ProcumentID = " & mProcumentID
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

                log.Error("Procument not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetProcument() As System.Data.DataSet

        Return GetProcument(mProcumentID)

    End Function

    Public Overridable Function GetProcument(ByVal ProcumentID As Long) As DataSet

        Dim sql As String

        If ProcumentID > 0 Then
            sql = "SELECT * FROM tblProcument WHERE ProcumentID = " & ProcumentID
        Else
            sql = "SELECT * FROM tblProcument WHERE ProcumentID = " & mProcumentID
        End If

        Return GetProcument(sql)

    End Function

    Protected Overridable Function GetProcument(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mProcumentID = Catchnull(.Item("ProcumentID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mCommodityID = Catchnull(.Item("CommodityID"), 0)
            mQuantityRequired = Catchnull(.Item("QuantityRequired"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateRequired = Catchnull(.Item("DateRequired"), "")
            mDateOrdered = Catchnull(.Item("DateOrdered"), "")
            mDateRequested = Catchnull(.Item("DateRequested"), "")
            mDateSupplied = Catchnull(.Item("DateSupplied"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mRequestedBy = Catchnull(.Item("RequestedBy"), "")
            mOrderedBy = Catchnull(.Item("OrderedBy"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ProcumentID", DBType.Int32, mProcumentID)
        db.AddInParameter(cmd, "@ProjectID", DBType.Int32, mProjectID)
        db.AddInParameter(cmd, "@CommodityID", DBType.Int32, mCommodityID)
        db.AddInParameter(cmd, "@QuantityRequired", DBType.Int32, mQuantityRequired)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateRequired", DBType.String, mDateRequired)
        db.AddInParameter(cmd, "@DateOrdered", DBType.String, mDateOrdered)
        db.AddInParameter(cmd, "@DateRequested", DBType.String, mDateRequested)
        db.AddInParameter(cmd, "@DateSupplied", DBType.String, mDateSupplied)
        db.AddInParameter(cmd, "@RequestedBy", DBType.String, mRequestedBy)
        db.AddInParameter(cmd, "@OrderedBy", DBType.String, mOrderedBy)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Procument")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mProcumentID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProcument SET Deleted = 1 WHERE ProcumentID = " & mProcumentID) 
        Return Delete("DELETE FROM tblProcument WHERE ProcumentID = " & mProcumentID)

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