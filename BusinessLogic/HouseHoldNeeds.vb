Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class HouseHoldNeeds

#region "Variables"

    Protected mHouseHoldNeedID As long
    Protected mBeneficiaryID As long
    Protected mNeedID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
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

    Public  Property HouseHoldNeedID() As long
        Get
		return mHouseHoldNeedID
        End Get
        Set(ByVal value As long)
		mHouseHoldNeedID = value
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

    Public  Property NeedID() As long
        Get
		return mNeedID
        End Get
        Set(ByVal value As long)
		mNeedID = value
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

    HouseHoldNeedID = 0
    mBeneficiaryID = 0
    mNeedID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mHouseHoldNeedID) 

    End Function 

    Public Overridable Function Retrieve(ByVal HouseHoldNeedID As Long) As Boolean 

        Dim sql As String 

        If HouseHoldNeedID > 0 Then 
            sql = "SELECT * FROM tblHouseHoldNeeds WHERE HouseHoldNeedID = " & HouseHoldNeedID
        Else 
            sql = "SELECT * FROM tblHouseHoldNeeds WHERE HouseHoldNeedID = " & mHouseHoldNeedID
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

                log.Warn("HouseHoldNeeds not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function

    Public Function GetNeeds(ByVal BeneficiaryID As Long) As DataSet

        Dim sql As String = "SELECT * FROM tblHouseHoldNeeds H inner join luNeeds N on H.NeedID = N.NeedID"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Overridable Function GetHouseHoldNeeds() As System.Data.DataSet

        Return GetHouseHoldNeeds(mHouseHoldNeedID) 

    End Function 

    Public Overridable Function GetHouseHoldNeeds(ByVal HouseHoldNeedID As Long) As DataSet 

        Dim sql As String 

        If HouseHoldNeedID > 0 Then 
            sql = "SELECT * FROM tblHouseHoldNeeds WHERE HouseHoldNeedID = " & HouseHoldNeedID
        Else 
            sql = "SELECT * FROM tblHouseHoldNeeds WHERE HouseHoldNeedID = " & mHouseHoldNeedID
        End If 

        Return GetHouseHoldNeeds(sql) 

    End Function 

    Protected Overridable Function GetHouseHoldNeeds(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function 

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mHouseHoldNeedID = Catchnull(.Item("HouseHoldNeedID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mNeedID = Catchnull(.Item("NeedID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@HouseHoldNeedID", DBType.Int32, mHouseHoldNeedID) 
        db.AddInParameter(cmd, "@BeneficiaryID", DBType.Int32, mBeneficiaryID) 
        db.AddInParameter(cmd, "@NeedID", DBType.Int32, mNeedID) 
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_HouseHoldNeeds") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mHouseHoldNeedID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblHouseHoldNeeds SET Deleted = 1 WHERE HouseHoldNeedID = " & mHouseHoldNeedID) 
        Return Delete("DELETE FROM tblHouseHoldNeeds WHERE HouseHoldNeedID = " & mHouseHoldNeedID) 

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