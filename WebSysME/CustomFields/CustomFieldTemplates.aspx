<%@ Page Language="vb" AutoEventWireup="false" Title="Custom Field Templates" MasterPageFile="~/Site.Master" CodeBehind="CustomFieldTemplates.aspx.vb"
    Inherits="WebSysME.CustomFieldTemplates" SmartNavigation="true" %>

<%@ Register Src="~/Controls/ComplementaryListboxes.ascx" TagName="ComplementaryListboxes"
    TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%;margin-right: 1%">
    <style type="text/css">
        body {
    font-family: "Segoe UI Web Regular","Segoe UI","Helvetica Neue",Arial;
    font-size: 11px;
    font-weight: 100;
    line-height: 16px;
    margin: 0;
}

.PageHeader, .PageHeader span {
    background-color: #0072C6;
    background-image: none;
    color: #FFF;
    font-family: "Segoe UI Web Light","Segoe UI Light","Segoe UI Web Regular","Segoe UI","Helvetica Neue",Arial!important;
    font-size: 16px!important;
    font-weight: 100!important;
    padding: 6px;
    vertical-align: middle;
}

    .PageHeader img, span label {
        vertical-align: middle;
    }

.PageHeader2 {
    background-color: #CCC;
    background-image: none;
    border-bottom: 1px solid #696969;
    border-top: 1px solid #696969;
}

.PageHeader3 {
    background-color: #DCDCDC;
    background-image: none;
    border-bottom: 1px solid #696969;
    border-top: 1px solid #696969;
}

.HiddenControl {
    display: none;
    height: 0;
    visibility: hidden;
    width: 0;
}

