Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class InterventionObjects

#region "Variables"

    Protected mInterventionObjectID As long
    Protected mInterventionID As long
    Protected mObjectID As long
    Protected mObjectTypeID As long
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

    Public  Property InterventionObjectID() As long
        Get
		return mInterventionObjectID
        End Get
        Set(ByVal value As long)
		mInterventionObjectID = value
        End Set
    End Property

    Public  Property InterventionID() As long
        Get
		return mInterventionID
        End Get
        Set(ByVal value As long)
		mInterventionID = value
        End Set
    End Property

    Public  Property ObjectID() As long
        Get
		return mObjectID
        End Get
        Set(ByVal value As long)
		mObjectID = value
        End Set
    End Property

    Public  Property ObjectTypeID() As long
        Get
		return mObjectTypeID
        End Get
        Set(ByVal value As long)
		mObjectTypeID = value
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

    InterventionObjectID = 0
    mInterventionID = 0
    mObjectID = 0
    mObjectTypeID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mInterventionObjectID) 

    End Function 

    Public Overridable Function Retrieve(ByVal InterventionObjectID As Long) As Boolean 

        Dim sql As String 

        If InterventionObjectID > 0 Then 
            sql = "SELECT * FROM tblInterventionObjects WHERE InterventionObjectID = " & InterventionObjectID
        Else 
            sql = "SELECT * FROM tblInterventionObjects WHERE InterventionObjectID = " & mInterventionObjectID
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

                log.Warn("InterventionObjects not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetInterventionObjects() As System.Data.DataSet

        Return GetInterventionObjects(mInterventionObjectID)

    End Function

    Public Overridable Function GetInterventionObjects(ByVal InterventionObjectID As Long) As DataSet

        Dim sql As String

        If InterventionObjectID > 0 Then
            sql = "SELECT * FROM tblInterventionObjects WHERE InterventionObjectID = " & InterventionObjectID
        Else
            sql = "SELECT * FROM tblInterventionObjects WHERE InterventionObjectID = " & mInterventionObjectID
        End If

        Return GetInterventionObjects(sql)

    End Function

    Protected Overridable Function GetInterventionObjects(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mInterventionObjectID = Catchnull(.Item("InterventionObjectID"), 0)
            mInterventionID = Catchnull(.Item("InterventionID"), 0)
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

        db.AddInParameter(cmd, "@InterventionObjectID", DBType.Int32, mInterventionObjectID)
        db.AddInParameter(cmd, "@InterventionID", DBType.Int32, mInterventionID)
        db.AddInParameter(cmd, "@ObjectID", DBType.Int32, mObjectID)
        db.AddInParameter(cmd, "@ObjectTypeID", DBType.Int32, mObjectTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_InterventionObjects")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mInterventionObjectID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblInterventionObjects SET Deleted = 1 WHERE InterventionObjectID = " & mInterventionObjectID) 
        Return Delete("DELETE FROM tblInterventionObjects WHERE InterventionObjectID = " & mInterventionObjectID)

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