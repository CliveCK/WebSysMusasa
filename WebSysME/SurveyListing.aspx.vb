Imports Telerik.Web.UI

Public Class SurveyListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub


    Public Sub LoadGrid()

        ViewState("Survey") = Nothing

        Dim objSurvey As New BusinessLogic.Survey(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objSurvey.GetSurvey("SELECT * FROM tblSurveys")

        With radSurveyListing

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("Survey") = .DataSource

        End With

    End Sub

    Private Sub radSurveyListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radSurveyListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "ViewSurveyDetails" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radSurveyListing.Items(index)

                Dim SurveyID As Long = Server.HtmlDecode(item("SurveyID").Text)

                Response.Redirect("~/SurveysPage?id=" & objUrlEncoder.Encrypt(SurveyID))

            End If

        End If

    End Sub

    Private Sub radSurveyListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radSurveyListing.NeedDataSource

        radSurveyListing.DataSource = DirectCast(ViewState("Survey"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/SurveysPage.aspx")

    End Sub

End Class