<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HouseholdExpenditure.ascx.vb" Inherits="WebSysME.HouseholdExpenditure" %>
<div style="margin-left:2%"><br />
    <table style="width:100%">
        <tr>
            <td colspan="3">
                <b>Expenditure</b>
            </td>
            <td colspan="3">
                <b>Debts</b>
            </td>
        </tr>
        <tr>
            <td colspan="3"><br /><asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel><br /></td>
        </tr>
        <tr>
            <td>
                Expenditure Item<br />
                <asp:DropDownList ID="cboExpenditureItem" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
            <td>Average Expenditure<br />
                <asp:TextBox ID="txtAverageExpenditure" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="cmdSaveExpenditure" runat="server"  Text="+" CssClass="btn btn-default"/>
            </td>
            <td>
                Debt Item<br />
                <asp:DropDownList ID="cboDebtItem" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
            <td>
                Amount Owed<br />
                <asp:TextBox ID="txtAmountOwed" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="cmdSaveDebt" runat="server" Text="+" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                 <telerik:RadGrid ID="radExpenditure" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ExpenditureID" UniqueName="ExpenditureID" HeaderText="ExpenditureID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ExpenditureItem" UniqueName="ExpenditureItem" HeaderText="ExpenditureItem">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AverageExpenditure" UniqueName="AverageExpenditure" HeaderText="AverageExpenditure">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
            <td colspan="3">
                <telerik:RadGrid ID="radDebts" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="DebtID" UniqueName="DebtID" HeaderText="DebtID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="DebtItem" UniqueName="DebtItem" HeaderText="DebtItem">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AmountOwed" UniqueName="AmountOwed" HeaderText="AmountOwed">
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
</div>
