<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IndicatorCategoryPage.aspx.vb" Inherits="WebSysME.IndicatorCategoryPage" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="padding-left:2%">
<table cellpadding="2" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td class="PageTitle"><h3>Indicator Category</h3></td> 
	</tr> 
	<tr>
		<td >Category</td> 
    </tr>
    <tr>
        	<td ><asp:dropdownlist id="cboIndicatorCategory" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
	<tr>
        <td>Indicators</td>
    </tr>
    <tr>
        <td>
            <telerik:RadGrid ID="radIndicators" runat="server" GridLines="None" Height="100%" AllowMultiRowSelection="True"
                      CellPadding="0" Width="80%">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    <MasterTableView AllowFilteringByColumn="True" AllowMultiColumnSorting="True" AllowPaging="True"
                    AllowSorting="True" CommandItemDisplay="Top" PagerStyle-Mode="NextPrevNumericAndAdvanced" AutoGenerateColumns="false">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="IndicatorID" UniqueName="IndicatorID" HeaderText="IndicatorID"
                                Display="false">                            
                            </telerik:GridBoundColumn>
                            <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn>
                             <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Definition" UniqueName="Definition" HeaderText="Definition">
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
            		<asp:button id="cmdSave" runat="server" Text="Map" CssClass="btn btn-default"></asp:button>
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtOutputActivityID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table>
</div> 
</asp:Content>
