Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class TripCosts

#region "Variables"

    Protected mTripCostID As long
    Protected mTripID As long
    Protected mUnitID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mQuantity As decimal
    Protected mCost As decimal
    Protected mItem As string

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

    Public  Property TripCostID() As long
        Get
		return mTripCostID
        End Get
        Set(ByVal value As long)
		mTripCostID = value
        End Set
    End Property

    Public  Property TripID() As long
        Get
		return mTripID
        End Get
        Set(ByVal value As long)
		mTripID = value
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

    Public  Property Cost() As decimal
        Get
		return mCost
        End Get
        Set(ByVal value As decimal)
		mCost = value
        End Set
    End Property

    Public  Property Item() As string
        Get
		return mItem
        End Get
        Set(ByVal value As string)
		mItem = value
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

    TripCostID = 0
    mTripID = 0
    mUnitID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mQuantity = 0.0
    mCost = 0.0
    mItem = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mTripCostID) 

    End Function 

    Public Overridable Function Retrieve(ByVal TripCostID As Long) As Boolean 

        Dim sql As String 

        If TripCostID > 0 Then 
            sql = "SELECT * FROM tblTripCosts WHERE TripCostID = " & TripCostID
        Else 
            sql = "SELECT * FROM tblTripCosts WHERE TripCostID = " & mTripCostID
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

                log.Error("TripCosts not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetTripCosts() As System.Data.DataSet

        Return GetTripCosts(mTripCostID)

    End Function

    Public Overridable Function GetTripCosts(ByVal TripCostID As Long) As DataSet

        Dim sql As String

        If TripCostID > 0 Then
            sql = "SELECT * FROM tblTripCosts WHERE TripCostID = " & TripCostID
        Else
            sql = "SELECT * FROM tblTripCosts WHERE TripCostID = " & mTripCostID
        End If

        Return GetTripCosts(sql)

    End Function

    Protected Overridable Function GetTripCosts(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mTripCostID = Catchnull(.Item("TripCostID"), 0)
            mTripID = Catchnull(.Item("TripID"), 0)
            mUnitID = Catchnull(.Item("UnitID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mQuantity = Catchnull(.Item("Quantity"), 0.0)
            mCost = Catchnull(.Item("Cost"), 0.0)
            mItem = Catchnull(.Item("Item"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@TripCostID", DBType.Int32, mTripCostID)
        db.AddInParameter(cmd, "@TripID", DBType.Int32, mTripID)
        db.AddInParameter(cmd, "@UnitID", DBType.Int32, mUnitID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Quantity", DbType.Decimal, mQuantity)
        db.AddInParameter(cmd, "@Cost", DbType.Decimal, mCost)
        db.AddInParameter(cmd, "@Item", DBType.String, mItem)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_TripCosts")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mTripCostID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblTripCosts SET Deleted = 1 WHERE TripCostID = " & mTripCostID) 
        Return Delete("DELETE FROM tblTripCosts WHERE TripCostID = " & mTripCostID)

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