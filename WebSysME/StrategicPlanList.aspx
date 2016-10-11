<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StrategicPlanList.aspx.vb" Inherits="WebSysME.StrategicPlanList" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="margin-left:2%">
        <tr>
            <td>
                <h4>Strategic Plan List</h4><br />
            </td> 
        </tr>
        <tr> 
		<td >
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br />
     </td> 
	</tr> 
        <tr>
            <td>
                <asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadGrid ID="radStrategicListing" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <PagerStyle AlwaysVisible="true" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="StrategicPlanID" UniqueName="StrategicPlanID" HeaderText="StrategicPlanID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="Organization" UniqueName="Organization" HeaderText="Organization" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PlanCode" UniqueName="PlanCode" HeaderText="PlanCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="StrategyName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StrategyPeriod" UniqueName="StrategyPeriod" HeaderText="StrategyPeriod">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Summary" UniqueName="Summary" HeaderText="Summary">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "StrategicPlanID")%>'
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
</asp:Content>