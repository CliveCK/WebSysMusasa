Imports BusinessLogic

Partial Class MedicalHistoryDetailsControl
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

            With cboCondition

                .DataSource = objLookup.Lookup("luConditions", "ConditionID", "Description")
                .DataValueField = "ConditionID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboDiagnosis

                .DataSource = objLookup.Lookup("luDiagnosis", "DiagnosisID", "Description")
                .DataValueField = "DiagnosisID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboReasonForUnstaging

                .DataSource = objLookup.Lookup("luReasonForUnstaging", "ReasonForUnstagingID", "Description")
                .DataValueField = "ReasonForUnstagingID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If CookiesWrapper.PatientID > 0 Then

                LoadMedicalHistory(CookiesWrapper.PatientID)

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

    Public Function LoadMedicalHistory(ByVal MedicalHistoryID As Long) As Boolean

        Try

            Dim objMedicalHistory As New MedicalHistory(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objMedicalHistory

                If .Retrieve(MedicalHistoryID) Then

                    txtMedicalHistoryID.Text = .MedicalHistoryID
                    If Not IsNothing(cboReasonForUnstaging.Items.FindByText(.ReasonForUnstaging)) Then cboReasonForUnstaging.SelectedItem.Text = .ReasonForUnstaging
                    If Not IsNothing(cboCondition.Items.FindByValue(.ConditionID)) Then cboCondition.SelectedValue = .ConditionID
                    If Not IsNothing(.DateOdDiagnosis) AndAlso .DateOdDiagnosis <> "" Then radDateOfDiagnosis.SelectedDate = .DateOdDiagnosis
                    If Not IsNothing(cboDiagnosis.Items.FindByValue(.Diagnosis)) Then cboDiagnosis.SelectedValue = .Diagnosis

                    ShowMessage("MedicalHistory loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    'ShowMessage("Failed to loadMedicalHistory: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objMedicalHistory As New MedicalHistory(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objMedicalHistory

                .MedicalHistoryID = IIf(IsNumeric(txtMedicalHistoryID.Text), txtMedicalHistoryID.Text, 0)
                If cboReasonForUnstaging.SelectedIndex > -1 Then .ReasonForUnstaging = cboReasonForUnstaging.SelectedItem.Text
                If cboCondition.SelectedIndex > -1 Then .ConditionID = cboCondition.SelectedValue
                If radDateOfDiagnosis.SelectedDate.HasValue Then .DateOdDiagnosis = radDateOfDiagnosis.SelectedDate
                If cboDiagnosis.SelectedIndex > -1 Then .Diagnosis = cboDiagnosis.SelectedValue
                .PatientID = CookiesWrapper.PatientID

                If .Save Then

                    If Not IsNumeric(txtMedicalHistoryID.Text) OrElse Trim(txtMedicalHistoryID.Text) = 0 Then txtMedicalHistoryID.Text = .MedicalHistoryID
                    ShowMessage("MedicalHistory saved successfully...", MessageTypeEnum.Information)

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

        If Not IsNothing(cboReasonForUnstaging.Items.FindByValue("")) Then
            cboReasonForUnstaging.SelectedValue = ""
        ElseIf Not IsNothing(cboReasonForUnstaging.Items.FindByValue(0)) Then
            cboReasonForUnstaging.SelectedValue = 0
        Else
            cboReasonForUnstaging.SelectedIndex = -1
        End If
        If Not IsNothing(cboCondition.Items.FindByValue("")) Then
            cboCondition.SelectedValue = ""
        ElseIf Not IsNothing(cboCondition.Items.FindByValue(0)) Then
            cboCondition.SelectedValue = 0
        Else
            cboCondition.SelectedIndex = -1
        End If
        radDateOfDiagnosis.Clear()
        If Not IsNothing(cboDiagnosis.Items.FindByValue("")) Then
            cboDiagnosis.SelectedValue = ""
        ElseIf Not IsNothing(cboDiagnosis.Items.FindByValue(0)) Then
            cboDiagnosis.SelectedValue = 0
        Else
            cboDiagnosis.SelectedIndex = -1
        End If

    End Sub

End Class

