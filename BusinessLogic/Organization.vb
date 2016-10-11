Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Organization

#region "Variables"

    Protected mOrganizationID As long
    Protected mWardID As long
    Protected mOrganizationTypeID As long
    Protected mLongitude As decimal
    Protected mLatitude As Decimal
    Protected mContactNo As String
    Protected mCellphoneNo As String
    Protected mName As string
    Protected mDescription As string
    Protected mContactName As String
    Protected mAddress As String
    Protected mEmail As String
    Protected mWebsiteAddress As String
    Protected mCreatedBy As Long
    Protected mUpdatedBy As Long
    Protected mCreatedDate As String
    Protected mUpdatedDate As String

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

    Public  Property OrganizationID() As long
        Get
		return mOrganizationID
        End Get
        Set(ByVal value As long)
		mOrganizationID = value
        End Set
    End Property

    Public  Property WardID() As long
        Get
		return mWardID
        End Get
        Set(ByVal value As long)
		mWardID = value
        End Set
    End Property

    Public  Property OrganizationTypeID() As long
        Get
		return mOrganizationTypeID
        End Get
        Set(ByVal value As long)
		mOrganizationTypeID = value
        End Set
    End Property

    Public  Property Longitude() As decimal
        Get
		return mLongitude
        End Get
        Set(ByVal value As decimal)
		mLongitude = value
        End Set
    End Property

    Public  Property Latitude() As decimal
        Get
		return mLatitude
        End Get
        Set(ByVal value As decimal)
		mLatitude = value
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
            Return mCellphoneNo
        End Get
        Set(ByVal value As String)
            mCellphoneNo = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Public Property WebsiteAddress() As String
        Get
            Return mWebsiteAddress
        End Get
        Set(ByVal value As String)
            mWebsiteAddress = value
        End Set
    End Property

    Public  Property Description() As string
        Get
		return mDescription
        End Get
        Set(ByVal value As string)
		mDescription = value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return mAddress
        End Get
        Set(value As String)
            mAddress = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return mEmail
        End Get
        Set(value As String)
            mEmail = value
        End Set
    End Property

    Public Property ContactName() As String
        Get
            Return mContactName
        End Get
        Set(ByVal value As String)
            mContactName = value
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

    OrganizationID = 0
    mWardID = 0
    mOrganizationTypeID = 0
    mLongitude = 0.0
    mLatitude = 0.0
        mContactNo = ""
        mCellphoneNo = ""
        mName = ""
    mDescription = ""
        mContactName = ""
        mEmail = ""
        mAddress = ""
        mWebsiteAddress = ""
        mUpdatedBy = 0
        mCreatedDate = ""
        mUpdatedDate = ""

    End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mOrganizationID) 

    End Function 

    Public Overridable Function Retrieve(ByVal OrganizationID As Long) As Boolean 

        Dim sql As String 

        If OrganizationID > 0 Then 
            sql = "SELECT * FROM tblOrganization WHERE OrganizationID = " & OrganizationID
        Else 
            sql = "SELECT * FROM tblOrganization WHERE OrganizationID = " & mOrganizationID
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

                log.Warn("Organization not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Function RetrieveAll() As DataSet

        Dim Criteria As String = IIf(GetParentOrganizationID() <> CookiesWrapper.OrganizationID Or CookiesWrapper.thisUserID <> 1, " WHERE O.OrganizationID = " & CookiesWrapper.OrganizationID, "")

        Dim sql As String = "Select T.Description As OrganizationType, O.* from tblOrganization O inner join luOrganizationTypes T on O.OrganizationTypeID = T.OrganizationTypeID"
        'sql &= Criteria

        Return GetOrganization(sql)

    End Function

    Public Overridable Function GetOrganization() As System.Data.DataSet

        Return GetOrganization(mOrganizationID)

    End Function

    Public Overridable Function GetOrganization(ByVal OrganizationID As Long) As DataSet

        Dim sql As String

        If OrganizationID > 0 Then
            sql = "SELECT * FROM tblOrganization WHERE OrganizationID = " & OrganizationID
        Else
            sql = "SELECT * FROM tblOrganization WHERE OrganizationID = " & mOrganizationID
        End If

        Return GetOrganization(sql)

    End Function

    Public Function GetPermittedUsersByOrganization(ByVal OrganizationID As Long) As String

        Dim sql As String
        Dim UserIDList As New List(Of String)

        sql = "Select S.UserID from tblUsers S inner join tblUserAccountProfileLink P on S.UserID = P.UserID "
        sql &= "inner join tblStaffMembers SM on SM.StaffID = P.StaffID where OrganizationID <> " & OrganizationID

        Dim ds As DataSet = GetOrganization(sql)

        If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

            For Each row As DataRow In ds.Tables(0).Rows

                UserIDList.Add(row(0))

            Next

            If UserIDList.Count > 0 Then Return String.Join(",", UserIDList.ToArray())

        End If

        Return "0"

    End Function

    Public Function GetParentOrganizationID() As Long

        Dim sql As String

        sql = "Select * from tblOrganization O inner join luOrganizationTypes OT on O.OrganizationTypeID = OT.OrganizationTypeID "
        sql &= "where OT.Description = 'Self'"

        Dim ds As DataSet = GetOrganization(sql)

        If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

            Return Catchnull(ds.Tables(0).Rows(0)("OrganizationID"), 0)

        End If

        Return 0

    End Function

    Protected Overridable Function GetOrganization(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mOrganizationID = Catchnull(.Item("OrganizationID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mOrganizationTypeID = Catchnull(.Item("OrganizationTypeID"), 0)
            mLongitude = Catchnull(.Item("Longitude"), 0.0)
            mLatitude = Catchnull(.Item("Latitude"), 0.0)
            mContactNo = Catchnull(.Item("ContactNo"), "")
            mCellphoneNo = Catchnull(.Item("CellPhoneNo"), "")
            mName = Catchnull(.Item("Name"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mContactName = Catchnull(.Item("ContactName"), "")
            mAddress = Catchnull(.Item("Address"), "")
            mEmail = Catchnull(.Item("EmailAddress"), "")
            mWebsiteAddress = Catchnull(.Item("WebsiteAddress"), "")
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@OrganizationID", DBType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@WardID", DBType.Int32, mWardID)
        db.AddInParameter(cmd, "@OrganizationTypeID", DBType.Int32, mOrganizationTypeID)
        db.AddInParameter(cmd, "@Longitude", DbType.Decimal, mLongitude)
        db.AddInParameter(cmd, "@Latitude", DbType.Decimal, mLatitude)
        db.AddInParameter(cmd, "@ContactNo", DbType.String, mContactNo)
        db.AddInParameter(cmd, "@CellPhoneNo", DbType.String, mCellphoneNo)
        db.AddInParameter(cmd, "@Name", DBType.String, mName)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)
        db.AddInParameter(cmd, "@ContactName", DbType.String, mContactName)
        db.AddInParameter(cmd, "@Address", DbType.String, mAddress)
        db.AddInParameter(cmd, "@EmailAddress", DbType.String, mEmail)
        db.AddInParameter(cmd, "@WebsiteAddress", DbType.String, mWebsiteAddress)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Organization")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mOrganizationID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblOrganization Set Deleted = 1 WHERE OrganizationID = " & mOrganizationID) 
        Return Delete("DELETE FROM tblOrganization WHERE OrganizationID = " & mOrganizationID)

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