Imports Telerik.Web.UI

Public Class MainOneStopCentreListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objOneStop As New BusinessLogic.MainOneStopCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radCBS

            .DataSource = objOneStop.GetAllMainOneStop
            .DataBind()

            ViewState("MainOneS") = .DataSource

        End With

    End Sub

    Protected Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        Response.Redirect("MainOneStopCentreDetails.aspx")
    End Sub

    Private Sub radCBS_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radCBS.NeedDataSource

        radCBS.DataSource = DirectCast(ViewState("MainOneS"), DataSet)

    End Sub

    Private Sub radCBS_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radCBS.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radCBS.Items(index)

            Dim MainOneStopCenterID As Long = Server.HtmlDecode(item("MainOneStopCenterID").Text)

            Response.Redirect("~/MainOneStopCentreDetails?id=" & objUrlEncoder.Encrypt(MainOneStopCenterID))

        End If

    End Sub
End Class