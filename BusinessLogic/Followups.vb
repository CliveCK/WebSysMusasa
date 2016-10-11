Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Followups

#region "Variables"

    Protected mFollowupID As long
    Protected mPatientID As long
    Protected mFollowupTypeID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mFollowupDate As string
    Protected mFollowupTime As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mLengthOfVisit As string
    Protected mCasePriority As string

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

    Public  Property FollowupID() As long
        Get
		return mFollowupID
        End Get
        Set(ByVal value As long)
		mFollowupID = value
        End Set
    End Property

    Public  Property PatientID() As long
        Get
		return mPatientID
        End Get
        Set(ByVal value As long)
		mPatientID = value
        End Set
    End Property

    Public  Property FollowupTypeID() As long
        Get
		return mFollowupTypeID
        End Get
        Set(ByVal value As long)
		mFollowupTypeID = value
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

    Public  Property FollowupDate() As string
        Get
		return mFollowupDate
        End Get
        Set(ByVal value As string)
		mFollowupDate = value
        End Set
    End Property

    Public  Property FollowupTime() As string
        Get
		return mFollowupTime
        End Get
        Set(ByVal value As string)
		mFollowupTime = value
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

    Public  Property LengthOfVisit() As string
        Get
		return mLengthOfVisit
        End Get
        Set(ByVal value As string)
		mLengthOfVisit = value
        End Set
    End Property

    Public  Property CasePriority() As string
        Get
		return mCasePriority
        End Get
        Set(ByVal value As string)
		mCasePriority = value
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

    FollowupID = 0
    mPatientID = 0
    mFollowupTypeID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mFollowupDate = ""
    mFollowupTime = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mLengthOfVisit = ""
    mCasePriority = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mFollowupID) 

    End Function 

    Public Overridable Function Retrieve(ByVal FollowupID As Long) As Boolean 

        Dim sql As String 

        If FollowupID > 0 Then 
            sql = "SELECT * FROM tblFollowups WHERE FollowupID = " & FollowupID
        Else 
            sql = "SELECT * FROM tblFollowups WHERE FollowupID = " & mFollowupID
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

                log.error("Followups not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetFollowups() As System.Data.DataSet

        Return GetFollowups(mFollowupID)

    End Function

    Public Overridable Function GetFollowups(ByVal FollowupID As Long) As DataSet

        Dim sql As String

        If FollowupID > 0 Then
            sql = "SELECT * FROM tblFollowups WHERE FollowupID = " & FollowupID
        Else
            sql = "SELECT * FROM tblFollowups WHERE FollowupID = " & mFollowupID
        End If

        Return GetFollowups(sql)

    End Function

    Public Function GetFollowUpByPatient(ByVal PatientID As Long) As DataSet

        Dim sql As String

        sql = "select F.*, FT.Description As FollowupType From tblFollowups F inner join luFollowupTypes FT on F.FollowupTypeID = FT.FollowupTypeID "
        sql &= " WHERE PatientID = " & PatientID

        Return GetFollowups(sql)

    End Function

    Protected Overridable Function GetFollowups(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mFollowupID = Catchnull(.Item("FollowupID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mFollowupTypeID = Catchnull(.Item("FollowupTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mFollowupDate = Catchnull(.Item("FollowupDate"), "")
            mFollowupTime = Catchnull(.Item("FollowupTime"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mLengthOfVisit = Catchnull(.Item("LengthOfVisit"), "")
            mCasePriority = Catchnull(.Item("CasePriority"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@FollowupID", DBType.Int32, mFollowupID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@FollowupTypeID", DBType.Int32, mFollowupTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@FollowupDate", DBType.String, mFollowupDate)
        db.AddInParameter(cmd, "@FollowupTime", DBType.String, mFollowupTime)
        db.AddInParameter(cmd, "@LengthOfVisit", DBType.String, mLengthOfVisit)
        db.AddInParameter(cmd, "@CasePriority", DBType.String, mCasePriority)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Followups")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mFollowupID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            log.error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblFollowups SET Deleted = 1 WHERE FollowupID = " & mFollowupID) 
        Return Delete("DELETE FROM tblFollowups WHERE FollowupID = " & mFollowupID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

#End Region

#end region

End Class