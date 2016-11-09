Imports SysPermissionsManager.Functionality
Imports Telerik.Web.UI

Public Class ShelterClientsList
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objShelterDetails As New BusinessLogic.ShelterClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim UseCriteria As Boolean

            If SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.AllowViewAdminDetails) Then

                UseCriteria = False

            Else

                UseCriteria = True

            End If

            Dim objSubOffices As New BusinessLogic.SubOffices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With radClients

                .DataSource = objShelterDetails.GetBenDetails(UseCriteria, CookiesWrapper.StaffID)

                ViewState("MShelter") = .DataSource

                End With

            End If

    End Sub

    Protected Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        Response.Redirect("~/ShelterClientDetails.aspx")
    End Sub

    Private Sub radClients_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radClients.NeedDataSource

        radClients.DataSource = DirectCast(ViewState("MShelter"), DataSet)

    End Sub

    Private Sub radClients_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radClients.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClients.Items(index)

                Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

                Response.Redirect("~/ShelterClientDetails?id=" & objUrlEncoder.Encrypt(BeneficiaryID))

            End If

            If e.CommandName = "Activity" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClients.Items(index)

                Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

                Response.Redirect("~/ClientCounsellingSessionActivityDetails?id=" & objUrlEncoder.Encrypt(BeneficiaryID))

            End If

        End If

    End Sub
End Class