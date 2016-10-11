Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ProjectObjectivesKeyChangePromises

#region "Variables"

    Protected mProjectObjectivesKeyChangePromises As long
    Protected mProjectObjectiveID As long
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

    Public  Property ProjectObjectivesKeyChangePromises() As long
        Get
		return mProjectObjectivesKeyChangePromises
        End Get
        Set(ByVal value As long)
		mProjectObjectivesKeyChangePromises = value
        End Set
    End Property

    Public  Property ProjectObjectiveID() As long
        Get
		return mProjectObjectiveID
        End Get
        Set(ByVal value As long)
		mProjectObjectiveID = value
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

    ProjectObjectivesKeyChangePromises = 0
    mProjectObjectiveID = 0
    mKeyChangePromiseID = 0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mProjectObjectivesKeyChangePromises) 

    End Function 

    Public Overridable Function Retrieve(ByVal ProjectObjectivesKeyChangePromises As Long) As Boolean 

        Dim sql As String 

        If ProjectObjectivesKeyChangePromises > 0 Then 
            sql = "SELECT * FROM tblProjectObjectivesKeyChangePromises WHERE ProjectObjectivesKeyChangePromises = " & ProjectObjectivesKeyChangePromises
        Else 
            sql = "SELECT * FROM tblProjectObjectivesKeyChangePromises WHERE ProjectObjectivesKeyChangePromises = " & mProjectObjectivesKeyChangePromises
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

                log.Error("ProjectObjectivesKeyChangePromises not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetProjectObjectivesKeyChangePromises() As System.Data.DataSet

        Return GetProjectObjectivesKeyChangePromises(mProjectObjectivesKeyChangePromises)

    End Function

    Public Overridable Function GetProjectObjectivesKeyChangePromises(ByVal ProjectObjectivesKeyChangePromises As Long) As DataSet

        Dim sql As String

        If ProjectObjectivesKeyChangePromises > 0 Then
            sql = "SELECT * FROM tblProjectObjectivesKeyChangePromises WHERE ProjectObjectivesKeyChangePromises = " & ProjectObjectivesKeyChangePromises
        Else
            sql = "SELECT * FROM tblProjectObjectivesKeyChangePromises WHERE ProjectObjectivesKeyChangePromises = " & mProjectObjectivesKeyChangePromises
        End If

        Return GetProjectObjectivesKeyChangePromises(sql)

    End Function

    Protected Overridable Function GetProjectObjectivesKeyChangePromises(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mProjectObjectivesKeyChangePromises = Catchnull(.Item("ProjectObjectivesKeyChangePromises"), 0)
            mProjectObjectiveID = Catchnull(.Item("ProjectObjectiveID"), 0)
            mKeyChangePromiseID = Catchnull(.Item("KeyChangePromiseID"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ProjectObjectivesKeyChangePromises", DbType.Int32, mProjectObjectivesKeyChangePromises)
        db.AddInParameter(cmd, "@ProjectObjectiveID", DbType.Int32, mProjectObjectiveID)
        db.AddInParameter(cmd, "@KeyChangePromiseID", DbType.Int32, mKeyChangePromiseID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ProjectObjectivesKeyChangePromises")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mProjectObjectivesKeyChangePromises = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProjectObjectivesKeyChangePromises SET Deleted = 1 WHERE ProjectObjectivesKeyChangePromises = " & mProjectObjectivesKeyChangePromises) 
        Return Delete("DELETE FROM tblProjectObjectivesKeyChangePromises WHERE ProjectObjectivesKeyChangePromises = " & mProjectObjectivesKeyChangePromises)

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