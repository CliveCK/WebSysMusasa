Imports Telerik.Web.UI

Public Class ViewKeyChangePromises
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Private Sub LoadGrid()

        Dim objKeyChange As New BusinessLogic.KeyChangePromises(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radKeyListing

            .DataSource = objKeyChange.RetriveAll().Tables(0)
            .DataBind()

            ViewState("KeyChange") = .DataSource

        End With

    End Sub

    Private Sub radKeyListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radKeyListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radKeyListing.Items(index)
            Dim KeyChangeID As Integer

            KeyChangeID = Server.HtmlDecode(item("KeyChangePromiseID").Text)

            Response.Redirect("~/KeyChangePromise.aspx?id=" & objUrlEncoder.Encrypt(KeyChangeID))

        End If

    End Sub

    Private Sub radKeyListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radKeyListing.NeedDataSource

        radKeyListing.DataSource = DirectCast(ViewState("KeyChange"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/KeyChangePromise.aspx")

    End Sub
End Class