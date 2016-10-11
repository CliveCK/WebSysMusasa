<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CurrencyManagement.aspx.vb" Inherits="WebSysME.CurrencyManagement" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1" >
    <script type="text/javascript">
 
        function isNumberKey(evt, obj) {
 
            var charCode = (evt.which) ? evt.which : event.keyCode
            var value = obj.value;
            var dotcontains = value.indexOf(".") != -1;
            if (dotcontains)
                if (charCode == 46) return false;
            if (charCode == 46) return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
</script>
        <table cellpadding="3" style="width:80%;margin-left:2%">
            <tr>
                <td colspan="4"><h4>Currency Management Console</h4><br /></td>
            </tr>
            <tr>
                <td colspan="4"><h4>Exchange Rates</h4></td>
            </tr>
            <tr>
                <td>From</td>
                <td><asp:DropDownList runat="server" ID="cboFrom" CssClass="form-control" Width="120px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>To</td>
                <td><asp:DropDownList runat="server" ID="cboTo" CssClass="form-control" Width="120px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Rate</td>
                <td><asp:TextBox ID="txtRate" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event,this)" Width="150px"></asp:TextBox></td>
            </tr>
            <tr>
                <td >Date Effective</td>
                <td colspan="3"><telerik:RadDatePicker ID="radDateEffective" runat="server" MinDate="1900-01-01">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
            </tr> 
            <tr> 
		        <td colspan="4"><asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel></td> 
	        </tr>           
        <tr>
            <td colspan="4"><asp:Button  runat="server" ID="btnSave" Text="Save" CssClass="btn btn-default"/>
        </tr>
            <tr>
                <td colspan="4"><telerik:RadGrid ID="radCurrency" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
                        <Columns>
                            <telerik:GridBoundColumn DataField="CurrencyExchangeRateID" UniqueName="CurrencyExchangeRateID" HeaderText="CurrencyExchangeRateID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="FromCurrency" UniqueName="FromCurrency" HeaderText="FromCurrency" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ToCurrency" UniqueName="ToCurrency" HeaderText="ToCurrency">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Rate" UniqueName="Rate" HeaderText="Rate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateEffective" UniqueName="DateEffective" HeaderText="DateEffective">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CurrencyExchangeRateID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove this entry?')"
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
                        <PagerStyle Position="Top" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid></td>
            </tr>        
    </table>
</asp:Content>
