Imports Telerik.Web.UI

Public Class OrganizationalBeneficiaries
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objOrganization As New BusinessLogic.Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radOrgListing

            .DataSource = objOrganization.RetrieveAll().Tables(0)
            .DataBind()

            ViewState("OrgDetails") = .DataSource

        End With

    End Sub

    Private Sub radOrgListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radOrgListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radOrgListing.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/OrganizationDetails.aspx?id=" & objUrlEncoder.Encrypt(item("OrganizationID").Text))

            End Select

        End If

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/OrganizationDetails.aspx")

    End Sub

    Private Sub radOrgListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radOrgListing.NeedDataSource

        radOrgListing.DataSource = DirectCast(ViewState("OrgDetails"), DataTable)

    End Sub
End Class