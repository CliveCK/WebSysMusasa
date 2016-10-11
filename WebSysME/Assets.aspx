<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Assets.aspx.vb" Inherits="WebSysME.Assets" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/HouseholdControls/HouseHoldAFWS.ascx" TagName="FileUploadControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:FileUploadControl ID="ucProjectControl" runat="server"  />
</asp:Content>
