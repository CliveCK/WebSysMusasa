<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProjectMeetingAgendaDetailsControl.ascx.vb" Inherits="WebSysME.ProjectMeetingAgendaDetailsControl" %>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h5><b>Project Meeting Agenda Details</b></h5><br /></td> 
	</tr>
	<tr> 
		<td >Activity</td> 
        	<td ><asp:textbox id="txtActivity" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Comments</td> 
        	<td ><asp:textbox id="txtComments" runat="server" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtProjectMeetingAgendaID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox id="txtProjectMeetingID" runat="server" CssClass="HiddenControl"></asp:TextBox> <br />
		</td> 
	</tr> 
    <tr>
            <td colspan="3">
                 <telerik:RadGrid ID="radAgenda" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ProjectMeetingAgendaID" UniqueName="ProjectMeetingAgendaID" HeaderText="ProjectMeetingAgendaID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ProjectMeetingID" UniqueName="ProjectMeetingID" HeaderText="ProjectMeetingID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="Activity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Comments" UniqueName="Comments" HeaderText="Comments">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
    </tr> 
</table> 
