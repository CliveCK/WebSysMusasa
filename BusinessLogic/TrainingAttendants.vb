Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class TrainingAttendants

#region "Variables"

    Protected mTrainingAttendantID As long
    Protected mTrainingID As long
    Protected mBeneficiaryTypeID As long
    Protected mBeneficiaryID As long
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

    Public  Property TrainingAttendantID() As long
        Get
		return mTrainingAttendantID
        End Get
        Set(ByVal value As long)
		mTrainingAttendantID = value
        End Set
    End Property

    Public  Property TrainingID() As long
        Get
		return mTrainingID
        End Get
        Set(ByVal value As long)
		mTrainingID = value
        End Set
    End Property

    Public  Property BeneficiaryTypeID() As long
        Get
		return mBeneficiaryTypeID
        End Get
        Set(ByVal value As long)
		mBeneficiaryTypeID = value
        End Set
    End Property

    Public  Property BeneficiaryID() As long
        Get
		return mBeneficiaryID
        End Get
        Set(ByVal value As long)
		mBeneficiaryID = value
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

    TrainingAttendantID = 0
    mTrainingID = 0
    mBeneficiaryTypeID = 0
    mBeneficiaryID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mTrainingAttendantID) 

    End Function 

    Public Overridable Function Retrieve(ByVal TrainingAttendantID As Long) As Boolean 

        Dim sql As String 

        If TrainingAttendantID > 0 Then 
            sql = "SELECT * FROM tblTrainingAttendants WHERE TrainingAttendantID = " & TrainingAttendantID
        Else 
            sql = "SELECT * FROM tblTrainingAttendants WHERE TrainingAttendantID = " & mTrainingAttendantID
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

                log.Warn("TrainingAttendants not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetTrainingAttendants() As System.Data.DataSet

        Return GetTrainingAttendants(mTrainingAttendantID)

    End Function

    Public Overridable Function GetTrainingAttendants(ByVal TrainingAttendantID As Long) As DataSet

        Dim sql As String

        If TrainingAttendantID > 0 Then
            sql = "SELECT * FROM tblTrainingAttendants WHERE TrainingAttendantID = " & TrainingAttendantID
        Else
            sql = "SELECT * FROM tblTrainingAttendants WHERE TrainingAttendantID = " & mTrainingAttendantID
        End If

        Return GetTrainingAttendants(sql)

    End Function

    Protected Overridable Function GetTrainingAttendants(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mTrainingAttendantID = Catchnull(.Item("TrainingAttendantID"), 0)
            mTrainingID = Catchnull(.Item("TrainingID"), 0)
            mBeneficiaryTypeID = Catchnull(.Item("BeneficiaryTypeID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@TrainingAttendantID", DBType.Int32, mTrainingAttendantID)
        db.AddInParameter(cmd, "@TrainingID", DBType.Int32, mTrainingID)
        db.AddInParameter(cmd, "@BeneficiaryTypeID", DBType.Int32, mBeneficiaryTypeID)
        db.AddInParameter(cmd, "@BeneficiaryID", DBType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_TrainingAttendants")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mTrainingAttendantID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblTrainingAttendants SET Deleted = 1 WHERE TrainingAttendantID = " & mTrainingAttendantID) 
        Return Delete("DELETE FROM tblTrainingAttendants WHERE TrainingAttendantID = " & mTrainingAttendantID)

    End Function

    Public Function DeleteEntries() As Boolean

        Return Delete("DELETE FROM tblTrainingAttendants WHERE BeneficiaryID = " & mBeneficiaryID & " AND TrainingID = " & mTrainingID)

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

#Region "Miscellaneous"

    Public Function GetBeneficiaryTypeIDByName(ByVal BeneficiaryType As String) As Long

        Dim sql As String = "SELECT BeneficiaryTypeID FROM luBeneficiaryType WHERE Description = '" & BeneficiaryType & "'"

        Return GetTrainingAttendants(sql).Tables(0).Rows(0)(0)

    End Function

    Public Function MarksRecordExists(ByVal TrainingID As Long, ByVal BlockID As Long, ByVal PeriodID As Long, ByVal PaperID As Long, ByVal HealthCenterStaffID As Long, ByVal BeneficiaryTypeID As Long) As Boolean

        Dim ds As DataSet
        Dim sql As String = "SELECT * FROM tblTrainingMarks WHERE TrainingID = " & TrainingID & " AND BlockID = " & BlockID & " AND PeriodID = " & PeriodID & " AND PaperID = " & PaperID & " AND BeneficiaryID = " & HealthCenterStaffID & " AND BeneficiaryTypeID = " & BeneficiaryTypeID

        ds = GetTrainingAttendants(sql)

        If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

            Return True

        End If

        Return False

    End Function

    Public Function CheckAttendantExistence(ByVal HealthCenterStaffID As Long, ByVal TrainingID As Long, ByVal BeneficiaryTypeID As Long) As Boolean

        Dim ds As DataSet
        Dim sql As String = "SELECT * FROM tblTrainingAttendants WHERE BeneficiaryID = " & HealthCenterStaffID & " AND TrainingID = " & TrainingID & " AND BeneficiaryTypeID = " & BeneficiaryTypeID

        ds = GetTrainingAttendants(sql)

        If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

            Return True

        End If

        Return False

    End Function

#End Region

#End Region

End Class