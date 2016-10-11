<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PatientsDetailsControl.ascx.vb" Inherits="WebSysME.PatientsDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Patient Profile </h4></td> 
	</tr> 
    <tr> 		
		<td >KidzCan Number</td> 
        	<td ><asp:textbox id="txtPatientNumber" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 		
		<td >First Name</td> 
        	<td ><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Surname</td> 
        	<td ><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox> </td> 		
	</tr> 
	<tr> 
		<td >Medical Aid Type</td> 
        	<td ><asp:dropdownlist id="cboMedicalAidType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Orphanhood</td> 
        	<td ><asp:dropdownlist id="cboOrphanhood" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Closest Health Center</td> 
        	<td ><asp:dropdownlist id="cboClosestHealthCenter" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >School</td> 
        	<td ><asp:dropdownlist id="cboSchool" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Date Of Birth</td> 
        	<td > <telerik:RadDatePicker ID="radDateofBirth" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
        <td >Date Of Death</td> 
        	<td > <telerik:RadDatePicker ID="radDateOfDeath" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr> 
        <td >Sex</td> 
        	<td ><asp:DropDownList id="cboSex" runat="server" CssClass="form-control">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="M" Value="M"></asp:ListItem>
                    <asp:ListItem Text="F" Value="F"></asp:ListItem>
        	     </asp:DropDownList> </td> 
        <td >Religion</td> 
        	<td ><asp:dropdownlist id="cboReligion" runat="server" CssClass="form-control"></asp:dropdownlist> </td>  
	</tr>
    <tr>
        <td >Telephone</td> 
        	<td ><asp:textbox id="txtTelephone" runat="server" CssClass="form-control"></asp:textbox> </td>
        <td >Status</td> 
        	<td ><asp:dropdownlist id="cboStatus" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
    </tr>
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
			<asp:TextBox  id="txtPatientID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
