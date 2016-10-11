<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TrainingAttendantsPage.aspx.vb" Inherits="WebSysME.TrainingAttendantsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/TrainingAttendantsDetailsControl.ascx" TagName="TrainingAttendantsControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:TrainingAttendantsControl ID="ucTrainingAttendantsControl" runat="server"  />
</asp:Content>
