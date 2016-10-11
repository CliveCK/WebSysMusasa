Imports Telerik.Web.UI

Public Class PatientList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Private Sub LoadGrid()

        Dim objPatients As New BusinessLogic.Patients(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radPatientListing

            .DataSource = objPatients.GetAllPatients().Tables(0)
            .DataBind()

            ViewState("Patients") = .DataSource

        End With

    End Sub

    Private Sub radPatientListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radPatientListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As Telerik.Web.UI.GridDataItem = radPatientListing.Items(index)
            Dim PatientID As Integer

            PatientID = Server.HtmlDecode(item("PatientID").Text)

            CookiesWrapper.PatientID = PatientID

            Response.Redirect("~/PatientDetailsPage")

        End If

    End Sub

    Private Sub radPatientListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radPatientListing.NeedDataSource

        radPatientListing.DataSource = DirectCast(ViewState("Patients"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        CookiesWrapper.PatientID = 0
        Response.Redirect("~/PatientDetailsPage")

    End Sub
End Class