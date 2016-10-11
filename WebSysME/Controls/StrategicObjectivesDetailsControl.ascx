<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StrategicObjectivesDetailsControl.ascx.vb" Inherits="WebSysME.StrategicObjectivesDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Strategic Objectives Details</h4><br /></td> 
	</tr> 
	<tr>  
		 <td >Code</td> 
        	<td ><asp:textbox id="txtCode" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
        <td >From Year</td> 
        	<td ><asp:textbox id="txtFromYear" runat="server" CssClass="form-control"></asp:textbox> </td>
		<td >To Year</td> 
        	<td ><asp:textbox id="txtToYear" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server"  TextMode="MultiLine" Rows="5" Columns="30" CssClass="form-control"></asp:textbox><br /> </td> 
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
			<asp:TextBox  id="txtStrategicObjectiveID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td><br />
            <h4>Strategic Objective Listing</h4>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radStrategicListing" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <PagerStyle AlwaysVisible="true" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="StrategicObjectiveID" UniqueName="StrategicObjectiveID" HeaderText="StrategicObjectiveID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="Code" UniqueName="Code" HeaderText="Code" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FromYear" UniqueName="FromYear" HeaderText="From">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ToYear" UniqueName="ToYear" HeaderText="To">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "StrategicObjectiveID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to delete?')"
                                    ToolTip="Click to delete " />
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
                        <PagerStyle Position="TopAndBottom" />
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
</table> 
