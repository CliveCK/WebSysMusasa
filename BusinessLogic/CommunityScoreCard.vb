Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class CommunityScoreCard

#region "Variables"

    Protected mCommunityScoreCardID As long
    Protected mCommunityID As long
    Protected mIndicatorID As long
    Protected mThermaticAreaID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mScore As string

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

    Public  Property CommunityScoreCardID() As long
        Get
		return mCommunityScoreCardID
        End Get
        Set(ByVal value As long)
		mCommunityScoreCardID = value
        End Set
    End Property

    Public  Property CommunityID() As long
        Get
		return mCommunityID
        End Get
        Set(ByVal value As long)
		mCommunityID = value
        End Set
    End Property

    Public  Property IndicatorID() As long
        Get
		return mIndicatorID
        End Get
        Set(ByVal value As long)
		mIndicatorID = value
        End Set
    End Property

    Public  Property ThermaticAreaID() As long
        Get
		return mThermaticAreaID
        End Get
        Set(ByVal value As long)
		mThermaticAreaID = value
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

    Public Property Date1() As String
        Get
            Return mDate
        End Get
        Set(ByVal value As String)
            mDate = value
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

    CommunityScoreCardID = 0
    mCommunityID = 0
    mIndicatorID = 0
    mThermaticAreaID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mScore = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCommunityScoreCardID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CommunityID As Long) As Boolean

        Dim sql As String

        If CommunityScoreCardID > 0 Then
            sql = "SELECT CommunityScoreCardID, [Date], I.Name as Indicator, T.Description as Area, Score FROM tblCommunityScoreCard CC "
            sql &= "inner join luThermaticArea T on CC.ThermaticAreaID = T.ThermaticAreaID "
            sql &= "inner join tblIndicators I on I.IndicatorID = CC.IndicatorID WHERE CommunityID = " & CommunityID
        Else
            sql = "SELECT CommunityScoreCardID, [Date], I.Name as Indicator, T.Description as Area, Score FROM tblCommunityScoreCard CC "
            sql &= "inner join luThermaticArea T on CC.ThermaticAreaID = T.ThermaticAreaID "
            sql &= "inner join tblIndicators I on I.IndicatorID = CC.IndicatorID WHERE CommunityID = " & mCommunityID
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

                log.error("CommunityScoreCard not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCommunityScoreCard() As System.Data.DataSet

        Return GetCommunityScoreCard(mCommunityScoreCardID)

    End Function

    Public Overridable Function GetCommunityScoreCard(ByVal CommunityID As Long) As DataSet

        Dim sql As String

        If CommunityID > 0 Then
            sql = "SELECT SC.*, I.Name As Indicator, T.Description As Area FROM tblCommunityScoreCard SC "
            sql &= "left outer join tblIndicators I on I.IndicatorID = SC.IndicatorID "
            sql &= "left outer join luThermaticArea T on T.ThermaticAreaID = SC.ThermaticAreaID WHERE CommunityID = " & CommunityID
        Else
            sql = "SELECT SC.*, I.Name As Indicator, T.Description As Area FROM tblCommunityScoreCard SC "
            sql &= " left outer join tblIndicators I On I.IndicatorID = SC.IndicatorID "
            sql &= "left outer join luThermaticArea T On T.ThermaticAreaID = SC.ThermaticAreaID WHERE CommunityID = " & mCommunityID
        End If

        Return GetCommunityScoreCard(sql)

    End Function

    Protected Overridable Function GetCommunityScoreCard(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mCommunityScoreCardID = Catchnull(.Item("CommunityScoreCardID"), 0)
            mCommunityID = Catchnull(.Item("CommunityID"), 0)
            mIndicatorID = Catchnull(.Item("IndicatorID"), 0)
            mThermaticAreaID = Catchnull(.Item("ThermaticAreaID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDate = Catchnull(.Item("Date"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mScore = Catchnull(.Item("Score"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@CommunityScoreCardID", DBType.Int32, mCommunityScoreCardID)
        db.AddInParameter(cmd, "@CommunityID", DBType.Int32, mCommunityID)
        db.AddInParameter(cmd, "@IndicatorID", DBType.Int32, mIndicatorID)
        db.AddInParameter(cmd, "@ThermaticAreaID", DBType.Int32, mThermaticAreaID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Date", DBType.String, mDate)
        db.AddInParameter(cmd, "@Score", DBType.String, mScore)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_CommunityScoreCard")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mCommunityScoreCardID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblCommunityScoreCard Set Deleted = 1 WHERE CommunityScoreCardID = " & mCommunityScoreCardID) 
        Return Delete("DELETE FROM tblCommunityScoreCard WHERE CommunityScoreCardID = " & mCommunityScoreCardID)

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