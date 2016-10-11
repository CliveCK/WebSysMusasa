Imports BusinessLogic.CommonFunctions
Imports Microsoft.Practices.EnterpriseLibrary.Data


Public Class Functionality


#Region "Variables"

    Private mFunctionalityID As Integer
    Private mUserGroupIDs() As String
    Private mGroupArraySize As Integer
    Private mStringUserGroupIDs As String


    Private db As Database
    Private mConnectionName As String
    Private mObjectUserID As Long

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

    Public Property FunctionalityID() As Integer
        Get
            Return mFunctionalityID
        End Get
        Set(ByVal value As Integer)
            mFunctionalityID = value
        End Set
    End Property

    Public Property UserGroupIDs(ByVal ArraySize As Integer) As String
        Get
            Return mUserGroupIDs(ArraySize)
        End Get
        Set(ByVal value As String)
            mUserGroupIDs(ArraySize) = value
        End Set
    End Property

    Public Property GroupArraySize() As Integer
        Get
            Return mGroupArraySize
        End Get
        Set(ByVal value As Integer)
            mGroupArraySize = value
        End Set
    End Property

    Public Property StringUserGroupIDs() As String
        Get
            Return mStringUserGroupIDs
        End Get
        Set(ByVal value As String)
            mStringUserGroupIDs = value
        End Set
    End Property
#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        Dim factory As DatabaseProviderFactory = New DatabaseProviderFactory()
        db = factory.Create(ConnectionName)

    End Sub

#End Region

#Region "Authentication"

    Public Function AuthenticateUserGroupFunctionalities(ByVal UserID As Long, ByVal FunctionalityID As Long) As Boolean

        Dim sql As String = "SELECT * FROM tblUserFunctionalities  WHERE [FunctionalityID] = " & FunctionalityID & " AND [UserID] IN (SELECT [UserGroupID] FROM tblUserUsergroups WHERE UserID  = " & UserID & ") AND UserType='UserGroup'"
        ' Assuming ADministrator is always 1

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

    Public Function AuthenticateUserFunctionalities(ByVal UserID As Long, ByVal FunctionalityID As Long) As Boolean

        Dim sql As String = "SELECT * FROM tblUserFunctionalities  WHERE [FunctionalityID] = " & FunctionalityID & " AND  UserID  = " & UserID & " AND UserType='User'" ' Assuming ADministrator is always 1

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

    Public Function AuthenticateRoleFunctionalities(ByVal UserID As Long, ByVal FunctionalityID As FunctionalityEnum) As Boolean

        Dim sql As String = "SELECT * FROM tblRolesFunctionalities  WHERE [FunctionalityID] = " & FunctionalityID & " AND [RoleID] IN (SELECT [RoleID] FROM tblUserRoles WHERE UserID  = " & UserID & " )"
        ' Assuming ADministrator is always 1
        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then
            Return True
        Else : Return False
        End If

    End Function

    Public Function CheckGroupIfUserIsInGroup(ByVal UserID As Long) As Boolean

        Dim sql As String = "SELECT RoleID FROM dbo.tblUserRoles where UserID=" & UserID

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        Dim StringRoleIDs As String = ""

        If ds.Tables(0).Rows.Count > 0 Then

            mGroupArraySize = ds.Tables(0).Rows.Count

            ReDim mUserGroupIDs(mGroupArraySize)

            For intCounter As Integer = 0 To ds.Tables(0).Rows.Count - 1

                mUserGroupIDs(intCounter) = ds.Tables(0).Rows(intCounter).Item(0).ToString

                mStringUserGroupIDs &= IIf(mStringUserGroupIDs = "", "", " , ") & mUserGroupIDs(intCounter)

            Next

            Return True

        Else : Return False

        End If

    End Function

    Public Function IsAdministrator(ByVal UserID As Long) As Boolean

        Dim sql As String = "SELECT * FROM tblUserUserGroups WHERE UserGroupID = 1 AND UserID=" & UserID

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

#End Region

