Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class HealthCenterStaff

#Region "Variables"

    Protected mHealthCenterStaffID As Long
    Protected mHealthCenterID As Long
    Protected mStaffRoleID As Long
    Protected mGroupTypeID As Long
    Protected mCreatedBy As Long
    Protected mUpdatedBy As Long
    Protected mProvinceID As Long
    Protected mDistrictID As Long
    Protected mDepartmentID As Long
    Protected mDOB As String
    Protected mCreatedDate As String
    Protected mUpdatedDate As String
    Protected mFirstName As String
    Protected mSurname As String
    Protected mTitle As String
    Protected mSex As String
    Protected mSite As String
    Protected mEmail As String
    Protected mContactNo As String
    Protected mIDNumber As String

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected db As Database
    Protected mConnectionName As String
    Protected mObjectUserID As Long

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

    Public Property HealthCenterStaffID() As Long
        Get
            Return mHealthCenterStaffID
        End Get
        Set(ByVal value As Long)
            mHealthCenterStaffID = value
        End Set
    End Property

    Public Property GroupTypeID() As Long
        Get
            Return mGroupTypeID
        End Get
        Set(ByVal value As Long)
            mGroupTypeID = value
        End Set
    End Property

    Public Property HealthCenterID() As Long
        Get
            Return mHealthCenterID
        End Get
        Set(ByVal value As Long)
            mHealthCenterID = value
        End Set
    End Property

    Public Property StaffRoleID() As Long
        Get
            Return mStaffRoleID
        End Get
        Set(ByVal value As Long)
            mStaffRoleID = value
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

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
        End Set
    End Property

    Public Property ProvinceID() As Long
        Get
            Return mProvinceID
        End Get
        Set(ByVal value As Long)
            mProvinceID = value
        End Set
    End Property

    Public Property DistrictID() As Long
        Get
            Return mDistrictID
        End Get
        Set(ByVal value As Long)
            mDistrictID = value
        End Set
    End Property

    Public Property DepartmentID() As Long
        Get
            Return mDepartmentID
        End Get
        Set(ByVal value As Long)
            mDepartmentID = value
        End Set
    End Property

    Public Property DOB() As String
        Get
            Return mDOB
        End Get
        Set(ByVal value As String)
            mDOB = value
        End Set
    End Property

    Public Property CreatedDate() As String
        Get
            Return mCreatedDate
        End Get
        Set(ByVal value As String)
            mCreatedDate = value
        End Set
    End Property

    Public Property UpdatedDate() As String
        Get
            Return mUpdatedDate
        End Get
        Set(ByVal value As String)
            mUpdatedDate = value
        End Set
    End Property

    Public Property FirstName() As String
        Get
            Return mFirstName
        End Get
        Set(ByVal value As String)
            mFirstName = value
        End Set
    End Property

    Public Property Surname() As String
        Get
            Return mSurname
        End Get
        Set(ByVal value As String)
            mSurname = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return mTitle
        End Get
        Set(ByVal value As String)
            mTitle = value
        End Set
    End Property

    Public Property Sex() As String
        Get
            Return mSex
        End Get
        Set(ByVal value As String)
            mSex = value
        End Set
    End Property

    Public Property Site() As String
        Get
            Return mSite
        End Get
        Set(ByVal value As String)
            mSite = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return mEmail
        End Get
        Set(ByVal value As String)
            mEmail = value
        End Set
    End Property

    Public Property ContactNo() As String
        Get
            Return mContactNo
        End Get
        Set(ByVal value As String)
            mContactNo = value
        End Set
    End Property

    Public Property IDNumber() As String
        Get
            Return mIDNumber
        End Get
        Set(ByVal value As String)
            mIDNumber = value
        End Set
    End Property

#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub

