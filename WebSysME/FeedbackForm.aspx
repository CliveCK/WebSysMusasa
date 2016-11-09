<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FeedbackForm.aspx.vb" Inherits="WebSysME.FeedbackForm" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/FeedBackDetailsControl.ascx" TagName="FeedBackControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:FeedBackControl ID="ucFeedBackControl" runat="server"  />
</asp:Content>

