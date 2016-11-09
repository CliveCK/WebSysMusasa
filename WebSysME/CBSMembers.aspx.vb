Imports Telerik.Web.UI
Imports Universal.CommonFunctions

Public Class CBSMembers
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Request.QueryString("id")) Then

            Dim objCBSMembers As New BusinessLogic.CBSMembers(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objCBSMemberReporting As New BusinessLogic.CBSMemberReporting(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With radCBS

                .DataSource = objCBSMembers.GetAllCBSMembersByCBSMemberReportingID(objUrlEncoder.Decrypt(Request.QueryString("id")))
                .DataBind()

                ViewState("CBSMem") = .DataSource

            End With

            With objCBSMemberReporting

                Dim ds As DataSet = .GetOneCBSMemberReporting(objUrlEncoder.Decrypt(Request.QueryString("id")))

                If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    txtDistrict.Text = Catchnull(ds.Tables(0).Rows(0)("District"), "")
                    txtWard.Text = Catchnull(ds.Tables(0).Rows(0)("Ward1"), "")
                    txtMonth.Text = Catchnull(ds.Tables(0).Rows(0)("Month"), "")
                    txtClub.Text = CatchNull(ds.Tables(0).Rows(0)("Club"), "")
                    txtYear.Text = Catchnull(ds.Tables(0).Rows(0)("Year"), "")

                End If

            End With

        End If

    End Sub

    Private Sub LoadGrid()

        Dim obj

    End Sub

    Protected Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        Response.Redirect("~/CBSMemberDetails.aspx?cbsid=" & Request.QueryString("id"))
    End Sub


    Protected Sub lnkCBSReporting_Click(sender As Object, e As EventArgs) Handles lnkCBSReporting.Click
        Response.Redirect("~/CBSMemberReporting.aspx")

    End Sub

    Private Sub radCBS_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radCBS.NeedDataSource

        radCBS.DataSource = DirectCast(ViewState("CBSMem"), DataSet)

    End Sub

    Private Sub radCBS_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radCBS.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radCBS.Items(index)

            Dim BeneficiaryID As Long = Server.HtmlDecode(item("BeneficiaryID").Text)

            Response.Redirect("~/CBSMemberDetails?id=" & objUrlEncoder.Encrypt(BeneficiaryID) & "&cbsid=" & Request.QueryString("id"))

        End If

    End Sub

    Private Sub radCBS_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs) Handles radCBS.DetailTableDataBind

        Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.DetailTableView.ParentItem, Telerik.Web.UI.GridDataItem)
        Dim objProblems As New BusinessLogic.CBSMemberNeeds(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If e.DetailTableView.Name = "dsProblems" Then

            e.DetailTableView.DataSource = objProblems.GetCBSMemberNeedsByBeneficiaryID(dataItem.GetDataKeyValue("BeneficiaryID").ToString())

        End If


    End Sub
End Class