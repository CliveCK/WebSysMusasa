Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class FirstPointOfContact

#region "Variables"

    Protected mFirstPointOfContactID As long
    Protected mPatientID As long
    Protected mTypeOfContactID As long
    Protected mAdmittedTo As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDateOfFirstContact As string
    Protected mDateOfFirstReferral As string
    Protected mDateFirstAdmitted As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mContactName As string
    Protected mReferralHospital As string

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

    Public  Property FirstPointOfContactID() As long
        Get
		return mFirstPointOfContactID
        End Get
        Set(ByVal value As long)
		mFirstPointOfContactID = value
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

    Public  Property TypeOfContactID() As long
        Get
		return mTypeOfContactID
        End Get
        Set(ByVal value As long)
		mTypeOfContactID = value
        End Set
    End Property

    Public  Property AdmittedTo() As long
        Get
		return mAdmittedTo
        End Get
        Set(ByVal value As long)
		mAdmittedTo = value
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

    Public  Property DateOfFirstContact() As string
        Get
		return mDateOfFirstContact
        End Get
        Set(ByVal value As string)
		mDateOfFirstContact = value
        End Set
    End Property

    Public  Property DateOfFirstReferral() As string
        Get
		return mDateOfFirstReferral
        End Get
        Set(ByVal value As string)
		mDateOfFirstReferral = value
        End Set
    End Property

    Public  Property DateFirstAdmitted() As string
        Get
		return mDateFirstAdmitted
        End Get
        Set(ByVal value As string)
		mDateFirstAdmitted = value
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

    Public  Property ContactName() As string
        Get
		return mContactName
        End Get
        Set(ByVal value As string)
		mContactName = value
        End Set
    End Property

    Public  Property ReferralHospital() As string
        Get
		return mReferralHospital
        End Get
        Set(ByVal value As string)
		mReferralHospital = value
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

    FirstPointOfContactID = 0
    mPatientID = 0
    mTypeOfContactID = 0
    mAdmittedTo = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDateOfFirstContact = ""
    mDateOfFirstReferral = ""
    mDateFirstAdmitted = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mContactName = ""
    mReferralHospital = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mFirstPointOfContactID) 

    End Function 

    Public Overridable Function Retrieve(ByVal FirstPointOfContactID As Long) As Boolean 

        Dim sql As String 

        If FirstPointOfContactID > 0 Then 
            sql = "SELECT * FROM tblFirstPointOfContact WHERE FirstPointOfContactID = " & FirstPointOfContactID
        Else 
            sql = "SELECT * FROM tblFirstPointOfContact WHERE FirstPointOfContactID = " & mFirstPointOfContactID
        End If 

        Return Retrieve(sql) 

    End Function

    Public Function RetreiveByPatient(ByVal PatientID As Long) As Boolean

        Dim sql As String

        If PatientID > 0 Then
            sql = "SELECT * FROM tblFirstPointOfContact WHERE PatientID = " & PatientID
        Else
            sql = "SELECT * FROM tblFirstPointOfContact WHERE PatientID = " & mPatientID
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

                log.error("FirstPointOfContact not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetFirstPointOfContact() As System.Data.DataSet

        Return GetFirstPointOfContact(mFirstPointOfContactID)

    End Function

    Public Overridable Function GetFirstPointOfContact(ByVal FirstPointOfContactID As Long) As DataSet

        Dim sql As String

        If FirstPointOfContactID > 0 Then
            sql = "SELECT * FROM tblFirstPointOfContact WHERE FirstPointOfContactID = " & FirstPointOfContactID
        Else
            sql = "SELECT * FROM tblFirstPointOfContact WHERE FirstPointOfContactID = " & mFirstPointOfContactID
        End If

        Return GetFirstPointOfContact(sql)

    End Function

    Protected Overridable Function GetFirstPointOfContact(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mFirstPointOfContactID = Catchnull(.Item("FirstPointOfContactID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mTypeOfContactID = Catchnull(.Item("TypeOfContactID"), 0)
            mAdmittedTo = Catchnull(.Item("AdmittedTo"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDateOfFirstContact = Catchnull(.Item("DateOfFirstContact"), Nothing)
            mDateOfFirstReferral = Catchnull(.Item("DateOfFirstReferral"), Nothing)
            mDateFirstAdmitted = Catchnull(.Item("DateFirstAdmitted"), Nothing)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mContactName = Catchnull(.Item("ContactName"), "")
            mReferralHospital = Catchnull(.Item("ReferralHospital"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@FirstPointOfContactID", DBType.Int32, mFirstPointOfContactID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@TypeOfContactID", DBType.Int32, mTypeOfContactID)
        db.AddInParameter(cmd, "@AdmittedTo", DBType.Int32, mAdmittedTo)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@DateOfFirstContact", DBType.String, mDateOfFirstContact)
        db.AddInParameter(cmd, "@DateOfFirstReferral", DBType.String, mDateOfFirstReferral)
        db.AddInParameter(cmd, "@DateFirstAdmitted", DBType.String, mDateFirstAdmitted)
        db.AddInParameter(cmd, "@ContactName", DBType.String, mContactName)
        db.AddInParameter(cmd, "@ReferralHospital", DBType.String, mReferralHospital)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_FirstPointOfContact")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mFirstPointOfContactID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblFirstPointOfContact SET Deleted = 1 WHERE FirstPointOfContactID = " & mFirstPointOfContactID) 
        Return Delete("DELETE FROM tblFirstPointOfContact WHERE FirstPointOfContactID = " & mFirstPointOfContactID)

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