<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImpactProjectPage.aspx.vb" Inherits="WebSysME.ImpactProjectPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ImpactProjectControl.ascx" TagName="ImpactProjectControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ImpactProjectControl ID="ucImpactProject" runat="server"  />
</asp:Content>
