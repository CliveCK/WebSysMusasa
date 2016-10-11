
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="DatePickerControl.ascx.vb" Inherits="WebSysME.DatePickerControl" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
    <table>
        <tr>
            <td nowrap="nowrap">
                <asp:DropDownList ID="cboDateRange" runat="server">
                    <asp:ListItem>On</asp:ListItem>
                    <asp:ListItem>Between</asp:ListItem>
                    <asp:ListItem>After</asp:ListItem>
                    <asp:ListItem>Before</asp:ListItem>
                    <asp:ListItem Value="Last">In The Last</asp:ListItem>
                    <asp:ListItem Selected="True">Any Date</asp:ListItem>
                </asp:DropDownList>&nbsp;&nbsp;
                <telerik:RadDatePicker ID="radStartDate" runat="server" >
                </telerik:RadDatePicker>
                <asp:Label ID="lblAnd" runat="server" EnableViewState="False"> And </asp:Label>&nbsp;<telerik:RadDatePicker
                    ID="radEndDate" runat="server" >
    
                </telerik:RadDatePicker>
                &nbsp;<asp:TextBox ID="txtInthe" runat="server" Width="88px"></asp:TextBox>
                <asp:DropDownList ID="cboInthe" runat="server">
                    <asp:ListItem Value="dd">Days</asp:ListItem>
                    <asp:ListItem Value="ww">Weeks</asp:ListItem>
                    <asp:ListItem Value="mm">Months</asp:ListItem>
                    <asp:ListItem Value="yy">Years</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
    </table>
