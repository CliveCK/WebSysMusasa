Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Outreach

#region "Variables"

    Protected mOutreachID As long
    Protected mProjectID As long
    Protected mOrganizationID As Long
    Protected mBeneficiaryTypeID As Long
    Protected mDistrictID As long
    Protected mWardID As long
    Protected mTotal As Long
    Protected mStartDate As String
    Protected mEndDate As String
    Protected mPurposeOfOutreach As String

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

    Public  Property OutreachID() As long
        Get
		return mOutreachID
        End Get
        Set(ByVal value As long)
		mOutreachID = value
        End Set
    End Property

    Public  Property ProjectID() As long
        Get
		return mProjectID
        End Get
        Set(ByVal value As long)
		mProjectID = value
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

    Public  Property BeneficiaryTypeID() As long
        Get
		return mBeneficiaryTypeID
        End Get
        Set(ByVal value As long)
		mBeneficiaryTypeID = value
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

    Public  Property WardID() As long
        Get
		return mWardID
        End Get
        Set(ByVal value As long)
		mWardID = value
        End Set
    End Property

    Public  Property Total() As long
        Get
		return mTotal
        End Get
        Set(ByVal value As long)
		mTotal = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return mStartDate
        End Get
        Set(ByVal value As String)
            mStartDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return mEndDate
        End Get
        Set(ByVal value As String)
            mEndDate = value
        End Set
    End Property

    Public Property PurposeOfOutreach() As String
        Get
            Return mPurposeOfOutreach
        End Get
        Set(ByVal value As String)
            mPurposeOfOutreach = value
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

    OutreachID = 0
    mProjectID = 0
    mOrganizationID = 0
    mBeneficiaryTypeID = 0
    mDistrictID = 0
    mWardID = 0
        mTotal = 0
        mStartDate = ""
        mEndDate = ""
        mPurposeOfOutreach = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mOutreachID) 

    End Function 

    Public Overridable Function Retrieve(ByVal OutreachID As Long) As Boolean 

        Dim sql As String 

        If OutreachID > 0 Then 
            sql = "SELECT * FROM tblOutreach WHERE OutreachID = " & OutreachID
        Else 
            sql = "SELECT * FROM tblOutreach WHERE OutreachID = " & mOutreachID
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

                log.error("Outreach not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetOutreach() As System.Data.DataSet

        Return GetOutreach(mOutreachID)

    End Function

    Public Overridable Function GetOutreach(ByVal OutreachID As Long) As DataSet

        Dim sql As String

        If OutreachID > 0 Then
            sql = "SELECT * FROM tblOutreach WHERE OutreachID = " & OutreachID
        Else
            sql = "SELECT * FROM tblOutreach WHERE OutreachID = " & mOutreachID
        End If

        Return GetOutreach(sql)

    End Function

    Public Overridable Function GetOutreach(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mOutreachID = Catchnull(.Item("OutreachID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mOrganizationID = Catchnull(.Item("OrganizationID"), 0)
            mBeneficiaryTypeID = Catchnull(.Item("BeneficiaryTypeID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mTotal = Catchnull(.Item("Total"), 0)
            mStartDate = Catchnull(.Item("StartDate"), "")
            mEndDate = Catchnull(.Item("EndDate"), "")
            mPurposeOfOutreach = Catchnull(.Item("PurposeOfOutreach"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@OutreachID", DBType.Int32, mOutreachID)
        db.AddInParameter(cmd, "@ProjectID", DBType.Int32, mProjectID)
        db.AddInParameter(cmd, "@OrganizationID", DBType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@BeneficiaryTypeID", DBType.Int32, mBeneficiaryTypeID)
        db.AddInParameter(cmd, "@DistrictID", DBType.Int32, mDistrictID)
        db.AddInParameter(cmd, "@WardID", DBType.Int32, mWardID)
        db.AddInParameter(cmd, "@Total", DbType.Int64, mTotal)
        db.AddInParameter(cmd, "@StartDate", DbType.String, mStartDate)
        db.AddInParameter(cmd, "@EndDate", DbType.String, mEndDate)
        db.AddInParameter(cmd, "@PurposeOfOutreach", DbType.String, mPurposeOfOutreach)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Outreach")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mOutreachID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblOutreach SET Deleted = 1 WHERE OutreachID = " & mOutreachID) 
        Return Delete("DELETE FROM tblOutreach WHERE OutreachID = " & mOutreachID)

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