<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProjectKeyChangePromise.aspx.vb" Inherits="WebSysME.ProjectKeyChangePromise" MasterPageFile="~/Site.Master"%>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server" >
    <table>
        <tr> 
		    <td colspan="4" class="PageTitle"><h4>Project & KeyChange Promises</h4><br /></td> 
	    </tr> 
        <tr> 
		    <td colspan="4"> 
            		    <asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
         </td> 
	    </tr> 
        <tr>
            <td >Project</td> 
        	    <td ><asp:dropdownlist id="cboProjects" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td>
        </tr>
        <tr>
            <td colspan="8"><br />
            <telerik:RadGrid ID="radKeyChange" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="231%">
                    <MasterTableView AutoGenerateColumns="True" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="KeyChangePromiseID" UniqueName="KeyChangePromiseID" HeaderText="KeyChangePromiseID" Display="false" >
                            </telerik:GridBoundColumn>
                           <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn> 
                            <telerik:GridBoundColumn DataField="StrategicObjective" UniqueName="StrategicObjective" HeaderText="StrategicObjective" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="KeyChangePromiseNo" UniqueName="KeyChangePromiseNo" HeaderText="KeyChangePromiseNo" >
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
        <tr> 
		<td> <br />
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass ="btn btn-default"></asp:button> 
     </td> 
	</tr>
    </table>
</asp:Content>