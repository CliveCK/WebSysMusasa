Imports Telerik.Web.UI

Public Class GroupsListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objGroups As New BusinessLogic.Groups(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radGroupListing

            .DataSource = objGroups.RetrieveAll().Tables(0)
            .DataBind()

            ViewState("GroupListing") = .DataSource

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/GroupsDetails.aspx")

    End Sub

    Private Sub radGroupListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radGroupListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radGroupListing.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/GroupsDetails.aspx?id=" & objUrlEncoder.Encrypt(Server.HtmlDecode(item("GroupID").Text)))

            End Select

        End If

    End Sub

    Private Sub radGroupListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radGroupListing.NeedDataSource

        radGroupListing.DataSource = DirectCast(ViewState("GroupListing"), DataTable)

    End Sub
End Class