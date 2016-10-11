<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrganizationalPlanListing.aspx.vb" Inherits="WebSysME.OrganizationalPlanListing" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>organizational Plans</h4>
        <asp:Button ID="cmdNew" runat="server" CssClass="btn btn-default" Text="AddNew" />
    <br />
    <telerik:RadGrid ID="radOrgPlanListing" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="OrganizationalPlanID" UniqueName="OrganizationalPlanID" HeaderText="OrganizationalPlanID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Year" UniqueName="Year" HeaderText="Year"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Period" UniqueName="Period" HeaderText="Period"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ActivityCategory" UniqueName="ActivityCategory" HeaderText="ActivityCategory">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="Activity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Status" UniqueName="Status" HeaderText="Status">
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
                </telerik:RadGrid><br />
    </div>
</asp:Content>


