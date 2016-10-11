Imports Telerik.Web.UI

Public Class ReceptionClientsView
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim objClients As New BusinessLogic.ClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With radClients

                .DataSource = objClients.GetallDetails
                .DataBind()

                ViewState("vClients") = .DataSource

            End With

        End If
    End Sub

    Protected Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        Response.Redirect("~/ClientDetails.aspx?tag=recp")
    End Sub

    Private Sub radClients_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radClients.NeedDataSource

        radClients.DataSource = DirectCast(ViewState("vClients"), DataSet)

    End Sub

    Private Sub radClients_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radClients.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClients.Items(index)

                Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

                Response.Redirect("~/ClientDetails?tag=recp&id=" & objUrlEncoder.Encrypt(BeneficiaryID))

            End If

        End If

    End Sub
End Class