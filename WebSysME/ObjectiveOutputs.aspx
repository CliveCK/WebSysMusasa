<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ObjectiveOutputs.aspx.vb" Inherits="WebSysME.ObjectiveOutputs" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ObjectiveOutputDetailsControl.ascx" TagName="ObjectiveOutputControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ObjectiveOutputControl ID="ucObjectiveOutput" runat="server"  />
</asp:Content>