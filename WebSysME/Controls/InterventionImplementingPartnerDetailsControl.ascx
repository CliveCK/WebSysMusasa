<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InterventionImplementingPartnerDetailsControl.ascx.vb" Inherits="WebSysME.InterventionImplementingPartnerDetailsControl" %>
<style type="text/css" >
.leftcolumn {
     width: 400px;
     padding: 0;
     padding-left:2%;
     margin: 2%;
     display: block;
     border: 1px solid white;
     position: fixed;
}

.rightcolumn {
     width: 400px;
     padding-left:40%;
     display: block;
     float: left;
     border: 1px solid white;
}
    </style> 

<fieldset><legend></legend>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Intervention & ImplementingPartner Details</h4><br /></td> 
	</tr>
    <tr> 
		<td >Programme</td> 
        	<td ><asp:dropdownlist id="cboProgramme" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
    <tr> 
		<td >Project</td> 
        	<td ><asp:dropdownlist id="cboProject" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Intervention</td> 
        	<td ><asp:dropdownlist id="cboIntervention" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
</table>
</fieldset><br />
<div style="align-self:center">Implementing Partner<br />
        	<asp:dropdownlist id="cboImplementingPartner" runat="server" CssClass="form-control"></asp:dropdownlist></div><br />
<br /><br />
<h4>Location</h4><br />
<div>Is Urban <br />
        	<asp:checkbox id="chkIsUrban" runat="server"></asp:checkbox></div><br />
<div class="leftcolumn">
<fieldset><legend></legend>
<table>
    <tr> 
		<td >Province</td> 
        	<td ><asp:dropdownlist id="cboProvince" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
	<tr> 
		<td >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
</table>
</fieldset>
</div>
<div class="rightcolumn">
    <fieldset><legend></legend>
    <table>
	<tr> 
		<td >City</td> 
        	<td ><asp:dropdownlist id="cboCity" runat="server" CssClass="form-control"></asp:dropdownlist> </td>
	</tr> 
</table>
</fieldset>
</div><br />
    <asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
 <br /><br />
<table>
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-detault"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:textbox id="txtInterventionImplementingPartnerID" runat="server"  Visible="false" ></asp:textbox> 
		</td> 
	</tr> 
</table> 
