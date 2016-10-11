<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportTracking.aspx.vb" Inherits="WebSysME.ReportTracking" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ReportSumbissionTrackingDetailsControl.ascx" TagName="ReportSubmissionControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:ReportSubmissionControl ID="ucReportSubmissionControl" runat="server"  />
</asp:Content>
