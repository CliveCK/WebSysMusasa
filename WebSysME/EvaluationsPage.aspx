<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EvaluationsPage.aspx.vb" Inherits="WebSysME.EvaluationsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/EvaluationDetailsControl.ascx" TagName="EvaluationControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:EvaluationControl ID="ucEvaluationControl" runat="server"  />
</asp:Content>
