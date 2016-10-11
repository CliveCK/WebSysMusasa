<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CommunityDetailsControl.ascx.vb" Inherits="WebSysME.CommunityDetailsControl" %>

<div style="margin-left:2%">
<table cellpadding="3" cellspacing="0" border="0" style="width:90%;margin-left:2%"> 
	<tr> 
		<td colspan="5" class="PageTitle"><h4>Community Details</h4><br /></td> 
	</tr> 
    <tr> 
		<td class="auto-style5" >Province</td> 
        	<td class="auto-style3" ><asp:dropdownlist id="cboProvince" runat="server" AutoPostBack="true" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
	</tr> 
    <tr> 
		<td class="auto-style6" >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td class="auto-style6" >Ward</td> 
        	<td ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td class="auto-style6" >No Of Households</td> 
        	<td ><asp:textbox id="txtNoOfHouseholds" runat="server"  CssClass="form-control"></asp:textbox> </td> 
		<td >No Of Individual Adult Males</td> 
        	<td ><asp:textbox id="txtNoOfIndividualAdultMales" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td></td>
	</tr> 
	<tr> 
		<td class="auto-style6" >No Of Individual Adult Females</td> 
        	<td ><asp:textbox id="txtNoOfIndividualAdultFemales" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >No Of Male Youths</td> 
        	<td ><asp:textbox id="txtNoOfMaleYouths" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td></td>
	</tr> 
	<tr> 
		<td class="auto-style5" >No Of Female Youth</td> 
        	<td class="auto-style3" ><asp:textbox id="txtNoOfFemaleYouth" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td class="auto-style3" >No Of Children</td> 
        	<td class="auto-style3" ><asp:textbox id="txtNoOfChildren" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td class="auto-style3"></td>
	</tr> 
	<tr> 
		<td class="auto-style6" >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server" CssClass="form-control"></asp:textbox> </td> 
        <td></td>
	</tr> 
	<tr> 
		<td colspan="5" class="auto-style4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="5"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="5"> 
			<asp:TextBox id="txtCommunityID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr>
        <tr>
        <td class="auto-style6">Date<telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        <td>Thematic Area<asp:DropDownList ID="cboThermaticArea" runat="server" CssClass="form-control" ></asp:DropDownList></td>
        <td>Indicator<asp:DropDownList ID="cboIndicator" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList></td>
        <td>Score<asp:TextBox ID="txtScore" runat="server" CssClass="form-control"></asp:TextBox>        
        </td>
        <td style="text-align:left;padding:0"><asp:Button ID="cmdPlus" runat="server" Text="+" CssClass="btn btn-default" /></td>
    </tr>
    <tr>
        <td colspan="5">
             <telerik:RadGrid ID="radCommunityScoreListing" runat="server" GridLines="None"  
                    CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" PageSize="5"
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="CommunityScoreCardID" UniqueName="CommunityScoreCardID" HeaderText="CommunityScoreCardID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Date" UniqueName="Date" HeaderText="Date"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Area" UniqueName="Area" HeaderText="Area"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Indicator" UniqueName="Indicator" HeaderText="Indicator">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Score" UniqueName="Score" HeaderText="Score">
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
                        <PagerStyle Position="Bottom" />
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="false">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid><br />
        </td>
    </tr> 
</table> 
</div>
