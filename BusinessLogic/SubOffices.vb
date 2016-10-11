Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class SubOffices

#region "Variables"

    Protected mSubOfficeID As long
    Protected mOrganizationID As long
    Protected mContactNo As long
    Protected mFax As long
    Protected mName As string
    Protected mEmail As string
    Protected mPhysicalAddress As string

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

    Public  Property SubOfficeID() As long
        Get
		return mSubOfficeID
        End Get
        Set(ByVal value As long)
		mSubOfficeID = value
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

    Public  Property ContactNo() As long
        Get
		return mContactNo
        End Get
        Set(ByVal value As long)
		mContactNo = value
        End Set
    End Property

    Public  Property Fax() As long
        Get
		return mFax
        End Get
        Set(ByVal value As long)
		mFax = value
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

    Public  Property Email() As string
        Get
		return mEmail
        End Get
        Set(ByVal value As string)
		mEmail = value
        End Set
    End Property

    Public  Property PhysicalAddress() As string
        Get
		return mPhysicalAddress
        End Get
        Set(ByVal value As string)
		mPhysicalAddress = value
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

    SubOfficeID = 0
    mOrganizationID = 0
    mContactNo = 0
    mFax = 0
    mName = ""
    mEmail = ""
    mPhysicalAddress = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mSubOfficeID) 

    End Function 

    Public Overridable Function Retrieve(ByVal SubOfficeID As Long) As Boolean 

        Dim sql As String 

        If SubOfficeID > 0 Then 
            sql = "SELECT * FROM tblSubOffices WHERE SubOfficeID = " & SubOfficeID
        Else 
            sql = "SELECT * FROM tblSubOffices WHERE SubOfficeID = " & mSubOfficeID
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

                log.Warn("SubOffices not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Function GetSubOfficesByOrganization(ByVal OrganizationID As Long) As DataSet

        Return GetSubOffices("SELECT * FROM tblSubOffices WHERE OrganizationID = " & OrganizationID)

    End Function

    Public Function GetOrganizationBySubOffices(ByVal SubOfficeID As Long) As DataSet

        Return GetSubOffices("SELECT * FROM tblSubOffices S inner join tblOrganization O on S.OrganizationID = O.OrganizationID WHERE SubOfficeID = " & SubOfficeID)

    End Function

    Public Overridable Function GetSubOffices() As System.Data.DataSet

        Return GetSubOffices(mSubOfficeID)

    End Function

    Public Overridable Function GetSubOffices(ByVal SubOfficeID As Long) As DataSet

        Dim sql As String

        If SubOfficeID > 0 Then
            sql = "SELECT * FROM tblSubOffices WHERE SubOfficeID = " & SubOfficeID
        Else
            sql = "SELECT * FROM tblSubOffices WHERE SubOfficeID = " & mSubOfficeID
        End If

        Return GetSubOffices(sql)

    End Function

    Public Overridable Function GetOrganizationSubOffices(ByVal OrganizationID As Long) As DataSet

        Dim sql As String

        If OrganizationID > 0 Then
            sql = "SELECT * FROM tblSubOffices WHERE OrganizationID = " & OrganizationID
        Else
            sql = "SELECT * FROM tblSubOffices WHERE OrganizationID = " & mOrganizationID
        End If

        Return GetSubOffices(sql)

    End Function

    Public Overridable Function GetSubOffices(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mSubOfficeID = Catchnull(.Item("SubOfficeID"), 0)
            mOrganizationID = Catchnull(.Item("OrganizationID"), 0)
            mContactNo = Catchnull(.Item("ContactNo"), 0)
            mFax = Catchnull(.Item("Fax"), 0)
            mName = Catchnull(.Item("Name"), "")
            mEmail = Catchnull(.Item("Email"), "")
            mPhysicalAddress = Catchnull(.Item("PhysicalAddress"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@SubOfficeID", DBType.Int32, mSubOfficeID)
        db.AddInParameter(cmd, "@OrganizationID", DBType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@ContactNo", DBType.Int64, mContactNo)
        db.AddInParameter(cmd, "@Fax", DBType.Int64, mFax)
        db.AddInParameter(cmd, "@Name", DBType.String, mName)
        db.AddInParameter(cmd, "@Email", DBType.String, mEmail)
        db.AddInParameter(cmd, "@PhysicalAddress", DBType.String, mPhysicalAddress)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_SubOffices")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mSubOfficeID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblSubOffices SET Deleted = 1 WHERE SubOfficeID = " & mSubOfficeID) 
        Return Delete("DELETE FROM tblSubOffices WHERE SubOfficeID = " & mSubOfficeID)

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