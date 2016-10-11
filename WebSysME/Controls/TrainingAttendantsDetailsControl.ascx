<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TrainingAttendantsDetailsControl.ascx.vb" Inherits="WebSysME.TrainingAttendantsDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="2" class="PageTitle"><h4>Training Attendants Details</h4><br /></td> 
	</tr> 
	<tr> 
		<td >Training</td> 
        	<td ><asp:dropdownlist id="cboTraining" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Beneficiary Type</td> 
        	<td ><asp:dropdownlist id="cboBeneficiaryType" runat="server" CssClass="form-control" AutoPostBack="true" ></asp:dropdownlist> </td> 
    </tr>
    <tr>
		<td >Beneficiary<br /> <br /></td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox runat="server" ID="cbxShowMapped" Text="Show Selected Only" AutoPostBack="true" />
        </td>
    </tr>
    <tr >
        <td>Group Type
            <asp:DropDownList ID="cboHealthGroupType" runat="server" CssClass="form-control"></asp:DropDownList>
        </td>
        <td>
            Province
            <asp:DropDownList ID="cboProvince" runat="server" CssClass="form-control"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="btn btn-default"/>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">
            <telerik:RadGrid ID="radBeneficiaries" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" AllowMultiRowSelection="true" CellPadding="0" Width="80%">
                    <MasterTableView AutoGenerateColumns="True" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <PagerStyle AlwaysVisible ="true" Position="Top"/>
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
                    <ClientSettings Selecting-AllowRowSelect ="true">
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
			<asp:TextBox  id="txtTrainingAttendantID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
    <tr>
        <td>
            <asp:LinkButton runat="server" ID="lnkBack" Text="<< Back to Training Details" Font-Bold="true"></asp:LinkButton>
        </td>
    </tr>
</table> 
