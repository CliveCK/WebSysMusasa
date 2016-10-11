<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TripTravellers.ascx.vb" Inherits="WebSysME.TripTravellers" %>

<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td class="PageTitle"><h5><b>Project Trip attendants</b></h5><br /></td> 
	</tr>
    <tr>
        <td>Staff</td>
    </tr>
    <tr>
		<td >
             <telerik:RadGrid ID="radStaff" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" AllowMultiRowSelection="true" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <PagerStyle AlwaysVisible ="true" Position="Top"/>
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="StaffID" UniqueName="StaffID" HeaderText="StaffID" Display="false" >
                            </telerik:GridBoundColumn>
                             <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn>
                            <telerik:GridBoundColumn DataField="Organization" UniqueName="Organization" HeaderText="Organization" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Firstname" UniqueName="Firstname1" HeaderText="Firstname" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex" >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Position" UniqueName="Position" HeaderText="Position" >
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "StaffID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove from Meeting?')"
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
                    <ClientSettings Selecting-AllowRowSelect ="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td> 
	</tr> 
	<tr> 
		<td> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td> 
			<asp:TextBox  id="txtTripTravellerID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox  id="txtTripID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
    </tr>
</table> 
