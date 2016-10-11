Imports Telerik.Web.UI

Public Class IndicatorListingControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Public Sub LoadGrid()

        ViewState("Indicator") = Nothing

        Dim objIndicator As New BusinessLogic.Indiactor(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objIndicator.GetAllIndicators()

        With radIndicatorListing

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("Indicator") = .DataSource

        End With

    End Sub

    Private Sub radIndicatorListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radIndicatorListing.ItemCommand

        If e.CommandName = "ViewIndicatorDetails" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radIndicatorListing.Items(index)

            Dim IndicatorID As Long = Server.HtmlDecode(item("IndicatorID").Text)

            Response.Redirect("~/Indicators?Inid=" & objUrlEncoder.Encrypt(IndicatorID))

        End If

    End Sub

    Private Sub radIndicatorListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radIndicatorListing.NeedDataSource

        radIndicatorListing.DataSource = DirectCast(ViewState("Indicator"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/Indicators.aspx")

    End Sub
End Class