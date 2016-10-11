<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProjectOutcomes.aspx.vb" Inherits="WebSysME.ProjectOutcomes" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ProjectOutcomeDetailsControl.ascx" TagName="ProjectOutcomeControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ProjectOutcomeControl ID="ucProjectOutcome" runat="server"  />
</asp:Content>
