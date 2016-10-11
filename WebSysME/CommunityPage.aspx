<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CommunityPage.aspx.vb" Inherits="WebSysME.CommunityPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/CommunityDetailsControl.ascx" TagName="CommunityControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:CommunityControl ID="ucCommunityControl" runat="server"  />
</asp:Content>
