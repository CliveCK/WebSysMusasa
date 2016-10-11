<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CustomFieldTemplateDetails.ascx.vb"
    Inherits="WebSysME.CustomFieldTemplateDetails" %>
<%@ Register Src="~/Controls/ComplementaryListboxes.ascx" TagName="ComplementaryListboxes"
    TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="CustomFieldTemplateActivation.ascx" TagName="CustomFieldTemplateActivation"
    TagPrefix="uc2" %>
<table style="width: 100%;">
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="4" class="PageHeader3">
            <asp:Label ID="lblTemplateName" runat="server" Font-Bold="True" Font-Size="12pt"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Label ID="lblError" runat="server" >
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadTabStrip ID="radtlbTemplate" runat="server" MultiPageID="radmpTemplate"
                SelectedIndex="0" Skin="Office2007">
                <Tabs>
                    <telerik:RadTab runat="server" Text="Security" PageViewID="radpgvwSecurity" Selected="True">
                    </telerik:RadTab>
                    <telerik:RadTab runat="server" Text="Activation" PageViewID="radpgvwActivation">
                    </telerik:RadTab>
                    <telerik:RadTab runat="server" Text="Comments" PageViewID="radpgvwComments">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="radmpTemplate" runat="server" SelectedIndex="0" Width="100%">
                <telerik:RadPageView ID="radpgvwSecurity" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                Type
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="cboSecurityType" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Font-Size="12pt">
                                    <asp:ListItem Selected="True" Text="Read" Value="Read" />
                                    <asp:ListItem Text="Write" Value="Write" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <uc1:ComplementaryListboxes ID="ucUserSecurity" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <uc1:ComplementaryListboxes ID="ucGroupSecurity" runat="server" />
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
                <telerik:RadPageView ID="radpgvwActivation" runat="server">
                    <uc2:CustomFieldTemplateActivation ID="ucCustomFieldTemplateActivation" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="radpgvwComments" runat="server">
                    <telerik:RadEditor runat="server" ID="txtComment" SkinID="DefaultSetOfTools" Height="125px"
                        ToolsFile="~/CustomFields/CustomFieldTemplateDetails.BasicTools.xml"
                        Width="95%">
                        <Content>
                
                
                        </Content>
                        <ImageManager EnableImageEditor="False" EnableThumbnailLinking="False"></ImageManager>
                    </telerik:RadEditor>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Button ID="cmdSave" runat="server"  Font-Bold="True" Text="Save">
            </asp:Button>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="4">
            &nbsp;
        </td>
    </tr>
</table>
