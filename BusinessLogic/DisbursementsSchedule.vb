Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class DisbursementsSchedule

#region "Variables"

    Protected mDisbursementsScheduleID As long
    Protected mGranteeID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mExpectedDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mAmount As decimal

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

    Public  Property DisbursementsScheduleID() As long
        Get
		return mDisbursementsScheduleID
        End Get
        Set(ByVal value As long)
		mDisbursementsScheduleID = value
        End Set
    End Property

    Public  Property GranteeID() As long
        Get
		return mGranteeID
        End Get
        Set(ByVal value As long)
		mGranteeID = value
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

    Public  Property ExpectedDate() As string
        Get
		return mExpectedDate
        End Get
        Set(ByVal value As string)
		mExpectedDate = value
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

    Public  Property Amount() As decimal
        Get
		return mAmount
        End Get
        Set(ByVal value As decimal)
		mAmount = value
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

    DisbursementsScheduleID = 0
    mGranteeID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mExpectedDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mAmount = 0.0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mDisbursementsScheduleID) 

    End Function 

    Public Overridable Function Retrieve(ByVal DisbursementsScheduleID As Long) As Boolean 

        Dim sql As String 

        If DisbursementsScheduleID > 0 Then 
            sql = "SELECT * FROM tblDisbursementsSchedule WHERE DisbursementsScheduleID = " & DisbursementsScheduleID
        Else 
            sql = "SELECT * FROM tblDisbursementsSchedule WHERE DisbursementsScheduleID = " & mDisbursementsScheduleID
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

                log.Error("DisbursementsSchedule not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetDisbursementsSchedule() As System.Data.DataSet

        Return GetDisbursementsSchedule(mDisbursementsScheduleID)

    End Function

    Public Overridable Function GetDisbursementsSchedule(ByVal DisbursementsScheduleID As Long) As DataSet

        Dim sql As String

        If DisbursementsScheduleID > 0 Then
            sql = "SELECT * FROM tblDisbursementsSchedule WHERE DisbursementsScheduleID = " & DisbursementsScheduleID
        Else
            sql = "SELECT * FROM tblDisbursementsSchedule WHERE DisbursementsScheduleID = " & mDisbursementsScheduleID
        End If

        Return GetDisbursementsSchedule(sql)

    End Function

    Public Overridable Function GetDisbursementsSchedules(ByVal GranteeID As Long) As DataSet

        Dim sql As String

        If GranteeID > 0 Then
            sql = "SELECT * FROM tblDisbursementsSchedule WHERE GranteeID = " & GranteeID
        Else
            sql = "SELECT * FROM tblDisbursementsSchedule WHERE GranteeID = " & mGranteeID
        End If

        Return GetDisbursementsSchedule(sql)

    End Function

    Protected Overridable Function GetDisbursementsSchedule(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mDisbursementsScheduleID = Catchnull(.Item("DisbursementsScheduleID"), 0)
            mGranteeID = Catchnull(.Item("GranteeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mExpectedDate = Catchnull(.Item("ExpectedDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mAmount = Catchnull(.Item("Amount"), 0.0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@DisbursementsScheduleID", DbType.Int32, mDisbursementsScheduleID)
        db.AddInParameter(cmd, "@GranteeID", DbType.Int32, mGranteeID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ExpectedDate", DbType.String, mExpectedDate)
        db.AddInParameter(cmd, "@Amount", DbType.Decimal, mAmount)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_DisbursementsSchedule")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mDisbursementsScheduleID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblDisbursementsSchedule SET Deleted = 1 WHERE DisbursementsScheduleID = " & mDisbursementsScheduleID) 
        Return Delete("DELETE FROM tblDisbursementsSchedule WHERE DisbursementsScheduleID = " & mDisbursementsScheduleID)

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