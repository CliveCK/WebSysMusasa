<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CustomFields.ascx.vb"
    Inherits="WebSysME.CustomFields" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Table ID="ControlsTable" runat="server" Width="100%">
</asp:Table>
<telerik:RadTabStrip ID="radtabFilterTabs" SelectedIndex="0" runat="server" MultiPageID="radmpFilterTabs"
    Visible="False">
</telerik:RadTabStrip>
<telerik:RadMultiPage ID="radmpFilterTabs" runat="server" SelectedIndex="0" Visible="False">
</telerik:RadMultiPage>
<asp:TextBox ID="txtDBFields" runat="server" CssClass="HiddenControl"></asp:TextBox>
<telerik:RadWindowManager ID="ucRadWindowManager" runat="server" Visible="False">
</telerik:RadWindowManager>
