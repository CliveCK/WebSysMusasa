Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class BudgetTrackingDetailsControl
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

            With cboBudgetMonth

                .DataSource = objLookup.Lookup("luMonths", "MonthID", "Description")
                .DataTextField = "Description"
                .DataValueField = "MonthID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                Dim objBudgets As New Budgets(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                Dim ds As DataSet
                Dim sql As String = "Select B.*, A.Description AS Activity, AC.Description AS ActivityCategory from tblBudgets B "
                sql &= "inner join tblActivities A on A.ActivityID = B.ActivityID "
                sql &= "inner join tblActivityCategory AC on B.ActivityCategoryID = AC.ActivityCategoryID where B.BudgetID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))


                ds = objBudgets.GetBudgets(sql)

                If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    txtActivity.Text = ds.Tables(0).Rows(0)("Activity")
                    txtActivityCategory.Text = ds.Tables(0).Rows(0)("ActivityCategory")

                End If

                LoadGrid(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadBudgetTracking(ByVal BudgetTrackinngID As Long) As Boolean

        Try

            Dim objBudgetTracking As New BudgetTracking(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBudgetTracking

                If .Retrieve(BudgetTrackinngID) Then

                    txtBudgetTrackinngID.Text = .BudgetTrackinngID
                    txtBudgetID.Text = .BudgetID
                    txtBudgetYear.Text = .BudgetYear
                    If Not IsNothing(cboBudgetMonth.Items.FindByValue(.BudgetMonthID)) Then cboBudgetMonth.SelectedValue = .BudgetMonthID
                    txtBudgetTarget.Text = .BudgetTarget
                    txtActual.Text = .Actual
                    txtComments.Text = .Comments

                    ShowMessage("BudgetTracking loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load BudgetTracking", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub LoadGrid(ByVal BudgetID As Long)

        Dim objBudgetTracking As New BudgetTracking(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radBudgetTracking

            .DataSource = objBudgetTracking.GetBudgetTrackingByBudget(objUrlEncoder.Decrypt(Request.QueryString("id")))
            .DataBind()

            ViewState("BudgetTracking") = .DataSource

        End With


    End Sub

    Public Function Save() As Boolean

        Try

            If Not IsNumeric(txtBudgetYear.Text) Then Return False

            Dim objBudgetTracking As New BudgetTracking(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBudgetTracking

                .BudgetTrackinngID = IIf(IsNumeric(txtBudgetTrackinngID.Text), txtBudgetTrackinngID.Text, 0)
                .BudgetID = objUrlEncoder.Decrypt(Request.QueryString("id"))
                .BudgetYear = txtBudgetYear.Text
                If cboBudgetMonth.SelectedIndex > 0 Then .BudgetMonthID = cboBudgetMonth.SelectedValue
                .BudgetTarget = txtBudgetTarget.Text
                .Actual = txtActual.Text
                .Comments = txtComments.Text

                If .Save Then

                    If Not IsNumeric(txtBudgetTrackinngID.Text) OrElse Trim(txtBudgetTrackinngID.Text) = 0 Then txtBudgetTrackinngID.Text = .BudgetTrackinngID
                    LoadGrid(objUrlEncoder.Decrypt(Request.QueryString("id")))
                    ShowMessage("BudgetTracking saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save Tracking...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtBudgetTrackinngID.Text = ""
        txtBudgetYear.Text = 0
        If Not IsNothing(cboBudgetMonth.Items.FindByValue("")) Then
            cboBudgetMonth.SelectedValue = ""
        ElseIf Not IsNothing(cboBudgetMonth.Items.FindByValue(0)) Then
            cboBudgetMonth.SelectedValue = 0
        Else
            cboBudgetMonth.SelectedIndex = -1
        End If
        txtBudgetTarget.Text = 0.0
        txtActual.Text = 0.0
        txtComments.Text = ""

    End Sub

    Private Sub radBudgetTracking_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radBudgetTracking.NeedDataSource

        radBudgetTracking.DataSource = DirectCast(ViewState("BudgetTracking"), DataSet)

    End Sub

    Private Sub radBudgetTracking_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radBudgetTracking.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radBudgetTracking.Items(index)
            Dim BudgetTrackingID As String

            BudgetTrackingID = Server.HtmlDecode(item("BudgetTrackingID").Text)

            LoadBudgetTracking(BudgetTrackingID)

        End If

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub
End Class

