<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CounsellingReturningClientsListing.aspx.vb" Inherits="WebSysME.CounsellingReturningClientsListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:80%;margin-left:2%">
        <tr><td><asp:Button runat="server" ID="cmdNew1" Text="Details" CssClass="btn btn-default"/></td>
            
        </tr>
        <tr><td></td></tr>
        <tr><td >
    <telerik:RadGrid ID="radClients" runat="server" Height="80%" 
                    CellPadding="0" Width="100%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="CBSID" UniqueName="CBSID" HeaderText="ClientID"
                                Display="false">
                                                            </telerik:GridBoundColumn>   
                            <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="District">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Ward" UniqueName="Ward" HeaderText="Ward">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FirstName" UniqueName="Club" HeaderText="FirstName">
                            </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="Month" UniqueName="Month" HeaderText="Surname"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Year" UniqueName="Year" HeaderText="PhoneNumber"  >
                            </telerik:GridBoundColumn>                            
                            <telerik:GridButtonColumn FilterControlAltText="Filter column3 column" Text="View Initial Session" UniqueName="column3">
                            </telerik:GridButtonColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" UniqueName="column" HeaderText="View History">
                            </telerik:GridButtonColumn>                         
                            <telerik:GridButtonColumn FilterControlAltText="Filter column2 column" HeaderText="Action" UniqueName="column2">
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
