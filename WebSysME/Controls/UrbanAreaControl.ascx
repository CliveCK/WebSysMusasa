<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UrbanAreaControl.ascx.vb" Inherits="WebSysME.UrbanAreaControl" %>
<style type="text/css">
    .auto-style1 {
        width: 235px;
    }
    .auto-style2 {
        width: 236px;
    }
</style>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Urban Area Settings</h4></td> 
	</tr> 
    <tr>
        <td >Country</td> 
        	<td ><asp:dropdownlist id="cboCountry" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >City</td> 
        	<td ><asp:dropdownlist id="cboCity" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
        <td >Suburb</td> 
        	<td ><asp:dropdownlist id="cboSuburb" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td>
        <td >Section</td> 
        	<td ><asp:dropdownlist id="cboSection" runat="server" CssClass="form-control"></asp:dropdownlist> </td>  
	</tr> 
    <tr> 
		<td ></td> 
        	<td ><asp:textbox id="txtCountry" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td ></td> 
        	<td ><asp:textbox id="txtCity" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td ></td> 
        	<td ><asp:textbox id="txtSuburb" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td ></td> 
        	<td ><asp:textbox id="txtSection" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td colspan="4">
            <asp:RadioButtonList runat="server" ID="rbLstSaveOption" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="true">
                <asp:ListItem Text="Country" Value="Country" Selected="True"></asp:ListItem>
                <asp:ListItem Text="City" Value="City"></asp:ListItem>
                <asp:ListItem Text="Suburb" Value="Suburb"></asp:ListItem>
                <asp:ListItem Text="Section" Value="Section"></asp:ListItem>
            </asp:RadioButtonList>
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
			<asp:TextBox id="txtCommunityID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
