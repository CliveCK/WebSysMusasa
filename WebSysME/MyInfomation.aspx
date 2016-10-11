<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MyInfomation.aspx.vb" Inherits="WebSysME.MyInfomation" MasterPageFile="~/Site.Master"%>

<%@ Register Src="UserAdministrationControl/MyInFoControl.ascx" TagName="MyInFoControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h4 style="background-color:steelblue;color:white;margin-left:3%;width:20%">My Details</h4>
    <table width="100%" style="margin-left:3%">
        <tr>
            <td class="PageTitle" colspan="1">
               </td>
        </tr>
        <tr>
            <td colspan="3">
    <uc1:MyInFoControl id="MyInFoControl1" runat="server">
    </uc1:MyInFoControl>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
