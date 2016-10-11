<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BulkMessaging.aspx.vb" Inherits="WebSysME.BulkMessaging" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/ComplementaryListboxes.ascx" TagName="ComplementaryListboxes"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1" >
        <table cellpadding="3" style="width:80%;margin-left:2%">
            <tr>
                <td><h4>Bulk Messaging</h4><br /></td>
            </tr>
            <tr>
                <td>Message Type</td>
                <td>
                    <asp:DropDownList runat="server" ID="cboMessageType" CssClass="form-control">
                        <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                        <asp:ListItem Text="SMS" Value="SMS"></asp:ListItem>
                        <asp:ListItem Text="Email & SMS" Value="Both"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Subject
                </td>
                <td><asp:TextBox runat="server" ID="txtSubject" CssClass="form-control" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Message</td>
                <td><asp:TextBox runat="server" ID="txtMessage" CssClass="form-control" TextMode="MultiLine" Columns="40" Rows="4"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Recipients Type</td>
                <td>
                    <asp:DropDownList runat="server" ID="cboRecipientsType" CssClass="form-control" AutoPostBack="true">
                         <asp:ListItem Text="--Select Recipient Type--" Value=""></asp:ListItem>
                         <asp:ListItem Text="StaffMembers" Value="Staff"></asp:ListItem>
                         <asp:ListItem Text="Beneficiaries" Value="Beneficiary"></asp:ListItem>
                         <asp:ListItem Text="System Users" Value="User"></asp:ListItem>
                    </asp:DropDownList><br />
                </td>
            </tr>
            <tr>
                <td>
                    Recipients
                </td>
                <td>
                    <uc2:ComplementaryListboxes ID="ucRecipients" runat="server" CssClass="form-control"></uc2:ComplementaryListboxes>
                </td>
            </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>        
        <tr> 
		   <td colspan="2"> <br />
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
                </td> 
	        </tr>
        <tr>
            <td><asp:Button  runat="server" ID="btnCampaign" Text="Send" CssClass="btn btn-default"/>
            </td>
        </tr>
    </table>
</asp:Content>

