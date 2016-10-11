<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ReadMail.aspx.cs" Inherits="ReadMail" Title="YaMessaging - Read Mail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div align="center">
<h3>Read Message</h3>
</div>

<div>
<table width="100%" border="1" bordercolor="thistle">
<tr>
<td width="10%">
Sender: 
</td>
<td width="90%">
    <asp:Label ID="lblSender" runat="server"></asp:Label></td>
</tr>
    <tr>
        <td width="10%">
            Reciever</td>
        <td width="90%">
            <asp:Label ID="lblReciever" runat="server"></asp:Label></td>
    </tr>
<tr>
<td width="10%">
Subject: 
</td>
<td width="90%">
    <asp:Label ID="lblSubject" runat="server"></asp:Label></td>
</tr>
    <tr>
        <td width="10%">
            Date:</td>
        <td width="90%">
            <asp:Label ID="lblDate" runat="server"></asp:Label></td>
    </tr>
<tr>
<td width="10%" style="height: 386px" valign="top">
Body: 
</td>
<td width="90%" style="height: 386px" valign="top">
    <asp:Label ID="lblBody" runat="server"></asp:Label></td>
</tr>
<tr>
<td width="10%">
Action: 
</td>
<td width="90%">
    <asp:Button ID="btnDone" runat="server" OnClick="btnDone_Click" Text="Done" />
    <asp:Button ID="btnReply" runat="server" OnClick="btnReply_Click" Text="Reply" />&nbsp;<asp:Button
        ID="btnForward" runat="server" OnClick="btnForward_Click" Text="Forward" /></td>
</tr>
</table>
</div>
</asp:Content>

