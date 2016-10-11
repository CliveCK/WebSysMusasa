Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Prognosis

#region "Variables"

    Protected mPrognosisID As long
    Protected mPatientID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mDate1 As String
    Protected mDate2 As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mPrognosis1 As string
    Protected mPrognosis2 As string

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

    Public  Property PrognosisID() As long
        Get
		return mPrognosisID
        End Get
        Set(ByVal value As long)
		mPrognosisID = value
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

    Public  Property Date1() As string
        Get
		return mDate1
        End Get
        Set(ByVal value As string)
		mDate1 = value
        End Set
    End Property

    Public  Property Date2() As string
        Get
		return mDate2
        End Get
        Set(ByVal value As string)
		mDate2 = value
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

    Public  Property Prognosis1() As string
        Get
		return mPrognosis1
        End Get
        Set(ByVal value As string)
		mPrognosis1 = value
        End Set
    End Property

    Public  Property Prognosis2() As string
        Get
		return mPrognosis2
        End Get
        Set(ByVal value As string)
		mPrognosis2 = value
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

    PrognosisID = 0
    mPatientID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mDate1 = ""
    mDate2 = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mPrognosis1 = ""
    mPrognosis2 = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mPrognosisID) 

    End Function 

    Public Overridable Function Retrieve(ByVal PrognosisID As Long) As Boolean 

        Dim sql As String 

        If PrognosisID > 0 Then 
            sql = "SELECT * FROM tblPrognosis WHERE PrognosisID = " & PrognosisID
        Else 
            sql = "SELECT * FROM tblPrognosis WHERE PrognosisID = " & mPrognosisID
        End If 

        Return Retrieve(sql) 

    End Function

    Public Overridable Function RetrieveByPatient(ByVal PatientID As Long) As Boolean

        Dim sql As String

        If PatientID > 0 Then
            sql = "SELECT * FROM tblPrognosis WHERE PatientID = " & PatientID
        Else
            sql = "SELECT * FROM tblPrognosis WHERE PatientID = " & mPatientID
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

                log.error("Prognosis not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetPrognosis() As System.Data.DataSet

        Return GetPrognosis(mPrognosisID)

    End Function

    Public Overridable Function GetPrognosis(ByVal PrognosisID As Long) As DataSet

        Dim sql As String

        If PrognosisID > 0 Then
            sql = "SELECT * FROM tblPrognosis WHERE PrognosisID = " & PrognosisID
        Else
            sql = "SELECT * FROM tblPrognosis WHERE PrognosisID = " & mPrognosisID
        End If

        Return GetPrognosis(sql)

    End Function

    Protected Overridable Function GetPrognosis(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mPrognosisID = Catchnull(.Item("PrognosisID"), 0)
            mPatientID = Catchnull(.Item("PatientID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mDate1 = Catchnull(.Item("Date1"), "")
            mDate2 = Catchnull(.Item("Date2"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mPrognosis1 = Catchnull(.Item("Prognosis1"), "")
            mPrognosis2 = Catchnull(.Item("Prognosis2"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@PrognosisID", DBType.Int32, mPrognosisID)
        db.AddInParameter(cmd, "@PatientID", DBType.Int32, mPatientID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Date1", DBType.String, mDate1)
        db.AddInParameter(cmd, "@Date2", DBType.String, mDate2)
        db.AddInParameter(cmd, "@Prognosis1", DBType.String, mPrognosis1)
        db.AddInParameter(cmd, "@Prognosis2", DBType.String, mPrognosis2)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Prognosis")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mPrognosisID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblPrognosis SET Deleted = 1 WHERE PrognosisID = " & mPrognosisID) 
        Return Delete("DELETE FROM tblPrognosis WHERE PrognosisID = " & mPrognosisID)

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