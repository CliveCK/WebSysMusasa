<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrganizationalPlanPage.aspx.vb" Inherits="WebSysME.OrganizationalPlanPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/OrganizationalPlanDetailsControl.ascx" TagName="OrganizationPlanControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:OrganizationPlanControl ID="ucOrganizationPlanControl" runat="server"  />
</asp:Content>
