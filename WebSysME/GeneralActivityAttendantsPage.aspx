<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GeneralActivityAttendantsPage.aspx.vb" Inherits="WebSysME.GeneralActivityAttendantsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/GeneralActivityAttendantsDetailsControl.ascx" TagName="GeneralActivityAttendantsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:GeneralActivityAttendantsControl ID="ucGeneralActivityAttendantsControl" runat="server"  />
</asp:Content>
