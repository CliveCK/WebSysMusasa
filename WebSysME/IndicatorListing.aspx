<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IndicatorListing.aspx.vb" Inherits="WebSysME.IndicatorListing" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/IndicatorListingControl.ascx" TagName="IndicatorControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:IndicatorControl ID="ucIndicatorControl" runat="server"  />
</asp:Content>
