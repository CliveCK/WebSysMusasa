Imports SysPermissionsManager.Functionality
Imports Telerik.Web.UI

Public Class ClientsListingCounsellor
    Inherits System.Web.UI.Page


    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim objCounsellorDetails As New BusinessLogic.InitialCounsellingSession(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim UseCriteria As Boolean

            If SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.AllowViewAdminDetails) Then

                UseCriteria = False

            Else

                UseCriteria = True

            End If

            With radClients

                .DataSource = objCounsellorDetails.GetBenDetails(UseCriteria, CookiesWrapper.StaffID)
                .DataBind()

                    ViewState("MCounsellor") = .DataSource

                End With

            End If

    End Sub

    Protected Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        Response.Redirect("~/ClientDetails.aspx?tag=couns")
    End Sub

    Protected Sub cmdReturningClients_Click(sender As Object, e As EventArgs) Handles cmdReturningClients.Click
        Response.Redirect("~/CounsellingReturningClientsListing.aspx")
    End Sub

    Protected Sub cmdInitSession_Click(sender As Object, e As EventArgs) Handles cmdInitSession.Click
        Response.Redirect("~/InitialCounsellingSession.aspx")
    End Sub

    Private Sub radClients_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radClients.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "Viewinitial" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClients.Items(index)

                Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

                Response.Redirect("~/InitialCounsellingSession?tag=couns&id=" & objUrlEncoder.Encrypt(BeneficiaryID))

            End If

            If e.CommandName = "ViewReturning" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClients.Items(index)

                Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

                Response.Redirect("~/CounsellingReturningClientDetails?tag=couns&id=" & objUrlEncoder.Encrypt(BeneficiaryID))

            End If

            If e.CommandName = "ViewSession" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClients.Items(index)

                Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

                Response.Redirect("~/ClientCounsellingSessionActivityDetails?tag=couns&id=" & objUrlEncoder.Encrypt(BeneficiaryID))

            End If

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radClients.Items(index)

                Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

                Response.Redirect("~/ClientDetails?tag=couns&id=" & objUrlEncoder.Encrypt(BeneficiaryID))

            End If

        End If

    End Sub

    Private Sub radClients_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radClients.NeedDataSource

        radClients.DataSource = DirectCast(ViewState("MCounsellor"), DataSet)

    End Sub
End Class