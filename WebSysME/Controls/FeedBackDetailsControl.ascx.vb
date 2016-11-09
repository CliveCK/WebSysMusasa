Imports BusinessLogic

Partial Class FeedBackDetailsControl
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

            With cboHowHelpful

                .DataSource = objLookup.Lookup("luFeebackHelp", "FeedBackHelpID", "Description")
                .DataTextField = "Description"
                .DataValueField = "FeedBackHelpID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboNatureOfProblem

                .DataSource = objLookup.Lookup("luNatureOfProblems", "NatureOfProblemID", "Description")
                .DataTextField = "Description"
                .DataValueField = "NatureOfProblemID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboAssistance

                .DataSource = objLookup.Lookup("luAssistenceAndServicesProvided", "AssistenceAndServicesID", "Description")
                .DataTextField = "Description"
                .DataValueField = "AssistenceAndServicesID"
                .DataBind()

                .Items.Insert(0, New ListItem("--Select--", ""))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadFeedBack(ByVal FeedbackID As Long) As Boolean

        Try

            Dim objFeedBack As New FeedBack(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFeedBack

                If .Retrieve(FeedbackID) Then

                    txtFeedbackID.Text = .FeedbackID
                    txtAge.Text = .Age
                    If Not IsNothing(cboNatureOfProblem.Items.FindByValue(.NatureOfProblemID)) Then cboNatureOfProblem.SelectedValue = .NatureOfProblemID
                    If Not IsNothing(cboAssistance.Items.FindByValue(.AssistanceID)) Then cboAssistance.SelectedValue = .AssistanceID
                    If Not .DateCompleted = "" Then radDate.SelectedDate = .DateCompleted
                    txtOfficeName.Text = .OfficeName
                    cboSex.SelectedValue = .Sex
                    txtOtherProblem.Text = .OtherProblem
                    txtOtherAssistance.Text = .OtherAssistance
                    cboSatisfiedOutcomeOfCase.SelectedValue = .SatisfiedOutcomeOfCase
                    txtExplainWhyCase.Text = .ExplainWhyCase
                    txtRecommendations.Text = .Recommendations
                    cboWouldRecommend.SelectedValue = .WouldRecommend
                    txtOtherChallenges.Text = .OtherChallenges
                    txtExpectation.Text = .Expectation
                    cboExpectationFulfilled.SelectedValue = .ExpectationFulfilled
                    txtExplainExpectation.Text = .ExplainExpectation
                    cboFeelSafe.SelectedValue = .FeelSafe
                    txtExplainFeelSafe.Text = .ExplainFeelSafe
                    cboSatisfiedWithService.SelectedValue = .SatisfiedWithService
                    txtExplaiWhyService.Text = .ExplaiWhyService
                    cboHelpInOtherAreas.SelectedValue = .HelpInOtherAreas
                    cboHowHelpful.SelectedValue = .HowHelpful
                    txtChallengesWithOrganization.Text = .ChallengesWithOrganization
                    txtChallengesWithServiceDelivery.Text = .ChallengesWithServiceDelivery

                    ShowMessage("FeedBack details loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load FeedBack details", MessageTypeEnum.Error)
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

            Dim objFeedBack As New FeedBack(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFeedBack

                .FeedbackID = IIf(IsNumeric(txtFeedbackID.Text), txtFeedbackID.Text, 0)
                .Age = txtAge.Text
                If cboNatureOfProblem.SelectedIndex > -1 Then .NatureOfProblemID = cboNatureOfProblem.SelectedValue
                If cboAssistance.SelectedIndex > -1 Then .AssistanceID = cboAssistance.SelectedValue
                If radDate.SelectedDate.HasValue Then .DateCompleted = radDate.SelectedDate
                .OfficeName = txtOfficeName.Text
                .Sex = cboSex.SelectedValue
                .OtherProblem = txtOtherProblem.Text
                .OtherAssistance = txtOtherAssistance.Text
                .SatisfiedOutcomeOfCase = cboSatisfiedOutcomeOfCase.SelectedValue
                .ExplainWhyCase = txtExplainWhyCase.Text
                .Recommendations = txtRecommendations.Text
                .WouldRecommend = cboWouldRecommend.SelectedValue
                .OtherChallenges = txtOtherChallenges.Text
                .Expectation = txtExpectation.Text
                .ExpectationFulfilled = cboExpectationFulfilled.SelectedValue
                .ExplainExpectation = txtExplainExpectation.Text
                .FeelSafe = cboFeelSafe.SelectedValue
                .ExplainFeelSafe = txtExplainFeelSafe.Text
                .SatisfiedWithService = cboSatisfiedWithService.SelectedValue
                .ExplaiWhyService = txtExplaiWhyService.Text
                .HelpInOtherAreas = cboHelpInOtherAreas.SelectedValue
                .HowHelpful = cboHowHelpful.SelectedValue
                .ChallengesWithOrganization = txtChallengesWithOrganization.Text
                .ChallengesWithServiceDelivery = txtChallengesWithServiceDelivery.Text

                If .Save Then

                    If Not IsNumeric(txtFeedbackID.Text) OrElse Trim(txtFeedbackID.Text) = 0 Then txtFeedbackID.Text = .FeedbackID
                    ShowMessage("FeedBack saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtFeedbackID.Text = ""
        txtAge.Text = 0
        If Not IsNothing(cboNatureOfProblem.Items.FindByValue("")) Then
            cboNatureOfProblem.SelectedValue = ""
        ElseIf Not IsNothing(cboNatureOfProblem.Items.FindByValue(0)) Then
            cboNatureOfProblem.SelectedValue = 0
        Else
            cboNatureOfProblem.SelectedIndex = -1
        End If
        If Not IsNothing(cboAssistance.Items.FindByValue("")) Then
            cboAssistance.SelectedValue = ""
        ElseIf Not IsNothing(cboAssistance.Items.FindByValue(0)) Then
            cboAssistance.SelectedValue = 0
        Else
            cboAssistance.SelectedIndex = -1
        End If
        radDate.Clear()
        txtOfficeName.Text = ""
        cboSex.SelectedValue = ""
        txtOtherProblem.Text = ""
        txtOtherAssistance.Text = ""
        cboSatisfiedOutcomeOfCase.SelectedValue = ""
        txtExplainWhyCase.Text = ""
        txtRecommendations.Text = ""
        cboWouldRecommend.SelectedValue = ""
        txtOtherChallenges.Text = ""
        txtExpectation.Text = ""
        cboExpectationFulfilled.SelectedValue = ""
        txtExplainExpectation.Text = ""
        cboFeelSafe.SelectedValue = ""
        txtExplainFeelSafe.Text = ""
        cboSatisfiedWithService.SelectedValue = ""
        txtExplaiWhyService.Text = ""
        cboHelpInOtherAreas.SelectedValue = ""
        cboHowHelpful.SelectedValue = ""
        txtChallengesWithOrganization.Text = ""
        txtChallengesWithServiceDelivery.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/FeedbackForm.aspx")

    End Sub
End Class

