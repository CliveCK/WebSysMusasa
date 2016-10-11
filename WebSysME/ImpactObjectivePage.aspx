<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImpactObjectivePage.aspx.vb" Inherits="WebSysME.ImpactObjectivePage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ImpactObjectiveControl.ascx" TagName="ImpactObjectiveControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ImpactObjectiveControl ID="ucImpactObjective" runat="server"  />
</asp:Content>
