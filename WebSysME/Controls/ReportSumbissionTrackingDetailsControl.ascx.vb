Imports BusinessLogic

Partial Class ReportSumbissionTrackingDetailsControl
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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboOrganization

                .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name").Tables(0)
                .DataValueField = "OrganizationID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With lstUsers

                .DataSource = objLookup.SqlExec("SELECT UserFirstname + ' ' + UserSurname As UserName, UserID from tblUsers where Deleted = 0")
                .DataTextField = "UserName"
                .DataValueField = "UserID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadReportSumbissionTracking(ByVal ReportSubmissionTrackingID As Long) As Boolean

        Try

            Dim objReportSumbissionTracking As New ReportSumbissionTracking(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objReportSumbissionTracking

                If .Retrieve(ReportSubmissionTrackingID) Then

                    txtReportSubmissionTrackingID.Text = .ReportSubmissionTrackingID
                    If Not IsNothing(cboOrganization.Items.FindByValue(.OrganizationID)) Then cboOrganization.SelectedValue = .OrganizationID
                    radExpectedDate.SelectedDate = .ExpectedDate
                    radActualSub.SelectedDate = .ActualSubmissionDate
                    txtReportTitle.Text = .ReportTitle
                    txtAuthor.Text = .Author
                    txtComments.Text = .Comments

                    ShowMessage("Report Sumbission Information loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Report Sumbission", MessageTypeEnum.Error)
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

            Dim objReportSumbissionTracking As New ReportSumbissionTracking(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objReportSumbissionTracking

                .ReportSubmissionTrackingID = IIf(IsNumeric(txtReportSubmissionTrackingID.Text), txtReportSubmissionTrackingID.Text, 0)
                If cboOrganization.SelectedIndex > -1 Then .OrganizationID = cboOrganization.SelectedValue
                If radExpectedDate.SelectedDate.HasValue Then .ExpectedDate = radExpectedDate.SelectedDate
                .ReportTitle = txtReportTitle.Text
                .Author = txtAuthor.Text
                .Comments = txtComments.Text

                If .Save Then

                    Dim objNotification As New PrimaSysMessaging.BusinessLogic.MessageHandler()
                    Dim UserID As New List(Of Long)
                    Dim MessageBody = "<br />Report Title: " & txtReportTitle.Text & "<br />"
                    MessageBody &= "Due Date: " & radExpectedDate.SelectedDate & "<br /><br />"
                    MessageBody &= txtComments.Text & "<br /><br />"
                    MessageBody &= "Sent by: " & CookiesWrapper.thisUserFullName

                    For Each u As ListItem In lstUsers.Items

                        If u.Selected Then

                            UserID.Add(u.Value)

                        End If

                    Next

                    If UserID.Count > 0 Then

                        objNotification.SendEmail(UserID.ToArray, MessageBody, "Report Submission")

                    End If

                    If Not IsNumeric(txtReportSubmissionTrackingID.Text) OrElse Trim(txtReportSubmissionTrackingID.Text) = 0 Then txtReportSubmissionTrackingID.Text = .ReportSubmissionTrackingID
                    ShowMessage("Report Sumbission details saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details..", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtReportSubmissionTrackingID.Text = ""
        If Not IsNothing(cboOrganization.Items.FindByValue("")) Then
            cboOrganization.SelectedValue = ""
        ElseIf Not IsNothing(cboOrganization.Items.FindByValue(0)) Then
            cboOrganization.SelectedValue = 0
        Else
            cboOrganization.SelectedIndex = -1
        End If
        radActualSub.Clear()
        radExpectedDate.Clear()
        txtReportTitle.Text = ""
        txtAuthor.Text = ""
        txtComments.Text = ""

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Clear()

    End Sub
End Class

