Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ActivityCategory

#region "Variables"

    Protected mActivityActivityCategoryID As long
    Protected mActivityCategoryID As long
    Protected mActivityID As long
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

    Public  Property ActivityActivityCategoryID() As long
        Get
		return mActivityActivityCategoryID
        End Get
        Set(ByVal value As long)
		mActivityActivityCategoryID = value
        End Set
    End Property

    Public  Property ActivityCategoryID() As long
        Get
		return mActivityCategoryID
        End Get
        Set(ByVal value As long)
		mActivityCategoryID = value
        End Set
    End Property

    Public  Property ActivityID() As long
        Get
		return mActivityID
        End Get
        Set(ByVal value As long)
		mActivityID = value
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

    ActivityActivityCategoryID = 0
    mActivityCategoryID = 0
    mActivityID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mActivityActivityCategoryID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ActivityActivityCategoryID As Long) As Boolean 

        Dim sql As String 

        If ActivityActivityCategoryID > 0 Then 
            sql = "SELECT * FROM tblActivityActivityCategory WHERE ActivityActivityCategoryID = " & ActivityActivityCategoryID
        Else 
            sql = "SELECT * FROM tblActivityActivityCategory WHERE ActivityActivityCategoryID = " & mActivityActivityCategoryID
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

                log.Error("ActivityCategory not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetActivityCategory() As System.Data.DataSet

        Return GetActivityCategory(mActivityActivityCategoryID)

    End Function

    Public Overridable Function GetActivityCategory(ByVal ActivityActivityCategoryID As Long) As DataSet

        Dim sql As String

        If ActivityActivityCategoryID > 0 Then
            sql = "SELECT * FROM tblActivityActivityCategory WHERE ActivityActivityCategoryID = " & ActivityActivityCategoryID
        Else
            sql = "SELECT * FROM tblActivityActivityCategory WHERE ActivityActivityCategoryID = " & mActivityActivityCategoryID
        End If

        Return GetActivityCategory(sql)

    End Function

    Public Overridable Function GetActivityCategory(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mActivityActivityCategoryID = Catchnull(.Item("ActivityActivityCategoryID"), 0)
            mActivityCategoryID = Catchnull(.Item("ActivityCategoryID"), 0)
            mActivityID = Catchnull(.Item("ActivityID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ActivityActivityCategoryID", DBType.Int32, mActivityActivityCategoryID)
        db.AddInParameter(cmd, "@ActivityCategoryID", DBType.Int32, mActivityCategoryID)
        db.AddInParameter(cmd, "@ActivityID", DBType.Int32, mActivityID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ActivityCategory")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mActivityActivityCategoryID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblActivityActivityCategory SET Deleted = 1 WHERE ActivityActivityCategoryID = " & mActivityActivityCategoryID) 
        Return Delete("DELETE FROM tblActivityActivityCategory WHERE ActivityActivityCategoryID = " & mActivityActivityCategoryID)

    End Function

    Public Overridable Function DeleteEntries() As Boolean

        'Return Delete("UPDATE tblActivityActivityCategory SET Deleted = 1 WHERE ActivityActivityCategoryID = " & mActivityActivityCategoryID) 
        Return Delete("DELETE FROM tblActivityActivityCategory WHERE ActivityCategoryID = " & mActivityCategoryID & " AND ActivityID = " & mActivityID)

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