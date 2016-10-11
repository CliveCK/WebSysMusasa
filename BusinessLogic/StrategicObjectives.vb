Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class StrategicObjectives

#region "Variables"

    Protected mStrategicObjectiveID As long
    Protected mFromYear As long
    Protected mToYear As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mCode As string
    Protected mDescription As string

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

    Public  Property StrategicObjectiveID() As long
        Get
		return mStrategicObjectiveID
        End Get
        Set(ByVal value As long)
		mStrategicObjectiveID = value
        End Set
    End Property

    Public  Property FromYear() As long
        Get
		return mFromYear
        End Get
        Set(ByVal value As long)
		mFromYear = value
        End Set
    End Property

    Public  Property ToYear() As long
        Get
		return mToYear
        End Get
        Set(ByVal value As long)
		mToYear = value
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

    Public  Property Code() As string
        Get
		return mCode
        End Get
        Set(ByVal value As string)
		mCode = value
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

    StrategicObjectiveID = 0
    mFromYear = 0
    mToYear = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mCode = ""
    mDescription = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mStrategicObjectiveID) 

    End Function 

    Public Overridable Function Retrieve(ByVal StrategicObjectiveID As Long) As Boolean 

        Dim sql As String 

        If StrategicObjectiveID > 0 Then 
            sql = "SELECT * FROM tblStrategicObjectives WHERE StrategicObjectiveID = " & StrategicObjectiveID
        Else 
            sql = "SELECT * FROM tblStrategicObjectives WHERE StrategicObjectiveID = " & mStrategicObjectiveID
        End If 

        Return Retrieve(sql) 

    End Function

    Public Function RetrieveAll() As DataSet

        Dim sql As String = "SELECT * FROM tblStrategicObjectives"

        Return GetStrategicObjectives(sql)

    End Function

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean 

        Try 

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql) 

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then 

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0)) 

                dsRetrieve = Nothing 
                Return True 

            Else 

                log.Warn("StrategicObjectives not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetStrategicObjectives() As System.Data.DataSet

        Return GetStrategicObjectives(mStrategicObjectiveID)

    End Function

    Public Overridable Function GetStrategicObjectives(ByVal StrategicObjectiveID As Long) As DataSet

        Dim sql As String

        If StrategicObjectiveID > 0 Then
            sql = "SELECT * FROM tblStrategicObjectives WHERE StrategicObjectiveID = " & StrategicObjectiveID
        Else
            sql = "SELECT * FROM tblStrategicObjectives WHERE StrategicObjectiveID = " & mStrategicObjectiveID
        End If

        Return GetStrategicObjectives(sql)

    End Function

    Protected Overridable Function GetStrategicObjectives(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mStrategicObjectiveID = Catchnull(.Item("StrategicObjectiveID"), 0)
            mFromYear = Catchnull(.Item("FromYear"), 0)
            mToYear = Catchnull(.Item("ToYear"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mCode = Catchnull(.Item("Code"), "")
            mDescription = Catchnull(.Item("Description"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@StrategicObjectiveID", DBType.Int32, mStrategicObjectiveID)
        db.AddInParameter(cmd, "@FromYear", DBType.Int32, mFromYear)
        db.AddInParameter(cmd, "@ToYear", DBType.Int32, mToYear)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Code", DBType.String, mCode)
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_StrategicObjectives")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mStrategicObjectiveID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblStrategicObjectives SET Deleted = 1 WHERE StrategicObjectiveID = " & mStrategicObjectiveID) 
        Return Delete("DELETE FROM tblStrategicObjectives WHERE StrategicObjectiveID = " & mStrategicObjectiveID)

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

#end region

End Class