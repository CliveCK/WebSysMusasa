<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IntakePage.aspx.vb" Inherits="WebSysME.IntakePage" MasterPageFile="~/Site.Master" %>

<%@ Register Src="~/Controls/IntakeDetailsControl.ascx" TagName="IntakeDetailsControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:IntakeDetailsControl ID="ucIntakeDetailsControl" runat="server"  />
</asp:Content>