input:hover, input:active {
    background-color: #ff9;
    background-image: none;
    border-style: 1 solid;
}
    </style>
    <script language="javascript" type="text/javascript">

        function EnsureUniqueFieldName(source, args) {

            var FieldName = args.Value;
            var CustomFieldID = $telerik.$("#" + source.controltovalidate).attr("CustomFieldID");

            $telerik.$.ajax({
                type: "POST",
                async: false,
                timeout: 500,
                contentType: "application/json",
                dataType: "json",
                url: "CustomFieldTemplates.aspx/IsFieldNameUnique",
                data: '{"FieldName":"' + FieldName + '","CustomFieldID":"' + CustomFieldID + '"}',
                success: function (result) {
                    args.IsValid = result.d.IsUnique;
                    source.errormessage = "The field name '" + FieldName + "' must be unique and must not be one of the reserved words: " + result.d.Message;
                    // Update validation summary for chosen validation group
                    ValidatorUpdateIsValid();
                    ValidationSummaryOnSubmit('');
                }
            });

        }

    </script>
    <table width="100%">
        <tr>
            <td class="PageHeader">Templates
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
            </td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="2" width="100%" bgcolor="white">
        <tr>
            <td style="width: 14%">Template
            </td>
            <td colspan="2">&nbsp;<table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td>
                        <asp:DropDownList ID="cboTemplates" runat="server" AutoPostBack="True" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td align="right">&nbsp;
                            <asp:LinkButton ID="cmdManageTemplate" runat="server">Manage Permissions</asp:LinkButton>&nbsp;|
                            <asp:LinkButton ID="cmdRenameTemplate" runat="server">Rename</asp:LinkButton>
                    </td>
                </tr>
            </table>
                <asp:Panel ID="pnlRenameTemplate" runat="server" Visible="False" Width="100%">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td>Current name:
                            </td>
                            <td style="width: 85%">
                                <asp:Label ID="lblTemplateName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Rename to:
                            </td>
                            <td>
                                <asp:TextBox ID="txtRenameTemplate" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:LinkButton ID="cmdUpdateTemplateRename" runat="server">Update</asp:LinkButton>&nbsp;
                                <asp:LinkButton ID="cmdCancelTemplateRename" runat="server">Cancel</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="PageHeader3" colspan="2">Apply this template to all newly created:</td>
            <td class="PageHeader3" colspan="1">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:CheckBoxList ID="chkApplyToAllNew" runat="server"
                    RepeatDirection="Horizontal" Width="60%">
                    <asp:ListItem Value="ALL-C">Contacts</asp:ListItem>
                    <asp:ListItem Value="ALL-D">Documents</asp:ListItem>
                    <asp:ListItem Value="ALL-P">Projects</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="PageHeader3" colspan="2">Current Fields
            </td>
            <td class="PageHeader3" colspan="1"></td>
        </tr>
        <tr>
            <td colspan="3">
                <telerik:RadGrid ID="radgTemplates" runat="server"
                    AllowSorting="True">
                    <ClientSettings EnableRowHoverStyle="True">
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" ShowFooter="True">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="CustomFieldID" Display="False" UniqueName="CustomFieldID">
                                <ColumnValidationSettings>
                                    <%--<ModelErrorMessage Text=""></ModelErrorMessage>--%>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FieldName" Display="False" UniqueName="FieldName2">
                                <ColumnValidationSettings>
                                    <%--<ModelErrorMessage Text=""></ModelErrorMessage>--%>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FieldType" Display="False" UniqueName="FieldType2">
                                <ColumnValidationSettings>
                                    <%--<ModelErrorMessage Text=""></ModelErrorMessage>--%>
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="FieldName" HeaderText="Field Name" UniqueName="FieldName">
                                <ItemTemplate>
                                    <asp:TextBox ID="FieldNameTextBox" runat="server" Text='<%# Bind("FieldName") %>'
                                        Width="95%" CssClass="form-control"></asp:TextBox>
                                    <asp:CustomValidator ID="cvalUniqueFieldName" ControlToValidate="FieldNameTextBox"
                                        runat="server" ClientValidationFunction="EnsureUniqueFieldName" Text="*" ErrorMessage="The field name must be unique and must not be one of the reserved words."></asp:CustomValidator>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="cmdSaveFields" runat="server" Text="Save" CommandName="SaveFields" Font-Bold="True" ForeColor="DarkBlue" CssClass="btn btn-default"/>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="FieldType" HeaderText="Field Type" UniqueName="FieldType">
                                <ItemTemplate>
                                    <telerik:RadComboBox ID="FieldTypeRadComboBox" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Checkbox" Value="Checkbox" />
                                            <telerik:RadComboBoxItem runat="server" Text="Combo" Value="Combo" />
                                            <telerik:RadComboBoxItem runat="server" Text="Country" Value="Country" />
                                            <telerik:RadComboBoxItem runat="server" Text="Contacts List" Value="ContactList" />
                                            <telerik:RadComboBoxItem runat="server" Text="Currency" Value="Currency" />
                                            <telerik:RadComboBoxItem runat="server" Text="Custom Data Lookup" Value="CustomDataLookup" />
                                            <telerik:RadComboBoxItem runat="server" Text="Entity List" Value="ContactsEntityList" />
                                            <telerik:RadComboBoxItem runat="server" Text="Contacts Lookup" Value="ContactsLookup" />
                                            <telerik:RadComboBoxItem runat="server" Text="Documents Checklist" Value="DocumentsChecklist" />
                                            <telerik:RadComboBoxItem runat="server" Text="Documents Table" Value="DocumentsTable" />
                                            <telerik:RadComboBoxItem runat="server" Text="Entity Lookup" Value="ContactsEntityLookup" />
                                            <telerik:RadComboBoxItem runat="server" Text="Freeform Text" Value="FreeText" />
                                            <telerik:RadComboBoxItem runat="server" Text="Follow Up Date" Value="FollowUpDate" />
                                            <telerik:RadComboBoxItem runat="server" Text="Location Index Lookup" Value="LocationIndexLookup" />
                                            <telerik:RadComboBoxItem runat="server" Text="Date" Value="Date" />
                                            <telerik:RadComboBoxItem runat="server" Text="DateTime" Value="DateTime" />
                                            <telerik:RadComboBoxItem runat="server" Text="Duplicate" Value="Duplicate" />
                                            <telerik:RadComboBoxItem runat="server" Text="Multi Valued List" Value="MultiValue" />
                                            <telerik:RadComboBoxItem runat="server" Text="Notes" Value="Notes" />
                                            <telerik:RadComboBoxItem runat="server" Text="Numeric" Value="Numeric" />
                                            <telerik:RadComboBoxItem runat="server" Text="Money" Value="Money" />
                                            <telerik:RadComboBoxItem runat="server" Text="Projects Lookup" Value="ProjectsLookup" />
                                            <telerik:RadComboBoxItem runat="server" Text="Rating" Value="Rating" />
                                            <telerik:RadComboBoxItem runat="server" Text="Text" Value="Text" />
                                            <telerik:RadComboBoxItem runat="server" Text="Time" Value="Time" />
                                            <telerik:RadComboBoxItem runat="server" Text="User Lookup" Value="UserLookup" />
                                            <telerik:RadComboBoxItem runat="server" Text="" Value=""></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:Button ID="cmdDataLookup" runat="server" Text="Lookup" OnClick="cmdDataLookup_Click" CssClass="btn btn-default"/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="Search" HeaderText="Show in Search" UniqueName="Search">
                                <ItemTemplate>
                                    <asp:CheckBox ID="SearchTextBox" runat="server" Checked='<%# Bind("Search") %>'></asp:CheckBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="Required" HeaderText="Required" UniqueName="Required">
                                <ItemTemplate>
                                    <asp:CheckBox ID="RequiredCheckBox" runat="server" Checked='<%# Bind("Required") %>'></asp:CheckBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="ShowInResults" HeaderText="ShowInResults"
                                UniqueName="ShowInResults">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ShowInResultsCheckBox" runat="server" Checked='<%# Bind("ShowInResults") %>'></asp:CheckBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="NewLine" HeaderText="NewLine" UniqueName="NewLine">
                                <ItemTemplate>
                                    <asp:CheckBox ID="NewLineCheckBox" runat="server" Checked='<%# Bind("NewLine") %>'></asp:CheckBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="LoadOnDemand" HeaderText="LoadOnDemand" UniqueName="LoadOnDemand">
                                <ItemTemplate>
                                    <asp:CheckBox ID="LoadOnDemandCheckBox" runat="server" Checked='<%# Bind("LoadOnDemand")%>'></asp:CheckBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn DataField="DisplayIndex" HeaderText="Order" UniqueName="DisplayIndex">
                                <ItemTemplate>
                                    <asp:TextBox ID="DisplayIndexTextBox" runat="server" Text='<%# Bind("DisplayIndex")%>'
                                        Columns="5" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="HelpNotes" HeaderText="Help Notes" UniqueName="HelpNotes">
                                <ItemTemplate>
                                    <asp:TextBox ID="HelpNotesTextBox" runat="server" Text='<%# Bind("HelpNotes")%>'
                                        Width="95%" CssClass="form-control"></asp:TextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="DeleteCustomField" HeaderText="Delete">
                                <FooterTemplate>
                                    <asp:Button ID="cmdDeleteALLFields" runat="server" CommandName="DeleteTemplate" CssClass="btn btn-default" Font-Bold="True" ForeColor="DarkBlue" OnClientClick="return confirm(&quot;Are you sure you want to delete all the fields in this template?\nNOTE: This will also delete the template.&quot;)" Text="Delete ALL Fields" />
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="cmdDeleteCustomField" runat="server" Text="Delete Field" CausesValidation="false" CssClass="btn btn-default"
                                        CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this custom field?');"
                                        CommandArgument='<%# Eval("CustomFieldID")%>' Visible='<%# Eval("CustomFieldID") > 0%>'></asp:Button>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>

                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                        </EditFormSettings>

                    </MasterTableView>

                    <FilterMenu EnableImageSprites="False"></FilterMenu>

                </telerik:RadGrid>
            </td>
        </tr>
        <tr>
            <td></td>
            <td align="right" colspan="2"></td>
        </tr>
        <tr>
            <td class="PageHeader2" colspan="3">New Template
            </td>
        </tr>
        <tr>
            <td>New Template
            </td>
            <td colspan="2"><br />
                <asp:TextBox ID="txtTemplate" runat="server" CssClass="form-control"></asp:TextBox>
                &nbsp;<asp:DropDownList ID="cboTemplateType" runat="server" CssClass="form-control">
                    <asp:ListItem Selected="True">Standard</asp:ListItem>
                    <asp:ListItem>Grid</asp:ListItem>
                </asp:DropDownList>
                &nbsp;<asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
            <td colspan="2"></td>
        </tr>
        <tr>
            <td class="PageHeader2" colspan="3">Update 
                existing Clients, Documents and Projects with changes to Template Fields
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="cmdUpdateMissingTemplateFields" runat="server" Text="Update" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td colspan="3"></td>
        </tr>
        <tr>
            <td class="PageHeader2" colspan="3">This Template must be automatically applied to:
            </td>
        </tr>
        <tr>
            <td colspan="3"></td>
        </tr>
        <tr>
            <td>Item:
            </td>
            <td>
                <asp:DropDownList ID="cboApplyTo" runat="server" AutoPostBack="True" CssClass="form-control">
                    <asp:ListItem Value="P">Projects</asp:ListItem>
                    <asp:ListItem Value="H">HealthCenter</asp:ListItem>
                    <asp:ListItem Value="S">Schools</asp:ListItem>
                    <asp:ListItem Value="O">Organization</asp:ListItem>
                    <asp:ListItem Value="C">Community</asp:ListItem>
                    <asp:ListItem Value="G">Groups</asp:ListItem>
                    <asp:ListItem Value="HH">Households</asp:ListItem>
                    <asp:ListItem Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="right">
                <asp:LinkButton ID="cmdClearAutoApplyRules" runat="server" OnClientClick="return confirm(&quot;Are you sure you would like to remove all previously configured rules for when this template must be automatically applied?&quot;)" Visible="False">Clear ALL Rules</asp:LinkButton>&nbsp;<asp:Label ID="lblRules" runat="server" Text="|" Visible="False"></asp:Label>
                &nbsp;<asp:LinkButton ID="cmdRemoveAllUsed" runat="server" OnClientClick="return confirm(&quot;Are you sure you want to remove this template from all the items that it has been used? This will also delete any information previously captured into the fields. This operation cannot be undone.&quot;)" Visible="False">Remove from ALL Items</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="2">
                <uc1:ComplementaryListboxes ID="ucAppliesTo" runat="server" AvailableOptionsCaption="Available"
                    SelectedOptionsCaption="Is Automatically Applied On" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="cmdSaveAppliesTo" runat="server" Text="Save Changes" CssClass="btn btn-default"/>
            </td>
            <td align="right">&nbsp;<asp:Button ID="cmdApplyTemplateToExisting" runat="server" Text="Apply Template to Existing Entries"
                Visible="False" Enabled="False" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td colspan="3"></td>
        </tr>
        <tr>
            <td class="PageHeader2" colspan="3">Cleanup
            </td>
        </tr>
        <tr>
            <td colspan="3">To avoid data loss in a replicated server environment, please make sure that you only run this command AFTER a complete syncronisation across all the servers.</td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="cmdDeleteOrphanTemplates" runat="server" Text="Delete Orphan Templates" CssClass="btn btn-default" />
            </td>
        </tr>
        <tr>
            <td class="PageHeader2" colspan="3">Custom Field Views
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="cmdRefreshCFView_Projects" runat="server" Text="Refresh Projects View" CssClass="btn btn-default"/>
                &nbsp;
                <asp:Button ID="cmdRefreshCFView_Contacts" runat="server" Text="Refresh Contacts View" CssClass="btn btn-default"/>
                &nbsp;
                <asp:Button ID="cmdRefreshCFView_Documents" runat="server" Text="Refresh Documents View" CssClass="btn btn-default"/>
            </td>
        </tr>
    </table>
    <asp:TextBox ID="txtNewFields" runat="server" TextMode="MultiLine" CssClass="HiddenControl"></asp:TextBox>
    <asp:TextBox ID="txtTemplateFields" runat="server" TextMode="MultiLine" CssClass="HiddenControl"></asp:TextBox>
    <asp:TextBox ID="txtTemplateNames" runat="server" TextMode="MultiLine" CssClass="HiddenControl"></asp:TextBox>
    </div>
</asp:Content>
