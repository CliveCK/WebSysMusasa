Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class StrategicPlanDetailsDetailsControl
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

            With cboObjective

                .DataSource = objLookup.Lookup("tblStrategicObjectives", "StrategicObjectiveID", "Code").Tables(0)
                .DataValueField = "StrategicObjectiveID"
                .DataTextField = "Code"
                .DataBind()
                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0


            End With

            With cboActivity

                .DataSource = objLookup.Lookup("tblActivityCategory", "ActivityCategoryID", "Description").Tables(0)
                .DataValueField = "ActivityCategoryID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboMonth

                .DataSource = objLookup.Lookup("luMonths", "MonthID", "Description").Tables(0)
                .DataValueField = "Description"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadGrid()

            End If


        End If

    End Sub

    Private Sub LoadGrid()

        Dim objStrategicDetails As New StrategicPlanDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "select S.*, A.Description As Activity, O.Description As Objective from tblStrategicPlanDetails S inner join "
        sql &= "tblStrategicPlans SP on S.StrategicPlanID = SP.StrategicPlanID "
        sql &= "inner join tblActivityCategory A on A.ActivityCategoryID = S.ActivityID "
        sql &= "inner join tblObjectives O on O.ObjectiveID = S.ObjectiveID WHERE S.StrategicPlanID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))

        With radMilestoneListing

            .DataSource = objStrategicDetails.GetStrategicPlanDetails(sql)
            .DataBind()

            Session("PlanDetails") = .DataSource

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadStrategicPlanDetails(ByVal StrategicPlanDetailID As Long) As Boolean

        Try

            Dim objStrategicPlanDetails As New StrategicPlanDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objStrategicPlanDetails

                If .Retrieve(StrategicPlanDetailID) Then

                    txtStrategicPlanDetailID.Text = .StrategicPlanDetailID
                    txtStrategicPlanID.Text = .StrategicPlanID
                    If Not IsNothing(cboObjective.Items.FindByValue(.ObjectiveID)) Then cboObjective.SelectedValue = .ObjectiveID
                    If Not IsNothing(cboActivity.Items.FindByValue(.ActivityID)) Then cboActivity.SelectedValue = .ActivityID
                    txtPlanYear.Text = .PlanYear
                    txtPlanQuarter.Text = .PlanQuarter
                    If Not IsNothing(cboMonth.Items.FindByValue(.PlanMonth)) Then cboMonth.SelectedValue = .PlanMonth
                    txtMilestone.Text = .Milestone

                    ShowMessage("StrategicPlanDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load StrategicPlanDetails: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objStrategicPlanDetails As New StrategicPlanDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objStrategicPlanDetails

                .StrategicPlanDetailID = IIf(IsNumeric(txtStrategicPlanDetailID.Text), txtStrategicPlanDetailID.Text, 0)
                .StrategicPlanID = IIf(IsNumeric(txtStrategicPlanID.Text), txtStrategicPlanID.Text, objUrlEncoder.Decrypt(Request.QueryString("id")))
                If cboObjective.SelectedIndex > -1 Then .ObjectiveID = cboObjective.SelectedValue
                If cboActivity.SelectedIndex > -1 Then .ActivityID = cboActivity.SelectedValue
                If cboMonth.SelectedIndex > -1 Then .PlanMonth = cboMonth.SelectedValue
                .PlanYear = txtPlanYear.Text
                .PlanQuarter = txtPlanQuarter.Text
                .Milestone = txtMilestone.Text

                If .Save Then

                    If Not IsNumeric(txtStrategicPlanDetailID.Text) OrElse Trim(txtStrategicPlanDetailID.Text) = 0 Then txtStrategicPlanDetailID.Text = .StrategicPlanDetailID
                    LoadGrid()
                    ShowMessage("StrategicPlanDetails saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtStrategicPlanDetailID.Text = ""
        If Not IsNothing(cboObjective.Items.FindByValue("")) Then
            cboObjective.SelectedValue = ""
        ElseIf Not IsNothing(cboObjective.Items.FindByValue(0)) Then
            cboObjective.SelectedValue = 0
        Else
            cboObjective.SelectedIndex = -1
        End If
        If Not IsNothing(cboActivity.Items.FindByValue("")) Then
            cboActivity.SelectedValue = ""
        ElseIf Not IsNothing(cboActivity.Items.FindByValue(0)) Then
            cboActivity.SelectedValue = 0
        Else
            cboActivity.SelectedIndex = -1
        End If
        If Not IsNothing(cboMonth.Items.FindByValue("")) Then
            cboMonth.SelectedValue = ""
        ElseIf Not IsNothing(cboMonth.Items.FindByValue(0)) Then
            cboMonth.SelectedValue = 0
        Else
            cboMonth.SelectedIndex = -1
        End If
        txtPlanYear.Text = ""
        txtPlanQuarter.Text = ""
        txtMilestone.Text = ""

    End Sub

    Private Sub radMilestoneListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radMilestoneListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radMilestoneListing.Items(index1)
            Dim StrategicPlanDetailID As Integer

            StrategicPlanDetailID = Server.HtmlDecode(item1("StrategicPlanDetailID").Text)

            LoadStrategicPlanDetails(StrategicPlanDetailID)

        End If

    End Sub

    Private Sub radMilestoneListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radMilestoneListing.NeedDataSource

        radMilestoneListing.DataSource = Session("PlanDetails")

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click

        Response.Redirect("~/StrategicPlansPage.aspx?id=" & Request.QueryString("id"))

    End Sub
End Class

