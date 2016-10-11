<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GrantManagementDetails.aspx.vb" Inherits="WebSysME.GrantManagementDetails"  MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/GrantDetailsDetailsControl.ascx" TagName="GrantDetailsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >  
        <uc1:GrantDetailsControl ID="ucGrantDetailsControl" runat="server"  />
</asp:Content>
