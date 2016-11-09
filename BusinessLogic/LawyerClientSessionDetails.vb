Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class LawyerClientSessionDetails

#region "Variables"

    Protected mLawyerClientSessionDetailID As long
    Protected mBeneficiaryID As long
    Protected mActionTobeTakenID As long
    Protected mReferralID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mSessionDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mCaseNotes As string
    Protected mReferralOther As string
    Protected mOtherProblem As string

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

    Public  Property LawyerClientSessionDetailID() As long
        Get
		return mLawyerClientSessionDetailID
        End Get
        Set(ByVal value As long)
		mLawyerClientSessionDetailID = value
        End Set
    End Property

    Public  Property BeneficiaryID() As long
        Get
		return mBeneficiaryID
        End Get
        Set(ByVal value As long)
		mBeneficiaryID = value
        End Set
    End Property

    Public  Property ActionTobeTakenID() As long
        Get
		return mActionTobeTakenID
        End Get
        Set(ByVal value As long)
		mActionTobeTakenID = value
        End Set
    End Property

    Public  Property ReferralID() As long
        Get
		return mReferralID
        End Get
        Set(ByVal value As long)
		mReferralID = value
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

    Public  Property SessionDate() As string
        Get
		return mSessionDate
        End Get
        Set(ByVal value As string)
		mSessionDate = value
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

    Public  Property CaseNotes() As string
        Get
		return mCaseNotes
        End Get
        Set(ByVal value As string)
		mCaseNotes = value
        End Set
    End Property

    Public  Property ReferralOther() As string
        Get
		return mReferralOther
        End Get
        Set(ByVal value As string)
		mReferralOther = value
        End Set
    End Property

    Public  Property OtherProblem() As string
        Get
		return mOtherProblem
        End Get
        Set(ByVal value As string)
		mOtherProblem = value
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

    LawyerClientSessionDetailID = 0
    mBeneficiaryID = 0
    mActionTobeTakenID = 0
    mReferralID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mSessionDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mCaseNotes = ""
    mReferralOther = ""
    mOtherProblem = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mLawyerClientSessionDetailID) 

    End Function

    Public Overridable Function Retrieve(ByVal BeneficiaryID As Long) As Boolean

        Dim sql As String

        If BeneficiaryID > 0 Then
            sql = "SELECT * FROM tblLaywerClientSessionDetails WHERE BeneficiaryID = " & BeneficiaryID
        Else
            sql = "SELECT * FROM tblLaywerClientSessionDetails WHERE BeneficiaryID = " & mBeneficiaryID
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

                log.Error("LawyerClientSessionDetails not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetLawyerClientSessionDetails() As System.Data.DataSet

        Return GetLawyerClientSessionDetails(mLawyerClientSessionDetailID)

    End Function

    Public Function GetBenDetails(ByVal UseCriteria As Boolean, ByVal StaffID As Long) As DataSet

        Dim Criteria As String = ""

        If UseCriteria Then

            Criteria = " AND LawyerID = " & StaffID

        End If

        Dim sql As String = "Select DISTINCT B.*, D.Name As District, W.Name As Ward, S.StaffFullName AS AssignedBy from tblBeneficiaries B "
        sql &= "Left outer join tblLaywerClientSessionDetails L on L.BeneficiaryID = B.BeneficiaryID "
        sql &= "Left outer join tblAddresses A on A.OwnerID = B.BeneficiaryID  "
        sql &= "left outer join tblDistricts D on D.DistrictID = A.DistrictID "
        sql &= "Left outer join tblWards W on W.WardID = A.WardID  "
        sql &= "left outer join tblClientDetails C on C.BeneficiaryID = B.BeneficiaryID AND ReferredToLaywer = 1 " & Criteria & " "
        sql &= "Left outer join tblStaffMembers S on S.StaffID = C.ReferredToLawyerByID "
        sql &= "where B.BeneficiaryID in (Select BeneficiaryID from tblLaywerClientSessionDetails) "
        sql &= " Or B.BeneficiaryID in (Select BeneficiaryID from tblClientDetails where ReferredToLaywer = 1 " & Criteria & " )"

        Return GetLawyerClientSessionDetails(sql)

    End Function

    Public Overridable Function GetLawyerClientSessionDetails(ByVal LawyerClientSessionDetailID As Long) As DataSet

        Dim sql As String

        If LawyerClientSessionDetailID > 0 Then
            sql = "SELECT * FROM tblLaywerClientSessionDetails WHERE LawyerClientSessionDetailID = " & LawyerClientSessionDetailID
        Else
            sql = "SELECT * FROM tblLaywerClientSessionDetails WHERE LawyerClientSessionDetailID = " & mLawyerClientSessionDetailID
        End If

        Return GetLawyerClientSessionDetails(sql)

    End Function

    Protected Overridable Function GetLawyerClientSessionDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mLawyerClientSessionDetailID = Catchnull(.Item("LawyerClientSessionDetailID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mActionTobeTakenID = Catchnull(.Item("ActionTobeTakenID"), 0)
            mReferralID = Catchnull(.Item("ReferralID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mSessionDate = Catchnull(.Item("SessionDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mCaseNotes = Catchnull(.Item("CaseNotes"), "")
            mReferralOther = Catchnull(.Item("ReferralOther"), "")
            mOtherProblem = Catchnull(.Item("OtherProblem"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@LawyerClientSessionDetailID", DbType.Int32, mLawyerClientSessionDetailID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@ActionTobeTakenID", DbType.Int32, mActionTobeTakenID)
        db.AddInParameter(cmd, "@ReferralID", DbType.Int32, mReferralID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@SessionDate", DbType.String, mSessionDate)
        db.AddInParameter(cmd, "@CaseNotes", DbType.String, mCaseNotes)
        db.AddInParameter(cmd, "@ReferralOther", DbType.String, mReferralOther)
        db.AddInParameter(cmd, "@OtherProblem", DbType.String, mOtherProblem)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_LawyerClientSessionDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mLawyerClientSessionDetailID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblLaywerClientSessionDetails SET Deleted = 1 WHERE LawyerClientSessionDetailID = " & mLawyerClientSessionDetailID) 
        Return Delete("DELETE FROM tblLaywerClientSessionDetails WHERE LawyerClientSessionDetailID = " & mLawyerClientSessionDetailID)

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