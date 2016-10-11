Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class InterventionsDetailsControl
    Inherits System.Web.UI.UserControl

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    'Private Property lblError As Object

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

            LoadGrid()

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboProgram

                .DataSource = objLookup.Lookup("tblProgrammes", "ProgramID", "Name").Tables(0)
                .DataValueField = "ProgramID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboProject

                .DataSource = objLookup.Lookup("tblProjects", "Project", "Name").Tables(0)
                .DataValueField = "Project"
                .DataTextField = "Name"
                .DataBind()

            End With
        End If

    End Sub

    Private Sub LoadGrid()

        Dim objIntervention As New BusinessLogic.Interventions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "SELECT P.Name as Project, I.* FROM tblInterventions I inner JOIN tblProjects P on I.ProjectID = P.Project "

        With radInterventionListing

            .DataSource = objIntervention.GetInterventions(sql)
            .DataBind()

            ViewState("Interventions") = .DataSource

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadInterventions(ByVal InterventionID As Long) As Boolean

        Try

            Dim objInterventions As New Interventions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objInterventions

                If .Retrieve(InterventionID) Then


                    txtInterventionID.Text = .InterventionID
                    If Not IsNothing(cboProject.Items.FindByValue(.ProjectID)) Then cboProject.SelectedValue = .ProjectID
                    txtBeneficiariesTarget.Text = .BeneficiariesTarget
                    txtActualBenficiaries.Text = .ActualBenficiaries
                    radStartDate.SelectedDate = .StartDate
                    radEndDate.SelectedDate = .EndDate
                    txtName.Text = .Name
                    txtDescription.Text = .Description
                    txtDescriptionOfBeneficiaries.Text = .DescriptionOfBeneficiaries

                    ShowMessage("Interventions loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Interventions:", MessageTypeEnum.Error)
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

            Dim objInterventions As New Interventions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objInterventions

                .InterventionID = IIf(txtInterventionID.Text <> "", txtInterventionID.Text, 0)
                If cboProject.SelectedIndex > -1 Then .ProjectID = cboProject.SelectedValue
                .BeneficiariesTarget = IIf(IsNumeric(txtBeneficiariesTarget.Text), txtBeneficiariesTarget.Text, 0)
                .ActualBenficiaries = IIf(IsNumeric(txtActualBenficiaries.Text), txtActualBenficiaries.Text, 0)
                .StartDate = radStartDate.SelectedDate
                .EndDate = radEndDate.SelectedDate
                .Name = txtName.Text
                .Description = txtDescription.Text
                .DescriptionOfBeneficiaries = txtDescriptionOfBeneficiaries.Text

                If .Save Then

                    If Not IsNumeric(txtInterventionID.Text) OrElse Trim(txtInterventionID.Text) = 0 Then txtInterventionID.Text = .InterventionID

                    LoadInterventions(.InterventionID)
                    LoadGrid()
                    ShowMessage("Intervention saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save Intervention", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtInterventionID.Text = ""
        If Not IsNothing(cboProgram.Items.FindByValue("")) Then
            cboProgram.SelectedValue = ""
        ElseIf Not IsNothing(cboProgram.Items.FindByValue(0)) Then
            cboProgram.SelectedValue = 0
        Else
            cboProgram.SelectedIndex = -1
        End If
        If Not IsNothing(cboProject.Items.FindByValue("")) Then
            cboProgram.SelectedValue = ""
        ElseIf Not IsNothing(cboProject.Items.FindByValue(0)) Then
            cboProject.SelectedValue = 0
        Else
            cboProject.SelectedIndex = -1
        End If
        txtBeneficiariesTarget.Text = 0
        txtActualBenficiaries.Text = 0
        radStartDate.Clear()
        radEndDate.Clear()
        txtName.Text = ""
        txtDescription.Text = ""
        txtDescriptionOfBeneficiaries.Text = ""

    End Sub

    Private Sub cboProgram_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProgram.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProgram.SelectedIndex > 0 Then

            With cboProject

                .DataSource = objLookup.Lookup("tblProjects", "Project", "Name", , "Program = " & cboProgram.SelectedValue).Tables(0)
                .DataValueField = "Project"
                .DataTextField = "Name"
                .DataBind()

            End With

        End If

    End Sub

    Private Sub radInterventionListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radInterventionListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radInterventionListing.Items(index)

                Dim InterventionID As Long = Server.HtmlDecode(item("InterventionID").Text)

                LoadInterventions(InterventionID)

            End If

        End If

    End Sub

    Private Sub radInterventionListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radInterventionListing.NeedDataSource

        radInterventionListing.DataSource = DirectCast(ViewState("Interventions"), DataTable)

    End Sub
End Class