#End Region

    Public Sub Clear()

        HealthCenterStaffID = 0
        mHealthCenterID = 0
        mStaffRoleID = 0
        mGroupTypeID = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mProvinceID = 0
        mDistrictID = 0
        mDepartmentID = 0
        mDOB = ""
        mCreatedDate = ""
        mUpdatedDate = ""
        mFirstName = ""
        mSurname = ""
        mTitle = ""
        mSex = ""
        mSite = ""
        mEmail = ""
        mContactNo = ""
        mIDNumber = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mHealthCenterStaffID)

    End Function

    Public Overridable Function Retrieve(ByVal HealthCenterStaffID As Long) As Boolean

        Dim sql As String

        If HealthCenterStaffID > 0 Then
            sql = "SELECT * FROM tblHealthCenterStaff WHERE HealthCenterStaffID = " & HealthCenterStaffID
        Else
            sql = "SELECT * FROM tblHealthCenterStaff WHERE HealthCenterStaffID = " & mHealthCenterStaffID
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

                log.Error("HealthCenterStaff not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetHealthCenterStaff() As System.Data.DataSet

        Return GetHealthCenterStaff(mHealthCenterStaffID)

    End Function

    Public Overridable Function GetHealthCenterStaff(ByVal HealthCenterStaffID As Long) As DataSet

        Dim sql As String

        If HealthCenterStaffID > 0 Then
            sql = "SELECT * FROM tblHealthCenterStaff WHERE HealthCenterStaffID = " & HealthCenterStaffID
        Else
            sql = "SELECT * FROM tblHealthCenterStaff WHERE HealthCenterStaffID = " & mHealthCenterStaffID
        End If

        Return GetHealthCenterStaff(sql)

    End Function

    Public Overridable Function GetHealthCenterStaff(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mHealthCenterStaffID = Catchnull(.Item("HealthCenterStaffID"), 0)
            mHealthCenterID = Catchnull(.Item("HealthCenterID"), 0)
            mStaffRoleID = Catchnull(.Item("StaffRoleID"), 0)
            mGroupTypeID = Catchnull(.Item("GroupTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mProvinceID = Catchnull(.Item("ProvinceID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mDepartmentID = Catchnull(.Item("DepartmentID"), 0)
            mDOB = Catchnull(.Item("DOB"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mFirstName = Catchnull(.Item("FirstName"), "")
            mSurname = Catchnull(.Item("Surname"), "")
            mTitle = Catchnull(.Item("Title"), "")
            mSex = Catchnull(.Item("Sex"), "")
            mSite = Catchnull(.Item("Site"), "")
            mEmail = Catchnull(.Item("Email"), "")
            mContactNo = Catchnull(.Item("ContactNo"), "")
            mIDNumber = Catchnull(.Item("IDNumber"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@HealthCenterStaffID", DbType.Int32, mHealthCenterStaffID)
        db.AddInParameter(cmd, "@HealthCenterID", DbType.Int32, mHealthCenterID)
        db.AddInParameter(cmd, "@StaffRoleID", DbType.Int32, mStaffRoleID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ProvinceID", DbType.Int32, mProvinceID)
        db.AddInParameter(cmd, "@DistrictID", DbType.Int32, mDistrictID)
        db.AddInParameter(cmd, "@DepartmentID", DbType.Int32, mDepartmentID)
        db.AddInParameter(cmd, "@DOB", DbType.String, mDOB)
        db.AddInParameter(cmd, "@FirstName", DbType.String, mFirstName)
        db.AddInParameter(cmd, "@Surname", DbType.String, mSurname)
        db.AddInParameter(cmd, "@Title", DbType.String, mTitle)
        db.AddInParameter(cmd, "@Sex", DbType.String, mSex)
        db.AddInParameter(cmd, "@Site", DbType.String, mSite)
        db.AddInParameter(cmd, "@Email", DbType.String, mEmail)
        db.AddInParameter(cmd, "@ContactNo", DbType.String, mContactNo)
        db.AddInParameter(cmd, "@IDNumber", DbType.String, mIDNumber)
        db.AddInParameter(cmd, "@GroupTypeID", DbType.String, mGroupTypeID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_HealthCenterStaff")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mHealthCenterStaffID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblHealthCenterStaff SET Deleted = 1 WHERE HealthCenterStaffID = " & mHealthCenterStaffID) 
        Return Delete("DELETE FROM tblHealthCenterStaff WHERE HealthCenterStaffID = " & mHealthCenterStaffID)

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

#Region "Miscellaneous"

    Public Function RetriveRecordByID(ByVal IDNumber As String) As DataSet

        Dim sql As String = "SELECT TOP 1 * FROM tblHealthCenterStaff WHERE REPLACE(REPLACE(REPLACE(IDNumber, '-', ''), ' ', ''), '_', '') = '" & Replace(Replace(Replace(IDNumber, "-", ""), " ", ""), "_", "") & "'"

        Return GetHealthCenterStaff(sql)

    End Function

#End Region

#End Region

End Class