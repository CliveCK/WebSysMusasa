<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InterventionsPage.aspx.vb" Inherits="WebSysME.InterventionsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/InterventionsDetailsControl.ascx" TagName="InterventionsControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:InterventionsControl ID="ucInterventionsControl" runat="server"  />
</asp:Content>

