<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TrainingMarksPage.aspx.vb" Inherits="WebSysME.TrainingMarksPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/TrainingMarksDetailsControl.ascx" TagName="TrainingMarksControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:TrainingMarksControl ID="ucTrainingMarksControl" runat="server"  />
</asp:Content>
