<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OutcomeObjectives.aspx.vb" Inherits="WebSysME.OutcomeObjectives" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ObjectiveOutcomesDetailsControl.ascx" TagName="ObjectiveOutcomeControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ObjectiveOutcomeControl ID="ucObjectiveOutcome" runat="server"  />
</asp:Content>
