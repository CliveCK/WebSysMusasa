<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="KeyChangePromiseListing.aspx.vb" Inherits="WebSysME.KeyChangePromiseListing" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
<div style="margin-left:2%">
<table>
    <tr>
        <td>
            <h3>Key Change Promise Listing</h3>
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
             <telerik:RadGrid ID="radKeyChangeListing" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" >
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="KeyChangePromiseID" UniqueName="KeyChangePromiseID" HeaderText="KeyChangePromiseID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn Display="True" HeaderText="" Text="View" UniqueName="KeyChangeList"
                                    CommandName="ViewKeyChangeDetails">
                               </telerik:GridButtonColumn> 
                            <telerik:GridBoundColumn DataField="Code" UniqueName="Code" HeaderText="Strategic Objective Code">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="KeyChangePromiseNo" UniqueName="KeyChangePromiseNo" HeaderText="KeyChangePromiseNo">
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

