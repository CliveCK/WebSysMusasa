Public Class OrganizationalContacts
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objOrganizationContacts As New BusinessLogic.Organization(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radContacts

            .DataSource = objOrganizationContacts.RetrieveAll().Tables(0)
            .DataBind()

            ViewState("OrgContacts") = .DataSource

        End With


    End Sub

    Private Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click

        Response.Redirect("~/OrganizationDetails.aspx")

    End Sub

    Private Sub radContacts_DetailTableDataBind(sender As Object, e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles radContacts.DetailTableDataBind

        Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.DetailTableView.ParentItem, Telerik.Web.UI.GridDataItem)
        Dim objSubOffices As New BusinessLogic.SubOffices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If e.DetailTableView.Name = "dsContacts" Then

            e.DetailTableView.DataSource = objSubOffices.GetSubOfficesByOrganization(dataItem.GetDataKeyValue("OrganizationID").ToString())

        End If

    End Sub

    Private Sub radContacts_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radContacts.ItemCommand

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            If e.CommandName = "View" Then

                Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item1 As Telerik.Web.UI.GridDataItem = radContacts.MasterTableView.Items(index1)
                Dim OrganizationID As Integer

                OrganizationID = item1("OrganizationID").Text

                Response.Redirect("~/OurOrganization?id=" & objUrlEncoder.Encrypt(OrganizationID))

            End If

        End If

    End Sub

    Private Sub radContacts_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radContacts.NeedDataSource

        If Not e.IsFromDetailTable Then

            radContacts.DataSource = DirectCast(ViewState("OrgContacts"), DataTable)

        End If

    End Sub
End Class