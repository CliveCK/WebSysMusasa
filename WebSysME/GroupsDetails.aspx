<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GroupsDetails.aspx.vb" Inherits="WebSysME.GroupsDetails" MasterPageFile="~/Site.Master" %>

<%@ Register Src="~/Controls/GroupsDetailsControl.ascx" TagName="GroupDetailsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:GroupDetailsControl ID="ucGroupDetailsControl" runat="server"  />
</asp:Content>