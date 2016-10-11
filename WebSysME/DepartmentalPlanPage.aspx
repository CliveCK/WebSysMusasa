<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DepartmentalPlanPage.aspx.vb" Inherits="WebSysME.DepartmentalPlanPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/DepartmentalPlanDetailsControl.ascx" TagName="DepartmentalPlanControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:DepartmentalPlanControl ID="ucDepartmentalPlanControl" runat="server"  />
</asp:Content>
