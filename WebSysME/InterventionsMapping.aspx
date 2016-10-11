<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InterventionsMapping.aspx.vb" Inherits="WebSysME.InterventionsMapping" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/InterventionObjectsDetailsControl.ascx" TagName="InterventionObjectControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:InterventionObjectControl ID="ucInterventionObjectControl" runat="server"  />
</asp:Content>
