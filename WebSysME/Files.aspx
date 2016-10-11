<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Files.aspx.vb" Inherits="WebSysME.Files" MasterPageFile="~/Site.Master"%>
<%@ Register Src="~/Controls/FileUploadControl.ascx" TagName="FileUploadControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:FileUploadControl ID="ucProjectControl" runat="server"  />
</asp:Content>
