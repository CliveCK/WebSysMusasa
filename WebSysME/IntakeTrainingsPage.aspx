<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IntakeTrainingsPage.aspx.vb" Inherits="WebSysME.IntakeTrainingsPage" MasterPageFile="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server" >
    <div style="padding-left:2%">
<table cellpadding="2" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="2" class="PageTitle"><h4>Intake Training Mapping</h4></td> 
	</tr> 
	<tr>
		<td >Intake</td>
        	<td ><asp:dropdownlist id="cboIntake" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td> 
	</tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
	<tr>
        <td>Training</td>
    </tr>
    <tr>
        <td colspan="2">
            <telerik:RadGrid ID="radTrainings" runat="server" GridLines="None" Height="100%" AllowMultiRowSelection="True"
                      CellPadding="0" Width="80%">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    <MasterTableView AllowFilteringByColumn="True" AllowMultiColumnSorting="True" AllowPaging="True"
                    AllowSorting="True" CommandItemDisplay="Top" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TrainingID")%>'
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
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
	</tr>
	<tr> 
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="2"> 
            		<asp:button id="cmdSave" runat="server" Text="Map" CssClass="btn btn-default"></asp:button>
     </td> 
	</tr>
</table>
</div> 


</asp:Content>
