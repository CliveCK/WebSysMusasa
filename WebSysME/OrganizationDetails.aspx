<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrganizationDetails.aspx.vb" Inherits="WebSysME.OrganizationDetails" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/OrganizationDetailsControl.ascx" TagName="OrganizationControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:OrganizationControl ID="ucOrganizationControl" runat="server"  />
</asp:Content>
