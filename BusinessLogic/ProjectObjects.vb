Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ProjectObjects

#Region "Variables"

    Protected mProjectObjectID As Long
    Protected mProjectID As Long
    Protected mObjectID As Long
    Protected mObjectTypeID As Long
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

    Public Property ProjectObjectID() As Long
        Get
            Return mProjectObjectID
        End Get
        Set(ByVal value As Long)
            mProjectObjectID = value
        End Set
    End Property

    Public Property ProjectID() As Long
        Get
            Return mProjectID
        End Get
        Set(ByVal value As Long)
            mProjectID = value
        End Set
    End Property

    Public Property ObjectID() As Long
        Get
            Return mObjectID
        End Get
        Set(ByVal value As Long)
            mObjectID = value
        End Set
    End Property

    Public Property ObjectTypeID() As Long
        Get
            Return mObjectTypeID
        End Get
        Set(ByVal value As Long)
            mObjectTypeID = value
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

        ProjectObjectID = 0
        mProjectID = 0
        mObjectID = 0
        mObjectTypeID = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mCreatedDate = ""
        mUpdatedDate = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mProjectObjectID)

    End Function

    Public Overridable Function Retrieve(ByVal ProjectObjectID As Long) As Boolean

        Dim sql As String

        If ProjectObjectID > 0 Then
            sql = "SELECT * FROM tblProjectObjects WHERE ProjectObjectID = " & ProjectObjectID
        Else
            sql = "SELECT * FROM tblProjectObjects WHERE ProjectObjectID = " & mProjectObjectID
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

                log.Warn("ProjectObjects not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetProjectObjects() As System.Data.DataSet

        Return GetProjectObjects(mProjectObjectID)

    End Function

    Public Overridable Function GetProjectObjects(ByVal ProjectObjectID As Long) As DataSet

        Dim sql As String

        If ProjectObjectID > 0 Then
            sql = "SELECT * FROM tblProjectObjects WHERE ProjectObjectID = " & ProjectObjectID
        Else
            sql = "SELECT * FROM tblProjectObjects WHERE ProjectObjectID = " & mProjectObjectID
        End If

        Return GetProjectObjects(sql)

    End Function

    Protected Overridable Function GetProjectObjects(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mProjectObjectID = Catchnull(.Item("ProjectObjectID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mObjectID = Catchnull(.Item("ObjectID"), 0)
            mObjectTypeID = Catchnull(.Item("ObjectTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ProjectObjectID", DbType.Int32, mProjectObjectID)
        db.AddInParameter(cmd, "@ProjectID", DbType.Int32, mProjectID)
        db.AddInParameter(cmd, "@ObjectID", DbType.Int32, mObjectID)
        db.AddInParameter(cmd, "@ObjectTypeID", DbType.Int32, mObjectTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ProjectObjects")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mProjectObjectID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProjectObjects SET Deleted = 1 WHERE ProjectObjectID = " & mProjectObjectID) 
        Return Delete("DELETE FROM tblProjectObjects WHERE ProjectObjectID = " & mProjectObjectID)

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