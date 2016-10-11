<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ViewKeyChangePromises.aspx.vb" Inherits="WebSysME.ViewKeyChangePromises" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table style="margin-left:2%">
        <tr>
            <td><br /><asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/></td>
        </tr>
        <tr>
            <td>
        <telerik:RadGrid ID="radKeyListing" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="KeyChangePromiseID" UniqueName="KeyChangePromiseID" HeaderText="KeyChangePromiseID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="StrategicObjective" UniqueName="StrategicObjective" HeaderText="StrategicObjective">
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
                    <ClientSettings EnablePostBackOnRowClick="false">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
    </table>

</asp:Content>
