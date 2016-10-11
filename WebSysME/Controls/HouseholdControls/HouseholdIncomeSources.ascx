<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HouseholdIncomeSources.ascx.vb" Inherits="WebSysME.HouseholdIncomeSources" %>

<div style="margin-left:2%">
    <fieldset><legend></legend>
    <table cellpadding="4">
        <tr>
            <td><h4>Income Sources</h4><br /><br /></td>
            <td><asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel><br /></td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="cmdMapIncome" Text="Select Income Source" CssClass="btn btn-default"/><br />
                <telerik:RadGrid ID="radIncomeSource" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="IncomeSourceID" UniqueName="IncomeSourceID" HeaderText="IncomeSourceID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                                 UniqueName="chkRowSelect">
                            </telerik:GridClientSelectColumn>
                             <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
        </fieldset>
</div>