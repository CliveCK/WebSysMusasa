Public Class CounsellingReturningClientsListing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With radClients

            .DataSource = String.Empty
            .DataBind()

        End With
    End Sub

    Protected Sub cmdNew1_Click(sender As Object, e As EventArgs) Handles cmdNew1.Click
        Response.Redirect("~/CounsellingReturningClientDetails.aspx")
    End Sub

 
End Class