<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BudgetsDetailsControl.ascx.vb" Inherits="WebSysME.BudgetsDetailsControl" %>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="10" class="PageTitle"><h4>Budgets</h4></td> 
	</tr> 
    <tr>
        <td>Project:</td>
        <td><asp:DropDownList ID="cboProjects" runat="server" CssClass="form-control" Width="220px" AutoPostBack="true"></asp:DropDownList></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
	<tr> 
		<td >Activity Category</td> 
        	<td ><asp:dropdownlist id="cboActivityCategory" runat="server" CssClass="form-control" Width="220px" AutoPostBack="true" ></asp:dropdownlist> </td> 
		<td >Activity</td> 
        	<td ><asp:dropdownlist id="cboActivity" runat="server" CssClass="form-control" Width="220px"></asp:dropdownlist> </td> 
		<td >Unit</td> 
        	<td ><asp:dropdownlist id="cboUnit" runat="server" CssClass="form-control" Width="220px"></asp:dropdownlist> </td> 
		<td >Unit Cost</td> 
        	<td ><asp:textbox id="txtUnitCost" runat="server" CssClass="form-control" Width="220px"></asp:textbox> </td> 
		<td >Number Of Units</td> 
        	<td ><asp:textbox id="txtNumberOfUnits" runat="server" CssClass="form-control"></asp:textbox> </td> 
    </tr>
    <tr> 
		<td colspan="10"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:Button ID="cmdNew" runat="server" Text="New" CssClass="btn btn-default"/>
     </td> 
	</tr> 
	<tr> 
		<td colspan="10"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
    <tr>
        <td colspan="10">
             <telerik:RadGrid ID="radBudgets" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" PageSize="10"
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="BudgetID" UniqueName="BudgetID" HeaderText="BudgetID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit" UniqueName="editcolumn"
                                CommandName="View" >
                            </telerik:GridButtonColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Tracking" UniqueName="trackcolumn"
                                CommandName="Track" >
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="ActivityCategory" UniqueName="ActivityCategory" HeaderText="ActivityCategory" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="Activity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Unit" UniqueName="Unit" HeaderText="Unit">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UnitCost" UniqueName="UnitCost" HeaderText="UnitCost">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NumberOfUnits" UniqueName="NumberOfUnits" HeaderText="# Of Units">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Total Amount ($)">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTotalAmount" Text='<%# Eval("UnitCost") * Eval("NumberOfUnits") %>'></asp:Label>
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
                        <PagerStyle Position="Top" AlwaysVisible="true"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid><br />
        </td>
    </tr>	
	<tr> 
		<td colspan="10"> 
			<asp:TextBox id="txtBudgetID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
