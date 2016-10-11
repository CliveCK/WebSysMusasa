Imports BusinessLogic

Partial Class EligibilityDetailsControl
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

            If CookiesWrapper.PatientID > 0 Then

                LoadEligibility(CookiesWrapper.PatientID)

            End If

        End If

    End Sub


    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If CookiesWrapper.PatientID Then

            Save()

        Else

            ShowMessage("Please save Patient details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadEligibility(ByVal PatientID As Long) As Boolean

        Try

            Dim objEligibility As New Eligibility(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objEligibility

                If .RetrieveByPatient(PatientID) Then

                    txtEligibilityID.Text = .EligibilityID
                    txtPriority.Text = .Priority
                    txtEvaluation.Text = .Evaluation
                    txtComments.Text = .Comments

                    SetColor(.Priority)

                    ShowMessage("Eligibility loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadEligibility: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objEligibility As New Eligibility(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objEligibility

                .EligibilityID = IIf(IsNumeric(txtEligibilityID.Text), txtEligibilityID.Text, 0)
                .PatientID = CookiesWrapper.PatientID
                .Priority = txtPriority.Text
                .Evaluation = txtEvaluation.Text
                .Comments = txtComments.Text

                If .Save Then

                    If Not IsNumeric(txtEligibilityID.Text) OrElse Trim(txtEligibilityID.Text) = 0 Then txtEligibilityID.Text = .EligibilityID
                    LoadEligibility(.PatientID)
                    ShowMessage("Eligibility saved successfully...", MessageTypeEnum.Information)

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

        txtEligibilityID.Text = ""
        txtPriority.Text = ""
        txtEvaluation.Text = ""
        txtComments.Text = ""

    End Sub

    Private Sub txtPriority_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtPriority.SelectedIndexChanged

        SetColor(txtPriority.SelectedValue)

    End Sub

    Private Sub SetColor(ByVal mColor As String)

        Select Case txtPriority.SelectedValue
            Case "Low"
                txtPriority.BackColor = Drawing.Color.LightGreen

            Case "Medium"
                txtPriority.BackColor = Drawing.Color.Orange

            Case "High"
                txtPriority.BackColor = Drawing.Color.Red
        End Select

    End Sub
End Class

