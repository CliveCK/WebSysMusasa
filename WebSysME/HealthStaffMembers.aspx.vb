Imports BusinessLogic

Public Class HealthStaffMembers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Private Sub LoadGrid()

        Dim objStaff As New HealthCenterStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "select S.*, P.Name As Province, D.Name As District, H.Name As HealthCenter, SR.Description As StaffRole from tblHealthCenterStaff S inner join tblHealthCenters H on S.HealthCenterID = H.HealthCenterID "
        sql &= "left outer join luStaffPosition SR on SR.PositionID = S.StaffRoleID "
        sql &= "inner join tblWards W on W.WardID = H.WardID "
        sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
        sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID  "

        With radStaffListing

            .DataSource = objStaff.GetHealthCenterStaff(sql)
            .DataBind()

            Session("HHStaff") = .DataSource

        End With

    End Sub

    Private Sub radStaffListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radStaffListing.NeedDataSource

        radStaffListing.DataSource = Session("HHStaff")

    End Sub

End Class