﻿Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GeneralActivity

#region "Variables"

    Protected mGeneralActivityID As long
    Protected mActivityTypeID As Long
    Protected mActivityDate As String
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mName As string
    Protected mDescription As string
    Protected mLocation As string
    Protected mFacilitator As string

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

    Public  Property GeneralActivityID() As long
        Get
		return mGeneralActivityID
        End Get
        Set(ByVal value As long)
		mGeneralActivityID = value
        End Set
    End Property

    Public  Property ActivityTypeID() As long
        Get
		return mActivityTypeID
        End Get
        Set(ByVal value As long)
		mActivityTypeID = value
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

    Public Property ActivityDate() As String
        Get
            Return mActivityDate
        End Get
        Set(ByVal value As String)
            mActivityDate = value
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

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
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

    Public  Property Location() As string
        Get
		return mLocation
        End Get
        Set(ByVal value As string)
		mLocation = value
        End Set
    End Property

    Public  Property Facilitator() As string
        Get
		return mFacilitator
        End Get
        Set(ByVal value As string)
		mFacilitator = value
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

    GeneralActivityID = 0
    mActivityTypeID = 0
    mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mActivityDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mName = ""
    mDescription = ""
    mLocation = ""
    mFacilitator = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGeneralActivityID) 

    End Function 

    Public Overridable Function Retrieve(ByVal GeneralActivityID As Long) As Boolean 

        Dim sql As String 

        If GeneralActivityID > 0 Then 
            sql = "SELECT * FROM tblGeneralActivities WHERE GeneralActivityID = " & GeneralActivityID
        Else 
            sql = "SELECT * FROM tblGeneralActivities WHERE GeneralActivityID = " & mGeneralActivityID
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

                log.error("GeneralActivity not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGeneralActivity() As System.Data.DataSet

        Return GetGeneralActivity(mGeneralActivityID)

    End Function

    Public Overridable Function GetGeneralActivity(ByVal GeneralActivityID As Long) As DataSet

        Dim sql As String

        If GeneralActivityID > 0 Then
            sql = "SELECT * FROM tblGeneralActivities WHERE GeneralActivityID = " & GeneralActivityID
        Else
            sql = "SELECT * FROM tblGeneralActivities WHERE GeneralActivityID = " & mGeneralActivityID
        End If

        Return GetGeneralActivity(sql)

    End Function

    Public Overridable Function GetGeneralActivity(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGeneralActivityID = Catchnull(.Item("GeneralActivityID"), 0)
            mActivityTypeID = Catchnull(.Item("ActivityTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mName = Catchnull(.Item("Name"), "")
            mActivityDate = Catchnull(.Item("ActivityDate"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mLocation = Catchnull(.Item("Location"), "")
            mFacilitator = Catchnull(.Item("Facilitator"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GeneralActivityID", DBType.Int32, mGeneralActivityID)
        db.AddInParameter(cmd, "@ActivityTypeID", DBType.Int32, mActivityTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Name", DbType.String, mName)
        db.AddInParameter(cmd, "@ActivityDate", DbType.String, mActivityDate)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)
        db.AddInParameter(cmd, "@Location", DBType.String, mLocation)
        db.AddInParameter(cmd, "@Facilitator", DBType.String, mFacilitator)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GeneralActivity")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGeneralActivityID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGeneralActivities SET Deleted = 1 WHERE GeneralActivityID = " & mGeneralActivityID) 
        Return Delete("DELETE FROM tblGeneralActivities WHERE GeneralActivityID = " & mGeneralActivityID)

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