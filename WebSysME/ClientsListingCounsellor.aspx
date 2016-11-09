<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ClientsListingCounsellor.aspx.vb" Inherits="WebSysME.ClientsListingCounsellor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table style="width:100%;margin-left:2%">
        <tr><td><asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/>
            <asp:Button runat="server" ID="cmdInitSession" Text="Initial Session" CssClass="HiddenControl"/>
            </td>
            <td><asp:Button runat="server" ID="cmdReturningClients" Text="Returning Clients" CssClass="HiddenControl"/></td>
            <td><asp:Button runat="server" ID="cmdServedClients" Text="Served Clients" CssClass="HiddenControl"/></td>
        </tr>
        <tr><td></td></tr>
        <tr><td colspan="3">
    <telerik:RadGrid ID="radClients" runat="server" Height="80%" 
                    CellPadding="0" Width="70%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="BeneficiaryID" UniqueName="BeneficiaryID" HeaderText="BeneficiaryID"
                                Display="false">
                               </telerik:GridBoundColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="View/Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>                        
                            <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="District">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Ward" UniqueName="Ward" HeaderText="Ward">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName">
                            </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ContactNo" UniqueName="PhoneNumber" HeaderText="PhoneNumber"  >
                            </telerik:GridBoundColumn>                            
                            <telerik:GridBoundColumn DataField="AssignedBy" HeaderText="Assigned By" UniqueName="column1">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="DateAssigned" UniqueName="DateAssigned" HeaderText="Date Assigned"  >
                            </telerik:GridBoundColumn>  
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="View/Edit Initial Session" UniqueName="column1"
                                CommandName="Viewinitial">
                            </telerik:GridButtonColumn>  
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="View/Edit Returning Clients" UniqueName="column2"
                                CommandName="ViewReturning">
                            </telerik:GridButtonColumn>  
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="View/Edit Session" UniqueName="column3"
                                CommandName="ViewSession">
                            </telerik:GridButtonColumn>        
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
                </telerik:RadGrid></td></tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
