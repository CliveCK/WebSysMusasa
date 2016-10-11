<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LookupListsManager.aspx.vb" Inherits="WebSysME.LookupListsManager" MasterPageFile="~/Site.Master"%>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">
    <script type="text/javascript" language="JavaScript">
        function check() {
            if (confirm('Are you sure you want to delete?')) {
                return true;
            }
            else {
                return false;
            }
            return true;
        }
		</script>
			<table id="table1" cellspacing="1" cellpadding="1" width="100%" border="0" style="margin-left:2%">
				<tr>
					<td class="heading" colspan="4">
                        <span style="font-size: 14pt; color: midnightblue">Lookup List Manager</span></td>
				</tr>
				<tr>
					<td class="detail" width="25%" ></td>
					<td class="detail" width="25%" ></td>
					<td class="detail" width="25%" ></td>
					<td class="detail" width="25%" ></td>
				</tr>
				<tr>
					<td colspan="4"><asp:textbox id="txtListName" runat="server" CssClass="HiddenControl"></asp:textbox><asp:label id="lblNotice" runat="server" Width="100%" ForeColor="Red" EnableViewState="False"></asp:label></td>
				</tr>
				<tr>
					<td colspan="4"></td>
				</tr>
				<tr>
					<td valign="top"><asp:datagrid id="dgLookupLists" runat="server" Width="97%" GridLines="None" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False">
							<SelectedItemStyle Font-Bold="True" ForeColor="#333333" BackColor="#C5BBAF"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#1C5E55"></HeaderStyle>
							<FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"></FooterStyle>
							<Columns>
								<asp:ButtonColumn Text="+" CommandName="Select"></asp:ButtonColumn>
								<asp:TemplateColumn HeaderText="ListName">
									<ItemTemplate>
										<asp:Label id="lblListName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ListName") %>' Font-Bold="true">
										</asp:Label>&nbsp;<strong>-</strong><br />
										<asp:Label id="lblListdescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Listdescription") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="Editable" SortExpression="Editable" HeaderText="Editable"
									FooterText="Editable"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#666666"></PagerStyle>
                        <EditItemStyle BackColor="#7C6F57" />
                        <ItemStyle BackColor="#E3EAEB" />
						</asp:datagrid></td>
					<td valign="top" colspan="3"><asp:panel id="pnlFilter" runat="server" Visible="False">
							<table id="table2" cellspacing="1" cellpadding="0" width="100%" border="0">
								<tr>
									<td nowrap="nowrap" width="25%" rowspan="4">
										<asp:ListBox id="lstColumns" runat="server" Width="90%" AutoPostBack="true"></asp:ListBox></td>
									<td nowrap="nowrap" width="5%">Column</td>
									<td nowrap="nowrap" width="25%">
										<asp:TextBox id="txtFilterColumn" runat="server" BackColor="Gainsboro" ReadOnly="true"></asp:TextBox></td>
									<td nowrap="nowrap" align="right" width="25%" rowSpan="4">
										<asp:ListBox id="lstFilters" runat="server" Width="90%" DESIGNTIMEDRAGDROP="90"></asp:ListBox></td>
								</tr>
								<tr>
									<td  nowrap="nowrap">Operation</td>
									<td  nowrap="nowrap">
										<asp:DropDownList id="cboFilterOperation" runat="server">
											<asp:ListItem Value="=">Equal</asp:ListItem>
											<asp:ListItem Value="&gt;">Greater than</asp:ListItem>
											<asp:ListItem Value="&gt;=">Greater than or equal to</asp:ListItem>
											<asp:ListItem Value="&lt;">Less than</asp:ListItem>
											<asp:ListItem Value="&lt;=">Less than or equal to</asp:ListItem>
											<asp:ListItem Value="INStr">Contains</asp:ListItem>
										</asp:DropDownList></td>
								</tr>
								<tr>
									<td nowrap="nowrap">Value</td>
									<td nowrap="nowrap">
										<asp:TextBox id="txtFilterValue" runat="server"></asp:TextBox></td>
								</tr>
								<tr>
									<td nowrap="nowrap"></td>
									<td nowrap="nowrap"></td>
								</tr>
								<tr>
									<td nowrap="nowrap"></td>
									<td nowrap="nowrap">
										<asp:Button id="cmdAddFilter" runat="server" Text="Add"></asp:Button></td>
									<td nowrap="nowrap"></td>
									<td nowrap="nowrap">
										<p align="right">
											<asp:Button id="Button2" runat="server" Text="Remove"></asp:Button></P>
									</td>
								</tr>
								<tr>
									<td nowrap="nowrap" colspan="4">
										<asp:TextBox id="txtFilter" runat="server" Width="100%"></asp:TextBox></td>
								</tr>
								<tr>
									<td nowrap="nowrap" align="right" colspan="4">
										<asp:Button id="cmdFilter" runat="server" Text="Apply Filter"></asp:Button></td>
								</tr>
							</table>
						</asp:panel><asp:datagrid id="dgData" runat="server" Width="100%" GridLines="None" CellPadding="4" ForeColor="#333333">
							<SelectedItemStyle Font-Bold="True" ForeColor="#333333" BackColor="#C5BBAF"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#1C5E55"></HeaderStyle>
							<FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"></FooterStyle>
							<Columns>
								<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel" EditText="Edit" >
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:EditCommandColumn>
								<asp:ButtonColumn Text="Delete" ButtonType="PushButton" CommandName="Delete" ></asp:ButtonColumn>
							</Columns>
							<pagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#666666"></PagerStyle>
                            <EditItemStyle BackColor="#7C6F57" />
                            <ItemStyle BackColor="#E3EAEB" />
						</asp:datagrid><br />
                        <asp:button id="cmdAdd" runat="server" Visible="False" Text="Add New" Enabled="False"></asp:button></td>
				</tr>
				<tr>
					<td colspan="4"><asp:label id="lblError" runat="server" Width="100%" ForeColor="Red" EnableViewState="False"></asp:label></td>
				</tr>
				<tr>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
				</tr>
			</table>
</asp:Content>