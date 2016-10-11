<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IndicatorTrackingListing.aspx.vb" Inherits="WebSysME.IndicatorTrackingListing" MasterPageFile="~/Site.Master"%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>Indicator Tracking</h4>
        <asp:Button ID="cmdNew" runat="server" CssClass="btn btn-default" Text="AddNew" />
    <br />
    <telerik:RadGrid ID="radFileListing" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="IndicatorTrackingID" UniqueName="IndicatorTrackingID" HeaderText="IndicatorTrackingID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="IndicatorType" UniqueName="IndicatorType" HeaderText="IndicatorType" Display="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Definition" UniqueName="Definition" HeaderText="Definition">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Year" UniqueName="Year" HeaderText="Year">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Month" UniqueName="Month" HeaderText="Month">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Target" UniqueName="Target" HeaderText="Target">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Achievement" UniqueName="Achievement" HeaderText="Achievement">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Comments" UniqueName="Comments" HeaderText="Comments">
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



