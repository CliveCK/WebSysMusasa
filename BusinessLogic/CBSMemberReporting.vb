Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class CBSMemberReporting

#region "Variables"

    Protected mCBSMemberReportingID As long
    Protected mProvinceID As long
    Protected mDistrictID As long
    Protected mWard As long
    Protected mClubID As long
    Protected mYear As long
    Protected mMonth As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mChallenges As string
    Protected mRecommendations As string

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

    Public  Property CBSMemberReportingID() As long
        Get
		return mCBSMemberReportingID
        End Get
        Set(ByVal value As long)
		mCBSMemberReportingID = value
        End Set
    End Property

    Public  Property ProvinceID() As long
        Get
		return mProvinceID
        End Get
        Set(ByVal value As long)
		mProvinceID = value
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

    Public  Property Ward() As long
        Get
		return mWard
        End Get
        Set(ByVal value As long)
		mWard = value
        End Set
    End Property

    Public  Property ClubID() As long
        Get
		return mClubID
        End Get
        Set(ByVal value As long)
		mClubID = value
        End Set
    End Property

    Public  Property Year() As long
        Get
		return mYear
        End Get
        Set(ByVal value As long)
		mYear = value
        End Set
    End Property

    Public  Property Month() As long
        Get
		return mMonth
        End Get
        Set(ByVal value As long)
		mMonth = value
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

    Public  Property Challenges() As string
        Get
		return mChallenges
        End Get
        Set(ByVal value As string)
		mChallenges = value
        End Set
    End Property

    Public  Property Recommendations() As string
        Get
		return mRecommendations
        End Get
        Set(ByVal value As string)
		mRecommendations = value
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

    CBSMemberReportingID = 0
    mProvinceID = 0
    mDistrictID = 0
    mWard = 0
    mClubID = 0
    mYear = 0
    mMonth = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mChallenges = ""
    mRecommendations = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCBSMemberReportingID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CBSMemberReportingID As Long) As Boolean 

        Dim sql As String 

        If CBSMemberReportingID > 0 Then 
            sql = "SELECT * FROM tblCBSMemberReporting WHERE CBSMemberReportingID = " & CBSMemberReportingID
        Else 
            sql = "SELECT * FROM tblCBSMemberReporting WHERE CBSMemberReportingID = " & mCBSMemberReportingID
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

                log.Error("CBSMemberReporting not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCBSMemberReporting() As System.Data.DataSet

        Return GetCBSMemberReporting(mCBSMemberReportingID)

    End Function

    Public Overridable Function GetOneCBSMemberReporting(ByVal CBSMemberReportingID As Long) As DataSet

        Dim sql As String

        sql = "SELECT C.*, P.Name as Province, D.Name As District, W.Name As Ward1, G.GroupName As Club FROM tblCBSMemberReporting C "
        sql &= "Left outer join tblProvinces P on C.ProvinceID = P.ProvinceID "
        sql &= "Left outer join tblDistricts D on D.DistrictID = C.DistrictID "
        sql &= "Left outer join tblWards W on W.WardID = C.Ward "
        sql &= "Left outer join tblGroups G on G.GroupID = C.ClubID WHERE CBSMemberReportingID = " & CBSMemberReportingID

        Return GetCBSMemberReporting(sql)

    End Function

    Public Overridable Function GetCBSMemberReporting(ByVal CBSMemberReportingID As Long) As DataSet

        Dim sql As String

        If CBSMemberReportingID > 0 Then
            sql = "SELECT * FROM tblCBSMemberReporting WHERE CBSMemberReportingID = " & CBSMemberReportingID
        Else
            sql = "SELECT * FROM tblCBSMemberReporting WHERE CBSMemberReportingID = " & mCBSMemberReportingID
        End If

        Return GetCBSMemberReporting(sql)

    End Function

    Public Function GetAllMemberReporting() As DataSet

        Dim sql As String = "Select M.*, P.Name as Province, D.Name As District, W.Name as Ward2, G.GroupName as Club from tblCBSMemberReporting M "
        sql &= "left outer join tblProvinces P On M.ProvinceID = P.ProvinceID "
        sql &= "left outer join tblDistricts D on D.DistrictID = M.DistrictID "
        sql &= "left outer join tblWards W On W.WardID = M.Ward "
        sql &= "left outer join tblGroups G on G.GroupID = M.ClubID"

        Return GetCBSMemberReporting(sql)

    End Function

    Protected Overridable Function GetCBSMemberReporting(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mCBSMemberReportingID = Catchnull(.Item("CBSMemberReportingID"), 0)
            mProvinceID = Catchnull(.Item("ProvinceID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mWard = Catchnull(.Item("Ward"), 0)
            mClubID = Catchnull(.Item("ClubID"), 0)
            mYear = Catchnull(.Item("Year"), 0)
            mMonth = Catchnull(.Item("Month"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mChallenges = Catchnull(.Item("Challenges"), "")
            mRecommendations = Catchnull(.Item("Recommendations"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@CBSMemberReportingID", DbType.Int32, mCBSMemberReportingID)
        db.AddInParameter(cmd, "@ProvinceID", DbType.Int32, mProvinceID)
        db.AddInParameter(cmd, "@DistrictID", DbType.Int32, mDistrictID)
        db.AddInParameter(cmd, "@Ward", DbType.Int32, mWard)
        db.AddInParameter(cmd, "@ClubID", DbType.Int32, mClubID)
        db.AddInParameter(cmd, "@Year", DbType.Int32, mYear)
        db.AddInParameter(cmd, "@Month", DbType.Int32, mMonth)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Challenges", DbType.String, mChallenges)
        db.AddInParameter(cmd, "@Recommendations", DbType.String, mRecommendations)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_CBSMemberReporting")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mCBSMemberReportingID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblCBSMemberReporting SET Deleted = 1 WHERE CBSMemberReportingID = " & mCBSMemberReportingID) 
        Return Delete("DELETE FROM tblCBSMemberReporting WHERE CBSMemberReportingID = " & mCBSMemberReportingID)

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