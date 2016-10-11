<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BudgetTrackingDetailsControl.ascx.vb" Inherits="WebSysME.BudgetTrackingDetailsControl" %>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="14" class="PageTitle"><h4>Budget Tracking</h4></td> 
	</tr> 
	<tr> 
        <td >Activity Category</td> 
        	<td ><asp:textbox id="txtActivityCategory" runat="server" Enabled="false" CssClass="form-control" Width="200px"></asp:textbox> </td> 
        <td >Activity</td> 
        	<td ><asp:textbox id="txtActivity" runat="server" Enabled="false" CssClass="form-control" Width="200px"></asp:textbox> </td> 
		<td >Year</td> 
        	<td ><asp:textbox id="txtBudgetYear" runat="server" CssClass="form-control" Width="100px"></asp:textbox> </td> 
		<td >Month</td> 
        	<td ><asp:dropdownlist id="cboBudgetMonth" runat="server" CssClass="form-control" Width="90px"></asp:dropdownlist> </td> 
		<td >Budget Target</td> 
        	<td ><asp:textbox id="txtBudgetTarget" runat="server" CssClass="form-control" Width="100px"></asp:textbox> </td> 
		<td >Actual</td> 
        	<td ><asp:textbox id="txtActual" runat="server" CssClass="form-control"></asp:textbox> </td>
        <td >Comments</td> 
        	<td ><asp:textbox id="txtComments" runat="server" CssClass="form-control"></asp:textbox> </td>  
         
	</tr> 
	<tr> 
		<td colspan="14"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="14"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button>             
                    <asp:Button ID="cmdNew" runat="server" Text="New" CssClass="btn btn-default"/>
     </td> 
	</tr> 
    <tr>
        <td colspan="14">
             <telerik:RadGrid ID="radBudgetTracking" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" PageSize="10"
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="BudgetTrackinngID" UniqueName="BudgetTrackingID" HeaderText="BudgetTrackingID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit" UniqueName="editcolumn"
                                CommandName="View" >
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="ActivityCategory" UniqueName="ActivityCategory" HeaderText="ActivityCategory" Display="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="Activity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BudgetYear" UniqueName="BudgetYear" HeaderText="Year">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BudgetMonth" UniqueName="BudgetMonth" HeaderText="Month">
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="BudgetTarget" UniqueName="BudgetTarget" HeaderText="Target">
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="Actual" UniqueName="NumberOfUnits" HeaderText="Comments">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Comments" UniqueName="Comments" HeaderText="Comments">
                            </telerik:GridBoundColumn>
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
		<td colspan="4"> 
			<asp:TextBox id="txtBudgetTrackinngID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
            <asp:TextBox id="txtBudgetID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
