<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CashDonationDetailsControl.ascx.vb" Inherits="WebSysME.CashDonationDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Cash Donation Details</h4></td> 
	</tr> 
	<tr> 
		<td >Fundraising Date</td> 
        	<td ><telerik:RadDatePicker ID="radFundDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr> 
		<td >Amount Received</td> 
        	<td ><asp:textbox id="txtAmountReceived" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Received From</td> 
        	<td ><asp:textbox id="txtReceivedFrom" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Purpose</td> 
        	<td ><asp:textbox id="txtPurpose" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td>ReceiptNo/ReferenceNo</td>
        <td ><asp:textbox id="txtReceipt" runat="server" CssClass="form-control"></asp:textbox> </td>
    </tr>
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdNew" runat="server" Text="New" CssClass="btn btn-default"></asp:button>
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtCashDonationID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
     <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radCashDonation" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="CashDonationID" UniqueName="CashDonationID" HeaderText="CashDonationID" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="FundraisingDate" UniqueName="Date" HeaderText="Date">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="AmountReceived" UniqueName="AmountReceived" HeaderText="AmountReceived">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ReceivedFrom" UniqueName="ReceivedFrom" HeaderText="ReceivedFrom">
                            </telerik:GridBoundColumn>     
                             <telerik:GridBoundColumn DataField="Purpose" UniqueName="Purpose" HeaderText="Purpose">
                            </telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="ReceiptNo" UniqueName="ReceiptNo" HeaderText="ReceiptNo">
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
