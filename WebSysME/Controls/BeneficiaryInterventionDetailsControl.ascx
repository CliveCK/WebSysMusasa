<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BeneficiaryInterventionDetailsControl.ascx.vb" Inherits="WebSysME.BeneficiaryInterventionDetailsControl" %>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><br /><h4>Beneficiary Intervention Mapping</h4><br /></td> 
	</tr> 
    <tr> 
		<td >Beneficiary Type</td> 
        	<td ><asp:dropdownlist id="cboBeneficiaryType" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Intervention</td> 
            <td ><telerik:RadAutoCompleteBox runat="server" ID="cboInterventions" EmptyMessage="Please type here"
                TextSearchMode="Contains" SelectMethod="Single" InputType="Text" Width="350" DropDownWidth="150px" CssClass="form-control">
            </telerik:RadAutoCompleteBox></td>
	</tr> 
	<tr>
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
    <tr>
        <td colspan="2">
            <telerik:RadGrid ID="radObjects" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="231%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                           <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn>                        
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
	<tr> 
		<td colspan="4"> <br />
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtBeneficiaryInterventionID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
