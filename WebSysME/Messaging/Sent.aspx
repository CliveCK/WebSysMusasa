<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Sent.aspx.cs" Inherits="Sent" Title="YaMessaging - Sent Mails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div align="center">
<h3>Sent Messages</h3>

    <table width="100%" border="1" bordercolor="Thistle">   
    
        <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
        <tr>
        <td width="30" align="left">
        <strong>Sent to</strong>
        </td>
        <td width="40%" align="left">
        <strong>Subject</strong>
        </td>
        <td width="30%" align="left">
        <strong>Date</strong>
        </td>
        </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
        <td align="left">
            <asp:Label ID="lblSender" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.recieverID")%>'></asp:Label>
        </td>
        <td align="left">
            <asp:HyperLink ID="hprSubject" runat="server" NavigateUrl='<%#DataBinder.Eval(Container, "DataItem.MessageID", "ReadMail.aspx?id={0}")%>'><%#DataBinder.Eval(Container,"DataItem.subject")%></asp:HyperLink>
        </td>
        <td align="left">
            <asp:Label ID="lblDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.datentime")%>'></asp:Label>
        </td>
        </tr>
        </ItemTemplate>      
       
        
        </asp:Repeater>
    </table>
</div>
</asp:Content>

