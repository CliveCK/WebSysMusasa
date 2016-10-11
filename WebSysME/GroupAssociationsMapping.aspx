﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GroupAssociationsMapping.aspx.vb" Inherits="WebSysME.GroupAssociationsMapping" MasterPageFile="~/Site.Master"%>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
<table cellpadding="4" cellspacing="0" border="0" style="width:90%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Group Associations Mapping</h4></td> 
	</tr> 
	<tr> 
		<td >Association</td> 
        	<td ><asp:dropdownlist id="cboGroupAssociation" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Group Type</td> 
        	<td ><asp:dropdownlist id="cboGroupType" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td> 
    </tr>
    <tr>
		<td >Group<br /> <br /></td>
    </tr>
    <tr>
        <td colspan="2">
            <telerik:RadGrid ID="radGroups" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" AllowMultiRowSelection="true" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="True" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <PagerStyle AlwaysVisible ="true" Position="Top"/>
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ObjectID" UniqueName="ObjectID" HeaderText="ObjectID" Display="false" >
                            </telerik:GridBoundColumn>                            
                           <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ObjectID")%>'
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
                    <ClientSettings Selecting-AllowRowSelect ="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
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
</table> 
</asp:Content>

