<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="KeyChangePromise.aspx.vb" Inherits="WebSysME.KeyChangePromise" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/KeyChangePromisesDetailsControl.ascx" TagName="KeyChangeControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:KeyChangeControl ID="ucKeyChangeControl" runat="server"  />
</asp:Content>
