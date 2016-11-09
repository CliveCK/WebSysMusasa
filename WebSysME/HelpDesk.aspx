<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HelpDesk.aspx.vb" Inherits="WebSysME.HelpDesk" MasterPageFile="~/Site.Master" %>

<%@ Register Src="~/Controls/ClientDeskDetailsControl.ascx" TagName="HelpDeskControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:HelpDeskControl ID="ucHelpDeskControl" runat="server"  />
</asp:Content>

