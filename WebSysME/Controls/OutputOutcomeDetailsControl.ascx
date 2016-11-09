<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OutputOutcomeDetailsControl.ascx.vb" Inherits="WebSysME.OutputOutcomeDetailsControl" %>

<div style="padding-left:2%">
<table cellpadding="2" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Output Outcomes</h4></td> 
	</tr> 
	<tr>
		<td >Output</td> 
    </tr>
    <tr>
        	<td ><asp:dropdownlist id="cboOutputs" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td> 
	</tr>
    <tr> 
		<td colspan="4"> <br />
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
	<tr>
        <td>Outcomes</td>
    </tr>
    <tr>
        <td>
            <telerik:RadGrid ID="radOutputOutcomes" runat="server" GridLines="None" Height="100%" AllowMultiRowSelection="True"
                      CellPadding="0" Width="90%">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    <MasterTableView AllowFilteringByColumn="True" AllowMultiColumnSorting="True" AllowPaging="True"
                    AllowSorting="True" CommandItemDisplay="Top" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn>
                             <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OutcomeID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove item?')"
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
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
	</tr>
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Map" CssClass="btn btn-default"></asp:button>
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtOutputOutcomeID" runat="server" Visible="false"></asp:TextBox> 
		</td> 
	</tr> 
</table>
</div> 

