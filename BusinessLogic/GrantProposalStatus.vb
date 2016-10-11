Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GrantProposalStatus

#region "Variables"

    Protected mGrantProposalStatusID As long
    Protected mGrantProposalID As long
    Protected mStatusID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mStatusDate As string
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

    Public  Property GrantProposalStatusID() As long
        Get
		return mGrantProposalStatusID
        End Get
        Set(ByVal value As long)
		mGrantProposalStatusID = value
        End Set
    End Property

    Public  Property GrantProposalID() As long
        Get
		return mGrantProposalID
        End Get
        Set(ByVal value As long)
		mGrantProposalID = value
        End Set
    End Property

    Public  Property StatusID() As long
        Get
		return mStatusID
        End Get
        Set(ByVal value As long)
		mStatusID = value
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

    Public  Property StatusDate() As string
        Get
		return mStatusDate
        End Get
        Set(ByVal value As string)
		mStatusDate = value
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

    GrantProposalStatusID = 0
    mGrantProposalID = 0
    mStatusID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mStatusDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGrantProposalStatusID) 

    End Function 

    Public Overridable Function Retrieve(ByVal GrantProposalStatusID As Long) As Boolean 

        Dim sql As String 

        If GrantProposalStatusID > 0 Then 
            sql = "SELECT * FROM tblGrantProposalStatus WHERE GrantProposalStatusID = " & GrantProposalStatusID
        Else 
            sql = "SELECT * FROM tblGrantProposalStatus WHERE GrantProposalStatusID = " & mGrantProposalStatusID
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

                log.Error("GrantProposalStatus not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGrantProposalStatus() As System.Data.DataSet

        Return GetGrantProposalStatus(mGrantProposalStatusID)

    End Function

    Public Overridable Function GetGrantProposalStatus(ByVal GrantProposalID As Long) As DataSet

        Dim sql As String

        If GrantProposalID > 0 Then
            sql = "SELECT *, S.Description as Status FROM tblGrantProposalStatus G inner join luGrantProposalStatus S on G.StatusID = S.StatusID "
            sql &= "WHERE GrantProposalID = " & GrantProposalID
        Else
            sql = "SELECT *, S.Description as Status FROM tblGrantProposalStatus G inner join luGrantProposalStatus S on G.StatusID = S.StatusID "
            sql &= "WHERE GrantProposalID = " & mGrantProposalID
        End If

        Return GetGrantProposalStatus(sql)

    End Function

    Public Overridable Function CheckGrantStatus(ByVal GrantProposalID As Long) As Boolean

        Dim sql As String

        sql = "SELECT StatusID FROM tblGrantProposalStatus "
        sql &= "WHERE GrantProposalID = " & GrantProposalID & " AND StatusID = " & mStatusID

        Return GetGrantProposalStatus(sql).Tables(0).Rows.Count > 0

    End Function
    Protected Overridable Function GetGrantProposalStatus(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGrantProposalStatusID = Catchnull(.Item("GrantProposalStatusID"), 0)
            mGrantProposalID = Catchnull(.Item("GrantProposalID"), 0)
            mStatusID = Catchnull(.Item("StatusID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mStatusDate = Catchnull(.Item("StatusDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GrantProposalStatusID", DbType.Int32, mGrantProposalStatusID)
        db.AddInParameter(cmd, "@GrantProposalID", DbType.Int32, mGrantProposalID)
        db.AddInParameter(cmd, "@StatusID", DbType.Int32, mStatusID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@StatusDate", DbType.String, mStatusDate)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GrantProposalStatus")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGrantProposalStatusID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGrantProposalStatus SET Deleted = 1 WHERE GrantProposalStatusID = " & mGrantProposalStatusID) 
        Return Delete("DELETE FROM tblGrantProposalStatus WHERE GrantProposalStatusID = " & mGrantProposalStatusID)

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