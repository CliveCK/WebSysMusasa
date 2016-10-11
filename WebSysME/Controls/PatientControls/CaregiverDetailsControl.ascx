<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CaregiverDetailsControl.ascx.vb" Inherits="WebSysME.CaregiverDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Caregiver Details</h4></td> 
	</tr>
	<tr>  
		<td >Firstname</td> 
        	<td ><asp:textbox id="txtFirstname" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Surname</td> 
        	<td ><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox> </td>
	</tr>
	<tr> 
		<td >Date Of Birth</td> 
        	<td ><telerik:RadDatePicker ID="radDateofBirth" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Contact No</td> 
        	<td ><asp:textbox id="txtContactNo" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 	
	<tr> 
		<td >Profession</td> 
        	<td ><asp:DropDownList id="cboProfession" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Relationship To Child</td> 
        	<td ><asp:DropDownList id="cboRelationship" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr>  
	<tr> 
		<td >Name Of Employer</td> 
        	<td ><asp:textbox id="txtNameOfEmployer" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Social Support Systems</td> 
        	<td ><asp:textbox id="txtSocialSupportSystems" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Alternate Sources Of Income</td> 
        	<td ><asp:textbox id="txtAlternateSourcesOfIncome" runat="server" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtCaregiverID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
            <asp:TextBox id="txtmPatientID" runat="server" CssClass="HiddenControl"></asp:TextBox>
		</td> 
	</tr> 
</table> 
