<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FirstPointOfContactDetailsControl.ascx.vb" Inherits="WebSysME.FirstPointOfContactDetailsControl" %>

<%@ Register src="MedicalHistoryDetailsControl.ascx" tagname="MedicalHistory" tagprefix="uc1" %>

<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>First Point Of Contact</h4></td> 
	</tr>
	<tr> 
		<td >Type Of Contact</td> 
        	<td ><asp:dropdownlist id="cboTypeOfContact" runat="server" CssClass="form-control"></asp:dropdownlist> </td>  
		<td >Contact Name</td> 
        	<td ><asp:textbox id="txtContactName" runat="server" CssClass="form-control"></asp:textbox> </td>
	</tr> 
	<tr> 
		<td >Date Of First Contact</td> 
        	<td ><telerik:RadDatePicker ID="radFirstContact" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Date Of First Referral</td> 
        	<td ><telerik:RadDatePicker ID="radFirstRefferal" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr> 
		<td >Date First Admitted</td> 
        	<td ><telerik:RadDatePicker ID="radFirstAdmitted" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Admitted To</td> 
        	<td ><asp:DropDownList id="txtAdmittedTo" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td >Referral Hospital</td> 
        	<td ><asp:textbox id="txtReferralHospital" runat="server" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtFirstPointOfContactID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
<uc1:MedicalHistory ID="ucMedicalHistory" runat="server"></uc1:MedicalHistory>