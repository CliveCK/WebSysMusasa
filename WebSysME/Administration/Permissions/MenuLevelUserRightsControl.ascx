<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MenuLevelUserRightsControl.ascx.vb"
    Inherits="WebSysME.MenuLevelRightsControl" %>
<table class="GreyBG" width="100%">
    <tr>
        <td class="DetailsSection" colspan="3">
            Users
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
            <asp:ListBox ID="lstUsers" runat="server" AutoPostBack="True" Height="208px" Width="170px" CssClass="form-control">
            </asp:ListBox>
        </td>
        <td style="background-color: whitesmoke" valign="top">
            <asp:Label ID="Label1" runat="server" Text="Main Menu Rights"></asp:Label>
            <telerik:RadTreeView ID="RadTreeUserRights" runat="server" CheckBoxes="True" TriStateCheckBoxes="True"
                CheckChildNodes="True">
            </telerik:RadTreeView>
        </td>
        <td style="background-color: whitesmoke" valign="top">
           
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-default" Width="56px" />
            <asp:TextBox ID="txtUserID" runat="server" Visible=" false"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>
</table>
