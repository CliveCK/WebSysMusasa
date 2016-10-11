Imports Telerik.Web.UI

Public Class GroupAssociationListing
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

            LoadGrid()

        End If

    End Sub

    Private Sub cmNew_Click(sender As Object, e As EventArgs) Handles cmNew.Click

        Response.Redirect("~/GroupAssociationsPage.aspx")

    End Sub

    Private Sub LoadGrid()

        Dim objGroupAssociation As New BusinessLogic.GroupAssociations(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radGroupAssociationListing

            .DataSource = objGroupAssociation.GetGroupAssociations("SELECT * FROM tblGroupAssociations").Tables(0)
            .DataBind()

            ViewState("GroupAssociations") = .DataSource

        End With

    End Sub

    Private Sub radGroupAssociationListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radGroupAssociationListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radGroupAssociationListing.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/GroupAssociationsPage.aspx?id=" & objUrlEncoder.Encrypt(item("GroupAssociationID").Text))

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

    Private Sub radGroupAssociationListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radGroupAssociationListing.NeedDataSource

        radGroupAssociationListing.DataSource = DirectCast(ViewState("GroupAssociations"), DataTable)

    End Sub
End Class