<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StrategicPlanDetailsDetailsControl.ascx.vb" Inherits="WebSysME.StrategicPlanDetailsDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Strategic Plan Milestones</h4></td> 
	</tr> 
	<tr>   
        	<td ><asp:TextBox id="txtStrategicPlanID" runat="server" CssClass="HiddenControl"></asp:TextBox> </td> 
	</tr> 
	<tr> 
		<td >Objective</td> 
        	<td ><asp:dropdownlist id="cboObjective" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Activity</td> 
        	<td ><asp:dropdownlist id="cboActivity" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Plan Year</td> 
        	<td ><asp:textbox id="txtPlanYear" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Plan Quarter</td> 
        	<td ><asp:textbox id="txtPlanQuarter" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
        <td >Plan Month</td> 
        	<td ><asp:DropDownList id="cboMonth" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Milestone</td> 
        	<td ><asp:textbox id="txtMilestone" runat="server" CssClass="form-control"></asp:textbox> </td> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdNew" runat="server" Text="New" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtStrategicPlanDetailID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td>
            <h3>MileStones</h3> 
        </td>
    </tr>
    <tr>
            <td colspan="4">
                <telerik:RadGrid ID="radMilestoneListing" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="StrategicPlanDetailID" UniqueName="StrategicPlanDetailID" HeaderText="StrategicPlanDetailID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="Objective" UniqueName="Objective" HeaderText="StrategicObjective">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="StrategicActivity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PlanYear" UniqueName="PlanYear" HeaderText="PlanYear">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PlanQuarter" UniqueName="PlanQuarter" HeaderText="PlanQuarter">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Milestone" UniqueName="Milestone" HeaderText="Milestone">
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
                </telerik:RadGrid>
            </td>
        </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:LinkButton runat="server" ID="lnkBack" Text="Back to Strategic Plans"></asp:LinkButton>
        </td>
    </tr>
</table> 

