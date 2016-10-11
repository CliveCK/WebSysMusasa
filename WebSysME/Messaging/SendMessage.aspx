<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SendMessage.aspx.vb" Inherits="WebSysME.SendMessage" %>

<!DOCTYPE html>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server"> 
    <title>Send Message</title>
    </head>   
<body>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script language="javascript" type="text/javascript">
            function CloseAndRebind(args) {
                GetRadWindow().close("InAppMail.aspx?status=true");
            }

            function ReCenter() {
                var oWindow = null;
                oWindow = GetRadWindow();
                oWindow.Center();
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog 
                else if (window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;

                //IE (and Moz as well)
                return oWindow;
            }
            function CancelEdit() {
                GetRadWindow().close("InAppMail.aspx?status=true");
            }

        </script>
    </telerik:RadScriptBlock>
     <form id="form1" runat="server"> 
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
     <asp:Panel runat="server" ID="pnlEditor" >
                <table style="width:90%">
                    <tr>
                        <td style="width: 150px;">
                            <strong>To</strong></td>
                        <td align="left">
                        <telerik:RadAutoCompleteBox runat="server" ID="cboTo" EmptyMessage="Please type here"
                            TextSearchMode="Contains" InputType="Text" Width="350" DropDownWidth="150px" CssClass="form-control">
                        </telerik:RadAutoCompleteBox></td>
                    </tr>
                     <tr>
                        <td style="width: 150px;">
                            <strong>Subject</strong></td>
                        <td align="left">
                        <asp:TextBox ID="txtSubject" runat="server" Width="50%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td  valign="top">
                        <strong>Message</strong></td>
                        <td align="left">
                        <telerik:RadEditor ID="radEMailMessage" runat="server" ToolsFile="BasicTools.xml" Width="90%" ContentAreaMode="Div" Skin="Silk">
                            <Content>
                            </Content>
                        </telerik:RadEditor>
                        <asp:Button ID="cmdSend" runat="server" Text="Send" />
                    </td>
                </tr>
                </table>
            </asp:Panel>
</form> 
</body>
 </html> 