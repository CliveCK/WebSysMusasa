<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SurveyListing.aspx.vb" Inherits="WebSysME.SurveyListing" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
<div style="margin-left:2%">
<table>
    <tr>
        <td>
            <h3>Survey Listing</h3>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>
        <asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/></td>
    </tr>
    <tr>
        <td>
             <telerik:RadGrid ID="radSurveyListing" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" >
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="SurveyID" UniqueName="SurveyID" HeaderText="SurveyID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn Display="True" HeaderText="" Text="View" UniqueName="Survey"
                                    CommandName="ViewSurveyDetails">
                               </telerik:GridButtonColumn> 
                            <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="FromDate" UniqueName="FromDate" HeaderText="From" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ToDate" UniqueName="ToDate" HeaderText="To">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
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
                    </MasterTableView>
                    <ClientSettings>
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
</table>
</div>
</asp:Content>