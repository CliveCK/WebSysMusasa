﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ImpactObjectiveControl.ascx.vb" Inherits="WebSysME.ImpactObjectiveControl" %>

<div style="padding-left:2%">
<table cellpadding="2" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="2" class="PageTitle"><h4>Impact Objectives</h4></td> 
	</tr> 
	<tr>
		<td >Impact</td>
        	<td ><asp:dropdownlist id="cboImpact" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td> 
	</tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
	<tr>
        <td>Objectives</td>
    </tr>
    <tr>
        <td colspan="2">
            <telerik:RadGrid ID="radObjectives" runat="server" GridLines="None" Height="100%" AllowMultiRowSelection="True"
                      CellPadding="0" Width="80%">
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
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ObjectiveID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove?')"
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
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="2"> 
            		<asp:button id="cmdSave" runat="server" Text="Map" CssClass="btn btn-default"></asp:button>
     </td> 
	</tr>
</table>
</div> 

