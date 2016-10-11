<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InterventionsDetailsControl.ascx.vb" Inherits="WebSysME.InterventionsDetailsControl" %>

<style type="text/css">
    .auto-style1 {
        width: 235px;
    }
    .auto-style2 {
        width: 236px;
    }
</style>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Intervention Details</h4><br /></td> 
	</tr> 
    <tr style="visibility:collapse"> 
		<td >Programme</td> 
        	<td ><asp:dropdownlist id="cboProgram" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
    <tr> 
		<td >Project</td> 
        	<td ><asp:dropdownlist id="cboProject" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
    <tr> 
		<td >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Beneficiaries Target</td> 
        	<td ><asp:textbox id="txtBeneficiariesTarget" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Actual Benficiaries</td> 
        	<td ><asp:textbox id="txtActualBenficiaries" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Start Date</td> 
        	<td > <telerik:RadDatePicker ID="radStartDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >End Date</td> 
        	<td ><telerik:RadDatePicker ID="radEndDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>  
	</tr> 
    <tr> 
		<td >Review Date</td> 
        	<td > <telerik:RadDatePicker ID="radReviewDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
    </tr> 
	<tr> 
		<td >Description Of Beneficiaries</td> 
        	<td ><asp:textbox id="txtDescriptionOfBeneficiaries" runat="server" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtInterventionID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr><td>
        <br />
        </td></tr>
    <tr style="visibility:collapse">
        <td>
            <asp:LinkButton runat="server" ID="lnkPartner" Text="Implementing Partner" ></asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton runat="server" ID="lnkLocations" Text="Locations" ></asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton runat="server" ID="lnkStaffMembers" Text="Staff Members" ></asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton runat="server" ID="lnkBeneficiaries" Text="Beneficiaries" ></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td>
            <h4>Intervention Listing</h4><br />
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radInterventionListing" runat="server" GridLines="None" Height="100%" 
                     AllowFilteringByColumn="True" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="InterventionID" UniqueName="InterventionID" HeaderText="InterventionID"
                                Display="false">                            
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>                            
                            <telerik:GridBoundColumn DataField="Project" UniqueName="Project" HeaderText="Project">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BeneficiariesTarget" UniqueName="BeneficiariesTarget" HeaderText="Beneficiaries Target">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ActualBeneficiaries" UniqueName="ActualBeneficiaries" HeaderText="ActualBeneficiaries">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StartDate" UniqueName="StartDate" HeaderText="StartDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EndDate" UniqueName="EndDate" HeaderText="EndDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
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
