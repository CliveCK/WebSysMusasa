<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="KindDonationsPage.aspx.vb" Inherits="WebSysME.KindDonationsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/KindDonationDetailsControl.ascx" TagName="KindDonationControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:KindDonationControl ID="ucKindDonationControl" runat="server"  />
</asp:Content>