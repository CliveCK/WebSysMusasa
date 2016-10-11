<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GeneralActivityPage.aspx.vb" Inherits="WebSysME.GeneralActivityPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/GeneralActivityDetailsControl.ascx" TagName="GeneralActivityControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:GeneralActivityControl ID="ucGeneralActivityControl" runat="server"  />
</asp:Content>