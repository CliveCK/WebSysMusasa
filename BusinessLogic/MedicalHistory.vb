Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class MedicalHistory

#region "Variables"

    Protected mMedicalHistoryID As long
    Protected mPatientID As long
    Protected mConditionID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateOdDiagnosis As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As String
    Protected mDiagnosis As Long
    Protected mReasonForUnstaging As string

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

    Public  Property MedicalHistoryID() As long
        Get
		return mMedicalHistoryID
        End Get
        Set(ByVal value As long)
		mMedicalHistoryID = value
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

    Public  Property ConditionID() As long
        Get
		return mConditionID
        End Get
        Set(ByVal value As long)
		mConditionID = value
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

    Public  Property DateOdDiagnosis() As string
        Get
		return mDateOdDiagnosis
        End Get
        Set(ByVal value As string)
		mDateOdDiagnosis = value
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

    Public Property Diagnosis() As Long
        Get
            Return mDiagnosis
        End Get
        Set(ByVal value As Long)
            mDiagnosis = value
        End Set
    End Property

    Public  Property ReasonForUnstaging() As string
        Get
		return mReasonForUnstaging
        End Get
        Set(ByVal value As string)
		mReasonForUnstaging = value
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

    MedicalHistoryID = 0
    mPatientID = 0
    mConditionID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateOdDiagnosis = ""
    mCreatedDate = ""
    mUpdatedDate = ""
        mDiagnosis = 0
        mReasonForUnstaging = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mMedicalHistoryID) 

    End Function 

    Public Overridable Function Retrieve(ByVal MedicalHistoryID As Long) As Boolean 

        Dim sql As String 

        If MedicalHistoryID > 0 Then 
            sql = "SELECT * FROM tblMedicalHistory WHERE PatientID = " & MedicalHistoryID
        Else 
            sql = "SELECT * FROM tblMedicalHistory WHERE PatientID = " & mPatientID
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

                log.error("MedicalHistory not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetMedicalHistory() As System.Data.DataSet

        Return GetMedicalHistory(mMedicalHistoryID)

    End Function

    Public Overridable Function GetMedicalHistory(ByVal MedicalHistoryID As Long) As DataSet

        Dim sql As String

        If MedicalHistoryID > 0 Then
            sql = "SELECT * FROM tblMedicalHistory WHERE MedicalHistoryID = " & MedicalHistoryID
        Else
            sql = "SELECT * FROM tblMedicalHistory WHERE MedicalHistoryID = " & mMedicalHistoryID
        End If

        Return GetMedicalHistory(sql)

    End Function

    Protected Overridable Function GetMedicalHistory(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mMedicalHistoryID = Catchnull(.Item("MedicalHistoryID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mConditionID = Catchnull(.Item("ConditionID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateOdDiagnosis = Catchnull(.Item("DateOdDiagnosis"), Nothing)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mDiagnosis = Catchnull(.Item("Diagnosis"), 0)
            mReasonForUnstaging = Catchnull(.Item("ReasonForUnstaging"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@MedicalHistoryID", DBType.Int32, mMedicalHistoryID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@ConditionID", DBType.Int32, mConditionID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateOdDiagnosis", DBType.String, mDateOdDiagnosis)
        db.AddInParameter(cmd, "@Diagnosis", DbType.Int32, mDiagnosis)
        db.AddInParameter(cmd, "@ReasonForUnstaging", DBType.String, mReasonForUnstaging)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_MedicalHistory")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mMedicalHistoryID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblMedicalHistory SET Deleted = 1 WHERE MedicalHistoryID = " & mMedicalHistoryID) 
        Return Delete("DELETE FROM tblMedicalHistory WHERE MedicalHistoryID = " & mMedicalHistoryID)

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