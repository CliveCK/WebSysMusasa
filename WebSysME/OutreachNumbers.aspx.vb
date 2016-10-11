Imports Telerik.Web.UI

Public Class OutreachNumbers
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Public Sub LoadGrid()

        Dim sql As String = "select OutreachID, OG.Name As [Partner], P.Name as Project, D.Name As District, W.Name as Ward, Total, BT.Description as BeneficiaryType from tblOutreach O "
        sql &= " inner join tblOrganization OG on O.[OrganizationID] = OG.OrganizationID"
        sql &= " inner join tblProjects P on P.Project = O.ProjectID"
        sql &= " left outer join luBeneficiaryType BT on BT.BeneficiaryTypeID = O.BeneficiaryTypeID "
        sql &= " left outer join tblDistricts D on D.DistrictID = O.DistrictID"
        sql &= " left outer join tblWards W on W.WardID = O.WardID"

        Dim objReach As New BusinessLogic.Outreach(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objReach.GetOutreach(sql)

        With radOutreach

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("Outreach") = .DataSource

        End With

    End Sub

    Private Sub radOutreach_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radOutreach.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radOutreach.Items(index)

                Dim OutreachID As Long = Server.HtmlDecode(item("OutreachID").Text)

                Response.Redirect("~/OutreachDetails?id=" & objUrlEncoder.Encrypt(OutreachID))

            End If

        End If

    End Sub

    Private Sub radOutreach_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radOutreach.NeedDataSource

        radOutreach.DataSource = DirectCast(ViewState("Outreach"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/OutreachDetails.aspx")

    End Sub

End Class