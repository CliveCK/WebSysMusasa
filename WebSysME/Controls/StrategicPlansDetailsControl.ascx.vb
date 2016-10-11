Imports BusinessLogic

Partial Class StrategicPlansDetailsControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

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

            With cboOrganization

                .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name").Tables(0)
                .DataValueField = "OrganizationID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboFromYear

                .DataSource = objLookup.Lookup("luYear", "YearID", "Description").Tables(0)
                .DataValueField = "YearID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboToYear

                .DataSource = objLookup.Lookup("luYear", "YearID", "Description").Tables(0)
                .DataValueField = "YearID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadStrategicPlans(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadStrategicPlans(ByVal StrategicPlanID As Long) As Boolean

        Try

            Dim objStrategicPlans As New StrategicPlans(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objStrategicPlans

                If .Retrieve(StrategicPlanID) Then

                    txtStrategicPlanID.Text = .StrategicPlanID
                    If Not IsNothing(cboOrganization.Items.FindByValue(.OrganizationID)) Then cboOrganization.SelectedValue = .OrganizationID
                    If Not IsNothing(cboFromYear.Items.FindByValue(.FromYear)) Then cboFromYear.SelectedValue = .FromYear
                    If Not IsNothing(cboToYear.Items.FindByValue(.ToYear)) Then cboToYear.SelectedValue = .ToYear
                    txtPlan.Text = .PlanID
                    txtName.Text = .Name
                    txtSummary.Text = .Summary

                    ShowMessage("StrategicPlans loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadStrategicPlans: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objStrategicPlans As New StrategicPlans(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objStrategicPlans

                .StrategicPlanID = IIf(IsNumeric(txtStrategicPlanID.Text), txtStrategicPlanID.Text, 0)
                If cboOrganization.SelectedIndex > -1 Then .OrganizationID = cboOrganization.SelectedValue
                If cboFromYear.SelectedIndex > -1 Then .FromYear = cboFromYear.SelectedValue
                If cboToYear.SelectedIndex > -1 Then .ToYear = cboToYear.SelectedValue
                .PlanID = txtPlan.Text
                .Name = txtName.Text
                .Summary = txtSummary.Text

                If .Save Then

                    If Not IsNumeric(txtStrategicPlanID.Text) OrElse Trim(txtStrategicPlanID.Text) = 0 Then txtStrategicPlanID.Text = .StrategicPlanID
                    ShowMessage("StrategicPlans saved successfully...", MessageTypeEnum.Information)

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

        txtStrategicPlanID.Text = ""
        If Not IsNothing(cboFromYear.Items.FindByValue("")) Then
            cboFromYear.SelectedValue = ""
        ElseIf Not IsNothing(cboFromYear.Items.FindByValue(0)) Then
            cboFromYear.SelectedValue = 0
        Else
            cboFromYear.SelectedIndex = -1
        End If
        If Not IsNothing(cboToYear.Items.FindByValue("")) Then
            cboToYear.SelectedValue = ""
        ElseIf Not IsNothing(cboToYear.Items.FindByValue(0)) Then
            cboToYear.SelectedValue = 0
        Else
            cboFromYear.SelectedIndex = -1
        End If
        If Not IsNothing(cboOrganization.Items.FindByValue("")) Then
            cboOrganization.SelectedValue = ""
        ElseIf Not IsNothing(cboOrganization.Items.FindByValue(0)) Then
            cboOrganization.SelectedValue = 0
        Else
            cboOrganization.SelectedIndex = -1
        End If
        txtPlan.Text = ""
        txtName.Text = ""
        txtSummary.Text = ""

    End Sub

    Private Sub lnkMilestone_Click(sender As Object, e As EventArgs) Handles lnkMilestone.Click

        If IsNumeric(txtStrategicPlanID.Text) Then

            Response.Redirect("~/StrategicPlanMilestones.aspx?id=" & objUrlEncoder.Encrypt(txtStrategicPlanID.Text))

        Else

            ShowMessage("Please save details first!", MessageTypeEnum.Error)

        End If

    End Sub
End Class

