<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DropDownControl.ascx.vb"
    Inherits="WebSysME.DropDownControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">

    function StopPropagation(e) {
        //cancel bubbling
        e.cancelBubble = true;
        if (e.stopPropagation) {
            e.stopPropagation();
        }
    }

    function nodeClicked(sender, args) {
        var comboBox = $find("<%= rcbTreeview.ClientID %>");
        var node = args.get_node();
        var multipleSelect = node.get_treeView().get_multipleSelect();

        if (multipleSelect == false) {
            supressDropDownClosing = false;       //prevent  combo from closing
            comboBox.set_text(node.get_text().toString()); //Replace the previously selected value
        } else {
            supressDropDownClosing = true;  //prevent  combo from closing
            comboBox.set_text(node.get_text().toString());
        }

    }

    function nodeChecked(sender, args) {
        var comboBox = sender
        // $find("<%= rcbTreeview.ClientID %>");

        var node = args.get_node();
        node.set_selected(node.get_checked());

        //check if 'Select All' node has been checked/unchecked
        var tempNode = args.get_node();
        var multipleSelect = tempNode.get_treeView().get_multipleSelect();

        if (tempNode.get_text().toString() == "(Select All)") {
            // check or uncheck all the nodes
        } else {
            var nodes = new Array();
            nodes = sender.get_checkedNodes();
            var vals = "";
            var i = 0;

            for (i = 0; i < nodes.length; i++) {
                var n = nodes[i];
                var nodeText = n.get_text().toString();
                if (nodeText != "(Select All)") {
                    vals = vals + n.get_text().toString() + ",";
                }
            }


            if (multipleSelect == false) {
                if (comboBox.get_text() == "") {           // only add to the combobox text if the text is empty
                    supressDropDownClosing = true;       //prevent  combo from closing
                    comboBox.set_text(vals);
                } else {

                }

            } else {
                supressDropDownClosing = true;  //prevent  combo from closing
                comboBox.set_text(vals);
            }
        }
    }


    function OnClientDropDownOpenedHandler(sender, eventArgs) {
        var tree = sender.get_items().getItem(0).findControl("rcbTree");
        var selectedNode = tree.get_selectedNode();
        if (selectedNode) {
            selectedNode.scrollIntoView();
        }
    }
</script>
<div>
    <table width="100%">
        <tr>
            <td style="height: 43px">
                <telerik:RadComboBox ID="rcbTreeview" runat="server" Width="100%" AllowCustomText="true"
                    OnClientDropDownOpened="OnClientDropDownOpenedHandler" EmptyMessage="Select  item"
                    DropDownWidth="300px" Height="300px" MaxHeight="400px" OnLoad="RcbTree_Load">
                    <ItemTemplate>
                        <div id="div1" onclick="StopPropagation(event)">
                            <telerik:RadTreeView runat="server" ID="rcbTree" CheckChildNodes="False" OnClientNodeChecked="nodeChecked"
                                OnClientNodeClicked="nodeClicked">
                            </telerik:RadTreeView>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rcbTree"
                                ErrorMessage="Please check at least one node" Enabled="False">
                            </asp:RequiredFieldValidator>
                        </div>
                    </ItemTemplate>
                    <Items>
                        <telerik:RadComboBoxItem Text="" Owner="rcbTreeview" />
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
    </table>
</div>
