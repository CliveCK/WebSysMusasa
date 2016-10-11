Imports Telerik.Web.UI

Public Class HouseholdList
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name")
                .DataTextField = "Name"
                .DataValueField = "DistrictID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0


            End With

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name")
                .DataTextField = "Name"
                .DataValueField = "WardID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0


            End With

        End If

    End Sub
    Private Sub LoadGrid(ByVal Criteria As String)

        Dim objBeneficiaries As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radBenListing

            .DataSource = objBeneficiaries.GetAllBeneficiaries(Criteria).Tables(0)
            .DataBind()

            ViewState("Beneficiaries") = .DataSource

        End With

    End Sub

    Private Sub radBenListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radBenListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radBenListing.Items(index)
            Dim BeneficiaryID As Integer

            BeneficiaryID = Server.HtmlDecode(item("BeneficiaryID").Text)

            CookiesWrapper.BeneficiaryID = BeneficiaryID

            Response.Redirect("~/Beneficiary.aspx?id=" & objUrlEncoder.Encrypt(BeneficiaryID))

        End If

    End Sub

    Private Sub radBenListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radBenListing.NeedDataSource

        radBenListing.DataSource = DirectCast(ViewState("Beneficiaries"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        CookiesWrapper.BeneficiaryID = 0
        Response.Redirect("~/Beneficiary.aspx")

    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click

        Dim Criteria As String = ""

        If cboDistrict.SelectedValue > 0 Then

            Criteria &= IIf(Criteria <> "", " AND D.DistrictID = " & cboDistrict.SelectedValue, " WHERE D.DistrictID = " & cboDistrict.SelectedValue)

        End If

        If cboWard.SelectedValue > 0 Then

            Criteria &= IIf(Criteria <> "", " And W.WardID = " & cboWard.SelectedValue, " WHERE W.WardID = " & cboWard.SelectedValue)

        End If
        LoadGrid(Criteria)

    End Sub
End Class