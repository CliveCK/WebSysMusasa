Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class BudgetsDetailsControl
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

            With cboProjects

                .DataSource = objLookup.Lookup("tblProjects", "Project", "Name")
                .DataTextField = "Name"
                .DataValueField = "Project"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboActivityCategory

                .DataSource = objLookup.Lookup("tblActivityCategory", "ActivityCategoryID", "Description")
                .DataTextField = "Description"
                .DataValueField = "ActivityCategoryID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboActivity

                .DataSource = objLookup.Lookup("tblActivities", "ActivityID", "Description")
                .DataTextField = "Description"
                .DataValueField = "ActivityID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboUnit

                .DataSource = objLookup.Lookup("luCommodities", "CommodityID", "Description")
                .DataTextField = "Description"
                .DataValueField = "CommodityID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                If Not IsNothing(cboProjects.Items.FindByValue(objUrlEncoder.Decrypt(Request.QueryString("id")))) Then cboProjects.SelectedValue = objUrlEncoder.Decrypt(Request.QueryString("id"))

                If cboProjects.SelectedIndex > 0 Then

                    LoadGrid(objUrlEncoder.Decrypt(Request.QueryString("id")))

                End If

            End If

        End If

    End Sub

    Private Sub LoadGrid(ByVal ProjectID As Long)

        Dim objBudgets As New Budgets(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radBudgets

            .DataSource = objBudgets.GetBudgetsByProject(ProjectID)
            .DataBind()

            ViewState("Budgets") = .DataSource

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadBudgets(ByVal BudgetID As Long) As Boolean

        Try

            Dim objBudgets As New Budgets(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBudgets

                If .Retrieve(BudgetID) Then

                    txtBudgetID.Text = .BudgetID
                    If Not IsNothing(cboProjects.Items.FindByValue(.ProjectID)) Then cboProjects.SelectedValue = .ProjectID
                    If Not IsNothing(cboActivityCategory.Items.FindByValue(.ActivityCategoryID)) Then cboActivityCategory.SelectedValue = .ActivityCategoryID
                    If Not IsNothing(cboActivity.Items.FindByValue(.ActivityID)) Then cboActivity.SelectedValue = .ActivityID
                    If Not IsNothing(cboUnit.Items.FindByValue(.UnitID)) Then cboUnit.SelectedValue = .UnitID
                    txtUnitCost.Text = .UnitCost
                    txtNumberOfUnits.Text = .NumberOfUnits

                    ShowMessage("Budgets loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Budgets", MessageTypeEnum.Error)
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

            Dim objBudgets As New Budgets(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBudgets

                .BudgetID = IIf(IsNumeric(txtBudgetID.Text), txtBudgetID.Text, 0)
                If cboProjects.SelectedIndex > 0 Then .ProjectID = cboProjects.SelectedValue
                If cboActivityCategory.SelectedIndex > 0 Then .ActivityCategoryID = cboActivityCategory.SelectedValue
                If cboActivity.SelectedIndex > 0 Then .ActivityID = cboActivity.SelectedValue
                If cboUnit.SelectedIndex > 0 Then .UnitID = cboUnit.SelectedValue
                .UnitCost = txtUnitCost.Text
                .NumberOfUnits = txtNumberOfUnits.Text

                If .Save Then

                    If Not IsNumeric(txtBudgetID.Text) OrElse Trim(txtBudgetID.Text) = 0 Then txtBudgetID.Text = .BudgetID
                    LoadGrid(.ProjectID)
                    ShowMessage("Budgets saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("An error occured during save...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtBudgetID.Text = ""
        'If Not IsNothing(cboProjects.Items.FindByValue("")) Then
        '    cboProjects.SelectedValue = ""
        'ElseIf Not IsNothing(cboProjects.Items.FindByValue(0)) Then
        '    cboProjects.SelectedValue = 0
        'Else
        '    cboProjects.SelectedIndex = 0
        'End If
        If Not IsNothing(cboActivityCategory.Items.FindByValue("")) Then
            cboActivityCategory.SelectedValue = ""
        ElseIf Not IsNothing(cboActivityCategory.Items.FindByValue(0)) Then
            cboActivityCategory.SelectedValue = 0
        Else
            cboActivityCategory.SelectedIndex = 0
        End If
        If Not IsNothing(cboActivity.Items.FindByValue("")) Then
            cboActivity.SelectedValue = ""
        ElseIf Not IsNothing(cboActivity.Items.FindByValue(0)) Then
            cboActivity.SelectedValue = 0
        Else
            cboActivity.SelectedIndex = 0
        End If
        If Not IsNothing(cboUnit.Items.FindByValue("")) Then
            cboUnit.SelectedValue = ""
        ElseIf Not IsNothing(cboUnit.Items.FindByValue(0)) Then
            cboUnit.SelectedValue = 0
        Else
            cboUnit.SelectedIndex = -1
        End If
        txtUnitCost.Text = 0.0
        txtNumberOfUnits.Text = 0.0

    End Sub

    Private Sub cboActivityCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboActivityCategory.SelectedIndexChanged

        If cboActivityCategory.SelectedIndex > 0 Then

            Dim objActivity As New BusinessLogic.ActivityCategory(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim sql As String = "Select * from tblActivities A inner join tblActivityActivityCategory AC on A.ActivityID = AC.ActivityID where AC.ActivityCategoryID = " & cboActivityCategory.SelectedValue

            With cboActivity

                .DataSource = objActivity.GetActivityCategory(sql)
                .DataTextField = "Description"
                .DataValueField = "ActivityID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub radBudgets_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radBudgets.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radBudgets.Items(index)
            Dim BudgetID As String

            BudgetID = Server.HtmlDecode(item("BudgetID").Text)

            LoadBudgets(BudgetID)

        End If

        If e.CommandName = "Track" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radBudgets.Items(index)
            Dim BudgetID As String

            BudgetID = Server.HtmlDecode(item("BudgetID").Text)

            Response.Redirect("~/BudgetTrackingPage.aspx?id=" & objUrlEncoder.Encrypt(BudgetID))

        End If

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub radBudgets_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radBudgets.NeedDataSource

        radBudgets.DataSource = DirectCast(ViewState("Budgets"), DataSet)

    End Sub

    Private Sub cboProjects_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProjects.SelectedIndexChanged

        If cboProjects.SelectedIndex > 0 Then

            LoadGrid(cboProjects.SelectedValue)

        End If

    End Sub
End Class

