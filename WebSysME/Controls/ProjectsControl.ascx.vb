Imports BusinessLogic
Imports Telerik.Web.UI
Imports SysPermissionsManager.Functionality

Public Class ProjectsControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Private objCustomFields As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)
    Private ProjectID As Long

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

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        Dim objHealth As New HealthCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If Not IsNothing(Request.QueryString("id")) Then

            ProjectID = objUrlEncoder.Decrypt(Request.QueryString("id"))

            phCustomFields.Controls.Add(objCustomFields.LoadStatusTemplateCustomFieldsPanel(Page, ProjectID, "P", My.Settings.DisplayDateFormat, objHealth.GetCustomFieldsObjectIDByCode("P"), "P", True, False))

        Else

            phCustomFields.Controls.Add(objCustomFields.LoadStatusTemplateCustomFieldsPanel(Page, 0, "P", My.Settings.DisplayDateFormat, objHealth.GetCustomFieldsObjectIDByCode("P"), "P", True, False))

        End If

    End Sub


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            LoadComboBoxes()
            If ProjectID > 0 Then LoadProjects(ProjectID)

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If Not SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.AddOrEditProjects) Then

            ShowMessage("You are not authorised to Add/Edit Project details!", MessageTypeEnum.Error)

        Else

            Save()

        End If

    End Sub

    Private Sub LoadComboBoxes()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With lstStrategicObjective

            .DataSource = objLookup.Lookup("tblStrategicObjectives", "StrategicObjectiveID", "Code").Tables(0)
            .DataValueField = "StrategicObjectiveID"
            .DataTextField = "Code"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With lstKeyChangePromise

            .DataSource = objLookup.Lookup("tblKeyChangePromises", "KeyChangePromiseID", "KeyChangePromiseNo").Tables(0)
            .DataValueField = "KeyChangePromiseID"
            .DataTextField = "KeyChangePromiseNo"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            '.SelectedIndex = 0

        End With

        With cboProgram

            .DataSource = objLookup.Lookup("tblProgrammes", "ProgramID", "Name").Tables(0)
            .DataValueField = "ProgramID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboProjectManager

            Dim sql As String = ""

            .DataSource = objLookup.SqlExec("select S.StaffID, StaffFullName from tblStaffMembers S inner join tblUserAccountProfileLink L on S.StaffID = L.StaffID ORDER BY StaffFullName")
            .DataValueField = "StaffID"
            .DataTextField = "StaffFullName"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            '.SelectedIndex = 0

        End With

    End Sub

    Private Sub ClearSelection()

        lstKeyChangePromise.ClearSelection()
        cboProjectManager.ClearSelection()
        lstStrategicObjective.ClearSelection()

    End Sub

    Public Function LoadProjects(ByVal Project As Long) As Boolean

        Try

            Dim objProjects As New BusinessLogic.Projects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objProjectKeyChangePromises As New BusinessLogic.ProjectKeyChangePromises(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objProjectStrategicObjectives As New BusinessLogic.ProjectStrategicObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProjects

                ClearSelection()

                If .Retrieve(Project) Then

                    txtProject.Text = .Project
                    txtProjectCode.Text = .ProjectCode
                    If Not IsNothing(cboProjectManager.Items.FindByValue(.ProjectManager)) Then cboProjectManager.SelectedValue = .ProjectManager
                    If Not IsNothing(cboProgram.Items.FindByValue(.ProgrameID)) Then cboProgram.SelectedValue = .ProgrameID
                    txtTargetedNoOfBeneficiaries.Text = .TargetedNoOfBeneficiaries
                    txtActualBeneficiaries.Text = .ActualBeneficiaries
                    If Not .StartDate = "" Then radStartDate.SelectedDate = .StartDate
                    If Not .FinalEvlDate = "" Then radFinalEvlDate.SelectedDate = .FinalEvlDate
                    If Not .EndDate = "" Then RadEndDate.SelectedDate = .EndDate
                    txtProjectBudget.Text = .ProjectBudget
                    txtName.Text = .Name
                    txtAcronym.Text = .Acronym
                    txtFinalGStatement.Text = .FinalGStatement
                    txtBenDescription.Text = .BenDescription
                    txtStakeholderDescription.Text = .StakeholderDescription

                    Dim dsProjectKCP As DataSet = objProjectKeyChangePromises.GetProjectKeyChangePromisesByProjectID(.Project)
                    Dim dsProjectSO As DataSet = objProjectStrategicObjectives.GetProjectStrategicObjectivesByProjectID(.Project)

                    If Not IsNothing(dsProjectKCP) AndAlso dsProjectKCP.Tables.Count > 0 AndAlso dsProjectKCP.Tables(0).Rows.Count > 0 Then

                        For Each i As ListItem In lstKeyChangePromise.Items

                            If dsProjectKCP.Tables(0).Select("KeyChangePromiseID = " & i.Value).Length > 0 Then

                                i.Selected = True

                            End If

                        Next

                    End If

                    If Not IsNothing(dsProjectSO) AndAlso dsProjectSO.Tables.Count > 0 AndAlso dsProjectSO.Tables(0).Rows.Count > 0 Then

                        For Each i As ListItem In lstStrategicObjective.Items

                            If dsProjectSO.Tables(0).Select("StrategicObjectiveID = " & i.Value).Length > 0 Then

                                i.Selected = True

                            End If

                        Next

                    End If

                    ShowMessage("Project loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Projects: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objProjects As New BusinessLogic.Projects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProjects

                .Project = IIf(txtProject.Text = "", 0, txtProject.Text)
                .ProjectCode = txtProjectCode.Text
                If cboProjectManager.SelectedIndex > 0 Then .ProjectManager = cboProjectManager.SelectedValue
                If cboProgram.SelectedIndex > 0 Then .ProgrameID = cboProgram.SelectedValue
                .TargetedNoOfBeneficiaries = IIf(IsNumeric(txtTargetedNoOfBeneficiaries.Text), txtTargetedNoOfBeneficiaries.Text, 0)
                .ActualBeneficiaries = IIf(IsNumeric(txtActualBeneficiaries.Text), txtActualBeneficiaries.Text, 0)
                If radStartDate.SelectedDate.HasValue Then .StartDate = radStartDate.SelectedDate
                If radFinalEvlDate.SelectedDate.HasValue Then .FinalEvlDate = radFinalEvlDate.SelectedDate
                If RadEndDate.SelectedDate.HasValue Then .EndDate = RadEndDate.SelectedDate
                .ProjectBudget = IIf(IsNumeric(txtProjectBudget.Text), txtProjectBudget.Text, 0)
                .Name = txtName.Text
                .Acronym = txtAcronym.Text
                .FinalGStatement = txtFinalGStatement.Text
                .BenDescription = txtBenDescription.Text
                .StakeholderDescription = txtStakeholderDescription.Text

                If .Save Then

                    Dim NumberOfNewFields As Integer = UpdateCustomFieldTemplates(.Project)

                    SaveKeyChangePromises(.Project)
                    SaveStrategicObjectives(.Project)

                    If (phCustomFields.Controls.Count > 0) Then
                        objCustomFields.SaveCustomFields(DirectCast(phCustomFields.Controls(0), RadPanelBar), .Project, "P")
                    End If

                    If Not IsNumeric(txtProject.Text) OrElse Trim(txtProject.Text) = 0 Then txtProject.Text = .Project
                    ShowMessage("Projects saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error saving data", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function UpdateCustomFieldTemplates(ByVal ProjectID As Long) As Integer

        'Creating a project: 
        '   - Now we know the type, and/or the status, so we need to 
        '     load the relevant custom fields
        'Updating a project: 
        '   - We check if the type/status has changed, and if so, we 
        '     need to load the relevant custom fields for this new 
        '     project type/status.

        Dim NewStatusTemplates As Long = 0, NewTypeTemplates As Long = 0

        If ProjectID > 0 Then

            Dim objCustomFields As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

            objCustomFields.UpdateObjectWithStatusTemplates(ProjectID, "P", ProjectID, BusinessLogic.CustomFields.AutomatorTypes.Project, RowsAffected:=NewStatusTemplates)

        End If

        Return NewStatusTemplates + NewTypeTemplates

    End Function

    Public Sub Clear()

        txtProject.Text = 0
        txtProjectCode.Text = 0
        txtTargetedNoOfBeneficiaries.Text = 0
        txtActualBeneficiaries.Text = 0
        radStartDate.Clear()
        radFinalEvlDate.Clear()
        RadEndDate.Clear()
        txtProjectBudget.Text = 0.0
        txtName.Text = ""
        lstKeyChangePromise.SelectedValue = 0
        txtAcronym.Text = ""
        txtFinalGStatement.Text = ""
        txtBenDescription.Text = ""
        txtStakeholderDescription.Text = ""
        cboProjectManager.SelectedValue = 0
        lstStrategicObjective.SelectedValue = 0
        cboProgram.SelectedValue = 0

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Response.Redirect("~/Projects.aspx")

    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click

        If Not SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.DeleteProjects) Then

            ShowMessage("You are not authorised to delete projects!", MessageTypeEnum.Error)

        Else

            Dim objProjects As New BusinessLogic.Projects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            If IsNumeric(txtProject.Text) Then

                With objProjects
                    .Project = txtProject.Text
                    If .Delete() Then
                        Clear()
                        ShowMessage("Project deleted successfully...", MessageTypeEnum.Information)
                    End If
                End With
            End If

        End If
    End Sub

    Private Function SaveKeyChangePromises(ByVal ProjectID As Long) As Boolean

        Try

            For Each i As ListItem In lstKeyChangePromise.Items

                Dim objKeyChangePromises As New BusinessLogic.ProjectKeyChangePromises(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objKeyChangePromises

                    .ProjectID = ProjectID
                    .KeyChangePromiseID = i.Value

                    If i.Selected = True And i.Value > 0 Then

                        If Not .CheckExistence() Then .Save()

                    Else

                        If .CheckExistence() Then

                            .DeleteEntry()

                        End If

                    End If

                  End With

            Next

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Private Function SaveStrategicObjectives(ByVal ProjectID As Long) As Boolean

        Try

            For Each i As ListItem In lstStrategicObjective.Items

                Dim objStrategicObjectives As New BusinessLogic.ProjectStrategicObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objStrategicObjectives

                    .ProjectID = ProjectID
                    .StrategicObjectiveID = i.Value

                    If i.Selected = True And i.Value > 0 Then

                        If Not .CheckExistence() Then .Save()

                    Else

                        If .CheckExistence() Then

                            .DeleteEntry()

                        End If

                    End If

                End With

            Next

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function
End Class
