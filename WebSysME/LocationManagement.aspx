<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LocationManagement.aspx.vb" Inherits="WebSysME.LocationManagement" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/UrbanAreaControl.ascx" TagName="UrbanAreaControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/RuralAreaControl.ascx" TagName="RuralAreaControl" TagPrefix="uc2" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:UrbanAreaControl ID="ucUrbanAreaControl" runat="server"  />
    <hr /><br />
     <uc2:RuralAreaControl ID="ucRuralAreaControl" runat="server"  />
</asp:Content>
