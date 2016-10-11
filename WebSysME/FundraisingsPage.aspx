<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FundraisingsPage.aspx.vb" Inherits="WebSysME.FundraisingsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/FundRaisingDetailsControl.ascx" TagName="FundRaisingControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:FundRaisingControl ID="ucFundRaisingControl" runat="server"  />
</asp:Content>
