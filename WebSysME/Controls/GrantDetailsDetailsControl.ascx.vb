Imports BusinessLogic
Imports SysPermissionsManager.Functionality
Imports Telerik.Web.UI

Partial Class GrantDetailsDetailsControl
    Inherits System.Web.UI.UserControl

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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

            With cboProject

                .DataSource = objLookup.Lookup("tblProjects", "Project", "Name")
                .DataTextField = "Name"
                .DataValueField = "Project"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboDonor

                .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name")
                .DataTextField = "Name"
                .DataValueField = "OrganizationID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboContractCurrency

                .DataSource = objLookup.Lookup("luCurrency", "CurrencyID", "Description")
                .DataTextField = "Description"
                .DataValueField = "CurrencyID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With lstKeyChangePromise

                .DataSource = objLookup.Lookup("tblKeyChangePromises", "KeyChangePromiseID", "KeyChangePromiseNo")
                .DataTextField = "KeyChangePromiseNo"
                .DataValueField = "KeyChangePromiseID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboTypeOfReport

                .DataSource = objLookup.Lookup("luContractReportTypes", "ReportTypeID", "Description")
                .DataTextField = "Description"
                .DataValueField = "ReportTypeID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboPersonResponsible

                .DataSource = objLookup.Lookup("tblStaffMembers", "StaffID", "StaffFullName")
                .DataTextField = "StaffFullName"
                .DataValueField = "StaffID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            radExpectedDate.MinDate = Now

            If Not IsNothing(Request.QueryString("id")) Then

                LoadGrantDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Private Sub LoadProjectManager(ByVal cboPM As DropDownList)

        With cboPM

            If .SelectedValue > 0 Then

                Dim objPM As New BusinessLogic.Projects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                Dim ds As DataSet = objPM.GetProjects("Select * from tblProjects P inner join tblStaffMembers S on P.ProjectManager = S.StaffID WHERE Project = " & .SelectedValue)
                txtProjectManager.Text = ds.Tables(0).Rows(0)("StaffFullName")

            End If

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub ClearSelection()

        lstKeyChangePromise.ClearSelection()

    End Sub

    Public Function LoadGrantDetails(ByVal GrantDetailID As Long) As Boolean

        Try

            Dim objGrantDetails As New GrantDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objGrantKeyChangePromises As New BusinessLogic.GrantKeyChangePromise(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGrantDetails

                ClearSelection()

                If .Retrieve(GrantDetailID) Then

                    txtGrantDetailID.Text = .GrantDetailID
                    If Not IsNothing(cboProject.Items.FindByValue(.ProjectID)) Then cboProject.SelectedValue = .ProjectID
                    If Not IsNothing(cboDonor.Items.FindByValue(.DonorID)) Then cboDonor.SelectedValue = .DonorID
                    'If Not IsNothing(cboKeyChangePromise.Items.FindByValue(.KeyChangePromiseID)) Then cboKeyChangePromise.SelectedValue = .KeyChangePromiseID
                    If Not IsNothing(cboContractCurrency.Items.FindByValue(.ContractCurrencyID)) Then cboContractCurrency.SelectedValue = .ContractCurrencyID
                    txtContractDurationInMonths.Text = .ContractDurationInMonths
                    txtNumberOfReports.Text = .NumberOfReports
                    If Not .ContractStartDate = "" Then radStartDate.SelectedDate = .ContractStartDate
                    If Not .ContractEndDate = "" Then radEndDate.SelectedDate = .ContractEndDate
                    If Not .NewContractEndDate = "" Then radContractEndDate.SelectedDate = .NewContractEndDate
                    If Not IsNothing(cboExtensionGranted.Items.FindByValue(.ExtensionGranted)) Then cboExtensionGranted.SelectedValue = .ExtensionGranted
                    txtContractValueInCurrency.Text = .ContractValueInCurrency
                    txtContractValueInUSD.Text = .ContractValueInUSD
                    txtContractValueInGBP.Text = .ContractValueInGBP
                    txtTotalProjectCosts.Text = .TotalProjectCosts
                    txtTotalAdminCosts.Text = .TotalAdminCosts
                    txtVATInfo.Text = .VATInfo
                    txtNatureOfExtension.Text = .NatureOfExtension
                    txtDonorRequirements.Text = .DonorRequirements

                    EnableDisableComponents(.ExtensionGranted)

                    LoadGrid(.GrantDetailID)
                    LoadProjectManager(cboProject)

                    Dim dsGrantKCP As DataSet = objGrantKeyChangePromises.GetGrantKeyChangePromisesByGrantDetailID(.GrantDetailID)

                    If Not IsNothing(dsGrantKCP) AndAlso dsGrantKCP.Tables.Count > 0 AndAlso dsGrantKCP.Tables(0).Rows.Count > 0 Then

                        For Each i As ListItem In lstKeyChangePromise.Items

                            If dsGrantKCP.Tables(0).Select("KeyChangePromiseID = " & i.Value).Length > 0 Then

                                i.Selected = True

                            End If

                        Next

                    End If

                    ShowMessage("GrantDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load GrantDetails", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub EnableDisableComponents(ByVal Enable As Boolean)

        txtNatureOfExtension.Enabled = Enable
        radContractEndDate.Enabled = Enable

    End Sub

    Public Function Save() As Boolean

        Try

            Dim objGrantDetails As New GrantDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objCurrency As New CurrencyExchageRates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGrantDetails

                .GrantDetailID = IIf(IsNumeric(txtGrantDetailID.Text), txtGrantDetailID.Text, 0)
                If cboProject.SelectedIndex > -1 Then .ProjectID = cboProject.SelectedValue
                If cboDonor.SelectedIndex > -1 Then .DonorID = cboDonor.SelectedValue
                'If cboKeyChangePromise.SelectedIndex > 0 Then .KeyChangePromiseID = cboKeyChangePromise.SelectedValue
                If cboContractCurrency.SelectedIndex > 0 Then .ContractCurrencyID = cboContractCurrency.SelectedValue
                .ContractDurationInMonths = txtContractDurationInMonths.Text
                .NumberOfReports = txtNumberOfReports.Text
                If radStartDate.SelectedDate.HasValue Then .ContractStartDate = radStartDate.SelectedDate
                If radEndDate.SelectedDate.HasValue Then .ContractEndDate = radEndDate.SelectedDate
                If radContractEndDate.SelectedDate.HasValue Then .NewContractEndDate = radContractEndDate.SelectedDate
                If cboExtensionGranted.SelectedIndex > -1 Then .ExtensionGranted = cboExtensionGranted.SelectedValue
                .ContractValueInCurrency = txtContractValueInCurrency.Text

                If .ContractValueInCurrency > 0 AndAlso Not .ContractValueInGBP > 0 Then

                    objCurrency.FromCurrencyID = .ContractCurrencyID
                    objCurrency.ToCurrencyID = objCurrency.GetCurrencyIDByTextSymbol("USD")

                    If IsNumeric(txtContractValueInCurrency.Text) Then .ContractValueInUSD = (txtContractValueInCurrency.Text * objCurrency.GetCurrentExchangeRate())

                    objCurrency.ToCurrencyID = objCurrency.GetCurrencyIDByTextSymbol("GBP")
                    If IsNumeric(txtContractValueInCurrency.Text) Then .ContractValueInGBP = (txtContractValueInCurrency.Text * objCurrency.GetCurrentExchangeRate())

                End If

                If IsNumeric(txtContractValueInCurrency.Text) Then

                    Dim result As Decimal = IIf(Not IsNumeric(txtTotalAdminCosts.Text), 0, Convert.ToDecimal(txtTotalAdminCosts.Text)) + IIf(Not IsNumeric(txtTotalProjectCosts.Text), 0, Convert.ToDecimal(txtTotalProjectCosts.Text))

                    If result > txtContractValueInCurrency.Text Then

                        ShowMessage("Error: Amount exceeds Contract value!!", MessageTypeEnum.Error)
                        txtTotalProjectCosts.BorderColor = Drawing.Color.Red
                        txtTotalAdminCosts.BorderColor = Drawing.Color.Red
                        Exit Function

                    End If

                    If result < txtContractValueInCurrency.Text Then

                        ShowMessage("Error: Amount is less than Contract value!!", MessageTypeEnum.Error)
                        txtTotalProjectCosts.BorderColor = Drawing.Color.Red
                        txtTotalAdminCosts.BorderColor = Drawing.Color.Red
                        Exit Function

                    End If

                    .TotalProjectCosts = txtTotalProjectCosts.Text
                    .TotalAdminCosts = txtTotalAdminCosts.Text

                End If
                .VATInfo = txtVATInfo.Text
                .NatureOfExtension = txtNatureOfExtension.Text
                .DonorRequirements = txtDonorRequirements.Text

                If .Save Then

                    SaveKeyChangePromises(.GrantDetailID)

                    LoadGrantDetails(.GrantDetailID)
                    If Not IsNumeric(txtGrantDetailID.Text) OrElse Trim(txtGrantDetailID.Text) = 0 Then txtGrantDetailID.Text = .GrantDetailID
                    ShowMessage("GrantDetails saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Could not save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtGrantDetailID.Text = ""
        If Not IsNothing(cboProject.Items.FindByValue("")) Then
            cboProject.SelectedValue = ""
        ElseIf Not IsNothing(cboProject.Items.FindByValue(0)) Then
            cboProject.SelectedValue = 0
        Else
            cboProject.SelectedIndex = -1
        End If
        If Not IsNothing(cboDonor.Items.FindByValue("")) Then
            cboDonor.SelectedValue = ""
        ElseIf Not IsNothing(cboDonor.Items.FindByValue(0)) Then
            cboDonor.SelectedValue = 0
        Else
            cboDonor.SelectedIndex = -1
        End If
        If Not IsNothing(cboContractCurrency.Items.FindByValue("")) Then
            cboContractCurrency.SelectedValue = ""
        ElseIf Not IsNothing(cboContractCurrency.Items.FindByValue(0)) Then
            cboContractCurrency.SelectedValue = 0
        Else
            cboContractCurrency.SelectedIndex = -1
        End If
        txtContractDurationInMonths.Text = 0
        txtNumberOfReports.Text = 0
        radStartDate.Clear()
        radEndDate.Clear()
        radContractEndDate.Clear()
        cboExtensionGranted.SelectedValue = 0
        txtContractValueInCurrency.Text = 0.0
        txtContractValueInUSD.Text = 0.0
        txtContractValueInGBP.Text = 0.0
        txtTotalProjectCosts.Text = 0.0
        txtTotalAdminCosts.Text = 0.0
        txtVATInfo.Text = ""
        txtNatureOfExtension.Text = ""
        txtDonorRequirements.Text = ""

    End Sub

    Private Sub LoadGrid(ByVal GrantDetailID As Long)

        Dim objReportDates As New ContractReportingDates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radReports

            .DataSource = objReportDates.GetContractReportingDates(GrantDetailID).Tables(0)
            .DataBind()

            ViewState("ContractReports") = .DataSource

        End With

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If IsNumeric(txtGrantDetailID.Text) AndAlso txtGrantDetailID.Text > 0 Then

            Dim objReportDates As New ContractReportingDates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objReportDates

                .GrantDetailID = txtGrantDetailID.Text
                .NameOfReport = txtReportName.Text
                If cboTypeOfReport.SelectedIndex > -1 Then .ReportTypeID = cboTypeOfReport.SelectedValue
                If radExpectedDate.SelectedDate.HasValue Then .ExpectedDate = radExpectedDate.SelectedDate
                .Comments = ""
                .StatusID = 1
                If cboPersonResponsible.SelectedIndex > -1 Then .PersonResponsibleID = cboPersonResponsible.SelectedValue

                If .Save Then

                    LoadGrid(.GrantDetailID)
                    ShowMessage("Report Date saved successfully...", MessageTypeEnum.Information)

                Else

                    ShowMessage("Failed to save Report Date...Contact to administrator!", MessageTypeEnum.Error)

                End If

            End With

        Else

            ShowMessage("Please save details before saving Report dates!...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radReports_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radReports.NeedDataSource

        radReports.DataSource = DirectCast(ViewState("ContractReports"), DataTable)

    End Sub

    Private Sub cboProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProject.SelectedIndexChanged

        LoadProjectManager(cboProject)

    End Sub

    Private Sub cboExtensionGranted_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboExtensionGranted.SelectedIndexChanged

        EnableDisableComponents(IIf(cboExtensionGranted.SelectedValue = "1", True, False))

    End Sub

    Private Sub lnkGrantee_Click(sender As Object, e As EventArgs) Handles lnkGrantee.Click

        If IsNumeric(txtGrantDetailID.Text) Then

            Response.Redirect("~/GranteeDetails.aspx?GrantDetailID=" & objUrlEncoder.Encrypt(txtGrantDetailID.Text))

        Else

            ShowMessage("Please save details first!!", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radReports_UpdateCommand(sender As Object, e As GridCommandEventArgs) Handles radReports.UpdateCommand

        If SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.UpdateContractReportsStatus) Then

            Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)

            'Locate the changed row in the DataSource
            Dim changedRows() As DataRow = DirectCast(ViewState("ContractReports"), DataTable).Select("ReportDateID = " & editedItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ReportDateID"))

            'Update new values
            Dim newValues As Hashtable = New Hashtable
            'The GridTableView will fill the values from all editable columns in the hash
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
            changedRows(0).BeginEdit()

            Dim cboStatus As RadDropDownList = DirectCast(editedItem.FindControl("cboSubmissionStatus"), RadDropDownList)
            Dim cboTypeOfReport As RadDropDownList = DirectCast(editedItem.FindControl("cboTypeOfReport"), RadDropDownList)
            Dim cboResponsiblePerson1 As RadDropDownList = DirectCast(editedItem.FindControl("cboResponsiblePerson1"), RadDropDownList)
            Dim NameOfReport As RadTextBox = DirectCast(editedItem.FindControl("NameOfReportTextBox"), RadTextBox)
            Dim TypeOfReport As RadTextBox = DirectCast(editedItem.FindControl("TypeOfReportTextBox"), RadTextBox)
            Dim ExpectedDate As RadTextBox = DirectCast(editedItem.FindControl("ExpectedDateTextBox"), RadTextBox)
            Dim CommentsText As RadTextBox = DirectCast(editedItem.FindControl("CommentsTextBox"), RadTextBox)

            Try

                Dim objReportDates As New ContractReportingDates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objReportDates

                    .GrantDetailID = txtGrantDetailID.Text
                    .ReportDateID = editedItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ReportDateID")
                    .NameOfReport = NameOfReport.Text
                    .ReportTypeID = cboTypeOfReport.SelectedValue
                    .StatusID = cboStatus.SelectedValue
                    .Comments = CommentsText.Text
                    .ExpectedDate = ExpectedDate.Text
                    .PersonResponsibleID = cboResponsiblePerson1.SelectedValue

                    .Save()

                End With


                For Each entry As DictionaryEntry In newValues

                    changedRows(0)(CType(entry.Key, String)) = entry.Value

                Next

                changedRows(0).EndEdit()

            Catch ex As Exception

                changedRows(0).CancelEdit()
                radReports.Controls.Add(New LiteralControl("Unable to update changes. Reason: " & ex.Message))
                e.Canceled = True

            End Try

        Else

            e.Canceled = True
            ShowMessage("You are not authorised to modify this entry!", MessageTypeEnum.Error)

        End If

    End Sub

    Private Function SaveKeyChangePromises(ByVal GrantDetailID As Long) As Boolean

        Try

            For Each i As ListItem In lstKeyChangePromise.Items

                Dim objKeyChangePromises As New BusinessLogic.GrantKeyChangePromise(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objKeyChangePromises

                    .GrantDetailID = GrantDetailID
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

    Private Sub radReports_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radReports.ItemDataBound

        If (TypeOf (e.Item) Is GridEditableItem) And e.Item.IsInEditMode Then

            Dim edit As GridEditableItem = DirectCast(e.Item, GridEditableItem)

            Dim cboStatus As RadDropDownList = edit.FindControl("cboSubmissionStatus")
            Dim cboReportType As RadDropDownList = edit.FindControl("cboTypeOfReport")
            Dim cboPersonResponsible1 As RadDropDownList = edit.FindControl("cboResponsiblePerson1")
            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboStatus

                .DataSource = objLookup.Lookup("luSubmissionStatus", "SubmissionStatusID", "Description").Tables(0)
                .DataTextField = "Description"
                .DataValueField = "SubmissionStatusID"
                .DataBind()

                .SelectedValue = DataBinder.Eval(edit.DataItem, "SubmissionStatusID").ToString

            End With

            With cboPersonResponsible1

                .DataSource = objLookup.Lookup("tblStaffMembers", "StaffID", "StaffFullName")
                .DataTextField = "StaffFullName"
                .DataValueField = "StaffID"
                .DataBind()

                .SelectedValue = DataBinder.Eval(edit.DataItem, "PersonResponsibleID").ToString

            End With

            With cboReportType

                .DataSource = objLookup.Lookup("luContractReportTypes", "ReportTypeID", "Description").Tables(0)
                .DataTextField = "Description"
                .DataValueField = "ReportTypeID"
                .DataBind()

                .SelectedValue = DataBinder.Eval(edit.DataItem, "ReportTypeID").ToString

            End With

        End If

    End Sub
End Class

