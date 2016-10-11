<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DefaultUserPermissions.ascx.vb" Inherits="WebSysME.DefaultUserPermissions" %>
<%@ Register Src="~/Controls/ComplementaryListboxes.ascx" TagName="ComplementaryListboxes"
    TagPrefix="uc1" %>
<table width="100%" class="GreyBG">
    <tr>
        <td class="DetailsSection" colspan="3">
            Users</td>
    </tr>
    <tr>
        <td rowspan="2" style="width: 170px" valign="top">
            <asp:ListBox ID="lstUsers" runat="server" AutoPostBack="True" Width="170px" Height="208px" CssClass="form-control"></asp:ListBox></td>
        <td style="width: 14px">
        </td>
        <td valign="top" style="background-color: whitesmoke">
            <uc1:ComplementaryListboxes ID="ComplementaryListboxes1" runat="server" CssClass="form-control"/>
            <asp:Button ID="cmdApply" runat="server" Text="Apply" CssClass="btn btn-default"/></td>
    </tr>
    <tr>
        <td style="width: 14px">
        </td>
        <td>
        </td>
    </tr>
</table>
