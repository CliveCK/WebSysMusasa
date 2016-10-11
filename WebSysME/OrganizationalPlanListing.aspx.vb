Public Class OrganizationalPlanListing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objOrgPlan As New BusinessLogic.OrganizationalPlan(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radOrgPlanListing

            .DataSource = objOrgPlan.RetrieveAll().Tables(0)
            .DataBind()

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/OrganizationalPlanPage.aspx")

    End Sub
End Class