Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Evaluation

#region "Variables"

    Protected mEvaluationID As long
    Protected mTypeOfEvaluationID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mFromDate As string
    Protected mToDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mStatus As boolean
    Protected mName As string
    Protected mDescription As string
    Protected mComments As string

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

    Public  Property EvaluationID() As long
        Get
		return mEvaluationID
        End Get
        Set(ByVal value As long)
		mEvaluationID = value
        End Set
    End Property

    Public  Property TypeOfEvaluationID() As long
        Get
		return mTypeOfEvaluationID
        End Get
        Set(ByVal value As long)
		mTypeOfEvaluationID = value
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

    Public  Property FromDate() As string
        Get
		return mFromDate
        End Get
        Set(ByVal value As string)
		mFromDate = value
        End Set
    End Property

    Public  Property ToDate() As string
        Get
		return mToDate
        End Get
        Set(ByVal value As string)
		mToDate = value
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

    Public  Property Status() As boolean
        Get
		return mStatus
        End Get
        Set(ByVal value As boolean)
		mStatus = value
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

    Public  Property Comments() As string
        Get
		return mComments
        End Get
        Set(ByVal value As string)
		mComments = value
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

    EvaluationID = 0
    mTypeOfEvaluationID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mFromDate = ""
    mToDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mStatus = FALSE
    mName = ""
    mDescription = ""
    mComments = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mEvaluationID) 

    End Function 

    Public Overridable Function Retrieve(ByVal EvaluationID As Long) As Boolean 

        Dim sql As String 

        If EvaluationID > 0 Then 
            sql = "SELECT * FROM tblEvaluations WHERE EvaluationID = " & EvaluationID
        Else 
            sql = "SELECT * FROM tblEvaluations WHERE EvaluationID = " & mEvaluationID
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

                log.Error("Evaluation not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetEvaluation() As System.Data.DataSet

        Return GetEvaluation(mEvaluationID)

    End Function

    Public Overridable Function GetEvaluation(ByVal EvaluationID As Long) As DataSet

        Dim sql As String

        If EvaluationID > 0 Then
            sql = "SELECT * FROM tblEvaluations WHERE EvaluationID = " & EvaluationID
        Else
            sql = "SELECT * FROM tblEvaluations WHERE EvaluationID = " & mEvaluationID
        End If

        Return GetEvaluation(sql)

    End Function

    Public Overridable Function GetEvaluation(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mEvaluationID = Catchnull(.Item("EvaluationID"), 0)
            mTypeOfEvaluationID = Catchnull(.Item("TypeOfEvaluationID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mFromDate = Catchnull(.Item("FromDate"), "")
            mToDate = Catchnull(.Item("ToDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mStatus = Catchnull(.Item("Status"), False)
            mName = Catchnull(.Item("Name"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mComments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@EvaluationID", DBType.Int32, mEvaluationID)
        db.AddInParameter(cmd, "@TypeOfEvaluationID", DBType.Int32, mTypeOfEvaluationID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@FromDate", DBType.String, mFromDate)
        db.AddInParameter(cmd, "@ToDate", DBType.String, mToDate)
        db.AddInParameter(cmd, "@Status", DBType.Boolean, mStatus)
        db.AddInParameter(cmd, "@Name", DBType.String, mName)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)
        db.AddInParameter(cmd, "@Comments", DBType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Evaluation")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mEvaluationID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblEvaluations SET Deleted = 1 WHERE EvaluationID = " & mEvaluationID) 
        Return Delete("DELETE FROM tblEvaluations WHERE EvaluationID = " & mEvaluationID)

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