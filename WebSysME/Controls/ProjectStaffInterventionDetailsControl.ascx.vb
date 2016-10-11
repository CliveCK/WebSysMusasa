Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic

Partial Class ProjectStaffInterventionDetailsControl
    Inherits System.Web.UI.UserControl

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

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

            With cboProgramme

                .DataSource = objLookup.Lookup("tblProgrammes", "ProgramID", "Name").Tables(0)
                .DataValueField = "ProgramID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboOrganization

                .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name").Tables(0)
                .DataValueField = "OrganizationID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadProjectStaffIntervention(ByVal ProjectStaffInterventionID As Long) As Boolean

        Try

            Dim objProjectStaffIntervention As New ProjectStaffIntervention(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProjectStaffIntervention

                If .Retrieve(ProjectStaffInterventionID) Then

                    txtProjectStaffInterventionID.Text = .ProjectStaffInterventionID
                    If Not IsNothing(cboOrganization.Items.FindByValue(.OrganizationID)) Then cboOrganization.SelectedValue = .OrganizationID
                    If Not IsNothing(cboStaff.Items.FindByValue(.StaffID)) Then cboStaff.SelectedValue = .StaffID
                    If Not IsNothing(cboIntervention.Items.FindByValue(.InterventionID)) Then cboIntervention.SelectedValue = .InterventionID

                    ShowMessage("ProjectStaffIntervention loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load ProjectStaffIntervention: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub LoadGrid(ByVal InterventionID As Long)

        Dim objStaff As New ProjectStaffIntervention(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radStaff

            .DataSource = objStaff.GetStaff(InterventionID).Tables(0)
            .DataBind()

        End With

    End Sub

    Public Function Save() As Boolean

        Try

            Dim objProjectStaffIntervention As New ProjectStaffIntervention(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProjectStaffIntervention

                .ProjectStaffInterventionID = IIf(IsNumeric(txtProjectStaffInterventionID.Text), txtProjectStaffInterventionID.Text, 0)
                If cboOrganization.SelectedIndex > -1 Then .OrganizationID = cboOrganization.SelectedValue
                If cboStaff.SelectedIndex > -1 Then .StaffID = cboStaff.SelectedValue
                If cboIntervention.SelectedIndex > -1 Then .InterventionID = cboIntervention.SelectedValue

                If .Save Then

                    If Not IsNumeric(txtProjectStaffInterventionID.Text) OrElse Trim(txtProjectStaffInterventionID.Text) = 0 Then txtProjectStaffInterventionID.Text = .ProjectStaffInterventionID
                    LoadGrid(.InterventionID)
                    ShowMessage("Mapping succeeded successfully...", MessageTypeEnum.Information)

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

        txtProjectStaffInterventionID.Text = ""
        If Not IsNothing(cboOrganization.Items.FindByValue("")) Then
            cboOrganization.SelectedValue = ""
        ElseIf Not IsNothing(cboOrganization.Items.FindByValue(0)) Then
            cboOrganization.SelectedValue = 0
        Else
            cboOrganization.SelectedIndex = -1
        End If
        If Not IsNothing(cboStaff.Items.FindByValue("")) Then
            cboStaff.SelectedValue = ""
        ElseIf Not IsNothing(cboStaff.Items.FindByValue(0)) Then
            cboStaff.SelectedValue = 0
        Else
            cboStaff.SelectedIndex = -1
        End If
        If Not IsNothing(cboIntervention.Items.FindByValue("")) Then
            cboIntervention.SelectedValue = ""
        ElseIf Not IsNothing(cboIntervention.Items.FindByValue(0)) Then
            cboIntervention.SelectedValue = 0
        Else
            cboIntervention.SelectedIndex = -1
        End If

    End Sub

    Private Sub cboOrganization_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrganization.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboOrganization.SelectedIndex > 0 Then

            With cboProgramme

                .DataSource = db.ExecuteDataSet("SELECT StaffID, ISNULL(FirstName, '') + ' '  + ISNULL(Surname,'') AS Name FROM tblStaffMembers where OrganizationID = " & cboOrganization.SelectedValue).Tables(0)
                .DataValueField = "StaffID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboProgramme_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProgramme.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProgramme.SelectedIndex > 0 Then

            With cboProject

                .DataSource = objLookup.Lookup("tblProjects", "Project", "Name", , "Program = " & cboProgramme.SelectedValue)
                .DataValueField = "StaffID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProject.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProject.SelectedIndex > 0 Then

            With cboProject

                .DataSource = objLookup.Lookup("tblInterventions", "InterventionID", "Name", , "ProjectID = " & cboProject.SelectedValue)
                .DataValueField = "InterventionID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboIntervention_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboIntervention.SelectedIndexChanged

        If cboIntervention.SelectedIndex > 0 Then

            LoadGrid(cboIntervention.SelectedValue)

        End If

    End Sub
End Class

