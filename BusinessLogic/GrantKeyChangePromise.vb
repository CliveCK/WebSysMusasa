Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GrantKeyChangePromise

#region "Variables"

    Protected mGrantKeyChangePromiseID As long
    Protected mGrantDetailID As long
    Protected mKeyChangePromiseID As long
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

    Public  Property GrantKeyChangePromiseID() As long
        Get
		return mGrantKeyChangePromiseID
        End Get
        Set(ByVal value As long)
		mGrantKeyChangePromiseID = value
        End Set
    End Property

    Public  Property GrantDetailID() As long
        Get
		return mGrantDetailID
        End Get
        Set(ByVal value As long)
		mGrantDetailID = value
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

    GrantKeyChangePromiseID = 0
    mGrantDetailID = 0
    mKeyChangePromiseID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGrantKeyChangePromiseID) 

    End Function 

    Public Overridable Function Retrieve(ByVal GrantKeyChangePromiseID As Long) As Boolean 

        Dim sql As String 

        If GrantKeyChangePromiseID > 0 Then 
            sql = "SELECT * FROM tblGrantKeyChangePromises WHERE GrantKeyChangePromiseID = " & GrantKeyChangePromiseID
        Else 
            sql = "SELECT * FROM tblGrantKeyChangePromises WHERE GrantKeyChangePromiseID = " & mGrantKeyChangePromiseID
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

                log.Error("GrantKeyChangePromise not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGrantKeyChangePromise() As System.Data.DataSet

        Return GetGrantKeyChangePromise(mGrantKeyChangePromiseID)

    End Function

    Public Overridable Function GetGrantKeyChangePromise(ByVal GrantKeyChangePromiseID As Long) As DataSet

        Dim sql As String

        If GrantKeyChangePromiseID > 0 Then
            sql = "SELECT * FROM tblGrantKeyChangePromises WHERE GrantKeyChangePromiseID = " & GrantKeyChangePromiseID
        Else
            sql = "SELECT * FROM tblGrantKeyChangePromises WHERE GrantKeyChangePromiseID = " & mGrantKeyChangePromiseID
        End If

        Return GetGrantKeyChangePromise(sql)

    End Function

    Protected Overridable Function GetGrantKeyChangePromise(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Overridable Function GetGrantKeyChangePromisesByGrantDetailID(ByVal GrantDetailID As Long) As DataSet

        Dim sql As String

        If GrantDetailID > 0 Then
            sql = "SELECT * FROM tblGrantKeyChangePromises WHERE GrantDetailID = " & GrantDetailID
        Else
            sql = "SELECT * FROM tblGrantKeyChangePromises WHERE GrantDetailID = " & mGrantDetailID
        End If

        Return GetGrantKeyChangePromise(sql)

    End Function

    Public Function CheckExistence() As Boolean

        Dim sql As String = "SELECT * FROM tblGrantKeyChangePromises WHERE GrantDetailID = " & mGrantDetailID & " AND KeyChangePromiseID = " & mKeyChangePromiseID

        Return GetGrantKeyChangePromise(sql).Tables(0).Rows.Count > 0

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGrantKeyChangePromiseID = Catchnull(.Item("GrantKeyChangePromiseID"), 0)
            mGrantDetailID = Catchnull(.Item("GrantDetailID"), 0)
            mKeyChangePromiseID = Catchnull(.Item("KeyChangePromiseID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GrantKeyChangePromiseID", DbType.Int32, mGrantKeyChangePromiseID)
        db.AddInParameter(cmd, "@GrantDetailID", DbType.Int32, mGrantDetailID)
        db.AddInParameter(cmd, "@KeyChangePromiseID", DbType.Int32, mKeyChangePromiseID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GrantKeyChangePromise")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGrantKeyChangePromiseID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGrantKeyChangePromises SET Deleted = 1 WHERE GrantKeyChangePromiseID = " & mGrantKeyChangePromiseID) 
        Return Delete("DELETE FROM tblGrantKeyChangePromises WHERE GrantKeyChangePromiseID = " & mGrantKeyChangePromiseID)

    End Function

    Public Overridable Function DeleteEntry() As Boolean

        'Return Delete("UPDATE tblProjectStrategicObjectives SET Deleted = 1 WHERE ProjectStrategicObjectiveID = " & mProjectStrategicObjectiveID) 
        Return Delete("DELETE FROM tblGrantKeyChangePromises WHERE GrantDetailID = " & mGrantDetailID & " AND KeyChangePromiseID = " & mKeyChangePromiseID)

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