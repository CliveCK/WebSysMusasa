<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GranteeDetails.aspx.vb" Inherits="WebSysME.GranteeDetails" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/GranteeDetailsDetailsControl.ascx" TagName="GranteeDetailsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >  
        <uc1:GranteeDetailsControl ID="ucGranteeDetailsControl" runat="server"  />
</asp:Content>
