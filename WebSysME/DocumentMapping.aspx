<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DocumentMapping.aspx.vb" Inherits="WebSysME.DocumentMapping" MasterPageFile="~/Site.Master" %>

<%@ Register Src="~/Controls/DocumentObjectsDetailsControl.ascx" TagName="DocumentObjectControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:DocumentObjectControl ID="ucDocumentObjectControl" runat="server" />
</asp:Content>