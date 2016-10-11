<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RestrictedAccess.aspx.vb" Inherits="WebSysME.RestrictedAccess" MasterPageFile="~/Site.Master"%>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="Content1">
    
     <p style="font-family:'Segoe UI';font-size:15pt;font-style:oblique; font-weight:bold;color:red;margin-left:2%">
         Sorry, Access to requested page is restricted...</p>
    <p>&nbsp</p>
    <p style="font-family:'Segoe UI';font-size:10pt;font-style:normal;color:red;margin-left:2%">
        If you feel this is incorrect, please contact your administrator. User information summarised below</p>
    <div style="border:1px solid darkred;font-size:10pt;font-family:'Segoe UI';font-weight:200;color:olivedrab;margin-left:2%;width:25%">
        <table cellpadding="3">
            <tr>
                <td >&nbsp</td>
            </tr>
            <tr>
                <td>
                    User Alias:
                </td>
                <td>
                    <asp:Label ID="lblUserName" runat="server" ForeColor="Red" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td>User Full Name:</td>
                <td><asp:Label ID="lblUserFullName" runat="server" ForeColor="Red" ></asp:Label></td>
            </tr>
            <tr>
                <td>User Assigned Group:</td>
                <td><asp:Label ID="lblUserGroup" runat="server" ForeColor="Red" ></asp:Label></td>
            </tr>
        </table>
    </div>
    <div style="text-align:center">
        <asp:Image ID="imgRestricted" ImageUrl="~/images/AccessRestricted.png" runat="server"/>
    </div>
</asp:Content>

