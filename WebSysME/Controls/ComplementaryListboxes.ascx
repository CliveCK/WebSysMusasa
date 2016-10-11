<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ComplementaryListboxes.ascx.vb" Inherits="WebSysME.ComplementaryListboxes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dw" Assembly="WebSysME" Namespace="WebSysME" %>
<table id="Table4" bordercolor="#000000" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tr>
        <td width="40%">
                <asp:Label ID="lblAvailableOptions" runat="server"></asp:Label>
        </td>
        <td align="center" width="20%"></td>
        <td width="40%">
            <asp:Label ID="lblSelectedOptions" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td width="40%" style="vertical-align:top">
            <dw:ListboxWithViewState ID="lstAvailableOptions" SelectionMode="Multiple" DataValueField="GroupID" DataTextField="Name"
                Rows="7" Width="100%" runat="server" CssClass="form-control">
            </dw:ListboxWithViewState></td>
        <td width="20%" style="vertical-align: middle; text-align: center">
            <asp:Button ID="cmdMoveSelected" Width="40px" runat="server" Text=">" CssClass="btn btn-default"></asp:Button><br />
            <asp:Button ID="cmdMoveAll" Width="40px" runat="server" Text=">>" CssClass="btn btn-default"></asp:Button><br />
            <asp:Button ID="cmdRemoveSelected" Width="40px" runat="server" Text="<" CssClass="btn btn-default"></asp:Button><br />
            <asp:Button ID="cmdRemoveAll" Width="40px" runat="server" Text="<<" CssClass="btn btn-default"></asp:Button><br />
        </td>
        <td width="40%" style="vertical-align:top">
            <dw:ListboxWithViewState ID="lstSelectedOptions" SelectionMode="Multiple" DataValueField="GroupID" DataTextField="Name"
                Rows="7" Width="100%" runat="server"  CssClass="form-control">
                <asp:ListItem></asp:ListItem>
            </dw:ListboxWithViewState></td>
    </tr>
</table>
