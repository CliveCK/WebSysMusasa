Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Indiactor

#region "Variables"

    Protected mIndicatorID As long
    Protected mIndicatorType As long
    Protected mOutputID As Long
    Protected mOutcomeID As Long
    Protected mObjectiveID As Long
    Protected mImpactID As Long
    Protected mActivityID As Long
    Protected mOrganizationID As Long
    Protected mDistrictID As Long
    Protected mUnitOfMeasurement As long
    Protected mBaselineValue As long
    Protected mDataSource As long
    Protected mTool As long
    Protected mDataCollectionFrequency As long
    Protected mName As string
    Protected mDefinition As string
    Protected mDescription As string
    Protected mDataCollectionMethod As string
    Protected mResponsibleParty As string
    Protected mProgramTargetValue As String
    Protected mYear As Integer
    Protected mMonth As Integer
    Protected mTarget As Double
    Protected mAchievement As Double
    Protected mComment As String

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected db As Database 
    Protected mConnectionName As String 
    Protected mObjectUserID As Long 

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

    Public  Property IndicatorID() As long
        Get
		return mIndicatorID
        End Get
        Set(ByVal value As long)
		mIndicatorID = value
        End Set
    End Property

    Public  Property IndicatorType() As long
        Get
		return mIndicatorType
        End Get
        Set(ByVal value As long)
		mIndicatorType = value
        End Set
    End Property

    Public Property OrganizationID() As Long
        Get
            Return mOrganizationID
        End Get
        Set(ByVal value As Long)
            mOrganizationID = value
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

    Public  Property OutputID() As long
        Get
		return mOutputID
        End Get
        Set(ByVal value As long)
		mOutputID = value
        End Set
    End Property

    Public Property OutcomeID() As Long
        Get
            Return mOutcomeID
        End Get
        Set(ByVal value As Long)
            mOutcomeID = value
        End Set
    End Property

    Public Property ObjectiveID() As Long
        Get
            Return mObjectiveID
        End Get
        Set(ByVal value As Long)
            mObjectiveID = value
        End Set
    End Property

    Public Property ImpactID() As Long
        Get
            Return mImpactID
        End Get
        Set(ByVal value As Long)
            mImpactID = value
        End Set
    End Property

    Public  Property ActivityID() As long
        Get
		return mActivityID
        End Get
        Set(ByVal value As long)
		mActivityID = value
        End Set
    End Property

    Public  Property UnitOfMeasurement() As long
        Get
		return mUnitOfMeasurement
        End Get
        Set(ByVal value As long)
		mUnitOfMeasurement = value
        End Set
    End Property

    Public  Property BaselineValue() As long
        Get
		return mBaselineValue
        End Get
        Set(ByVal value As long)
		mBaselineValue = value
        End Set
    End Property

    Public  Property DataSource() As long
        Get
		return mDataSource
        End Get
        Set(ByVal value As long)
		mDataSource = value
        End Set
    End Property

    Public  Property Tool() As long
        Get
		return mTool
        End Get
        Set(ByVal value As long)
		mTool = value
        End Set
    End Property

    Public  Property DataCollectionFrequency() As long
        Get
		return mDataCollectionFrequency
        End Get
        Set(ByVal value As long)
		mDataCollectionFrequency = value
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

    Public  Property Definition() As string
        Get
		return mDefinition
        End Get
        Set(ByVal value As string)
		mDefinition = value
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

    Public  Property DataCollectionMethod() As string
        Get
		return mDataCollectionMethod
        End Get
        Set(ByVal value As string)
		mDataCollectionMethod = value
        End Set
    End Property

    Public  Property ResponsibleParty() As string
        Get
		return mResponsibleParty
        End Get
        Set(ByVal value As string)
		mResponsibleParty = value
        End Set
    End Property

    Public  Property ProgramTargetValue() As string
        Get
		return mProgramTargetValue
        End Get
        Set(ByVal value As string)
		mProgramTargetValue = value
        End Set
    End Property

    Public Property Year() As Integer
        Get
            Return mYear
        End Get
        Set(ByVal value As Integer)
            mYear = value
        End Set
    End Property

    Public Property Month() As Integer
        Get
            Return mMonth
        End Get
        Set(ByVal value As Integer)
            mMonth = value
        End Set
    End Property

    Public Property Target() As Double
        Get
            Return mTarget
        End Get
        Set(ByVal value As Double)
            mTarget = value
        End Set
    End Property
    Public Property Achievement() As Double
        Get
            Return mAchievement
        End Get
        Set(ByVal value As Double)
            mAchievement = value
        End Set
    End Property

    Public Property Comment() As String
        Get
            Return mComment
        End Get
        Set(ByVal value As String)
            mComment = value
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

        IndicatorID = 0
    mIndicatorType = 0
    mOutputID = 0
    mOutcomeID = 0
    mActivityID = 0
    mUnitOfMeasurement = 0
        mBaselineValue = 0
        mOrganizationID = 0
        mDistrictID = 0
        mObjectiveID = 0
        mImpactID = 0
        mDataSource = 0
    mTool = 0
    mDataCollectionFrequency = 0
    mName = ""
    mDefinition = ""
    mDescription = ""
    mDataCollectionMethod = ""
    mResponsibleParty = ""
    mProgramTargetValue = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mIndicatorID) 

    End Function 

    Public Overridable Function Retrieve(ByVal IndicatorID As Long) As Boolean 

        Dim sql As String 

        If IndicatorID > 0 Then 
            sql = "SELECT * FROM tblIndicators WHERE IndicatorID = " & IndicatorID
        Else 
            sql = "SELECT * FROM tblIndicators WHERE IndicatorID = " & mIndicatorID
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

                log.Warn("Indiactor not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetIndiactor() As System.Data.DataSet

        Return GetIndiactor(mIndicatorID) 

    End Function 

    Public Overridable Function GetIndiactor(ByVal IndicatorID As Long) As DataSet 

        Dim sql As String 

        If IndicatorID > 0 Then 
            sql = "SELECT * FROM tblIndicators WHERE IndicatorID = " & IndicatorID
        Else 
            sql = "SELECT * FROM tblIndicators WHERE IndicatorID = " & mIndicatorID
        End If 

        Return GetIndiactor(sql) 

    End Function

    Public Function GetTracking() As DataSet

        Dim objOrganization As New Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim Criteria = IIf(CookiesWrapper.OrganizationID = objOrganization.GetParentOrganizationID(), "", " WHERE I.CreatedBy not in (" & objOrganization.GetPermittedUsersByOrganization(CookiesWrapper.OrganizationID) & ")")

        Dim sql As String = "Select IndicatorTrackingID ,IT.Description As IndicatorType, Definition, I.Description, [Year], luMonths.Description As [Month], [Target], Achievement, Comments from tblIndicators I inner join tblIndicatorTracking T"
        sql &= " on I.IndicatorID =T.IndicatorID inner join luMonths on luMonths.MonthID = T.[Month]"
        sql &= "  inner join luIndicatorType IT on IT.IndicatorTypeID = I.IndicatorType " & Criteria

        Return GetIndiactor(sql)

    End Function
    Public Function GetAllIndicators() As DataSet

        Dim objOrganization As New Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim Criteria = IIf(CookiesWrapper.OrganizationID = objOrganization.GetParentOrganizationID(), "", " WHERE CreatedBy not in (" & objOrganization.GetPermittedUsersByOrganization(CookiesWrapper.OrganizationID) & ")")
        Dim sql As String = "SELECT * FROM tblIndicators" & Criteria

        Return GetIndiactor(sql)

    End Function

    Protected Overridable Function GetIndiactor(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function 

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mIndicatorID = Catchnull(.Item("IndicatorID"), 0)
            mIndicatorType = Catchnull(.Item("IndicatorType"), 0)
            mOutputID = Catchnull(.Item("OutputID"), 0)
            mOutcomeID = Catchnull(.Item("OutcomeID"), 0)
            mObjectiveID = Catchnull(.Item("ObjectiveID"), 0)
            mImpactID = Catchnull(.Item("ImpactID"), 0)
            mActivityID = Catchnull(.Item("ActivityID"), 0)
            mUnitOfMeasurement = Catchnull(.Item("UnitOfMeasurement"), 0)
            mBaselineValue = Catchnull(.Item("BaselineValue"), 0)
            mDataSource = Catchnull(.Item("DataSource"), 0)
            mTool = Catchnull(.Item("Tool"), 0)
            mDataCollectionFrequency = Catchnull(.Item("DataCollectionFrequency"), 0)
            mName = Catchnull(.Item("Name"), "")
            mDefinition = Catchnull(.Item("Definition"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mDataCollectionMethod = Catchnull(.Item("DataCollectionMethod"), "")
            mResponsibleParty = Catchnull(.Item("ResponsibleParty"), "")
            mProgramTargetValue = Catchnull(.Item("ProgramTargetValue"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@IndicatorID", DBType.Int32, mIndicatorID)
        db.AddInParameter(cmd, "@IndicatorType", DbType.Int32, mIndicatorType)
        db.AddInParameter(cmd, "@ImpactID", DbType.Int32, mImpactID)
        db.AddInParameter(cmd, "@OutputID", DBType.Int32, mOutputID) 
        db.AddInParameter(cmd, "@OutcomeID", DBType.Int32, mOutcomeID) 
        db.AddInParameter(cmd, "@ActivityID", DBType.Int32, mActivityID) 
        db.AddInParameter(cmd, "@UnitOfMeasurement", DBType.Int32, mUnitOfMeasurement) 
        db.AddInParameter(cmd, "@BaselineValue", DBType.Int32, mBaselineValue) 
        db.AddInParameter(cmd, "@DataSource", DBType.Int32, mDataSource) 
        db.AddInParameter(cmd, "@Tool", DBType.Int32, mTool) 
        db.AddInParameter(cmd, "@DataCollectionFrequency", DBType.Int32, mDataCollectionFrequency) 
        db.AddInParameter(cmd, "@Name", DBType.String, mName) 
        db.AddInParameter(cmd, "@Definition", DBType.String, mDefinition) 
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription) 
        db.AddInParameter(cmd, "@DataCollectionMethod", DBType.String, mDataCollectionMethod) 
        db.AddInParameter(cmd, "@ResponsibleParty", DBType.String, mResponsibleParty)
        db.AddInParameter(cmd, "@ProgramTargetValue", DbType.String, mProgramTargetValue)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.String, mObjectUserID)

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Indiactor") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mIndicatorID = ds.Tables(0).Rows(0)(0) 

            End If 

            Return True 

        Catch ex As Exception 

            log.Error(ex)
            Return False 

        End Try 

    End Function

    Public Function SaveTracking(ByVal IndicatorTrackingID As Long) As Boolean

        Dim sql As String = ""

        Try

            If IndicatorTrackingID > 0 Then

                sql = "UPDATE tblIndicatorTracking SET [Achievement] = " & mAchievement & ", [Comments] = '" & mComment & "' WHERE IndicatorTrackingID = " & IndicatorTrackingID

            Else

                sql = "INSERT INTO tblIndicatorTracking ([IndicatorID],[Year],[Month],[Target],[OrganizationID],[DistrictID]) VALUES (" & mIndicatorID & "," & mYear & "," & mMonth & "," & mTarget & "," & mOrganizationID & "," & mDistrictID & ")"

            End If

            db.ExecuteNonQuery(CommandType.Text, sql)

            Return True

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

#End Region 

#Region "Delete" 

    Public Overridable Function Delete() As Boolean 

        'Return Delete("UPDATE tblIndicators SET Deleted = 1 WHERE IndicatorID = " & mIndicatorID) 
        Return Delete("DELETE FROM tblIndicators WHERE IndicatorID = " & mIndicatorID) 

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