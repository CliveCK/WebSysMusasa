<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OutreachDetailsControl.ascx.vb" Inherits="WebSysME.OutreachDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Outreach Details</h4></td> 
	</tr> 
	<tr> 
        <td >Organization</td> 
        	<td ><asp:dropdownlist id="cboOrganization" runat="server" CssClass="form-control" ></asp:dropdownlist> </td>
	</tr> 
	<tr> 
		 <td >Project</td> 
        	<td ><asp:dropdownlist id="cboProject" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Beneficiary Type</td> 
        	<td ><asp:dropdownlist id="cboBeneficiaryType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" CssClass="form-control" AutoPostBack="true" ></asp:dropdownlist> </td> 
		<td >Ward</td> 
        	<td ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
    <tr> 
		<td >Start Date</td> 
        	<td > <telerik:RadDatePicker ID="radStartDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >End Date</td> 
        	<td ><telerik:RadDatePicker ID="radEndDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>  
	</tr> 
    <tr>
        <td >Purpose of Outreach</td> 
        	<td ><asp:textbox id="txtPurposeOfOut" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Columns="30"></asp:textbox> </td> 
    </tr>
	<tr> 
		<td >Total</td> 
        	<td ><asp:textbox id="txtTotal" runat="server" CssClass="form-control"></asp:textbox> </td> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
            <asp:button id="cmdNew" runat="server" Text="New" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtOutreachID1" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
