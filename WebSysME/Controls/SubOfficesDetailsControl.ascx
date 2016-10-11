<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubOfficesDetailsControl.ascx.vb" Inherits="WebSysME.SubOfficesDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>SubOffices Details</h4><br /></td> 
	</tr> 
	<tr> 
		<td >Organization:</td> 
        	<td ><asp:Label id="lblOrgName" runat="server" Font-Bold="true" ></asp:Label> </td> 
	</tr> 
	<tr> 
		<td >Contact No</td> 
        	<td ><asp:textbox id="txtContactNo" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Fax</td> 
        	<td ><asp:textbox id="txtFax" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Name</td> 
        	<td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Email</td> 
        	<td ><asp:textbox id="txtEmail" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Physical Address</td> 
        	<td ><asp:textbox id="txtPhysicalAddress" runat="server" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtSubOfficeID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox id="txtOrganizationID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr>
    <tr>
        <td>
            <asp:LinkButton ID="lnkBack" runat="server" Text="Back to previous page"></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td colspan="4">
         <telerik:RadGrid ID="radSubListing" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="SubOfficeID" UniqueName="SubOfficeID" HeaderText="SubOfficeID"
                                Display="false">
                            </telerik:GridBoundColumn>                            
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                            </telerik:GridBoundColumn> 
                             <telerik:GridBoundColumn DataField="ContactNo" UniqueName="ContactNo" HeaderText="ContactNo"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Fax" UniqueName="Fax" HeaderText="Fax"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Email" UniqueName="Email" HeaderText="Email">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PhysicalAddress" UniqueName="PhysicalAddress" HeaderText="PhysicalAddress">
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
