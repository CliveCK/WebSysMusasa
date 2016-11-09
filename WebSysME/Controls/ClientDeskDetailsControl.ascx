<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClientDeskDetailsControl.ascx.vb" Inherits="WebSysME.ClientDeskDetailsControl" %>
<table cellpadding="3" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Help Desk</h4></td> 
	</tr> 
    <tr> 
		<td >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Sex</td> 
        	<td ><asp:DropDownList id="cboSex" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
        	     </asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td >Age</td> 
        	<td ><asp:textbox id="txtAge" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 	
	<tr> 
		<td >Where From</td> 
        	<td ><asp:textbox id="txtWhereFrom" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
		<td >Information Provided</td> 
        	<td ><asp:textbox id="txtInformationProvided" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> &nbsp;
                    <asp:button id="cmdNew" runat="server" Text="New" CssClass="btn btn-default"></asp:button> 
            <asp:TextBox ID="txtCleintDeskInforID" runat="server" CssClass="HiddenControl"></asp:TextBox>
     </td> 
	</tr> 	
</table> 
