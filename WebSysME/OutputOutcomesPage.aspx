<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OutputOutcomesPage.aspx.vb" Inherits="WebSysME.OutputOutcomesPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/OutputOutcomeDetailsControl.ascx" TagName="OutputOutcomeControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:OutputOutcomeControl ID="ucOutputOutcome" runat="server"  />
</asp:Content>
