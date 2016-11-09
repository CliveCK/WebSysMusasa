<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HelpDeskInfoListing.aspx.vb" Inherits="WebSysME.HelpDeskInfoListing" MasterPageFile="~/Site.Master"%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>Help Desk</h4>
        <asp:Button ID="cmdNew" runat="server" CssClass="btn btn-default" Text="AddNew" />
    <br />
    <telerik:RadGrid ID="radHelpDesk" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="80%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="ClientDeskInforID" UniqueName="ClientDeskInforID" HeaderText="ClientDeskInforID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                            </telerik:GridBoundColumn> 
                             <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Age" UniqueName="Age" HeaderText="Age"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="WHereFrom" UniqueName="From" HeaderText="From">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="InformationProvided" UniqueName="InformationProvided" HeaderText="InformationProvided">
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

