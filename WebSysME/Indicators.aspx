<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Indicators.aspx.vb" Inherits="WebSysME.Indicators" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/IndiactorDetailsControl.ascx" TagName="IndicatorControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:IndicatorControl ID="ucIndicator" runat="server"  />
</asp:Content>
