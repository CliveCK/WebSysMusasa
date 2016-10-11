Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class KeyChangePromiseDescription

#region "Variables"

    Protected mKeyChangePromiseDescriptionID As long
    Protected mKeyChangePromiseID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mDescription As string

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

    Public  Property KeyChangePromiseDescriptionID() As long
        Get
		return mKeyChangePromiseDescriptionID
        End Get
        Set(ByVal value As long)
		mKeyChangePromiseDescriptionID = value
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

    Public  Property Description() As string
        Get
		return mDescription
        End Get
        Set(ByVal value As string)
		mDescription = value
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

    KeyChangePromiseDescriptionID = 0
    mKeyChangePromiseID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mDescription = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mKeyChangePromiseDescriptionID) 

    End Function 

    Public Overridable Function Retrieve(ByVal KeyChangePromiseDescriptionID As Long) As Boolean 

        Dim sql As String 

        If KeyChangePromiseDescriptionID > 0 Then 
            sql = "SELECT * FROM tblKeyChangePromiseDescription WHERE KeyChangePromiseDescriptionID = " & KeyChangePromiseDescriptionID
        Else 
            sql = "SELECT * FROM tblKeyChangePromiseDescription WHERE KeyChangePromiseDescriptionID = " & mKeyChangePromiseDescriptionID
        End If 

        Return Retrieve(sql) 

    End Function 

    Public Function RetrieveAll() As DataSet

        Dim sql As String = "SELECT * FROM tblKeyChangePromiseDescription"

        Return GetKeyChangePromiseDescription(sql)

    End Function

    Public Function GetDescriptionsByPromiseID(ByVal KeyChangePromiseID As Long) As DataSet

        Dim sql As String = "SELECT * FROM tblKeyChangePromiseDescription WHERE KeyChangePromiseID = " & KeyChangePromiseID

        Return GetKeyChangePromiseDescription(sql)

    End Function
    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean 

        Try 

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql) 

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then 

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0)) 

                dsRetrieve = Nothing 
                Return True 

            Else 

                log.Warn("KeyChangePromiseDescription not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetKeyChangePromiseDescription() As System.Data.DataSet

        Return GetKeyChangePromiseDescription(mKeyChangePromiseDescriptionID)

    End Function

    Public Overridable Function GetKeyChangePromiseDescription(ByVal KeyChangePromiseDescriptionID As Long) As DataSet

        Dim sql As String

        If KeyChangePromiseDescriptionID > 0 Then
            sql = "SELECT * FROM tblKeyChangePromiseDescription WHERE KeyChangePromiseDescriptionID = " & KeyChangePromiseDescriptionID
        Else
            sql = "SELECT * FROM tblKeyChangePromiseDescription WHERE KeyChangePromiseDescriptionID = " & mKeyChangePromiseDescriptionID
        End If

        Return GetKeyChangePromiseDescription(sql)

    End Function

    Protected Overridable Function GetKeyChangePromiseDescription(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mKeyChangePromiseDescriptionID = Catchnull(.Item("KeyChangePromiseDescriptionID"), 0)
            mKeyChangePromiseID = Catchnull(.Item("KeyChangePromiseID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mDescription = Catchnull(.Item("Description"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@KeyChangePromiseDescriptionID", DBType.Int32, mKeyChangePromiseDescriptionID)
        db.AddInParameter(cmd, "@KeyChangePromiseID", DBType.Int32, mKeyChangePromiseID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_KeyChangePromiseDescription")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mKeyChangePromiseDescriptionID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblKeyChangePromiseDescription SET Deleted = 1 WHERE KeyChangePromiseDescriptionID = " & mKeyChangePromiseDescriptionID) 
        Return Delete("DELETE FROM tblKeyChangePromiseDescription WHERE KeyChangePromiseDescriptionID = " & mKeyChangePromiseDescriptionID)

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