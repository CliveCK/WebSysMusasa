Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class CBSMemberNeeds

#region "Variables"

    Protected mCBSMemberNeedID As Long
    Protected mNeedID As Long
    Protected mBeneficiaryID As Long
    Protected mAssistanceID As long
    Protected mReferredToID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
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

    Public  Property CBSMemberNeedID() As long
        Get
		return mCBSMemberNeedID
        End Get
        Set(ByVal value As long)
		mCBSMemberNeedID = value
        End Set
    End Property

    Public Property NeedID() As Long
        Get
            Return mNeedID
        End Get
        Set(ByVal value As Long)
            mNeedID = value
        End Set
    End Property

    Public Property BeneficiaryID() As Long
        Get
            Return mBeneficiaryID
        End Get
        Set(ByVal value As Long)
            mBeneficiaryID = value
        End Set
    End Property

    Public  Property AssistanceID() As long
        Get
		return mAssistanceID
        End Get
        Set(ByVal value As long)
		mAssistanceID = value
        End Set
    End Property

    Public  Property ReferredToID() As long
        Get
		return mReferredToID
        End Get
        Set(ByVal value As long)
		mReferredToID = value
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

    CBSMemberNeedID = 0
        mNeedID = 0
        mBeneficiaryID = 0
        mAssistanceID = 0
    mReferredToID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mComments = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCBSMemberNeedID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CBSMemberNeedID As Long) As Boolean 

        Dim sql As String 

        If CBSMemberNeedID > 0 Then 
            sql = "SELECT * FROM tblCBSMemberNeeds WHERE CBSMemberNeedID = " & CBSMemberNeedID
        Else 
            sql = "SELECT * FROM tblCBSMemberNeeds WHERE CBSMemberNeedID = " & mCBSMemberNeedID
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

                log.Error("CBSMemberNeeds not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCBSMemberNeeds() As System.Data.DataSet

        Return GetCBSMemberNeeds(mCBSMemberNeedID)

    End Function

    Public Overridable Function GetCBSMemberNeeds(ByVal CBSMemberNeedID As Long) As DataSet

        Dim sql As String

        If CBSMemberNeedID > 0 Then
            sql = "SELECT * FROM tblCBSMemberNeeds WHERE CBSMemberNeedID = " & CBSMemberNeedID
        Else
            sql = "SELECT * FROM tblCBSMemberNeeds WHERE CBSMemberNeedID = " & mCBSMemberNeedID
        End If

        Return GetCBSMemberNeeds(sql)

    End Function

    Protected Overridable Function GetCBSMemberNeeds(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mCBSMemberNeedID = Catchnull(.Item("CBSMemberNeedID"), 0)
            mNeedID = Catchnull(.Item("NeedID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mAssistanceID = Catchnull(.Item("AssistanceID"), 0)
            mReferredToID = Catchnull(.Item("ReferredToID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mComments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@CBSMemberNeedID", DbType.Int32, mCBSMemberNeedID)
        db.AddInParameter(cmd, "@NeedID", DbType.Int32, mNeedID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@AssistanceID", DbType.Int32, mAssistanceID)
        db.AddInParameter(cmd, "@ReferredToID", DbType.Int32, mReferredToID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Comments", DbType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_CBSMemberNeeds")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mCBSMemberNeedID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblCBSMemberNeeds SET Deleted = 1 WHERE CBSMemberNeedID = " & mCBSMemberNeedID) 
        Return Delete("DELETE FROM tblCBSMemberNeeds WHERE CBSMemberNeedID = " & mCBSMemberNeedID)

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