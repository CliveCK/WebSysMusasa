Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ProjectMeetingAttendants

#region "Variables"

    Protected mProjectMeetingAttendantID As long
    Protected mProjectMeetingID As long
    Protected mStaffID As long
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

    Public  Property ProjectMeetingAttendantID() As long
        Get
		return mProjectMeetingAttendantID
        End Get
        Set(ByVal value As long)
		mProjectMeetingAttendantID = value
        End Set
    End Property

    Public  Property ProjectMeetingID() As long
        Get
		return mProjectMeetingID
        End Get
        Set(ByVal value As long)
		mProjectMeetingID = value
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

    ProjectMeetingAttendantID = 0
    mProjectMeetingID = 0
    mStaffID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mProjectMeetingAttendantID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ProjectMeetingAttendantID As Long) As Boolean 

        Dim sql As String 

        If ProjectMeetingAttendantID > 0 Then 
            sql = "SELECT * FROM tblProjectMeetingAttendants WHERE ProjectMeetingAttendantID = " & ProjectMeetingAttendantID
        Else 
            sql = "SELECT * FROM tblProjectMeetingAttendants WHERE ProjectMeetingAttendantID = " & mProjectMeetingAttendantID
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

                log.Error("ProjectMeetingAttendants not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetProjectMeetingAttendants() As System.Data.DataSet

        Return GetProjectMeetingAttendants(mProjectMeetingAttendantID)

    End Function

    Public Overridable Function GetProjectMeetingAttendants(ByVal ProjectMeetingAttendantID As Long) As DataSet

        Dim sql As String

        If ProjectMeetingAttendantID > 0 Then
            sql = "SELECT * FROM tblProjectMeetingAttendants WHERE ProjectMeetingAttendantID = " & ProjectMeetingAttendantID
        Else
            sql = "SELECT * FROM tblProjectMeetingAttendants WHERE ProjectMeetingAttendantID = " & mProjectMeetingAttendantID
        End If

        Return GetProjectMeetingAttendants(sql)

    End Function

    Protected Overridable Function GetProjectMeetingAttendants(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mProjectMeetingAttendantID = Catchnull(.Item("ProjectMeetingAttendantID"), 0)
            mProjectMeetingID = Catchnull(.Item("ProjectMeetingID"), 0)
            mStaffID = Catchnull(.Item("StaffID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ProjectMeetingAttendantID", DBType.Int32, mProjectMeetingAttendantID)
        db.AddInParameter(cmd, "@ProjectMeetingID", DBType.Int32, mProjectMeetingID)
        db.AddInParameter(cmd, "@StaffID", DBType.Int32, mStaffID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ProjectMeetingAttendants")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mProjectMeetingAttendantID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProjectMeetingAttendants SET Deleted = 1 WHERE ProjectMeetingAttendantID = " & mProjectMeetingAttendantID) 
        Return Delete("DELETE FROM tblProjectMeetingAttendants WHERE ProjectMeetingAttendantID = " & mProjectMeetingAttendantID)

    End Function

    Public Function DeleteEntries() As Boolean

        Return Delete("DELETE FROM tblProjectMeetingAttendants WHERE ProjectMeetingID = " & mProjectMeetingID & " AND StaffID = " & mStaffID)

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