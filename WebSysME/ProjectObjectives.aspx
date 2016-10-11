<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProjectObjectives.aspx.vb" Inherits="WebSysME.ProjectObjectives" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ProjectObjectiveDetailsControl.ascx" TagName="ProjectObjectiveControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ProjectObjectiveControl ID="ucProjectObjective" runat="server"  />
</asp:Content>