Public Class DepartmentalPlanListing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objDepPlan As New BusinessLogic.DepartmentalPlan(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radDepPlanListing

            .DataSource = objDepPlan.RetrieveAll().Tables(0)
            .DataBind()

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/DepartmentalPlanPage.aspx")

    End Sub
End Class