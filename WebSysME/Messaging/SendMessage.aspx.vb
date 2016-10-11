Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports PrimaSysMessaging.BusinessLogic
Imports BusinessLogic

Partial Public Class SendMessage
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        LoadUserList()

        If IsPostBack = False Then

            If Request.QueryString("action") IsNot Nothing AndAlso Session("Message") IsNot Nothing Then
                Dim msg As Message = DirectCast(Session("Message"), Message)

                Select Case Request.QueryString("action").ToString()
                    Case "reply"
                        txtSubject.Text = "Re: " + msg.Subject
                        cboTo.Entries.Add(New Telerik.Web.UI.AutoCompleteBoxEntry(msg.Sender, msg.SenderId))
                        radEMailMessage.Content = Server.HtmlDecode("<br/><br/><br/> Original Message<hr/> From: " + msg.Sender + "<br/> To: " + msg.Receiver + "<br/> Message contents: " + msg.Body)
                        Exit Select

                    Case "forward"
                        txtSubject.Text = "Fw: " + msg.Subject
                        radEMailMessage.Content = Server.HtmlDecode("<br/><br/><br/> Original Message<hr/> From: " + msg.Sender + "<br/> To: " + msg.Receiver + "<br/> Message contents: " + msg.Body)
                        Exit Select
                End Select

            End If
        End If
    End Sub

    Private Sub LoadUserList()

        Dim userHandler As New UserHandler
        Dim table As DataTable = userHandler.GetUserList()

        With cboTo

            .DataSource = table
            .DataTextField = "username"
            .DataValueField = "userid"
            .DataBind()

        End With



    End Sub
    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles cmdSend.Click
        If cboTo.Entries.Count = 0 Then
            'Set the notification label here
            Return
        End If

        Dim userHandler As New UserHandler

        ' Let us get the list of valid users first
        Dim table As DataTable = userHandler.GetUserList()

        'Now get the recievers list entered by user  
        Dim failList As String() = New String(table.Rows.Count) {}
        Dim successList As String() = New String(table.Rows.Count) {}

        Dim successCount As Integer = 0
        Dim failCount As Integer = 0

        Dim handler As New MessageHandler()
        For Each user As Telerik.Web.UI.AutoCompleteBoxEntry In cboTo.Entries
            If userHandler.IsValidUser(user.Value) = True Then
                If True = handler.SendMessage(user.Value, CookiesWrapper.thisUserID.ToString(), txtSubject.Text, Server.HtmlEncode(radEMailMessage.Content)) Then
                    successList(System.Math.Max(System.Threading.Interlocked.Increment(successCount), successCount - 1)) = user.Value
                Else
                    failList(System.Math.Max(System.Threading.Interlocked.Increment(failCount), failCount - 1)) = user.Value
                End If
            Else
                failList(System.Math.Max(System.Threading.Interlocked.Increment(failCount), failCount - 1)) = user.Value
            End If
        Next

        SendEmail(cboTo, Server.HtmlDecode(radEMailMessage.Content), txtSubject.Text)

        Session("SuccessList") = successList
        Session("FailList") = failList

        Page.ClientScript.RegisterStartupScript(GetType(Mail), "CloseWindow", "CloseAndRebind('Rebind');", True)
    End Sub

    Private Sub SendEmail(ByVal UserID As Telerik.Web.UI.RadAutoCompleteBox, ByVal Message As String, ByVal Subject As String)

        'Get the User Email address
        Dim User As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim email As New List(Of String)
        Dim Cell() As String = New String() {}

        For Each u As Telerik.Web.UI.AutoCompleteBoxEntry In UserID.Entries

            With User

                .Retrieve(u.Value)
                email.Add(.EmailAddress)

            End With

        Next

        Dim objCampaign As New BusinessLogic.EmailSMSDistribution(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If objCampaign.CreateCampaign(IIf(User.Retrieve(CookiesWrapper.thisUserID), User.EmailAddress, ""), Cell, email.ToArray, Message, Subject, "") Then

        End If

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("Default.aspx")
    End Sub
End Class
