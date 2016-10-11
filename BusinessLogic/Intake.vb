Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Intake

#region "Variables"

    Protected mIntakeID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mStartDate As string
    Protected mEndDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mDescription As string

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

    Public  Property IntakeID() As long
        Get
		return mIntakeID
        End Get
        Set(ByVal value As long)
		mIntakeID = value
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

    Public  Property StartDate() As string
        Get
		return mStartDate
        End Get
        Set(ByVal value As string)
		mStartDate = value
        End Set
    End Property

    Public  Property EndDate() As string
        Get
		return mEndDate
        End Get
        Set(ByVal value As string)
		mEndDate = value
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

    Public  Property Description() As string
        Get
		return mDescription
        End Get
        Set(ByVal value As string)
		mDescription = value
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

    IntakeID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mStartDate = ""
    mEndDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mDescription = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mIntakeID) 

    End Function 

    Public Overridable Function Retrieve(ByVal IntakeID As Long) As Boolean 

        Dim sql As String 

        If IntakeID > 0 Then 
            sql = "SELECT * FROM tblIntake WHERE IntakeID = " & IntakeID
        Else 
            sql = "SELECT * FROM tblIntake WHERE IntakeID = " & mIntakeID
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

                Log.Error("Intake not found.")

                Return False

            End If

        Catch e As Exception

            Log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetIntake() As System.Data.DataSet

        Return GetIntake(mIntakeID)

    End Function

    Public Overridable Function GetIntake(ByVal IntakeID As Long) As DataSet

        Dim sql As String

        If IntakeID > 0 Then
            sql = "SELECT * FROM tblIntake WHERE IntakeID = " & IntakeID
        Else
            sql = "SELECT * FROM tblIntake WHERE IntakeID = " & mIntakeID
        End If

        Return GetIntake(sql)

    End Function

    Protected Overridable Function GetIntake(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mIntakeID = Catchnull(.Item("IntakeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mStartDate = Catchnull(.Item("StartDate"), "")
            mEndDate = Catchnull(.Item("EndDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mDescription = Catchnull(.Item("Description"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@IntakeID", DbType.Int32, mIntakeID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@StartDate", DbType.String, mStartDate)
        db.AddInParameter(cmd, "@EndDate", DbType.String, mEndDate)
        db.AddInParameter(cmd, "@Description", DbType.String, mDescription)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Intake")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mIntakeID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblIntake SET Deleted = 1 WHERE IntakeID = " & mIntakeID) 
        Return Delete("DELETE FROM tblIntake WHERE IntakeID = " & mIntakeID)

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