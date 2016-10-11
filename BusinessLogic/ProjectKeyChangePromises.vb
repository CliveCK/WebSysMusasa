Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ProjectKeyChangePromises

#region "Variables"

    Protected mProjectKeyChangePromiseID As long
    Protected mProjectID As long
    Protected mKeyChangePromiseID As long

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

    Public  Property ProjectKeyChangePromiseID() As long
        Get
		return mProjectKeyChangePromiseID
        End Get
        Set(ByVal value As long)
		mProjectKeyChangePromiseID = value
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

    Public  Property KeyChangePromiseID() As long
        Get
		return mKeyChangePromiseID
        End Get
        Set(ByVal value As long)
		mKeyChangePromiseID = value
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

    ProjectKeyChangePromiseID = 0
    mProjectID = 0
    mKeyChangePromiseID = 0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mProjectKeyChangePromiseID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ProjectKeyChangePromiseID As Long) As Boolean 

        Dim sql As String 

        If ProjectKeyChangePromiseID > 0 Then 
            sql = "SELECT * FROM tblProjectKeyChangePromises WHERE ProjectKeyChangePromiseID = " & ProjectKeyChangePromiseID
        Else 
            sql = "SELECT * FROM tblProjectKeyChangePromises WHERE ProjectKeyChangePromiseID = " & mProjectKeyChangePromiseID
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

                log.Error("ProjectKeyChangePromises not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetProjectKeyChangePromises() As System.Data.DataSet

        Return GetProjectKeyChangePromises(mProjectKeyChangePromiseID)

    End Function

    Public Overridable Function GetProjectKeyChangePromises(ByVal ProjectKeyChangePromiseID As Long) As DataSet

        Dim sql As String

        If ProjectKeyChangePromiseID > 0 Then
            sql = "SELECT * FROM tblProjectKeyChangePromises WHERE ProjectKeyChangePromiseID = " & ProjectKeyChangePromiseID
        Else
            sql = "SELECT * FROM tblProjectKeyChangePromises WHERE ProjectKeyChangePromiseID = " & mProjectKeyChangePromiseID
        End If

        Return GetProjectKeyChangePromises(sql)

    End Function

    Public Overridable Function GetProjectKeyChangePromisesByProjectID(ByVal ProjectID As Long) As DataSet

        Dim sql As String

        If ProjectID > 0 Then
            sql = "SELECT * FROM tblProjectKeyChangePromises WHERE ProjectID = " & ProjectID
        Else
            sql = "SELECT * FROM tblProjectKeyChangePromises WHERE ProjectID = " & mProjectID
        End If

        Return GetProjectKeyChangePromises(sql)

    End Function

    Public Function CheckExistence() As Boolean

        Dim sql As String = "SELECT * FROM tblProjectKeyChangePromises WHERE ProjectID = " & mProjectID & " AND KeyChangePromiseID = " & mKeyChangePromiseID

        Return GetProjectKeyChangePromises(sql).Tables(0).Rows.Count > 0

    End Function

    Protected Overridable Function GetProjectKeyChangePromises(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mProjectKeyChangePromiseID = Catchnull(.Item("ProjectKeyChangePromiseID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mKeyChangePromiseID = Catchnull(.Item("KeyChangePromiseID"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ProjectKeyChangePromiseID", DbType.Int32, mProjectKeyChangePromiseID)
        db.AddInParameter(cmd, "@ProjectID", DbType.Int32, mProjectID)
        db.AddInParameter(cmd, "@KeyChangePromiseID", DbType.Int32, mKeyChangePromiseID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ProjectKeyChangePromises")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mProjectKeyChangePromiseID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProjectKeyChangePromises SET Deleted = 1 WHERE ProjectKeyChangePromiseID = " & mProjectKeyChangePromiseID) 
        Return Delete("DELETE FROM tblProjectKeyChangePromises WHERE ProjectKeyChangePromiseID = " & mProjectKeyChangePromiseID)

    End Function

    Public Overridable Function DeleteEntry() As Boolean

        'Return Delete("UPDATE tblProjectStrategicObjectives SET Deleted = 1 WHERE ProjectStrategicObjectiveID = " & mProjectStrategicObjectiveID) 
        Return Delete("DELETE FROM tblProjectKeyChangePromises WHERE ProjectID = " & mProjectID & " AND KeyChangePromiseID = " & mKeyChangePromiseID)

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