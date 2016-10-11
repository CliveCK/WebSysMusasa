<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GranteeDetailListing.aspx.vb" Inherits="WebSysME.GranteeDetailListing" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="margin-left:2%;width:80%">
        <tr>
            <td colspan="4">
                <h4>Grantee Detail Listing</h4><br />
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
                <telerik:RadGrid ID="radGrantee" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <PagerStyle AlwaysVisible="true" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="GranteeID" UniqueName="GranteeID" HeaderText="GranteeID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="PartnerName" UniqueName="PartnerName" HeaderText="PartnerName" >
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="ParentDonor" UniqueName="ParentDonor" HeaderText="ParentDonor">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ProjectTitle" UniqueName="ProjectTitle" HeaderText="ProjectTitle">
                            </telerik:GridBoundColumn>   
                            <telerik:GridBoundColumn DataField="TotalGrantValue" UniqueName="TotalGrantValue" HeaderText="TotalGrantValue">
                                </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ProjectStartDate" UniqueName="ProjectStartDate" HeaderText="ProjectStartDate">
                                </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="ProjectEndDate" UniqueName="ProjectEndDate" HeaderText="ProjectEndDate">
                                </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ProjectDuration" UniqueName="ProjectDuration" HeaderText="ProjectDuration">
                                </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ProjectManager" UniqueName="ProjectManager" HeaderText="ProjectManager">
                                </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="AreaOfImplementation">
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


