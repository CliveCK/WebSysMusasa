Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GeneralActivityAttendants

#region "Variables"

    Protected mGeneralActivityAttendantID As long
    Protected mGeneralActivityID As long
    Protected mAttendantTypeID As long
    Protected mAttendantID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

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

    Public  Property GeneralActivityAttendantID() As long
        Get
		return mGeneralActivityAttendantID
        End Get
        Set(ByVal value As long)
		mGeneralActivityAttendantID = value
        End Set
    End Property

    Public  Property GeneralActivityID() As long
        Get
		return mGeneralActivityID
        End Get
        Set(ByVal value As long)
		mGeneralActivityID = value
        End Set
    End Property

    Public  Property AttendantTypeID() As long
        Get
		return mAttendantTypeID
        End Get
        Set(ByVal value As long)
		mAttendantTypeID = value
        End Set
    End Property

    Public  Property AttendantID() As long
        Get
		return mAttendantID
        End Get
        Set(ByVal value As long)
		mAttendantID = value
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

    GeneralActivityAttendantID = 0
    mGeneralActivityID = 0
    mAttendantTypeID = 0
    mAttendantID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGeneralActivityAttendantID) 

    End Function 

    Public Overridable Function Retrieve(ByVal GeneralActivityAttendantID As Long) As Boolean 

        Dim sql As String 

        If GeneralActivityAttendantID > 0 Then 
            sql = "SELECT * FROM tblGeneralActivityAttendants WHERE GeneralActivityAttendantID = " & GeneralActivityAttendantID
        Else 
            sql = "SELECT * FROM tblGeneralActivityAttendants WHERE GeneralActivityAttendantID = " & mGeneralActivityAttendantID
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

                log.error("GeneralActivityAttendants not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGeneralActivityAttendants() As System.Data.DataSet

        Return GetGeneralActivityAttendants(mGeneralActivityAttendantID)

    End Function

    Public Overridable Function GetGeneralActivityAttendants(ByVal GeneralActivityAttendantID As Long) As DataSet

        Dim sql As String

        If GeneralActivityAttendantID > 0 Then
            sql = "SELECT * FROM tblGeneralActivityAttendants WHERE GeneralActivityAttendantID = " & GeneralActivityAttendantID
        Else
            sql = "SELECT * FROM tblGeneralActivityAttendants WHERE GeneralActivityAttendantID = " & mGeneralActivityAttendantID
        End If

        Return GetGeneralActivityAttendants(sql)

    End Function

    Protected Overridable Function GetGeneralActivityAttendants(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGeneralActivityAttendantID = Catchnull(.Item("GeneralActivityAttendantID"), 0)
            mGeneralActivityID = Catchnull(.Item("GeneralActivityID"), 0)
            mAttendantTypeID = Catchnull(.Item("AttendantTypeID"), 0)
            mAttendantID = Catchnull(.Item("AttendantID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GeneralActivityAttendantID", DBType.Int32, mGeneralActivityAttendantID)
        db.AddInParameter(cmd, "@GeneralActivityID", DBType.Int32, mGeneralActivityID)
        db.AddInParameter(cmd, "@AttendantTypeID", DBType.Int32, mAttendantTypeID)
        db.AddInParameter(cmd, "@AttendantID", DBType.Int32, mAttendantID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GeneralActivityAttendants")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGeneralActivityAttendantID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGeneralActivityAttendants SET Deleted = 1 WHERE GeneralActivityAttendantID = " & mGeneralActivityAttendantID) 
        Return Delete("DELETE FROM tblGeneralActivityAttendants WHERE GeneralActivityAttendantID = " & mGeneralActivityAttendantID)

    End Function

    Public Function DeleteEntries() As Boolean

        Return Delete("DELETE FROM tblGeneralActivityAttendants WHERE AttendantID = " & mAttendantID & " AND GeneralActivityID = " & mGeneralActivityID)

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