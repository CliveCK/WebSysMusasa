Imports BusinessLogic

Partial Class PrognosisDetailsControl
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

            If CookiesWrapper.PatientID <> 0 Then

                LoadPrognosis(CookiesWrapper.PatientID)

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadPrognosis(ByVal PatientID As Long) As Boolean

        Try

            Dim objPrognosis As New Prognosis(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objPrognosis

                If .RetrieveByPatient(PatientID) Then

                    txtPrognosisID.Text = .PrognosisID
                    txtPrognosisPatientID.Text = .PatientID
                    If Not .Date1 = "" Then radDate1.SelectedDate = .Date1
                    If Not .Date2 = "" Then radDate2.SelectedDate = .Date2
                    txtPrognosis1.Text = .Prognosis1
                    txtPrognosis2.Text = .Prognosis2

                    ShowMessage("Prognosis loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Prognosis", MessageTypeEnum.Error)
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

            Dim objPrognosis As New Prognosis(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            If IsNumeric(CookiesWrapper.PatientID) AndAlso CookiesWrapper.PatientID > 0 Then

                With objPrognosis

                    .PrognosisID = IIf(IsNumeric(txtPrognosisID.Text), txtPrognosisID.Text, 0)
                    .PatientID = IIf(IsNumeric(txtPrognosisPatientID.Text), txtPrognosisPatientID.Text, CookiesWrapper.PatientID)
                    If radDate1.SelectedDate.HasValue Then .Date1 = radDate1.SelectedDate
                    If radDate2.SelectedDate.HasValue Then .Date2 = radDate2.SelectedDate
                    .Prognosis1 = txtPrognosis1.Text
                    .Prognosis2 = txtPrognosis2.Text

                    If .Save Then

                        If Not IsNumeric(txtPrognosisID.Text) OrElse Trim(txtPrognosisID.Text) = 0 Then txtPrognosisID.Text = .PrognosisID
                        LoadPrognosis(.PatientID)
                        ShowMessage("Prognosis saved successfully...", MessageTypeEnum.Information)

                        Return True

                    Else

                        ShowMessage("Error saving details...", MessageTypeEnum.Error)
                        Return False

                    End If

                End With

            Else

                ShowMessage("Save patient profile first before attempting to save Prognosis...", MessageTypeEnum.Error)

            End If

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtPrognosisID.Text = ""
        radDate1.Clear()
        radDate2.Clear()
        txtPrognosis1.Text = ""
        txtPrognosis2.Text = ""

    End Sub

End Class

