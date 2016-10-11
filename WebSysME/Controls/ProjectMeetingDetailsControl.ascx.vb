Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class ProjectMeetingDetailsControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

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

        If Not IsNothing(Request.QueryString("id")) Then

            txtProjectID.Text = objUrlEncoder.Decrypt(Request.QueryString("id"))

        End If

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadProjectMeeting(ByVal ProjectMeetingID As Long) As Boolean

        Try

            Dim objProjectMeeting As New ProjectMeeting(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProjectMeeting

                If .Retrieve(ProjectMeetingID) Then

                    txtProjectMeetingID.Text = .ProjectMeetingID
                    txtProjectID.Text = .ProjectID
                    radDateOfMeeting.SelectedDate = .DateOfMeeting
                    txtPlace.Text = .Place
                    txtPurpose.Text = .Purpose

                    ucProjectMeetingAttendants.Page_Load(Me, New System.EventArgs)
                    ucProjectMeetingAgenda.Page_Load(Me, New System.EventArgs)
                    ucProjectMeetingDocuments.Page_Load(Me, New System.EventArgs)

                    ShowMessage("ProjectMeeting loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadProjectMeeting: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objProjectMeeting As New ProjectMeeting(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProjectMeeting

                .ProjectMeetingID = IIf(IsNumeric(txtProjectMeetingID.Text), txtProjectMeetingID.Text, 0)
                If IsNumeric(txtProjectID.Text) Then .ProjectID = txtProjectID.Text Else Return False
                .DateOfMeeting = radDateOfMeeting.SelectedDate
                .Place = txtPlace.Text
                .Purpose = txtPurpose.Text

                If .Save Then

                    If Not IsNumeric(txtProjectMeetingID.Text) OrElse Trim(txtProjectMeetingID.Text) = 0 Then txtProjectMeetingID.Text = .ProjectMeetingID
                    LoadProjectMeeting(.ProjectMeetingID)
                    LoadGrid()
                    ShowMessage("Project Meeting details saved successfully...", MessageTypeEnum.Information)

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

    Public Sub Clear()

        txtProjectMeetingID.Text = ""
        txtProjectID.Text = ""
        radDateOfMeeting.Clear()
        txtPlace.Text = ""
        txtPurpose.Text = ""

    End Sub

    Public Sub LoadGrid()

        If IsNumeric(txtProjectID.Text) Then

            Dim objMeeting As New BusinessLogic.ProjectMeeting(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim ds As DataSet = objMeeting.GetProjectMeeting("SELECT * FROM tblProjectMeetings WHERE ProjectID = " & txtProjectID.Text)

            With radMeetingListing

                .DataSource = ds.Tables(0)
                .DataBind()

                ViewState("ProjectMeeting") = .DataSource

            End With

        Else

            ShowMessage("Failed to load grid: Missing parameter", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radMeetingListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radMeetingListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "ViewMeetingDetails" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radMeetingListing.Items(index)

                Dim ProjectMeetingID As Long = Server.HtmlDecode(item("ProjectMeetingID").Text)

                LoadProjectMeeting(ProjectMeetingID)

            End If

        End If

    End Sub

    Private Sub radMeetingListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radMeetingListing.NeedDataSource

        radMeetingListing.DataSource = DirectCast(ViewState("ProjectMeeting"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub
End Class

