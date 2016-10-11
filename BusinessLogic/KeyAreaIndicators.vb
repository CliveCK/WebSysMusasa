Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions

Public Class KeyAreaIndicators

#Region "Variables"

    Protected mKeyAreaIndicatorID As Long
    Protected mKeyAreaID As Long
    Protected mIndicatorID As Long
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

    Public Property KeyAreaIndicatorID() As Long
        Get
            Return mKeyAreaIndicatorID
        End Get
        Set(ByVal value As Long)
            mKeyAreaIndicatorID = value
        End Set
    End Property

    Public Property KeyAreaID() As Long
        Get
            Return mKeyAreaID
        End Get
        Set(ByVal value As Long)
            mKeyAreaID = value
        End Set
    End Property

    Public Property IndicatorID() As Long
        Get
            Return mIndicatorID
        End Get
        Set(ByVal value As Long)
            mIndicatorID = value
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

        KeyAreaIndicatorID = 0
        mKeyAreaID = 0
        mIndicatorID = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mCreatedDate = ""
        mUpdatedDate = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mKeyAreaIndicatorID)

    End Function

    Public Overridable Function Retrieve(ByVal KeyAreaIndicatorID As Long) As Boolean

        Dim sql As String

        If KeyAreaIndicatorID > 0 Then
            sql = "SELECT * FROM tblKeyAreaIndicators WHERE KeyAreaIndicatorID = " & KeyAreaIndicatorID
        Else
            sql = "SELECT * FROM tblKeyAreaIndicators WHERE KeyAreaIndicatorID = " & mKeyAreaIndicatorID
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

                log.Error("KeyAreaIndicators not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetKeyAreaIndicators() As System.Data.DataSet

        Return GetKeyAreaIndicators(mKeyAreaIndicatorID)

    End Function

    Public Overridable Function GetKeyAreaIndicators(ByVal KeyAreaIndicatorID As Long) As DataSet

        Dim sql As String

        If KeyAreaIndicatorID > 0 Then
            sql = "SELECT * FROM tblKeyAreaIndicators WHERE KeyAreaIndicatorID = " & KeyAreaIndicatorID
        Else
            sql = "SELECT * FROM tblKeyAreaIndicators WHERE KeyAreaIndicatorID = " & mKeyAreaIndicatorID
        End If

        Return GetKeyAreaIndicators(sql)

    End Function

    Protected Overridable Function GetKeyAreaIndicators(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mKeyAreaIndicatorID = Catchnull(.Item("KeyAreaIndicatorID"), 0)
            mKeyAreaID = Catchnull(.Item("KeyAreaID"), 0)
            mIndicatorID = Catchnull(.Item("IndicatorID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@KeyAreaIndicatorID", DbType.Int32, mKeyAreaIndicatorID)
        db.AddInParameter(cmd, "@KeyAreaID", DbType.Int32, mKeyAreaID)
        db.AddInParameter(cmd, "@IndicatorID", DbType.Int32, mIndicatorID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_KeyAreaIndicators")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mKeyAreaIndicatorID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblKeyAreaIndicators SET Deleted = 1 WHERE KeyAreaIndicatorID = " & mKeyAreaIndicatorID) 
        Return Delete("DELETE FROM tblKeyAreaIndicators WHERE KeyAreaIndicatorID = " & mKeyAreaIndicatorID)

    End Function

    Public Function DeleteEntries() As Boolean

        Return Delete("DELETE FROM tblKeyAreaIndicators WHERE KeyAreaID = " & mKeyAreaID & " AND IndicatorID = " & IndicatorID)

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