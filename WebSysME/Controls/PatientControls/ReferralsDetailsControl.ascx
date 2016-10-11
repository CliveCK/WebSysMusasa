<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReferralsDetailsControl.ascx.vb" Inherits="WebSysME.ReferralsDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Referrals</h4></td> 
	</tr> 
	<tr> 
		<td >KidzCan No</td> 
        	<td ><asp:TextBox id="txtPatientID" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox> </td> 
	</tr> 
	<tr> 
		<td >Referred Date</td> 
        	<td ><telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Referred To</td> 
        	<td ><asp:textbox id="txtReferredTo" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Reference No</td> 
        	<td ><asp:textbox id="txtPatientNo" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Dignosis Concern</td> 
        	<td ><asp:textbox id="txtDignosisConcern" runat="server" TextMode="MultiLine" Rows="5" Columns="25" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Assistance Needed</td> 
        	<td ><asp:textbox id="txtAssistanceNeeded" runat="server" TextMode="MultiLine" Rows="5" Columns="25" CssClass="form-control"></asp:textbox> </td> 
		<td >Assistance Offered</td> 
        	<td ><asp:textbox id="txtAssistanceOffered" runat="server" TextMode="MultiLine" Rows="5" Columns="25" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClas="btn btn-default"></asp:button> 
                    <asp:button id="cmdNew" runat="server" Text="New" CssClas="btn btn-default"></asp:button>
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtReferralID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr>
    <tr>
            <td colspan="8">
                <telerik:RadGrid ID="radReferrals" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ReferralID" UniqueName="ReferralID" HeaderText="ReferralID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="PatientNo" UniqueName="PatientNo" HeaderText="PatientNo">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ReferredTo" UniqueName="ReferredTo" HeaderText="ReferredTo">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ReferredDate" UniqueName="ReferredDate" HeaderText="ReferredDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DignosisConcern" UniqueName="DiagnosisConcern" HeaderText="DiagnosisConcern">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AssistanceNeeded" UniqueName="AssistanceNeeded" HeaderText="AssistanceNeeded">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AssistanceOffered" UniqueName="AssistanceOffered" HeaderText="AssistanceOffered">
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
