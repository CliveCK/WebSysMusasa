Imports BusinessLogic

Partial Class IntakeDetailsControl
    Inherits System.Web.UI.UserControl

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then


        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadIntake(ByVal IntakeID As Long) As Boolean

        Try

            Dim objIntake As New Intake(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objIntake

                If .Retrieve(IntakeID) Then

                    txtIntakeID.Text = .IntakeID
                    radStartDate.SelectedDate = .StartDate
                    radEndDate.SelectedDate = .EndDate
                    txtDescription.Text = .Description

                    ShowMessage("Intake loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadIntake: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objIntake As New Intake(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objIntake

                .IntakeID = IIf(IsNumeric(txtIntakeID.Text), txtIntakeID.Text, 0)
                If radStartDate.SelectedDate.HasValue Then .StartDate = radStartDate.SelectedDate
                If radEndDate.SelectedDate.HasValue Then .EndDate = radEndDate.SelectedDate
                .Description = txtDescription.Text

                If .Save Then

                    If Not IsNumeric(txtIntakeID.Text) OrElse Trim(txtIntakeID.Text) = 0 Then txtIntakeID.Text = .IntakeID
                    ShowMessage("Intake saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failes to save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtIntakeID.Text = ""
        radStartDate.Clear()
        radEndDate.Clear()
        txtDescription.Text = ""

    End Sub

End Class

