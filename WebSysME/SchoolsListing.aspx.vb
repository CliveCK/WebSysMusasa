Imports Telerik.Web.UI

Public Class SchoolsListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objSchools As New BusinessLogic.Schools(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radSchoolListing

            .DataSource = objSchools.RetrieveAll().Tables(0)
            .DataBind()

            ViewState("SchoolsList") = .DataSource

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/SchoolManagement.aspx")

    End Sub

    Private Sub radSchoolListingg_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radSchoolListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radSchoolListing.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/SchoolManagement.aspx?id=" & objUrlEncoder.Encrypt(Server.HtmlDecode(item("SchoolID").Text)))

                Case "Delete"

                    'Dim objHealthCenter As New BusinessLogic.HealthCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    'With objHealthCenter

                    '    .HealthCenterID = Server.HtmlDecode(e.CommandArgument)

                    '    If .Delete() Then

                    '        ShowMessage("Health Center deleted successfully...", MessageTypeEnum.Information)

                    '    End If

                    'End With

            End Select

        End If

    End Sub

    Private Sub radSchoolListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radSchoolListing.NeedDataSource

        radSchoolListing.DataSource = DirectCast(ViewState("SchoolsList"), DataTable)

    End Sub
End Class