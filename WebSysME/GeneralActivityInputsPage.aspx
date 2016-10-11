<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GeneralActivityInputsPage.aspx.vb" Inherits="WebSysME.GeneralActivityInputsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/GeneralActivityInputsDetailsControl.ascx" TagName="GeneralActivityInputsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:GeneralActivityInputsControl ID="ucGeneralActivityInputsControl" runat="server"  />
</asp:Content>
