Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class ProjectMeetingAgendaDetailsControl
    Inherits System.Web.UI.UserControl

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


    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ProjectMeetingIDText As TextBox = DirectCast(Parent.Parent.FindControl("txtProjectMeetingID"), TextBox)
        txtProjectMeetingID.Text = ProjectMeetingIDText.Text

        If IsNumeric(txtProjectMeetingID.Text) Then LoadGrid()

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If IsNumeric(txtProjectMeetingID.Text) AndAlso txtProjectMeetingID.Text > 0 Then

            Save()

        Else

            ShowMessage("Please save Meeting details first!", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadProjectMeetingAgenda(ByVal ProjectMeetingAgendaID As Long) As Boolean

        Try

            Dim objProjectMeetingAgenda As New ProjectMeetingAgenda(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProjectMeetingAgenda

                If .Retrieve(ProjectMeetingAgendaID) Then

                    txtProjectMeetingAgendaID.Text = .ProjectMeetingAgendaID
                    txtProjectMeetingID.Text = .ProjectMeetingID
                    txtActivity.Text = .Activity
                    txtComments.Text = .Comments

                    ShowMessage("Project Meeting Agenda details loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function Save() As Boolean

        Try

            Dim objProjectMeetingAgenda As New ProjectMeetingAgenda(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProjectMeetingAgenda

                .ProjectMeetingAgendaID = IIf(IsNumeric(txtProjectMeetingAgendaID.Text), txtProjectMeetingAgendaID.Text, 0)
                If IsNumeric(txtProjectMeetingID.Text) Then .ProjectMeetingID = txtProjectMeetingID.Text Else Return False
                .Activity = txtActivity.Text
                .Comments = txtComments.Text

                If .Save Then

                    If Not IsNumeric(txtProjectMeetingAgendaID.Text) OrElse Trim(txtProjectMeetingAgendaID.Text) = 0 Then txtProjectMeetingAgendaID.Text = .ProjectMeetingAgendaID
                    LoadGrid()
                    ShowMessage("Project Meeting Agenda details saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function LoadGrid()

        If IsNumeric(txtProjectMeetingID.Text) Then

            Dim objAgenda As New ProjectMeetingAgenda(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With radAgenda

                .DataSource = objAgenda.GetProjectMeetingAgenda("SELECT * FROM tblProjectMeetingAgenda WHERE ProjectMeetingID = " & txtProjectMeetingID.Text).Tables(0)
                .DataBind()

                ViewState("MeetingAgenda") = .DataSource

            End With

        Else

            ShowMessage("Failed to load grid: Missing paramter...", MessageTypeEnum.Error)

        End If

    End Function

    Public Sub Clear()

        txtProjectMeetingAgendaID.Text = ""
        txtActivity.Text = ""
        txtComments.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub radAgenda_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radAgenda.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radAgenda.Items(index)

            LoadProjectMeetingAgenda(Server.HtmlDecode(item("ProjectMeetingAgendaID").Text))

        End If

    End Sub

    Private Sub radAgenda_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radAgenda.NeedDataSource

        radAgenda.DataSource = DirectCast(ViewState("MeetingAgenda"), DataTable)

    End Sub
End Class

