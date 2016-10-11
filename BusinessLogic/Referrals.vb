Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Referrals

#region "Variables"

    Protected mReferralID As long
    Protected mPatientID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mReferredDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mReferredTo As string
    Protected mPatientNo As string
    Protected mDignosisConcern As string
    Protected mAssistanceNeeded As string
    Protected mAssistanceOffered As string

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

    Public  Property ReferralID() As long
        Get
		return mReferralID
        End Get
        Set(ByVal value As long)
		mReferralID = value
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

    Public  Property ReferredDate() As string
        Get
		return mReferredDate
        End Get
        Set(ByVal value As string)
		mReferredDate = value
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

    Public  Property ReferredTo() As string
        Get
		return mReferredTo
        End Get
        Set(ByVal value As string)
		mReferredTo = value
        End Set
    End Property

    Public  Property PatientNo() As string
        Get
		return mPatientNo
        End Get
        Set(ByVal value As string)
		mPatientNo = value
        End Set
    End Property

    Public  Property DignosisConcern() As string
        Get
		return mDignosisConcern
        End Get
        Set(ByVal value As string)
		mDignosisConcern = value
        End Set
    End Property

    Public  Property AssistanceNeeded() As string
        Get
		return mAssistanceNeeded
        End Get
        Set(ByVal value As string)
		mAssistanceNeeded = value
        End Set
    End Property

    Public  Property AssistanceOffered() As string
        Get
		return mAssistanceOffered
        End Get
        Set(ByVal value As string)
		mAssistanceOffered = value
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

    ReferralID = 0
    mPatientID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mReferredDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mReferredTo = ""
    mPatientNo = ""
    mDignosisConcern = ""
    mAssistanceNeeded = ""
    mAssistanceOffered = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mReferralID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ReferralID As Long) As Boolean 

        Dim sql As String 

        If ReferralID > 0 Then 
            sql = "SELECT * FROM tblReferrals WHERE ReferralID = " & ReferralID
        Else 
            sql = "SELECT * FROM tblReferrals WHERE ReferralID = " & mReferralID
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

                log.error("Referrals not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetReferrals() As System.Data.DataSet

        Return GetReferrals(mReferralID)

    End Function

    Public Overridable Function GetReferrals(ByVal ReferralID As Long) As DataSet

        Dim sql As String

        If ReferralID > 0 Then
            sql = "SELECT * FROM tblReferrals WHERE ReferralID = " & ReferralID
        Else
            sql = "SELECT * FROM tblReferrals WHERE ReferralID = " & mReferralID
        End If

        Return GetReferrals(sql)

    End Function

    Public Overridable Function GetReferrals(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mReferralID = Catchnull(.Item("ReferralID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mReferredDate = Catchnull(.Item("ReferredDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mReferredTo = Catchnull(.Item("ReferredTo"), "")
            mPatientNo = Catchnull(.Item("PatientNo"), "")
            mDignosisConcern = Catchnull(.Item("DignosisConcern"), "")
            mAssistanceNeeded = Catchnull(.Item("AssistanceNeeded"), "")
            mAssistanceOffered = Catchnull(.Item("AssistanceOffered"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ReferralID", DBType.Int32, mReferralID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@ReferredDate", DBType.String, mReferredDate)
        db.AddInParameter(cmd, "@ReferredTo", DBType.String, mReferredTo)
        db.AddInParameter(cmd, "@PatientNo", DBType.String, mPatientNo)
        db.AddInParameter(cmd, "@DignosisConcern", DBType.String, mDignosisConcern)
        db.AddInParameter(cmd, "@AssistanceNeeded", DBType.String, mAssistanceNeeded)
        db.AddInParameter(cmd, "@AssistanceOffered", DBType.String, mAssistanceOffered)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Referrals")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mReferralID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblReferrals SET Deleted = 1 WHERE ReferralID = " & mReferralID) 
        Return Delete("DELETE FROM tblReferrals WHERE ReferralID = " & mReferralID)

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