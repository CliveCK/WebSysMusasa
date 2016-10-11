Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Eligibility

#region "Variables"

    Protected mEligibilityID As long
    Protected mPatientID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mPriority As string
    Protected mEvaluation As string
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

    Public  Property EligibilityID() As long
        Get
		return mEligibilityID
        End Get
        Set(ByVal value As long)
		mEligibilityID = value
        End Set
    End Property

    Public  Property PatientID() As long
        Get
		return mPatientID
        End Get
        Set(ByVal value As long)
		mPatientID = value
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

    Public  Property Priority() As string
        Get
		return mPriority
        End Get
        Set(ByVal value As string)
		mPriority = value
        End Set
    End Property

    Public  Property Evaluation() As string
        Get
		return mEvaluation
        End Get
        Set(ByVal value As string)
		mEvaluation = value
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

    EligibilityID = 0
    mPatientID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mPriority = ""
    mEvaluation = ""
    mComments = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mEligibilityID) 

    End Function 

    Public Overridable Function Retrieve(ByVal EligibilityID As Long) As Boolean 

        Dim sql As String 

        If EligibilityID > 0 Then 
            sql = "SELECT * FROM tblEligibility WHERE EligibilityID = " & EligibilityID
        Else 
            sql = "SELECT * FROM tblEligibility WHERE EligibilityID = " & mEligibilityID
        End If 

        Return Retrieve(sql) 

    End Function

    Public Function RetrieveByPatient(ByVal PatientID As Long) As Boolean

        Dim sql As String

        If PatientID > 0 Then
            sql = "SELECT * FROM tblEligibility WHERE PatientID = " & PatientID
        Else
            sql = "SELECT * FROM tblEligibility WHERE PatientID = " & mPatientID
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

                log.error("Eligibility not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetEligibility() As System.Data.DataSet

        Return GetEligibility(mEligibilityID)

    End Function

    Public Overridable Function GetEligibility(ByVal EligibilityID As Long) As DataSet

        Dim sql As String

        If EligibilityID > 0 Then
            sql = "SELECT * FROM tblEligibility WHERE EligibilityID = " & EligibilityID
        Else
            sql = "SELECT * FROM tblEligibility WHERE EligibilityID = " & mEligibilityID
        End If

        Return GetEligibility(sql)

    End Function

    Protected Overridable Function GetEligibility(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mEligibilityID = Catchnull(.Item("EligibilityID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mPriority = Catchnull(.Item("Priority"), "")
            mEvaluation = Catchnull(.Item("Evaluation"), "")
            mComments = Catchnull(.Item("Comments"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@EligibilityID", DBType.Int32, mEligibilityID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Priority", DBType.String, mPriority)
        db.AddInParameter(cmd, "@Evaluation", DBType.String, mEvaluation)
        db.AddInParameter(cmd, "@Comments", DBType.String, mComments)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Eligibility")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mEligibilityID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblEligibility SET Deleted = 1 WHERE EligibilityID = " & mEligibilityID) 
        Return Delete("DELETE FROM tblEligibility WHERE EligibilityID = " & mEligibilityID)

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