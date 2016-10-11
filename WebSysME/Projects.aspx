<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Projects.aspx.vb" Inherits="WebSysME.Projects"  MasterPageFile="~/Site.Master"%>
<%@ Register Src="~/Controls/ProjectsControl.ascx" TagName="ProjectsControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:ProjectsControl ID="ucProjectControl" runat="server"  />
</asp:Content>
