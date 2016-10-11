<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProcumentDetailsControl.ascx.vb" Inherits="WebSysME.ProcumentDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Procument Details</h4></td> 
	</tr> 
	<tr> 
	</tr> 
	<tr> 
		<td >Commodity</td> 
        	<td ><asp:dropdownlist id="cboCommodity" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
    </tr>
    <tr>
		<td >Quantity Required</td> 
        	<td ><asp:textbox id="txtQuantityRequired" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Date Required</td> 
        	<td ><telerik:RadDatePicker ID="radDateRequired" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker> </td> 
		<td >Date Ordered</td> 
        	<td ><telerik:RadDatePicker ID="radDateOrdered" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr> 
		<td >Date Requested</td> 
        	<td ><telerik:RadDatePicker ID="radDateRequested" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Date Supplied</td> 
        	<td ><telerik:RadDatePicker ID="radDateSupplied" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar4" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput4" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr> 
		<td >Requested By</td> 
        	<td ><asp:textbox id="txtRequestedBy" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Ordered By</td> 
        	<td ><asp:textbox id="txtOrderedBy" runat="server" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox  id="txtProcumentID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
   <tr>
        <td>
            <h4>Procument Listing</h4>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td colspan="4">
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
