Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class InitialSessionProblemCategory

#region "Variables"

    Protected mInitialSessionProblemCategory As long
    Protected mInitialCounsellingSessionID As long
    Protected mProblemID As long

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

    Public  Property InitialSessionProblemCategory() As long
        Get
		return mInitialSessionProblemCategory
        End Get
        Set(ByVal value As long)
		mInitialSessionProblemCategory = value
        End Set
    End Property

    Public  Property InitialCounsellingSessionID() As long
        Get
		return mInitialCounsellingSessionID
        End Get
        Set(ByVal value As long)
		mInitialCounsellingSessionID = value
        End Set
    End Property

    Public  Property ProblemID() As long
        Get
		return mProblemID
        End Get
        Set(ByVal value As long)
		mProblemID = value
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

    InitialSessionProblemCategory = 0
    mInitialCounsellingSessionID = 0
    mProblemID = 0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mInitialSessionProblemCategory) 

    End Function 

    Public Overridable Function Retrieve(ByVal InitialSessionProblemCategory As Long) As Boolean 

        Dim sql As String 

        If InitialSessionProblemCategory > 0 Then 
            sql = "SELECT * FROM tblInitialSessionProblemCategory WHERE InitialSessionProblemCategory = " & InitialSessionProblemCategory
        Else 
            sql = "SELECT * FROM tblInitialSessionProblemCategory WHERE InitialSessionProblemCategory = " & mInitialSessionProblemCategory
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

                log.Error("InitialSessionProblemCategory not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Function CheckExistence() As Boolean

        Dim sql As String = "SELECT * FROM tblInitialSessionProblemCategory WHERE InitialCounsellingSessionID = " & mInitialCounsellingSessionID & " AND ProblemID = " & mProblemID

        Return GetInitialSessionProblemCategory(sql).Tables(0).Rows.Count > 0

    End Function

    Public Overridable Function GetInitialSessionProblemCategory() As System.Data.DataSet

        Return GetInitialSessionProblemCategory(mInitialSessionProblemCategory)

    End Function

    Public Overridable Function GetInitialSessionProblemCategory(ByVal InitialCounsellingSessionID As Long) As DataSet

        Dim sql As String

        If InitialCounsellingSessionID > 0 Then
            sql = "SELECT * FROM tblInitialSessionProblemCategory WHERE InitialCounsellingSessionID = " & InitialCounsellingSessionID
        Else
            sql = "SELECT * FROM tblInitialSessionProblemCategory WHERE InitialCounsellingSessionID = " & mInitialCounsellingSessionID
        End If

        Return GetInitialSessionProblemCategory(sql)

    End Function

    Protected Overridable Function GetInitialSessionProblemCategory(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mInitialSessionProblemCategory = Catchnull(.Item("InitialSessionProblemCategory"), 0)
            mInitialCounsellingSessionID = Catchnull(.Item("InitialCounsellingSessionID"), 0)
            mProblemID = Catchnull(.Item("ProblemID"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@InitialSessionProblemCategory", DbType.Int32, mInitialSessionProblemCategory)
        db.AddInParameter(cmd, "@InitialCounsellingSessionID", DbType.Int32, mInitialCounsellingSessionID)
        db.AddInParameter(cmd, "@ProblemID", DbType.Int32, mProblemID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_InitialSessionProblemCategory")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mInitialSessionProblemCategory = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblInitialSessionProblemCategory SET Deleted = 1 WHERE InitialSessionProblemCategory = " & mInitialSessionProblemCategory) 
        Return Delete("DELETE FROM tblInitialSessionProblemCategory WHERE InitialSessionProblemCategory = " & mInitialSessionProblemCategory)

    End Function

    Public Overridable Function DeleteEntry() As Boolean

        'Return Delete("UPDATE tblProjectStrategicObjectives SET Deleted = 1 WHERE ProjectStrategicObjectiveID = " & mProjectStrategicObjectiveID) 
        Return Delete("DELETE FROM tblInitialSessionProblemCategory WHERE InitialCounsellingSessionID = " & mInitialCounsellingSessionID & " AND ProblemID = " & mProblemID)

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