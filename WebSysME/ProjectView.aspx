<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProjectView.aspx.vb" Inherits="WebSysME.ProjectView1" MasterPageFile="~/Site.Master"%>
<%@ Register Src="~/Controls/ProjectView.ascx" TagName="ProjectView" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ProjectView ID="ucProjectControl" runat="server"  />
</asp:Content>
