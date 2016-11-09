<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ClientCounsellingSessionActivityListing.aspx.vb" Inherits="WebSysME.ClientCounsellingSessionActivityListing" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table style="width:100%;margin-left:2%">
        <tr><td><asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr><td></td></tr>
        <tr><td colspan="3">
    <telerik:RadGrid ID="radClientsActivity" runat="server" Height="80%" 
                    CellPadding="0" Width="70%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="ClientSessionActivityID" UniqueName="ClientSessionActivityID" HeaderText="ClientSessionActivityID"
                                Display="false">
                               </telerik:GridBoundColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="View/Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>                        
                            <telerik:GridBoundColumn DataField="ActivityDate" UniqueName="ActivityDate" HeaderText="ActivityDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="Activity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="Outcome" UniqueName="Outcome" HeaderText="Outcome"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Remarks" UniqueName="Remarks" HeaderText="Remarks"  >
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
                </telerik:RadGrid></td></tr>
    </table>
</asp:Content>
