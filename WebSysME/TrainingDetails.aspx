<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TrainingDetails.aspx.vb" Inherits="WebSysME.TrainingDetails" MasterPageFile="~/Site.Master" Title="Training"%>

<%@ Register Src="~/Controls/TrainingDetailsControl.ascx" TagName="TrainingControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:TrainingControl ID="ucTrainingControl" runat="server"  />
</asp:Content>

