<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GrantManagementListing.aspx.vb" Inherits="WebSysME.GrantManagementListing" Title="Health Centers" MasterPageFile ="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="margin-left:2%;width:80%">
        <tr>
            <td colspan="4">
                <h4>Grant Management Listing</h4><br />
            </td> 
        </tr>
        <tr> 
		<td colspan="4">
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br />
     </td> 
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <telerik:RadGrid ID="radGrantManagement" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <PagerStyle AlwaysVisible="true" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="GrantDetailID" UniqueName="GrantDetailID" HeaderText="GrantDetailID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="ProjectTitle" UniqueName="ProjectTitle" HeaderText="ProjectTitle" >
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="Donor" UniqueName="Donor" HeaderText="Donor">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="KCP" UniqueName="KCP" HeaderText="KCP">
                            </telerik:GridBoundColumn>   
                            <telerik:GridBoundColumn DataField="ContractValueInUSD" UniqueName="ContractValueInUSD" HeaderText="Project Value In USD">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContractStartDate" UniqueName="ContractStartDate" HeaderText="ContractStartDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContractEndDate" UniqueName="ContractEndDate" HeaderText="ContractEndDate">
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
