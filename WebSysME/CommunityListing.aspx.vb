Imports Telerik.Web.UI

Public Class CommunityListing
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region

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

            'LoadGrid()

        End If

    End Sub

    Private Sub LoadGrid(ByVal Criteria As String)

        Dim objCommunity As New BusinessLogic.Community(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radCommunityListing

            .DataSource = objCommunity.RetrieveAll(Criteria).Tables(0)
            .DataBind()

            ViewState("Community") = .DataSource

        End With

    End Sub

    Private Sub radCommunityListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radCommunityListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radCommunityListing.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/CommunityPage.aspx?id=" & objUrlEncoder.Encrypt(item("CommunityID").Text))

                Case "Delete"

                    Dim objCommunity As New BusinessLogic.Community(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objCommunity

                        .CommunityID = Server.HtmlDecode(e.CommandArgument)

                        If .Delete() Then

                            ShowMessage("Community deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radCommunityListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radCommunityListing.NeedDataSource

        radCommunityListing.DataSource = DirectCast(ViewState("Community"), DataTable)

    End Sub

    Private Sub cmNew_Click(sender As Object, e As EventArgs) Handles cmNew.Click

        Response.Redirect("~/CommunityPage.aspx")

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