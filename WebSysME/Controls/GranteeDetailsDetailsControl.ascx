<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GranteeDetailsDetailsControl.ascx.vb" Inherits="WebSysME.GranteeDetailsDetailsControl" %>

<script type="text/javascript">
 
        function isNumberKey(evt, obj) {
 
            var charCode = (evt.which) ? evt.which : event.keyCode
            var value = obj.value;
            var dotcontains = value.indexOf(".") != -1;
            if (dotcontains)
                if (charCode == 46) return false;
            if (charCode == 46) return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
</script>
<table cellpadding="3" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Grantee Details</h4></td> 
	</tr>
    <tr> 
		<td >Parent Donor</td> 
        	<td ><asp:TextBox id="txtParentDonor" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox> </td> 
    </tr>
    <tr> 
		<td >Project Manager</td> 
        	<td ><asp:TextBox id="txtProjectManager" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox> </td> 
    </tr>
	<tr> 
		<td >Partner Name</td> 
        	<td ><asp:dropdownlist id="cboPartner" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
    </tr>
    <tr>
		<td >Contact Person</td> 
        	<td ><asp:dropdownlist id="cboContactPerson" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
    <tr>         
		<td >Project Title</td> 
        	<td ><asp:textbox id="txtProjectTitle" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Total Grant Value</td> 
        	<td ><asp:textbox id="txtTotalGrantValue" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event,this)"></asp:textbox> </td>  
    </tr> 
	<tr> 
		<td >Partnership Type</td> 
        	<td ><asp:dropdownlist id="cboPartnershipType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Project Status</td> 
        	<td ><asp:dropdownlist id="cboProjectStatus" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Project Duration</td> 
        	<td ><asp:textbox id="txtProjectDuration" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
	<tr> 
		<td >Project Start Date</td> 
        	<td ><telerik:RadDatePicker ID="radStartDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker> </td> 
		<td >Project End Date</td> 
        	<td ><telerik:RadDatePicker ID="radEndDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker> </td> 
	</tr> 
	<tr>
        <td >KCP</td> 
        	<td ><asp:ListBox id="lstKeyChangePromise" runat="server" CssClass="form-control" Enabled="false" SelectionMode="Multiple"></asp:ListBox> </td> 
		<td >Key Deliverables</td> 
        	<td ><asp:textbox id="txtProjectDeliverables" runat="server" TextMode="MultiLine" Rows="4" Columns="30" CssClass="form-control"></asp:textbox> </td> 
	</tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><h4>Disbursement Schedule </h4></td>
    </tr> 
    <tr>
        <td>Date</td>
        <td><telerik:RadDatePicker ID="radExpectedDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        <td>Amount</td>
        <td><asp:TextBox runat="server" ID="txtAmount" CssClass="form-control" onkeypress="return isNumberKey(event,this)"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radSchedule" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="DisbursementsScheduleID" UniqueName="DisbursementsScheduleID" HeaderText="DisbursementsScheduleID" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ExpectedDate" UniqueName="ExpectedDate" HeaderText="ExpectedDate">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Amount" UniqueName="Amount" HeaderText="Amount">
                            </telerik:GridBoundColumn>
                             <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "DisbursementsScheduleID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove this entry?')"
                                    ToolTip="Click to remove " />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                  
                        </Columns>
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
	<tr> 
		<td >Number Of Reports</td> 
        	<td ><asp:textbox id="txtNumberOfReports" runat="server" CssClass="form-control" TextMode="Number"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Extension Granted</td> 
        	<td ><asp:checkbox id="chkExtensionGranted" runat="server" AutoPostBack="true" ></asp:checkbox> </td>
	</tr>  
	<tr> 
		<td >Extension Type</td> 
        	<td ><asp:dropdownlist id="cboExtensionType" runat="server" Enabled="false" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Reason For Extension</td> 
        	<td ><asp:textbox id="txtReasonForExtension" runat="server" Enabled="false" CssClass="form-control"></asp:textbox> </td>
    <//tr>
    <tr>
        <td>
            Partnership Agreements and Reports
        </td>
        <td>
            <asp:FileUpload ID="fuUpload" runat="server" />
        </td>
    </tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtGranteeID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
            <asp:TextBox id="txtGrantDetailID" runat="server" CssClass="HiddenControl"></asp:TextBox>
		</td> 
	</tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><h4>Attachments</h4></td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radAttachments" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="FileID" UniqueName="FileID" HeaderText="FileID" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FilePath" UniqueName="FilePath" HeaderText="FilePath" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Title" UniqueName="Title" HeaderText="Title">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="PushButton" Text="Download" UniqueName="column"
                                CommandName="Download" ButtonCssClass="btn btn-default">
                            </telerik:GridButtonColumn>                   
                        </Columns>
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr> 
</table> 
<table style="margin-left:2%;width:90%">
    <tr><td>&nbsp;</td></tr>
    <tr>
        <td><h4>Contract Reporting Dates</h4></td>
    </tr>
    <tr>
        <td>Name Of Report</td>
        <td>Type Of Report</td>
        <td>Date Expected</td>
        <td>Person Responsible</td>
    </tr>
    <tr>
        <td><asp:TextBox ID="txtReportName" runat="server" CssClass="form-control"></asp:TextBox></td>
        <td><asp:DropDownList ID="cboTypeOfReport" runat="server" CssClass="form-control"></asp:DropDownList></td>
        <td><telerik:RadDatePicker ID="radExpectedDate2" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar4" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput4" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        <td><asp:DropDownList runat="server" ID="cboPersonResponsible" CssClass="form-control"></asp:DropDownList></td>
    </tr>
    <tr>
        <td><asp:Button ID="btnSave" runat="server" Text="Add" CssClass="btn btn-default"/></td>
    </tr>
    <tr>
        <td colspan="4">
            
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" CssClass="div-container no-bg" LoadingPanelID="RadAjaxLoadingPanel1">
            <telerik:RadGrid ID="radReports" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="90%" AutoGenerateColumns="False" >
                    <MasterTableView  AlternatingItemStyle-BackColor="#66ccff" EditMode="InPlace" DataKeyNames="ReportDateID">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="NameOfReport" SortExpression="NameOfReport">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "NameOfReport")%>
                             </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="NameOfReportTextBox" runat="server" Enabled="false" Width="100px" Text='<%# Bind("NameOfReport")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="TypeOfReport" SortExpression="TypeOfReport">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "TypeOfReport")%>
                             </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList  ID="cboTypeOfReport" runat="server" Enabled="false" Width="100px" >
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ExpectedDate" SortExpression="ExpectedDate">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "ExpectedDate")%>
                             </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="ExpectedDateTextBox" runat="server" Enabled="false" Width="100px" Text='<%# Bind("ExpectedDate")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="SubmissionStatus" SortExpression="SubmissionStatus">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "SubmissionStatus")%>
                             </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="cboSubmissionStatus" runat="server" Width="100px" >
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Comments" SortExpression="Comments">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Comments")%>
                             </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="CommentsTextBox" runat="server" TextMode="MultiLine" Columns="30" Rows="5" Width="100px" Text='<%# Bind("Comments")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="ResponsiblePerson" SortExpression="SubmissionStatus">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "StaffResponsible")%>
                             </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="cboResponsiblePerson1" runat="server" Width="100px" Enabled="false">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="DateSubmitted" SortExpression="DateSubmitted">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "DateSubmitted")%>
                             </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReportDateID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove this entry?')"
                                    ToolTip="Click to remove " />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                            
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UpdateText="Update" CancelText="Cancel"
                        EditText="Edit">
                    </telerik:GridEditCommandColumn>                  
                        </Columns>
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
    </telerik:RadAjaxPanel>
        </td>
    </tr>
</table>
