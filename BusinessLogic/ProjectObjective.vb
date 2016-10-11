﻿Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ProjectObjective

#region "Variables"

    Protected mProjectObjectiveID As long
    Protected mProjectID As Long
    Protected mObjectiveID As Long
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

    Public  Property ProjectObjectiveID() As long
        Get
		return mProjectObjectiveID
        End Get
        Set(ByVal value As long)
		mProjectObjectiveID = value
        End Set
    End Property

    Public  Property ProjectID() As long
        Get
		return mProjectID
        End Get
        Set(ByVal value As long)
		mProjectID = value
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

    Public Property ObjectiveID() As Long
        Get
            Return mObjectiveID
        End Get
        Set(ByVal value As Long)
            mObjectiveID = value
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

    ProjectObjectiveID = 0
    mProjectID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
        mObjectiveID = 0

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mProjectObjectiveID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ProjectObjectiveID As Long) As Boolean 

        Dim sql As String 

        If ProjectObjectiveID > 0 Then 
            sql = "SELECT * FROM tblProjectObjectives WHERE ProjectObjectiveID = " & ProjectObjectiveID
        Else 
            sql = "SELECT * FROM tblProjectObjectives WHERE ProjectObjectiveID = " & mProjectObjectiveID
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

                log.Warn("ProjectObjective not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetProjectObjective() As System.Data.DataSet

        Return GetProjectObjective(mProjectObjectiveID) 

    End Function 

    Public Overridable Function GetProjectObjective(ByVal ProjectObjectiveID As Long) As DataSet 

        Dim sql As String 

        If ProjectObjectiveID > 0 Then 
            sql = "SELECT * FROM tblProjectObjectives WHERE ProjectObjectiveID = " & ProjectObjectiveID
        Else 
            sql = "SELECT * FROM tblProjectObjectives WHERE ProjectObjectiveID = " & mProjectObjectiveID
        End If 

        Return GetProjectObjective(sql) 

    End Function 

    Protected Overridable Function GetProjectObjective(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function 

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mProjectObjectiveID = Catchnull(.Item("ProjectObjectiveID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mObjectiveID = Catchnull(.Item("ObjectiveID"), 0)

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@ProjectObjectiveID", DBType.Int32, mProjectObjectiveID) 
        db.AddInParameter(cmd, "@ProjectID", DBType.Int32, mProjectID) 
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID) 
        db.AddInParameter(cmd, "@ObjectiveID", DbType.String, mObjectiveID)

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ProjectObjective") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mProjectObjectiveID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblProjectObjectives SET Deleted = 1 WHERE ProjectObjectiveID = " & mProjectObjectiveID) 
        Return Delete("DELETE FROM tblProjectObjectives WHERE ProjectObjectiveID = " & mProjectObjectiveID) 

    End Function

    Public Function DeleteEntries() As Boolean

        'Return Delete("UPDATE tblProjectObjectives SET Deleted = 1 WHERE ProjectObjectiveID = " & mProjectObjectiveID) 
        Return Delete("DELETE FROM tblProjectObjectives WHERE ProjectID = " & mProjectID & " AND ObjectiveID = " & mObjectiveID)

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