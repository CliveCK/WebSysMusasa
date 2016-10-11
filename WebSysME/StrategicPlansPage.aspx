<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StrategicPlansPage.aspx.vb" Inherits="WebSysME.StrategicPlansPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/StrategicPlansDetailsControl.ascx" TagName="StrategicPlansControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:StrategicPlansControl ID="ucStrategicPlansControl" runat="server"  />
</asp:Content>
