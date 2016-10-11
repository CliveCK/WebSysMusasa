Imports SysPermissionsManager.Functionality
Imports Telerik.Web.UI

Public Class ShelterClientsList
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objShelterDetails As New BusinessLogic.ShelterClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim UseCriteria As Boolean
            Dim SubOffices As String = "0"

            If SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.AllowViewAdminDetails) Then

                UseCriteria = False

            Else

                UseCriteria = True

            End If

            Dim objSubOffices As New BusinessLogic.SubOffices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim ds As DataSet = objSubOffices.GetSubOfficesByOrganization(CookiesWrapper.OrganizationID)
            Dim myList As New List(Of String)

            If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                For Each row In ds.Tables(0).Rows

                    myList.Add(row("SubOfficeID"))

                Next

            End If

            SubOffices = IIf(myList.Count > 0, String.Join(",", myList.ToArray), "0")

            With radClients

                .DataSource = objShelterDetails.GetBenDetails(UseCriteria, SubOffices)

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