<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MainOneStopCentreListing.aspx.vb" Inherits="WebSysME.MainOneStopCentreListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left:2%">
    <h4>Main One Stop Centre</h4>
    <br />
        <asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/> &nbsp;&nbsp;&nbsp;&nbsp
        <asp:Button runat="server" ID="cmdUpload" Text="Upload From MS Excel Template" CssClass="btn btn-default"/>
    <telerik:RadGrid ID="radCBS" runat="server" Height="80%" 
                    CellPadding="0" Width="80%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="MainOneStopCenterID" UniqueName="MainOneStopCenterID" HeaderText="MainOneStopCenterID"
                                Display="false">
                            </telerik:GridBoundColumn>   
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="View/Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>                         
                             <telerik:GridBoundColumn DataField="CentreName" UniqueName="CentreName" HeaderText="CentreName"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Year" UniqueName="Year" HeaderText="Year">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Month" UniqueName="Month" HeaderText="Month">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="District">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Ward" UniqueName="Ward" HeaderText="Ward"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="NationalID" UniqueName="NationalIDNum" HeaderText="NationalIDNum"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="DOB" UniqueName="DOB" HeaderText="DOB"  >
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
