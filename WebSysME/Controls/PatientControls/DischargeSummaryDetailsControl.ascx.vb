Imports BusinessLogic

Partial Class DischargeSummaryDetailsControl
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

            With cboHospital

                .DataSource = objLookup.Lookup("tblHealthCenters", "HealthCenterID", "Name")
                .DataValueField = "HealthCenterID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If CookiesWrapper.PatientID <> 0 Then

                LoadDischargeSummary(CookiesWrapper.PatientID)

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadDischargeSummary(ByVal PatientID As Long) As Boolean

        Try

            Dim objDischargeSummary As New DischargeSummary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objDischargeSummary

                If .RetrieveByPatient(PatientID) Then

                    txtDischargeSummaryID.Text = .DischargeSummaryID
                    txtDischargePatientID.Text = .PatientID
                    If Not IsNothing(cboHospital.Items.FindByValue(.HospitalID)) Then cboHospital.SelectedValue = .HospitalID
                    If Not .ReviewDate = "" Then radReviewDate.SelectedDate = .ReviewDate
                    If Not .DischargedDate = "" Then radDischargedDate.SelectedDate = .DischargedDate
                    txtDischarged.Text = .Discharged
                    txtWard.Text = .Ward
                    txtDischargedTo.Text = .DischargedTo
                    txtReasonForDischarge.Text = .ReasonForDischarge

                    ShowMessage("Discharge Summary loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Discharge Summary", MessageTypeEnum.Error)
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

            Dim objDischargeSummary As New DischargeSummary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            If IsNumeric(CookiesWrapper.PatientID) AndAlso CookiesWrapper.PatientID > 0 Then

                With objDischargeSummary

                    .DischargeSummaryID = IIf(IsNumeric(txtDischargeSummaryID.Text), txtDischargeSummaryID.Text, 0)
                    .PatientID = IIf(IsNumeric(txtDischargePatientID.Text), txtDischargePatientID.Text, CookiesWrapper.PatientID)
                    If cboHospital.SelectedIndex > -1 Then .HospitalID = cboHospital.SelectedValue
                    If radDischargedDate.SelectedDate.HasValue Then .DischargedDate = radDischargedDate.SelectedDate
                    If radReviewDate.SelectedDate.HasValue Then .ReviewDate = radReviewDate.SelectedDate
                    .Discharged = txtDischarged.Text
                    .Ward = txtWard.Text
                    .DischargedTo = txtDischargedTo.Text
                    .ReasonForDischarge = txtReasonForDischarge.Text

                    If .Save Then

                        If Not IsNumeric(txtDischargeSummaryID.Text) OrElse Trim(txtDischargeSummaryID.Text) = 0 Then txtDischargeSummaryID.Text = .DischargeSummaryID
                        LoadDischargeSummary(.PatientID)
                        ShowMessage("Discharge Summary saved successfully...", MessageTypeEnum.Information)

                        Return True

                    Else

                        ShowMessage("Error saving details...", MessageTypeEnum.Error)
                        Return False

                    End If

                End With

            Else

                ShowMessage("Please save Patient details first...", MessageTypeEnum.Error)

            End If


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtDischargeSummaryID.Text = ""
        txtDischargePatientID.Text = ""
        If Not IsNothing(cboHospital.Items.FindByValue("")) Then
            cboHospital.SelectedValue = ""
        ElseIf Not IsNothing(cboHospital.Items.FindByValue(0)) Then
            cboHospital.SelectedValue = 0
        Else
            cboHospital.SelectedIndex = -1
        End If
        radReviewDate.Clear()
        radDischargedDate.Clear()
        txtDischarged.Text = ""
        txtWard.Text = ""
        txtDischargedTo.Text = ""
        txtReasonForDischarge.Text = ""

    End Sub

End Class

