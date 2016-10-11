Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class FundRaising

#region "Variables"

    Protected mFundraisingID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mFundraisingDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mAmountRaised As decimal
    Protected mFundraisingEvent As string
    Protected mLocation As string

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

    Public  Property FundraisingID() As long
        Get
		return mFundraisingID
        End Get
        Set(ByVal value As long)
		mFundraisingID = value
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

    Public  Property FundraisingDate() As string
        Get
		return mFundraisingDate
        End Get
        Set(ByVal value As string)
		mFundraisingDate = value
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

    Public  Property AmountRaised() As decimal
        Get
		return mAmountRaised
        End Get
        Set(ByVal value As decimal)
		mAmountRaised = value
        End Set
    End Property

    Public  Property FundraisingEvent() As string
        Get
		return mFundraisingEvent
        End Get
        Set(ByVal value As string)
		mFundraisingEvent = value
        End Set
    End Property

    Public  Property Location() As string
        Get
		return mLocation
        End Get
        Set(ByVal value As string)
		mLocation = value
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

    FundraisingID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mFundraisingDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mAmountRaised = 0.0
    mFundraisingEvent = ""
    mLocation = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mFundraisingID) 

    End Function 

    Public Overridable Function Retrieve(ByVal FundraisingID As Long) As Boolean 

        Dim sql As String 

        If FundraisingID > 0 Then 
            sql = "SELECT * FROM tblFundraisings WHERE FundraisingID = " & FundraisingID
        Else 
            sql = "SELECT * FROM tblFundraisings WHERE FundraisingID = " & mFundraisingID
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

                log.error("FundRaising not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetFundRaising() As System.Data.DataSet

        Return GetFundRaising(mFundraisingID)

    End Function

    Public Overridable Function GetFundRaising(ByVal FundraisingID As Long) As DataSet

        Dim sql As String

        If FundraisingID > 0 Then
            sql = "SELECT * FROM tblFundraisings WHERE FundraisingID = " & FundraisingID
        Else
            sql = "SELECT * FROM tblFundraisings WHERE FundraisingID = " & mFundraisingID
        End If

        Return GetFundRaising(sql)

    End Function

    Public Overridable Function GetFundRaising(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mFundraisingID = Catchnull(.Item("FundraisingID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mFundraisingDate = Catchnull(.Item("FundraisingDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mAmountRaised = Catchnull(.Item("AmountRaised"), 0.0)
            mFundraisingEvent = Catchnull(.Item("FundraisingEvent"), "")
            mLocation = Catchnull(.Item("Location"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@FundraisingID", DBType.Int32, mFundraisingID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@FundraisingDate", DBType.String, mFundraisingDate)
        db.AddInParameter(cmd, "@AmountRaised", DbType.Decimal, mAmountRaised)
        db.AddInParameter(cmd, "@FundraisingEvent", DBType.String, mFundraisingEvent)
        db.AddInParameter(cmd, "@Location", DBType.String, mLocation)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_FundRaising")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mFundraisingID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblFundraisings SET Deleted = 1 WHERE FundraisingID = " & mFundraisingID) 
        Return Delete("DELETE FROM tblFundraisings WHERE FundraisingID = " & mFundraisingID)

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