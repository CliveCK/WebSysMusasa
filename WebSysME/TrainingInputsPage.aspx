<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TrainingInputsPage.aspx.vb" Inherits="WebSysME.TrainingInputsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/TrainingInputsDetailsControl.ascx" TagName="TraininginputsControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:TraininginputsControl ID="ucTraininginputsControl" runat="server"  />
</asp:Content>
