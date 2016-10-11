Public Class ProjectListing
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadContextMenu()

            If Request.QueryString("op") = "v" Then 'Status 0: Archived - 1: Active

                LoadGrid(0)
                lblStatus.Text = "Archived"
                lblStatus.ForeColor = Drawing.Color.Red

            Else

                LoadGrid(1)
                lblStatus.Text = "Active"
                lblStatus.ForeColor = Drawing.Color.Green

            End If

        End If

    End Sub

    Public Sub LoadGrid(ByVal StatusID As Int16)

        Session("Projects") = Nothing

        Dim objProjects As New BusinessLogic.Projects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objProjects.GetAllProjects(StatusID)

        With radProjectListing

            .DataSource = ds
            .DataBind()

            Session("Projects") = .DataSource

        End With

    End Sub

    Private Sub LoadContextMenu()

        Dim objMenu As New BusinessLogic.Menu(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objMenu.GetContextMenu()
        Session("Menu") = ds

        With radMContextMenu

            .DataSource = ds
            .DataTextField = "MenuName"
            .DataValueField = "MenuID"
            .DataFieldID = "MenuID"
            .DataFieldParentID = "ParentID"

            .DataBind()

            .AppendDataBoundItems = True

        End With

    End Sub

    Private Sub radProjectListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radProjectListing.ItemCommand

    End Sub

    Private Sub radProjectListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radProjectListing.NeedDataSource

        radProjectListing.DataSource = Session("Projects")

    End Sub

    Private Sub txtMenuID_TextChanged(sender As Object, e As EventArgs) Handles txtMenuID.TextChanged

      

    End Sub

    Private Sub radMContextMenu_ItemClick(sender As Object, e As Telerik.Web.UI.RadMenuEventArgs) Handles radMContextMenu.ItemClick

        Dim radGridClickedRowIndex As Integer
        radGridClickedRowIndex = Convert.ToInt32(Request.Form("radGridClickedRowIndex"))

        Dim griddataitem As Telerik.Web.UI.GridDataItem = radProjectListing.Items.Item(radGridClickedRowIndex)
        Dim mID As String = griddataitem.GetDataKeyValue("Project").ToString()
        Dim MenuID As Long = txtMenuID.Text

        Dim ds As DataSet = Session("Menu")

        Dim rows As DataRow() = ds.Tables(0).Select("MenuID=" & e.Item.Value)

        Dim row As DataRow = rows(0)

        Dim RedirectURL As String = row("URL").ToString

        'Dim dv As DataView = ds.Tables(0).DefaultView

        'dv.RowFilter = "MenuID=" & MenuID

        'Dim RedirectURL As String = dv.Table.Rows(0)("URL").ToString

        If RedirectURL <> DBNull.Value.ToString Then

            If RedirectURL <> "" Then

                Response.Redirect(RedirectURL & "?id=" & objUrlEncoder.Encrypt(mID))

            End If

        End If

    End Sub
End Class