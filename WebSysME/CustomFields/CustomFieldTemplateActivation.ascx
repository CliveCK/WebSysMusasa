<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CustomFieldTemplateActivation.ascx.vb"
    Inherits="WebSysME.CustomFieldTemplateActivation" %>
<%@ Register Src="~/Controls/ComplementaryListboxes.ascx" TagName="ComplementaryListboxes"
    TagPrefix="uc1" %>
Activate by:
<asp:DropDownList ID="cboApplyTo" runat="server" AutoPostBack="true">
    <asp:ListItem Value="D">Document Status</asp:ListItem>
    <asp:ListItem Value="P">Project Status</asp:ListItem>
    <asp:ListItem Value="C">Client Status</asp:ListItem>
    <asp:ListItem Selected="True"></asp:ListItem>
</asp:DropDownList>
<uc1:ComplementaryListboxes ID="ucAppliesTo" runat="server" />

