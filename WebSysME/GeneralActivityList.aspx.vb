Imports Telerik.Web.UI

Public Class GeneralActivityList
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub


    Private Sub LoadGrid()

        Dim objActivities As New BusinessLogic.GeneralActivity(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "select A.*, AT.Description As Activity from Appointments A inner join tblActivities AT on A.ActivityID = AT.ActivityID "

        With radActivities

            .DataSource = objActivities.GetGeneralActivity(sql).Tables(0)
            .DataBind()

            ViewState("qActivities") = .DataSource

        End With


    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/GeneralActivityPage.aspx")

    End Sub

    Private Sub radActivities_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radActivities.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radActivities.Items(index1)
            Dim GeneralActivityID As Integer

            GeneralActivityID = Server.HtmlDecode(item1("GeneralActivityID").Text)

            Response.Redirect("~/GeneralActivityPage?id=" & objUrlEncoder.Encrypt(GeneralActivityID))

        End If

    End Sub

    Private Sub radActivities_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radActivities.NeedDataSource

        radActivities.DataSource = DirectCast(ViewState("qActivities"), DataTable)

    End Sub
End Class