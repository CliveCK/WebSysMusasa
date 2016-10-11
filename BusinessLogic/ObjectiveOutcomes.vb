Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ObjectiveOutcomes

#region "Variables"

    Protected mObjectiveOutcomeID As long
    Protected mObjectiveID As long
    Protected mOutcomeID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

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

    Public  Property ObjectiveOutcomeID() As long
        Get
		return mObjectiveOutcomeID
        End Get
        Set(ByVal value As long)
		mObjectiveOutcomeID = value
        End Set
    End Property

    Public  Property ObjectiveID() As long
        Get
		return mObjectiveID
        End Get
        Set(ByVal value As long)
		mObjectiveID = value
        End Set
    End Property

    Public  Property OutcomeID() As long
        Get
		return mOutcomeID
        End Get
        Set(ByVal value As long)
		mOutcomeID = value
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

    ObjectiveOutcomeID = 0
    mObjectiveID = 0
    mOutcomeID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mObjectiveOutcomeID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ObjectiveOutcomeID As Long) As Boolean 

        Dim sql As String 

        If ObjectiveOutcomeID > 0 Then 
            sql = "SELECT * FROM tblObjectiveOutcomes WHERE ObjectiveOutcomeID = " & ObjectiveOutcomeID
        Else 
            sql = "SELECT * FROM tblObjectiveOutcomes WHERE ObjectiveOutcomeID = " & mObjectiveOutcomeID
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

                log.Error("ObjectiveOutcomes not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetObjectiveOutcomes() As System.Data.DataSet

        Return GetObjectiveOutcomes(mObjectiveOutcomeID)

    End Function

    Public Overridable Function GetObjectiveOutcomes(ByVal ObjectiveOutcomeID As Long) As DataSet

        Dim sql As String

        If ObjectiveOutcomeID > 0 Then
            sql = "SELECT * FROM tblObjectiveOutcomes WHERE ObjectiveOutcomeID = " & ObjectiveOutcomeID
        Else
            sql = "SELECT * FROM tblObjectiveOutcomes WHERE ObjectiveOutcomeID = " & mObjectiveOutcomeID
        End If

        Return GetObjectiveOutcomes(sql)

    End Function

    Protected Overridable Function GetObjectiveOutcomes(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mObjectiveOutcomeID = Catchnull(.Item("ObjectiveOutcomeID"), 0)
            mObjectiveID = Catchnull(.Item("ObjectiveID"), 0)
            mOutcomeID = Catchnull(.Item("OutcomeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ObjectiveOutcomeID", DBType.Int32, mObjectiveOutcomeID)
        db.AddInParameter(cmd, "@ObjectiveID", DBType.Int32, mObjectiveID)
        db.AddInParameter(cmd, "@OutcomeID", DBType.Int32, mOutcomeID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ObjectiveOutcomes")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mObjectiveOutcomeID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblObjectiveOutcomes SET Deleted = 1 WHERE ObjectiveOutcomeID = " & mObjectiveOutcomeID) 
        Return Delete("DELETE FROM tblObjectiveOutcomes WHERE ObjectiveOutcomeID = " & mObjectiveOutcomeID)

    End Function

    Public Function DeleteEntries() As Boolean

        'Return Delete("UPDATE tblObjectiveOutcomes SET Deleted = 1 WHERE ObjectiveOutcomeID = " & mObjectiveOutcomeID) 
        Return Delete("DELETE FROM tblObjectiveOutcomes WHERE ObjectiveID = " & mObjectiveID & " AND OutcomeID = " & mOutcomeID)

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