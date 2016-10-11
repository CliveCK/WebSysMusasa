<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HealthCentersPage.aspx.vb" Inherits="WebSysME.HealthCentersPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/HealthCenterDetailsControl.ascx" TagName="HealthCenterControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:HealthCenterControl ID="ucHealthCenterControl" runat="server"  />
</asp:Content>
