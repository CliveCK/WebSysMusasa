<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MicrofinanceClientsListing.aspx.vb" Inherits="WebSysME.MicrofinanceClientsListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table style="width:100%;margin-left:2%">
        <tr><td style="height: 38px"><asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/></td>
            
        </tr>
        <tr><td></td></tr>
        <tr><td >
    <telerik:RadGrid ID="radClients" runat="server" Height="80%" 
                    CellPadding="0" Width="80%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="BeneficiaryID" UniqueName="BeneficiaryID" HeaderText="BeneficiaryID"
                                Display="false">
                             </telerik:GridBoundColumn>   
                            <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="District">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Ward" UniqueName="Ward" HeaderText="Ward">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName">
                            </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="NationlIDNo" UniqueName="NationalIDNo" HeaderText="National ID #"  >
                            </telerik:GridBoundColumn>                            
                            <telerik:GridBoundColumn DataField="Sex" HeaderText="Sex" UniqueName="Sex">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ContactNo" UniqueName="ContactNo" HeaderText="ContactNo"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Capture Repayment" UniqueName="column2" HeaderText="Action" CommandName="View">
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
