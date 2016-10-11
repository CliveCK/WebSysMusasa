Public Partial Class FindUsers
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly SecurityLog As log4net.ILog = log4net.LogManager.GetLogger("SecurityLogger")

    Sub InitialisePageObjects()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        InitialisePageObjects()

    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)

        CType(CType(sender, CheckBox).Parent.Parent, Telerik.Web.UI.GridItem).Selected = CType(sender, CheckBox).Checked

    End Sub

    Private Sub rdResults_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rdResults.ItemCommand

        If (TypeOf e.Item Is Telerik.Web.UI.GridDataItem) Then

            Dim gridDataItem As Telerik.Web.UI.GridDataItem = e.Item

            Select Case e.CommandName

                Case "DeactivateUser"

                    Dim objUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                    If objUser.DeactivateUserByUserID(gridDataItem("UserID").Text) Then


                        SecurityLog.Info(txtUsername.Text.ToUpper() & " User deactivated. User IP Address: " & WebHelper.GetUserIPAddress())
                        lblStatus.CssClass = "msgInformation"
                        lblStatus.Text = "User Has Been Successfully Deactivated.."


                    End If

                    cmdSavePermissions.Visible = False

                Case "ActivateUser"

                    Dim objUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                    If objUser.ActivateUserByUserID(gridDataItem("UserID").Text) Then


                        SecurityLog.Info(txtUsername.Text.ToUpper() & " User deactivated. User IP Address: " & WebHelper.GetUserIPAddress())
                        lblStatus.CssClass = "msgInformation"
                        lblStatus.Text = "User Has Been Successfully Deactivated.."


                    End If

                    cmdSavePermissions.Visible = False

                Case "Select"

                    Session("UserID") = gridDataItem("UserID").Text

                    Response.Redirect("~/UserAdministration.aspx?op=eu")

            End Select

        End If

        PopulateGrid()

    End Sub

    Private Sub rdResults_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles rdResults.ItemDataBound

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            Dim gridItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim DeactivateButton As Button = DirectCast(gridItem.FindControl("btnDeact"), Button)
            Dim ActivateButton As Button = DirectCast(gridItem.FindControl("btnAct"), Button)

            If gridItem("Deleted").Text = "True" Then

                ActivateButton.Visible = True

            End If

            If gridItem("Deleted").Text = "False" Then

                DeactivateButton.Visible = True

            End If

        End If

    End Sub

    Protected Sub rdResults_NeedDataSource(ByVal source As System.Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rdResults.NeedDataSource

        rdResults.DataSource = Session("FindUserResults")

    End Sub

    Protected Sub cmdFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFind.Click

        PopulateGrid()

    End Sub

    Sub PopulateGrid()
        Try
            Session("FindUserResults") = (New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)).FindUsers(pnlFindUsers)

            With rdResults

                .DataSource = Session("FindUserResults")
                .DataBind()

            End With

            lblStatus.Text = ""
        Catch ex As Exception
            lblMsg.CssClass = "msgError"
            lblMsg.Text = ex.ToString
        End Try

    End Sub

End Class