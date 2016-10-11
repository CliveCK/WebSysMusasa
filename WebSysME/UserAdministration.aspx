<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserAdministration.aspx.vb" Inherits="WebSysME.UserAdministration" MasterPageFile="~/Site.Master"%>



<%@ Register Src="UserAdministrationControl/Userpermissions.ascx" TagName="Userpermissions"
    TagPrefix="uc4" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UserAdministrationControl/FindUsers.ascx" TagName="FindUsers" TagPrefix="uc3" %>
<%@ Register Src="UserAdministrationControl/DeleteUser.ascx" TagName="DeleteUser"
    TagPrefix="uc2" %>
<%@ Register Src="UserAdministrationControl/CreateNewUserControl.ascx" TagName="CreateNewUserControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td class="PageTitle" colspan="4">
                User Administration</td>
        </tr>
        <tr>
            <td colspan="1" rowspan="3" style="width: 140px" valign="top">
                <telerik:RadPanelBar ID="radpMemberActions" runat="server" AppendDataBoundItems="True"
                    Width="170px">
                    <Items>
                        <telerik:RadPanelItem ID="UserActions" runat="server" Text="User Actions" Expanded="True">
                            <Items>
                                <telerik:RadPanelItem ID="cmdAdd" runat="server" Text="Add New User" Expanded="True"
                                    NavigateUrl="~/UserAdministration.aspx?op=new">
                                </telerik:RadPanelItem>
                                <telerik:RadPanelItem ID="cmdFind" runat="server" Text="FindUser(s)" Expanded="True"
                                    NavigateUrl="~/UserAdministration.aspx?op=vw">
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelBar>
            </td>
            <td class="DetailsSection" colspan="3">
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc1:CreateNewUserControl ID="CreateNewUserControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc3:FindUsers ID="FindUsers1" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="1" style="width: 149px">
            </td>
            <td colspan="3">
            </td>
        </tr>
    </table>
</asp:Content>

