<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SchedulerPortal.aspx.vb" Inherits="WebSysME.SchedulerPortal" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/Scheduler.ascx" TagName="SchedulerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:SchedulerControl ID="ucSchedulerControl" runat="server"  />
</asp:Content>
<%--  --%>