Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class InterventionImplementingPartner

#region "Variables"

    Protected mInterventionImplementingPartner As long
    Protected mInterventionID As long
    Protected mImplementingPartnerID As long
    Protected mDistrictID As long
    Protected mCityID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mUpdatedDate As String
    Protected mCreatedDate As string
    Protected mIsUrban As boolean

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

    Public  Property InterventionImplementingPartner() As long
        Get
		return mInterventionImplementingPartner
        End Get
        Set(ByVal value As long)
		mInterventionImplementingPartner = value
        End Set
    End Property

    Public  Property InterventionID() As long
        Get
		return mInterventionID
        End Get
        Set(ByVal value As long)
		mInterventionID = value
        End Set
    End Property

    Public  Property ImplementingPartnerID() As long
        Get
		return mImplementingPartnerID
        End Get
        Set(ByVal value As long)
		mImplementingPartnerID = value
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

    Public  Property CityID() As long
        Get
		return mCityID
        End Get
        Set(ByVal value As long)
		mCityID = value
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

    Public Property UpdatedDate() As String
        Get
            Return mUpdatedDate
        End Get
        Set(ByVal value As String)
            mUpdatedDate = value
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

    Public  Property IsUrban() As boolean
        Get
		return mIsUrban
        End Get
        Set(ByVal value As boolean)
		mIsUrban = value
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

    InterventionImplementingPartner = 0
    mInterventionID = 0
    mImplementingPartnerID = 0
    mDistrictID = 0
    mCityID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
        mUpdatedDate = ""
    mCreatedDate = ""
    mIsUrban = FALSE

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mInterventionImplementingPartner) 

    End Function 

    Public Overridable Function Retrieve(ByVal InterventionImplementingPartner As Long) As Boolean 

        Dim sql As String 

        If InterventionImplementingPartner > 0 Then 
            sql = "SELECT * FROM tblInterventionImplementingPartner WHERE InterventionImplementingPartner = " & InterventionImplementingPartner
        Else 
            sql = "SELECT * FROM tblInterventionImplementingPartner WHERE InterventionImplementingPartner = " & mInterventionImplementingPartner
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

                log.Warn("InterventionImplementingPartner not found.")

                Return False 

            End If 

        Catch e As Exception 

            Log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetInterventionImplementingPartner() As System.Data.DataSet

        Return GetInterventionImplementingPartner(mInterventionImplementingPartner)

    End Function

    Public Overridable Function GetInterventionImplementingPartner(ByVal InterventionImplementingPartner As Long) As DataSet

        Dim sql As String

        If InterventionImplementingPartner > 0 Then
            sql = "SELECT * FROM tblInterventionImplementingPartner WHERE InterventionImplementingPartner = " & InterventionImplementingPartner
        Else
            sql = "SELECT * FROM tblInterventionImplementingPartner WHERE InterventionImplementingPartner = " & mInterventionImplementingPartner
        End If

        Return GetInterventionImplementingPartner(sql)

    End Function

    Protected Overridable Function GetInterventionImplementingPartner(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mInterventionImplementingPartner = Catchnull(.Item("InterventionImplementingPartner"), 0)
            mInterventionID = Catchnull(.Item("InterventionID"), 0)
            mImplementingPartnerID = Catchnull(.Item("ImplementingPartnerID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mCityID = Catchnull(.Item("CityID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mIsUrban = Catchnull(.Item("IsUrban"), False)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@InterventionImplementingPartner", DBType.Int32, mInterventionImplementingPartner)
        db.AddInParameter(cmd, "@InterventionID", DBType.Int32, mInterventionID)
        db.AddInParameter(cmd, "@ImplementingPartnerID", DBType.Int32, mImplementingPartnerID)
        db.AddInParameter(cmd, "@DistrictID", DBType.Int32, mDistrictID)
        db.AddInParameter(cmd, "@CityID", DBType.Int32, mCityID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@IsUrban", DBType.Boolean, mIsUrban)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_InterventionImplementingPartner")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mInterventionImplementingPartner = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            Log.Error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblInterventionImplementingPartner SET Deleted = 1 WHERE InterventionImplementingPartner = " & mInterventionImplementingPartner) 
        Return Delete("DELETE FROM tblInterventionImplementingPartner WHERE InterventionImplementingPartner = " & mInterventionImplementingPartner)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            Log.Error(e)
            Return False

        End Try

    End Function

#End Region

#end region

End Class