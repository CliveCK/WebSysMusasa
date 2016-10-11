<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BudgetTrackingPage.aspx.vb" Inherits="WebSysME.BudgetTrackingPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/BudgetTrackingDetailsControl.ascx" TagName="BudgetsTrackingControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:BudgetsTrackingControl ID="ucBudgetsTrackingControl" runat="server"  />
</asp:Content>
