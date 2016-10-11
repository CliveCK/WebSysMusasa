Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class OneStopCenterMemberNeedsServices

#region "Variables"

    Protected mOneStopCenterMemberNeedsServices As long
    Protected mMainOneStopCenterID As long
    Protected mTypeOfViolenceID As long
    Protected mAssistanceID As long
    Protected mReferredFromID As long
    Protected mReferredToID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As Long
    Protected mCreatedDate As String
    Protected mComments As String
    Protected mUpdatedDate As string

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

    Public  Property OneStopCenterMemberNeedsServices() As long
        Get
		return mOneStopCenterMemberNeedsServices
        End Get
        Set(ByVal value As long)
		mOneStopCenterMemberNeedsServices = value
        End Set
    End Property

    Public  Property MainOneStopCenterID() As long
        Get
		return mMainOneStopCenterID
        End Get
        Set(ByVal value As long)
		mMainOneStopCenterID = value
        End Set
    End Property

    Public  Property TypeOfViolenceID() As long
        Get
		return mTypeOfViolenceID
        End Get
        Set(ByVal value As long)
		mTypeOfViolenceID = value
        End Set
    End Property

    Public  Property AssistanceID() As long
        Get
		return mAssistanceID
        End Get
        Set(ByVal value As long)
		mAssistanceID = value
        End Set
    End Property

    Public  Property ReferredFromID() As long
        Get
		return mReferredFromID
        End Get
        Set(ByVal value As long)
		mReferredFromID = value
        End Set
    End Property

    Public Property ReferredToID() As Long
        Get
            Return mReferredToID
        End Get
        Set(ByVal value As Long)
            mReferredToID = value
        End Set
    End Property

    Public Property Comments() As String
        Get
            Return mComments
        End Get
        Set(ByVal value As String)
            mComments = value
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

    OneStopCenterMemberNeedsServices = 0
    mMainOneStopCenterID = 0
    mTypeOfViolenceID = 0
    mAssistanceID = 0
    mReferredFromID = 0
        mReferredToID = 0
        mComments = 0
        mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mOneStopCenterMemberNeedsServices) 

    End Function 

    Public Overridable Function Retrieve(ByVal OneStopCenterMemberNeedsServices As Long) As Boolean 

        Dim sql As String 

        If OneStopCenterMemberNeedsServices > 0 Then 
            sql = "SELECT * FROM tblOneStopCenterMemberNeedsServices WHERE OneStopCenterMemberNeedsServices = " & OneStopCenterMemberNeedsServices
        Else 
            sql = "SELECT * FROM tblOneStopCenterMemberNeedsServices WHERE OneStopCenterMemberNeedsServices = " & mOneStopCenterMemberNeedsServices
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

                log.Error("OneStopCenterMemberNeedsServices not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetOneStopCenterMemberNeedsServices() As System.Data.DataSet

        Return GetOneStopCenterMemberNeedsServices(mOneStopCenterMemberNeedsServices)

    End Function

    Public Overridable Function GetOneStopCenterMemberNeedsServices(ByVal OneStopCenterMemberNeedsServices As Long) As DataSet

        Dim sql As String

        If OneStopCenterMemberNeedsServices > 0 Then
            sql = "SELECT * FROM tblOneStopCenterMemberNeedsServices WHERE OneStopCenterMemberNeedsServices = " & OneStopCenterMemberNeedsServices
        Else
            sql = "SELECT * FROM tblOneStopCenterMemberNeedsServices WHERE OneStopCenterMemberNeedsServices = " & mOneStopCenterMemberNeedsServices
        End If

        Return GetOneStopCenterMemberNeedsServices(sql)

    End Function

    Public Overridable Function GetAllOneStopCenterMemberNeedsServices(ByVal MainOneStopCenterID As Long) As DataSet

        Dim sql As String

        sql = "SELECT *, T.Description as TypeOfViolence, A.Description as AssistanceProvided, F.Description as ReferredFrom, T1.Description As ReferredTo, Comments "
        sql &= "From tblOneStopCenterMemberNeedsServices S left outer join luTypesOfViolence T on S.TypeOfViolenceID = T.TypesOfViolenceID "
            sql &= "Left outer join luAssistenceAndServicesProvided A on A.AssistenceAndServicesID = S.AssistanceID "
            sql &= "Left outer join luReferralCentreTypes F on F.ReferralCentreTypeID = S.ReferredFromID "
        sql &= "Left outer join luReferralCentreTypes T1 on T1.ReferralCentreTypeID = S.ReferredFromID where MainOneStopCenterID = " & MainOneStopCenterID


        Return GetOneStopCenterMemberNeedsServices(sql)

    End Function

    Protected Overridable Function GetOneStopCenterMemberNeedsServices(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mOneStopCenterMemberNeedsServices = Catchnull(.Item("OneStopCenterMemberNeedsServices"), 0)
            mMainOneStopCenterID = Catchnull(.Item("MainOneStopCenterID"), 0)
            mTypeOfViolenceID = Catchnull(.Item("TypeOfViolenceID"), 0)
            mAssistanceID = Catchnull(.Item("AssistanceID"), 0)
            mReferredFromID = Catchnull(.Item("ReferredFromID"), 0)
            mReferredToID = Catchnull(.Item("ReferredToID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mComments = Catchnull(.Item("Comments"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@OneStopCenterMemberNeedsServices", DbType.Int32, mOneStopCenterMemberNeedsServices)
        db.AddInParameter(cmd, "@MainOneStopCenterID", DbType.Int32, mMainOneStopCenterID)
        db.AddInParameter(cmd, "@TypeOfViolenceID", DbType.Int32, mTypeOfViolenceID)
        db.AddInParameter(cmd, "@AssistanceID", DbType.Int32, mAssistanceID)
        db.AddInParameter(cmd, "@ReferredFromID", DbType.Int32, mReferredFromID)
        db.AddInParameter(cmd, "@ReferredToID", DbType.Int32, mReferredToID)
        db.AddInParameter(cmd, "@Comments", DbType.String, mComments)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_OneStopCenterMemberNeedsServices")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mOneStopCenterMemberNeedsServices = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblOneStopCenterMemberNeedsServices SET Deleted = 1 WHERE OneStopCenterMemberNeedsServices = " & mOneStopCenterMemberNeedsServices) 
        Return Delete("DELETE FROM tblOneStopCenterMemberNeedsServices WHERE OneStopCenterMemberNeedsServices = " & mOneStopCenterMemberNeedsServices)

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