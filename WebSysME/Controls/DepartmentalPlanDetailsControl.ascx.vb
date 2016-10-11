Imports BusinessLogic

Partial Class DepartmentalPlanDetailsControl
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

            With cboStatus

                .DataSource = objLookup.Lookup("tblOrganizationalPlan", "OrganizationPlanID", "Activity").Tables(0)
                .DataValueField = "OrganizationPlanID"
                .DataTextField = "Activity"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboStatus

                .DataSource = objLookup.Lookup("luActivityStatus", "ActivityStatusID", "Description").Tables(0)
                .DataValueField = "ActivityStatusID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadDepartmentalPlan(ByVal DepartmentPlanID As Long) As Boolean

        Try

            Dim objDepartmentalPlan As New DepartmentalPlan(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objDepartmentalPlan

                If .Retrieve(DepartmentPlanID) Then

                    txtDepartmentPlanID.Text = .DepartmentPlanID
                    If Not IsNothing(cboOrganizationPlan.Items.FindByValue(.OrganizationPlanID)) Then cboOrganizationPlan.SelectedValue = .OrganizationPlanID
                    txtYear.Text = .Year
                    If Not IsNothing(cboPeriod.Items.FindByValue(.Period)) Then cboPeriod.SelectedValue = .Period
                    If Not IsNothing(cboStatus.Items.FindByValue(.StatusID)) Then cboStatus.SelectedValue = .StatusID
                    txtActivityCategory.Text = .ActivityCategory
                    txtActivity.Text = .Activity
                    txtComments.Text = .Comments

                    ShowMessage("DepartmentalPlan loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadDepartmentalPlan: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objDepartmentalPlan As New DepartmentalPlan(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objDepartmentalPlan

                .DepartmentPlanID = IIf(IsNumeric(txtDepartmentPlanID.Text), txtDepartmentPlanID.Text, 0)
                If cboOrganizationPlan.SelectedIndex > -1 Then .OrganizationPlanID = cboOrganizationPlan.SelectedValue
                .Year = txtYear.Text
                .Period = cboPeriod.SelectedValue
                If cboStatus.SelectedIndex > -1 Then .StatusID = cboStatus.SelectedValue
                .ActivityCategory = txtActivityCategory.Text
                .Activity = txtActivity.Text
                .Comments = txtComments.Text

                If .Save Then

                    If Not IsNumeric(txtDepartmentPlanID.Text) OrElse Trim(txtDepartmentPlanID.Text) = 0 Then txtDepartmentPlanID.Text = .DepartmentPlanID
                    LoadDepartmentalPlan(.DepartmentPlanID)
                    ShowMessage("DepartmentalPlan saved successfully...", MessageTypeEnum.Information)

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

        If Not IsNothing(cboPeriod.Items.FindByValue("")) Then
            cboPeriod.SelectedValue = ""
        ElseIf Not IsNothing(cboPeriod.Items.FindByValue(0)) Then
            cboPeriod.SelectedValue = 0
        Else
            cboPeriod.SelectedIndex = -1
        End If
        If Not IsNothing(cboOrganizationPlan.Items.FindByValue("")) Then
            cboOrganizationPlan.SelectedValue = ""
        ElseIf Not IsNothing(cboOrganizationPlan.Items.FindByValue(0)) Then
            cboOrganizationPlan.SelectedValue = 0
        Else
            cboOrganizationPlan.SelectedIndex = -1
        End If
        txtYear.Text = 0
        txtDepartmentPlanID.Text = 0
        If Not IsNothing(cboStatus.Items.FindByValue("")) Then
            cboStatus.SelectedValue = ""
        ElseIf Not IsNothing(cboStatus.Items.FindByValue(0)) Then
            cboStatus.SelectedValue = 0
        Else
            cboStatus.SelectedIndex = -1
        End If
        txtActivityCategory.Text = ""
        txtActivity.Text = ""
        txtComments.Text = ""

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Clear()

    End Sub
End Class

