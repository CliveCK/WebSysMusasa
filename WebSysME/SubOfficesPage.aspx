<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SubOfficesPage.aspx.vb" Inherits="WebSysME.SubOfficesPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/SubOfficesDetailsControl.ascx" TagName="SubOfficesControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:SubOfficesControl ID="ucSubOfficesControl" runat="server"  />
</asp:Content>
