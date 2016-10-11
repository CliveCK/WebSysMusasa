Imports System.Xml.Linq
Imports Universal.CommonFunctions
Imports Microsoft.Practices.EnterpriseLibrary.Data

Partial Public Class CustomFieldTemplates
    Inherits System.Web.UI.Page

    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database
    Private objCF As BusinessLogic.CustomFields.CustomFieldsManager

    Private UserID As Long
    Private ContactID As Long
    Private DataLookup As New BusinessLogic.CommonFunctions

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        UserID = CookiesWrapper.thisUserID

        ContactID = Request.QueryString("clientID")

        db = New DatabaseProviderFactory().Create("ConnectionString")
        objCF = New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

        If Not Page.IsPostBack Then

            load_templates()
            LoadTemplateFields() 'cboTemplates_SelectedIndexChanged(Nothing, Nothing)

            With ucAppliesTo

                With .AvailableOptions
                    .DataTextField = "Description"
                    .DataValueField = "ID"
                End With

                With .SelectedOptions
                    .DataTextField = "Description"
                    .DataValueField = "ID"
                End With

            End With

        End If

    End Sub

    <Web.Services.WebMethod()> _
    Public Shared Function IsFieldNameUnique(ByVal FieldName As String, ByVal CustomFieldID As Long) As CustomFieldValidationResult

        Dim sql As String = String.Empty

        sql &= "declare @CustomFieldID int, @FieldName varchar(1000) " & vbCrLf & vbCrLf
        sql &= "select @CustomFieldID = " & CustomFieldID & ", @FieldName = '" & FieldName.Replace("'", "''") & "'" & vbCrLf & vbCrLf
        sql &= ";with FieldNames as ( " & vbCrLf
        sql &= "  select CustomFieldID, FieldName, TemplateName from CustomField_Fields F INNER JOIN CustomField_Templates T ON T.TemplateID = F.TemplateID " & vbCrLf
        sql &= "  union " & vbCrLf
        sql &= "  select null, [name], 'Reserved' from sys.columns  " & vbCrLf
        sql &= "  where [object_id] In ( " & vbCrLf
        sql &= "    object_id('Lookup_Root'),  " & vbCrLf
        sql &= "    object_id('Contacts'),  " & vbCrLf
        sql &= "    object_id('Projects'),  " & vbCrLf
        sql &= "    object_id('Documents') " & vbCrLf
        sql &= "  ) " & vbCrLf
        sql &= ") " & vbCrLf
        sql &= "select * from FieldNames " & vbCrLf
        sql &= "where (FieldName = @FieldName)  " & vbCrLf
        If CustomFieldID > 0 Then sql &= "  and (CustomFieldID is null or (CustomFieldID is not null and CustomFieldID <> @CustomFieldID)) " & vbCrLf

        Dim db = New DatabaseProviderFactory().Create("ConnectionString")
        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql, "Fields")

        Dim r As New CustomFieldValidationResult

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

            For Each dr As DataRow In ds.Tables(0).Rows

                r.Message &= IIf(String.IsNullOrEmpty(r.Message), "", ", ") & Catchnull(dr("TemplateName"), "") & ": " & Catchnull(dr("FieldName"), "")

            Next

            r.IsUnique = False

        Else

            r.IsUnique = True

        End If

        Return r

    End Function

    Protected Sub cboTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTemplates.SelectedIndexChanged

        If Page.IsPostBack Then

            LoadTemplateFields()

        End If

    End Sub

    Private Sub LoadTemplateFields()

        Try

            If cboTemplates.SelectedIndex > -1 AndAlso IsNumeric(cboTemplates.SelectedItem.Value) Then

                With radgTemplates

                    Dim objT As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

                    Dim dt As DataTable = objT.GetTemplateFields(cboTemplates.SelectedItem.Value)
                    If dt IsNot Nothing Then

                        Dim dr As DataRow = dt.NewRow
                        dr("CustomFieldID") = 0
                        dr("Search") = 0
                        dr("Required") = 0
                        dr("NewLine") = 0
                        dr("FieldType") = "Text"
                        dr("HelpNotes") = String.Empty
                        dr("ShowInResults") = 1
                        dr("LoadOnDemand") = 1
                        dt.Rows.Add(dr)

                    End If

                    dt.DefaultView.Sort = "DisplayIndex"

                    ViewState("customfields") = dt

                    ucAppliesTo.AvailableOptions.Items.Clear()
                    ucAppliesTo.SelectedOptions.Items.Clear()
                    cboApplyTo.SelectedValue = ""

                    If cboTemplates.SelectedItem IsNot Nothing Then

                        lblTemplateName.Text = cboTemplates.SelectedItem.Text
                        txtRenameTemplate.Text = cboTemplates.SelectedItem.Text

                    End If

                    .DataSource = dt
                    .DataBind()

                    .ShowFooter = dt.Rows.Count > 0

                    cmdRemoveAllUsed.Visible = True
                    cmdClearAutoApplyRules.Visible = True
                    lblRules.Visible = True

                End With

            End If

        Catch ex As Exception

            Log.Error(ex)

        End Try

    End Sub

    Private Sub load_templates()

        Dim objProject As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)
        Dim templateTable As DataTable = objProject.Get_Templates(False)

        cboTemplates.Items.Clear()
        txtTemplateFields.Text = ""

        For i As Integer = 0 To templateTable.Rows.Count - 1

            Dim dr As DataRow = templateTable.Rows(i)

            cboTemplates.Items.Add(New ListItem(dr("TemplateName"), dr("TemplateID")))

            Dim fieldsTable As DataTable = objProject.GetTemplateFields(dr("TemplateID"))

            For j As Integer = 0 To fieldsTable.Rows.Count - 1

                txtTemplateFields.Text = txtTemplateFields.Text & dr(0) & ";" & fieldsTable.Rows(j).Item(0) & ";" & fieldsTable.Rows(j).Item(1) & ";" & fieldsTable.Rows(j).Item("CustomFieldID") & "~"

            Next

        Next

        If cboTemplates.Items.Count > 0 Then cboTemplates.SelectedIndex = 0

    End Sub

    Protected Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        If txtTemplate.Text <> "" AndAlso cboTemplates.Items.FindByText(txtTemplate.Text) Is Nothing Then

            cboTemplates.Items.Add(New ListItem(txtTemplate.Text, txtTemplate.Text, True))
            cboTemplates.SelectedValue = cboTemplates.Items.FindByText(txtTemplate.Text).Value

            Dim objCFT As New BusinessLogic.CustomFields.CustomFieldTemplate("ConnectionString", CookiesWrapper.thisUserID)

            objCFT.RetrieveByName(txtTemplate.Text)
            objCFT.TemplateName = txtTemplate.Text
            objCFT.TemplateType = cboTemplateType.Text

            If objCFT.Save() Then
                ShowMessage("Custom Field Template successfully created...", MessageTypeEnum.Information)
            Else
                ShowMessage(objCFT.ErrorMessage, MessageTypeEnum.Error)
            End If

            txtTemplate.Text = ""
            cboTemplates_SelectedIndexChanged(Nothing, Nothing)

        End If

    End Sub

    Protected Sub SaveFields()

        Try

            For Each r As Telerik.Web.UI.GridDataItem In radgTemplates.Items

                Dim DisplayIndex As String = CType(r("DisplayIndex").Controls(1), TextBox).Text
                If Not IsNumeric(DisplayIndex) Then
                    DisplayIndex = radgTemplates.Items.Count
                End If

                Dim objT As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

                If IsNumeric(r("CustomFieldID").Text) AndAlso CInt(r("CustomFieldID").Text) > 0 AndAlso _
                    r("FieldType2").Text <> CType(r("FieldType").Controls(1), Telerik.Web.UI.RadComboBox).SelectedValue Then

                    objT.ChangeTemplateFieldType(r("CustomFieldID").Text, CType(r("FieldType").Controls(1), Telerik.Web.UI.RadComboBox).SelectedValue)

                End If

                If IsNumeric(r("CustomFieldID").Text) AndAlso CInt(r("CustomFieldID").Text) > 0 AndAlso _
                    r("FieldName2").Text <> CType(r("FieldName").Controls(1), TextBox).Text Then

                    objT.RenameTemplateField(r("CustomFieldID").Text, CType(r("FieldName").Controls(1), TextBox).Text)

                End If

                objT.SaveTemplateField( _
                    r("CustomFieldID").Text, _
                    cboTemplates.SelectedItem.Value, _
                    CType(r("FieldName").Controls(1), TextBox).Text, _
                    CType(r("FieldType").Controls(1), Telerik.Web.UI.RadComboBox).SelectedValue, _
                    DisplayIndex, _
                    CType(r("Search").Controls(1), CheckBox).Checked, _
                    CType(r("Required").Controls(1), CheckBox).Checked, _
                    CType(r("NewLine").Controls(1), CheckBox).Checked, _
                    CType(r("ShowInResults").Controls(1), CheckBox).Checked,
                    CType(r("LoadOnDemand").Controls(1), CheckBox).Checked,
                    CType(r("HelpNotes").Controls(1), TextBox).Text
                 )

            Next

            cboTemplates_SelectedIndexChanged(Nothing, Nothing)

            ShowMessage("Changes to template fields saved successfully...", MessageTypeEnum.Information)

        Catch ex As Exception

            Log.Error(ex)
            ShowMessage("An error occured: " & ex.Message, MessageTypeEnum.Error)

        End Try

    End Sub

    Private Sub radgTemplates_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles radgTemplates.ItemCommand

        Select Case e.CommandName

            Case "SaveFields"

                SaveFields()

            Case "DeleteTemplate"

                Dim CFID As String = cboTemplates.SelectedValue

                If Not String.IsNullOrWhiteSpace(CFID) AndAlso
                    IsNumeric(CFID) Then


                    If Not objCF.DeleteTemplate(CFID) Then
                        lblError.Text = objCF.ErrorMessage
                    Else

                        ShowMessage(String.Format("Successfully deleted the '{0}' template and all it's related data...", cboTemplates.Text), MessageTypeEnum.Information)

                        load_templates()
                        LoadTemplateFields() 'cboTemplates_SelectedIndexChanged(Nothing, Nothing)

                    End If


                Else

                    ShowMessage("Please select a template to delete", MessageTypeEnum.Warning)

                End If

            Case "Delete"

                Dim cmdDeleteCustomField As Button = e.Item.FindControl("cmdDeleteCustomField")

                If cmdDeleteCustomField IsNot Nothing AndAlso IsNumeric(cmdDeleteCustomField.CommandArgument) Then

                    Dim CFID As Long = cmdDeleteCustomField.CommandArgument

                    Dim sql As New System.Text.StringBuilder
                    sql.AppendLine("declare @CFID int, @CFTemplate varchar(255), @CFTemplateID int")
                    sql.AppendLine("")
                    sql.AppendLine("select @CFID = {0}")
                    sql.AppendLine("")
                    sql.AppendLine("select @CFTemplate = cft.TemplateName, @CFTemplateID = cft.TemplateID")
                    sql.AppendLine("from CustomField_Fields pt")
                    sql.AppendLine("	inner join CustomField_Templates cft on cft.TemplateID = pt.TemplateID")
                    sql.AppendLine("where cft.TemplateType = 'Grid' and pt.TemplateID = @CFID")
                    sql.AppendLine("")
                    sql.AppendLine("if exists (")
                    sql.AppendLine("	select * from sys.columns")
                    sql.AppendLine("	where name='CField_' + cast(@CFID as varchar)")
                    sql.AppendLine("		and object_id=object_id('CustomFields_Grid_' + cast(@CFTemplateID as varchar))")
                    sql.AppendLine("")
                    sql.AppendLine("begin")
                    sql.AppendLine("")
                    sql.AppendLine("	exec ('alter CustomFields_Grid_' + cast(@CFTemplateID as varchar) + ' drop column CField_' + cast(@CFID as varchar))")
                    sql.AppendLine("")
                    sql.AppendLine("end")

                    Dim gridSql As String = String.Format(sql.ToString, CFID)

                    sql = New System.Text.StringBuilder
                    sql.AppendFormat("DELETE FROM CustomField_ObjectData WHERE CustomFieldID = {0}", CFID)
                    sql.AppendLine()

                    sql.AppendFormat("DELETE FROM CustomField_Fields WHERE CustomFieldID = {0}", CFID)

                    If Not BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, sql.ToString) Then
                        ShowMessage(BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)
                    Else

                        BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, gridSql)

                        ShowMessage("Custom Field deleted successfully...", MessageTypeEnum.Information)
                        cboTemplates_SelectedIndexChanged(Nothing, Nothing)

                    End If

                End If

        End Select

    End Sub

    Private Sub radgTemplates_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles radgTemplates.ItemDataBound

        Try

            If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

                Dim dr As DataRowView = e.Item.DataItem
                Dim txtFieldName As TextBox = e.Item.FindControl("FieldNameTextBox")
                Dim cmdDataLookup As Button = e.Item.FindControl("cmdDataLookup")

                If Universal.CommonFunctions.Catchnull(dr("CustomFieldID"), 0) <= 0 Then
                    e.Item.BackColor = Drawing.Color.WhiteSmoke
                    If txtFieldName IsNot Nothing Then txtFieldName.Attributes("CustomFieldID") = "0"
                Else
                    If txtFieldName IsNot Nothing Then txtFieldName.Attributes("CustomFieldID") = dr("CustomFieldID")
                End If

                Dim FieldTypeRadComboBox As Telerik.Web.UI.RadComboBox = e.Item.FindControl("FieldTypeRadComboBox")
                If Not IsDBNull(dr("FieldType")) AndAlso _
                    FieldTypeRadComboBox IsNot Nothing AndAlso _
                    FieldTypeRadComboBox.Items.FindItemByValue(dr("FieldType"), True) IsNot Nothing Then

                    FieldTypeRadComboBox.SelectedValue = dr("FieldType")

                    Dim lookupTypes As New List(Of String)
                    lookupTypes.AddRange("Combo,ContactList,ContactsEntityList,ContactsLookup,ContactsEntityLookup,LocationIndexLookup,ProjectsLookup,UserLookup".Split(","))

                    cmdDataLookup.Visible = lookupTypes.Contains(dr("FieldType"))
                    cmdDataLookup.Attributes("FieldName") = Catchnull(dr("FieldName"), String.Empty)
                    cmdDataLookup.Attributes("HelpNotes") = Catchnull(dr("FieldType"), String.Empty)

                Else

                    cmdDataLookup.Visible = False

                End If

            End If

        Catch ex As Exception

            Log.Error(ex)

        End Try

    End Sub

    Protected Sub cmdUpdateMissingTemplateFields_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdateMissingTemplateFields.Click

        Dim objT As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)
        If objT.UpdateObjectsUsingTemplate Then
            ShowMessage("Clients with missing template fields updated successfully...", MessageTypeEnum.Information)
        Else
            ShowMessage(objT.ErrorMessage, MessageTypeEnum.Error)
        End If

    End Sub

    Protected Sub cmdDataLookup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try

            If Not String.IsNullOrWhiteSpace(sender.Attributes("FieldName")) Then
                Response.Redirect("~/LookupListsManager.aspx?LN=Custom%20Field%20Lookups&C=FieldName&CV=" & HttpUtility.UrlEncode(sender.Attributes("FieldName")))
            End If

        Catch ex As Exception
            Log.Error(ex)
        End Try

    End Sub

    Private Sub radgTemplates_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radgTemplates.NeedDataSource

        Try

            If Page.IsPostBack Then

                If ViewState("customfields") IsNot Nothing Then
                    radgTemplates.DataSource = ViewState("customfields")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub cboApplyTo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboApplyTo.SelectedIndexChanged

        If cboApplyTo.SelectedValue <> "" AndAlso cboTemplates.SelectedItem.Text <> "" Then

            Select Case cboApplyTo.SelectedValue

                Case "P" 'PROJECT

                    LoadObjects("tblCustomFields_Objects", "ObjectID", "Description", "P", "")

                Case "H" 'HEALTHCENTER

                    LoadObjects("tblCustomFields_Objects", "ObjectID", "Description", "H", "")

                Case "S" 'SCHOOLS

                    LoadObjects("tblCustomFields_Objects", "ObjectID", "Description", "S", "")

                Case "O" 'ORGANIZATIONS

                    LoadObjects("tblCustomFields_Objects", "ObjectID", "Description", "O", "")

                Case "C" 'COMMMUNITIES

                    LoadObjects("tblCustomFields_Objects", "ObjectID", "Description", "O", "")

                Case "G" 'GROUPS

                    LoadObjects("tblCustomFields_Objects", "ObjectID", "Description", "G", "")

                Case "HH" 'GROUPS

                    LoadObjects("tblCustomFields_Objects", "ObjectID", "Description", "HH", " AND Suffix = 1")

            End Select

            cmdApplyTemplateToExisting.Visible = True

        Else

            ucAppliesTo.AvailableOptions.Items.Clear()
            ucAppliesTo.SelectedOptions.Items.Clear()

        End If

    End Sub

    Private Sub LoadObjects(ByVal TableName As String, ByVal ValueColumn As String, ByVal DescriptionColumn As String, ByVal AutomatorType As String, ByVal Filter As String)

        With ucAppliesTo

            With .AvailableOptions
                .DataValueField = ValueColumn
                .DataTextField = DescriptionColumn
                .DataSource = DataLookup.Lookup(TableName, ValueColumn, DescriptionColumn, DescriptionColumn, ValueColumn & " NOT IN (SELECT AutomatorID FROM CustomField_Automation WHERE AutomatorType='" & AutomatorType & "' AND TemplateID = " & cboTemplates.SelectedItem.Value & ") " & Filter)
                .DataBind()
            End With

            With .SelectedOptions
                .DataValueField = ValueColumn
                .DataTextField = DescriptionColumn
                .DataSource = DataLookup.Lookup(TableName, ValueColumn, DescriptionColumn, DescriptionColumn, ValueColumn & " IN (SELECT AutomatorID FROM CustomField_Automation WHERE AutomatorType='" & AutomatorType & "' AND TemplateID = " & cboTemplates.SelectedItem.Value & ") " & Filter)
                .DataBind()
            End With

            cmdApplyTemplateToExisting.Enabled = .SelectedValues.Length > 0

        End With

    End Sub

    Protected Sub cmdSaveAppliesTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveAppliesTo.Click

        If cboTemplates.SelectedItem IsNot Nothing AndAlso cboApplyTo.SelectedValue <> "" AndAlso IsNumeric(cboTemplates.SelectedItem.Value) Then

            Dim s As String = Join(ucAppliesTo.SelectedValues, ",").TrimStart(",").TrimEnd(",").Replace(",,", ",")
            Dim a As String = Join(ucAppliesTo.AvailableValues, ",").TrimStart(",").TrimEnd(",").Replace(",,", ",")

            Dim sql As String = ""
            sql &= "DELETE FROM CustomField_Automation WHERE AutomatorID IN (0" & a & ") AND AutomatorType = '" & cboApplyTo.SelectedValue & "' AND TemplateID = " & cboTemplates.SelectedItem.Value & vbCrLf
            sql &= " " & vbCrLf
            sql &= "INSERT INTO CustomField_Automation (AutomatorID, AutomatorType, TemplateID, CreatedBy) " & vbCrLf
            sql &= "SELECT ID, '" & cboApplyTo.SelectedValue & "', " & cboTemplates.SelectedItem.Value & ", " & CookiesWrapper.thisUserID & vbCrLf
            sql &= "FROM (" & vbCrLf
            sql &= "     SELECT {0} AS ID " & vbCrLf
            sql &= "     FROM {1} Data " & vbCrLf
            sql &= "     WHERE {0} IN (0" & s & ") " & vbCrLf
            sql &= "          AND {0} NOT IN (" & vbCrLf
            sql &= "               SELECT AutomatorID " & vbCrLf
            sql &= "               FROM CustomField_Automation " & vbCrLf
            sql &= "               WHERE AutomatorType = '" & cboApplyTo.SelectedValue & "' " & vbCrLf
            sql &= "               AND TemplateID = " & cboTemplates.SelectedItem.Value & "" & vbCrLf
            sql &= "     ) " & vbCrLf
            sql &= ") Data" & vbCrLf

            Select Case cboApplyTo.SelectedValue

                Case "P" 'Project

                    sql = String.Format(sql, "ObjectID", "tblCustomFields_Objects")

                Case "H" 'Client Status

                    sql = String.Format(sql, "ObjectID", "tblCustomFields_Objects")

                Case "S" 'Client Types

                    sql = String.Format(sql, "ObjectID", "tblCustomFields_Objects")

                Case "O" 'Project Types

                    sql = String.Format(sql, "ObjectID", "tblCustomFields_Objects")

                Case "C" 'Project Types

                    sql = String.Format(sql, "ObjectID", "tblCustomFields_Objects")

                Case "G" 'Project Types

                    sql = String.Format(sql, "ObjectID", "tblCustomFields_Objects")

                Case "HH" 'Project Types

                    sql = String.Format(sql, "ObjectID", "tblCustomFields_Objects")

            End Select

            If BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, sql) Then

                ShowMessage(cboApplyTo.SelectedItem.Text & " settings updated successfully.", MessageTypeEnum.Information)
                cmdApplyTemplateToExisting.Enabled = ucAppliesTo.SelectedValues.Length > 0

            Else

                ShowMessage("Error: " & BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)

            End If

        End If

    End Sub

    Protected Sub cmdCancelTemplateRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelTemplateRename.Click

        txtRenameTemplate.Text = ""
        pnlRenameTemplate.Visible = False

    End Sub

    Protected Sub cmdRenameTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRenameTemplate.Click

        pnlRenameTemplate.Visible = True

    End Sub

    Protected Sub cmdUpdateTemplateRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdateTemplateRename.Click

        'TODO: If you rename a template, you actually need to
        '          also rename the View. This is fine, however
        '          in replicated scenarios, how will you handle
        '          this? The stored procedure will result in 
        '          creation of a second template. How will we 
        '          maintain this view/

        If objCF.RenameTemplate(cboTemplates.SelectedItem.Text, txtRenameTemplate.Text) Then

            With cboTemplates.SelectedItem

                .Text = txtRenameTemplate.Text
                '.Value = txtRenameTemplate.Text

            End With

            cboTemplates_SelectedIndexChanged(Nothing, Nothing)

            pnlRenameTemplate.Visible = False

            ShowMessage("Template renamed successfully...", MessageTypeEnum.Information)

        Else

            lblError.Text = "Error updating template name: " & objCF.ErrorMessage

        End If

    End Sub

    Protected Sub cmdDeleteOrphanTemplates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteOrphanTemplates.Click

        Dim sql As String = String.Empty

        sql &= "delete from CustomField_ObjectData where ObjectType='h' and ObjectID not in (select HealthCenterID from tblHealthCenters) " & vbCrLf
        sql &= "delete from CustomField_ObjectData where ObjectType='p' and ObjectID not in (select Project from tblProjects) " & vbCrLf
        sql &= "delete from CustomField_ObjectData where ObjectType='o' and ObjectID not in (select OrganizationID from tblOrganization) " & vbCrLf
        sql &= "delete from CustomField_ObjectData where ObjectType='c' and ObjectID not in (select CommunityID from tblCommunities) " & vbCrLf
        sql &= "delete from CustomField_ObjectData where ObjectType='s' and ObjectID not in (select SchoolID from tblSchools) " & vbCrLf
        sql &= "delete from CustomField_ObjectData where ObjectType='g' and ObjectID not in (select GroupID from tblGroups) " & vbCrLf
        sql &= "delete from CustomField_ObjectData where ObjectType='hh' and ObjectID not in (select BeneficiaryID from tblBeneficiaries) " & vbCrLf

        sql &= "delete from CustomField_ObjectTemplates where ObjectType='h' and ObjectID not in (select HealthCenterID from tblHealthCenters) " & vbCrLf
        sql &= "delete from CustomField_ObjectTemplates where ObjectType='p' and ObjectID not in (select Project from tblProjects) " & vbCrLf
        sql &= "delete from CustomField_ObjectTemplates where ObjectType='o' and ObjectID not in (select OrganizationID from tblOrganization) " & vbCrLf
        sql &= "delete from CustomField_ObjectTemplates where ObjectType='c' and ObjectID not in (select CommunityID from tblCommunities) " & vbCrLf
        sql &= "delete from CustomField_ObjectTemplates where ObjectType='s' and ObjectID not in (select SchoolID from tblSchools) " & vbCrLf
        sql &= "delete from CustomField_ObjectTemplates where ObjectType='g' and ObjectID not in (select GroupID from tblGroups) " & vbCrLf
        sql &= "delete from CustomField_ObjectTemplates where ObjectType='hh' and ObjectID not in (select BeneficiaryID from tblBeneficiaries) " & vbCrLf

        If BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, sql) Then
            ShowMessage("Orphan templates successfully deleted...", MessageTypeEnum.Information)
        Else
            ShowMessage(BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)
        End If

    End Sub

    Protected Sub cmdManageTemplate_Click(sender As Object, e As EventArgs) Handles cmdManageTemplate.Click

        If IsNumeric(cboTemplates.SelectedValue) Then
            Response.Redirect("~/CustomFieldTemplateManager.aspx?id=" & cboTemplates.SelectedValue)
        End If

    End Sub

    Protected Sub cmdRefreshCFView_Projects_Click(sender As Object, e As EventArgs) Handles cmdRefreshCFView_Projects.Click

        If BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, "Create_Projects_CustomFieldsMatrix") Then
            ShowMessage("Successfully create the projects custom field matrix...", MessageTypeEnum.Information)
        Else
            ShowMessage(BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)
        End If

    End Sub

    Protected Sub cmdRefreshCFView_Documents_Click(sender As Object, e As EventArgs) Handles cmdRefreshCFView_Documents.Click

        If BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, "Create_Documents_CustomFieldsMatrix") Then
            ShowMessage("Successfully create the documents custom field matrix...", MessageTypeEnum.Information)
        Else
            ShowMessage(BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)
        End If

    End Sub

    Protected Sub cmdRefreshCFView_Contacts_Click(sender As Object, e As EventArgs) Handles cmdRefreshCFView_Contacts.Click

        If BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, "Create_Contacts_CustomFieldsMatrix") Then
            ShowMessage("Successfully create the contacts custom field matrix...", MessageTypeEnum.Information)
        Else
            ShowMessage(BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)
        End If

    End Sub

    Protected Sub cmdApplyTemplateToExisting_Click(sender As Object, e As EventArgs) Handles cmdApplyTemplateToExisting.Click

        If cboApplyTo.SelectedValue <> "" AndAlso cboTemplates.SelectedItem.Text <> "" Then

            Dim sql As String = String.Empty, ObjectType As String = String.Empty, TableName As String = String.Empty
            Dim FilterColumn As String = String.Empty, FilterValues As String = String.Empty, TableIDColumn As String = String.Empty

            FilterValues = Join(ucAppliesTo.SelectedValues, ",")

            Select Case cboApplyTo.SelectedValue

                Case "P" 'Project 

                    ObjectType = "P"
                    TableName = "tblCustomFields_Objects"
                    FilterColumn = "ObjectID"
                    TableIDColumn = "ObjectID"

                Case "H" 'HealthCenter

                    ObjectType = "H"
                    TableName = "tblCustomFields_Objects"
                    FilterColumn = "ObjectID"
                    TableIDColumn = "ObjectID"

                Case "S" 'Schools

                    ObjectType = "S"
                    TableName = "tblCustomFields_Objects"
                    FilterColumn = "ObjectID"
                    TableIDColumn = "ObjectID"

                Case "O" 'Organizations

                    ObjectType = "O"
                    TableName = "tblCustomFields_Objects"
                    FilterColumn = "ObjectID"
                    TableIDColumn = "ObjectID"

                Case "C" 'Communities

                    ObjectType = "C"
                    TableName = "tblCustomFields_Objects"
                    FilterColumn = "ObjectID"
                    TableIDColumn = "ObjectID"

                Case "G" 'Project Types

                    ObjectType = "G"
                    TableName = "tblCustomFields_Objects"
                    FilterColumn = "ObjectID"
                    TableIDColumn = "ObjectID"

                Case "HH" 'Project Types

                    ObjectType = "HH"
                    TableName = "tblCustomFields_Objects"
                    FilterColumn = "ObjectID"
                    TableIDColumn = "ObjectID"

            End Select

            sql &= "DECLARE  " & vbCrLf
            sql &= "	@TemplateID varchar(1000), --name of the template to bulk assign. NULL for all templates " & vbCrLf
            sql &= "	@ObjectType char(1)			 --entity type. must be one of D, P or C " & vbCrLf
            sql &= vbCrLf
            sql &= "SELECT  " & vbCrLf
            sql &= "	@TemplateID=" & cboTemplates.SelectedItem.Value & ",  " & vbCrLf
            sql &= "	@ObjectType = '" & ObjectType.ToUpper.Trim & "' " & vbCrLf
            sql &= vbCrLf
            sql &= "INSERT INTO CustomField_ObjectTemplates(ObjectID, TemplateID, ObjectType, CreatedBy) " & vbCrLf
            sql &= "SELECT DISTINCT Data." & TableIDColumn & ", TemplateID, @ObjectType AS ObjectType, " & CookiesWrapper.thisUserID & vbCrLf
            sql &= "FROM " & TableName & " Data " & vbCrLf
            sql &= "	CROSS JOIN CustomField_Fields PT " & vbCrLf
            sql &= "WHERE  " & vbCrLf
            sql &= "	PT.TemplateID = ISNULL(@TemplateID, PT.TemplateID) " & vbCrLf
            sql &= "	AND Data." & FilterColumn.Trim & " IN (" & FilterValues & ") " & vbCrLf
            sql &= "	AND Data." & TableIDColumn & " IS NOT NULL " & vbCrLf
            sql &= "	AND NOT EXISTS ( " & vbCrLf
            sql &= "		SELECT * FROM CustomField_ObjectTemplates U  " & vbCrLf
            sql &= "		WHERE U.TemplateID=PT.TemplateID " & vbCrLf
            sql &= "			AND U.ObjectType=@ObjectType " & vbCrLf
            sql &= "			AND U.ObjectID=Data." & TableIDColumn & " " & vbCrLf
            sql &= "	)  " & vbCrLf
            sql &= vbCrLf
            sql &= "SELECT @@ROWCOUNT " & vbCrLf

            Log.Debug(sql)

            Dim cmd As Common.DbCommand = db.GetSqlStringCommand(sql)
            cmd.CommandTimeout = 0

            Dim result As Object = db.ExecuteScalar(cmd)

            If result IsNot Nothing Then

                lblError.Text = String.Format("{0} settings updated successfully - {1} items affected...", cboApplyTo.SelectedItem.Text, result.ToString)

            Else

                ShowMessage("Error: " & BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)
                Log.Error(BusinessLogic.CustomFields.DataHelper.ErrorMessage & vbCrLf & sql)

            End If

        End If

    End Sub

    Protected Sub cmdClearAutoApplyRules_Click(sender As Object, e As EventArgs) Handles cmdClearAutoApplyRules.Click

        If objCF.RemoveAllAutoApplyRules(cboTemplates.Text) Then
            ShowMessage(String.Format("Successfully cleared all automatic application rules for the '{0}' template...", cboTemplates.Text), MessageTypeEnum.Information)
        Else
            ShowMessage(BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)
        End If

    End Sub

    Protected Sub cmdRemoveAllUsed_Click(sender As Object, e As EventArgs) Handles cmdRemoveAllUsed.Click

        Dim CFID As String = cboTemplates.SelectedValue

        If Not String.IsNullOrWhiteSpace(CFID) AndAlso
            IsNumeric(CFID) Then

            If objCF.RemoveTemplateFromAllObjects(cboTemplates.Text) Then
                ShowMessage(String.Format("Successfully removed the '{0}' template from all items that were using it...", cboTemplates.Text), MessageTypeEnum.Information)
            Else
                ShowMessage(BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)
            End If

        Else

            ShowMessage("Please select a template first...", MessageTypeEnum.Error)

        End If

    End Sub

End Class

Public Class CustomFieldValidationResult

    Public IsUnique As Boolean = False
    Public Message As String = String.Empty

End Class