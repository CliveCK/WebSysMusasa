Imports SysPermissionsManager.Functionality
Imports Telerik.Web.UI

Public Class LawyerClientsList
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim objLawyerDetails As New BusinessLogic.LawyerClientSessionDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim UseCriteria As Boolean

            If SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.AllowViewAdminDetails) Then

                UseCriteria = False

            Else

                UseCriteria = True

            End If

            With radClients

                .DataSource = objLawyerDetails.GetBenDetails(UseCriteria, CookiesWrapper.StaffID)
                .DataBind()

                ViewState("Mlawyer") = .DataSource

            End With

        End If

    End Sub

    Protected Sub cmdNew0_Click(sender As Object, e As EventArgs) Handles cmdNew0.Click
        Response.Redirect("~/LawyerClientSessionDetails.aspx")
    End Sub

    Private Sub radClients_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radClients.NeedDataSource

        radClients.DataSource = DirectCast(ViewState("Mlawyer"), DataSet)

    End Sub

    Private Sub radClients_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radClients.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClients.Items(index)

                Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

                Response.Redirect("~/LawyerClientSessionDetails?id=" & objUrlEncoder.Encrypt(BeneficiaryID))

            End If

        End If

    End Sub
End Class