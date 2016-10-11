Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class SchoolStaff

#region "Variables"

    Protected mSchoolStaffID As long
    Protected mSchoolID As long
    Protected mStaffRoleID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDOB As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mFirstName As string
    Protected mSurname As string
    Protected mTitle As string
    Protected mSex As string

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

    Public  Property SchoolStaffID() As long
        Get
		return mSchoolStaffID
        End Get
        Set(ByVal value As long)
		mSchoolStaffID = value
        End Set
    End Property

    Public  Property SchoolID() As long
        Get
		return mSchoolID
        End Get
        Set(ByVal value As long)
		mSchoolID = value
        End Set
    End Property

    Public  Property StaffRoleID() As long
        Get
		return mStaffRoleID
        End Get
        Set(ByVal value As long)
		mStaffRoleID = value
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

    Public  Property DOB() As string
        Get
		return mDOB
        End Get
        Set(ByVal value As string)
		mDOB = value
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

    Public  Property FirstName() As string
        Get
		return mFirstName
        End Get
        Set(ByVal value As string)
		mFirstName = value
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

    Public  Property Title() As string
        Get
		return mTitle
        End Get
        Set(ByVal value As string)
		mTitle = value
        End Set
    End Property

    Public  Property Sex() As string
        Get
		return mSex
        End Get
        Set(ByVal value As string)
		mSex = value
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

    SchoolStaffID = 0
    mSchoolID = 0
    mStaffRoleID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDOB = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mFirstName = ""
    mSurname = ""
    mTitle = ""
    mSex = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mSchoolStaffID) 

    End Function 

    Public Overridable Function Retrieve(ByVal SchoolStaffID As Long) As Boolean 

        Dim sql As String 

        If SchoolStaffID > 0 Then 
            sql = "SELECT * FROM tblSchoolStaff WHERE SchoolStaffID = " & SchoolStaffID
        Else 
            sql = "SELECT * FROM tblSchoolStaff WHERE SchoolStaffID = " & mSchoolStaffID
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

                log.error("SchoolStaff not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetSchoolStaff() As System.Data.DataSet

        Return GetSchoolStaff(mSchoolStaffID)

    End Function

    Public Overridable Function GetSchoolStaff(ByVal SchoolStaffID As Long) As DataSet

        Dim sql As String

        If SchoolStaffID > 0 Then
            sql = "SELECT * FROM tblSchoolStaff WHERE SchoolStaffID = " & SchoolStaffID
        Else
            sql = "SELECT * FROM tblSchoolStaff WHERE SchoolStaffID = " & mSchoolStaffID
        End If

        Return GetSchoolStaff(sql)

    End Function

    Public Overridable Function GetSchoolStaff(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mSchoolStaffID = Catchnull(.Item("SchoolStaffID"), 0)
            mSchoolID = Catchnull(.Item("SchoolID"), 0)
            mStaffRoleID = Catchnull(.Item("StaffRoleID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDOB = Catchnull(.Item("DOB"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mFirstName = Catchnull(.Item("FirstName"), "")
            mSurname = Catchnull(.Item("Surname"), "")
            mTitle = Catchnull(.Item("Title"), "")
            mSex = Catchnull(.Item("Sex"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@SchoolStaffID", DBType.Int32, mSchoolStaffID)
        db.AddInParameter(cmd, "@SchoolID", DBType.Int32, mSchoolID)
        db.AddInParameter(cmd, "@StaffRoleID", DBType.Int32, mStaffRoleID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DOB", DBType.String, mDOB)
        db.AddInParameter(cmd, "@FirstName", DBType.String, mFirstName)
        db.AddInParameter(cmd, "@Surname", DBType.String, mSurname)
        db.AddInParameter(cmd, "@Title", DBType.String, mTitle)
        db.AddInParameter(cmd, "@Sex", DBType.String, mSex)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_SchoolStaff")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mSchoolStaffID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblSchoolStaff SET Deleted = 1 WHERE SchoolStaffID = " & mSchoolStaffID) 
        Return Delete("DELETE FROM tblSchoolStaff WHERE SchoolStaffID = " & mSchoolStaffID)

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