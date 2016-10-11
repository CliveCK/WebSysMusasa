Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GrantProposal

#region "Variables"

    Protected mGrantProposalID As long
    Protected mActionID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mDonorName As string
    Protected mProposalTitle As string
    Protected mProposedProjectName As string
    Protected mComments As string

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

    Public  Property GrantProposalID() As long
        Get
		return mGrantProposalID
        End Get
        Set(ByVal value As long)
		mGrantProposalID = value
        End Set
    End Property

    Public  Property ActionID() As long
        Get
		return mActionID
        End Get
        Set(ByVal value As long)
		mActionID = value
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

    Public  Property DonorName() As string
        Get
		return mDonorName
        End Get
        Set(ByVal value As string)
		mDonorName = value
        End Set
    End Property

    Public  Property ProposalTitle() As string
        Get
		return mProposalTitle
        End Get
        Set(ByVal value As string)
		mProposalTitle = value
        End Set
    End Property

    Public  Property ProposedProjectName() As string
        Get
		return mProposedProjectName
        End Get
        Set(ByVal value As string)
		mProposedProjectName = value
        End Set
    End Property

    Public  Property Comments() As string
        Get
		return mComments
        End Get
        Set(ByVal value As string)
		mComments = value
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

    GrantProposalID = 0
    mActionID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mDonorName = ""
    mProposalTitle = ""
    mProposedProjectName = ""
    mComments = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGrantProposalID) 

    End Function

    Public Overridable Function Retrieve(ByVal GrantProposalID As Long) As Boolean

        Dim sql As String

        If GrantProposalID > 0 Then
            sql = "SELECT * FROM tblGrantProposals WHERE GrantProposalID = " & GrantProposalID
        Else
            sql = "SELECT * FROM tblGrantProposals WHERE GrantProposalID = " & mGrantProposalID
        End If

        Return Retrieve(sql)

    End Function

    Public Overridable Function RetrieveAll() As DataSet

        Dim sql As String

        sql = "Select G.*, GS.StatusDate as DateSubmitted, S.Description As Status from tblGrantProposals G left outer join "
        sql &= " tblGrantProposalStatus GS On G.GrantProposalID = GS.GrantProposalID "
        sql &= " Left outer join luGrantProposalStatus S On S.StatusID = GS.StatusID"

        Return GetGrantProposal(sql)

    End Function

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean 

        Try 

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql) 

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then 

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0)) 

                dsRetrieve = Nothing 
                Return True 

            Else

                log.Error("GrantProposal not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGrantProposal() As System.Data.DataSet

        Return GetGrantProposal(mGrantProposalID)

    End Function

    Public Overridable Function GetGrantProposal(ByVal GrantProposalID As Long) As DataSet

        Dim sql As String

        If GrantProposalID > 0 Then
            sql = "SELECT * FROM tblGrantProposals WHERE GrantProposalID = " & GrantProposalID
        Else
            sql = "SELECT * FROM tblGrantProposals WHERE GrantProposalID = " & mGrantProposalID
        End If

        Return GetGrantProposal(sql)

    End Function

    Protected Overridable Function GetGrantProposal(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGrantProposalID = Catchnull(.Item("GrantProposalID"), 0)
            mActionID = Catchnull(.Item("ActionID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mDonorName = Catchnull(.Item("DonorName"), "")
            mProposalTitle = Catchnull(.Item("ProposalTitle"), "")
            mProposedProjectName = Catchnull(.Item("ProposedProjectName"), "")
            mComments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GrantProposalID", DbType.Int32, mGrantProposalID)
        db.AddInParameter(cmd, "@ActionID", DbType.Int32, mActionID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DonorName", DbType.String, mDonorName)
        db.AddInParameter(cmd, "@ProposalTitle", DbType.String, mProposalTitle)
        db.AddInParameter(cmd, "@ProposedProjectName", DbType.String, mProposedProjectName)
        db.AddInParameter(cmd, "@Comments", DbType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GrantProposal")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGrantProposalID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGrantProposals SET Deleted = 1 WHERE GrantProposalID = " & mGrantProposalID) 
        Return Delete("DELETE FROM tblGrantProposals WHERE GrantProposalID = " & mGrantProposalID)

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