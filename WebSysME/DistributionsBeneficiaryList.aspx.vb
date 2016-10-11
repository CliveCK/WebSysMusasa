Public Class DistributionsBeneficiaryList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        With radTraining

            .DataSource = String.Empty
            .DataBind()

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/DistributionBeneficiaryPage.aspx")

    End Sub
End Class