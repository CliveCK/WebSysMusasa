<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TrainingPermissions.aspx.vb" Inherits="WebSysME.TrainingPermissions" MasterPageFile="~/Site.Master"%>


<%@ Register Src="StaffTrainingPermissions.ascx" TagName="TrainingPermissionsControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" style="margin-left:2%">
        <tr>
            <td class="PageTitle" colspan="3">
                Training Permissions</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblMessages" runat="server"></asp:Label></td>
        </tr>
         <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3"><asp:Panel id="pnlError" runat="server" EnableViewState="False" Width="90%"><asp:Label id="lblError" runat="server" EnableViewState="False"></asp:Label></asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc1:TrainingPermissionsControl id="TrainingPermissionsControl1" runat="server">
                </uc1:TrainingPermissionsControl></td>
        </tr>
    </table>
</asp:Content>

