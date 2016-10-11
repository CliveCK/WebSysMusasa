<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GroupAssociationsPage.aspx.vb" Inherits="WebSysME.GroupAssociationsPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/GroupAssociationsDetailsControl.ascx" TagName="GroupAssociationControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:GroupAssociationControl ID="ucGroupAssociationControl" runat="server"  />
</asp:Content>
