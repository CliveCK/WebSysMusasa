Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GranteeDetails

#region "Variables"

    Protected mGranteeID As long
    Protected mGrantDetailID As long
    Protected mPartnerID As long
    Protected mContactPersonID As long
    Protected mPartnershipTypeID As long
    Protected mProjectStatusID As long
    Protected mProjectDuration As long
    Protected mDistrictID As long
    Protected mNumberOfReports As long
    Protected mExtensionTypeID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mProjectStartDate As string
    Protected mProjectEndDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mExtensionGranted As boolean
    Protected mTotalGrantValue As decimal
    Protected mProjectTitle As string
    Protected mProjectDeliverables As string
    Protected mReasonForExtension As string

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

    Public  Property GranteeID() As long
        Get
		return mGranteeID
        End Get
        Set(ByVal value As long)
		mGranteeID = value
        End Set
    End Property

    Public  Property GrantDetailID() As long
        Get
		return mGrantDetailID
        End Get
        Set(ByVal value As long)
		mGrantDetailID = value
        End Set
    End Property

    Public  Property PartnerID() As long
        Get
		return mPartnerID
        End Get
        Set(ByVal value As long)
		mPartnerID = value
        End Set
    End Property

    Public  Property ContactPersonID() As long
        Get
		return mContactPersonID
        End Get
        Set(ByVal value As long)
		mContactPersonID = value
        End Set
    End Property

    Public  Property PartnershipTypeID() As long
        Get
		return mPartnershipTypeID
        End Get
        Set(ByVal value As long)
		mPartnershipTypeID = value
        End Set
    End Property

    Public  Property ProjectStatusID() As long
        Get
		return mProjectStatusID
        End Get
        Set(ByVal value As long)
		mProjectStatusID = value
        End Set
    End Property

    Public  Property ProjectDuration() As long
        Get
		return mProjectDuration
        End Get
        Set(ByVal value As long)
		mProjectDuration = value
        End Set
    End Property

    Public  Property DistrictID() As long
        Get
		return mDistrictID
        End Get
        Set(ByVal value As long)
		mDistrictID = value
        End Set
    End Property

    Public  Property NumberOfReports() As long
        Get
		return mNumberOfReports
        End Get
        Set(ByVal value As long)
		mNumberOfReports = value
        End Set
    End Property

    Public  Property ExtensionTypeID() As long
        Get
		return mExtensionTypeID
        End Get
        Set(ByVal value As long)
		mExtensionTypeID = value
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

    Public  Property ProjectStartDate() As string
        Get
		return mProjectStartDate
        End Get
        Set(ByVal value As string)
		mProjectStartDate = value
        End Set
    End Property

    Public  Property ProjectEndDate() As string
        Get
		return mProjectEndDate
        End Get
        Set(ByVal value As string)
		mProjectEndDate = value
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

    Public  Property ExtensionGranted() As boolean
        Get
		return mExtensionGranted
        End Get
        Set(ByVal value As boolean)
		mExtensionGranted = value
        End Set
    End Property

    Public  Property TotalGrantValue() As decimal
        Get
		return mTotalGrantValue
        End Get
        Set(ByVal value As decimal)
		mTotalGrantValue = value
        End Set
    End Property

    Public  Property ProjectTitle() As string
        Get
		return mProjectTitle
        End Get
        Set(ByVal value As string)
		mProjectTitle = value
        End Set
    End Property

    Public  Property ProjectDeliverables() As string
        Get
		return mProjectDeliverables
        End Get
        Set(ByVal value As string)
		mProjectDeliverables = value
        End Set
    End Property

    Public  Property ReasonForExtension() As string
        Get
		return mReasonForExtension
        End Get
        Set(ByVal value As string)
		mReasonForExtension = value
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

    GranteeID = 0
    mGrantDetailID = 0
    mPartnerID = 0
    mContactPersonID = 0
    mPartnershipTypeID = 0
    mProjectStatusID = 0
    mProjectDuration = 0
    mDistrictID = 0
    mNumberOfReports = 0
    mExtensionTypeID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mProjectStartDate = ""
    mProjectEndDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mExtensionGranted = FALSE
    mTotalGrantValue = 0.0
    mProjectTitle = ""
    mProjectDeliverables = ""
    mReasonForExtension = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGranteeID) 

    End Function 

    Public Overridable Function Retrieve(ByVal GranteeID As Long) As Boolean 

        Dim sql As String 

        If GranteeID > 0 Then 
            sql = "SELECT * FROM tblGranteeDetails WHERE GranteeID = " & GranteeID
        Else 
            sql = "SELECT * FROM tblGranteeDetails WHERE GranteeID = " & mGranteeID
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

                log.Error("GranteeDetails not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGranteeDetails() As System.Data.DataSet

        Return GetGranteeDetails(mGranteeID)

    End Function

    Public Overridable Function GetGranteeDetails(ByVal GranteeID As Long) As DataSet

        Dim sql As String

        If GranteeID > 0 Then
            sql = "SELECT * FROM tblGranteeDetails WHERE GranteeID = " & GranteeID
        Else
            sql = "SELECT * FROM tblGranteeDetails WHERE GranteeID = " & mGranteeID
        End If

        Return GetGranteeDetails(sql)

    End Function

    Public Overridable Function RetrieveAll() As DataSet

        Dim sql As String

        sql = "Select G.*, S.StaffFullName As ProjectManager, Ds.Name As District, O.Name As PartnerName, O1.Name As ParentDonor from tblGranteeDetails G inner join "
        sql &= "tblGrantDetails D On G.GrantDetailID = D.GrantDetailID "
        sql &= " Left outer join tblProjects P On P.Project = D.ProjectID "
        sql &= "left outer join tblStaffMembers S On S.StaffID = P.ProjectManager "
        sql &= "Left outer join tblDistricts Ds On Ds.DistrictID = G.DistrictID "
        sql &= "left outer join tblOrganization O On O.OrganizationID = G.PartnerID "
        sql &= "Left outer join tblOrganization O1 On O1.OrganizationID = D.DonorID"

        Return GetGranteeDetails(sql)

    End Function

    Protected Overridable Function GetGranteeDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGranteeID = Catchnull(.Item("GranteeID"), 0)
            mGrantDetailID = Catchnull(.Item("GrantDetailID"), 0)
            mPartnerID = Catchnull(.Item("PartnerID"), 0)
            mContactPersonID = Catchnull(.Item("ContactPersonID"), 0)
            mPartnershipTypeID = Catchnull(.Item("PartnershipTypeID"), 0)
            mProjectStatusID = Catchnull(.Item("ProjectStatusID"), 0)
            mProjectDuration = Catchnull(.Item("ProjectDuration"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mNumberOfReports = Catchnull(.Item("NumberOfReports"), 0)
            mExtensionTypeID = Catchnull(.Item("ExtensionTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mProjectStartDate = Catchnull(.Item("ProjectStartDate"), "")
            mProjectEndDate = Catchnull(.Item("ProjectEndDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mExtensionGranted = Catchnull(.Item("ExtensionGranted"), False)
            mTotalGrantValue = Catchnull(.Item("TotalGrantValue"), 0.0)
            mProjectTitle = Catchnull(.Item("ProjectTitle"), "")
            mProjectDeliverables = Catchnull(.Item("ProjectDeliverables"), "")
            mReasonForExtension = Catchnull(.Item("ReasonForExtension"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GranteeID", DbType.Int32, mGranteeID)
        db.AddInParameter(cmd, "@GrantDetailID", DbType.Int32, mGrantDetailID)
        db.AddInParameter(cmd, "@PartnerID", DbType.Int32, mPartnerID)
        db.AddInParameter(cmd, "@ContactPersonID", DbType.Int32, mContactPersonID)
        db.AddInParameter(cmd, "@PartnershipTypeID", DbType.Int32, mPartnershipTypeID)
        db.AddInParameter(cmd, "@ProjectStatusID", DbType.Int32, mProjectStatusID)
        db.AddInParameter(cmd, "@ProjectDuration", DbType.Int32, mProjectDuration)
        db.AddInParameter(cmd, "@DistrictID", DbType.Int32, mDistrictID)
        db.AddInParameter(cmd, "@NumberOfReports", DbType.Int32, mNumberOfReports)
        db.AddInParameter(cmd, "@ExtensionTypeID", DbType.Int32, mExtensionTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ProjectStartDate", DbType.String, mProjectStartDate)
        db.AddInParameter(cmd, "@ProjectEndDate", DbType.String, mProjectEndDate)
        db.AddInParameter(cmd, "@ExtensionGranted", DbType.Boolean, mExtensionGranted)
        db.AddInParameter(cmd, "@TotalGrantValue", DbType.Decimal, mTotalGrantValue)
        db.AddInParameter(cmd, "@ProjectTitle", DbType.String, mProjectTitle)
        db.AddInParameter(cmd, "@ProjectDeliverables", DbType.String, mProjectDeliverables)
        db.AddInParameter(cmd, "@ReasonForExtension", DbType.String, mReasonForExtension)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GranteeDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGranteeID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGranteeDetails Set Deleted = 1 WHERE GranteeID = " & mGranteeID) 
        Return Delete("DELETE FROM tblGranteeDetails WHERE GranteeID = " & mGranteeID)

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