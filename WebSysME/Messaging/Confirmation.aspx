<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Confirmation.aspx.cs" Inherits="Confirmation" Title="YaMessaging - Sent Confirmation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
<h2> Done </h2>

<p>
    <strong>Your message has been successfully sent to following users:</strong></p>
    <asp:Label ID="lblSuccess" runat="server"></asp:Label><p>
        <strong>Your message could not be sent to following users: </strong>
    </p>
    <asp:Label ID="lblFail" runat="server"></asp:Label><br />
    <br />
    <br />
    <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back To Inbox" /></div>
</asp:Content>

