<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReportPermissionsControl.ascx.vb"
    Inherits="WebSysME.ReportPermissionsControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
            <!--
    //<![CDATA[

    function UpdateAllChildren(nodes, checked) {
        var i;
        for (i = 0; i < nodes.length; i++) {
            checked ? nodes[i].Check() : nodes[i].UnCheck();
            if (nodes[i].Nodes.length > 0) {
                UpdateAllChildren(nodes[i].Nodes, checked);
            }
        }
    }

    function CheckChildNodes(node) {
        UpdateAllChildren(node.Nodes, node.Checked);
    }


    function DropTest(source, dest, events) {
        var target = (document.all) ? events.srcElement : events.target;
        if (target.tagName == "INPUT") {
            target.style.cursor = "default";
            target.value = source.Text;
            return false;
        }

        if (grid != null) {
            source.TreeView.HtmlElementID = "Grid";
            grid.style.cursor = "default";
            grid = null;
            return true;
        }

        if (dest != null) {
            return true;
        }

        return false;
    }

    function ClientMove(events) {
        var target = (document.all) ? events.srcElement : events.target;
        if (target.tagName == "INPUT") {
            target.style.cursor = "hand";
        }

        var dummy = IsMouseOverGrid(events)
        if (dummy != null) {
            grid = dummy;
            grid.style.cursor = "hand";
        }
        else {
            grid = null;
        }
    }

    function ContextMenuClick(node, itemText) {
        if (itemText == "Disable") {
            node.Disable();
            return false;
        }
        if (itemText == "Enable All") {
            for (var i = 0; i < node.TreeView.AllNodes.length; i++) {
                node.TreeView.AllNodes[i].Enable();
            }
        }
        if (itemText == "Edit") {
            node.StartEdit();
        }

        return true;
    }

    //]]>
            // -->
</script>
<table width="100%">
    <tr>
        <td class="notes" colspan="3">
            This page manages who views certain reports.
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td colspan="2">
        </td>
    </tr>
    <tr>
        <td style="width: 180px" valign="top">
            <table width="100%">
                <tr>
                    <td class="DetailsSection">
                        Users
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="lstUsers" runat="server" AutoPostBack="True" Height="208px" Width="170px" CssClass="form-control"
                            BackColor="LavenderBlush"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td >
                        UserGroups
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="lstRoles" runat="server" AutoPostBack="True" Height="192px" Width="170px" CssClass="form-control"
                            BackColor="LavenderBlush" ></asp:ListBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="2" valign="top">
            <asp:Panel ID="pnlCodes" runat="server" GroupingText="Reports List" Height="100%"
                Width="100%">
                <table width="100%">
                    <tr>
                        <td>
                            Select the reports the
                            <asp:Label ID="lblUsertype" runat="server"></asp:Label>
                            should see.
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <telerik:RadTreeView ID="radTreeCategories" runat="server" AutoPostBack="True" CheckBoxes="True"
                                TriStateCheckBoxes="true" Width="100%" OnClientNodeDragging="ClientMove" OnClientNodeDropping="DropTest"
                                AfterClientCheck="CheckChildNodes">
                            </telerik:RadTreeView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="cmdSave" runat="server" CssClass="btn btn-default" Text="Save Selected" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:TextBox ID="txtUserID" runat="server" CssClass="HiddenControl">0</asp:TextBox>
            <asp:TextBox ID="txtUserType" runat="server" CssClass="HiddenControl"></asp:TextBox>
        </td>
    </tr>
</table>
