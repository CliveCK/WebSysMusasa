Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ShelterClientDetails

#region "Variables"

    Protected mShelterClientDetailID As long
    Protected mBeneficiaryID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mEmploymentStatus As string
    Protected mEmployerTelNo As string
    Protected mEmployerAddress As String
    Protected mReferredBy As String
    Protected mReferredTo As Long
    Protected mReferrerTelNo As string
    Protected mSheltedBefore As string
    Protected mInjuriesSustained As string
    Protected mAnySpecialMedicalNeeds As string
    Protected mMedicalNeeds As string
    Protected mCarePlan As string
    Protected mPresentingProblem As string
    Protected mSkillsToNature As string
    Protected mSkills As string
    Protected mName As string
    Protected mRelationship As string
    Protected mContactNo As String
    Protected mContactAddress As String
    Protected mArrivalTime As String
    Protected mTotalAdmitted As Long
    Protected mTestedForHIV As Boolean
    Protected mDiscloseStatus As Boolean
    Protected mHIVStatus As String
    Protected mOnART As String

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

    Public  Property ShelterClientDetailID() As long
        Get
		return mShelterClientDetailID
        End Get
        Set(ByVal value As long)
		mShelterClientDetailID = value
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

    Public Property CreatedBy() As Long
        Get
            Return mCreatedBy
        End Get
        Set(ByVal value As Long)
            mCreatedBy = value
        End Set
    End Property

    Public Property TotalAdmitted() As Long
        Get
            Return mTotalAdmitted
        End Get
        Set(ByVal value As Long)
            mTotalAdmitted = value
        End Set
    End Property

    Public Property TestedForHIV() As Boolean
        Get
            Return mTestedForHIV
        End Get
        Set(ByVal value As Boolean)
            mTestedForHIV = value
        End Set
    End Property

    Public Property DiscloseStatus() As Boolean
        Get
            Return mDiscloseStatus
        End Get
        Set(ByVal value As Boolean)
            mDiscloseStatus = value
        End Set
    End Property

    Public Property HIVStatus() As String
        Get
            Return mHIVStatus
        End Get
        Set(ByVal value As String)
            mHIVStatus = value
        End Set
    End Property

    Public Property OnART() As String
        Get
            Return mOnART
        End Get
        Set(ByVal value As String)
            mOnART = value
        End Set
    End Property

    Public Property ArrivalTime() As String
        Get
            Return mArrivalTime
        End Get
        Set(ByVal value As String)
            mArrivalTime = value
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

    Public  Property EmploymentStatus() As string
        Get
		return mEmploymentStatus
        End Get
        Set(ByVal value As string)
		mEmploymentStatus = value
        End Set
    End Property

    Public  Property EmployerTelNo() As string
        Get
		return mEmployerTelNo
        End Get
        Set(ByVal value As string)
		mEmployerTelNo = value
        End Set
    End Property

    Public  Property EmployerAddress() As string
        Get
		return mEmployerAddress
        End Get
        Set(ByVal value As string)
		mEmployerAddress = value
        End Set
    End Property

    Public Property ReferredBy() As String
        Get
            Return mReferredBy
        End Get
        Set(ByVal value As String)
            mReferredBy = value
        End Set
    End Property

    Public Property ReferredTo() As Long
        Get
            Return mReferredTo
        End Get
        Set(ByVal value As Long)
            mReferredTo = value
        End Set
    End Property

    Public  Property ReferrerTelNo() As string
        Get
		return mReferrerTelNo
        End Get
        Set(ByVal value As string)
		mReferrerTelNo = value
        End Set
    End Property

    Public  Property SheltedBefore() As string
        Get
		return mSheltedBefore
        End Get
        Set(ByVal value As string)
		mSheltedBefore = value
        End Set
    End Property

    Public  Property InjuriesSustained() As string
        Get
		return mInjuriesSustained
        End Get
        Set(ByVal value As string)
		mInjuriesSustained = value
        End Set
    End Property

    Public  Property AnySpecialMedicalNeeds() As string
        Get
		return mAnySpecialMedicalNeeds
        End Get
        Set(ByVal value As string)
		mAnySpecialMedicalNeeds = value
        End Set
    End Property

    Public  Property MedicalNeeds() As string
        Get
		return mMedicalNeeds
        End Get
        Set(ByVal value As string)
		mMedicalNeeds = value
        End Set
    End Property

    Public  Property CarePlan() As string
        Get
		return mCarePlan
        End Get
        Set(ByVal value As string)
		mCarePlan = value
        End Set
    End Property

    Public  Property PresentingProblem() As string
        Get
		return mPresentingProblem
        End Get
        Set(ByVal value As string)
		mPresentingProblem = value
        End Set
    End Property

    Public  Property SkillsToNature() As string
        Get
		return mSkillsToNature
        End Get
        Set(ByVal value As string)
		mSkillsToNature = value
        End Set
    End Property

    Public  Property Skills() As string
        Get
		return mSkills
        End Get
        Set(ByVal value As string)
		mSkills = value
        End Set
    End Property

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
        End Set
    End Property

    Public  Property Relationship() As string
        Get
		return mRelationship
        End Get
        Set(ByVal value As string)
		mRelationship = value
        End Set
    End Property

    Public  Property ContactNo() As string
        Get
		return mContactNo
        End Get
        Set(ByVal value As string)
		mContactNo = value
        End Set
    End Property

    Public  Property ContactAddress() As string
        Get
		return mContactAddress
        End Get
        Set(ByVal value As string)
		mContactAddress = value
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

    ShelterClientDetailID = 0
        mBeneficiaryID = 0
        mArrivalTime = ""
        mTotalAdmitted = 0
        mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mEmploymentStatus = ""
    mEmployerTelNo = ""
    mEmployerAddress = ""
        mReferredBy = ""
        mReferredTo = 0
        mReferrerTelNo = ""
    mSheltedBefore = ""
    mInjuriesSustained = ""
    mAnySpecialMedicalNeeds = ""
    mMedicalNeeds = ""
    mCarePlan = ""
    mPresentingProblem = ""
    mSkillsToNature = ""
    mSkills = ""
    mName = ""
    mRelationship = ""
    mContactNo = ""
        mContactAddress = ""
        mTestedForHIV = False
        mDiscloseStatus = False
        mHIVStatus = ""
        mOnART = ""

    End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mShelterClientDetailID) 

    End Function

    Public Overridable Function Retrieve(ByVal BeneficiaryID As Long) As Boolean

        Dim sql As String

        If BeneficiaryID > 0 Then
            sql = "SELECT * FROM tblShelterClientDetails WHERE BeneficiaryID = " & BeneficiaryID
        Else
            sql = "SELECT * FROM tblShelterClientDetails WHERE BeneficiaryID = " & mBeneficiaryID
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

                log.Error("ShelterClientDetails not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetShelterClientDetails() As System.Data.DataSet

        Return GetShelterClientDetails(mShelterClientDetailID)

    End Function

    Public Function GetBenDetails(ByVal UseCriteria As Boolean, ByVal StaffID As Long) As DataSet

        Dim Criteria As String = ""

        If UseCriteria Then

            Criteria = " AND ShelterID = " & StaffID

        End If

        Dim sql As String = "Select DISTINCT B.*, D.Name As District, W.Name As Ward, S.StaffFullName AS AssignedBy from tblBeneficiaries B "
        sql &= "Left outer join tblShelterClientDetails L on L.BeneficiaryID = B.BeneficiaryID "
        sql &= "Left outer join tblAddresses A on A.OwnerID = B.BeneficiaryID  "
        sql &= "left outer join tblDistricts D on D.DistrictID = A.DistrictID "
        sql &= "left outer join tblClientDetails C on C.BeneficiaryID = B.BeneficiaryID AND ReferredToShelter = 1 " & Criteria & " "
        sql &= "Left outer join tblStaffMembers S on S.StaffID = C.ReferredToShelterByID "
        sql &= "Left outer join tblWards W on W.WardID = A.WardID  "
        sql &= "where B.BeneficiaryID in (Select BeneficiaryID from tblShelterClientDetails) "
        sql &= " Or B.BeneficiaryID In (Select BeneficiaryID from tblClientDetails where ReferredToShelter = 1 " & Criteria & ")"

        Return GetShelterClientDetails(sql)

    End Function

    Public Overridable Function GetShelterClientDetails(ByVal ShelterClientDetailID As Long) As DataSet

        Dim sql As String

        If ShelterClientDetailID > 0 Then
            sql = "Select * FROM tblShelterClientDetails WHERE ShelterClientDetailID = " & ShelterClientDetailID
        Else
            sql = "Select * FROM tblShelterClientDetails WHERE ShelterClientDetailID = " & mShelterClientDetailID
        End If

        Return GetShelterClientDetails(sql)

    End Function

    Protected Overridable Function GetShelterClientDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mShelterClientDetailID = Catchnull(.Item("ShelterClientDetailID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mEmploymentStatus = Catchnull(.Item("EmploymentStatus"), "")
            mEmployerTelNo = Catchnull(.Item("EmployerTelNo"), "")
            mEmployerAddress = Catchnull(.Item("EmployerAddress"), "")
            mReferredBy = Catchnull(.Item("ReferredBy"), "")
            mReferredTo = Catchnull(.Item("ReferredTo"), "")
            mReferrerTelNo = Catchnull(.Item("ReferrerTelNo"), "")
            mSheltedBefore = Catchnull(.Item("SheltedBefore"), "")
            mInjuriesSustained = Catchnull(.Item("InjuriesSustained"), "")
            mAnySpecialMedicalNeeds = Catchnull(.Item("AnySpecialMedicalNeeds"), "")
            mMedicalNeeds = Catchnull(.Item("MedicalNeeds"), "")
            mCarePlan = Catchnull(.Item("CarePlan"), "")
            mPresentingProblem = Catchnull(.Item("PresentingProblem"), "")
            mSkillsToNature = Catchnull(.Item("SkillsToNature"), "")
            mSkills = Catchnull(.Item("Skills"), "")
            mName = Catchnull(.Item("Name"), "")
            mRelationship = Catchnull(.Item("Relationship"), "")
            mContactNo = Catchnull(.Item("ContactNo"), "")
            mContactAddress = Catchnull(.Item("ContactAddress"), "")
            mArrivalTime = Catchnull(.Item("ArrivalTime"), "")
            mTotalAdmitted = Catchnull(.Item("TotalAdmitted"), 0)
            mTestedForHIV = Catchnull(.Item("TestedForHIV"), 0)
            mDiscloseStatus = Catchnull(.Item("DiscloseStatus"), 0)
            mHIVStatus = Catchnull(.Item("HIVStatus"), "")
            mOnART = Catchnull(.Item("OnART"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ShelterClientDetailID", DbType.Int32, mShelterClientDetailID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@EmploymentStatus", DbType.String, mEmploymentStatus)
        db.AddInParameter(cmd, "@EmployerTelNo", DbType.String, mEmployerTelNo)
        db.AddInParameter(cmd, "@EmployerAddress", DbType.String, mEmployerAddress)
        db.AddInParameter(cmd, "@ReferredBy", DbType.String, mReferredBy)
        db.AddInParameter(cmd, "@ReferredTo", DbType.String, mReferredTo)
        db.AddInParameter(cmd, "@ReferrerTelNo", DbType.String, mReferrerTelNo)
        db.AddInParameter(cmd, "@SheltedBefore", DbType.String, mSheltedBefore)
        db.AddInParameter(cmd, "@InjuriesSustained", DbType.String, mInjuriesSustained)
        db.AddInParameter(cmd, "@AnySpecialMedicalNeeds", DbType.String, mAnySpecialMedicalNeeds)
        db.AddInParameter(cmd, "@MedicalNeeds", DbType.String, mMedicalNeeds)
        db.AddInParameter(cmd, "@CarePlan", DbType.String, mCarePlan)
        db.AddInParameter(cmd, "@PresentingProblem", DbType.String, mPresentingProblem)
        db.AddInParameter(cmd, "@SkillsToNature", DbType.String, mSkillsToNature)
        db.AddInParameter(cmd, "@Skills", DbType.String, mSkills)
        db.AddInParameter(cmd, "@Name", DbType.String, mName)
        db.AddInParameter(cmd, "@Relationship", DbType.String, mRelationship)
        db.AddInParameter(cmd, "@ContactNo", DbType.String, mContactNo)
        db.AddInParameter(cmd, "@ContactAddress", DbType.String, mContactAddress)
        db.AddInParameter(cmd, "@ArrivalTime", DbType.String, mArrivalTime)
        db.AddInParameter(cmd, "@TotalAdmitted", DbType.Int32, mTotalAdmitted)
        db.AddInParameter(cmd, "@TestedForHIV", DbType.Boolean, mTestedForHIV)
        db.AddInParameter(cmd, "@DiscloseStatus", DbType.Boolean, mDiscloseStatus)
        db.AddInParameter(cmd, "@HIVStatus", DbType.String, mHIVStatus)
        db.AddInParameter(cmd, "@OnART", DbType.String, mOnART)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ShelterClientDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mShelterClientDetailID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblShelterClientDetails Set Deleted = 1 WHERE ShelterClientDetailID = " & mShelterClientDetailID) 
        Return Delete("DELETE FROM tblShelterClientDetails WHERE ShelterClientDetailID = " & mShelterClientDetailID)

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