Imports SysPermissionsManager.Functionality

Partial Public Class DefaultUserPermissions
    Inherits System.Web.UI.UserControl

    Public Event ReceiveControlMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum)
    Private Shared ReadOnly SecurityLog As log4net.ILog = log4net.LogManager.GetLogger("SecurityLogger")
    Private objUserPermissions As New Global.SysPermissionsManager.Functionality(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.ChangeAccessPermissions) Then


            Dim myEx As New ApplicationException("You are not athorised to view this page details.Contact your administrator")
            Throw myEx

        End If

        ComplementaryListboxes1.SelectedOptionsCaption = "Assigned User Functionalities"

        ComplementaryListboxes1.AvailableOptionsCaption = "Available User Functionalities"

        If Not Page.IsPostBack Then

            LoadUsers()
            LoadAvailableUserPermmisions(0)
            LoadAssignedUserPermmisions(0)

        End If

    End Sub

    Sub LoadAvailableUserPermmisions(ByVal UserID As Long)

        Dim objPermmisions As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(UserID) Then

            If UserID > 0 Then

                Dim ds As DataSet = objPermmisions.GetAvailableUserFunctionalities(UserID)

                ComplementaryListboxes1.AvailableOptions.DataSource = ds

                ComplementaryListboxes1.AvailableOptions.DataTextField = "Description"

                ComplementaryListboxes1.AvailableOptions.DataValueField = "FunctionalityID"

                ComplementaryListboxes1.AvailableOptions.DataBind()

            End If

        End If

    End Sub

    Sub LoadAssignedUserPermmisions(ByVal UserID As Long)

        Dim objPermmisions As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(UserID) Then

            If UserID > 0 Then

                Dim ds As DataSet = objPermmisions.GetSelectedUserFunctionalities(UserID)

                ComplementaryListboxes1.SelectedOptions.DataSource = ds

                ComplementaryListboxes1.SelectedOptions.DataTextField = "Description"

                ComplementaryListboxes1.SelectedOptions.DataValueField = "FunctionalityID"

                ComplementaryListboxes1.SelectedOptions.DataBind()

            End If

        End If

    End Sub

    Sub LoadUsers()

        Dim DataLookup As New BusinessLogic.CommonFunctions

        With lstUsers

            '.Items.Clear 'CHECK: Is it necessary to clear items before we load new data?
            .DataTextField = "Username"
            .DataValueField = "UserID"
            .DataSource = DataLookup.Lookup("tblUsers", "UserID", "Username", "Username", "Deleted=0 AND [Type] = 'SystemUser' ").Tables(0)
            .DataBind()

        End With

    End Sub

    Private Sub lstUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstUsers.SelectedIndexChanged

        If IsNumeric(lstUsers.SelectedValue) Then

            LoadAvailableUserPermmisions(lstUsers.SelectedValue)

            LoadAssignedUserPermmisions(lstUsers.SelectedValue)

        End If

    End Sub

    Protected Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click

        Try
            If Save(ComplementaryListboxes1, lstUsers, "User") Then

                RaiseEvent ReceiveControlMessage("User Permissions applied..", MessageTypeEnum.Information)
                SecurityLog.Info("User Permissions applied..")
            End If
        Catch ex As Exception
            RaiseEvent ReceiveControlMessage(ex.Message, MessageTypeEnum.Error)
        End Try

    End Sub

    Private Function Save(ByVal ucComplementaryListBox As ComplementaryListboxes, ByVal ctrListBox As ListBox, ByVal UserType As String) As Boolean

        Dim objUserFunctionality As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        'revoke all permissions for this user
        If ucComplementaryListBox.AvailableOptions.Items.Count > 0 AndAlso ctrListBox.SelectedValue = 1 Then

            ' we are attempting to revoke permissions for administrators... we should not allow this.
            RaiseEvent ReceiveControlMessage("Revoking Permissions for " & UserType & " " & ctrListBox.SelectedItem.Text & " is not allowed", MessageTypeEnum.Warning)
            Return False
            Exit Function

        End If

        Try
            objUserFunctionality.Revoke(ctrListBox.SelectedValue, UserType)

            'save selected
            For i As Integer = 0 To ucComplementaryListBox.SelectedOptions.Items.Count - 1

                ucComplementaryListBox.SelectedOptions.SelectedIndex = i

                If ucComplementaryListBox.SelectedOptions.SelectedValue <> "" Then

                    objUserFunctionality.SaveDetail(ctrListBox.SelectedValue, ucComplementaryListBox.SelectedOptions.SelectedValue, UserType)

                End If

            Next

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

End Class