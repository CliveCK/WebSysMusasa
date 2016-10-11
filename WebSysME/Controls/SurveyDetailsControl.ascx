<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SurveyDetailsControl.ascx.vb" Inherits="WebSysME.SurveyDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h5><b>Survey Details</b></h5><br /></td> 
	</tr> 
	<tr> 
        <td >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td>
	</tr> 
	<tr>          
		<td >From Date</td> 
        	<td><telerik:RadDatePicker ID="radFromDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >To Date</td> 
        	<td><telerik:RadDatePicker ID="radToDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
    </tr>
    <tr>
		<td >Status</td> 
        	<td ><asp:checkbox id="chkStatus" runat="server"></asp:checkbox> </td> 
	</tr> 
	<tr> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="30" CssClass="form-control"></asp:textbox> </td> 
		<td >Comments</td> 
        	<td ><asp:textbox id="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Columns="30"></asp:textbox> </td> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="New" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:dropdownlist id="txtSurveyID" runat="server" CssClass="HiddenControl"></asp:dropdownlist> 
		</td> 
	</tr> 
    <tr>
        <td>
            <h5><b>Survey Listing</b></h5>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td colspan="4">
             <telerik:RadGrid ID="radSurveyListing" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="Survey" UniqueName="Survey" HeaderText="Survey"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn Display="True" HeaderText="" Text="View" UniqueName="Evaluation"
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
