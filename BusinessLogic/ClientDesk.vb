Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ClientDesk

#region "Variables"

    Protected mClientDeskInforID As long
    Protected mAge As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mName As string
    Protected mSex As string
    Protected mWhereFrom As string
    Protected mInformationProvided As string

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

    Public  Property ClientDeskInforID() As long
        Get
		return mClientDeskInforID
        End Get
        Set(ByVal value As long)
		mClientDeskInforID = value
        End Set
    End Property

    Public  Property Age() As long
        Get
		return mAge
        End Get
        Set(ByVal value As long)
		mAge = value
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

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
        End Set
    End Property

    Public  Property Sex() As string
        Get
		return mSex
        End Get
        Set(ByVal value As string)
		mSex = value
        End Set
    End Property

    Public  Property WhereFrom() As string
        Get
		return mWhereFrom
        End Get
        Set(ByVal value As string)
		mWhereFrom = value
        End Set
    End Property

    Public  Property InformationProvided() As string
        Get
		return mInformationProvided
        End Get
        Set(ByVal value As string)
		mInformationProvided = value
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

    ClientDeskInforID = 0
    mAge = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mName = ""
    mSex = ""
    mWhereFrom = ""
    mInformationProvided = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mClientDeskInforID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ClientDeskInforID As Long) As Boolean 

        Dim sql As String 

        If ClientDeskInforID > 0 Then 
            sql = "SELECT * FROM tblClientDeskInfor WHERE ClientDeskInforID = " & ClientDeskInforID
        Else 
            sql = "SELECT * FROM tblClientDeskInfor WHERE ClientDeskInforID = " & mClientDeskInforID
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

                log.Error("ClientDesk not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetClientDesk() As System.Data.DataSet

        Return GetClientDesk(mClientDeskInforID)

    End Function

    Public Overridable Function GetClientDeskAll() As System.Data.DataSet

        Return GetClientDesk("SELECT * FROM tblClientDeskInfor")

    End Function

    Public Overridable Function GetClientDesk(ByVal ClientDeskInforID As Long) As DataSet

        Dim sql As String

        If ClientDeskInforID > 0 Then
            sql = "SELECT * FROM tblClientDeskInfor WHERE ClientDeskInforID = " & ClientDeskInforID
        Else
            sql = "SELECT * FROM tblClientDeskInfor WHERE ClientDeskInforID = " & mClientDeskInforID
        End If

        Return GetClientDesk(sql)

    End Function

    Protected Overridable Function GetClientDesk(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mClientDeskInforID = Catchnull(.Item("ClientDeskInforID"), 0)
            mAge = Catchnull(.Item("Age"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mName = Catchnull(.Item("Name"), "")
            mSex = Catchnull(.Item("Sex"), "")
            mWhereFrom = Catchnull(.Item("WhereFrom"), "")
            mInformationProvided = Catchnull(.Item("InformationProvided"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ClientDeskInforID", DbType.Int32, mClientDeskInforID)
        db.AddInParameter(cmd, "@Age", DbType.Int32, mAge)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Name", DbType.String, mName)
        db.AddInParameter(cmd, "@Sex", DbType.String, mSex)
        db.AddInParameter(cmd, "@WhereFrom", DbType.String, mWhereFrom)
        db.AddInParameter(cmd, "@InformationProvided", DbType.String, mInformationProvided)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ClientDesk")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mClientDeskInforID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblClientDeskInfor SET Deleted = 1 WHERE ClientDeskInforID = " & mClientDeskInforID) 
        Return Delete("DELETE FROM tblClientDeskInfor WHERE ClientDeskInforID = " & mClientDeskInforID)

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