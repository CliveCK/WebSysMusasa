<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MapLocationsPage.aspx.vb" Inherits="WebSysME.MapLocationsPage" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Location Settings</h4><br /></td> 
	</tr> 
    <tr> 
		<td colspan="8"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
    <tr>
        <td >Country</td> 
        	<td ><asp:dropdownlist id="cboCountry" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td>
    </tr>
    <tr> 
		<td >Province</td> 
        	<td ><asp:dropdownlist id="cboProvince" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
        <td >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td>
        <td >Ward</td> 
        	<td ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td>  
	</tr> 
    <tr> 
		<td >City</td> 
        	<td ><asp:dropdownlist id="cboCity" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
        <td >Suburb</td> 
        	<td ><asp:dropdownlist id="cboSuburb" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td>
        <td >Section</td> 
        	<td ><asp:dropdownlist id="cboSection" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td>  
        <td >Street</td> 
        	<td ><asp:dropdownlist id="cboStreet" runat="server" CssClass="form-control" AutoPostBack="true" ></asp:dropdownlist> </td>  
	</tr>
	<tr> 
		<td colspan="8"><br />
            <asp:RadioButtonList runat="server" ID="rbLstSaveOption" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="true">
                <asp:ListItem Text="Country" Value="Country" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Province" Value="Province"></asp:ListItem>
                <asp:ListItem Text="District" Value="District"></asp:ListItem>
                <asp:ListItem Text="Ward" Value="Ward"></asp:ListItem>
                <asp:ListItem Text="City" Value="City"></asp:ListItem>
                <asp:ListItem Text="Suburb" Value="Suburb"></asp:ListItem>
                <asp:ListItem Text="Section" Value="Section"></asp:ListItem>
                <asp:ListItem Text="Street" Value="Street"></asp:ListItem>
            </asp:RadioButtonList>
		</td>
	</tr> 
    <tr>
        <td><br />
            Map to:
        </td>
        <td>
            <asp:DropDownList ID="cboObjectType" runat="server" CssClass="form-control" AutoPostBack="true" ></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="8"><br />
            <telerik:RadGrid ID="radObjects" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" AllowMultiRowSelection="true"  CellPadding="0" Width="80%">
                    <MasterTableView AutoGenerateColumns="True" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ObjectID" UniqueName="ObjectID" HeaderText="ObjectID" Display="false" >
                            </telerik:GridBoundColumn>
                           <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn> 
                             <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ObjectID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove from Training?')"
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
                    <ClientSettings Selecting-AllowRowSelect="true" >
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
	<tr> 
		<td colspan="4"> <br />
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass ="btn btn-default"></asp:button> 
     </td> 
	</tr>
</table> 
</asp:Content>