<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PatientAddress.ascx.vb" Inherits="WebSysME.PatientAddress" %>
<div style="padding-left:2%"><br />
<fieldset>
    <legend>Patient Address</legend><br />
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td></td>
	</tr> 
    <tr>
        <td colspan="4">
            <asp:RadioButtonList runat="server" ID="chkIsRuralUrban" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="true" >
                <asp:ListItem Value="IsRural" Selected="True">Rural Household</asp:ListItem>
                <asp:ListItem Value="IsUrban">Urban Household</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
	<tr> 
		<td style="width: 15%">District</td> 
        	<td><asp:DropDownList id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList> </td> 
		<td >City</td> 
        	<td ><asp:DropDownList id="cboCity" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td >Ward</td> 
        	<td ><asp:DropDownList id="cboWard" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Suburb</td> 
        	<td ><asp:DropDownList id="cboSuburb" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
    <tr> 
		<td >Village</td> 
        	<td ><asp:DropDownList id="cboVillage" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Section</td> 
        	<td ><asp:DropDownList id="cboSection" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
    <tr> 
		<td >Address</td> 
        	<td ><asp:TextBox id="txtSerialNo" runat="server" CssClass="form-control"></asp:TextBox> </td> 
		<td >Street</td> 
        	<td ><asp:DropDownList id="cboStreet" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
    <tr> 
		<td ></td> 
        	<td > </td> 
		<td >House No</td> 
        	<td ><asp:TextBox id="txtHouseNo" runat="server" CssClass="form-control"></asp:TextBox> </td> 
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
			<asp:TextBox id="txtAddressID1" runat="server" Visible="false"></asp:TextBox> 
		</td> 
	</tr> 

</table> 
</fieldset> 
</div>
