Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class ProjectStaffIntervention

#region "Variables"

    Protected mProjectStaffInterventionID As long
    Protected mStaffID As Long
    Protected mOrganizationID As Long
    Protected mInterventionID As long
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

    Public  Property ProjectStaffInterventionID() As long
        Get
		return mProjectStaffInterventionID
        End Get
        Set(ByVal value As long)
		mProjectStaffInterventionID = value
        End Set
    End Property

    Public  Property StaffID() As long
        Get
		return mStaffID
        End Get
        Set(ByVal value As long)
		mStaffID = value
        End Set
    End Property

    Public Property OrganizationID() As Long
        Get
            Return mOrganizationID
        End Get
        Set(ByVal value As Long)
            mOrganizationID = value
        End Set
    End Property

    Public  Property InterventionID() As long
        Get
		return mInterventionID
        End Get
        Set(ByVal value As long)
		mInterventionID = value
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

    ProjectStaffInterventionID = 0
        mStaffID = 0
        mOrganizationID = 0
    mInterventionID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mProjectStaffInterventionID) 

    End Function 

    Public Overridable Function Retrieve(ByVal ProjectStaffInterventionID As Long) As Boolean 

        Dim sql As String 

        If ProjectStaffInterventionID > 0 Then 
            sql = "SELECT * FROM tblProjectStaffIntervention WHERE ProjectStaffInterventionID = " & ProjectStaffInterventionID
        Else 
            sql = "SELECT * FROM tblProjectStaffIntervention WHERE ProjectStaffInterventionID = " & mProjectStaffInterventionID
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

                log.Error("ProjectStaffIntervention not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetProjectStaffIntervention() As System.Data.DataSet

        Return GetProjectStaffIntervention(mProjectStaffInterventionID) 

    End Function 

    Public Overridable Function GetProjectStaffIntervention(ByVal ProjectStaffInterventionID As Long) As DataSet 

        Dim sql As String 

        If ProjectStaffInterventionID > 0 Then 
            sql = "SELECT * FROM tblProjectStaffIntervention WHERE ProjectStaffInterventionID = " & ProjectStaffInterventionID
        Else 
            sql = "SELECT * FROM tblProjectStaffIntervention WHERE ProjectStaffInterventionID = " & mProjectStaffInterventionID
        End If 

        Return GetProjectStaffIntervention(sql) 

    End Function 

    Protected Overridable Function GetProjectStaffIntervention(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function

    Public Function GetStaff(ByVal InterventionID As Long) As DataSet

        Dim sql As String = "SELECT M.StaffID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname, '') As StaffName, Position from tblStaffMembers M inner join tblProjectStaffIntervention PI on M.StaffID = PI.StaffID "
        sql &= "inner join tblInterventions I "
        sql &= "on I.InterventionID = PI.InterventionID where PI.InterventionID = " & InterventionID

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mProjectStaffInterventionID = Catchnull(.Item("ProjectStaffInterventionID"), 0)
            mStaffID = Catchnull(.Item("StaffID"), 0)
            mOrganizationID = Catchnull(.Item("Organization"), 0)
            mInterventionID = Catchnull(.Item("InterventionID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@ProjectStaffInterventionID", DBType.Int32, mProjectStaffInterventionID) 
        db.AddInParameter(cmd, "@StaffID", DbType.Int32, mStaffID)
        db.AddInParameter(cmd, "@OrganizationID", DbType.Int32, mOrganizationID)
        db.AddInParameter(cmd, "@InterventionID", DBType.Int32, mInterventionID) 
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_ProjectStaffIntervention") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mProjectStaffInterventionID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblProjectStaffIntervention SET Deleted = 1 WHERE ProjectStaffInterventionID = " & mProjectStaffInterventionID) 
        Return Delete("DELETE FROM tblProjectStaffIntervention WHERE ProjectStaffInterventionID = " & mProjectStaffInterventionID) 

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