Imports Telerik.Web.UI

Public Class KeyChangePromiseListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Public Sub LoadGrid()

        ViewState("KeyChangeList") = Nothing

        Dim objSurvey As New BusinessLogic.Survey(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objSurvey.GetSurvey("SELECT P.KeyChangePromiseID, P.KeyChangePromiseNo, O.* FROM tblKeyChangePromises P INNER JOIN tblStrategicObjectives O on P.StrategicObjectiveID = O.StrategicObjectiveID")

        With radKeyChangeListing

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("KeyChangeList") = .DataSource

        End With

    End Sub

    Private Sub radKeyChangeListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radKeyChangeListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "ViewKeyChangeDetails" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radKeyChangeListing.Items(index)

                Dim KeyChangePromiseID As Long = Server.HtmlDecode(item("KeyChangePromiseID").Text)

                Response.Redirect("~/KeyChangePromise.aspx?id=" & objUrlEncoder.Encrypt(KeyChangePromiseID))

            End If

        End If

    End Sub

    Private Sub radKeyChangeListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radKeyChangeListing.NeedDataSource

        radKeyChangeListing.DataSource = DirectCast(ViewState("KeyChangeList"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/KeyChangePromise.aspx")

    End Sub

End Class