Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class HouseHoldAssetsDetails

#region "Variables"

    Protected mHouseholdAssetDetailID As long
    Protected mAriableLandSize As long
    Protected mMajorSourceOfFoodID As long
    Protected mConditionOfHouseID As long
    Protected mTenureID As long
    Protected mWealthRankID As long
    Protected mSourceOfWaterID As long
    Protected mTypeOfToiletID As long
    Protected mHealthCareProvider As long
    Protected mHouseholdID As Long
    Protected mRoomOccupationRatio As String

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

    Public  Property HouseholdAssetDetailID() As long
        Get
		return mHouseholdAssetDetailID
        End Get
        Set(ByVal value As long)
		mHouseholdAssetDetailID = value
        End Set
    End Property

    Public  Property AriableLandSize() As long
        Get
		return mAriableLandSize
        End Get
        Set(ByVal value As long)
		mAriableLandSize = value
        End Set
    End Property

    Public  Property MajorSourceOfFoodID() As long
        Get
		return mMajorSourceOfFoodID
        End Get
        Set(ByVal value As long)
		mMajorSourceOfFoodID = value
        End Set
    End Property

    Public  Property ConditionOfHouseID() As long
        Get
		return mConditionOfHouseID
        End Get
        Set(ByVal value As long)
		mConditionOfHouseID = value
        End Set
    End Property

    Public  Property TenureID() As long
        Get
		return mTenureID
        End Get
        Set(ByVal value As long)
		mTenureID = value
        End Set
    End Property

    Public  Property WealthRankID() As long
        Get
		return mWealthRankID
        End Get
        Set(ByVal value As long)
		mWealthRankID = value
        End Set
    End Property

    Public  Property SourceOfWaterID() As long
        Get
		return mSourceOfWaterID
        End Get
        Set(ByVal value As long)
		mSourceOfWaterID = value
        End Set
    End Property

    Public  Property TypeOfToiletID() As long
        Get
		return mTypeOfToiletID
        End Get
        Set(ByVal value As long)
		mTypeOfToiletID = value
        End Set
    End Property

    Public  Property HealthCareProvider() As long
        Get
		return mHealthCareProvider
        End Get
        Set(ByVal value As long)
		mHealthCareProvider = value
        End Set
    End Property

    Public Property HouseholdID() As Long
        Get
            Return mHouseholdID
        End Get
        Set(ByVal value As Long)
            mHouseholdID = value
        End Set
    End Property

    Public  Property RoomOccupationRatio() As string
        Get
		return mRoomOccupationRatio
        End Get
        Set(ByVal value As string)
		mRoomOccupationRatio = value
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

    HouseholdAssetDetailID = 0
    mAriableLandSize = 0
    mMajorSourceOfFoodID = 0
    mConditionOfHouseID = 0
    mTenureID = 0
    mWealthRankID = 0
    mSourceOfWaterID = 0
    mTypeOfToiletID = 0
    mHealthCareProvider = 0
        mHouseholdID = 0
    mRoomOccupationRatio = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mHouseholdAssetDetailID) 

    End Function 

    Public Overridable Function Retrieve(ByVal HouseholdAssetDetailID As Long) As Boolean 

        Dim sql As String 

        If HouseholdAssetDetailID > 0 Then 
            sql = "SELECT * FROM tblHouseholdAssetDetails WHERE HouseholdAssetDetailID = " & HouseholdAssetDetailID
        Else 
            sql = "SELECT * FROM tblHouseholdAssetDetails WHERE HouseholdAssetDetailID = " & mHouseholdAssetDetailID
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

                log.Warn("HouseHoldAssetsDetails not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetHouseHoldAssetsDetails() As System.Data.DataSet

        Return GetHouseHoldAssetsDetails(mHouseholdAssetDetailID) 

    End Function 

    Public Overridable Function GetHouseHoldAssetsDetails(ByVal HouseholdAssetDetailID As Long) As DataSet 

        Dim sql As String 

        If HouseholdAssetDetailID > 0 Then 
            sql = "SELECT * FROM tblHouseholdAssetDetails WHERE HouseholdAssetDetailID = " & HouseholdAssetDetailID
        Else 
            sql = "SELECT * FROM tblHouseholdAssetDetails WHERE HouseholdAssetDetailID = " & mHouseholdAssetDetailID
        End If 

        Return GetHouseHoldAssetsDetails(sql) 

    End Function 

    Protected Overridable Function GetHouseHoldAssetsDetails(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function 

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mHouseholdAssetDetailID = Catchnull(.Item("HouseholdAssetDetailID"), 0)
            mAriableLandSize = Catchnull(.Item("AriableLandSize"), 0)
            mMajorSourceOfFoodID = Catchnull(.Item("MajorSourceOfFoodID"), 0)
            mConditionOfHouseID = Catchnull(.Item("ConditionOfHouseID"), 0)
            mTenureID = Catchnull(.Item("TenureID"), 0)
            mWealthRankID = Catchnull(.Item("WealthRankID"), 0)
            mSourceOfWaterID = Catchnull(.Item("SourceOfWaterID"), 0)
            mTypeOfToiletID = Catchnull(.Item("TypeOfToiletID"), 0)
            mHealthCareProvider = Catchnull(.Item("HealthCareProvider"), 0)
            mHouseholdID = Catchnull(.Item("HouseholdID"), "")
            mRoomOccupationRatio = Catchnull(.Item("RoomOccupationRatio"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@HouseholdAssetDetailID", DBType.Int32, mHouseholdAssetDetailID) 
        db.AddInParameter(cmd, "@AriableLandSize", DBType.Int32, mAriableLandSize) 
        db.AddInParameter(cmd, "@MajorSourceOfFoodID", DBType.Int32, mMajorSourceOfFoodID) 
        db.AddInParameter(cmd, "@ConditionOfHouseID", DBType.Int32, mConditionOfHouseID) 
        db.AddInParameter(cmd, "@TenureID", DBType.Int32, mTenureID) 
        db.AddInParameter(cmd, "@WealthRankID", DBType.Int32, mWealthRankID) 
        db.AddInParameter(cmd, "@SourceOfWaterID", DBType.Int32, mSourceOfWaterID) 
        db.AddInParameter(cmd, "@TypeOfToiletID", DBType.Int32, mTypeOfToiletID) 
        db.AddInParameter(cmd, "@HealthCareProvider", DBType.Int32, mHealthCareProvider) 
        db.AddInParameter(cmd, "@HouseholdID", DbType.String, mHouseholdID)
        db.AddInParameter(cmd, "@RoomOccupationRatio", DBType.String, mRoomOccupationRatio) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_HouseHoldAssetsDetails") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mHouseholdAssetDetailID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblHouseholdAssetDetails SET Deleted = 1 WHERE HouseholdAssetDetailID = " & mHouseholdAssetDetailID) 
        Return Delete("DELETE FROM tblHouseholdAssetDetails WHERE HouseholdAssetDetailID = " & mHouseholdAssetDetailID) 

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