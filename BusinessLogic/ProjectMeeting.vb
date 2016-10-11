Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ProjectMeeting

#region "Variables"

    Protected mProjectMeetingID As long
    Protected mProjectID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateOfMeeting As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mPlace As string
    Protected mPurpose As string

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

    Public  Property ProjectMeetingID() As long
        Get
		return mProjectMeetingID
        End Get
        Set(ByVal value As long)
		mProjectMeetingID = value
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

    Public  Property DateOfMeeting() As string
        Get
		return mDateOfMeeting
        End Get
        Set(ByVal value As string)
		mDateOfMeeting = value
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

    Public  Property Place() As string
        Get
		return mPlace
        End Get
        Set(ByVal value As string)
		mPlace = value
        End Set
    End Property

    Public  Property Purpose() As string
        Get
		return mPurpose
        End Get
        Set(ByVal value As string)
		mPurpose = value
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

    ProjectMeetingID = 0
    mProjectID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateOfMeeting = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mPlace = ""
    mPurpose = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mProjectMeetingID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ProjectMeetingID As Long) As Boolean 

        Dim sql As String 

        If ProjectMeetingID > 0 Then 
            sql = "SELECT * FROM tblProjectMeetings WHERE ProjectMeetingID = " & ProjectMeetingID
        Else 
            sql = "SELECT * FROM tblProjectMeetings WHERE ProjectMeetingID = " & mProjectMeetingID
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

                log.Error("ProjectMeeting not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetProjectMeeting() As System.Data.DataSet

        Return GetProjectMeeting(mProjectMeetingID)

    End Function

    Public Overridable Function GetProjectMeeting(ByVal ProjectMeetingID As Long) As DataSet

        Dim sql As String

        If ProjectMeetingID > 0 Then
            sql = "SELECT * FROM tblProjectMeetings WHERE ProjectMeetingID = " & ProjectMeetingID
        Else
            sql = "SELECT * FROM tblProjectMeetings WHERE ProjectMeetingID = " & mProjectMeetingID
        End If

        Return GetProjectMeeting(sql)

    End Function

    Public Overridable Function GetProjectMeeting(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mProjectMeetingID = Catchnull(.Item("ProjectMeetingID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateOfMeeting = Catchnull(.Item("DateOfMeeting"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mPlace = Catchnull(.Item("Place"), "")
            mPurpose = Catchnull(.Item("Purpose"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ProjectMeetingID", DBType.Int32, mProjectMeetingID)
        db.AddInParameter(cmd, "@ProjectID", DBType.Int32, mProjectID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateOfMeeting", DBType.String, mDateOfMeeting)
        db.AddInParameter(cmd, "@Place", DBType.String, mPlace)
        db.AddInParameter(cmd, "@Purpose", DBType.String, mPurpose)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ProjectMeeting")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mProjectMeetingID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblProjectMeetings SET Deleted = 1 WHERE ProjectMeetingID = " & mProjectMeetingID) 
        Return Delete("DELETE FROM tblProjectMeetings WHERE ProjectMeetingID = " & mProjectMeetingID)

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