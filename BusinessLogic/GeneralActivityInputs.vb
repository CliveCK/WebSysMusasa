Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class GeneralActivityInputs

#region "Variables"

    Protected mGeneralActivityInputID As long
    Protected mGeneralActivityID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mCost As decimal
    Protected mDescription As string
    Protected mQuantity As string

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

    Public  Property GeneralActivityInputID() As long
        Get
		return mGeneralActivityInputID
        End Get
        Set(ByVal value As long)
		mGeneralActivityInputID = value
        End Set
    End Property

    Public  Property GeneralActivityID() As long
        Get
		return mGeneralActivityID
        End Get
        Set(ByVal value As long)
		mGeneralActivityID = value
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

    Public  Property Cost() As decimal
        Get
		return mCost
        End Get
        Set(ByVal value As decimal)
		mCost = value
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

    Public  Property Quantity() As string
        Get
		return mQuantity
        End Get
        Set(ByVal value As string)
		mQuantity = value
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

    GeneralActivityInputID = 0
    mGeneralActivityID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mCost = 0.0
    mDescription = ""
    mQuantity = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGeneralActivityInputID) 

    End Function 

    Public Overridable Function Retrieve(ByVal GeneralActivityInputID As Long) As Boolean 

        Dim sql As String 

        If GeneralActivityInputID > 0 Then 
            sql = "SELECT * FROM tblGeneralActivityInputs WHERE GeneralActivityInputID = " & GeneralActivityInputID
        Else 
            sql = "SELECT * FROM tblGeneralActivityInputs WHERE GeneralActivityInputID = " & mGeneralActivityInputID
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

                log.error("GeneralActivityInputs not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetGeneralActivityInputs() As System.Data.DataSet

        Return GetGeneralActivityInputs(mGeneralActivityInputID)

    End Function

    Public Overridable Function GetGeneralActivityInputs(ByVal GeneralActivityInputID As Long) As DataSet

        Dim sql As String

        If GeneralActivityInputID > 0 Then
            sql = "SELECT * FROM tblGeneralActivityInputs WHERE GeneralActivityInputID = " & GeneralActivityInputID
        Else
            sql = "SELECT * FROM tblGeneralActivityInputs WHERE GeneralActivityInputID = " & mGeneralActivityInputID
        End If

        Return GetGeneralActivityInputs(sql)

    End Function

    Protected Overridable Function GetGeneralActivityInputs(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGeneralActivityInputID = Catchnull(.Item("GeneralActivityInputID"), 0)
            mGeneralActivityID = Catchnull(.Item("GeneralActivityID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mCost = Catchnull(.Item("Cost"), 0.0)
            mDescription = Catchnull(.Item("Description"), "")
            mQuantity = Catchnull(.Item("Quantity"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GeneralActivityInputID", DBType.Int32, mGeneralActivityInputID)
        db.AddInParameter(cmd, "@GeneralActivityID", DBType.Int32, mGeneralActivityID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Cost", DbType.Decimal, mCost)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)
        db.AddInParameter(cmd, "@Quantity", DBType.String, mQuantity)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_GeneralActivityInputs")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGeneralActivityInputID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGeneralActivityInputs SET Deleted = 1 WHERE GeneralActivityInputID = " & mGeneralActivityInputID) 
        Return Delete("DELETE FROM tblGeneralActivityInputs WHERE GeneralActivityInputID = " & mGeneralActivityInputID)

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