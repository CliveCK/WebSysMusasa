﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GeneralActivityInputsDetailsControl.ascx.vb" Inherits="WebSysME.GeneralActivityInputsDetailsControl" %>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h1>General Activity Inputs</h1></td> 
	</tr> 
	<tr> 
		<td >Quantity</td> 
        	<td ><asp:textbox id="txtQuantity" runat="server"  CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Cost</td> 
        	<td ><asp:textbox id="txtCost" runat="server"  CssClass="form-control"></asp:textbox> </td> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server"  CssClass="form-control"></asp:textbox> </td> 
        <td> 
            		<asp:button id="cmdSave" runat="server" Text="+" CssClass="btn btn-default"></asp:button></td> 
	</tr>
    <tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr>
        <td colspan="5">Inputs Listing<br /> <br />
            <telerik:RadGrid ID="radInputs" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="True" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff" >
                        <PagerStyle AlwaysVisible ="false" Position="Top"/>
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="GeneralActivityInputID" UniqueName="GeneralActivityInputID" HeaderText="GeneralActivityInputID" Display="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False" 
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GeneralActivityInputID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove from Training?')"
                                    ToolTip="Click to remove " />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                                                    
                        </Columns>
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td> 
	</tr> 
	<tr> 
		<td colspan="5"> 
			<asp:TextBox id="txtGeneralActivityInputID" runat="server" CssClass="HiddenControl"></asp:TextBox> <br />
		</td> 
	</tr> 
    <tr>
        <td>
            <asp:LinkButton runat="server" ID="lnkBack" Text="<< Back to Activity Details" Font-Bold="true"></asp:LinkButton>
        </td>
    </tr>
</table> 
