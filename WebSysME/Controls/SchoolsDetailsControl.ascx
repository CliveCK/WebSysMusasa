<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SchoolsDetailsControl.ascx.vb" Inherits="WebSysME.SchoolsDetailsControl" %>

<div style="margin-left:2%">
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Schools Details</h4><br /></td> 
	</tr>
    <tr> 
		<td >Province</td> 
        	<td ><asp:dropdownlist id="cboProvince" runat="server" CssClass="form-control" AutoPostBack="true" ></asp:dropdownlist> </td> 
	</tr>
    <tr> 
		<td >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td> 
	</tr> 
	<tr>
		<td >Ward</td> 
        	<td ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >School Type</td> 
        	<td ><asp:dropdownlist id="cboSchoolType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
     </tr>
    <tr>         
		<td >School Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Staff Compliment</td> 
        	<td ><asp:textbox id="txtStaffCompliment" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >No Of Male Students</td> 
        	<td ><asp:textbox id="txtNoOfMaleStudents" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >No Of Female Students</td> 
        	<td ><asp:textbox id="txtNoOfFemaleStudents" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Total Enrollment</td> 
        	<td ><asp:textbox id="txtTotalEnrollment" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Longitude</td> 
        	<td ><asp:textbox id="txtLongitude" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Latitude</td> 
        	<td ><asp:textbox id="txtLatitude" runat="server" CssClass="form-control"></asp:textbox><br /></td>
	</tr> 
	<tr> 
		<td colspan="4"><br /> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> <br />
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-detault"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtSchoolID" runat="server" Visible="false" ></asp:TextBox> 
		</td> 
	</tr> 
</table> 
</div> 
