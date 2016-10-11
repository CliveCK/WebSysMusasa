Imports Telerik.Web.UI

Public Class GranteeDetailListing
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

    Private Sub LoadGrid()

        Dim objGranteeDetail As New BusinessLogic.GranteeDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radGrantee

            .DataSource = objGranteeDetail.RetrieveAll().Tables(0)
            .DataBind()

            ViewState("radGrantee") = .DataSource

        End With

    End Sub

    Private Sub radGrantee_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radGrantee.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radGrantee.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/GranteeDetails.aspx?id=" & objUrlEncoder.Encrypt(Server.HtmlDecode(item("GranteeID").Text)))

            End Select

        End If

    End Sub

    Private Sub radGrantee_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radGrantee.NeedDataSource

        radGrantee.DataSource = DirectCast(ViewState("radGrantee"), DataTable)

    End Sub

End Class