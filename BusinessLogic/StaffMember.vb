Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class StaffMember

#region "Variables"

    Protected mStaffID As long
    Protected mOrganizationID As Long
    Protected mOrganizationTypeID As Long
    Protected mOrganization As String
    Protected mOrganizationType As String
    Protected mStaffPosition As String
    Protected mContactNo As String
    Protected mCellPhoneNo As String
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mName As string
    Protected mFirstName As string
    Protected mSurname As string
    Protected mSex As string
    Protected mPositionID As Long
    Protected mAddress As string
    Protected mEmailAddress As string

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

    Public  Property StaffID() As long
        Get
		return mStaffID
        End Get
        Set(ByVal value As long)
		mStaffID = value
        End Set
    End Property

    Public  Property OrganizationID() As long
        Get
		return mOrganizationID
        End Get
        Set(ByVal value As long)
		mOrganizationID = value
        End Set
    End Property

    Public Property OrganizationTypeID() As Long
        Get
            Return mOrganizationTypeID
        End Get
        Set(ByVal value As Long)
            mOrganizationTypeID = value
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

    Public Property CellPhoneNo() As String
        Get
            Return mCellPhoneNo
        End Get
        Set(ByVal value As String)
            mCellPhoneNo = value
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

    Public Property Organization() As String
        Get
            Return mOrganization
        End Get
        Set(ByVal value As String)
            mOrganization = value
        End Set
    End Property

    Public Property OrganizationType() As String
        Get
            Return mOrganizationType
        End Get
        Set(ByVal value As String)
            mOrganizationType = value
        End Set
    End Property

    Public Property StaffPosition() As String
        Get
            Return mStaffPosition
        End Get
        Set(ByVal value As String)
            mStaffPosition = value
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

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
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

    Public  Property Sex() As string
        Get
		return mSex
        End Get
        Set(ByVal value As string)
		mSex = value
        End Set
    End Property

    Public Property PositionID() As String
        Get
            Return mPositionID
        End Get
        Set(ByVal value As String)
            mPositionID = value
        End Set
    End Property

    Public  Property Address() As string
        Get
		return mAddress
        End Get
        Set(ByVal value As string)
		mAddress = value
        End Set
    End Property

    Public  Property EmailAddress() As string
        Get
		return mEmailAddress
        End Get
        Set(ByVal value As string)
		mEmailAddress = value
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

    StaffID = 0
    mOrganizationID = 0
        mContactNo = ""
        mCellPhoneNo = ""
        mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mName = ""
    mFirstName = ""
    mSurname = ""
    mSex = ""
        mPositionID = 0
    mAddress = ""
    mEmailAddress = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mStaffID) 

    End Function 

    Public Overridable Function Retrieve(ByVal StaffID As Long) As Boolean 

        Dim sql As String 

        If StaffID > 0 Then 
            sql = "SELECT S.*, O.Name As Organization, OT.OrganizationTypeID, OT.Description As OrganizationType, SP.Description as StaffPosition FROM tblStaffMembers S inner join tblOrganization O on S.OrganizationID = O.OrganizationID "
            sql &= "inner join luOrganizationTypes OT on OT.OrganizationTypeID = O.OrganizationTypeID "
            sql &= "left outer join luStaffPosition SP on SP.PositionID = S.PositionID WHERE StaffID = " & StaffID
        Else 
            sql = "SELECT S.*, O.Name As Organization, OT.OrganizationTypeID, OT.Description As OrganizationType, SP.Description as StaffPosition FROM tblStaffMembers S inner join tblOrganization O on S.OrganizationID = O.OrganizationID "
            sql &= "inner join luOrganizationTypes OT on OT.OrganizationTypeID = O.OrganizationTypeID "
            sql &= "left outer join luStaffPosition SP on SP.PositionID = S.PositionID WHERE StaffID = " & mStaffID
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

                log.Warn("StaffMember not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetStaffMember() As System.Data.DataSet

        Return GetStaffMember(mStaffID) 

    End Function 

    Public Overridable Function GetStaffMember(ByVal StaffID As Long) As DataSet 

        Dim sql As String 

        If StaffID > 0 Then 
            sql = "SELECT * FROM tblStaffMembers WHERE StaffID = " & StaffID
        Else 
            sql = "SELECT * FROM tblStaffMembers WHERE StaffID = " & mStaffID
        End If 

        Return GetStaffMember(sql) 

    End Function 

    Public Overridable Function GetStaffMember(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Overridable Function GetAllStaffMember() As DataSet

        Dim objOrganization As New Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim Criteria = IIf(objOrganization.GetParentOrganizationID() <> CookiesWrapper.OrganizationID Or CookiesWrapper.thisUserID <> 1, " WHERE O.OrganizationID = " & CookiesWrapper.OrganizationID, "")

        Dim sql As String = "SELECT S.StaffID, O.Name As Organization, SP.Description As Position, S.FirstName, S.Surname, S.ContactNo, S.CellPhoneNo, "
        sql &= " S.EmailAddress, S.Address, S.Sex FROM tblStaffMembers S inner join tblOrganization O on S.OrganizationID = O.OrganizationID"
        sql &= " left outer join luStaffPosition SP on SP.PositionID = S.PositionID" '& Criteria

        Return db.ExecuteDataSet(CommandType.Text, Sql)

    End Function

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mStaffID = Catchnull(.Item("StaffID"), 0)
            mOrganizationID = Catchnull(.Item("OrganizationID"), 0)
            mOrganizationTypeID = Catchnull(.Item("OrganizationTypeID"), 0)
            mContactNo = Catchnull(.Item("ContactNo"), "")
            mCellPhoneNo = Catchnull(.Item("CellPhoneNo"), "")
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mName = Catchnull(.Item("Name"), "")
            mFirstName = Catchnull(.Item("FirstName"), "")
            mSurname = Catchnull(.Item("Surname"), "")
            mSex = Catchnull(.Item("Sex"), "")
            mPositionID = Catchnull(.Item("PositionID"), 0)
            mAddress = Catchnull(.Item("Address"), "")
            mEmailAddress = Catchnull(.Item("EmailAddress"), "")
            mOrganization = Catchnull(.Item("Organization"), "")
            mOrganizationType = Catchnull(.Item("OrganizationType"), "")
            mStaffPosition = Catchnull(.Item("StaffPosition"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@StaffID", DBType.Int32, mStaffID) 
        db.AddInParameter(cmd, "@OrganizationID", DBType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@ContactNo", DbType.String, mContactNo)
        db.AddInParameter(cmd, "@CellPhoneNo", DbType.String, mCellPhoneNo)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID) 
        db.AddInParameter(cmd, "@Name", DBType.String, mName) 
        db.AddInParameter(cmd, "@FirstName", DBType.String, mFirstName) 
        db.AddInParameter(cmd, "@Surname", DBType.String, mSurname) 
        db.AddInParameter(cmd, "@Sex", DBType.String, mSex) 
        db.AddInParameter(cmd, "@PositionID", DbType.String, mPositionID)
        db.AddInParameter(cmd, "@Address", DBType.String, mAddress) 
        db.AddInParameter(cmd, "@EmailAddress", DBType.String, mEmailAddress) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_StaffMember") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mStaffID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblStaffMembers SET Deleted = 1 WHERE StaffID = " & mStaffID) 
        Return Delete("DELETE FROM tblStaffMembers WHERE StaffID = " & mStaffID) 

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