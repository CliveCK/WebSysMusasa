<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GrantDetailsDetailsControl.ascx.vb" Inherits="WebSysME.GrantDetailsDetailsControl" %>
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
<table cellpadding="3" cellspacing="0" border="0" style="margin-left:2%;width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Grant Management Details</h4></td> 
	</tr> 
	<tr> 
		<td >Project</td> 
        	<td ><asp:dropdownlist id="cboProject" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td> 
	</tr> 
    <tr>
        <td>Project Manager</td>
        <td><asp:TextBox ID="txtProjectManager" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox></td>
    </tr>
	<tr> 
		<td >Donor</td> 
        	<td ><asp:dropdownlist id="cboDonor" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Relevant KCP</td> 
        	<td ><asp:ListBox id="lstKeyChangePromise" runat="server" CssClass="form-control"></asp:ListBox> </td> 
	</tr> 
	<tr> 
		<td >Contract Currency</td> 
        	<td ><asp:dropdownlist id="cboContractCurrency" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Contract Value In Currency</td> 
        	<td ><asp:textbox id="txtContractValueInCurrency" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event,this)"></asp:textbox> </td> 
	</tr> 
    <tr> 
		<td >Contract Value In USD</td> 
        	<td ><asp:textbox id="txtContractValueInUSD" runat="server" CssClass="form-control" Enabled="false" ></asp:textbox> </td> 
		<td >Contract Value In GBP</td> 
        	<td ><asp:textbox id="txtContractValueInGBP" runat="server" CssClass="form-control" Enabled="false" ></asp:textbox> </td> 
	</tr>
    <tr> 
		<td >Total Program Costs</td> 
        	<td ><asp:textbox id="txtTotalProjectCosts" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event,this)"></asp:textbox> </td> 
		<td >Support Costs</td> 
        	<td ><asp:textbox id="txtTotalAdminCosts" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event,this)"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >VAT Info</td> 
        	<td ><asp:textbox id="txtVATInfo" runat="server" CssClass="form-control"></asp:textbox> </td>  
		<td >Contract Duration In Months</td> 
        	<td ><asp:textbox id="txtContractDurationInMonths" runat="server" CssClass="form-control" TextMode="Number"></asp:textbox> </td> 
	</tr>      
	<tr> 
        <td >Contract Start Date</td> 
        	<td ><telerik:RadDatePicker ID="radStartDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
		<td >Contract End Date</td> 
        	<td ><telerik:RadDatePicker ID="radEndDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
    </tr>
    <tr>
        <td>Attached Contract Approval/<br />Contract Addendum</td>
        <td colspan="3">
            <asp:FileUpload ID="fuUpload" runat="server" />
        </td>
    </tr>
    <tr> 
		<td >Extension Granted</td> 
        	<td ><asp:DropDownList id="cboExtensionGranted" runat="server" CssClass="form-control" AutoPostBack="true" >
                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                    <asp:ListItem Text="YES" Value="1"></asp:ListItem>
        	     </asp:DropDownList> </td>
	</tr> 
    <tr>
		<td >Nature Of Extension</td> 
        	<td ><asp:textbox id="txtNatureOfExtension" runat="server" CssClass="form-control" Enabled="false"></asp:textbox> </td> 
		<td >New Contract End Date</td> 
        	<td ><telerik:RadDatePicker ID="radContractEndDate" runat="server" MinDate="1900-01-01" Enabled="false" 
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr>		
	
	<tr> 
		<td >Number Of Reports</td> 
        	<td ><asp:textbox id="txtNumberOfReports" runat="server" CssClass="form-control" TextMode="Number"></asp:textbox> </td> 
        <td></td>		 
        <td></td>
	</tr> 
	<tr> 
		<td >Donor Specific Requirements</td> 
        	<td ><asp:textbox id="txtDonorRequirements" runat="server" TextMode="MultiLine" Rows="5" Columns="25" CssClass="form-control"></asp:textbox> </td> 
         <td></td>		 
        <td></td>
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
			<asp:TextBox id="txtGrantDetailID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr>
</table> 
<table style="margin-left:2%;width:90%">    
    <tr>
        <td><h4>Contract Reporting Dates</h4></td>
    </tr>
    <tr>
        <td>Name Of Report</td>
        <td>Type Of Report</td>
        <td>Expected Date</td>
        <td>Responsible Person</td>
    </tr>
    <tr>
        <td><asp:TextBox ID="txtReportName" runat="server" CssClass="form-control"></asp:TextBox></td>
        <td><asp:DropDownList ID="cboTypeOfReport" runat="server" CssClass="form-control"></asp:DropDownList></td>
        <td><telerik:RadDatePicker ID="radExpectedDate" runat="server" MinDate="1900-01-01"
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
    <tr><td>&nbsp;</td></tr>
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
                            <telerik:GridTemplateColumn HeaderText="ResponsiblePerson" SortExpression="SubmissionStatus">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "StaffResponsible")%>
                             </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="cboResponsiblePerson1" runat="server" Width="100px" Enabled="false">
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
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:LinkButton ID="lnkGrantee" runat="server" Text="Grantee Details"></asp:LinkButton>
        </td>
    </tr>
</table>
