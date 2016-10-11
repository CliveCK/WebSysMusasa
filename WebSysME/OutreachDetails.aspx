<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OutreachDetails.aspx.vb" Inherits="WebSysME.OutreachDetails" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/OutreachDetailsControl.ascx" TagName="OutreachDetails" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:OutreachDetails ID="ucOutreachDetails" runat="server"  />
</asp:Content>
