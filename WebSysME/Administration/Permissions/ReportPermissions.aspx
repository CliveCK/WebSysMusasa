<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ReportPermissions.aspx.vb" Inherits="WebSysME.ReportPermissions" 
    title="Report Permissions" %>

<%@ Register Src="ReportPermissionsControl.ascx" TagName="ReportPermissionsControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" style="margin-left:2%">
        <tr>
            <td class="PageTitle" colspan="3">
                Reports Permissions</td>
        </tr>
        <tr>
            <td colspan="3"><asp:Panel id="pnlError" runat="server" EnableViewState="False" Width="90%"><asp:Label id="lblError" runat="server" EnableViewState="False"></asp:Label></asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc1:ReportPermissionsControl id="ReportPermissionsControl1" runat="server">
                </uc1:ReportPermissionsControl></td>
        </tr>
    </table>
</asp:Content>
