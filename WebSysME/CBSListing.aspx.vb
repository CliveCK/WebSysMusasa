Imports Telerik.Web.UI

Public Class CBSListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objMembers As New BusinessLogic.CBSMemberReporting(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radCBS

            .DataSource = objMembers.GetAllMemberReporting
            .DataBind()

            ViewState("CBSMemberR") = .DataSource

        End With

    End Sub

    Protected Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        Response.Redirect("~/CBSMemberReporting.aspx")
    End Sub

    Protected Sub cmdUpload_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
        Response.Redirect("~/IndividualInforUpload.aspx")
    End Sub

    Private Sub radCBS_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radCBS.NeedDataSource

        radCBS.DataSource = DirectCast(ViewState("CBSMemberR"), DataSet)

    End Sub

    Private Sub radCBS_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radCBS.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radCBS.Items(index)

            Dim CBSMemberReportingID As Long = Server.HtmlDecode(item("CBSMemberReportingID").Text)

            Response.Redirect("~/CBSMemberReporting?id=" & objUrlEncoder.Encrypt(CBSMemberReportingID))

        End If

    End Sub
End Class