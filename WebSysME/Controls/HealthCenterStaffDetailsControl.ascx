<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HealthCenterStaffDetailsControl.ascx.vb" Inherits="WebSysME.HealthCenterStaffDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Health Center Staff</h4></td> 
	</tr>
     <tr>
        <td >Departments</td> 
        	<td ><asp:DropDownList id="cboDepartment" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
    </tr>
    <tr>
        <td >Group Type</td> 
        	<td ><asp:DropDownList id="cboGroupType" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
    </tr>
	<tr> 
		<td >Staff Role</td> 
        	<td ><asp:dropdownlist id="cboStaffRole" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
        <td >Title</td> 
        	<td ><asp:DropDownList id="cboTitle" runat="server" CssClass="form-control"></asp:DropDownList> </td>
    </tr>
	<tr> 
		<td >First Name</td> 
        	<td ><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Surname</td> 
        	<td ><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr>        
		<td >D O B</td> 
        	<td ><telerik:RadDatePicker ID="radDOB" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Sex</td> 
        	<td ><asp:DropDownList id="cboSex" runat="server" CssClass="form-control" >
                    <asp:ListItem Text="M" Value="M"></asp:ListItem>
                    <asp:ListItem Text="F" Value="F"></asp:ListItem>
        	     </asp:DropDownList> </td> 
	</tr> 
    <tr>
        <td>ID Number</td>
        <td ><asp:textbox id="txtIDNumber" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td>Email</td>
        <td ><asp:textbox id="txtEmail" runat="server" CssClass="form-control"></asp:textbox> </td> 
    </tr>
    <tr>
        <td>Site</td>
        <td ><asp:textbox id="txtSite" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td>Contact No</td>
        <td ><asp:textbox id="txtContactNo" runat="server" CssClass="form-control"></asp:textbox> </td> 
    </tr>
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="2"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdNew" runat="server" Text="New" CssClass="btn btn-default"></asp:button>
     </td> 
        <td>
                    <asp:button id="cmdDelete" runat="server" Text="Delete" CssClass="btn btn-default"></asp:button>
        </td>
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtHealthCenterStaffID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
            <asp:TextBox id="txtHealthCenterID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr>
     <tr>
            <td colspan="4">
                <telerik:RadGrid ID="radStaffListing" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="HealthCenterStaffID" UniqueName="HealthCenterStaffID" HeaderText="HealthCenterStaffID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="HealthCenter" UniqueName="HealthCenter" HeaderText="HealthCenter">
                            </telerik:GridBoundColumn>                            
                            <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Title" UniqueName="Title" HeaderText="Title">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DOB" UniqueName="DOB" HeaderText="DateOfBirth">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StaffRole" UniqueName="StaffRole" HeaderText="StaffRole">
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
                        <PagerStyle Position="Top" AlwaysVisible="true"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr> 
</table> 