#Region "Functionality Checks"

    Public Function CheckFunctionalityPermission(ByVal UserID As Long, ByVal FunctionalityID As Long) As Boolean

        If IsAdministrator(UserID) Then ' 

            Return True ' adminis do not have restrictions

        Else

            If AuthenticateUserFunctionalities(UserID, FunctionalityID) Then
                ' apply specific user permissions 
                Return True

            Else 'No Specific User Permission are available so apply group permissionz.

                If AuthenticateUserGroupFunctionalities(UserID, FunctionalityID) Then
                    'Do nothing
                    Return True

                Else

                    Return False

                End If

            End If


        End If

    End Function

    'Public Function CheckMemberPagePermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ViewMemberDetails) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckDeleteMembersPermissions(ByVal UserID As Long) As Boolean
    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.DeleteMembers) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try

    'End Function

    'Public Function CheckAddOrEditPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.AddOrEditMembers) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckAddDeleteSubscriptionsPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.AddDeleteSubscriptions) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckChangeRatesPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ChangeRates) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckRunBillingPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.RunBilling) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckViewReportsPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ViewReports) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckDeleteUsersPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.DeleteUsers) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckAddOrEditUsersPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.AddOrEditUsers) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckChangeAccessPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ChangeAccessPermissions) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckAddOrEditOrDeleteParametersPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.AddOrEditOrDeleteParameters) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckSetupAuditTrailPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.SetupAuditTrail) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try

    'End Function

    'Public Function CheckViewAuditLogsPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ViewAuditLogs) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try

    'End Function

    'Public Function CheckViewExceptionReportsPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ViewExceptionReports) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckCreateMemberAccountsPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.CreateMemberAccounts) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckAddOrEditPackagesAndSchemesPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.AddOrEditPackagesAndSchemes) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckScheduleMailPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ScheduleMail) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckAddOrEditSegregatedFundsPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.AddOrEditSegregatedFunds) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckAdministerImportedMemberAccountsPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.AdministerImportedMemberAccounts) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckAddOrEditOrDeleteSegregatedFundsPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.AddOrEditOrDeleteSegregatedFunds) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckReverseBillingPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ReverseBilling) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckViewBillingHistoryPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ViewBillingHistory) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckReverseReceiptPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ReverseReceipt) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

    'Public Function CheckViewReceiptHistoryPermissions(ByVal UserID As Long) As Boolean

    '    Try

    '        If Not CheckFunctionalityPermission(UserID, FunctionalityEnum.ViewReceiptHistory) Then
    '            Return False
    '        Else
    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ex.ToString()
    '        Return False
    '    End Try
    'End Function

#End Region

    Public Function GetSelectedUserFunctionalityRights(ByVal UserID As Long) As DataSet

        Dim script As New System.Text.StringBuilder("")

        script.AppendLine("SELECT * FROM tblUserFunctionalities ")
        script.AppendLine("WHERE FunctionalityID IN (")
        script.AppendLine("	SELECT FunctionalityID FROM tblUserFunctionalities where UserID = " & UserID & " AND UserType='User' ")
        script.AppendLine("	UNION")
        script.AppendLine("	SELECT FunctionalityID FROM tblUserFunctionalities WHERE UserType='UserGroup' AND UserID IN (SELECT UserGroupID FROM [tblUserUserGroups] WHERE [tblUserUserGroups].UserID = " & UserID & ")")
        script.AppendLine(") ")

        Return db.ExecuteDataSet(CommandType.Text, script.ToString)

    End Function

#End Region

    Public Enum FunctionalityEnum As Integer

        [AddOrEditProjects] = 1
        [DeleteProjects] = 2
        [ViewProjectDetails] = 3
        [AddOrDeleteFiles] = 4
        [DownloadFiles] = 5
        [AddOrEditOrganizationDetails] = 6
        [ViewReports] = 7
        [AddOrEditUsers] = 8
        [DeleteUsers] = 9
        [ChangeAccessPermissions] = 10
        [AddOrEditOrDeleteParameters] = 11
        [SetupAuditTrail] = 12
        [ViewAuditLogs] = 13
        [AddOrEditStaffMembers] = 14
        [AddOrEditTrainingMarks] = 15
        [AddOrEditBeneficiaryDetails] = 16
        [AddOrEditPatientDetails] = 17
        [AddOrEditSecurityPolicies] = 18
        [ViewOrganizationDetails] = 19
        [UpdateContractReportsStatus] = 20
        [AllowViewAdminDetails] = 21

    End Enum

End Class

