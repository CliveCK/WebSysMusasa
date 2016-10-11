<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DeleteUser.ascx.vb" Inherits="WebSysME.DeleteUser" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<table width="100%">
    <tr>
        <td colspan="4" rowspan="1">
            <asp:Panel ID="pnlFindUsers" runat="server" Height="100px" Width="100%">
                <table width="100%">
                    <tr>
                        <td style="width: 301px; height: 21px">
            Username</td>
                        <td style="width: 321px; height: 21px">
                            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox></td>
                        <td style="width: 325px; height: 21px">
                            Surname</td>
                        <td style="width: 411px; height: 21px">
                            <asp:TextBox ID="txtSurname" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 301px">
                            Firstname</td>
                        <td style="width: 321px">
                            <asp:TextBox ID="txtFirstname" runat="server"></asp:TextBox></td>
                        <td style="width: 325px">
                            Email Address</td>
                        <td style="width: 411px">
                            <asp:TextBox ID="txtEmailAddress" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 301px; height: 21px">
                        </td>
                        <td style="width: 321px; height: 21px">
                        </td>
                        <td style="width: 325px; height: 21px">
                        </td>
                        <td style="width: 411px; height: 21px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 301px; height: 21px">
                            <asp:Button ID="cmdFind" runat="server" Text="Find User(s)" /></td>
                        <td style="width: 321px; height: 21px">
                        </td>
                        <td style="width: 325px; height: 21px">
                        </td>
                        <td style="width: 411px; height: 21px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="4" rowspan="2">
            <telerik:RadGrid ID="RadGrid1" runat="server">
            </telerik:RadGrid></td>
    </tr>
    <tr>
    </tr>
    <tr>
        <td colspan="1" style="width: 233px">
            <asp:CheckBox ID="chkDeleteRelatedRecords" runat="server" Text="Delete All Related Records" /></td>
        <td colspan="3">
        </td>
    </tr>
    <tr>
        <td colspan="4" style="height: 31px">
            <asp:Label ID="lblStatus" runat="server" Width="100%" Height="24px"></asp:Label></td>
    </tr>
    <tr>
        <td colspan="1" style="width: 233px">
            <asp:Button ID="cmdDelete" runat="server" Text="Delete" /></td>
        <td colspan="3">
            </td>
    </tr>
</table>
