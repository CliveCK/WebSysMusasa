
Partial Public Class DefaultGroupPermissions
    Inherits System.Web.UI.UserControl

    Public Event ReceiveControlMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum)
    Private Shared ReadOnly SecurityLog As log4net.ILog = log4net.LogManager.GetLogger("SecurityLogger")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComplementaryListboxes1.SelectedOptionsCaption = "Assigned Usergroup Functionalities"

        ComplementaryListboxes1.AvailableOptionsCaption = "Available Usergroup Functionalities"

        If Not Page.IsPostBack Then

            LoadRoles()
            LoadAvailablePermmisions(0)
            LoadAssignedPermmisions(0)

        End If

    End Sub

    Sub LoadAvailablePermmisions(ByVal UserGroupID As Integer)

        Dim objPermmisions As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(UserGroupID) Then

            If UserGroupID > 0 Then

                Dim ds As DataSet = objPermmisions.GetAvailableUserGroupFunctionalities(UserGroupID)

                ComplementaryListboxes1.AvailableOptions.DataSource = ds

                ComplementaryListboxes1.AvailableOptions.DataTextField = "Description"

                ComplementaryListboxes1.AvailableOptions.DataValueField = "FunctionalityID"

                ComplementaryListboxes1.AvailableOptions.DataBind()

            End If

        End If

    End Sub

    Sub LoadAssignedPermmisions(ByVal UserGroupID As Integer)

        Dim objPermmisions As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(UserGroupID) Then

            If UserGroupID > 0 Then

                Dim ds As DataSet = objPermmisions.GetSelectedUserGroupFunctionalities(UserGroupID)

                ComplementaryListboxes1.SelectedOptions.DataSource = ds

                ComplementaryListboxes1.SelectedOptions.DataTextField = "Description"

                ComplementaryListboxes1.SelectedOptions.DataValueField = "FunctionalityID"

                ComplementaryListboxes1.SelectedOptions.DataBind()

            End If

        End If


    End Sub

    Sub LoadRoles()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With lstRoles

            '.Items.Clear 'CHECK: Is it necessary to clear items before we load new data?
            .DataTextField = "Description"
            .DataValueField = "UserGroupID"
            .DataSource = objLookup.Lookup("luUserGroups", "UserGroupID", "Description", "Description").Tables(0)
            .DataBind()

        End With

    End Sub

    Private Sub lstRoles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstRoles.SelectedIndexChanged

        If IsNumeric(lstRoles.SelectedValue) Then

            LoadAvailablePermmisions(lstRoles.SelectedValue)

            LoadAssignedPermmisions(lstRoles.SelectedValue)

            txtDescription.Text = lstRoles.SelectedItem.Text

            txtUserGroupID.Text = lstRoles.SelectedValue

        End If

    End Sub

    Protected Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click

        Try
            If Save(ComplementaryListboxes1, lstRoles, "UserGroup") Then

                RaiseEvent ReceiveControlMessage("UserGroup Permissions applied..", MessageTypeEnum.Information)
                SecurityLog.Info("UserGroup Permissions applied..")

                CacheWrapper.MenuItemsAllCache = Nothing
                CacheWrapper.MenuRightsCache = Nothing
                CacheWrapper.PageRightsCache = Nothing
                CacheWrapper.MenuItemsCache = Nothing

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

    Protected Sub cmaSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmaSave.Click

        Dim objUserGroup As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If txtDescription.Text = "" Then

            RaiseEvent ReceiveControlMessage("Please enter the description!", MessageTypeEnum.Error)

            Exit Sub

        End If

        With objUserGroup

            Try

                If txtUserGroupID.Text = 0 Then ' we are creatin a usergroup

                    .CreateUsergroup(txtDescription.Text)

                    LoadRoles()

                Else

                    'edit 
                    .EditUserGroup(lstRoles.SelectedValue, txtDescription.Text)
                    Dim objSelectedvalue As Long = lstRoles.SelectedValue


                    LoadRoles()

                    If IsNumeric(objSelectedvalue) Then

                        lstRoles.SelectedValue = objSelectedvalue

                    End If
                End If

                RaiseEvent ReceiveControlMessage("Details saved..", MessageTypeEnum.Error)

            Catch ex As Exception

                RaiseEvent ReceiveControlMessage(ex.Message, MessageTypeEnum.Error)

            End Try

        End With


    End Sub
End Class