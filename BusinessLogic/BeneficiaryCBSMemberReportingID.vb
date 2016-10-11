Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class BeneficiaryCBSMemberReportingID

#region "Variables"

    Protected mBeneficiaryCBSMemberReportingID As long
    Protected mCBSMemberReportingID As long
    Protected mBeneficiaryID As long

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

    Public  Property BeneficiaryCBSMemberReportingID() As long
        Get
		return mBeneficiaryCBSMemberReportingID
        End Get
        Set(ByVal value As long)
		mBeneficiaryCBSMemberReportingID = value
        End Set
    End Property

    Public  Property CBSMemberReportingID() As long
        Get
		return mCBSMemberReportingID
        End Get
        Set(ByVal value As long)
		mCBSMemberReportingID = value
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

    BeneficiaryCBSMemberReportingID = 0
    mCBSMemberReportingID = 0
    mBeneficiaryID = 0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mBeneficiaryCBSMemberReportingID) 

    End Function 

    Public Overridable Function Retrieve(ByVal BeneficiaryCBSMemberReportingID As Long) As Boolean 

        Dim sql As String 

        If BeneficiaryCBSMemberReportingID > 0 Then 
            sql = "SELECT * FROM tblBeneficiaryCBSMemberReportingID WHERE BeneficiaryCBSMemberReportingID = " & BeneficiaryCBSMemberReportingID
        Else 
            sql = "SELECT * FROM tblBeneficiaryCBSMemberReportingID WHERE BeneficiaryCBSMemberReportingID = " & mBeneficiaryCBSMemberReportingID
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

                log.Error("BeneficiaryCBSMemberReportingID not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetBeneficiaryCBSMemberReportingID() As System.Data.DataSet

        Return GetBeneficiaryCBSMemberReportingID(mBeneficiaryCBSMemberReportingID)

    End Function

    Public Overridable Function GetBeneficiaryCBSMemberReportingID(ByVal BeneficiaryCBSMemberReportingID As Long) As DataSet

        Dim sql As String

        If BeneficiaryCBSMemberReportingID > 0 Then
            sql = "SELECT * FROM tblBeneficiaryCBSMemberReportingID WHERE BeneficiaryCBSMemberReportingID = " & BeneficiaryCBSMemberReportingID
        Else
            sql = "SELECT * FROM tblBeneficiaryCBSMemberReportingID WHERE BeneficiaryCBSMemberReportingID = " & mBeneficiaryCBSMemberReportingID
        End If

        Return GetBeneficiaryCBSMemberReportingID(sql)

    End Function

    Protected Overridable Function GetBeneficiaryCBSMemberReportingID(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mBeneficiaryCBSMemberReportingID = Catchnull(.Item("BeneficiaryCBSMemberReportingID"), 0)
            mCBSMemberReportingID = Catchnull(.Item("CBSMemberReportingID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@BeneficiaryCBSMemberReportingID", DbType.Int32, mBeneficiaryCBSMemberReportingID)
        db.AddInParameter(cmd, "@CBSMemberReportingID", DbType.Int32, mCBSMemberReportingID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_BeneficiaryCBSMemberReportingID")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mBeneficiaryCBSMemberReportingID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblBeneficiaryCBSMemberReportingID SET Deleted = 1 WHERE BeneficiaryCBSMemberReportingID = " & mBeneficiaryCBSMemberReportingID) 
        Return Delete("DELETE FROM tblBeneficiaryCBSMemberReportingID WHERE BeneficiaryCBSMemberReportingID = " & mBeneficiaryCBSMemberReportingID)

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