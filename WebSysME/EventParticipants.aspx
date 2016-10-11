<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EventParticipants.aspx.vb" Inherits="WebSysME.EventParticipants" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ParticipantsControl.ascx" TagName="ParticipantsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:ParticipantsControl ID="ucParticipantsControl" runat="server"  />
</asp:Content>