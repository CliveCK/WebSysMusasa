<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProcumentPage.aspx.vb" Inherits="WebSysME.ProcumentPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ProcumentDetailsControl.ascx" TagName="ProcumentControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:ProcumentControl ID="ucProcumentControl" runat="server"  />
</asp:Content>
