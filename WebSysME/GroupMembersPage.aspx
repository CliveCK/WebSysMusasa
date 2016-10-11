<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GroupMembersPage.aspx.vb" Inherits="WebSysME.GroupMembersPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/GroupMembers.ascx" TagName="GroupMembersControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:GroupMembersControl ID="ucGroupMembersControl" runat="server"  />
</asp:Content>