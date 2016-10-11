<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SchoolManagement.aspx.vb" Inherits="WebSysME.SchoolManagement" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/SchoolsDetailsControl.ascx" TagName="SchoolsControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:SchoolsControl ID="ucSchoolsControl" runat="server"  />
</asp:Content>
