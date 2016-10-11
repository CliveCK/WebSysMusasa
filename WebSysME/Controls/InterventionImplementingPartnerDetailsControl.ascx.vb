Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic

Partial Class InterventionImplementingPartnerDetailsControl
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

           

        End If

    End Sub

    Private Sub InitializeObjectsI()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboProvince

            .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name").Tables(0)
            .DataValueField = "ProvinceID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboCity

            .DataSource = objLookup.Lookup("tblCities", "CityID", "Name").Tables(0)
            .DataValueField = "CityID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboCity

            .DataSource = objLookup.Lookup("tblProgrammes", "ProgramID", "Name").Tables(0)
            .DataValueField = "ProgramID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboImplementingPartner

            .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name").Tables(0)
            .DataValueField = "Organization"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadInterventionImplementingPartner(ByVal InterventionImplementingPartner As Long) As Boolean

        Try

            Dim objInterventionImplementingPartner As New InterventionImplementingPartner(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objInterventionImplementingPartner

                If .Retrieve(InterventionImplementingPartner) Then

                    txtInterventionImplementingPartnerID.Text = .InterventionImplementingPartner
                    If Not IsNothing(cboIntervention.Items.FindByValue(.InterventionID)) Then cboIntervention.SelectedValue = .InterventionID
                    If Not IsNothing(cboImplementingPartner.Items.FindByValue(.ImplementingPartnerID)) Then cboImplementingPartner.SelectedValue = .ImplementingPartnerID
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    If Not IsNothing(cboCity.Items.FindByValue(.CityID)) Then cboCity.SelectedValue = .CityID
                    chkIsUrban.Checked = .IsUrban

                    ShowMessage("Intervention Implementing Partner loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Intervention Implementing Partner: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objInterventionImplementingPartner As New InterventionImplementingPartner(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objInterventionImplementingPartner

                .InterventionImplementingPartner = txtInterventionImplementingPartnerID.Text
                If cboIntervention.SelectedIndex > -1 Then .InterventionID = cboIntervention.SelectedValue
                If cboImplementingPartner.SelectedIndex > -1 Then .ImplementingPartnerID = cboImplementingPartner.SelectedValue
                If cboDistrict.SelectedIndex > -1 Then .DistrictID = cboDistrict.SelectedValue
                If cboCity.SelectedIndex > -1 Then .CityID = cboCity.SelectedValue
                .IsUrban = chkIsUrban.Checked

                If .Save Then

                    If Not IsNumeric(txtInterventionImplementingPartnerID.Text) OrElse Trim(txtInterventionImplementingPartnerID.Text) = 0 Then txtInterventionImplementingPartnerID.Text = .InterventionImplementingPartner
                    ShowMessage("Details saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error saving details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtInterventionImplementingPartnerID.Text = 0
        If Not IsNothing(cboIntervention.Items.FindByValue("")) Then
            cboIntervention.SelectedValue = ""
        ElseIf Not IsNothing(cboIntervention.Items.FindByValue(0)) Then
            cboIntervention.SelectedValue = 0
        Else
            cboIntervention.SelectedIndex = -1
        End If
        If Not IsNothing(cboImplementingPartner.Items.FindByValue("")) Then
            cboImplementingPartner.SelectedValue = ""
        ElseIf Not IsNothing(cboImplementingPartner.Items.FindByValue(0)) Then
            cboImplementingPartner.SelectedValue = 0
        Else
            cboImplementingPartner.SelectedIndex = -1
        End If
        If Not IsNothing(cboDistrict.Items.FindByValue("")) Then
            cboDistrict.SelectedValue = ""
        ElseIf Not IsNothing(cboDistrict.Items.FindByValue(0)) Then
            cboDistrict.SelectedValue = 0
        Else
            cboDistrict.SelectedIndex = -1
        End If
        If Not IsNothing(cboCity.Items.FindByValue("")) Then
            cboCity.SelectedValue = ""
        ElseIf Not IsNothing(cboCity.Items.FindByValue(0)) Then
            cboCity.SelectedValue = 0
        Else
            cboCity.SelectedIndex = -1
        End If
        chkIsUrban.Checked = False

    End Sub

    Private Sub cboProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProvince.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProvince.SelectedIndex > 0 Then

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name", , "ProvinceID = " & cboProvince.SelectedValue).Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

            End With

        End If

    End Sub

    Private Sub cboProgramme_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProgramme.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProject.SelectedIndex > 0 Then

            With cboCity

                .DataSource = objLookup.Lookup("tblProjects", "Project", "Name", , "Program = " & cboProgramme.SelectedValue).Tables(0)
                .DataValueField = "Project"
                .DataTextField = "Name"
                .DataBind()

            End With

        End If

    End Sub

    Private Sub cboProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProject.SelectedIndexChanged

        If cboProject.SelectedIndex > 0 Then

            Dim sql As String = "SELECT I.* FROM tblProjects P inner join tblProjectIntervention PI on I.Project = PI.ProjectID inner join tblInterventions I on I.InterventionID "
            sql &= " = PI.InterventionID WHERE PI.ProjectID = " & cboProject.SelectedValue

            With cboCity

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataValueField = "InterventionID"
                .DataTextField = "Name"
                .DataBind()

            End With

        End If

    End Sub
End Class

