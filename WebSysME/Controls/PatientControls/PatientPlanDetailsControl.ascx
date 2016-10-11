<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PatientPlanDetailsControl.ascx.vb" Inherits="WebSysME.PatientPlanDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="8" class="PageTitle"><h4>Patient Management - Plan</h4></td> 
	</tr> 
	<tr> 
        <td >Need/Service Category</td> 
        	<td ><asp:DropDownList id="cboNeedCategory" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList> </td>
    </tr>
    <tr>
		<td >Need/Service</td> 
        	<td ><asp:DropDownList id="cboNeed" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Action</td> 
        	<td ><asp:DropDownList id="cboAction" runat="server" CssClass="form-control" ></asp:DropDownList> </td>
		<td >Service Provider</td> 
        	<td ><asp:DropDownList id="cboServiceProvider" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
    <tr>
        <td >Plan Date</td> 
        	<td > <telerik:RadDatePicker ID="radPlanDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
        <td >Actual Date</td> 
        	<td > <telerik:RadDatePicker ID="radActualDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Cost</td> 
        	<td ><asp:textbox id="txtCost" runat="server" CssClass="form-control"></asp:textbox> </td> 
        </tr>
    <tr>
		<td >Comments</td> 
        	<td ><asp:textbox id="txtComments" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td></td>
        <td></td>
    </tr>
	<tr> 
		<td colspan="8"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="8"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="8"> 
			<asp:TextBox id="txtPatientPlanID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox id="txtPatientID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
     <tr>
            <td colspan="8">
                <telerik:RadGrid ID="radPatientListing" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="PatientPlanID" UniqueName="PatientPlanID" HeaderText="PatientPlanID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="PlanDate" UniqueName="PlanDate" HeaderText="Plan Date">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ActualDate" UniqueName="ActualDate" HeaderText="Actual Date">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NeedCategory" UniqueName="NeedCategory" HeaderText="Need/Service Category">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Need" UniqueName="Need" HeaderText="Need/Service">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Action" UniqueName="Action" HeaderText="Action">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ServiceProvider" UniqueName="ServiceProvider" HeaderText="ServiceProvider">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Cost" UniqueName="Cost" HeaderText="Cost">
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
