Imports BusinessLogic

Partial Class FollowupsDetailsControl
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

            With cboFollowupType

                .DataSource = objLookup.Lookup("luFollowupTypes", "FollowupTypeID", "Description")
                .DataValueField = "FollowupTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If CookiesWrapper.PatientID > 0 Then

                LoadGrid()

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If CookiesWrapper.PatientID > 0 Then

            Save()

        Else

            ShowMessage("Please save Patient details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadFollowups(ByVal FollowupID As Long) As Boolean

        Try

            Dim objFollowups As New Followups(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFollowups

                If .Retrieve(FollowupID) Then

                    txtFollowupID.Text = .FollowupID
                    If Not IsNothing(cboFollowupType.Items.FindByValue(.FollowupTypeID)) Then cboFollowupType.SelectedValue = .FollowupTypeID
                    radFollowupDate.SelectedDate = .FollowupDate
                    txtFollowupTime.Text = .FollowupTime
                    txtLengthOfVisit.Text = .LengthOfVisit
                    txtCasePriority.Text = .CasePriority

                    ShowMessage("Followups loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadFollowups: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objFollowups As New Followups(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFollowups

                .FollowupID = IIf(IsNumeric(txtFollowupID.Text), txtFollowupID.Text, 0)
                .PatientID = CookiesWrapper.PatientID
                If cboFollowupType.SelectedIndex > -1 Then .FollowupTypeID = cboFollowupType.SelectedValue
                .FollowupDate = radFollowupDate.SelectedDate
                .FollowupTime = txtFollowupTime.Text
                .LengthOfVisit = txtLengthOfVisit.Text
                .CasePriority = txtCasePriority.Text

                If .Save Then

                    If Not IsNumeric(txtFollowupID.Text) OrElse Trim(txtFollowupID.Text) = 0 Then txtFollowupID.Text = .FollowupID
                    LoadFollowups(.FollowupID)
                    LoadGrid()
                    ShowMessage("Followups saved successfully...", MessageTypeEnum.Information)

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

    Private Sub LoadGrid()

        Dim objFollowups As New BusinessLogic.Followups(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radFollowupListing

            .DataSource = objFollowups.GetFollowUpByPatient(CookiesWrapper.PatientID)
            .DataBind()

            ViewState("Followups") = .DataSource

        End With

    End Sub

    Public Sub Clear()

        txtFollowupID.Text = ""
        If Not IsNothing(cboFollowupType.Items.FindByValue("")) Then
            cboFollowupType.SelectedValue = ""
        ElseIf Not IsNothing(cboFollowupType.Items.FindByValue(0)) Then
            cboFollowupType.SelectedValue = 0
        Else
            cboFollowupType.SelectedIndex = -1
        End If
        radFollowupDate.Clear()
        txtFollowupTime.Text = ""
        txtLengthOfVisit.Text = ""
        txtCasePriority.Text = ""

    End Sub

    Private Sub radFollowupListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radFollowupListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As Telerik.Web.UI.GridDataItem = radFollowupListing.Items(index)
            Dim FollowupID As Integer

            FollowupID = Server.HtmlDecode(item("FollowupID").Text)

            LoadFollowups(FollowupID)

        End If

    End Sub

    Private Sub radFollowupListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radFollowupListing.NeedDataSource

        radFollowupListing.DataSource = DirectCast(ViewState("Followups"), DataSet)

    End Sub
End Class

