<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProjectsControl.ascx.vb" Inherits="WebSysME.ProjectsControl" %>
<style type="text/css">
    .auto-style1 {
        width: 235px;
    }
    .auto-style2 {
        width: 236px;
    }
</style>
<asp:Panel ID="pnlProjects" runat="server" Width="100%">
<div style="margin-left:2%">
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Projects Information</h4></td> 
	</tr>
    <tr>
        <td colspan="2">
 
        </td>
    </tr>
    
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr>
     <tr>
        <td >Programme</td> 
        	<td ><asp:DropDownList id="cboProgram" runat="server" CssClass="form-control" ></asp:DropDownList> </td>
    </tr> 
	<tr> 
		<td class="auto-style2" >Program Code</td> 
        	<td ><asp:textbox id="txtProjectCode" runat="server"  CssClass="form-control"></asp:textbox> </td> 
	</tr>
    <tr>
        <td >Key Change Promise</td> 
        	<td ><asp:ListBox id="lstKeyChangePromise" runat="server" CssClass="form-control" SelectionMode="Multiple" ToolTip="To select Multiple items hold down the Ctrl Key while selecting"></asp:ListBox> </td>
    </tr>
    <tr> 
		<td class="auto-style2" >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server"  CssClass="form-control"></asp:textbox> </td> 
        <td class="auto-style2" >Acronym</td> 
        	<td ><asp:textbox id="txtAcronym" runat="server"  CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Strategic Objective</td> 
        	<td ><asp:ListBox id="lstStrategicObjective" runat="server" CssClass="form-control" SelectionMode="Multiple" ToolTip="To select Multiple items hold down the Ctrl Key while selecting"></asp:ListBox> </td> 
		<td >Project Manager</td> 
        	<td ><asp:DropDownList id="cboProjectManager" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Targeted No Of Beneficiaries</td> 
        	<td ><asp:textbox id="txtTargetedNoOfBeneficiaries" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Actual Beneficiaries</td> 
        	<td ><asp:textbox id="txtActualBeneficiaries" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Start Date</td> 
        	<td > <telerik:RadDatePicker ID="radStartDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Final Evaluation Date</td> 
        	<td > <telerik:RadDatePicker ID="radFinalEvlDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >End Date</td> 
        	<td > <telerik:RadDatePicker ID="RadEndDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Project Budget</td> 
        	<td ><asp:textbox id="txtProjectBudget" runat="server"  CssClass="form-control"></asp:textbox> </td> 
	</tr> 
</table>
    </div>
<div style="padding-left:2%">
<table cellpadding="4" cellspacing="0" border="0" style="width:100%" colspan="2">    
    <tr> 
		<td class="auto-style1" >Beneficiary Description</td> 
        	<td ><asp:textbox id="txtBenDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="40"  CssClass="form-control"></asp:textbox> </td> 
    </tr>
    <tr>
		<td class="auto-style1" >Stakeholder Description</td> 
        	<td ><asp:textbox id="txtStakeholderDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="40"  CssClass="form-control"></asp:textbox> </td> 
	</tr> 
    <tr> 
		<td class="auto-style1" >Final Goal Statement</td> 
        	<td ><asp:textbox id="txtFinalGStatement" runat="server" TextMode="MultiLine" Rows="5" Columns="40"  CssClass="form-control"></asp:textbox> </td> 
    </tr>
    <tr>
        <td colspan="2"><asp:PlaceHolder ID="phCustomFields" runat="server"></asp:PlaceHolder></td>
    </tr>
	<tr> 
		<td colspan="2"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default" Width="75px"></asp:button>
            <asp:button id="cmdClear" runat="server" Text="New" CssClass="btn btn-default" Width="75px"></asp:button>  
     </td> 
        <td>
            <asp:button id="cmdDelete" runat="server" Text="Delete" CssClass="btn btn-default" Width="75px" 
                OnClientClick="javascript:return confirm('Are you sure you want to delete this project?')"></asp:button> 
        </td>
	</tr> 
	<tr> 
		<td colspan="2"> 
			<asp:textbox id="txtProject" runat="server" Visible="false"></asp:textbox> 
		</td> 
	</tr> 
</table>
    </div>
</asp:Panel>