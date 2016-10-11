<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProjectMapping.aspx.vb" Inherits="WebSysME.ProjectMapping" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ProjectObjectsDetailsControl.ascx" TagName="ProjectObjectControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ProjectObjectControl ID="ucProjectObjectControl" runat="server"  />
</asp:Content>
