Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ClientDetails 

#region "Variables"

    Protected mClientDetailID As long
    Protected mBeneficiaryID As long
    Protected mNoOfChildren As Long
    Protected mEmploymentStatusID As String
    Protected mAccompanyingChildren As long
    Protected mCounsellorID As long
    Protected mLawyerID As long
    Protected mShelterID As long
    Protected mCapturedFromID As Long
    Protected mReferredToCounsellorByID As Long
    Protected mReferredToLawyerByID As Long
    Protected mReferredToShelterByID As Long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mReferredToCounsellor As boolean
    Protected mReferredToLaywer As boolean
    Protected mReferredToShelter As boolean
    Protected mNextOfKin As string
    Protected mContactNo As string
    Protected mNatureOfRelationship As string
    Protected mAccompanyingAdult1 As string
    Protected mAccompanyingAdult2 As string
    Protected mReferredBy As string

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

    Public  Property ClientDetailID() As long
        Get
		return mClientDetailID
        End Get
        Set(ByVal value As long)
		mClientDetailID = value
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

    Public  Property NoOfChildren() As long
        Get
		return mNoOfChildren
        End Get
        Set(ByVal value As long)
		mNoOfChildren = value
        End Set
    End Property

    Public Property EmploymentStatusID() As String
        Get
            Return mEmploymentStatusID
        End Get
        Set(ByVal value As String)
            mEmploymentStatusID = value
        End Set
    End Property

    Public  Property AccompanyingChildren() As long
        Get
		return mAccompanyingChildren
        End Get
        Set(ByVal value As long)
		mAccompanyingChildren = value
        End Set
    End Property

    Public  Property CounsellorID() As long
        Get
		return mCounsellorID
        End Get
        Set(ByVal value As long)
		mCounsellorID = value
        End Set
    End Property

    Public  Property LawyerID() As long
        Get
		return mLawyerID
        End Get
        Set(ByVal value As long)
		mLawyerID = value
        End Set
    End Property

    Public  Property ShelterID() As long
        Get
		return mShelterID
        End Get
        Set(ByVal value As long)
		mShelterID = value
        End Set
    End Property

    Public  Property CapturedFromID() As long
        Get
		return mCapturedFromID
        End Get
        Set(ByVal value As long)
		mCapturedFromID = value
        End Set
    End Property

    Public Property ReferredToCounsellorByID() As Long
        Get
            Return mReferredToCounsellorByID
        End Get
        Set(ByVal value As Long)
            mReferredToCounsellorByID = value
        End Set
    End Property

    Public Property ReferredToLawyerByID() As Long
        Get
            Return mReferredToLawyerByID
        End Get
        Set(ByVal value As Long)
            mReferredToLawyerByID = value
        End Set
    End Property

    Public Property ReferredToShelterByID() As Long
        Get
            Return mReferredToShelterByID
        End Get
        Set(ByVal value As Long)
            mReferredToShelterByID = value
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

    Public  Property ReferredToCounsellor() As boolean
        Get
		return mReferredToCounsellor
        End Get
        Set(ByVal value As boolean)
		mReferredToCounsellor = value
        End Set
    End Property

    Public  Property ReferredToLaywer() As boolean
        Get
		return mReferredToLaywer
        End Get
        Set(ByVal value As boolean)
		mReferredToLaywer = value
        End Set
    End Property

    Public  Property ReferredToShelter() As boolean
        Get
		return mReferredToShelter
        End Get
        Set(ByVal value As boolean)
		mReferredToShelter = value
        End Set
    End Property

    Public  Property NextOfKin() As string
        Get
		return mNextOfKin
        End Get
        Set(ByVal value As string)
		mNextOfKin = value
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

    Public  Property NatureOfRelationship() As string
        Get
		return mNatureOfRelationship
        End Get
        Set(ByVal value As string)
		mNatureOfRelationship = value
        End Set
    End Property

    Public  Property AccompanyingAdult1() As string
        Get
		return mAccompanyingAdult1
        End Get
        Set(ByVal value As string)
		mAccompanyingAdult1 = value
        End Set
    End Property

    Public  Property AccompanyingAdult2() As string
        Get
		return mAccompanyingAdult2
        End Get
        Set(ByVal value As string)
		mAccompanyingAdult2 = value
        End Set
    End Property

    Public  Property ReferredBy() As string
        Get
		return mReferredBy
        End Get
        Set(ByVal value As string)
		mReferredBy = value
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

    ClientDetailID = 0
    mBeneficiaryID = 0
    mNoOfChildren = 0
    mEmploymentStatusID = 0
    mAccompanyingChildren = 0
    mCounsellorID = 0
    mLawyerID = 0
    mShelterID = 0
    mCapturedFromID = 0
        mReferredToCounsellorByID = 0
        mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mReferredToCounsellor = FALSE
    mReferredToLaywer = FALSE
    mReferredToShelter = FALSE
    mNextOfKin = ""
    mContactNo = ""
    mNatureOfRelationship = ""
    mAccompanyingAdult1 = ""
    mAccompanyingAdult2 = ""
    mReferredBy = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mClientDetailID) 

    End Function

    Public Overridable Function Retrieve(ByVal BeneficiaryID As Long) As Boolean

        Dim sql As String

        If BeneficiaryID > 0 Then
            sql = "SELECT * FROM tblClientDetails WHERE BeneficiaryID = " & BeneficiaryID
        Else
            sql = "SELECT * FROM tblClientDetails WHERE BeneficiaryID = " & mBeneficiaryID
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

                log.Error("ClientDetails not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetClientDetails() As System.Data.DataSet

        Return GetClientDetails(mClientDetailID)

    End Function

    Public Overridable Function GetClientDetails(ByVal ClientDetailID As Long) As DataSet

        Dim sql As String

        If ClientDetailID > 0 Then
            sql = "SELECT * FROM tblClientDetails WHERE ClientDetailID = " & ClientDetailID
        Else
            sql = "SELECT * FROM tblClientDetails WHERE ClientDetailID = " & mClientDetailID
        End If

        Return GetClientDetails(sql)

    End Function

    Public Function GetallDetails() As DataSet

        Dim sql As String = "Select DISTINCT B.*, D.Name As District, W.Name As Ward, P.Name as Province from tblBeneficiaries B "
        sql &= "inner join tblClientDetails C on C.BeneficiaryID = B.BeneficiaryID "
        sql &= "Left outer join tblAddresses A on A.OwnerID = B.BeneficiaryID  "
        sql &= "left outer join tblDistricts D on D.DistrictID = A.DistrictID "
        sql &= "left outer join tblProvinces P on P.ProvinceID = D.ProvinceID "
        sql &= "Left outer join tblWards W on W.WardID = A.WardID  "

        Return GetClientDetails(sql)

    End Function

    Protected Overridable Function GetClientDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mClientDetailID = Catchnull(.Item("ClientDetailID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mNoOfChildren = Catchnull(.Item("NoOfChildren"), 0)
            mEmploymentStatusID = Catchnull(.Item("EmploymentStatusID"), "")
            mAccompanyingChildren = Catchnull(.Item("AccompanyingChildren"), 0)
            mCounsellorID = Catchnull(.Item("CounsellorID"), 0)
            mLawyerID = Catchnull(.Item("LawyerID"), 0)
            mShelterID = Catchnull(.Item("ShelterID"), 0)
            mCapturedFromID = Catchnull(.Item("CapturedFromID"), 0)
            mReferredToCounsellorByID = Catchnull(.Item("ReferredToCounsellorByID"), 0)
            mReferredToLawyerByID = Catchnull(.Item("ReferredToLawyerByID"), 0)
            mReferredToShelterByID = Catchnull(.Item("ReferredToShelterByID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mReferredToCounsellor = Catchnull(.Item("ReferredToCounsellor"), False)
            mReferredToLaywer = Catchnull(.Item("ReferredToLaywer"), False)
            mReferredToShelter = Catchnull(.Item("ReferredToShelter"), False)
            mNextOfKin = Catchnull(.Item("NextOfKin"), "")
            mContactNo = Catchnull(.Item("ContactNo"), "")
            mNatureOfRelationship = Catchnull(.Item("NatureOfRelationship"), "")
            mAccompanyingAdult1 = Catchnull(.Item("AccompanyingAdult1"), "")
            mAccompanyingAdult2 = Catchnull(.Item("AccompanyingAdult2"), "")
            mReferredBy = Catchnull(.Item("ReferredBy"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ClientDetailID", DbType.Int32, mClientDetailID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@NoOfChildren", DbType.Int32, mNoOfChildren)
        db.AddInParameter(cmd, "@EmploymentStatusID", DbType.String, mEmploymentStatusID)
        db.AddInParameter(cmd, "@AccompanyingChildren", DbType.Int32, mAccompanyingChildren)
        db.AddInParameter(cmd, "@CounsellorID", DbType.Int32, mCounsellorID)
        db.AddInParameter(cmd, "@LawyerID", DbType.Int32, mLawyerID)
        db.AddInParameter(cmd, "@ShelterID", DbType.Int32, mShelterID)
        db.AddInParameter(cmd, "@CapturedFromID", DbType.Int32, mCapturedFromID)
        db.AddInParameter(cmd, "@ReferredToCounsellorByID", DbType.Int32, mReferredToCounsellorByID)
        db.AddInParameter(cmd, "@ReferredToLawyerByID", DbType.Int32, mReferredToLawyerByID)
        db.AddInParameter(cmd, "@ReferredToShelterByID", DbType.Int32, mReferredToShelterByID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ReferredToCounsellor", DbType.Boolean, mReferredToCounsellor)
        db.AddInParameter(cmd, "@ReferredToLaywer", DbType.Boolean, mReferredToLaywer)
        db.AddInParameter(cmd, "@ReferredToShelter", DbType.Boolean, mReferredToShelter)
        db.AddInParameter(cmd, "@NextOfKin", DbType.String, mNextOfKin)
        db.AddInParameter(cmd, "@ContactNo", DbType.String, mContactNo)
        db.AddInParameter(cmd, "@NatureOfRelationship", DbType.String, mNatureOfRelationship)
        db.AddInParameter(cmd, "@AccompanyingAdult1", DbType.String, mAccompanyingAdult1)
        db.AddInParameter(cmd, "@AccompanyingAdult2", DbType.String, mAccompanyingAdult2)
        db.AddInParameter(cmd, "@ReferredBy", DbType.String, mReferredBy)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ClientDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mClientDetailID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

    Public Sub SyncReferredBy(ByVal BeneficiaryID As Long, ByVal ReferredToLaywerByID As Long, ByVal ReferredToShelterByID As Long)

        Dim sql As String = ""

        If ReferredToLaywerByID > 0 And ReferredToShelterByID < 0 Then

            sql = "UPDATE tblInitialCounsellingSession SET ReferredToLaywer = 1 , LawyerID = " & ReferredToLaywerByID & " WHERE BeneficiaryID = " & BeneficiaryID

        ElseIf ReferredToShelterByID > 0 And ReferredToLaywerByID < 0 Then

            sql = "UPDATE tblInitialCounsellingSession SET ReferredToShelter = 1 , ShelterID = " & ReferredToShelterByID & " WHERE BeneficiaryID = " & BeneficiaryID

        ElseIf ReferredToShelterByID > 0 And ReferredToLaywerByID > 0 Then

            sql = "UPDATE tblInitialCounsellingSession SET ReferredToLaywer = 1 , LawyerID = " & ReferredToLaywerByID & ", ReferredToShelter = 1 , ShelterID = " & ReferredToShelterByID & " WHERE BeneficiaryID = " & BeneficiaryID

        End If

        db.ExecuteNonQuery(CommandType.Text, sql)

    End Sub

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblClientDetails SET Deleted = 1 WHERE ClientDetailID = " & mClientDetailID) 
        Return Delete("DELETE FROM tblClientDetails WHERE ClientDetailID = " & mClientDetailID)

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