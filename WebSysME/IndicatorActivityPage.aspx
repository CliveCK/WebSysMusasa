<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IndicatorActivityPage.aspx.vb" Inherits="WebSysME.IndicatorActivityPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/IndicatorActivityDetailsControl.ascx" TagName="IndicatorActivityControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:IndicatorActivityControl ID="ucIndicatorActivityControl" runat="server"  />
</asp:Content>