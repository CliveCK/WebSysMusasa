Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class KeyChangePromises

#region "Variables"

    Protected mKeyChangePromiseID As long
    Protected mStrategicObjectiveID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mKeyChangePromiseNo As string

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

    Public  Property KeyChangePromiseID() As long
        Get
		return mKeyChangePromiseID
        End Get
        Set(ByVal value As long)
		mKeyChangePromiseID = value
        End Set
    End Property

    Public  Property StrategicObjectiveID() As long
        Get
		return mStrategicObjectiveID
        End Get
        Set(ByVal value As long)
		mStrategicObjectiveID = value
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

    Public  Property KeyChangePromiseNo() As string
        Get
		return mKeyChangePromiseNo
        End Get
        Set(ByVal value As string)
		mKeyChangePromiseNo = value
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

    KeyChangePromiseID = 0
    mStrategicObjectiveID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mKeyChangePromiseNo = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mKeyChangePromiseID) 

    End Function 

    Public Overridable Function Retrieve(ByVal KeyChangePromiseID As Long) As Boolean 

        Dim sql As String 

        If KeyChangePromiseID > 0 Then 
            sql = "SELECT * FROM tblKeyChangePromises WHERE KeyChangePromiseID = " & KeyChangePromiseID
        Else 
            sql = "SELECT * FROM tblKeyChangePromises WHERE KeyChangePromiseID = " & mKeyChangePromiseID
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

                log.Warn("KeyChangePromises not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Function RetriveAll() As DataSet

        Dim sql = "SELECT S.Description As StrategicObjective, K.* FROM tblKeyChangePromises K inner join tblStrategicObjectives S on K.StrategicObjectiveID = S.StrategicObjectiveID"

        Return GetKeyChangePromises(sql)

    End Function

    Public Overridable Function GetKeyChangePromises() As System.Data.DataSet

        Return GetKeyChangePromises(mKeyChangePromiseID)

    End Function

    Public Overridable Function GetKeyChangePromises(ByVal KeyChangePromiseID As Long) As DataSet

        Dim sql As String

        If KeyChangePromiseID > 0 Then
            sql = "SELECT * FROM tblKeyChangePromises WHERE KeyChangePromiseID = " & KeyChangePromiseID
        Else
            sql = "SELECT * FROM tblKeyChangePromises WHERE KeyChangePromiseID = " & mKeyChangePromiseID
        End If

        Return GetKeyChangePromises(sql)

    End Function

    Protected Overridable Function GetKeyChangePromises(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mKeyChangePromiseID = Catchnull(.Item("KeyChangePromiseID"), 0)
            mStrategicObjectiveID = Catchnull(.Item("StrategicObjectiveID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mKeyChangePromiseNo = Catchnull(.Item("KeyChangePromiseNo"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@KeyChangePromiseID", DBType.Int32, mKeyChangePromiseID)
        db.AddInParameter(cmd, "@StrategicObjectiveID", DBType.Int32, mStrategicObjectiveID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@KeyChangePromiseNo", DBType.String, mKeyChangePromiseNo)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_KeyChangePromises")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mKeyChangePromiseID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblKeyChangePromises SET Deleted = 1 WHERE KeyChangePromiseID = " & mKeyChangePromiseID) 
        Return Delete("DELETE FROM tblKeyChangePromises WHERE KeyChangePromiseID = " & mKeyChangePromiseID)

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