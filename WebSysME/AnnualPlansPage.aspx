<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AnnualPlansPage.aspx.vb" Inherits="WebSysME.AnnualPlansPage" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
<div style="margin-left:2%">
<table>
    <tr>
        <td>
            <h4>Annual Plan</h4>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>
             <telerik:RadGrid ID="radAnnualPlan" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" >
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns> 
                            <telerik:GridBoundColumn DataField="Organization" UniqueName="Organization" HeaderText="Organization">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="PlanYear" UniqueName="Year" HeaderText="Year" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PlanQuarter" UniqueName="Quarter" HeaderText="Quarter">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Month" UniqueName="Month" HeaderText="Month">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="Activity">
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
                    </MasterTableView>
                    <ClientSettings>
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
</table>
</div>
</asp:Content>