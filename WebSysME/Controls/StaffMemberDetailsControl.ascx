<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StaffMemberDetailsControl.ascx.vb" Inherits="WebSysME.StaffMemberDetailsControl" %>

<style type="text/css">
    .auto-style1 {
        width: 235px;
    }
    .auto-style2 {
        width: 236px;
    }
</style>
<div style="margin-left:2%">
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Staff Member Information</h4></td> 
	</tr> 
    <tr>
        <td>
            <asp:Label ID="lblProjectName" runat="server" BackColor="LightGray" Visible="false" ></asp:Label><br />
        </td>
    </tr>
    <tr>
        <td >OrganizationType</td> 
        	<td ><asp:DropDownList id="cboOrganizationType" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList> </td>
    </tr>
	<tr> 
		<td >Organization</td> 
        	<td ><asp:DropDownList id="cboOrganization" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td >Contact No</td> 
        	<td ><asp:textbox id="txtContactNo" runat="server" CssClass="form-control"></asp:textbox> </td>
        <td >CellPhone No</td> 
        	<td ><asp:textbox id="txtCellPhoneNo" runat="server" CssClass="form-control"></asp:textbox> </td>  
    </tr>
    <tr>
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control" Visible="false" ></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >First Name</td> 
        	<td ><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Surname</td> 
        	<td ><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Sex</td> 
        	<td ><asp:DropDownList id="cboSex" runat="server" CssClass="form-control">
                       <asp:ListItem Text="" Value=""></asp:ListItem>
                       <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                       <asp:ListItem Text="Female" Value="F"></asp:ListItem>
        	     </asp:DropDownList> </td>  
		<td >Position</td> 
        	<td ><asp:DropDownList id="cboPosition" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td >Address</td> 
        	<td ><asp:textbox id="txtAddress" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Email Address</td> 
        	<td ><asp:textbox id="txtEmailAddress" runat="server" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtStaffID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td>
            <asp:LinkButton ID="lnkIOP" Text="IOP" runat="server"></asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton ID="lnkAssignments" Text="ASSIGNMENTS" runat="server"></asp:LinkButton>
        </td>
        <td colspan="2">
            <asp:FileUpload ID="fupCV" Text="CV" runat="server"></asp:FileUpload><asp:Button ID="cmdUpload" Text="UploadCV" runat="server" CssClass="btn btn-default"/>
        </td>

    </tr>
</table> 
</div>