<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OrganizationDetailsControl.ascx.vb" Inherits="WebSysME.OrganizationDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Organization Details</h4><br /></td> 
	</tr>
    <tr>
        <td >Organization Type</td> 
        	<td ><asp:dropdownlist id="cboOrganizationType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
    </tr>
    <tr> 
		<td >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr>
	<tr>  
		<td >Contact No</td> 
        	<td ><asp:textbox id="txtContactNo" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td >CellPhone No</td> 
        	<td ><asp:textbox id="txtCellPhoneNo" runat="server" CssClass="form-control"></asp:textbox><br /></td>
    </tr>
    <tr>
		<td >Contact Name</td> 
        	<td ><asp:textbox id="txtContactName" runat="server" CssClass="form-control"></asp:textbox><br /></td>        
        <td >Website</td> 
        	<td ><asp:textbox id="txtWebsiteAddress" runat="server" CssClass="form-control"></asp:textbox> </td> 
    </tr> 
    <tr>  
		<td >Address</td> 
        	<td ><asp:textbox id="txtAddress" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Email</td> 
        	<td ><asp:textbox id="txtEmail" runat="server" CssClass="form-control"></asp:textbox><br /></td>
    </tr> 
	<tr>
		<td >Longitude</td> 
        	<td ><asp:textbox id="txtLongitude" runat="server" CssClass="form-control"></asp:textbox> </td>
        <td >Latitude</td> 
        	<td ><asp:textbox id="txtLatitude" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr>
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"><br />
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdNew" runat="server" Text="New" CssClass="btn btn-default"></asp:button>
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtOrganizationID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td>
            <asp:LinkButton ID="lnkSubOffices" runat="server" Text="Add Sub Office" Visible="false"></asp:LinkButton>
        </td>
    </tr>
</table> 
