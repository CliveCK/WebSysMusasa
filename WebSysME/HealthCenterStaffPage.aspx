<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HealthCenterStaffPage.aspx.vb" Inherits="WebSysME.HealthCenterStaffPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/HealthCenterStaffDetailsControl.ascx" TagName="HealthCenterStaffControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:HealthCenterStaffControl ID="ucHealthCenterStaffControl" runat="server"  />
</asp:Content>
