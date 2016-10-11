Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class StaffTrainingAccess

#region "Variables"

    Protected mStaffTrainingAccessID As long
    Protected mStaffID As long
    Protected mTrainingID As long
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

    Public  Property StaffTrainingAccessID() As long
        Get
		return mStaffTrainingAccessID
        End Get
        Set(ByVal value As long)
		mStaffTrainingAccessID = value
        End Set
    End Property

    Public  Property StaffID() As long
        Get
		return mStaffID
        End Get
        Set(ByVal value As long)
		mStaffID = value
        End Set
    End Property

    Public  Property TrainingID() As long
        Get
		return mTrainingID
        End Get
        Set(ByVal value As long)
		mTrainingID = value
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

    StaffTrainingAccessID = 0
    mStaffID = 0
    mTrainingID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mStaffTrainingAccessID) 

    End Function 

    Public Overridable Function Retrieve(ByVal StaffTrainingAccessID As Long) As Boolean 

        Dim sql As String 

        If StaffTrainingAccessID > 0 Then 
            sql = "SELECT * FROM tblStaffTrainingAccess WHERE StaffTrainingAccessID = " & StaffTrainingAccessID
        Else 
            sql = "SELECT * FROM tblStaffTrainingAccess WHERE StaffTrainingAccessID = " & mStaffTrainingAccessID
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

                log.Error("StaffTrainingAccess not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetStaffTrainingAccess() As System.Data.DataSet

        Return GetStaffTrainingAccess(mStaffTrainingAccessID)

    End Function

    Public Overridable Function GetStaffTrainingAccess(ByVal StaffTrainingAccessID As Long) As DataSet

        Dim sql As String

        If StaffTrainingAccessID > 0 Then
            sql = "SELECT * FROM tblStaffTrainingAccess WHERE StaffTrainingAccessID = " & StaffTrainingAccessID
        Else
            sql = "SELECT * FROM tblStaffTrainingAccess WHERE StaffTrainingAccessID = " & mStaffTrainingAccessID
        End If

        Return GetStaffTrainingAccess(sql)

    End Function

    Protected Overridable Function GetStaffTrainingAccess(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mStaffTrainingAccessID = Catchnull(.Item("StaffTrainingAccessID"), 0)
            mStaffID = Catchnull(.Item("StaffID"), 0)
            mTrainingID = Catchnull(.Item("TrainingID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@StaffTrainingAccessID", DbType.Int32, mStaffTrainingAccessID)
        db.AddInParameter(cmd, "@StaffID", DbType.Int32, mStaffID)
        db.AddInParameter(cmd, "@TrainingID", DbType.Int32, mTrainingID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_StaffTrainingAccess")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mStaffTrainingAccessID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

    Public Function SaveDetail(ByVal TrainingID As Long, ByVal StaffID As Long) As Boolean

        Dim InsertSQL As String = "INSERT INTO tblStaffTrainingAccess (TrainingID, StaffID) VALUES (" & TrainingID & ", " & StaffID & ")"

        db.ExecuteNonQuery(CommandType.Text, InsertSQL)

        Return True

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblStaffTrainingAccess SET Deleted = 1 WHERE StaffTrainingAccessID = " & mStaffTrainingAccessID) 
        Return Delete("DELETE FROM tblStaffTrainingAccess WHERE StaffTrainingAccessID = " & mStaffTrainingAccessID)

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