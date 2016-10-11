<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EligibilityDetailsControl.ascx.vb" Inherits="WebSysME.EligibilityDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Analysis of Eligibility</h4></td> 
	</tr> 
	<tr> 
		<td >Priority</td> 
        	<td ><asp:DropDownList id="txtPriority" runat="server" CssClass="form-control" AutoPostBack="true" >
                    <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                    <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                    <asp:ListItem Text="High" Value="High"></asp:ListItem>
        	     </asp:DropDownList> </td>
    </tr>
    <tr>
		<td >Evaluation</td> 
        	<td ><asp:textbox id="txtEvaluation" runat="server" TextMode="MultiLine" Rows="5" Columns="30" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Comments</td> 
        	<td ><asp:textbox id="txtComments" runat="server" TextMode="MultiLine" Rows="5" Columns="30" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtEligibilityID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
