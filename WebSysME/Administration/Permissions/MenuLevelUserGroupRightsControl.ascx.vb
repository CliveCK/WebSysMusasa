Imports SysPermissionsManager.Functionality
Imports Telerik.Web.UI

Partial Public Class MenuLevelUserGroupRightsControl
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

            LoadRoles()
            'LoadTreeViewData(0)
            'LoadContextTreeViewData(0)
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

            txtUserGroupID.Text = lstRoles.SelectedValue
            LoadTreeViewData(lstRoles.SelectedValue)
            'LoadContextTreeViewData(lstRoles.SelectedValue)

        End If

    End Sub

    Public Sub LoadTreeViewData(ByVal UserGroupID As Integer)

    
        Try
            Session("UserGroupMenuRights") = Nothing

            Dim ds As DataSet = CacheWrapper.MainMenuItemsCache

            Dim objPermmisions As New Global.SysPermissionsManager.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Session("UserGroupMenuRights") = objPermmisions.GetSelectedUserGroupMenuRights(UserGroupID)

            With RadTreeUserGroupRights

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
            LoadTreeViewData(0)
        End Try

    End Sub

    Private Sub RadTreeUserGroupContextRights_NodeBound(ByVal o As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles RadTreeUserGroupContextRights.NodeDataBound

        Try

            Dim dr As DataRowView = e.Node.DataItem

            If Not dr Is Nothing Then

                If e.Node.Enabled Then e.Node.CollapseParentNodes()

                If IsNumeric(txtUserGroupID.Text) AndAlso txtUserGroupID.Text > 0 Then

                    Dim ds As DataSet = Session("UserGroupContextMenuRights")

                    If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                        For Each row As DataRow In ds.Tables(0).Rows

                            If e.Node.Value = row("MenuID") Then
                                e.Node.Checked = True
                            End If

                        Next

                        ShowMessage("Menu Rights retrieved successfully", MessageTypeEnum.Information)

                    End If

                End If

            End If

        Catch : End Try

    End Sub

    Protected Function SaveUsercheckedNodes(ByVal RadTreeUserRights As Telerik.Web.UI.RadTreeView, ByVal UserType As String, ByVal objPermission As SysPermissionsManager.MenuLevelAccess) As Boolean

        Dim NodeFullPath As String = ""
        Dim MenuID As Integer

        Try


            If RadTreeUserRights.CheckedNodes.Count > 0 Then


                Dim ds As DataSet = CreateErrorDataset()

                For Each Node As RadTreeNode In RadTreeUserRights.CheckedNodes

                    NodeFullPath = Node.FullPath
                    MenuID = Node.Value
                    objPermission.SaveDetail(txtUserGroupID.Text, MenuID, UserType)

                Next Node
                DisplayErrors(ds)
                Return True

            End If


        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        Dim objPermission As New Global.SysPermissionsManager.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        objPermission.Revoke(txtUserGroupID.Text, "UserGroup")    'revoke existing user rights

        If RadTreeUserGroupRights.CheckedNodes.Count > 0 Then
            If SaveUsercheckedNodes(RadTreeUserGroupRights, "UserGroup", objPermission) Then
                ShowMessage("Details saved!", MessageTypeEnum.Information)
            Else
                ShowMessage("Failed to apply permissions!", MessageTypeEnum.Error)
            End If
        Else
            ShowMessage("Details saved!", MessageTypeEnum.Information)
        End If

        If RadTreeUserGroupContextRights.CheckedNodes.Count > 0 Then
            If SaveUsercheckedNodes(RadTreeUserGroupContextRights, "UserGroup", objPermission) Then
                ShowMessage("Details saved!", MessageTypeEnum.Information)
            Else
                ShowMessage("Failed to apply permissions!", MessageTypeEnum.Error)
            End If
        Else
            ShowMessage("Details saved!", MessageTypeEnum.Information)
        End If



    End Sub

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

    'Public Sub LoadContextTreeViewData(ByVal UserGroupID As Integer)

    '    Try

    '        Dim objUserMenu As New UserMenu(CookiesWrapper.thisConnectionName)
    '        Session("UserGroupContextMenuRights") = Nothing

    '        Dim objPermmisions As New Global.SysPermissionsManager.MenuLevelAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
    '        Session("UserGroupContextMenuRights") = objPermmisions.GetSelectedUserGroupContextMenuRights(UserGroupID)

    '        Dim ds As DataSet = objUserMenu.GetNonMainMenuItems

    '        With RadTreeUserGroupContextRights

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
    '        LoadContextTreeViewData(0)
    '    End Try

    'End Sub

    Private Sub RadTreeUserRights_NodeBound(ByVal o As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles RadTreeUserGroupRights.NodeDataBound

        Try

            Dim dr As DataRowView = e.Node.DataItem

            If Not dr Is Nothing Then

                If e.Node.Enabled Then e.Node.CollapseParentNodes()

                If IsNumeric(txtUserGroupID.Text) AndAlso txtUserGroupID.Text > 0 Then

                    Dim ds As DataSet = Session("UserGroupMenuRights")

                    If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                        For Each row As DataRow In ds.Tables(0).Rows

                            If e.Node.Value = row("MenuID") Then
                                e.Node.Checked = True
                            End If

                        Next

                        ShowMessage("Menu Rights retrieved successfully", MessageTypeEnum.Information)

                    End If

                End If

            End If

        Catch : End Try

    End Sub

End Class

