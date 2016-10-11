Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class IndiactorDetailsControl
    Inherits System.Web.UI.UserControl

    Protected ProjectID As Integer = 0
    Protected IndicatorID As Integer = 0
    Protected db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

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

            InitializeComponents()

            'radIndicatorTargets.EditIndexes.Add(0)

            If Not IsNothing(Request.QueryString("id")) Then 'pid = ProjectID, Inid = IndicatorID

                ProjectID = objUrlEncoder.Decrypt(Request.QueryString("id"))

                If ProjectID <> 0 Then

                    Dim sql As String = "SELECT o.ObjectiveID, o.Description FROM tblObjectives o INNER JOIN [dbo].[tblProjectObjectives] oa on o.ObjectiveID = oa.ObjectiveID "
                    sql &= "INNER JOIN tblProjects obj on obj.Project  = oa.ProjectID WHERE obj.Project = " & ProjectID

                    With cboObjective

                        .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                        .DataValueField = "ObjectiveID"
                        .DataTextField = "Description"
                        .DataBind()

                    End With

                End If

            ElseIf Not IsNothing(Request.QueryString("Inid")) Then

                IndicatorID = objUrlEncoder.Decrypt(Request.QueryString("Inid"))

                Dim sql As String = "SELECT * FROM tblObjectives WHERE ObjectiveID in (SELECT ObjectiveID FROM tblIndicators WHERE IndicatorID =" & IndicatorID & ")"

                With cboObjective

                    .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                    .DataValueField = "ObjectiveID"
                    .DataTextField = "Description"
                    .DataBind()

                    .SelectedIndex = -1

                End With

                LoadIndiactor(IndicatorID)
                LoadGrid()
                pnlIndicatorTracking.Visible = True

            Else

                Dim sql As String = "SELECT * FROM tblObjectives"

                With cboObjective

                    .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                    .DataValueField = "ObjectiveID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

            End If

            LoadComponents()

        End If

    End Sub

    Private Sub LoadGrid()

        Dim sql As String = "SELECT I.*, O.Name as Organization, D.Name as District FROM tblIndicatorTracking I left outer join tblOrganization O on I.OrganizationID = O.OrganizationID "
        sql &= "left outer join tblDistricts D on D.DistrictID = I.DistrictID WHERE IndicatorID = " & IndicatorID

        With radIndicatorTargets

            .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
            .DataBind()

            ViewState("Tracking") = .DataSource

        End With

    End Sub

    Private Sub InitializeComponents()

        LoadCombo(cboIndicatorType, "luIndicatorType", "Description", "IndicatorTypeID")
        LoadCombo(cboTool, "luTools", "Description", "ToolID")
        LoadCombo(cboDataCollectionFrequency, "luDataCollectionFrequency", "Description", "DataCollectionFrequencyID")
        LoadCombo(cboDataSource, "luDataSource", "Description", "DataSourceID")
        LoadCombo(cboUnitOfMeasurement, "luUnitOfMeasurement", "Description", "UnitOfMeasurementID")
        LoadCombo(cboMonth, "luMonths", "Description", "MonthID")
        LoadCombo(cboOrganization, "tblOrganization", "Name", "OrganizationID")
        LoadCombo(cboDistrict, "tblDistricts", "Name", "DistrictID")
        LoadCombo(cboImpact, "luImpact", "Description", "ImpactID")

    End Sub

    Public Sub LoadCombo(ByVal cboCombo As DropDownList, ByVal Table As String, ByVal TextField As String, ByVal ValueField As String)

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboCombo

            .DataSource = objLookup.Lookup(Table, ValueField, TextField).Tables(0)
            .DataValueField = ValueField
            .DataTextField = TextField
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadIndiactor(ByVal IndicatorID As Long) As Boolean

        Try

            Dim objIndiactor As New Indiactor(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objIndiactor

                If .Retrieve(IndicatorID) Then

                    txtIndicatorID.Text = .IndicatorID
                    If Not IsNothing(cboIndicatorType.Items.FindByValue(.IndicatorType)) Then cboIndicatorType.SelectedValue = .IndicatorType
                    If Not IsNothing(cboOutput.Items.FindByValue(.OutputID)) Then cboOutput.SelectedValue = .OutputID
                    If Not IsNothing(cboOutcome.Items.FindByValue(.OutcomeID)) Then cboOutcome.SelectedValue = .OutcomeID
                    If Not IsNothing(cboImpact.Items.FindByValue(.ImpactID)) Then cboImpact.SelectedValue = .ImpactID
                    If Not IsNothing(cboObjective.Items.FindByValue(.ObjectiveID)) Then cboObjective.SelectedValue = .ObjectiveID
                    If Not IsNothing(cboActivity.Items.FindByValue(.ActivityID)) Then cboActivity.SelectedValue = .ActivityID
                    If Not IsNothing(cboUnitOfMeasurement.Items.FindByValue(.UnitOfMeasurement)) Then cboUnitOfMeasurement.SelectedValue = .UnitOfMeasurement
                    txtBaselineValue.Text = .BaselineValue
                    If Not IsNothing(cboDataSource.Items.FindByValue(.DataSource)) Then cboDataSource.SelectedValue = .DataSource
                    If Not IsNothing(cboTool.Items.FindByValue(.Tool)) Then cboTool.SelectedValue = .Tool
                    If Not IsNothing(cboDataCollectionFrequency.Items.FindByValue(.DataCollectionFrequency)) Then cboDataCollectionFrequency.SelectedValue = .DataCollectionFrequency
                    txtName.Text = .Name
                    txtDefinition.Text = .Definition
                    txtDescription.Text = .Description
                    txtDataCollectionMethod.Text = .DataCollectionMethod
                    txtResponsibleParty.Text = .ResponsibleParty
                    txtProgramTargetValue.Text = .ProgramTargetValue

                    CheckIndicatorType(cboIndicatorType)

                    ShowMessage("Indicator loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Indicator: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Function CheckIndicatorType(ByVal cboIndicatorType As DropDownList) As Boolean

        Select Case cboIndicatorType.SelectedItem.Text

            Case "Output"
                cboOutput.Enabled = True
                cboOutcome.Enabled = False
                cboActivity.Enabled = False

            Case "Outcome"
                cboOutput.Enabled = False
                cboOutcome.Enabled = True
                cboActivity.Enabled = False

            Case "Activity"
                cboOutput.Enabled = True
                cboOutcome.Enabled = True
                cboActivity.Enabled = True

        End Select

    End Function

    Public Function Save() As Boolean

        Try

            Dim objIndiactor As New Indiactor(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objIndiactor

                .IndicatorID = IIf(IsNumeric(txtIndicatorID.Text), txtIndicatorID.Text, 0)
                If cboIndicatorType.SelectedIndex > -1 Then .IndicatorType = cboIndicatorType.SelectedValue
                If cboImpact.SelectedIndex > -1 Then .ImpactID = cboImpact.SelectedValue
                If cboObjective.SelectedIndex > -1 Then .ObjectiveID = cboObjective.SelectedValue
                If cboOutput.SelectedIndex > -1 AndAlso cboOutput.Enabled Then .OutputID = cboOutput.SelectedValue
                If cboOutcome.SelectedIndex > -1 AndAlso cboOutcome.Enabled Then .OutcomeID = cboOutcome.SelectedValue
                If cboActivity.SelectedIndex > -1 AndAlso cboActivity.Enabled Then .ActivityID = cboActivity.SelectedValue
                If cboUnitOfMeasurement.SelectedIndex > -1 Then .UnitOfMeasurement = cboUnitOfMeasurement.SelectedValue
                If IsNumeric(txtBaselineValue.Text) Then .BaselineValue = txtBaselineValue.Text
                If cboDataSource.SelectedIndex > -1 Then .DataSource = cboDataSource.SelectedValue
                If cboTool.SelectedIndex > -1 Then .Tool = cboTool.SelectedValue
                If cboDataCollectionFrequency.SelectedIndex > -1 Then .DataCollectionFrequency = cboDataCollectionFrequency.SelectedValue
                .Name = txtName.Text
                .Definition = txtDefinition.Text
                .Description = txtDescription.Text
                .DataCollectionMethod = txtDataCollectionMethod.Text
                .ResponsibleParty = txtResponsibleParty.Text
                .ProgramTargetValue = txtProgramTargetValue.Text

                If .Save Then

                    If Not IsNumeric(txtIndicatorID.Text) OrElse Trim(txtIndicatorID.Text) = 0 Then txtIndicatorID.Text = .IndicatorID
                    LoadIndiactor(.IndicatorID)
                    ShowMessage("Indiactor saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error: Failed to save indicator...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtIndicatorID.Text = ""
        If Not IsNothing(cboDataCollectionFrequency.Items.FindByValue("")) Then
            cboDataCollectionFrequency.SelectedValue = ""
        ElseIf Not IsNothing(cboDataCollectionFrequency.Items.FindByValue(0)) Then
            cboDataCollectionFrequency.SelectedValue = 0
        Else
            cboDataCollectionFrequency.SelectedIndex = -1
        End If
        If Not IsNothing(cboIndicatorType.Items.FindByValue("")) Then
            cboIndicatorType.SelectedValue = ""
        ElseIf Not IsNothing(cboIndicatorType.Items.FindByValue(0)) Then
            cboIndicatorType.SelectedValue = 0
        Else
            cboIndicatorType.SelectedIndex = -1
        End If
        If Not IsNothing(cboOutput.Items.FindByValue("")) Then
            cboOutput.SelectedValue = ""
        ElseIf Not IsNothing(cboOutput.Items.FindByValue(0)) Then
            cboOutput.SelectedValue = 0
        Else
            cboOutput.SelectedIndex = -1
        End If
        If Not IsNothing(cboOutcome.Items.FindByValue("")) Then
            cboOutcome.SelectedValue = ""
        ElseIf Not IsNothing(cboOutcome.Items.FindByValue(0)) Then
            cboOutcome.SelectedValue = 0
        Else
            cboOutcome.SelectedIndex = -1
        End If
        If Not IsNothing(cboActivity.Items.FindByValue("")) Then
            cboActivity.SelectedValue = ""
        ElseIf Not IsNothing(cboActivity.Items.FindByValue(0)) Then
            cboActivity.SelectedValue = 0
        Else
            cboActivity.SelectedIndex = -1
        End If
        If Not IsNothing(cboUnitOfMeasurement.Items.FindByValue("")) Then
            cboUnitOfMeasurement.SelectedValue = ""
        ElseIf Not IsNothing(cboUnitOfMeasurement.Items.FindByValue(0)) Then
            cboUnitOfMeasurement.SelectedValue = 0
        Else
            cboUnitOfMeasurement.SelectedIndex = -1
        End If
        txtBaselineValue.Text = 0
        If Not IsNothing(cboDataSource.Items.FindByValue("")) Then
            cboDataSource.SelectedValue = ""
        ElseIf Not IsNothing(cboDataSource.Items.FindByValue(0)) Then
            cboDataSource.SelectedValue = 0
        Else
            cboDataSource.SelectedIndex = -1
        End If
        If Not IsNothing(cboTool.Items.FindByValue("")) Then
            cboTool.SelectedValue = ""
        ElseIf Not IsNothing(cboTool.Items.FindByValue(0)) Then
            cboTool.SelectedValue = 0
        Else
            cboTool.SelectedIndex = -1
        End If
        If Not IsNothing(cboImpact.Items.FindByValue("")) Then
            cboImpact.SelectedValue = ""
        ElseIf Not IsNothing(cboImpact.Items.FindByValue(0)) Then
            cboImpact.SelectedValue = 0
        Else
            cboImpact.SelectedIndex = -1
        End If
        If Not IsNothing(cboObjective.Items.FindByValue("")) Then
            cboObjective.SelectedValue = ""
        ElseIf Not IsNothing(cboObjective.Items.FindByValue(0)) Then
            cboObjective.SelectedValue = 0
        Else
            cboObjective.SelectedIndex = -1
        End If
        If Not IsNothing(cboDataCollectionFrequency.Items.FindByValue("")) Then
            cboDataCollectionFrequency.SelectedValue = ""
        ElseIf Not IsNothing(cboDataCollectionFrequency.Items.FindByValue(0)) Then
            cboDataCollectionFrequency.SelectedValue = 0
        Else
            cboDataCollectionFrequency.SelectedIndex = -1
        End If
        txtName.Text = ""
        txtDefinition.Text = ""
        txtDescription.Text = ""
        txtDataCollectionMethod.Text = ""
        txtResponsibleParty.Text = ""
        txtProgramTargetValue.Text = ""

    End Sub

    Private Sub LoadComponents()

        Dim sql As String = ""

        If cboObjective.SelectedValue <> "" Then

            If cboObjective.SelectedValue > 0 Then

                sql = "SELECT o.OutputID, o.Description FROM tblOutput o INNER JOIN tblObjectiveOutputs oo on o.OutputID = oo.OutputID "
                sql &= "INNER JOIN tblObjectives obj on obj.ObjectiveID = oo.ObjectiveID where obj.ObjectiveID = " & cboObjective.SelectedValue

                With cboOutput

                    .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                    .DataValueField = "OutputID"
                    .DataTextField = "Description"
                    .DataBind()

                End With

                sql = ""
                sql = "SELECT o.OutcomeID, o.Description FROM tblOutcomes o INNER JOIN [dbo].[tblObjectiveOutcomes] oa on o.OutcomeID = oa.OutcomeID "
                sql &= "INNER JOIN tblObjectives obj on obj.ObjectiveID  = oa.ObjectiveID where obj.ObjectiveID = " & cboObjective.SelectedValue

                With cboOutcome

                    .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                    .DataValueField = "OutcomeID"
                    .DataTextField = "Description"
                    .DataBind()

                End With

            End If

        End If

        If cboOutput.SelectedValue <> "" Then
            If cboOutput.SelectedValue > 0 Then

                sql = "SELECT A.ActivityID, A.Description FROM tblOutput o INNER JOIN tblOutputActivities oa on o.OutputID = oa.OutputID "
                sql &= "INNER JOIN tblActivities A on A.ActivityID = oa.ActivityID where o.OutputID = " & cboOutput.SelectedValue

                With cboActivity

                    .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                    .DataValueField = "ActivityID"
                    .DataTextField = "Description"
                    .DataBind()

                End With

            End If

        End If

    End Sub

    Private Sub cboObjective_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboObjective.SelectedIndexChanged

        LoadComponents()

    End Sub

    Private Sub cboOutput_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOutput.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions
        Dim sql As String = ""

        If cboOutput.SelectedValue > 0 Then

            sql = "SELECT A.ActivityID, A.Description FROM tblOutput o INNER JOIN tblOutputActivities oa on o.OutputID = oa.OutputID "
            sql &= "INNER JOIN tblActivities A on A.ActivityID = oa.ActivityID where o.OutputID = " & cboOutput.SelectedValue

            With cboActivity

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataValueField = "ActivityID"
                .DataTextField = "Description"
                .DataBind()

            End With

        End If

    End Sub

    Private Sub radIndicatorTargets_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radIndicatorTargets.NeedDataSource

        radIndicatorTargets.DataSource = DirectCast(ViewState("Tracking"), DataTable)

    End Sub

    Private Sub radIndicatorTargets_UpdateCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radIndicatorTargets.UpdateCommand

        Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)

        'Locate the changed row in the DataSource
        Dim changedRows() As DataRow = DirectCast(ViewState("Tracking"), DataTable).Select("IndicatorTrackingID = " & editedItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("IndicatorTrackingID"))

        'Update new values
        Dim newValues As Hashtable = New Hashtable
        'The GridTableView will fill the values from all editable columns in the hash
        e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
        changedRows(0).BeginEdit()

        Dim AchievementText As RadNumericTextBox = DirectCast(editedItem.FindControl("achieveNum"), RadNumericTextBox)
        Dim CommentsText As RadTextBox = DirectCast(editedItem.FindControl("commentsTextBox"), RadTextBox)

        Try

            Dim objTracking As New Indiactor(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTracking

                .Achievement = AchievementText.Text
                .Comment = CommentsText.Text

                .SaveTracking(editedItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("IndicatorTrackingID"))

            End With


            For Each entry As DictionaryEntry In newValues

                changedRows(0)(CType(entry.Key, String)) = entry.Value

            Next

            changedRows(0).EndEdit()

        Catch ex As Exception

            changedRows(0).CancelEdit()
            radIndicatorTargets.Controls.Add(New LiteralControl("Unable to update Indicator Tracking. Reason: " & ex.Message))
            e.Canceled = True

        End Try

    End Sub

    Private Sub cmdSaveTracking_Click(sender As Object, e As EventArgs) Handles cmdSaveTracking.Click

        Dim objIndicators As New Indiactor(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objIndicators

            .IndicatorID = txtIndicatorID.Text
            .Year = IIf(IsNumeric(txtYear.Text), txtYear.Text, 0)
            .Month = cboMonth.SelectedValue
            .Target = IIf(IsNumeric(txtTarget.Text), txtTarget.Text, 0)
            If cboOrganization.SelectedIndex > -1 Then .OrganizationID = cboOrganization.SelectedValue
            If cboDistrict.SelectedIndex > -1 Then .DistrictID = cboDistrict.SelectedValue

            If .SaveTracking(0) Then

                IndicatorID = .IndicatorID
                LoadGrid()
                ShowMessage("Indicator tracking saved successfully...", MessageTypeEnum.Information)

            Else

                ShowMessage("Error while saving Indicator tracking...", MessageTypeEnum.Error)

            End If

        End With

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Response.Redirect("~/Indicators.aspx")

    End Sub

    Private Sub cboIndicatorType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboIndicatorType.SelectedIndexChanged

        Select Case cboIndicatorType.SelectedItem.Text

            Case "Output"
                cboOutput.Enabled = True
                cboOutcome.Enabled = False
                cboActivity.Enabled = False

            Case "Outcome"
                cboOutput.Enabled = False
                cboOutcome.Enabled = True
                cboActivity.Enabled = False

            Case "Activity"
                cboOutput.Enabled = True
                cboOutcome.Enabled = True
                cboActivity.Enabled = True

            Case "Impact"
                cboOutput.Enabled = False
                cboOutcome.Enabled = False
                cboActivity.Enabled = False


        End Select

    End Sub

    Private Sub cboImpact_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboImpact.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions
        Dim sql As String = ""

        If cboOutput.SelectedValue > 0 Then

            sql = "SELECT A.ObjectiveID, A.Description FROM tblObjectives o INNER JOIN tblImpactObjectives oa on o.ObjectiveID = oa.ObjectiveID "
            sql &= "INNER JOIN luImpact A on A.ImpactID = oa.ImpactID where o.ImpactID = " & cboImpact.SelectedValue

            With cboObjective

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataValueField = "ObjectiveID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub
End Class

