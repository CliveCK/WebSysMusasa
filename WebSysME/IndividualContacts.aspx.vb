Public Class IndividualContacts
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objStaff As New BusinessLogic.StaffMember(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radContacts

            .DataSource = objStaff.GetAllStaffMember().Tables(0)
            .DataBind()

            ViewState("IndiContacts") = .DataSource

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/StaffMembers.aspx")

    End Sub

    Private Sub radContacts_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radContacts.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As Telerik.Web.UI.GridDataItem = radContacts.Items(index1)
            Dim StaffID As Integer

            StaffID = Server.HtmlDecode(item1("StaffID").Text)

            Response.Redirect("~/StaffMembers.aspx?id=" & objUrlEncoder.Encrypt(StaffID))

        End If

    End Sub

    Private Sub radContacts_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radContacts.NeedDataSource

        radContacts.DataSource = DirectCast(ViewState("IndiContacts"), DataTable)

    End Sub

End Class