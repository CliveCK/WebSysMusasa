<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="IndicatorListingControl.ascx.vb" Inherits="WebSysME.IndicatorListingControl" %>

<div style="margin-left:2%">
<table>
    <tr>
        <td>
            <h3>Indicator Listing</h3>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>
        <asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/></td>
    </tr>
    <tr>
        <td>
             <telerik:RadGrid ID="radIndicatorListing" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" >
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="IndicatorID" UniqueName="IndicatorID" HeaderText="IndicatorID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn Display="True" HeaderText="" Text="View" UniqueName="ProjectID"
                                    CommandName="ViewIndicatorDetails">
                               </telerik:GridButtonColumn> 
                            <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BaselineValue" UniqueName="BaselineValue" HeaderText="BaselineValue">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ProgramTargetValue" UniqueName="ProgramTargetValue" HeaderText="ProgramTargetValue">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ResponsibleParty" UniqueName="ResponsibleParty" HeaderText="ResponsibleParty">
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
                         <ClientEvents OnRowContextMenu="RowContextMenu"></ClientEvents>
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
</table>
</div>