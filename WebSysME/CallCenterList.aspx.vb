Imports Telerik.Web.UI

Public Class CallCenterList
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objCallCenter As New BusinessLogic.CallCenterDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radCallCenter

            .DataSource = objCallCenter.GetAllCallCenterDetails
            .DataBind()

            ViewState("mCallCenter") = .DataSource

        End With

    End Sub

    Protected Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        Response.Redirect("~/CallCenterDetails.aspx")
    End Sub

    Private Sub radCallCenter_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radCallCenter.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radCallCenter.Items(index)

                Dim CallCenterDetailID As Long = Server.HtmlDecode(item("CallCenterDetailID").Text)

                Response.Redirect("~/CallCenterDetails?id=" & objUrlEncoder.Encrypt(CallCenterDetailID))

            End If

        End If

    End Sub

    Private Sub radCallCenter_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radCallCenter.NeedDataSource

        radCallCenter.DataSource = DirectCast(ViewState("mCallCenter"), DataSet)

    End Sub
End Class