Imports Telerik.Web.UI

Public Class MicrofinanceClientsListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With radClients

                .DataSource = objBeneficiary.GetBeneficiary("Select *, D.Name as District, W.Name as Ward FROM tblBeneficiaries B 
                                    inner join tblAddresses A on A.OwnerID = B.BeneficiaryID 
                                    inner join tblDistricts D on D.DistrictID = A.DistrictID
                                    inner join tblWards W on W.WardID = A.WardID 
                                    where BeneficiaryID in (Select BeneficiaryID from tblMicrofinanceClientDetails)")
                .DataBind()

                ViewState("mMicrofinanceClientDetails") = .DataSource

            End With

        End If

    End Sub

    Protected Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        Response.Redirect("~/MicrofinanceClientDetails.aspx")
    End Sub

    Private Sub radClients_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radClients.NeedDataSource

        radClients.DataSource = DirectCast(ViewState("mMicrofinanceClientDetails"), DataSet)

    End Sub

    Private Sub radClients_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radClients.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radClients.Items(index)

            Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

            Response.Redirect("~/MicrofinanceClientDetails?id=" & objUrlEncoder.Encrypt(BeneficiaryID))

        End If

    End Sub
End Class