<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ReportsFilterControl.ascx.vb"
    Inherits="WebSysME.ReportsFilterControl"   EnableViewState="true"%>
<%@ Register Src="~/CustomFields/CustomFields.ascx" TagName="CustomFields" TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div style="width:100%;  text-align:left; padding:2px 2px 2px 2px;  " >
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td >
            </td>
        </tr>
        <tr>
            <td style="height: 49px" >
                <uc1:CustomFields ID="ucReportsFilter" runat="server" Visible="true" EnableViewState="true" />
            </td>
        </tr>
        <tr>
            <td style="height: 25px" >
                <asp:Button ID="cmdViewReport" runat="server" Text="View Report" CssClass="btn btn-default"  OnClientClick="clearcontrols();displayMessage('Loading report..please wait.','Ajax');" />&nbsp;
                <asp:TextBox ID="txtReportID" runat="server" CssClass="HiddenControl"></asp:TextBox></td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="lblCriteria" runat="server" CssClass="Error"></asp:Label>
                <telerik:RadWindowManager ID="radwinManager" runat="server">
                    <Windows>
                    </Windows>
                </telerik:RadWindowManager>
            </td>
        </tr>
    </table>
</div>
