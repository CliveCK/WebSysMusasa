Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GroupMaturityIndex

#region "Variables"

    Protected mGroupMaturityIndexID As long
    Protected mGroupID As long
    Protected mKeyAreaID As long
    Protected mMonthID As long
    Protected mYear As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mScore As string

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

    Public  Property GroupMaturityIndexID() As long
        Get
		return mGroupMaturityIndexID
        End Get
        Set(ByVal value As long)
		mGroupMaturityIndexID = value
        End Set
    End Property

    Public  Property GroupID() As long
        Get
		return mGroupID
        End Get
        Set(ByVal value As long)
		mGroupID = value
        End Set
    End Property

    Public  Property KeyAreaID() As long
        Get
		return mKeyAreaID
        End Get
        Set(ByVal value As long)
		mKeyAreaID = value
        End Set
    End Property

    Public  Property MonthID() As long
        Get
		return mMonthID
        End Get
        Set(ByVal value As long)
		mMonthID = value
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

    Public  Property Score() As string
        Get
		return mScore
        End Get
        Set(ByVal value As string)
		mScore = value
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

    GroupMaturityIndexID = 0
    mGroupID = 0
    mKeyAreaID = 0
    mMonthID = 0
    mYear = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mScore = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGroupMaturityIndexID) 

    End Function 

    Public Overridable Function Retrieve(ByVal GroupMaturityIndexID As Long) As Boolean 

        Dim sql As String 

        If GroupMaturityIndexID > 0 Then 
            sql = "SELECT * FROM tblGroupMaturityIndex WHERE GroupMaturityIndexID = " & GroupMaturityIndexID
        Else 
            sql = "SELECT * FROM tblGroupMaturityIndex WHERE GroupMaturityIndexID = " & mGroupMaturityIndexID
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

                log.Error("GroupMaturityIndex not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGroupMaturityIndex() As System.Data.DataSet

        Return GetGroupMaturityIndex(mGroupMaturityIndexID)

    End Function

    Public Overridable Function GetGroupMaturityIndex(ByVal GroupID As Long) As DataSet

        Dim sql As String

        If GroupID > 0 Then
            sql = "SELECT G.GroupMaturityIndexID, M.Description as [Month], [Year], A.Description as MaturityArea, Score FROM tblGroupMaturityIndex G "
            sql &= "left outer join luMaturityArea A On G.KeyAreaID = A.MaturityAreaID "
            sql &= "left outer join luMonths M on M.MonthID = G.MonthID WHERE GroupID = " & GroupID
        Else
            sql = "SELECT G.GroupMaturityIndexID, M.Description as [Month], [Year], A.Description as MaturityArea, Score FROM tblGroupMaturityIndex G "
            sql &= "left outer join luMaturityArea A On G.KeyAreaID = A.MaturityAreaID "
            sql &= "left outer join luMonths M on M.MonthID = G.MonthID WHERE GroupID = " & mGroupID
        End If

        Return GetGroupMaturityIndex(sql)

    End Function

    Protected Overridable Function GetGroupMaturityIndex(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGroupMaturityIndexID = Catchnull(.Item("GroupMaturityIndexID"), 0)
            mGroupID = Catchnull(.Item("GroupID"), 0)
            mKeyAreaID = Catchnull(.Item("KeyAreaID"), 0)
            mMonthID = Catchnull(.Item("MonthID"), 0)
            mYear = Catchnull(.Item("Year"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mScore = Catchnull(.Item("Score"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GroupMaturityIndexID", DbType.Int32, mGroupMaturityIndexID)
        db.AddInParameter(cmd, "@GroupID", DbType.Int32, mGroupID)
        db.AddInParameter(cmd, "@KeyAreaID", DbType.Int32, mKeyAreaID)
        db.AddInParameter(cmd, "@MonthID", DbType.Int32, mMonthID)
        db.AddInParameter(cmd, "@Year", DbType.Int32, mYear)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Score", DbType.String, mScore)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GroupMaturityIndex")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGroupMaturityIndexID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGroupMaturityIndex SET Deleted = 1 WHERE GroupMaturityIndexID = " & mGroupMaturityIndexID) 
        Return Delete("DELETE FROM tblGroupMaturityIndex WHERE GroupMaturityIndexID = " & mGroupMaturityIndexID)

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