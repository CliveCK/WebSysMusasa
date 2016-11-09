Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class AccompanyingChildren

#region "Variables"

    Protected mAccompanyingChildrenID As long
    Protected mBeneficiaryID As Long
    Protected mAge As Decimal
    Protected mSex As String
    Protected mFirstName As String
    Protected mSurname As String

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

    Public  Property AccompanyingChildrenID() As long
        Get
		return mAccompanyingChildrenID
        End Get
        Set(ByVal value As long)
		mAccompanyingChildrenID = value
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

    Public Property Age() As Decimal
        Get
            Return mAge
        End Get
        Set(ByVal value As Decimal)
            mAge = value
        End Set
    End Property

    Public Property Sex() As String
        Get
            Return mSex
        End Get
        Set(ByVal value As String)
            mSex = value
        End Set
    End Property

    Public Property Firstname() As String
        Get
            Return mFirstName
        End Get
        Set(ByVal value As String)
            mFirstName = value
        End Set
    End Property

    Public Property Surname() As String
        Get
            Return mSurname
        End Get
        Set(ByVal value As String)
            mSurname = value
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

    AccompanyingChildrenID = 0
    mBeneficiaryID = 0
        mAge = 0.0
        mSex = ""
        mFirstName = ""
        mSurname = ""

    End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mAccompanyingChildrenID) 

    End Function 

    Public Overridable Function Retrieve(ByVal AccompanyingChildrenID As Long) As Boolean 

        Dim sql As String 

        If AccompanyingChildrenID > 0 Then 
            sql = "SELECT * FROM tblAccompanyingChildren WHERE AccompanyingChildrenID = " & AccompanyingChildrenID
        Else 
            sql = "SELECT * FROM tblAccompanyingChildren WHERE AccompanyingChildrenID = " & mAccompanyingChildrenID
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

                log.Error("AccompanyingChildren not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetAccompanyingChildren() As System.Data.DataSet

        Return GetAccompanyingChildren(mAccompanyingChildrenID)

    End Function

    Public Overridable Function GetAccompanyingChildren(ByVal BeneficiaryID As Long) As DataSet

        Dim sql As String

        If BeneficiaryID > 0 Then
            sql = "SELECT * FROM tblAccompanyingChildren WHERE BeneficiaryID = " & BeneficiaryID
        Else
            sql = "SELECT * FROM tblAccompanyingChildren WHERE BeneficiaryID = " & mBeneficiaryID
        End If

        Return GetAccompanyingChildren(sql)

    End Function

    Protected Overridable Function GetAccompanyingChildren(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mAccompanyingChildrenID = Catchnull(.Item("AccompanyingChildrenID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mAge = Catchnull(.Item("Age"), 0.0)
            mSex = Catchnull(.Item("Sex"), "")
            mFirstName = Catchnull(.Item("FirstName"), "")
            mSurname = Catchnull(.Item("Surname"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@AccompanyingChildrenID", DbType.Int32, mAccompanyingChildrenID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@Age", DbType.Decimal, mAge)
        db.AddInParameter(cmd, "@Sex", DbType.String, mSex)
        db.AddInParameter(cmd, "@FirstName", DbType.String, mFirstName)
        db.AddInParameter(cmd, "@Surname", DbType.String, mSurname)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_AccompanyingChildren")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mAccompanyingChildrenID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblAccompanyingChildren SET Deleted = 1 WHERE AccompanyingChildrenID = " & mAccompanyingChildrenID) 
        Return Delete("DELETE FROM tblAccompanyingChildren WHERE AccompanyingChildrenID = " & mAccompanyingChildrenID)

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