<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CBSListing.aspx.vb" Inherits="WebSysME.CBSListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left:2%">
    <h4>CBS</h4>
    <br />
        <asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/> &nbsp;&nbsp;&nbsp;&nbsp
        <asp:Button runat="server" ID="cmdUpload" Text="Upload From MS Excel Template" CssClass="btn btn-default"/>
    <telerik:RadGrid ID="radCBS" runat="server" Height="80%" 
                    CellPadding="0" Width="100%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="CBSMemberReportingID" UniqueName="CBSMemberReportingID" HeaderText="CBSMemberReportingID"
                                Display="false">
                             </telerik:GridBoundColumn>   
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>                         
                             <telerik:GridBoundColumn DataField="Province" UniqueName="Province" HeaderText="Province"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="District">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Ward2" UniqueName="Ward" HeaderText="Ward">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Club" UniqueName="Club" HeaderText="Club">
                            </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="Month" UniqueName="Month" HeaderText="Month" >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Year" UniqueName="Year" HeaderText="Year"  >
                            </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="Challenges" UniqueName="Challenges" HeaderText="Challenges"  >
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
