<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProjectMeetingDetailsControl.ascx.vb" Inherits="WebSysME.ProjectMeetingDetailsControl" %>

<%@ Register src="~/Controls/ProjectMeetingAttendantsControl.ascx" tagname="ProjectMeetingAttendants" tagprefix="uc1" %>
<%@ Register src="~/Controls/ProjectMeetingAgendaDetailsControl.ascx" tagname="ProjectMeetingAgenda" tagprefix="uc2" %>
<%@ Register src="~/Controls/ProjectMeetingDocuments.ascx" tagname="ProjectMeetingDocuments" tagprefix="uc3" %>
<table cellpadding="2" cellspacing="0" border="0" style="width:100%">
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radMeetingListing" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ProjectMeetingID" UniqueName="ProjectMeetingID" HeaderText="ProjectMeetingID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn Display="True" HeaderText="" Text="View" UniqueName="Meeting"
                                    CommandName="ViewMeetingDetails">
                               </telerik:GridButtonColumn> 
                            <telerik:GridBoundColumn DataField="Place" UniqueName="Place" HeaderText="Place">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="DateOfMeeting" UniqueName="DateOfMeeting" HeaderText="DateOfMeeting" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Purpose" UniqueName="Purpose" HeaderText="Purpose">
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
		<td colspan="4" class="PageTitle"><h5><b>Project Meeting Details</b></h5><br /></td> 
	</tr>
	<tr> 
		<td >Date Of Meeting</td> 
        	<td ><telerik:RadDatePicker ID="radDateOfMeeting" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Place</td> 
        	<td ><asp:textbox id="txtPlace" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Purpose</td> 
        	<td ><asp:textbox id="txtPurpose" runat="server" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtProjectMeetingID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
            <asp:TextBox id="txtProjectID" runat="server" CssClass="HiddenControl"></asp:TextBox>
		</td> 
	</tr> 
</table> <br />
<hr /><br />
<div>
     <telerik:RadTabStrip ID="radTab" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="3" Align="Justify" Width="100%">
                <Tabs>
                    <telerik:RadTab Text="Meeting Attendants" Selected ="true">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Agenda">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Documents">
                    </telerik:RadTab>
                </Tabs>
   </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0"
                Width="100%" CssClass="multiPage">
                <telerik:RadPageView runat="server" ID="RadPageView1" > 
                    <uc1:ProjectMeetingAttendants runat="server" ID="ucProjectMeetingAttendants"></uc1:ProjectMeetingAttendants>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView2" > 
                    <uc2:ProjectMeetingAgenda runat="server" ID="ucProjectMeetingAgenda"></uc2:ProjectMeetingAgenda>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView3" > 
                    <uc3:ProjectMeetingDocuments runat="server" ID="ucProjectMeetingDocuments"></uc3:ProjectMeetingDocuments>                     
                </telerik:RadPageView>
   </telerik:RadMultiPage>
</div>
