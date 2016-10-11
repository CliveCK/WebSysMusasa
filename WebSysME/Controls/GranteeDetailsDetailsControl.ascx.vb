Imports System.IO
Imports BusinessLogic
Imports SysPermissionsManager.Functionality
Imports Telerik.Web.UI
Imports Universal.CommonFunctions

Partial Class GranteeDetailsDetailsControl
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

            LoadCombos()

            If Not IsNothing(Request.QueryString("id")) Then

                LoadGranteeDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

            If Not IsNothing(Request.QueryString("GrantDetailID")) Then

                txtGrantDetailID.Text = objUrlEncoder.Decrypt(Request.QueryString("GrantDetailID"))
                LoadProjectManager(txtGrantDetailID.Text)

            End If

        End If

    End Sub

    Private Sub LoadProjectManager(ByVal GrantDetailID As Long)

        Dim objPM As New BusinessLogic.Projects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objGrantKeyChangePromises As New BusinessLogic.GrantKeyChangePromise(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Dim ds As DataSet = objPM.GetProjects("Select *, O.Name as ParentDonor from tblProjects P inner join tblGrantDetails G on G.ProjectID = P.Project
            left outer join tblStaffMembers S on P.ProjectManager = S.StaffID 
            left outer join tblOrganization O on O.OrganizationID = G.DonorID 
            WHERE G.GrantDetailID = " & GrantDetailID)
        txtProjectManager.Text = Catchnull(ds.Tables(0).Rows(0)("StaffFullName"), "")
        txtParentDonor.Text = Catchnull(ds.Tables(0).Rows(0)("ParentDonor"), "")

        Dim dsGrantKCP As DataSet = objGrantKeyChangePromises.GetGrantKeyChangePromisesByGrantDetailID(GrantDetailID)

        If Not IsNothing(dsGrantKCP) AndAlso dsGrantKCP.Tables.Count > 0 AndAlso dsGrantKCP.Tables(0).Rows.Count > 0 Then

            For Each i As ListItem In lstKeyChangePromise.Items

                If dsGrantKCP.Tables(0).Select("KeyChangePromiseID = " & i.Value).Length > 0 Then

                    If Not i.Value = 0 Then i.Selected = True

                End If

            Next

        End If

    End Sub

    Private Sub LoadDisbursementsSchedule(ByVal GranteeID As Long)

        Dim objDisbursementsSchedule As New DisbursementsSchedule(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radSchedule

            .DataSource = objDisbursementsSchedule.GetDisbursementsSchedules(GranteeID)
            .DataBind()

            ViewState("DSchedule") = .DataSource

        End With

    End Sub

    Private Sub LoadCombos()

        Dim objLookup As New BusinessLogic.CommonFunctions


        With lstKeyChangePromise

            .DataSource = objLookup.Lookup("tblKeyChangePromises", "KeyChangePromiseID", "KeyChangePromiseNo")
            .DataTextField = "KeyChangePromiseNo"
            .DataValueField = "KeyChangePromiseID"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboPartner

            .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name")
            .DataTextField = "Name"
            .DataValueField = "OrganizationID"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboContactPerson

            .DataSource = objLookup.Lookup("tblStaffMembers", "StaffID", "StaffFullName")
            .DataTextField = "StaffFullName"
            .DataValueField = "StaffID"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboPartnershipType

            .DataSource = objLookup.Lookup("luPartnershipType", "PartnershipTypeID", "Description")
            .DataTextField = "Description"
            .DataValueField = "PartnershipTypeID"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboProjectStatus

            .DataSource = objLookup.Lookup("luProjectStatus", "ProjectStatusID", "Description")
            .DataTextField = "Description"
            .DataValueField = "ProjectStatusID"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboDistrict

            .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name")
            .DataTextField = "Name"
            .DataValueField = "DistrictID"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboExtensionType

            .DataSource = objLookup.Lookup("luProjectExtensionType", "ProjectExtensionTypeID", "Description")
            .DataTextField = "Description"
            .DataValueField = "ProjectExtensionTypeID"
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

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadGranteeDetails(ByVal GranteeID As Long) As Boolean

        Try

            Dim objGranteeDetails As New BusinessLogic.GranteeDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGranteeDetails

                If .Retrieve(GranteeID) Then

                    txtGranteeID.Text = .GranteeID
                    txtGrantDetailID.Text = .GrantDetailID
                    If Not IsNothing(cboPartner.Items.FindByValue(.PartnerID)) Then cboPartner.SelectedValue = .PartnerID
                    If Not IsNothing(cboContactPerson.Items.FindByValue(.ContactPersonID)) Then cboContactPerson.SelectedValue = .ContactPersonID
                    If Not IsNothing(cboPartnershipType.Items.FindByValue(.PartnershipTypeID)) Then cboPartnershipType.SelectedValue = .PartnershipTypeID
                    If Not IsNothing(cboProjectStatus.Items.FindByValue(.ProjectStatusID)) Then cboProjectStatus.SelectedValue = .ProjectStatusID
                    txtProjectDuration.Text = .ProjectDuration
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    txtNumberOfReports.Text = .NumberOfReports
                    If Not IsNothing(cboExtensionType.Items.FindByValue(.ExtensionTypeID)) Then cboExtensionType.SelectedValue = .ExtensionTypeID
                    If Not .ProjectStartDate = "" Then radStartDate.SelectedDate = .ProjectStartDate
                    If Not .ProjectEndDate = "" Then radEndDate.SelectedDate = .ProjectEndDate
                    chkExtensionGranted.Checked = .ExtensionGranted
                    txtTotalGrantValue.Text = .TotalGrantValue
                    txtProjectTitle.Text = .ProjectTitle
                    txtProjectDeliverables.Text = .ProjectDeliverables
                    txtReasonForExtension.Text = .ReasonForExtension

                    EnableDisableComponents(.ExtensionGranted)

                    LoadGrid(.GranteeID)
                    LoadProjectManager(.GrantDetailID)
                    LoadDisbursementsSchedule(.GranteeID)
                    LoadFiles(.GranteeID)

                    ShowMessage("Grantee Details loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Grantee Details", MessageTypeEnum.Error)
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

            Dim objGranteeDetails As New BusinessLogic.GranteeDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGranteeDetails

                .GranteeID = IIf(IsNumeric(txtGranteeID.Text), txtGranteeID.Text, 0)
                .GrantDetailID = IIf(IsNumeric(txtGrantDetailID.Text), txtGrantDetailID.Text, 0)
                If cboPartner.SelectedIndex > -1 Then .PartnerID = cboPartner.SelectedValue
                If cboContactPerson.SelectedIndex > -1 Then .ContactPersonID = cboContactPerson.SelectedValue
                If cboPartnershipType.SelectedIndex > -1 Then .PartnershipTypeID = cboPartnershipType.SelectedValue
                If cboProjectStatus.SelectedIndex > -1 Then .ProjectStatusID = cboProjectStatus.SelectedValue
                .ProjectDuration = txtProjectDuration.Text
                If cboDistrict.SelectedIndex > -1 Then .DistrictID = cboDistrict.SelectedValue
                .NumberOfReports = txtNumberOfReports.Text
                If cboExtensionType.SelectedIndex > -1 Then .ExtensionTypeID = cboExtensionType.SelectedValue
                If radStartDate.SelectedDate.HasValue Then .ProjectStartDate = radStartDate.SelectedDate
                If radEndDate.SelectedDate.HasValue Then .ProjectEndDate = radEndDate.SelectedDate
                .ExtensionGranted = chkExtensionGranted.Checked
                .TotalGrantValue = txtTotalGrantValue.Text
                .ProjectTitle = txtProjectTitle.Text
                .ProjectDeliverables = txtProjectDeliverables.Text
                .ReasonForExtension = txtReasonForExtension.Text

                If .Save Then

                    If radExpectedDate.SelectedDate.HasValue AndAlso IsNumeric(txtAmount.Text) Then

                        Dim objSchedule As New DisbursementsSchedule(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                        With objSchedule

                            .GranteeID = objGranteeDetails.GranteeID
                            .ExpectedDate = radExpectedDate.SelectedDate
                            .Amount = txtAmount.Text

                            .Save()

                        End With

                    End If

                    'Save any attached files
                    SaveFile(.GranteeID)

                    LoadGranteeDetails(.GranteeID)
                    If Not IsNumeric(txtGranteeID.Text) OrElse Trim(txtGranteeID.Text) = 0 Then txtGranteeID.Text = .GranteeID
                    ShowMessage("GranteeDetails saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save Grantee details!", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub SaveFile(ByVal ObjectID As Long)

        If fuUpload.HasFile Then

            Try

                Dim FilePath As String = Path.GetFileName(fuUpload.FileName)
                Dim Ext As String = Path.GetExtension(FilePath)

                fuUpload.SaveAs(Server.MapPath("~/FileUploads/") & FilePath)

                Save(FilePath, Ext, ObjectID)

            Catch ex As Exception

                log.Error(ex)

            End Try

        End If

    End Sub

    Public Function Save(ByVal FilePath As String, ByVal FileExt As String, ByVal ObjectID As Long) As Boolean

        Try

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFiles

                .FileID = 0
                .FileDate = Now
                .FileTypeID = 0
                .Title = fuUpload.FileName
                .Author = CookiesWrapper.thisUserFullName
                .Description = "Grant Report"
                .FilePath = FilePath
                .FileExtension = FileExt
                .AuthorOrganization = ""
                .ApplySecurity = False

                If .Save() Then

                    Dim objObjects As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objObjects

                        .DocumentObjectID = 0
                        .DocumentID = objFiles.FileID
                        .ObjectID = ObjectID
                        .ObjectTypeID = .GetObjectTypeIDByDescription("Grantee")

                        .Save()

                    End With

                End If

            End With


        Catch ex As Exception
            log.Error(ex)
            Return False

        End Try

    End Function

    Private Sub radAttachments_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radAttachments.NeedDataSource

        radAttachments.DataSource = DirectCast(ViewState("GranteeAttachments"), DataSet)

    End Sub

    Private Sub radAttachments_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radAttachments.ItemCommand

        If e.CommandName = "Download" Then

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radAttachments.Items(index)
            Dim FilePath As String

            FilePath = Server.HtmlDecode(item("FilePath").Text)

            With Response

                .Clear()
                .ClearContent()
                .ClearHeaders()
                .BufferOutput = True

            End With

            If File.Exists(Request.PhysicalApplicationPath & FilePath) Or File.Exists(Server.MapPath("~/FileUploads/" & FilePath)) Then

                Dim oFileStream As FileStream
                Dim fileLen As Long

                Try

                    oFileStream = File.Open(Server.MapPath("~/FileUploads/" & FilePath), FileMode.Open, FileAccess.Read, FileShare.None)
                    fileLen = oFileStream.Length

                    Dim ByteFile(fileLen - 1) As Byte

                    If fileLen > 0 Then
                        oFileStream.Read(ByteFile, 0, oFileStream.Length - 1)
                        oFileStream.Close()

                        With Response

                            .AddHeader("Content-Disposition", "attachment; filename=" & FilePath.Replace(" ", "_"))
                            .ContentType = "application/octet-stream"
                            .BinaryWrite(ByteFile)
                            '.WriteFile(Server.MapPath("~/FileUploads/" & FilePath))

                            objFiles.UpdateDownloadCount(Server.HtmlDecode(item("FileID").Text))

                            .End()
                            HttpContext.Current.ApplicationInstance.CompleteRequest()

                        End With

                    Else
                        log.Error("Empty File...")
                    End If

                Catch ex As Exception

                End Try

            Else

                ShowMessage("Error: File not found!!!", MessageTypeEnum.Error)

            End If
        End If

    End Sub

    Public Sub Clear()

        txtGranteeID.Text = ""
        txtGrantDetailID.Text = ""
        If Not IsNothing(cboPartner.Items.FindByValue("")) Then
            cboPartner.SelectedValue = ""
        ElseIf Not IsNothing(cboPartner.Items.FindByValue(0)) Then
            cboPartner.SelectedValue = 0
        Else
            cboPartner.SelectedIndex = -1
        End If
        If Not IsNothing(cboContactPerson.Items.FindByValue("")) Then
            cboContactPerson.SelectedValue = ""
        ElseIf Not IsNothing(cboContactPerson.Items.FindByValue(0)) Then
            cboContactPerson.SelectedValue = 0
        Else
            cboContactPerson.SelectedIndex = -1
        End If
        If Not IsNothing(cboPartnershipType.Items.FindByValue("")) Then
            cboPartnershipType.SelectedValue = ""
        ElseIf Not IsNothing(cboPartnershipType.Items.FindByValue(0)) Then
            cboPartnershipType.SelectedValue = 0
        Else
            cboPartnershipType.SelectedIndex = -1
        End If
        If Not IsNothing(cboProjectStatus.Items.FindByValue("")) Then
            cboProjectStatus.SelectedValue = ""
        ElseIf Not IsNothing(cboProjectStatus.Items.FindByValue(0)) Then
            cboProjectStatus.SelectedValue = 0
        Else
            cboProjectStatus.SelectedIndex = -1
        End If
        txtProjectDuration.Text = 0
        If Not IsNothing(cboDistrict.Items.FindByValue("")) Then
            cboDistrict.SelectedValue = ""
        ElseIf Not IsNothing(cboDistrict.Items.FindByValue(0)) Then
            cboDistrict.SelectedValue = 0
        Else
            cboDistrict.SelectedIndex = -1
        End If
        txtNumberOfReports.Text = 0
        If Not IsNothing(cboExtensionType.Items.FindByValue("")) Then
            cboExtensionType.SelectedValue = ""
        ElseIf Not IsNothing(cboExtensionType.Items.FindByValue(0)) Then
            cboExtensionType.SelectedValue = 0
        Else
            cboExtensionType.SelectedIndex = -1
        End If
        radStartDate.Clear()
        radEndDate.Clear()
        chkExtensionGranted.Checked = False
        txtTotalGrantValue.Text = 0.0
        txtProjectTitle.Text = ""
        txtProjectDeliverables.Text = ""
        txtReasonForExtension.Text = ""

    End Sub

    Private Sub LoadGrid(ByVal GranteeDetailID As Long)

        Dim objReportDates As New GranteeContractReportingDates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radReports

            .DataSource = objReportDates.GetGranteeContractReportingDates(GranteeDetailID).Tables(0)
            .DataBind()

            ViewState("GranteeContractReports") = .DataSource

        End With

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If IsNumeric(txtGrantDetailID.Text) AndAlso txtGrantDetailID.Text > 0 Then

            Dim objReportDates As New GranteeContractReportingDates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objReportDates

                .GrantDetailID = txtGranteeID.Text
                .NameOfReport = txtReportName.Text
                If cboTypeOfReport.SelectedIndex > -1 Then .ReportTypeID = cboTypeOfReport.SelectedValue
                If radExpectedDate2.SelectedDate.HasValue Then .ExpectedDate = radExpectedDate2.SelectedDate
                .Comments = ""
                .SubmissionStatusID = 1
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


    Private Sub LoadFiles(ByVal GranteeID As Long)

        Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "Select FileID, Title, FilePath from tblFiles F inner join tblDocumentObjects D on D.DocumentID = F.FileID "
        sql &= "inner join luObjectTypes O on O.ObjectTypeID = D.ObjectTypeID  where O.Description = 'Grantee' "
        sql &= " And ObjectID = " & GranteeID

        With radAttachments

            .DataSource = objFiles.GetFiles(sql)
            .DataBind()

            ViewState("GranteeAttachments") = .DataSource

        End With

    End Sub

    Private Sub radSchedule_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radSchedule.NeedDataSource

        radSchedule.DataSource = DirectCast(ViewState("DSchedule"), DataSet)

    End Sub

    Private Sub chkExtensionGranted_CheckedChanged(sender As Object, e As EventArgs) Handles chkExtensionGranted.CheckedChanged

        EnableDisableComponents(chkExtensionGranted.Checked)

    End Sub

    Private Sub EnableDisableComponents(ByVal Enable As Boolean)

        txtReasonForExtension.Enabled = Enable
        cboExtensionType.Enabled = Enable

    End Sub

    Private Sub radReports_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radReports.NeedDataSource

        radReports.DataSource = DirectCast(ViewState("GranteeContractReports"), DataTable)

    End Sub


    Private Sub radSchedule_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radSchedule.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radSchedule.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objHealthCenter As New BusinessLogic.DisbursementsSchedule(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objHealthCenter

                        .DisbursementsScheduleID = Server.HtmlDecode(e.CommandArgument)

                        If .Delete() Then

                            ShowMessage("Schedule deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radReports_UpdateCommand(sender As Object, e As GridCommandEventArgs) Handles radReports.UpdateCommand

        If SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.UpdateContractReportsStatus) Then

            Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)

            'Locate the changed row in the DataSource
            Dim changedRows() As DataRow = DirectCast(ViewState("GranteeContractReports"), DataTable).Select("ReportDateID = " & editedItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ReportDateID"))

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

                Dim objReportDates As New GranteeContractReportingDates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objReportDates

                    .GrantDetailID = txtGranteeID.Text
                    .ReportDateID = editedItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ReportDateID")
                    .NameOfReport = NameOfReport.Text
                    .ReportTypeID = cboTypeOfReport.SelectedValue
                    .SubmissionStatusID = cboStatus.SelectedValue
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
                radReports.Controls.Add(New LiteralControl("Unable to update entry. Reason: " & ex.Message))
                e.Canceled = True

            End Try

        Else

            e.Canceled = True
            ShowMessage("You are not authorised to modify this entry!", MessageTypeEnum.Error)

        End If

    End Sub

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

