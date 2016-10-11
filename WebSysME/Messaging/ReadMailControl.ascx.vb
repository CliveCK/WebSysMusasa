Imports PrimaSysMessaging.BusinessLogic

Public Class ReadMailControl
    Inherits System.Web.UI.UserControl

    Private NewMsgCount As Integer = 0
    Private DeletedMsgCount As Integer = 0
    Private OutboxMsgCount As Integer = 0

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageTypeEnum)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)


        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageTypeEnum)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Link.Attributes("href") = "javascript:void(0);"
            cmdReply.Attributes("href") = "javascript:void(0);"
            cmdForward.Attributes("href") = "javascript:void(0);"
            Session("Message") = Nothing

            If Not IsNothing(Request.QueryString("status")) Then

                If Request.QueryString("status") = "true" Then ShowMessage("Your message was sent", MessageTypeEnum.Information)

            End If

        End If

    End Sub

    Private Sub ConfigurePanel()

        Dim objMessageDb As New MessageHandler

        With objMessageDb

            NewMsgCount = .GetMessageCount(CookiesWrapper.thisUserID, TypeEnum.Type.New)
            DeletedMsgCount = .GetMessageCount(CookiesWrapper.thisUserID, TypeEnum.Type.Deleted)
            OutboxMsgCount = .GetMessageCount(CookiesWrapper.thisUserID, TypeEnum.Type.Outbox)

        End With

    End Sub

    Private Sub radpMail_ItemClick(sender As Object, e As Telerik.Web.UI.RadPanelBarEventArgs) Handles radpMail.ItemClick

        Select Case radpMail.SelectedItem.Value

            Case "cmdNew"

            Case "cmdInbox"
                lblSelected.Text = "Inbox"
                LoadGrid("Inbox")

            Case "cmdSent"
                lblSelected.Text = "Sent Items"
                LoadGrid("Sent")

            Case "cmdDeletedItem"
                lblSelected.Text = "Deleted Messages"
                LoadGrid("Deleted")

        End Select

    End Sub

    Private Sub radpMail_ItemCreated(sender As Object, e As Telerik.Web.UI.RadPanelBarEventArgs) Handles radpMail.ItemCreated

        ConfigurePanel()

        With radpMail

            .FindItemByValue("cmdInbox").Text = "Inbox " & IIf(NewMsgCount > 0, "[" & NewMsgCount & "]", "")
            .FindItemByValue("cmdDeletedItem").Text = "Deleted Items " & IIf(DeletedMsgCount > 0, "[" & DeletedMsgCount & "]", "")

        End With

    End Sub

    Private Sub LoadGrid(ByVal LoadTarget As String)

        Dim objMessageDb As New MessageHandler
        Dim dt As DataTable = Nothing

        Select Case LoadTarget

            Case "Inbox"
                dt = objMessageDb.GetAllMessages(CookiesWrapper.thisUserID)

            Case "Sent"
                dt = objMessageDb.GetSentMessages(CookiesWrapper.thisUserID)

            Case "Deleted"
                dt = objMessageDb.GetDeletedMessages(CookiesWrapper.thisUserID)

        End Select

        If Not IsNothing(radpMail.Items) Then

            With radMail

                .DataSource = dt
                .DataBind()

                ViewState("Mail") = .DataSource

            End With

        End If

        pnlEditor.Visible = False

    End Sub

    Private Sub radMail_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radMail.NeedDataSource

        radMail.DataSource = DirectCast(ViewState("Mail"), DataTable)

    End Sub

    Private Sub radMail_PreRender(sender As Object, e As EventArgs) Handles radMail.PreRender

        If Not IsNothing(radMail.DataSource) Then

            With radMail.MasterTableView

                .GetColumn("messageid").Display = False
                .GetColumn("senderid").Display = False
                .GetColumn("recieverid").Visible = False
                .GetColumn("status").Visible = False
                .GetColumn("datenTime").Visible = False
                .GetColumn("body").Visible = False
                .GetColumn("deleted").Visible = False
                .GetColumn("receiver").Visible = False

            End With

        End If

    End Sub

    Private Sub radMail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles radMail.SelectedIndexChanged

        Dim item1 As Telerik.Web.UI.GridDataItem = DirectCast(radMail.SelectedItems(0), Telerik.Web.UI.GridDataItem)
        Dim markAsRead As Boolean = False

        If Not IsNothing(item1) Then

            Dim objMessageDb As New MessageHandler

            Dim Msg As Message = objMessageDb.GetMessageDetails(CookiesWrapper.thisUserID, Convert.ToInt32(IIf(IsNumeric(item1("messageid").Text), item1("messageid").Text, 0)), IIf(IIf(IsNumeric(item1("senderid").Text), item1("senderid").Text, 0) = CookiesWrapper.thisUserID, False, True))

            If Not IsNothing(Msg) Then

                pnlEditor.Visible = True
                txtFrom.Text = Msg.Sender
                txtSubject.Text = Msg.Subject

                radEMailMessage.Content = Server.HtmlDecode(Msg.Body)

                Session("Message") = Msg

            End If

        End If

    End Sub
End Class