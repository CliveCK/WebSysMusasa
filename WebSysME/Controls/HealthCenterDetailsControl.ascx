<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HealthCenterDetailsControl.ascx.vb" Inherits="WebSysME.HealthCenterDetailsControl" %>

<div style="margin-left:2%">
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>HealthCenter Details</h4><br /></td> 
	</tr> 
    <tr> 
		<td >Province</td> 
        	<td ><asp:dropdownlist id="cboProvince" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
    <tr> 
		<td >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
	<tr> 
		<td >Ward</td> 
        	<td ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
    <tr>
        <td >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
    </tr>
	<tr> 
		<td >Health Center Type</td> 
        	<td ><asp:dropdownlist id="cboHealthCenterType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >No Of Doctors</td> 
        	<td ><asp:textbox id="txtNoOfDoctors" runat="server" CssClass="form-control" ></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >No Of Nurses</td> 
        	<td ><asp:textbox id="txtNoOfNurses" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >No Of Beds</td> 
        	<td ><asp:textbox id="txtNoOfBeds" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Has Ambulance</td> 
        	<td ><asp:checkbox id="chkHasAmbulance" runat="server" ></asp:checkbox> </td> 
        <td >Has Waiting Mother's Shelter</td> 
        	<td ><asp:checkbox id="chkMotherShelter" runat="server" ></asp:checkbox> </td> 
    </tr>
    <tr>
		<td >Longitude</td> 
        	<td ><asp:textbox id="txtLongitude" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Latitude</td> 
        	<td ><asp:textbox id="txtLatitude" runat="server" CssClass="form-control"></asp:textbox> </td>
	</tr> 
	<tr> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server" TextMode="MultiLine" Rows="3" Columns="40" CssClass="form-control"></asp:textbox><br /> </td>
     </tr> 
     <tr>
        <td colspan="4"><asp:PlaceHolder ID="phCustomFields" runat="server"></asp:PlaceHolder></td>
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
			<asp:TextBox id="txtHealthCenterID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td>
            <asp:LinkButton runat="server" ID="lnkStaff" Text="StaffMembers"></asp:LinkButton>
        </td>
    </tr>
</table> 
    </div>