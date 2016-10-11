<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FollowupsDetailsControl.ascx.vb" Inherits="WebSysME.FollowupsDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Followups/Home Visits</h4></td> 
	</tr> 
	<tr> 
		<td >Followup Type</td> 
        	<td ><asp:dropdownlist id="cboFollowupType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Followup Date</td> 
        	<td ><telerik:RadDatePicker ID="radFollowupDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr> 
		<td >Followup Time</td> 
        	<td ><asp:textbox id="txtFollowupTime" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Length Of Visit</td> 
        	<td ><asp:textbox id="txtLengthOfVisit" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Case Priority</td> 
        	<td ><asp:textbox id="txtCasePriority" runat="server" CssClass="form-control"></asp:textbox> </td> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-control"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtFollowupID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr>
    <tr>
            <td colspan="14">
                <telerik:RadGrid ID="radFollowupListing" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="FollowupID" UniqueName="FollowupID" HeaderText="FollowupID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="FollowupType" UniqueName="FollowupType" HeaderText="FollowupType">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="FollowupDate" UniqueName="FollowupDate" HeaderText="FollowupDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="LengthOfVisit" UniqueName="LengthOfVisit" HeaderText="LengthOfVisit">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FollowupTime" UniqueName="FollowupTime" HeaderText="Time">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CasePriority" UniqueName="CasePriority" HeaderText="CasePriority">
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
