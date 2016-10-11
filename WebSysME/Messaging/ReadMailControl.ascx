<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReadMailControl.ascx.vb" Inherits="WebSysME.ReadMailControl" %>


<style type="text/css">
    .auto-style1 {
        width: 22%;
    }
</style>
<table cellpadding="4" cellspacing="2" style="width:100%">
    <tr>
        <td style="color:darkblue;font-weight:bold;font-size:14px;padding-left:2%">
            Messaging Portal
        </td>
    </tr>
    <tr>
        <td style="padding-left:2%">
            <asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td style="padding-left:2%">
            <asp:LinkButton ID="Link" runat="server" Text="Compose" OnClientClick="ShowMessageForm(0); return false;" CssClass="btn btn-default"/>
        </td>
    </tr>
    <tr>
        <td rowspan="2" style="text-align:left;vertical-align:top;padding-left:2%;border:1px solid darkblue" class="auto-style1">
            <telerik:RadPanelBar ID="radpMail" runat="server" AllowCollapseAllItems="True" 
                                ExpandMode="SingleExpandedItem" CausesValidation="False" Width="80%" 
                                AppendDataBoundItems="True">
                                <Items>
                                    <telerik:RadPanelItem Value="Mail" runat="server" Text="My Messages"
                                        NavigateUrl="#" Expanded="true" >
                                        <Items>
                                            <telerik:RadPanelItem Value="cmdInbox" runat="server" >
                                            </telerik:RadPanelItem>
                                            <telerik:RadPanelItem Value="cmdSent" runat="server" Text="Sent Items">
                                            </telerik:RadPanelItem>
                                            <telerik:RadPanelItem Value="cmdDeletedItem" runat="server" Text="Deleted Items">
                                            </telerik:RadPanelItem>
                                        </Items>
                                    </telerik:RadPanelItem>
                                </Items>
               </telerik:RadPanelBar>
        </td>
            <td rowspan="2" style="text-align:center;vertical-align:top;width:20%;padding-left:1%;border:1px solid darkblue">
                <asp:Label runat="server" ID="lblSelected" Font-Bold="true" ></asp:Label>
                <telerik:RadGrid ID="radMail" runat="server" GridLines="Horizontal" Height="80%" 
                    CellPadding="0" Width="90%" Skin="MetroTouch">
                    <MasterTableView AutoGenerateColumns="True" AllowFilteringByColumn="False" AllowPaging="True" 
                        PagerStyle-Mode="NextPrev">
                        <Columns>
                        </Columns>
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <PagerStyle Position="Bottom" AlwaysVisible="false"/>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true" >
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        <td valign="top" style="padding-left:1%;align-content:center;border:1px solid darkblue">
            <asp:Panel runat="server" ID="pnlEditor" Visible="false" >
                <table style="width:90%">
                    <tr>
                        <td style="width: 150px;">
                            <strong>From</strong></td>
                        <td align="left">
                        <asp:TextBox ID="txtFrom" runat="server" Width="50%" CssClass="form-control" Enabled="false"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td style="width: 150px;">
                            <strong>Subject</strong></td>
                        <td align="left">
                        <asp:TextBox ID="txtSubject" runat="server" Width="50%" CssClass="form-control" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td  valign="top">
                        <strong>Message</strong></td>
                        <td align="left">
                        <telerik:RadEditor ID="radEMailMessage" ToolsFile="BasicTools.xml" runat="server" Width="90%" RenderMode="Lightweight" ContentAreaMode="Div" Skin="Silk" EditModes="Preview">
                            <Content>
                            </Content>
                        </telerik:RadEditor>
                         <asp:Button ID="cmdReply" runat="server" Text="Reply" OnClientClick="ShowMessageForm('reply'); return false;" CssClass="btn btn-default"/>
                        <asp:Button ID="cmdForward" runat="server" Text="Forward" OnClientClick="ShowMessageForm('forward'); return false;" CssClass="btn btn-default"/>
                    </td>
                </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<telerik:RadWindowManager ID="RadWindowManager1" runat="server" OnClientClose="OnClientClose">
   <Windows>
     <telerik:RadWindow ID="RadWindow1" runat="server"  Modal="true" >
        <ContentTemplate>
         </ContentTemplate>
     </telerik:RadWindow>
  </Windows>
</telerik:RadWindowManager>  
<script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        function ShowMessageForm(token) {
            var win = window.radopen("SendMessage.aspx?action=" + token, "RadWin", "1100px", "500px");
            win.set_modal(true);
            return false;
        }
        function OnClientClose(sender, args) {
            if (args.get_argument() != null) {
                window.location.href = args.get_argument();
            }
        }
    </script>
 