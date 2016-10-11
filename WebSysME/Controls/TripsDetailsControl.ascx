<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TripsDetailsControl.ascx.vb" Inherits="WebSysME.TripsDetailsControl" %>

<%@ Register src="~/Controls/TripTravellers.ascx" tagname="TripTravellersDetails" tagprefix="uc1" %>
<%@ Register src="~/Controls/TripCostsDetailsControl.ascx" tagname="TripCostsDetails" tagprefix="uc2" %>
<%@ Register src="~/Controls/TripDocuments.ascx" tagname="TripDocuments" tagprefix="uc3" %>

<table cellpadding="3" cellspacing="0" border="0" style="width:100%;"> 
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radTripListing" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="TripID" UniqueName="TripID" HeaderText="TripID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn Display="True" HeaderText="" Text="View" UniqueName="Trip"
                                    CommandName="ViewTripDetails">
                               </telerik:GridButtonColumn> 
                            <telerik:GridBoundColumn DataField="Purpose" UniqueName="Purpose" HeaderText="Purpose">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="TravellingFrom" UniqueName="TravellingFrom" HeaderText="TravellingFrom" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TravellingTo" UniqueName="TravellingTo" HeaderText="TravellingTo">
                                </telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="FromDate" UniqueName="FromDate" HeaderText="From">
                                </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ToDateDate" UniqueName="ToDateDate" HeaderText="To">
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
    <tr>
        <td colspan="4"><hr style="line-height:2px"/></td>
    </tr> 
	<tr> 
		<td colspan="4" class="PageTitle"><h5><b>Trip Details</b></h5></td> 
	</tr> 
	<tr> 
		<td >From Date</td> 
        	<td ><telerik:RadDatePicker ID="radFromDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker> </td>
		<td >To Date</td> 
        	<td ><telerik:RadDatePicker ID="radToDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr>  
		<td >Purpose</td> 
        	<td ><asp:textbox id="txtPurpose" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Travelling From</td> 
        	<td ><asp:textbox id="txtTravellingFrom" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Travelling To</td> 
        	<td ><asp:textbox id="txtTravellingTo" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtTripID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox id="txtProjectID" runat="server" CssClass="HiddenControl"></asp:TextBox>
		</td> 
	</tr> 
</table> <br />
<hr /><br />
<div>
     <telerik:RadTabStrip ID="radTabTrips" runat="server" MultiPageID="RadMultiPageTrips" SelectedIndex="3" Align="Justify" Width="100%">
                <Tabs>
                    <telerik:RadTab Text="Travellers" Selected ="true">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Costs">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Documents">
                    </telerik:RadTab>
                </Tabs>
   </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="RadMultiPageTrips" SelectedIndex="0"
                Width="100%" CssClass="multiPage">
                <telerik:RadPageView runat="server" ID="RadPageTripsView1" > 
                    <uc1:TripTravellersDetails runat="server" ID="ucTripTravellersDetails"></uc1:TripTravellersDetails>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageTripsView2" > 
                    <uc2:TripCostsDetails runat="server" ID="ucTripCostsDetails"></uc2:TripCostsDetails>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageTripsView3" > 
                    <uc3:TripDocuments runat="server" ID="ucTripDocuments"></uc3:TripDocuments>                     
                </telerik:RadPageView>
   </telerik:RadMultiPage>
</div>
