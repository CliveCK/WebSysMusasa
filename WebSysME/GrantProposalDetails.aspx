<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GrantProposalDetails.aspx.vb" Inherits="WebSysME.GrantProposalDetails" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/GrantProposalDetailsControl.ascx" TagName="GrantProposalControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:GrantProposalControl ID="ucGrantProposalControl" runat="server"  />
</asp:Content>