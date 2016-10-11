Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions

Public Class OutputActivity

#Region "Variables"

    Protected mOutputActivityID As Long
    Protected mActivityID As Long
    Protected mOutputID As Long
    Protected mCreatedBy As Long
    Protected mUpdatedBy As Long
    Protected mCreatedDate As String
    Protected mUpdatedDate As String

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

    Public Property OutputActivityID() As Long
        Get
            Return mOutputActivityID
        End Get
        Set(ByVal value As Long)
            mOutputActivityID = value
        End Set
    End Property

    Public Property ActivityID() As Long
        Get
            Return mActivityID
        End Get
        Set(ByVal value As Long)
            mActivityID = value
        End Set
    End Property

    Public Property CreatedBy() As Long
        Get
            Return mCreatedBy
        End Get
        Set(ByVal value As Long)
            mCreatedBy = value
        End Set
    End Property

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
        End Set
    End Property

    Public Property CreatedDate() As String
        Get
            Return mCreatedDate
        End Get
        Set(ByVal value As String)
            mCreatedDate = value
        End Set
    End Property

    Public Property UpdatedDate() As String
        Get
            Return mUpdatedDate
        End Get
        Set(ByVal value As String)
            mUpdatedDate = value
        End Set
    End Property

    Public Property OutputID() As Long
        Get
            Return mOutputID
        End Get
        Set(ByVal value As Long)
            mOutputID = value
        End Set
    End Property

#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub

#End Region

    Public Sub Clear()

        mOutputActivityID = 0
        mActivityID = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mCreatedDate = ""
        mUpdatedDate = ""
        mOutputID = 0

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mOutputActivityID)

    End Function

    Public Overridable Function Retrieve(ByVal OutputActivityID As Long) As Boolean

        Dim sql As String

        If OutputActivityID > 0 Then
            sql = "SELECT * FROM tblOutputActivities WHERE OutputActivityID = " & OutputActivityID
        Else
            sql = "SELECT * FROM tblOutputActivities WHERE OutputActivityID = " & mOutputActivityID
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

                log.Warn("ProjectActivity not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetOutputActivity() As System.Data.DataSet

        Return GetOutputActivity(mOutputActivityID)

    End Function

    Public Overridable Function GetOutputActivity(ByVal OutputActivityID As Long) As DataSet

        Dim sql As String

        If OutputActivityID > 0 Then
            sql = "SELECT * FROM tblOutputActivities WHERE OutputActivityID = " & OutputActivityID
        Else
            sql = "SELECT * FROM tblOutputActivities WHERE OutputActivityID = " & mOutputActivityID
        End If

        Return GetOutputActivity(sql)

    End Function

    Protected Overridable Function GetOutputActivity(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mOutputActivityID = Catchnull(.Item("OutputActivityID"), 0)
            mActivityID = Catchnull(.Item("ActivityID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mOutputID = Catchnull(.Item("OutputID"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@OutputActivityID", DbType.Int32, mOutputActivityID)
        db.AddInParameter(cmd, "@ActivityID", DbType.Int32, mActivityID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@OutputID", DbType.String, mOutputID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_OutputActivity")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mOutputActivityID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProjectActivitys SET Deleted = 1 WHERE OutputActivityID = " & mOutputActivityID) 
        Return Delete("DELETE FROM tblOutputActivities WHERE OutputActivityID = " & mOutputActivityID)

    End Function

    Public Function DeleteEntries() As Boolean

        Return Delete("DELETE FROM tblOutputActivities WHERE OutputID = " & mOutputID & " AND ActivityID = " & mActivityID)

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

#End Region

End Class

