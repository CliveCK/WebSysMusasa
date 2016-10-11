<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SecurityPolicyManagement.aspx.vb" Inherits="WebSysME.SecurityPolicyManagement" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/UserAdministrationControl/PasswordPolicyManagementConsole.ascx" TagName="PasswordPolicyControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:PasswordPolicyControl ID="ucPasswordPolicyControl" runat="server"  />
</asp:Content>
