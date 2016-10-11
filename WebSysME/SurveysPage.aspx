<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SurveysPage.aspx.vb" Inherits="WebSysME.SurveysPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/SurveyDetailsControl.ascx" TagName="SurveyControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:SurveyControl ID="ucSurveyControl" runat="server"  />
</asp:Content>
