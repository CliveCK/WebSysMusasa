﻿Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class LawyerSessionProblems

#region "Variables"

    Protected mLawyerSessionProblemID As long
    Protected mLawyerClientSessionDetailID As long
    Protected mProblemID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

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

    Public  Property LawyerSessionProblemID() As long
        Get
		return mLawyerSessionProblemID
        End Get
        Set(ByVal value As long)
		mLawyerSessionProblemID = value
        End Set
    End Property

    Public  Property LawyerClientSessionDetailID() As long
        Get
		return mLawyerClientSessionDetailID
        End Get
        Set(ByVal value As long)
		mLawyerClientSessionDetailID = value
        End Set
    End Property

    Public  Property ProblemID() As long
        Get
		return mProblemID
        End Get
        Set(ByVal value As long)
		mProblemID = value
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

    LawyerSessionProblemID = 0
    mLawyerClientSessionDetailID = 0
    mProblemID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mLawyerSessionProblemID) 

    End Function 

    Public Overridable Function Retrieve(ByVal LawyerSessionProblemID As Long) As Boolean 

        Dim sql As String 

        If LawyerSessionProblemID > 0 Then 
            sql = "SELECT * FROM tblLawyerSessionProblems WHERE LawyerSessionProblemID = " & LawyerSessionProblemID
        Else 
            sql = "SELECT * FROM tblLawyerSessionProblems WHERE LawyerSessionProblemID = " & mLawyerSessionProblemID
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

                log.Error("LawyerSessionProblems not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetLawyerSessionProblems() As System.Data.DataSet

        Return GetLawyerSessionProblems(mLawyerSessionProblemID)

    End Function

    Public Overridable Function GetLawyerSessionProblems(ByVal LawyerSessionProblemID As Long) As DataSet

        Dim sql As String

        If LawyerSessionProblemID > 0 Then
            sql = "SELECT * FROM tblLawyerSessionProblems WHERE LawyerSessionProblemID = " & LawyerSessionProblemID
        Else
            sql = "SELECT * FROM tblLawyerSessionProblems WHERE LawyerSessionProblemID = " & mLawyerSessionProblemID
        End If

        Return GetLawyerSessionProblems(sql)

    End Function

    Protected Overridable Function GetLawyerSessionProblems(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mLawyerSessionProblemID = Catchnull(.Item("LawyerSessionProblemID"), 0)
            mLawyerClientSessionDetailID = Catchnull(.Item("LawyerClientSessionDetailID"), 0)
            mProblemID = Catchnull(.Item("ProblemID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@LawyerSessionProblemID", DbType.Int32, mLawyerSessionProblemID)
        db.AddInParameter(cmd, "@LawyerClientSessionDetailID", DbType.Int32, mLawyerClientSessionDetailID)
        db.AddInParameter(cmd, "@ProblemID", DbType.Int32, mProblemID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_LawyerSessionProblems")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mLawyerSessionProblemID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblLawyerSessionProblems SET Deleted = 1 WHERE LawyerSessionProblemID = " & mLawyerSessionProblemID) 
        Return Delete("DELETE FROM tblLawyerSessionProblems WHERE LawyerSessionProblemID = " & mLawyerSessionProblemID)

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