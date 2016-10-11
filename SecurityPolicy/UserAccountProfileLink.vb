Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class UserAccountProfileLink

#region "Variables"

    Protected mUserAccountProfileLink As long
    Protected mUserID As long
    Protected mStaffID As long
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

    Public  Property UserAccountProfileLink() As long
        Get
		return mUserAccountProfileLink
        End Get
        Set(ByVal value As long)
		mUserAccountProfileLink = value
        End Set
    End Property

    Public  Property UserID() As long
        Get
		return mUserID
        End Get
        Set(ByVal value As long)
		mUserID = value
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

    UserAccountProfileLink = 0
    mUserID = 0
    mStaffID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mUserAccountProfileLink) 

    End Function 

    Public Overridable Function Retrieve(ByVal UserAccountProfileLink As Long) As Boolean 

        Dim sql As String 

        If UserAccountProfileLink > 0 Then 
            sql = "SELECT * FROM tblUserAccountProfileLink WHERE UserAccountProfileLink = " & UserAccountProfileLink
        Else 
            sql = "SELECT * FROM tblUserAccountProfileLink WHERE UserAccountProfileLink = " & mUserAccountProfileLink
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

                log.error("UserAccountProfileLink not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetUserAccountProfileLink() As System.Data.DataSet

        Return GetUserAccountProfileLink(mUserAccountProfileLink)

    End Function

    Protected Overridable Function GetUserAccountProfileLink(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mUserAccountProfileLink = Catchnull(.Item("UserAccountProfileLink"), 0)
            mUserID = Catchnull(.Item("UserID"), 0)
            mStaffID = Catchnull(.Item("StaffID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@UserAccountProfileLink", DBType.Int32, mUserAccountProfileLink)
        db.AddInParameter(cmd, "@UserID", DBType.Int32, mUserID)
        db.AddInParameter(cmd, "@StaffID", DBType.Int32, mStaffID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_UserAccountProfileLink")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mUserAccountProfileLink = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblUserAccountProfileLink SET Deleted = 1 WHERE UserAccountProfileLink = " & mUserAccountProfileLink) 
        Return Delete("DELETE FROM tblUserAccountProfileLink WHERE UserAccountProfileLink = " & mUserAccountProfileLink)

    End Function

    Public Overridable Function DeleteByUserID(ByVal UserID As Long) As Boolean

        'Return Delete("UPDATE tblUserAccountProfileLink SET Deleted = 1 WHERE UserAccountProfileLink = " & mUserAccountProfileLink) 
        Return Delete("DELETE FROM tblUserAccountProfileLink WHERE UserID = " & UserID)

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


#Region "Öther Methods"

    Public Function CheckUserAccountProfileLink(ByVal UserID As Long) As Boolean

        Dim sql As String = "SELECT * FROM tblUsers U inner join tblUserAccountProfileLink A on A.UserID = U.UserID  "
        sql &= " inner join tblStaffMembers S on S.StaffID = A.StaffID  WHERE U.UserID = " & UserID

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count Then

            Return True

        Else

            Return False

        End If

    End Function

    Public Function GetUserIDByStaffOrOrganizationID(ByVal ObjectID As Integer, ByVal ObjectType As String) As Long

        Select Case ObjectType

            Case "Staff"
                Dim sql As String = "SELECT U.UserID FROM tblUsers U inner join tblUserAccountProfileLink A on A.UserID = U.UserID  "
                sql &= " inner join tblStaffMembers S on S.StaffID = A.StaffID  WHERE S.StaffID = " & ObjectID

                Dim ds As DataSet = GetUserAccountProfileLink(sql)
                If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    Return Catchnull(ds.Tables(0).Rows(0)(0), 0)

                End If

                Return 0

        End Select

    End Function

    Public Overridable Function GetUserAccountProfileLink(ByVal UserID As Long) As DataSet

        Dim sql As String = "SELECT * FROM tblUsers U inner join tblUserAccountProfileLink A on A.UserID = U.UserID  "
        sql &= " inner join tblStaffMembers S on S.StaffID = A.StaffID  WHERE U.UserID = " & UserID

        Return GetUserAccountProfileLink(sql)

    End Function

#End Region
#End Region

End Class