<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProjectListing.aspx.vb" Inherits="WebSysME.ProjectListing1" MasterPageFile="~/Site.Master"%>
<%@ Register Src="~/Controls/ProjectListing.ascx" TagName="ProjectList" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server" >
    <uc1:ProjectList ID="ucProjectControl" runat="server"  />
</asp:Content>
