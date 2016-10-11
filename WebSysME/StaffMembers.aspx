<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StaffMembers.aspx.vb" Inherits="WebSysME.StaffMembers"  MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/StaffMemberDetailsControl.ascx" TagName="StaffMembers" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:StaffMembers ID="ucStaffMembers" runat="server"  />
</asp:Content>

