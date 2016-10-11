<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GeneralActivityList.aspx.vb" Inherits="WebSysME.GeneralActivityList" MasterPageFile="~/Site.Master"%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>Activities</h4>
    <br />
        <asp:Button ID="cmdNew" runat="server" Text="Add New" CssClass="btn btn-default"/>
    <telerik:RadGrid ID="radActivities" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="ID" UniqueName="GeneralActivityID" HeaderText="GeneralActivityID"
                                Display="false">
                            </telerik:GridBoundColumn>   
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>                         
                             <telerik:GridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="Activity"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Subject" UniqueName="Subject" HeaderText="Subject">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Start" UniqueName="Start" HeaderText="Start Date">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="End" UniqueName="EndDate" HeaderText="EndDate"  >
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

