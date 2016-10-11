Public Class BeneficiaryListing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name").Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboProject

                .DataSource = objLookup.Lookup("tblProjects", "Project", "Name").Tables(0)
                .DataValueField = "Project"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub LoadGrid()

        Dim objBeneficiaries As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radBenListing

            .DataSource = objBeneficiaries.GetAllBeneficiaries("").Tables(0)
            .DataBind()

            ViewState("Beneficiaries") = .DataSource

        End With

    End Sub

    Private Sub radBenListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radBenListing.NeedDataSource

        radBenListing.DataSource = DirectCast(ViewState("Beneficiaries"), DataTable)

    End Sub

    Private Function BuildCriteria() As String

        Dim Criteria As String = ""

        Criteria &= IIf(cboDistrict.SelectedIndex > 0, IIf(Criteria <> "", " AND DistrictID = " & cboDistrict.SelectedValue, " WHERE DistrictID = " & cboDistrict.SelectedValue), "")
        Criteria &= IIf(cboWard.SelectedIndex > 0, IIf(Criteria <> "", " AND WardID = " & cboWard.SelectedValue, " WHERE WardID = " & cboWard.SelectedValue), "")
        Criteria &= IIf(cboProject.SelectedIndex > 0, IIf(Criteria <> "", " AND ProjectID = " & cboProject.SelectedValue, " WHERE ProjectID = " & cboProject.SelectedValue), "")

        Return Criteria

    End Function

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click

        Dim Criteria As String = BuildCriteria()

        If Criteria <> "" Then

            With radBenListing

                .DataSource = Nothing
                .DataBind()

            End With

        End If

    End Sub

    Private Sub cboDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDistrict.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboDistrict

            .DataSource = objLookup.Lookup("tblWards", "WardID", "Name", , "DistrictID = " & cboDistrict.SelectedValue).Tables(0)
            .DataValueField = "WardID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub
End Class