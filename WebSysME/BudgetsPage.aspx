<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BudgetsPage.aspx.vb" Inherits="WebSysME.BudgetsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/BudgetsDetailsControl.ascx" TagName="BudgetsControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:BudgetsControl ID="ucBudgetsControl" runat="server"  />
</asp:Content>