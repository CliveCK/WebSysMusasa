<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GrantProposalDetailsControl.ascx.vb" Inherits="WebSysME.GrantProposalDetailsControl" %>
<table cellpadding="3" cellspacing="0" border="0" style="margin-left:2%;width:90%"> 
	<tr > 
		<td colspan="4" class="PageTitle"><h4>Grant Proposal Details</h4></td> 
	</tr> 
	<tr> 
		<td >Action</td> 
        	<td ><asp:dropdownlist id="cboAction" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Donor Name</td> 
        	<td ><asp:textbox id="txtDonorName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Proposal Title</td> 
        	<td ><asp:textbox id="txtProposalTitle" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Proposed Project Name</td> 
        	<td ><asp:textbox id="txtProposedProjectName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Comments</td> 
        	<td ><asp:textbox id="txtComments" runat="server" TextMode="MultiLine" Rows="4" Columns="30" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
    <tr>
        <td>Attachment of Proposal Document</td>
        <td><asp:FileUpload ID="fuUpload" runat="server" /></td>
    </tr>
    <tr>
        <td>Date</td>
        <td><telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        <td>Status</td>
        <td><asp:DropDownList runat="server" ID="cboStatus" CssClass="form-control"></asp:DropDownList></td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radStatus" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="GrantProposalStatusID" UniqueName="StatusID" HeaderText="StatusID" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StatusDate" UniqueName="StatusDate" HeaderText="StatusDate">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Status" UniqueName="Status" HeaderText="Status">
                            </telerik:GridBoundColumn>
                             <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GrantProposalStatusID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove this entry?')"
                                    ToolTip="Click to remove " />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                  
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
			<asp:TextBox id="txtGrantProposalID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radAttachments" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="FileID" UniqueName="FileID" HeaderText="FileID" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FilePath" UniqueName="FilePath" HeaderText="FilePath" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Title" UniqueName="Title" HeaderText="Title">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="PushButton" Text="Download" UniqueName="column"
                                CommandName="Download" ButtonCssClass="btn btn-default">
                            </telerik:GridButtonColumn>                   
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
