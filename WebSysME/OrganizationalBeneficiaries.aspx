<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrganizationalBeneficiaries.aspx.vb" Inherits="WebSysME.OrganizationalBeneficiaries" MasterPageFile="~/Site.Master"%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>Organizations</h4>
        <asp:Button ID="cmdNew" runat="server" CssClass="btn btn-default" Text="AddNew" />
    <br />
    <telerik:RadGrid ID="radOrgListing" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="OrganizationID" UniqueName="OrganizationID" HeaderText="OrganizationID"
                                Display="false">
                            </telerik:GridBoundColumn>                            
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="OrganizationType" UniqueName="OrganizationType" HeaderText="OrganizationType">
                            </telerik:GridBoundColumn> 
                             <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContactName" UniqueName="ContactName" HeaderText="ContactName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContactNo" UniqueName="ContactNo" HeaderText="ContactNo">
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

