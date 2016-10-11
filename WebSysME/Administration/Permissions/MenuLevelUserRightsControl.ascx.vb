Imports SpectrumITS.MemberManagement.BusinessLogic
Imports Telerik.Web.UI


Partial Public Class MenuLevelRightsControl
    Inherits System.Web.UI.UserControl

    Private Shared log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Event ReceiveControlMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

            lblError.Text = Message.Message
            If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
            If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If Not Page.IsPostBack Then

            LoadUsers()
            'LoadUserTreeViewData(0)
            'LoadUserContextTreeViewData(0)

        End If


    End Sub

    Sub LoadUsers()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With lstUsers

            '.Items.Clear 'CHECK: Is it necessary to clear items before we load new data?
            .DataTextField = "Username"
            .DataValueField = "UserID"
            .DataSource = objLookup.Lookup("tblUsers", "UserID", "Username", "Username", "Deleted=0 AND [Type] = 'SystemUser' ").Tables(0)
            .DataBind()

        End With

    End Sub

    Sub lstUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstUsers.SelectedIndexChanged

        If IsNumeric(lstUsers.SelectedValue) Then
            txtUserID.Text = lstUsers.SelectedValue
            LoadUserTreeViewData(lstUsers.SelectedValue)
            'LoadUserContextTreeViewData(lstUsers.SelectedValue)
        End If

    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        Dim objPermission As New Global.SysPermissionsManager.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        objPermission.Revoke(txtUserID.Text, "User")
        If RadTreeUserRights.CheckedNodes.Count > 0 Then
            If SaveUsercheckedNodes(RadTreeUserRights, "User", objPermission) Then
                ShowMessage("Details saved!", MessageTypeEnum.Information)
            Else
                ShowMessage("Failed to apply permissions!", MessageTypeEnum.Error)
            End If
        End If

    End Sub

    Sub LoadUserTreeViewData(ByVal UserID As Integer)

        Try
            Session("UserMenuRights") = Nothing
            Dim ds As DataSet = CacheWrapper.MainMenuItemsCache

            Dim objPermmisions As New Global.SysPermissionsManager.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Session("UserMenuRights") = objPermmisions.GetSelectedUserMenuRights(UserID)


            With RadTreeUserRights()

                .DataFieldID = "MenuID"
                .DataFieldParentID = "ParentID"
                .DataTextField = "MenuName"
                .DataValueField = "MenuID"
                .DataSource = ds
                .DataBind()
                .CollapseAllNodes()

            End With

        Catch ex As Exception
            log.Error(ex)
            ShowMessage(ex.Message, MessageTypeEnum.Error)
            LoadUserTreeViewData(0)
            ShowMessage(ex.Message, MessageTypeEnum.Error)
        End Try

    End Sub

    Private Sub radtreeuserrights_NodeBound(ByVal o As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles RadTreeUserRights.NodeDataBound

        Try

            Dim dr As DataRowView = e.Node.DataItem

            If Not dr Is Nothing Then

                If e.Node.Enabled Then e.Node.CollapseParentNodes()

                If IsNumeric(txtUserID.Text) AndAlso txtUserID.Text > 0 Then

                    Dim ds As DataSet = Session("UserMenuRights")

                    If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                        For Each row As DataRow In ds.Tables(0).Rows

                            If e.Node.Value = row("MenuID") Then
                                e.Node.Checked = True
                            End If

                        Next

                        ShowMessage("Permissions retrieved successfully", MessageTypeEnum.Information)

                    End If

                End If

            End If

        Catch : End Try

    End Sub

    Protected Function SaveUsercheckedNodes(ByVal rdgUserRights As Telerik.Web.UI.RadTreeView, ByVal UserType As String, ByVal objPermission As SysPermissionsManager.MenuLevelAccess) As Boolean

        Dim NodeFullPath As String = ""
        Dim MenuID As Integer

        Try

            'revoke existing user rights

            If rdgUserRights.CheckedNodes.Count > 0 Then

                Dim ds As DataSet = CreateErrorDataset()

                For Each Node As RadTreeNode In rdgUserRights.CheckedNodes


                    NodeFullPath = Node.FullPath
                    MenuID = Node.Value
                    objPermission.SaveDetail(txtUserID.Text, MenuID, UserType)

                Next Node
                DisplayErrors(ds)
                Return True

            End If

        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Function CreateErrorDataset() As DataSet

        Dim ds As New DataSet
        ds.Tables.Add("Errors")

        With ds.Tables("Errors").Columns

            .Add(New DataColumn("Error", GetType(String)))
            .Add(New DataColumn("Details", GetType(String)))

        End With

        Return ds

    End Function

    Private Function DisplayErrors(ByVal dsErrors As DataSet) As Boolean

        If dsErrors.Tables(0).Rows.Count > 0 Then

            Dim msg As String = ""

            For Each row As DataRow In dsErrors.Tables(0).Rows

                msg = "<span class = 'Error'>" & row("Details") & " </span> <br/>"

            Next

        End If

    End Function

    'Public Sub LoadUserContextTreeViewData(ByVal UserID As Integer)

    '    Try
    '        Dim objUserMenu As New UserMenu(CookiesWrapper.thisConnectionName)
    '        Session("UserContextMenuRights") = Nothing

    '        Dim ds As DataSet = objUserMenu.GetNonMainMenuItems
    '        Dim objPermmisions As New Global.SpectrumITS.PermissionsManager.BusinessLogic.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
    '        Session("UserContextMenuRights") = objPermmisions.GetSelectedUserContextMenuRights(UserID)

    '        With RadTreeContextUserRights

    '            .DataFieldID = "MenuID"
    '            .DataFieldParentID = "ParentID"
    '            .DataTextField = "MenuName"
    '            .DataValueField = "MenuID"
    '            .DataSource = ds
    '            .DataBind()
    '            .CollapseAllNodes()

    '        End With



    '    Catch ex As Exception
    '        log.Error(ex)
    '        ShowMessage(ex.Message, MessageTypeEnum.Error)
    '        LoadUserContextTreeViewData(0)
    '    End Try

    'End Sub

    'Private Sub RadTreeContextUserRights_NodeBound(ByVal o As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles RadTreeContextUserRights.NodeDataBound

    '    Try

    '        Dim dr As DataRowView = e.Node.DataItem

    '        If Not dr Is Nothing Then

    '            If e.Node.Enabled Then e.Node.CollapseParentNodes()

    '            If IsNumeric(txtUserID.Text) AndAlso txtUserID.Text > 0 Then

    '                Dim ds As DataSet = Session("UserContextMenuRights")

    '                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

    '                    For Each row As DataRow In ds.Tables(0).Rows

    '                        If e.Node.Value = row("MenuID") Then
    '                            e.Node.Checked = True
    '                        End If

    '                    Next

    '                    ShowMessage("Menu Rights retrieved successfully", MessageTypeEnum.Information)

    '                End If

    '            End If

    '        End If

    '    Catch ex As Exception
    '        log.Error(ex)
    '    End Try

    'End Sub

End Class




