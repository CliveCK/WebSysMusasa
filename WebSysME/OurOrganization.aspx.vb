Public Class OurOrganization
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not SystemInitialization.EnforceUserFunctionalitySecurity(SysPermissionsManager.Functionality.FunctionalityEnum.ViewOrganizationDetails) Then

            Response.Redirect("~/RestrictedAccess.aspx?msg=DenyAccess")

        Else

            If Not IsPostBack Then
                If Not IsNothing(Request.QueryString("id")) Then

                    LoadGrid(objUrlEncoder.Decrypt(Request.QueryString("id")))

                Else

                    LoadGrid(IIf(IsNumeric(DirectCast(ucOrganizationControl.FindControl("txtOrganizationID"), TextBox).Text), DirectCast(ucOrganizationControl.FindControl("txtOrganizationID"), TextBox).Text, 0))

                End If

            End If

        End If

    End Sub

    Public Sub LoadGrid(ByVal OrganizationID As Long)

        Dim objSubOffices As New BusinessLogic.SubOffices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radAddress

            .DataSource = objSubOffices.GetOrganizationSubOffices(OrganizationID).Tables(0)
            .DataBind()

            ViewState("Off") = .DataSource

        End With

        Dim objStaff As New BusinessLogic.StaffMember(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radStaff

            .DataSource = objStaff.GetStaffMember("SELECT * FROM tblStaffMembers WHERE OrganizationID = " & OrganizationID).Tables(0)
            .DataBind()

            ViewState("myStaff") = .DataSource

        End With

    End Sub

    Private Sub radStaff_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radStaff.NeedDataSource

        radStaff.DataSource = DirectCast(ViewState("myStaff"), DataTable)

    End Sub

    Private Sub radAddress_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radAddress.NeedDataSource

        radAddress.DataSource = DirectCast(ViewState("Off"), DataTable)

    End Sub
End Class