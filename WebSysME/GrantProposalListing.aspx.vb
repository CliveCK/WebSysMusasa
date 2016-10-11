Imports Telerik.Web.UI

Public Class GrantProposalListing
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

        Dim objGrantProposal As New BusinessLogic.GrantProposal(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radGrantProposal

            .DataSource = objGrantProposal.RetrieveAll().Tables(0)
            .DataBind()

            ViewState("radGrantProposal") = .DataSource

        End With

    End Sub

    Private Sub radGrantProposal_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radGrantProposal.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radGrantProposal.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/GrantProposalDetails.aspx?id=" & objUrlEncoder.Encrypt(Server.HtmlDecode(item("GrantProposalID").Text)))

            End Select

        End If

    End Sub

    Private Sub radGrantProposal_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radGrantProposal.NeedDataSource

        radGrantProposal.DataSource = DirectCast(ViewState("radGrantProposal"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/GrantProposalDetails.aspx")

    End Sub

End Class