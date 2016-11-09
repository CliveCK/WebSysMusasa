Imports Telerik.Web.UI

Public Class ClientCounsellingSessionActivityListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim objCounsellorSessionActivity As New BusinessLogic.CounsellingSessionActivities(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            If Not IsNothing(Request.QueryString("id")) Then

                With radClientsActivity

                    .DataSource = objCounsellorSessionActivity.GetCounsellingSessionActivitiesByBeneficiaryID(objUrlEncoder.Decrypt(Request.QueryString("id")))
                    .DataBind()

                    ViewState("radSessionActivity") = .DataSource

                End With

            End If

        End If

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/ClientCounsellingSessionActivityDetails.aspx?benid=" & Request.QueryString("id"))

    End Sub

    Private Sub radClientsActivity_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radClientsActivity.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClientsActivity.Items(index)

                Dim ClientSessionActivityID As Long = Server.HtmlDecode(item("ClientSessionActivityID").Text)

                Response.Redirect("~/ClientCounsellingSessionActivityDetails?id=" & objUrlEncoder.Encrypt(ClientSessionActivityID) & "&benid=" & Request.QueryString("id"))

            End If

        End If

    End Sub

    Private Sub radClientsActivity_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radClientsActivity.NeedDataSource

        radClientsActivity.DataSource = DirectCast(ViewState("radSessionActivity"), DataSet)

    End Sub
End Class