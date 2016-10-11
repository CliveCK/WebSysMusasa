<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StrategicPlansDetailsControl.ascx.vb" Inherits="WebSysME.StrategicPlansDetailsControl" %>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Strategic Plan Details</h4></td> 
	</tr> 
	<tr> 
		<td >Organization</td> 
        	<td ><asp:dropdownlist id="cboOrganization" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>  
	<tr> 
		<td >Plan I D</td> 
        	<td ><asp:TextBox id="txtPlan" runat="server" CssClass="form-control"></asp:TextBox> </td> 
		<td >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >From Year</td> 
        	<td ><asp:DropDownList id="cboFromYear" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >To Year</td> 
        	<td ><asp:DropDownList id="cboToYear" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr>
	<tr> 
		<td >Summary</td> 
        	<td ><asp:textbox id="txtSummary" runat="server" TextMode="MultiLine" Rows="5" Columns="25" CssClass="form-control"></asp:textbox> </td>
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
			<asp:TextBox id="txtStrategicPlanID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    '<tr>
        <td>&nbsp;</td>
     </tr>
    <tr>
       <td> <asp:LinkButton runat="server" ID="lnkMilestone" Text="Milestones"></asp:LinkButton></td>
    </tr>
</table> 
