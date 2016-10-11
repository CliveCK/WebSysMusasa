<%@ Page Language="vb" AutoEventWireup="False" MasterPageFile="~/Site.Master"
    CodeBehind="Reports.aspx.vb" Inherits="WebSysME.Reports" Title="Reports" EnableSessionState="True"
    EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="ReportsFilterControl.ascx" TagName="ReportsFilterControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" EnableViewState="true">
    <script type="text/javascript" src="Scripts/Cookies.js"></script>
    <script language="javascript" type="text/javascript">

        function OnReportClick(sender, node) {
            displayMessage('Loading report..please wait.', 'Ajax'); //alert('Showing report: ' + node.Text + ', ID: ' + node.Value + ', ' + node.Category);
            document.getElementById('<%= txtReportID.ClientID %>').value = node.get_node().get_value();
            SetCookie("thisReportID", node.get_node().get_value());
            document.getElementById('<% Response.Write(txtSaveAs.ClientID) %>').value = '-';
            document.getElementById('<% Response.Write(txtSaveAsExcel.ClientID) %>').value = '-';
            document.getElementById('<% Response.Write(txtSaveAsWord.ClientID) %>').value = '-';
            document.getElementById('<% Response.Write(txtSaveAsCSV.ClientID) %>').value = '-';

            if (node.get_node().get_category() == 'HasUserInput=1') {
                displayMessage('Loading report..please wait.', 'Ajax');
                //OpenReportFilter(node.Value); //Use this to open ReportFilter in separate window
                document.getElementById('<%= txtUserReportFilter.ClientID %>').value = '';
                document.getElementById('<%= txtReportName.ClientID %>').value = '';
                document.forms[0].submit();

                return false;

            } else if (node.get_node().get_category() == 'HasUserInput=0') {

                displayMessage('Loading report..please wait.', 'Ajax');
                document.getElementById('<%= txtUserReportFilter.ClientID %>').value = '';
                document.getElementById('<%= txtReportName.ClientID %>').value = '';
                document.forms[0].submit();

                return false;

            }
        }

        function OpenReportFilter(arg) {
            //Show new window
            //not providing a name as a second parameter will create a new window
            var oWindow = window.radopen("ReportFilter.aspx?ReportID=" + arg);

            //Using the reference to the window its clientside methods can be called
            oWindow.SetSize(620, 310);
            oWindow.SetTitle("Enter report parameters...");
        }

        function clearcontrols() {

            document.getElementById('<% Response.Write(txtSaveAs.ClientID) %>').value = '-';
            document.getElementById('<% Response.Write(txtSaveAsExcel.ClientID) %>').value = '-';
            document.getElementById('<% Response.Write(txtSaveAsWord.ClientID) %>').value = '-';
            document.getElementById('<% Response.Write(txtSaveAsCSV.ClientID) %>').value = '-';
        }

        //        function CallBackFunction(radWindow, returnValue)
        //        {
        //            //Do something with the return value!
        //            document.getElementById('<% Response.Write(txtUserReportFilter.ClientID) %>').value = returnValue;
        //            document.forms[0].submit();
        //        }

        function ReportFilterCallBackFunction(radWindow, returnValue) {
            alert('Sending Criteria: ' + returnValue);

            document.getElementById('<%= txtUserReportFilter.ClientID %>').value = returnValue;
            document.getElementById('<%= txtReportName.ClientID %>').value = '';
            document.forms[0].submit();
            return false;

        }

        function displayMessage(msg, msgstyle) {

            var lbl = $("#lblErrorDisplay");

            lbl.text(msg);

            lbl.fadeOut("fast");
            lbl.removeClass();

            switch (msgstyle.substring(0, 1).toUpperCase()) {

                case "E": //Error

                    lbl.addClass("msgError");
                    break;

                case "M": //Message

                    lbl.addClass("msgMessage");
                    break;

                case "W": //Warning

                    lbl.addClass("msgWarning")
                    break;

                case "A": //Warning

                    lbl.addClass("msgAjax")
                    break;

                case "I": //Information
                case "N": //Notice
                default: //Any other

                    lbl.addClass("success")


            }

            lbl.fadeIn("fast");

        }
        
        
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; margin-left:2%">
        <tr>
            <td class="PageTitle" colspan="2" rowspan="1" valign="top">
                Reports
                <asp:Label ID="lblReportName" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
        <tr>
            <td rowspan="4" style="width: 100px; background-color: whitesmoke; border: solid 1px #C0C0C0;"
                valign="top">
                <telerik:RadTreeView ID="tvwReportsCategories" CausesValidation="false" runat="server"
                    Width="200px" OnClientNodeClicking="OnReportClick" Enabled="true">
                </telerik:RadTreeView>
                &nbsp;<br />
            </td>
            <td valign="top">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <telerik:RadPanelBar ID="radpCriteria" runat="server" CausesValidation="false" Width="100%">
                                <Items>
                                    <telerik:RadPanelItem ID="Template1" runat="server" Text="Criteria &lt;small&gt;Click  to Expand or Collapse&lt;/small&gt;"
                                        ImageCollapsed="HeaderCollapsedImage.gif" ImageExpanded="HeaderExpandedImage.gif"
                                        ImageHoverCollapsed="HeaderHoverImage.gif" ImageHoverExpanded="HeaderHoverImage.gif"
                                        ImagePosition="Right" Expanded="True">
                                        <ItemTemplate>
                                            <uc1:ReportsFilterControl ID="ucReportsFilterControl" runat="server" Visible="true"
                                                OnFilterReport="ucReportsFilterControl_FilterReport" />
                                        </ItemTemplate>
                                    </telerik:RadPanelItem>
                                </Items>
                            </telerik:RadPanelBar>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="msgValidation" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="lblErrorDisplay">
                                <asp:Panel ID="pnlMessages" runat="server" Width="95%" EnableViewState="false">
                                    <asp:Label ID="lblMessages" runat="server"></asp:Label>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="border-right: lightgrey 1px solid; border-top: lightgrey 1px solid;
                            padding-bottom: 4px; border-left: lightgrey 1px solid; padding-top: 4px; border-bottom: lightgrey 1px solid;
                            background-color: whitesmoke">
                            <telerik:RadToolBar ID="radTbarReportOperations" runat="server" Width="100%" OnClientButtonClicked="click_handler"
                                UseFadeEffect="True">
                            </telerik:RadToolBar>
                            <asp:DropDownList ID="cboPrinters" runat="server" Visible="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center">
                            <%-- <CR:CrystalReportSource ID="crsreport" runat="server" CacheDuration ="180" >
                            </CR:CrystalReportSource>--%>
                            <CR:CrystalReportViewer ID="crvReports1" runat="server" AutoDataBind="true" DisplayToolbar="true"
                                HasCrystalLogo="False"/>
                            <asp:Label ID="lblReports" runat="server" Text="REPORTS" Font-Size="45"> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadWindowManager ID="radwinManager" runat="server" OnClientClose="ReportFilterCallBackFunction">
                                <Windows>
                                </Windows>
                            </telerik:RadWindowManager>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <asp:TextBox ID="txtUserReportFilter" runat="server" CssClass="HiddenControl"></asp:TextBox>
                            <asp:TextBox ID="txtReportID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                            <asp:TextBox ID="txtReportName" runat="server" CssClass="HiddenControl"></asp:TextBox>
                            <asp:TextBox ID="txtReportTitle" runat="server" CssClass="HiddenControl"></asp:TextBox>
                            <asp:TextBox ID="txtReportPath" runat="server" CssClass="HiddenControl"></asp:TextBox>
                            <asp:TextBox ID="txtPrint" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <asp:TextBox ID="txtLast" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <asp:TextBox ID="txtFirst" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <asp:TextBox ID="txtNext" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <asp:TextBox ID="txtPrevious" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <script type="text/javascript" language="javascript">
                                //if (document.getElementById('<% Response.write(txtReportName.ClientID) %>').value != ''){

                                function click_handler(sender, e) {

                                    document.getElementById('<% Response.Write(txtSaveAs.ClientID) %>').value = '-';
                                    document.getElementById('<% Response.Write(txtSaveAsExcel.ClientID) %>').value = '-';
                                    document.getElementById('<% Response.Write(txtSaveAsWord.ClientID) %>').value = '-';

                                    switch (e.get_item().get_commandName()) {
                                        case "Email":

                                            //showEmailPopUp(e)

                                            break;
                                        case "Save":

                                            var arg = 'savereport';
                                            document.getElementById('<% Response.Write(txtSaveAs.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;

                                        case "SaveWord":

                                            var arg = 'saveasword';
                                            document.getElementById('<% Response.Write(txtSaveAsWord.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;
                                        case "SaveExcel":

                                            var arg = 'saveasexcel';
                                            document.getElementById('<% Response.Write(txtSaveAsExcel.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;
                                        case "SaveCSV":

                                            var arg = 'saveascsv';
                                            document.getElementById('<% Response.Write(txtSaveAsCSV.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;
                                        case "Print":

                                            var arg = Math.random();
                                            document.getElementById('<% Response.Write(txtPrint.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;

                                        case "Last":

                                            var arg = Math.random();
                                            document.getElementById('<% Response.Write(txtLast.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;
                                        case "First":

                                            var arg = Math.random();
                                            document.getElementById('<% Response.Write(txtFirst.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;

                                        case "Previous":

                                            var arg = Math.random();
                                            document.getElementById('<% Response.Write(txtPrevious.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;

                                        case "Next":

                                            var arg = Math.random();
                                            document.getElementById('<% Response.Write(txtNext.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;
                                        case "Group":

                                            var arg = Math.random();
                                            document.getElementById('<% Response.Write(txtDisplayGroupTree.ClientID) %>').value = arg;
                                            document.forms[0].submit();

                                            break;


                                    }
                                }
                                //   }                         
                            </script>
                            <asp:TextBox ID="txtSaveAs" runat="server" AutoPostBack="True" CssClass="HiddenControl"></asp:TextBox>
                            <asp:TextBox ID="txtHasParameters" runat="server" CssClass="HiddenControl">False</asp:TextBox>
                            <asp:TextBox ID="txtSaveAsExcel" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <asp:TextBox ID="txtSaveAsWord" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <asp:TextBox ID="txtSaveAsCSV" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                            <asp:TextBox ID="txtDisplayGroupTree" runat="server" CssClass="HiddenControl" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
        </tr>
    </table>
    <div style="width: 700px; text-align: left; vertical-align: top;" id="filter">
    </div>
   
    <script language="javascript" type="text/javascript" src="Scripts/FloatingWindow.js"></script>
    <script type="text/javascript">

        function showReportCriteria(evt) {
            evt = (evt) ? evt : event;
            if (evt) {
                if (document.getElementById("filter").style.visibility != "visible") {
                    var elem = (evt.target) ? evt.target : evt.srcElement;
                    var position = getElementPosition(elem.id);
                    shiftTo("filter", position.left + elem.offsetWidth, position.top);
                    show("filter");

                } else {
                    hide("filter");
                }
            }
        }

        function showEmailPopUp(evt) {
            var toolBar = $find("<%= radTbarReportOperations.ClientID %>");
            var item = toolBar.findItemByText("Email Report");

            evt = (evt) ? evt : event;
            if (evt) {
                if (document.getElementById("emailpopup").style.visibility != "visible") {

                    var position = getElementPosition(item);
                    shiftTo("emailpopup", position.left + 32 + 240, position.top + 210);
                    //show("emailpopup");

                } else {
                    hide("emailpopup");
                }
            }
        }














        function Check() {

        }
    </script>
</asp:Content>
