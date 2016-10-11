<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MenuLevelUserGroupRightsControl.ascx.vb"
    Inherits="WebSysME.MenuLevelUserGroupRightsControl" %>
<table cellspacing="0" class="GreyBG" width="100%">
    <tr>
        <td class="DetailsSection" colspan="3">
            Usergroups
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Panel ID="pnlError" runat="server" EnableViewState="False">
                <asp:Label ID="lblError" runat="server" CssClass="Error" EnableViewState="False"
                    Width="100%"></asp:Label>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td rowspan="2" style="width: 170px" valign="top">
            <asp:ListBox ID="lstRoles" runat="server" AutoPostBack="True" Height="192px" Width="170px" CssClass="form-control">
            </asp:ListBox>
        </td>
        <td style="background-color: whitesmoke" valign="top">
            <asp:Label ID="Label1" runat="server" Text="Main Menu Rights"></asp:Label>
            <telerik:RadTreeView ID="RadTreeUserGroupRights" runat="server" CheckBoxes="true"
                TriStateCheckBoxes="true" CheckChildNodes="True">
            </telerik:RadTreeView>
        </td>
        <td style="background-color: whitesmoke" valign="top">
            <asp:Label ID="Label2" runat="server" Text="Action Menu Rights" Width="50%"></asp:Label>
            <telerik:RadTreeView ID="RadTreeUserGroupContextRights" runat="server" CheckBoxes="true"
                TriStateCheckBoxes="true" CheckChildNodes="True">
            </telerik:RadTreeView>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-default" Width="56px"
                Height="26px" />
            <asp:TextBox ID="txtUserGroupID" runat="server" Visible="false"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>
</table>
