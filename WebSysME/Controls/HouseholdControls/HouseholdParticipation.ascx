<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HouseholdParticipation.ascx.vb" Inherits="WebSysME.HouseholdParticipation" %>
<div style="margin-left:2%">
    <fieldset>
    <table cellpadding="4" width="70%" style="margin-left:15%">
        <tr>
            <td><br /><br /></td>
        </tr>
        <tr>
            <td><b>Interventions</b><br /></td>
            <td><br /></td>
            <td><b>Groups</b><br /></td>
        </tr>
        <tr>
            <td><asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel><br /></td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="cmdMapIntervention" Text="Select Intervention" CssClass="btn btn-default"/><br />
                <telerik:RadGrid ID="radInterventions" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%" AllowMultiRowSelection="true" >
                     <ClientSettings Selecting-AllowRowSelect ="true">
                        </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="InterventionID" UniqueName="InterventionID" HeaderText="InterventionID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                                 UniqueName="chkRowSelect">
                            </telerik:GridClientSelectColumn>
                             <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
            <td>

            </td>
            <td>
                <asp:Button runat="server" ID="cmdSaveGroups" Text="Select Groups" CssClass="btn btn-default"/><br />
                <telerik:RadGrid ID="radGroups" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="GroupID" UniqueName="GroupID" HeaderText="GroupID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                                 UniqueName="chkRowSelect">
                            </telerik:GridClientSelectColumn>
                             <telerik:GridBoundColumn DataField="GroupName" UniqueName="GroupName" HeaderText="GroupName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings Selecting-AllowRowSelect ="true">
                        </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
        </fieldset>
</div>