<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DistributionsPage.aspx.vb" Inherits="WebSysME.DistributionsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/DistributionsDetailsControl.ascx" TagName="DistributionsControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:DistributionsControl ID="ucDistributionsControl" runat="server"  />
</asp:Content>
