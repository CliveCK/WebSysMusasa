<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Userpermissions.ascx.vb" Inherits="WebSysME.Userpermissions" %>
<table width="100%">
    <tr>
        <td style="width: 171px"><asp:Label ID="Label2" runat="server" Text="UnAssigned Permissions" Width="184px"></asp:Label>
        </td>
        <td style="width: 65px">
        </td>
        <td style="width: 12px"><asp:Label ID="Label1" runat="server" Text="Assigned Permissions" Width="168px"></asp:Label>
        </td>
    </tr>
    <tr>
        <td rowspan="4" style="width: 171px" align="center">
            <asp:ListBox ID="lstUnAssigned" runat="server" Height="216px" Width="224px"></asp:ListBox></td>
        <td style="height: 25px; width: 65px;">
            <asp:Button ID="cmdAssignAll" runat="server" Text=">>" /></td>
        <td rowspan="4" style="width: 12px" align="left">
            <asp:ListBox ID="lstAssigned" runat="server" Height="208px" Width="240px"></asp:ListBox></td>
    </tr>
    <tr>
        <td style="height: 25px; width: 65px;">
            <asp:Button ID="cmdUnAssignSelected" runat="server" Text="<" /></td>
    </tr>
    <tr>
        <td style="height: 25px; width: 65px;">
            <asp:Button ID="cmdAssignSelected" runat="server" Text=">" /></td>
    </tr>
    <tr>
        <td style="height: 25px; width: 65px;">
            <asp:Button ID="cmdUnAssignAll" runat="server" Text="<<" /></td>
    </tr>
    <tr>
        <td style="width: 171px">
        </td>
        <td style="width: 65px">
        </td>
        <td style="width: 12px">
        </td>
    </tr>
</table>
