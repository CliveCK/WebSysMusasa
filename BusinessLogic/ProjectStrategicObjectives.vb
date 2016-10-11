Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ProjectStrategicObjectives

#region "Variables"

    Protected mProjectStrategicObjectiveID As long
    Protected mProjectID As long
    Protected mStrategicObjectiveID As long

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

    Public  Property ProjectStrategicObjectiveID() As long
        Get
		return mProjectStrategicObjectiveID
        End Get
        Set(ByVal value As long)
		mProjectStrategicObjectiveID = value
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

    Public  Property StrategicObjectiveID() As long
        Get
		return mStrategicObjectiveID
        End Get
        Set(ByVal value As long)
		mStrategicObjectiveID = value
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

    ProjectStrategicObjectiveID = 0
    mProjectID = 0
    mStrategicObjectiveID = 0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mProjectStrategicObjectiveID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ProjectStrategicObjectiveID As Long) As Boolean 

        Dim sql As String 

        If ProjectStrategicObjectiveID > 0 Then 
            sql = "SELECT * FROM tblProjectStrategicObjectives WHERE ProjectStrategicObjectiveID = " & ProjectStrategicObjectiveID
        Else 
            sql = "SELECT * FROM tblProjectStrategicObjectives WHERE ProjectStrategicObjectiveID = " & mProjectStrategicObjectiveID
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

                log.Error("ProjectStrategicObjectives not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetProjectStrategicObjectives() As System.Data.DataSet

        Return GetProjectStrategicObjectives(mProjectStrategicObjectiveID)

    End Function

    Public Overridable Function GetProjectStrategicObjectives(ByVal ProjectStrategicObjectiveID As Long) As DataSet

        Dim sql As String

        If ProjectStrategicObjectiveID > 0 Then
            sql = "SELECT * FROM tblProjectStrategicObjectives WHERE ProjectStrategicObjectiveID = " & ProjectStrategicObjectiveID
        Else
            sql = "SELECT * FROM tblProjectStrategicObjectives WHERE ProjectStrategicObjectiveID = " & mProjectStrategicObjectiveID
        End If

        Return GetProjectStrategicObjectives(sql)

    End Function

    Public Overridable Function GetProjectStrategicObjectivesByProjectID(ByVal ProjectID As Long) As DataSet

        Dim sql As String

        If ProjectID > 0 Then
            sql = "SELECT * FROM tblProjectStrategicObjectives WHERE ProjectID = " & ProjectID
        Else
            sql = "SELECT * FROM tblProjectStrategicObjectives WHERE ProjectID = " & mProjectID
        End If

        Return GetProjectStrategicObjectives(sql)

    End Function

    Public Function CheckExistence() As Boolean

        Dim sql As String = "SELECT * FROM tblProjectStrategicObjectives WHERE ProjectID = " & mProjectID & " AND StrategicObjectiveID = " & mStrategicObjectiveID

        Return GetProjectStrategicObjectives(sql).Tables(0).Rows.Count > 0

    End Function

    Protected Overridable Function GetProjectStrategicObjectives(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mProjectStrategicObjectiveID = Catchnull(.Item("ProjectStrategicObjectiveID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mStrategicObjectiveID = Catchnull(.Item("StrategicObjectiveID"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ProjectStrategicObjectiveID", DbType.Int32, mProjectStrategicObjectiveID)
        db.AddInParameter(cmd, "@ProjectID", DbType.Int32, mProjectID)
        db.AddInParameter(cmd, "@StrategicObjectiveID", DbType.Int32, mStrategicObjectiveID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ProjectStrategicObjectives")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mProjectStrategicObjectiveID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProjectStrategicObjectives SET Deleted = 1 WHERE ProjectStrategicObjectiveID = " & mProjectStrategicObjectiveID) 
        Return Delete("DELETE FROM tblProjectStrategicObjectives WHERE ProjectStrategicObjectiveID = " & mProjectStrategicObjectiveID)

    End Function

    Public Overridable Function DeleteEntry() As Boolean

        'Return Delete("UPDATE tblProjectStrategicObjectives SET Deleted = 1 WHERE ProjectStrategicObjectiveID = " & mProjectStrategicObjectiveID) 
        Return Delete("DELETE FROM tblProjectStrategicObjectives WHERE ProjectID = " & mProjectID & " AND StrategicObjectiveID = " & mStrategicObjectiveID)

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