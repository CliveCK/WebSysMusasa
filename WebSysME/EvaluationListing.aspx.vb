Imports Telerik.Web.UI

Public Class EvaluationListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Public Sub LoadGrid()

        ViewState("Evaluation") = Nothing

        Dim objEvaluation As New BusinessLogic.Evaluation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objEvaluation.GetEvaluation("SELECT * FROM tblEvaluations")

        With radEvaluationListing

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("Evaluation") = .DataSource

        End With

    End Sub

    Private Sub radEvaluationListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radEvaluationListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "ViewEvaluationDetails" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radEvaluationListing.Items(index)

                Dim EvaluationID As Long = Server.HtmlDecode(item("EvaluationID").Text)

                Response.Redirect("~/EvaluationsPage?id=" & objUrlEncoder.Encrypt(EvaluationID))

            End If

        End If

    End Sub

    Private Sub radEvaluationListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radEvaluationListing.NeedDataSource

        radEvaluationListing.DataSource = DirectCast(ViewState("Evaluation"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/EvaluationsPage.aspx")

    End Sub
End Class