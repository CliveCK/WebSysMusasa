<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProcumentList.aspx.vb" Inherits="WebSysME.ProcumentList" MasterPageFile="~/Site.Master"%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
<div style="margin-left:2%">
<table>
    <tr>
        <td>
            <h4>Procument Listing</h4>
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
             <telerik:RadGrid ID="radProcumentListing" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" >
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ProcumentID" UniqueName="ProcumentID" HeaderText="ProcumentID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn Display="True" HeaderText="" Text="View" UniqueName="Procument"
                                    CommandName="View">
                               </telerik:GridButtonColumn> 
                            <telerik:GridBoundColumn DataField="Project" UniqueName="Project" HeaderText="Project">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="QuantityRequired" UniqueName="QuantityRequired" HeaderText="QuantityRequired" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateRequired" UniqueName="DateRequired" HeaderText="DateRequired">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateRequested" UniqueName="DateRequested" HeaderText="DateRequested">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateOrdered" UniqueName="DateOrdered" HeaderText="DateOrdered">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateSupplied" UniqueName="DateSupplied" HeaderText="DateSupplied">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RequestedBy" UniqueName="RequestedBy" HeaderText="RequestedBy">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="OrderedBy" UniqueName="OrderedBy" HeaderText="OrderedBy">
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