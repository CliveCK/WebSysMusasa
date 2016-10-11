﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OrganizationalPlanDetailsControl.ascx.vb" Inherits="WebSysME.OrganizationalPlanDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Organizational Plan Details</h4><br /></td> 
	</tr> 
	<tr>  
		<td >Year</td> 
        	<td ><asp:textbox id="txtYear" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr>
    <tr> 
		<td >Activity Category</td> 
        	<td ><asp:textbox id="txtActivityCategory" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Activity</td> 
        	<td ><asp:textbox id="txtActivity" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr>  
	<tr> 
		<td >Period</td> 
        	<td ><asp:DropDownList id="cboPeriod" runat="server" CssClass="form-control">
                    <asp:ListItem Text="First Quarter" Value="First Quarter"></asp:ListItem>
                    <asp:ListItem Text="Second Quarter" Value="Second Quarter"></asp:ListItem> 
                    <asp:ListItem Text="Third Quarter" Value="Third Quarter"></asp:ListItem> 
                    <asp:ListItem Text="Fourth Quarter" Value="Fourth Quarter"></asp:ListItem> 
        	     </asp:DropDownList> </td> 
		<td >Status</td> 
        	<td ><asp:dropdownlist id="cboStatus" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
	<tr> 
		<td >Comments</td> 
        	<td ><asp:textbox id="txtComments" runat="server" CssClass="form-control"></asp:textbox> </td> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="New" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:dropdownlist id="txtOrganizationPlanID" runat="server" CssClass="HiddenControl"></asp:dropdownlist> 
		</td> 
	</tr> 
</table> 
