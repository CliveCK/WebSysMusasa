<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StategicObjectivesPage.aspx.vb" Inherits="WebSysME.StategicObjectivesPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/StrategicObjectivesDetailsControl.ascx" TagName="StrategicObjectivesControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:StrategicObjectivesControl ID="ucStrategicObjectivesControl" runat="server"  />
</asp:Content>
