<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OutputActivity.aspx.vb" Inherits="WebSysME.OutputActivity" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/OutputActivityDetailsControl.ascx" TagName="OutputActivityControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:OutputActivityControl ID="ucOutputCtivity" runat="server"  />
</asp:Content>