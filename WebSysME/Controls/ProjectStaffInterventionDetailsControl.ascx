<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProjectStaffInterventionDetailsControl.ascx.vb" Inherits="WebSysME.ProjectStaffInterventionDetailsControl" %>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h1>ProjectStaff - Intervention Details</h1><br /></td> 
	</tr>
    <tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel><br />
     </td> 
    <tr> 
		<td >Programme</td> 
        	<td ><asp:dropdownlist id="cboProgramme" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
    <tr> 
		<td >Project</td> 
        	<td ><asp:dropdownlist id="cboProject" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
        <td >Intervention</td> 
        	<td ><asp:dropdownlist id="cboIntervention" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
     </tr>
    <tr>
        <td >Organization</td> 
        	<td ><asp:dropdownlist id="cboOrganization" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Staff</td> 
        	<td ><asp:dropdownlist id="cboStaff" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtProjectStaffInterventionID" runat="server"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td>
            <telerik:RadGrid ID="radStaff" runat="server" GridLines="None" Height="100%" AllowMultiRowSelection="True"
                      CellPadding="0" Width="100%">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    <MasterTableView AllowFilteringByColumn="True" AllowMultiColumnSorting="True" AllowPaging="True"
                    AllowSorting="True" CommandItemDisplay="Top" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="StaffID" UniqueName="StaffID" HeaderText="StaffID"
                                Display="false">                            
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
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
</table> 
