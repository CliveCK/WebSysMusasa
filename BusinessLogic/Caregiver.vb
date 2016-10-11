Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Caregiver

#region "Variables"

    Protected mCaregiverID As long
    Protected mPatientID As long
    Protected mProfession As long
    Protected mRelationshipToChild As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateOfBirth As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mFirstname As string
    Protected mSurname As string
    Protected mContactNo As string
    Protected mNameOfEmployer As string
    Protected mSocialSupportSystems As string
    Protected mAlternateSourcesOfIncome As string

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

    Public  Property CaregiverID() As long
        Get
		return mCaregiverID
        End Get
        Set(ByVal value As long)
		mCaregiverID = value
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

    Public  Property Profession() As long
        Get
		return mProfession
        End Get
        Set(ByVal value As long)
		mProfession = value
        End Set
    End Property

    Public  Property RelationshipToChild() As long
        Get
		return mRelationshipToChild
        End Get
        Set(ByVal value As long)
		mRelationshipToChild = value
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

    Public  Property DateOfBirth() As string
        Get
		return mDateOfBirth
        End Get
        Set(ByVal value As string)
		mDateOfBirth = value
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

    Public  Property Firstname() As string
        Get
		return mFirstname
        End Get
        Set(ByVal value As string)
		mFirstname = value
        End Set
    End Property

    Public  Property Surname() As string
        Get
		return mSurname
        End Get
        Set(ByVal value As string)
		mSurname = value
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

    Public  Property NameOfEmployer() As string
        Get
		return mNameOfEmployer
        End Get
        Set(ByVal value As string)
		mNameOfEmployer = value
        End Set
    End Property

    Public  Property SocialSupportSystems() As string
        Get
		return mSocialSupportSystems
        End Get
        Set(ByVal value As string)
		mSocialSupportSystems = value
        End Set
    End Property

    Public  Property AlternateSourcesOfIncome() As string
        Get
		return mAlternateSourcesOfIncome
        End Get
        Set(ByVal value As string)
		mAlternateSourcesOfIncome = value
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

    CaregiverID = 0
    mPatientID = 0
    mProfession = 0
    mRelationshipToChild = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateOfBirth = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mFirstname = ""
    mSurname = ""
    mContactNo = ""
    mNameOfEmployer = ""
    mSocialSupportSystems = ""
    mAlternateSourcesOfIncome = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCaregiverID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CaregiverID As Long) As Boolean 

        Dim sql As String 

        If CaregiverID > 0 Then 
            sql = "SELECT * FROM tblCaregiver WHERE CaregiverID = " & CaregiverID
        Else 
            sql = "SELECT * FROM tblCaregiver WHERE CaregiverID = " & mCaregiverID
        End If 

        Return Retrieve(sql) 

    End Function

    Public Function RetrieveByPatient(ByVal PatientID As Long) As Boolean

        Dim sql As String

        If PatientID > 0 Then
            sql = "SELECT * FROM tblCaregiver WHERE PatientID = " & PatientID
        Else
            sql = "SELECT * FROM tblCaregiver WHERE PatientID = " & mPatientID
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

                log.error("Caregiver not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCaregiver() As System.Data.DataSet

        Return GetCaregiver(mCaregiverID)

    End Function

    Public Overridable Function GetCaregiver(ByVal CaregiverID As Long) As DataSet

        Dim sql As String

        If CaregiverID > 0 Then
            sql = "SELECT * FROM tblCaregiver WHERE CaregiverID = " & CaregiverID
        Else
            sql = "SELECT * FROM tblCaregiver WHERE CaregiverID = " & mCaregiverID
        End If

        Return GetCaregiver(sql)

    End Function

    Protected Overridable Function GetCaregiver(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mCaregiverID = Catchnull(.Item("CaregiverID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mProfession = Catchnull(.Item("Profession"), 0)
            mRelationshipToChild = Catchnull(.Item("RelationshipToChild"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateOfBirth = Catchnull(.Item("DateOfBirth"), Nothing)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mFirstname = Catchnull(.Item("Firstname"), "")
            mSurname = Catchnull(.Item("Surname"), "")
            mContactNo = Catchnull(.Item("ContactNo"), "")
            mNameOfEmployer = Catchnull(.Item("NameOfEmployer"), "")
            mSocialSupportSystems = Catchnull(.Item("SocialSupportSystems"), "")
            mAlternateSourcesOfIncome = Catchnull(.Item("AlternateSourcesOfIncome"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@CaregiverID", DBType.Int32, mCaregiverID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@Profession", DBType.Int32, mProfession)
        db.AddInParameter(cmd, "@RelationshipToChild", DBType.Int32, mRelationshipToChild)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateOfBirth", DBType.String, mDateOfBirth)
        db.AddInParameter(cmd, "@Firstname", DBType.String, mFirstname)
        db.AddInParameter(cmd, "@Surname", DBType.String, mSurname)
        db.AddInParameter(cmd, "@ContactNo", DBType.String, mContactNo)
        db.AddInParameter(cmd, "@NameOfEmployer", DBType.String, mNameOfEmployer)
        db.AddInParameter(cmd, "@SocialSupportSystems", DBType.String, mSocialSupportSystems)
        db.AddInParameter(cmd, "@AlternateSourcesOfIncome", DBType.String, mAlternateSourcesOfIncome)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Caregiver")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mCaregiverID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblCaregiver SET Deleted = 1 WHERE CaregiverID = " & mCaregiverID) 
        Return Delete("DELETE FROM tblCaregiver WHERE CaregiverID = " & mCaregiverID)

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