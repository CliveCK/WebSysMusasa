<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Beneficiary.aspx.vb" Inherits="WebSysME.Beneficiary"  MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/BeneficiaryControl.ascx" TagName="BeneficiaryControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:BeneficiaryControl ID="ucBeneficiaryControl" runat="server"  />
</asp:Content>
