<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DistributionBeneficiaryPage.aspx.vb" Inherits="WebSysME.DistributionBeneficiaryPage" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/DistributionBeneficiariesDetailsControl.ascx" TagName="DistributionsBeneficiaryControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:DistributionsBeneficiaryControl ID="ucDistributionsBeneficiaryControl" runat="server"  />
</asp:Content>

