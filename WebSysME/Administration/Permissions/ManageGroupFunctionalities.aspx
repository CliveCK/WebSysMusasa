<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ManageGroupFunctionalities.aspx.vb" Inherits="WebSysME.ManageGroupFunctionalities" 
    title="Group Permissions" %>

<%@ Register Src="DefaultUserPermissions.ascx" TagName="DefaultUserPermissions" TagPrefix="uc2" %>

<%@ Register Src="DefaultGroupPermissions.ascx" TagName="DefaultGroupPermissions"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" style="margin-left:2%">
        <tr>
            <td class="PageTitle" colspan="3">
                Command Level Permissions Management</td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblMessages" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="3">
                <uc2:DefaultUserPermissions id="DefaultUserPermissions1" runat="server">
                </uc2:DefaultUserPermissions></td>
        </tr>
        <tr>
            <td colspan="3">
                <uc1:DefaultGroupPermissions ID="DefaultGroupPermissions1" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblError" runat="server" CssClass="Error"></asp:Label></td>
        </tr>
    </table>
</asp:Content>
