Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class DischargeSummary

#region "Variables"

    Protected mDischargeSummaryID As long
    Protected mPatientID As long
    Protected mHospitalID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mReviewDate As String
    Protected mDischargedDate As String
    Protected mWard As String
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mDischarged As string
    Protected mDischargedTo As string
    Protected mReasonForDischarge As string

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

    Public  Property DischargeSummaryID() As long
        Get
		return mDischargeSummaryID
        End Get
        Set(ByVal value As long)
		mDischargeSummaryID = value
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

    Public  Property HospitalID() As long
        Get
		return mHospitalID
        End Get
        Set(ByVal value As long)
		mHospitalID = value
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

    Public  Property ReviewDate() As string
        Get
		return mReviewDate
        End Get
        Set(ByVal value As string)
		mReviewDate = value
        End Set
    End Property

    Public Property DischargedDate() As String
        Get
            Return mDischargedDate
        End Get
        Set(ByVal value As String)
            mDischargedDate = value
        End Set
    End Property

    Public Property Ward() As String
        Get
            Return mWard
        End Get
        Set(ByVal value As String)
            mWard = value
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

    Public  Property Discharged() As string
        Get
		return mDischarged
        End Get
        Set(ByVal value As string)
		mDischarged = value
        End Set
    End Property

    Public  Property DischargedTo() As string
        Get
		return mDischargedTo
        End Get
        Set(ByVal value As string)
		mDischargedTo = value
        End Set
    End Property

    Public  Property ReasonForDischarge() As string
        Get
		return mReasonForDischarge
        End Get
        Set(ByVal value As string)
		mReasonForDischarge = value
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

    DischargeSummaryID = 0
    mPatientID = 0
    mHospitalID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
        mReviewDate = ""
        mDischargedDate = ""
        mWard = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mDischarged = ""
    mDischargedTo = ""
    mReasonForDischarge = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mDischargeSummaryID) 

    End Function 

    Public Overridable Function Retrieve(ByVal DischargeSummaryID As Long) As Boolean 

        Dim sql As String 

        If DischargeSummaryID > 0 Then 
            sql = "SELECT * FROM tblDischargeSummary WHERE DischargeSummaryID = " & DischargeSummaryID
        Else 
            sql = "SELECT * FROM tblDischargeSummary WHERE DischargeSummaryID = " & mDischargeSummaryID
        End If 

        Return Retrieve(sql) 

    End Function

    Public Overridable Function RetrieveByPatient(ByVal PatientID As Long) As Boolean

        Dim sql As String

        If PatientID > 0 Then
            sql = "SELECT * FROM tblDischargeSummary WHERE PatientID = " & PatientID
        Else
            sql = "SELECT * FROM tblDischargeSummary WHERE PatientID = " & PatientID
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

                log.error("DischargeSummary not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetDischargeSummary() As System.Data.DataSet

        Return GetDischargeSummary(mDischargeSummaryID)

    End Function

    Public Overridable Function GetDischargeSummary(ByVal DischargeSummaryID As Long) As DataSet

        Dim sql As String

        If DischargeSummaryID > 0 Then
            sql = "SELECT * FROM tblDischargeSummary WHERE DischargeSummaryID = " & DischargeSummaryID
        Else
            sql = "SELECT * FROM tblDischargeSummary WHERE DischargeSummaryID = " & mDischargeSummaryID
        End If

        Return GetDischargeSummary(sql)

    End Function

    Protected Overridable Function GetDischargeSummary(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mDischargeSummaryID = Catchnull(.Item("DischargeSummaryID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mHospitalID = Catchnull(.Item("HospitalID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mReviewDate = Catchnull(.Item("ReviewDate"), Nothing)
            mDischargedDate = Catchnull(.Item("DischargedDate"), Nothing)
            mWard = Catchnull(.Item("Ward"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mDischarged = Catchnull(.Item("Discharged"), "")
            mDischargedTo = Catchnull(.Item("DischargedTo"), "")
            mReasonForDischarge = Catchnull(.Item("ReasonForDischarge"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@DischargeSummaryID", DBType.Int32, mDischargeSummaryID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@HospitalID", DBType.Int32, mHospitalID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ReviewDate", DbType.String, mReviewDate)
        db.AddInParameter(cmd, "@DischargedDate", DbType.String, mDischargedDate)
        db.AddInParameter(cmd, "@Ward", DbType.String, mWard)
        db.AddInParameter(cmd, "@Discharged", DBType.String, mDischarged)
        db.AddInParameter(cmd, "@DischargedTo", DBType.String, mDischargedTo)
        db.AddInParameter(cmd, "@ReasonForDischarge", DBType.String, mReasonForDischarge)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_DischargeSummary")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mDischargeSummaryID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblDischargeSummary SET Deleted = 1 WHERE DischargeSummaryID = " & mDischargeSummaryID) 
        Return Delete("DELETE FROM tblDischargeSummary WHERE DischargeSummaryID = " & mDischargeSummaryID)

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