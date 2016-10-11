Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class TrainingMarks

#region "Variables"

    Protected mTrainingMarkID As long
    Protected mTrainingID As long
    Protected mPaperID As Long
    Protected mBlockID As Long
    Protected mPeriodID As Long
    Protected mBeneficiaryID As long
    Protected mBeneficiaryTypeID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As Long
    Protected mMark As Decimal
    Protected mCreatedDate As String
    Protected mUpdatedDate As String
    Protected mComments As String

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

    Public  Property TrainingMarkID() As long
        Get
		return mTrainingMarkID
        End Get
        Set(ByVal value As long)
		mTrainingMarkID = value
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

    Public Property BlockID() As Long
        Get
            Return mBlockID
        End Get
        Set(ByVal value As Long)
            mBlockID = value
        End Set
    End Property

    Public Property PeriodID() As Long
        Get
            Return mPeriodID
        End Get
        Set(ByVal value As Long)
            mPeriodID = value
        End Set
    End Property

    Public Property PaperID() As Long
        Get
            Return mPaperID
        End Get
        Set(ByVal value As Long)
            mPaperID = value
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

    Public  Property BeneficiaryTypeID() As long
        Get
		return mBeneficiaryTypeID
        End Get
        Set(ByVal value As long)
		mBeneficiaryTypeID = value
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

    Public Property Comments() As String
        Get
            Return mComments
        End Get
        Set(ByVal value As String)
            mComments = value
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

    Public  Property Mark() As decimal
        Get
		return mMark
        End Get
        Set(ByVal value As decimal)
		mMark = value
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

    TrainingMarkID = 0
    mTrainingID = 0
        mPaperID = 0
        mPeriodID = 0
        mBlockID = 0
    mBeneficiaryID = 0
    mBeneficiaryTypeID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
        mUpdatedDate = ""
        mComments = 0
    mMark = 0.0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mTrainingMarkID) 

    End Function 

    Public Overridable Function Retrieve(ByVal TrainingMarkID As Long) As Boolean 

        Dim sql As String 

        If TrainingMarkID > 0 Then 
            sql = "SELECT * FROM tblTrainingMarks WHERE TrainingMarkID = " & TrainingMarkID
        Else 
            sql = "SELECT * FROM tblTrainingMarks WHERE TrainingMarkID = " & mTrainingMarkID
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

                log.error("TrainingMarks not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetTrainingMarks() As System.Data.DataSet

        Return GetTrainingMarks(mTrainingMarkID)

    End Function

    Public Overridable Function GetTrainingMarks(ByVal TrainingMarkID As Long) As DataSet

        Dim sql As String

        If TrainingMarkID > 0 Then
            sql = "SELECT * FROM tblTrainingMarks WHERE TrainingMarkID = " & TrainingMarkID
        Else
            sql = "SELECT * FROM tblTrainingMarks WHERE TrainingMarkID = " & mTrainingMarkID
        End If

        Return GetTrainingMarks(sql)

    End Function

    Protected Overridable Function GetTrainingMarks(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function AddTrainingMarksBatch(ByVal sql As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, sql)
            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mTrainingMarkID = Catchnull(.Item("TrainingMarkID"), 0)
            mTrainingID = Catchnull(.Item("TrainingID"), 0)
            mPaperID = Catchnull(.Item("PaperID"), 0)
            mBlockID = Catchnull(.Item("BlockID"), 0)
            mPeriodID = Catchnull(.Item("PeriodID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mBeneficiaryTypeID = Catchnull(.Item("BeneficiaryTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mComments = Catchnull(.Item("Comments"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mMark = Catchnull(.Item("Mark"), 0.0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@TrainingMarkID", DBType.Int32, mTrainingMarkID)
        db.AddInParameter(cmd, "@TrainingID", DbType.Int32, mTrainingID)
        db.AddInParameter(cmd, "@BlockID", DbType.Int32, mBlockID)
        db.AddInParameter(cmd, "@PeriodID", DbType.Int32, mPeriodID)
        db.AddInParameter(cmd, "@PaperID", DbType.Int32, mPaperID)
        db.AddInParameter(cmd, "@BeneficiaryID", DBType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@BeneficiaryTypeID", DbType.Int32, mBeneficiaryTypeID)
        db.AddInParameter(cmd, "@Comments", DbType.String, mComments)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Mark", DbType.Decimal, mMark)

    End Sub

    Public Function SaveTrainingMarks() As Boolean

        Dim sql As String = ""

        Try

            If mTrainingMarkID > 0 Then

                sql = "UPDATE tblTrainingMarks SET [Mark] = " & mMark & ", [Comments] = '" & mComments & "' WHERE "
                sql &= " TrainingMarkID = " & mTrainingMarkID

                db.ExecuteNonQuery(CommandType.Text, sql)

                Return True

            End If

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_TrainingMarks")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mTrainingMarkID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblTrainingMarks SET Deleted = 1 WHERE TrainingMarkID = " & mTrainingMarkID) 
        Return Delete("DELETE FROM tblTrainingMarks WHERE TrainingMarkID = " & mTrainingMarkID)

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