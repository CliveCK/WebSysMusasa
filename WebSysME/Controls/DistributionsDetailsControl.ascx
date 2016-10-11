<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DistributionsDetailsControl.ascx.vb" Inherits="WebSysME.DistributionsDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Distributions Details</h4><br /></td> 
	</tr> 
    <tr> 
		<td >Organization</td> 
        	<td ><asp:dropdownlist id="cboOrganization" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
	<tr>
		<td >Distribution Type</td> 
        	<td ><asp:dropdownlist id="cboDistributionType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Location</td> 
        	<td ><asp:textbox id="txtLocation" runat="server" CssClass="form-control"></asp:textbox> </td>
        <td>Date</td> 
        <td>
            <telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker>
        </td>
        </tr> 
    <tr>
            <td >Commodity</td> 
        	<td ><asp:DropDownList id="cboCommodity" runat="server" CssClass="form-control"></asp:DropDownList> </td>   
        <td >Unit</td> 
        	<td ><asp:DropDownList id="cboUnits" runat="server" CssClass="form-control"></asp:DropDownList> </td>   
     </tr>
    <tr>          
		<td >Quantity per Beneficiary</td> 
        	<td ><asp:textbox id="txtQuantityPerBen" runat="server" TextMode="Number" CssClass="form-control"></asp:textbox> 
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
			<asp:TextBox id="txtDistributionID1" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td><asp:LinkButton ID="lnkBeneficiaries" runat="server" Text="Beneficiaries" Visible="<%# IIf(IsNumeric(txtDistributionID1.Text), True, False)%>"></asp:LinkButton></td>
    </tr>
</table> 
