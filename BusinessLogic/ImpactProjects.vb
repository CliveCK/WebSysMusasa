Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ImpactProjects

#region "Variables"

    Protected mImpactProjectID As long
    Protected mImpactID As long
    Protected mProjectID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

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

    Public  Property ImpactProjectID() As long
        Get
		return mImpactProjectID
        End Get
        Set(ByVal value As long)
		mImpactProjectID = value
        End Set
    End Property

    Public  Property ImpactID() As long
        Get
		return mImpactID
        End Get
        Set(ByVal value As long)
		mImpactID = value
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

    ImpactProjectID = 0
    mImpactID = 0
    mProjectID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mImpactProjectID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ImpactProjectID As Long) As Boolean 

        Dim sql As String 

        If ImpactProjectID > 0 Then 
            sql = "SELECT * FROM tblImpactProjects WHERE ImpactProjectID = " & ImpactProjectID
        Else 
            sql = "SELECT * FROM tblImpactProjects WHERE ImpactProjectID = " & mImpactProjectID
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

                log.Error("ImpactProjects not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetImpactProjects() As System.Data.DataSet

        Return GetImpactProjects(mImpactProjectID)

    End Function

    Public Overridable Function GetImpactProjects(ByVal ImpactProjectID As Long) As DataSet

        Dim sql As String

        If ImpactProjectID > 0 Then
            sql = "SELECT * FROM tblImpactProjects WHERE ImpactProjectID = " & ImpactProjectID
        Else
            sql = "SELECT * FROM tblImpactProjects WHERE ImpactProjectID = " & mImpactProjectID
        End If

        Return GetImpactProjects(sql)

    End Function

    Protected Overridable Function GetImpactProjects(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mImpactProjectID = Catchnull(.Item("ImpactProjectID"), 0)
            mImpactID = Catchnull(.Item("ImpactID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ImpactProjectID", DbType.Int32, mImpactProjectID)
        db.AddInParameter(cmd, "@ImpactID", DbType.Int32, mImpactID)
        db.AddInParameter(cmd, "@ProjectID", DbType.Int32, mProjectID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ImpactProjects")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mImpactProjectID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblImpactProjects SET Deleted = 1 WHERE ImpactProjectID = " & mImpactProjectID) 
        Return Delete("DELETE FROM tblImpactProjects WHERE ImpactProjectID = " & mImpactProjectID)

    End Function

    Public Overridable Function DeleteEntries() As Boolean

        'Return Delete("UPDATE tblImpactProjects SET Deleted = 1 WHERE ImpactProjectID = " & mImpactProjectID) 
        Return Delete("DELETE FROM tblImpactProjects WHERE ImpactID = " & mImpactID & " AND ProjectID = " & mProjectID)

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