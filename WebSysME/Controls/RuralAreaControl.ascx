<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RuralAreaControl.ascx.vb" Inherits="WebSysME.RuralAreaControl" %>

<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Rural Area Settings</h4><br /></td> 
	</tr> 
    <tr>
        <td >Country</td> 
        	<td ><asp:dropdownlist id="cboCountry" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Province</td> 
        	<td ><asp:dropdownlist id="cboProvince" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
        <td >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td>
        <td >Town</td> 
        	<td ><asp:dropdownlist id="cboTown" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td>
        <td >Ward</td> 
        	<td ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
        <td >Village</td> 
        	<td ><asp:dropdownlist id="cboVillage" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
    <tr> 
		<td ></td> 
        	<td ><asp:textbox id="txtCountry" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td ></td> 
        	<td ><asp:textbox id="txtProvince" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td ></td> 
        	<td ><asp:textbox id="txtDistrict" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td ></td> 
        	<td ><asp:textbox id="txtTown" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td ></td> 
        	<td ><asp:textbox id="txtWard" runat="server" CssClass="form-control"></asp:textbox> </td> 
         <td ></td> 
        	<td ><asp:textbox id="txtVillage" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td colspan="4">
            <asp:RadioButtonList runat="server" ID="rbLstSaveOption" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="true">
                <asp:ListItem Text="Country" Value="Country" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Province" Value="Province"></asp:ListItem>
                <asp:ListItem Text="District" Value="District"></asp:ListItem>
                <asp:ListItem Text="Town" Value="Town"></asp:ListItem>
                <asp:ListItem Text="Ward" Value="Ward"></asp:ListItem>
                <asp:ListItem Text="Village" Value="Village"></asp:ListItem>
            </asp:RadioButtonList>
		</td>
	</tr> 
	<tr> 
		<td colspan="8"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass ="btn btn-default"></asp:button> 
     </td> 
	</tr>
</table> 