<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HouseholdNeeds.ascx.vb" Inherits="WebSysME.HouseholdNeeds" %>

<h4 style="text-align:center">Household Priority Needs</h4>
<div style="padding-left:10%">
<fieldset style="align-content:center"><legend style="align-self:center">Need</legend>
<table style="margin-left:20%">
    <tr>
        <td >
           <br />
        </td>
    </tr>
    <tr>
        <td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br /><br />
     </td> 
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="cboNeeds" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
        </td>
        <td>
            <asp:Button runat="server" ID="cmdAddNeed" Text="+" CssClass="btn btn-default"/>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadGrid ID="radBenListing" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="231%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="NeedID" UniqueName="NeedID" HeaderText="NeedID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Need" UniqueName="Need" HeaderText="Need">
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
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
</table>
 </fieldset>
</div>