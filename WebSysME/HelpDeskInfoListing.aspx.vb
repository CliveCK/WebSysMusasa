Imports Telerik.Web.UI

Public Class HelpDeskInfoListing
    Inherits System.Web.UI.Page


    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objHelpDesk As New BusinessLogic.ClientDesk(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radHelpDesk

            .DataSource = objHelpDesk.GetClientDeskAll().Tables(0)
            .DataBind()

            ViewState("HelpDesk") = .DataSource

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/HelpDesk.aspx")

    End Sub

    Private Sub radHelpDesk_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radHelpDesk.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radHelpDesk.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/HelpDesk.aspx?id=" & objUrlEncoder.Encrypt(Server.HtmlDecode(item("ClientDeskInforID").Text)))

            End Select

        End If

    End Sub

    Private Sub radGroupListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radHelpDesk.NeedDataSource

        radHelpDesk.DataSource = DirectCast(ViewState("HelpDesk"), DataTable)

    End Sub

End Class