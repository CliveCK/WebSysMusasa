Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class TrainingInputs
#Region "Variables"

    Protected mTrainingInputID As Long
    Protected mTrainingID As Long
    Protected mQuantity As Long
    Protected mCreatedBy As Long
    Protected mUpdatedBy As Long
    Protected mCreatedDate As String
    Protected mUpdatedDate As String
    Protected mCost As Decimal
    Protected mDescription As String

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

    Public Property TrainingInputID() As Long
        Get
            Return mTrainingInputID
        End Get
        Set(ByVal value As Long)
            mTrainingInputID = value
        End Set
    End Property

    Public Property TrainingID() As Long
        Get
            Return mTrainingID
        End Get
        Set(ByVal value As Long)
            mTrainingID = value
        End Set
    End Property

    Public Property Quantity() As Long
        Get
            Return mQuantity
        End Get
        Set(ByVal value As Long)
            mQuantity = value
        End Set
    End Property

    Public Property CreatedBy() As Long
        Get
            Return mCreatedBy
        End Get
        Set(ByVal value As Long)
            mCreatedBy = value
        End Set
    End Property

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
        End Set
    End Property

    Public Property CreatedDate() As String
        Get
            Return mCreatedDate
        End Get
        Set(ByVal value As String)
            mCreatedDate = value
        End Set
    End Property

    Public Property UpdatedDate() As String
        Get
            Return mUpdatedDate
        End Get
        Set(ByVal value As String)
            mUpdatedDate = value
        End Set
    End Property

    Public Property Cost() As Decimal
        Get
            Return mCost
        End Get
        Set(ByVal value As Decimal)
            mCost = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal value As String)
            mDescription = value
        End Set
    End Property

#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub

#End Region

    Public Sub Clear()

        TrainingInputID = 0
        mTrainingID = 0
        mQuantity = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mCreatedDate = ""
        mUpdatedDate = ""
        mCost = 0.0
        mDescription = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mTrainingInputID)

    End Function

    Public Overridable Function Retrieve(ByVal TrainingInputID As Long) As Boolean

        Dim sql As String

        If TrainingInputID > 0 Then
            sql = "SELECT * FROM tblTrainingInputs WHERE TrainingInputID = " & TrainingInputID
        Else
            sql = "SELECT * FROM tblTrainingInputs WHERE TrainingInputID = " & mTrainingInputID
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

                log.Warn("TrainingInputs not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetTrainingInputs() As System.Data.DataSet

        Return GetTrainingInputs(mTrainingInputID)

    End Function

    Public Overridable Function GetTrainingInputs(ByVal TrainingInputID As Long) As DataSet

        Dim sql As String

        If TrainingInputID > 0 Then
            sql = "SELECT * FROM tblTrainingInputs WHERE TrainingInputID = " & TrainingInputID
        Else
            sql = "SELECT * FROM tblTrainingInputs WHERE TrainingInputID = " & mTrainingInputID
        End If

        Return GetTrainingInputs(sql)

    End Function

    Protected Overridable Function GetTrainingInputs(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mTrainingInputID = Catchnull(.Item("TrainingInputID"), 0)
            mTrainingID = Catchnull(.Item("TrainingID"), 0)
            mQuantity = Catchnull(.Item("Quantity"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mCost = Catchnull(.Item("Cost"), 0.0)
            mDescription = Catchnull(.Item("Description"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@TrainingInputID", DBType.Int32, mTrainingInputID)
        db.AddInParameter(cmd, "@TrainingID", DBType.Int32, mTrainingID)
        db.AddInParameter(cmd, "@Quantity", DBType.Int32, mQuantity)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Cost", DbType.Decimal, mCost)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_TrainingInputs")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mTrainingInputID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblTrainingInputs SET Deleted = 1 WHERE TrainingInputID = " & mTrainingInputID) 
        Return Delete("DELETE FROM tblTrainingInputs WHERE TrainingInputID = " & mTrainingInputID)

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

#End Region

End Class