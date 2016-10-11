Imports BusinessLogic

Partial Class ReferralsDetailsControl
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

            If CookiesWrapper.PatientID Then

                LoadGrid()

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If CookiesWrapper.PatientID > 0 Then

            Save()

        Else

            ShowMessage("Please save Patient details before saving this tab", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub LoadGrid()

        Dim objReferrals As New Referrals(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "SELECT * FROM tblReferrals WHERE PatientID = " & CookiesWrapper.PatientID

        With radReferrals

            .DataSource = objReferrals.GetReferrals(sql)
            .DataBind()

            ViewState("Referrals") = .DataSource

        End With

    End Sub

    Public Function LoadReferrals(ByVal ReferralID As Long) As Boolean

        Try

            Dim objReferrals As New Referrals(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objReferrals

                If .Retrieve(ReferralID) Then

                    txtReferralID.Text = .ReferralID
                    txtPatientID.Text = .PatientID
                    radDate.SelectedDate = .ReferredDate
                    txtReferredTo.Text = .ReferredTo
                    txtPatientNo.Text = .PatientNo
                    txtDignosisConcern.Text = .DignosisConcern
                    txtAssistanceNeeded.Text = .AssistanceNeeded
                    txtAssistanceOffered.Text = .AssistanceOffered

                    ShowMessage("Referrals loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Referrals...", MessageTypeEnum.Error)
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

            Dim objReferrals As New Referrals(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objReferrals

                .ReferralID = IIf(IsNumeric(txtReferralID.Text), txtReferralID.Text, 0)
                .PatientID = CookiesWrapper.PatientID
                .ReferredDate = radDate.SelectedDate
                .ReferredTo = txtReferredTo.Text
                .PatientNo = txtPatientNo.Text
                .DignosisConcern = txtDignosisConcern.Text
                .AssistanceNeeded = txtAssistanceNeeded.Text
                .AssistanceOffered = txtAssistanceOffered.Text

                If .Save Then

                    If Not IsNumeric(txtReferralID.Text) OrElse Trim(txtReferralID.Text) = 0 Then txtReferralID.Text = .ReferralID
                    LoadReferrals(.ReferralID)
                    ShowMessage("Referrals saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to load Referral", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtReferralID.Text = ""
        txtPatientID.Text = ""
        radDate.Clear()
        txtReferredTo.Text = ""
        txtPatientNo.Text = ""
        txtDignosisConcern.Text = ""
        txtAssistanceNeeded.Text = ""
        txtAssistanceOffered.Text = ""

    End Sub

    Private Sub radReferrals_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radReferrals.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As Telerik.Web.UI.GridDataItem = radReferrals.Items(index)
            Dim ReferralID As Integer

            ReferralID = Server.HtmlDecode(item("ReferralID").Text)

            LoadReferrals(ReferralID)

        End If

    End Sub
End Class

