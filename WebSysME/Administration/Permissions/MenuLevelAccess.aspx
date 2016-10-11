<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MenuLevelAccess.aspx.vb" Inherits="WebSysME.MenuLevelAccess" 
    title="Menu Level Rights" %>

<%@ Register Src="MenuLevelUserGroupRightsControl.ascx" TagName="MenuLevelUserGroupRightsControl"
    TagPrefix="uc2" %>

<%@ Register Src="MenuLevelUserRightsControl.ascx" TagName="MenuLevelUserRightsControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" style="margin-left:2%">
        <tr>
            <td class="PageTitle" colspan="3">
                Menu Level Permissions</td>
        </tr>
        <tr>
            <td colspan="3" style="border-right: #ccccff 1px dotted; border-top: #ccccff 1px dotted;
                border-left: #ccccff 1px dotted; width: 100%; border-bottom: #ccccff 1px dotted;
                background-color: #eef7f5">
                This page manages menu level permissions. Users will not be able to browse pages
                if these are not set.
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc1:MenuLevelUserRightsControl ID="MenuLevelUserRightsControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc2:MenuLevelUserGroupRightsControl id="MenuLevelUserGroupRightsControl" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
