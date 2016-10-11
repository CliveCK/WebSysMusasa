<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CashDonationsPage.aspx.vb" Inherits="WebSysME.CashDonationsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/CashDonationDetailsControl.ascx" TagName="CashDonationsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:CashDonationsControl ID="ucCashDonationsControl" runat="server"  />
</asp:Content>