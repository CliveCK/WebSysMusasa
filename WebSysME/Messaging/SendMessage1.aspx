<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="SendMessage.aspx.cs" Inherits="SendMessage" Title="YaMessaging - Send Message" ValidateRequest="false" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div align="center">
<h3>Send Message</h3>
<table width="100%" border="1" bordercolor="thistle">
    <tr>
        <td align="left" width="20%">
            <strong>Select Users to Send</strong></td>
        <td width="80%">
            <strong>Enter Your Message</strong></td>
    </tr>
<tr>
<td width="20%" align="left" valign="top">
    <asp:CheckBoxList ID="chkLstUsers" runat="server">
    </asp:CheckBoxList>
    <br />
    <asp:Button ID="btnAddSelected" runat="server" Text="Add Selected" OnClick="btnAddSelected_Click" CausesValidation="False" /></td>
<td width="80%" align="left" valign="top">
    <table width="100%">
        <tr>
            <td style="width: 77px; height: 34px;" valign="top">
                TO:</td>
            <td colspan="2" style="height: 34px">
                <asp:TextBox ID="txtToList" runat="server" Height="43px" TextMode="MultiLine" Width="551px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToList"
                    ErrorMessage="List of recievers is EMPTY">**</asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 77px">
                Subject:</td>
            <td colspan="2">
                <asp:TextBox ID="txtSubject" runat="server" Width="589px"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 77px" valign="top">
                Body:</td>
            <td colspan="2">
                <FTB:FreeTextBox id="FreeTextBox1" runat="Server"/>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="Send Message" />&nbsp;
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CausesValidation="False" /></td>
        </tr>
        <tr>
            <asp:TextBox runat="server" Visible="false" ID="FreeTextBox1"></asp:TextBox>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />

</td>
</tr>
</table>

</div>
</asp:Content>

