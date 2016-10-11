<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="KeyChangePromisesDetailsControl.ascx.vb" Inherits="WebSysME.KeyChangePromisesDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>KeyChange Promises Details</h4></td> 
	</tr> 
	<tr> 
		<td >Strategic Objective</td> 
        	<td ><asp:dropdownlist id="cboStrategicObjective" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Key Change Promise No</td> 
        	<td ><asp:textbox id="txtKeyChangePromiseNo" runat="server" CssClass="form-control"></asp:textbox> </td> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="New" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtKeyChangePromiseID" runat="server" CssClass="HiddenControl"></asp:TextBox> <br />
		</td> 
	</tr> 
</table> 
<asp:Panel ID="pnlDescription" runat="server" Visible="false">
    <table style="margin-left:2%">
        <tr>
            <td><h4>Add Descriptions</h4><br /></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblPromiseNo" Text="<%# txtKeyChangePromiseNo.Text %>" BackColor="LightGray"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows ="8" Columns="70" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:button id="cmdAddDescription" runat="server" Text="Add Description" CssClass="btn btn-default"></asp:button>
                <asp:button id="cmdClearDescription" runat="server" Text="New" CssClass="btn btn-default"></asp:button> <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtKeyChangePromiseDescriptionID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <telerik:RadGrid ID="radKeyChangeDescription" runat="server" GridLines="None" Height="100%" AllowMultiRowSelection="True"
                      CellPadding="0" Width="700px">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    <MasterTableView AllowFilteringByColumn="True" AllowMultiColumnSorting="True" AutoGenerateColumns="false"  AllowPaging="True"
                    AllowSorting="True" CommandItemDisplay="Top" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <PagerStyle AlwaysVisible="true" />
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="KeyChangePromiseDescriptionID" UniqueName="KeyChangePromiseDescriptionID" HeaderText="KeyChangePromiseDescriptionID"
                                Display="false">                            
                            </telerik:GridBoundColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn> 
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">                            
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "KeyChangePromiseDescriptionID")%>'
                                        CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to delete?')"
                                        ToolTip="Click to delete " />
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
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
</asp:Panel>
