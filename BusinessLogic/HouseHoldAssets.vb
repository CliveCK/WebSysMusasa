Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class HouseHoldAssets

#region "Variables"

    Protected mHouseholdAssetID As long
    Protected mAssetTypeID As long
    Protected mAssetID As long
    Protected mQuantity As long
    Protected mHouseholdNo As String

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected db As Database 
    Protected mConnectionName As String 
    Protected mObjectUserID As Long 

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

    Public  Property HouseholdAssetID() As long
        Get
		return mHouseholdAssetID
        End Get
        Set(ByVal value As long)
		mHouseholdAssetID = value
        End Set
    End Property

    Public  Property AssetTypeID() As long
        Get
		return mAssetTypeID
        End Get
        Set(ByVal value As long)
		mAssetTypeID = value
        End Set
    End Property

    Public  Property AssetID() As long
        Get
		return mAssetID
        End Get
        Set(ByVal value As long)
		mAssetID = value
        End Set
    End Property

    Public  Property Quantity() As long
        Get
		return mQuantity
        End Get
        Set(ByVal value As long)
		mQuantity = value
        End Set
    End Property

    Public  Property HouseholdNo() As string
        Get
		return mHouseholdNo
        End Get
        Set(ByVal value As string)
		mHouseholdNo = value
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

    HouseholdAssetID = 0
    mAssetTypeID = 0
    mAssetID = 0
    mQuantity = 0
    mHouseholdNo = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mHouseholdAssetID) 

    End Function 

    Public Overridable Function Retrieve(ByVal HouseholdAssetID As Long) As Boolean 

        Dim sql As String 

        If HouseholdAssetID > 0 Then 
            sql = "SELECT * FROM tblHouseholdAssets WHERE HouseholdAssetID = " & HouseholdAssetID
        Else 
            sql = "SELECT * FROM tblHouseholdAssets WHERE HouseholdAssetID = " & mHouseholdAssetID
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

                log.Warn("HouseHoldAssets not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetHouseHoldAssets() As System.Data.DataSet

        Return GetHouseHoldAssets(mHouseholdAssetID) 

    End Function

    Public Function GetAssets(ByVal HouseHoldNo As String) As DataSet

        Dim sql As String = "SELECT HouseholdAssetID, AT.Description as Type, A.Description as Asset, Quantity FROM tblHouseHoldAssets HA inner join tblAssets A on HA.AssetID = A.AssetID inner join luAssetTypes AT on  "
        sql &= " HA.AssetTypeID = AT.AssetTypeID Where HA.HouseHoldNo = '" & HouseHoldNo & "'"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Overridable Function GetHouseHoldAssets(ByVal HouseholdAssetID As Long) As DataSet 

        Dim sql As String 

        If HouseholdAssetID > 0 Then 
            sql = "SELECT * FROM tblHouseholdAssets WHERE HouseholdAssetID = " & HouseholdAssetID
        Else 
            sql = "SELECT * FROM tblHouseholdAssets WHERE HouseholdAssetID = " & mHouseholdAssetID
        End If 

        Return GetHouseHoldAssets(sql) 

    End Function 

    Protected Overridable Function GetHouseHoldAssets(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function 

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mHouseholdAssetID = Catchnull(.Item("HouseholdAssetID"), 0)
            mAssetTypeID = Catchnull(.Item("AssetTypeID"), 0)
            mAssetID = Catchnull(.Item("AssetID"), 0)
            mQuantity = Catchnull(.Item("Quantity"), 0)
            mHouseholdNo = Catchnull(.Item("HouseholdNo"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@HouseholdAssetID", DBType.Int32, mHouseholdAssetID) 
        db.AddInParameter(cmd, "@AssetTypeID", DBType.Int32, mAssetTypeID) 
        db.AddInParameter(cmd, "@AssetID", DBType.Int32, mAssetID) 
        db.AddInParameter(cmd, "@Quantity", DBType.Int32, mQuantity) 
        db.AddInParameter(cmd, "@HouseholdNo", DBType.String, mHouseholdNo) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_HouseHoldAssets") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mHouseholdAssetID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblHouseholdAssets SET Deleted = 1 WHERE HouseholdAssetID = " & mHouseholdAssetID) 
        Return Delete("DELETE FROM tblHouseholdAssets WHERE HouseholdAssetID = " & mHouseholdAssetID) 

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