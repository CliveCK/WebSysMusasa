Imports Telerik.Web.UI

Public Class ProcumentList
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Public Sub LoadGrid()

        Dim objSurvey As New BusinessLogic.Survey(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objSurvey.GetSurvey("SELECT * FROM tblProcument")

        With radProcumentListing

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("Procument") = .DataSource

        End With

    End Sub

    Private Sub radProcumentListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radProcumentListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radProcumentListing.Items(index)

                Dim ProcumentID As Long = Server.HtmlDecode(item("ProcumentID").Text)

                Response.Redirect("~/ProcumentPage?Procid=" & objUrlEncoder.Encrypt(ProcumentID))

            End If

        End If

    End Sub

    Private Sub radProcumentListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radProcumentListing.NeedDataSource

        radProcumentListing.DataSource = DirectCast(ViewState("Procument"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/ProcumentPage.aspx")

    End Sub

End Class