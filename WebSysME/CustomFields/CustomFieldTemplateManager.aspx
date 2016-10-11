<%@ Page Language="vb" AutoEventWireup="false" Title="Custom Field Template Security Manager" MasterPageFile="~/Site.Master" CodeBehind="CustomFieldTemplateManager.aspx.vb"
    Inherits="WebSysME.CustomFieldTemplateManager" %>

<%@ Register Src="~/CustomFields/CustomFieldTemplateDetails.ascx" TagName="CustomFieldTemplateDetails"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CustomFieldTemplateDetails ID="CustomFieldTemplateDetails1" runat="server" />
</asp:Content>
