<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CustomFieldsBulkAssign.ascx.vb" Inherits="SampleApp.CustomFieldsBulkAssign" %>
<table border="0" cellpadding="4" cellspacing="0" style="width: 100%">
    <tr>
        <td style="width: 20%">
            Used fields</td>
        <td style="width: 80%">
            <asp:DropDownList ID="cboFieldName" runat="server" AutoPostBack="True">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>
            Values Filter</td>
        <td>
            <asp:CheckBox ID="chkFilterByValue" runat="server" AutoPostBack="True"
                Text=" " />
            <asp:DropDownList ID="cboFieldValues" runat="server" Enabled="False">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>
            New Value</td>
        <td>
            <asp:TextBox ID="txtNewValue" runat="server" Width="60%"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:Button ID="cmdUpdate" runat="server" Text="Update Custom Fields" /></td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="PageHeader" colspan="2">
            Custom Field Templates</td>
    </tr>
    <tr>
        <td>
            Templates</td>
        <td>
            <asp:DropDownList ID="cboTemplates" runat="server">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:Button ID="cmdAssignCustomFieldTemplate" runat="server" Text="Assign" />
            <asp:Button ID="cmdRemoveCustomFieldTemplate" runat="server" OnClientClick="return confirm('Are you sure you want to remove this template from all the objects? This cannot be undone.');"
                Text="Remove" /></td>
    </tr>
</table>
