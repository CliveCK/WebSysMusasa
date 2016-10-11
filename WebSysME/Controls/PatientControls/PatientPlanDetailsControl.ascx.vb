Imports BusinessLogic

Partial Class PatientPlanDetailsControl
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

            InitializeComponents()

                'LoadPatientPlan(CookiesWrapper.PatientID)
                LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub InitializeComponents()

        LoadCombo(cboNeedCategory, "luNeedCategory", "Description", "NeedCategoryID")
        LoadCombo(cboNeed, "luNeedServices", "Description", "NeedServiceID")
        LoadCombo(cboAction, "luNeedAction", "Description", "NeedActionID")

        Dim OBjSql As New Referrals(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim Sql As String = "select * from tblOrganization O inner join luOrganizationTypes OT on O.OrganizationTypeID = OT.OrganizationTypeID where OT.Description = 'Service Provider'"

        With cboServiceProvider

            .DataSource = OBjSql.GetReferrals(Sql)
            .DataValueField = "OrganizationID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Public Sub LoadCombo(ByVal cboCombo As DropDownList, ByVal Table As String, ByVal TextField As String, ByVal ValueField As String)

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboCombo

            .DataSource = objLookup.Lookup(Table, ValueField, TextField).Tables(0)
            .DataValueField = ValueField
            .DataTextField = TextField
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Public Function LoadPatientPlan(ByVal PatientPlanID As Long) As Boolean

        Try

            Dim objPatientPlan As New PatientPlan(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objPatientPlan

                If .Retrieve(PatientPlanID) Then

                    Dim NeedCategory As Long = objPatientPlan.GetPatientPlan("select * from luNeedServices WHERE NeedServiceID = " & .Need).Tables(0).Rows(0)("NeedCategoryID")

                    txtPatientPlanID.Text = .PatientPlanID
                    txtPatientID.Text = .PatientID
                    If Not IsNothing(cboNeedCategory.Items.FindByValue(NeedCategory)) Then cboNeedCategory.SelectedValue = NeedCategory
                    If Not IsNothing(cboNeed.Items.FindByValue(.Need)) Then cboNeed.SelectedValue = .Need
                    If Not IsNothing(cboAction.Items.FindByValue(.Action)) Then cboAction.SelectedValue = .Action
                    If Not IsNothing(cboServiceProvider.Items.FindByValue(.ServiceProvider)) Then cboServiceProvider.SelectedValue = .ServiceProvider
                    If Not .PlanDate = "" Then radPlanDate.SelectedDate = .PlanDate
                    If Not .ActualDate = "" Then radActualDate.SelectedDate = .ActualDate
                    txtCost.Text = .Cost
                    txtComments.Text = .Comments

                    ShowMessage("Patient Plan loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Patient Plan...", MessageTypeEnum.Error)
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

            Dim objPatientPlan As New PatientPlan(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            If IsNumeric(CookiesWrapper.PatientID) AndAlso CookiesWrapper.PatientID > 0 Then

                With objPatientPlan

                    .PatientPlanID = IIf(IsNumeric(txtPatientPlanID.Text), txtPatientPlanID.Text, 0)
                    .PatientID = CookiesWrapper.PatientID
                    If cboNeed.SelectedIndex > -1 Then .Need = cboNeed.SelectedValue
                    If cboAction.SelectedIndex > -1 Then .Action = cboAction.SelectedValue
                    If cboServiceProvider.SelectedIndex > -1 Then .ServiceProvider = cboServiceProvider.SelectedValue
                    If radPlanDate.SelectedDate.HasValue Then .PlanDate = radPlanDate.SelectedDate
                    If radActualDate.SelectedDate.HasValue Then .ActualDate = radActualDate.SelectedDate
                    .Cost = txtCost.Text
                    .Comments = txtComments.Text

                    If .Save Then

                        If Not IsNumeric(txtPatientPlanID.Text) OrElse Trim(txtPatientPlanID.Text) = 0 Then txtPatientPlanID.Text = .PatientPlanID
                        LoadPatientPlan(.PatientPlanID)
                        LoadGrid()
                        ShowMessage("Patient Plan saved successfully...", MessageTypeEnum.Information)

                        Return True

                    Else

                        ShowMessage("Patient Plan details saved successfully", MessageTypeEnum.Error)
                        Return False

                    End If

                End With

            Else

                ShowMessage("Save patient details first!", MessageTypeEnum.Error)

            End If

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub LoadGrid()

        Dim objPatientPlan As New BusinessLogic.PatientPlan(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radPatientListing

            .DataSource = objPatientPlan.GetPatientPlanDetails(CookiesWrapper.PatientID)
            .DataBind()

            ViewState("PatientPlan") = .DataSource

        End With

    End Sub

    Public Sub Clear()

        txtPatientPlanID.Text = ""
        If Not IsNothing(cboNeed.Items.FindByValue("")) Then
            cboNeed.SelectedValue = ""
        ElseIf Not IsNothing(cboNeed.Items.FindByValue(0)) Then
            cboNeed.SelectedValue = 0
        Else
            cboNeed.SelectedIndex = -1
        End If
        If Not IsNothing(cboAction.Items.FindByValue("")) Then
            cboAction.SelectedValue = ""
        ElseIf Not IsNothing(cboAction.Items.FindByValue(0)) Then
            cboAction.SelectedValue = 0
        Else
            cboAction.SelectedIndex = -1
        End If
        If Not IsNothing(cboServiceProvider.Items.FindByValue("")) Then
            cboServiceProvider.SelectedValue = ""
        ElseIf Not IsNothing(cboServiceProvider.Items.FindByValue(0)) Then
            cboServiceProvider.SelectedValue = 0
        Else
            cboServiceProvider.SelectedIndex = -1
        End If
        radPlanDate.Clear()
        radActualDate.Clear()
        txtCost.Text = 0.0
        txtComments.Text = ""

    End Sub

    Private Sub radPatientListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radPatientListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As Telerik.Web.UI.GridDataItem = radPatientListing.Items(index)
            Dim PatientPlanID As Integer

            PatientPlanID = Server.HtmlDecode(item("PatientPlanID").Text)

            LoadPatientPlan(PatientPlanID)

        End If

    End Sub

    Private Sub cboNeedCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNeedCategory.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboNeedCategory.SelectedIndex > 0 Then

            With cboNeed

                .DataSource = objLookup.Lookup("luNeedServices", "NeedServiceID", "Description", , "NeedCategoryID = " & cboNeedCategory.SelectedValue).Tables(0)
                .DataValueField = "NeedServiceID"
                .DataTextField = "Description"
                .DataBind()

            End With

        End If

    End Sub

    Private Sub cboNeed_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNeed.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboNeed.SelectedIndex > 0 Then

            With cboAction

                .DataSource = objLookup.Lookup("luNeedAction", "NeedActionID", "Description", , "NeedServiceID = " & cboNeed.SelectedValue).Tables(0)
                .DataValueField = "NeedActionID"
                .DataTextField = "Description"
                .DataBind()

            End With

        End If

    End Sub

    Private Sub radPatientListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radPatientListing.NeedDataSource

        radPatientListing.DataSource = DirectCast(ViewState("PatientPlan"), DataSet)

    End Sub
End Class

