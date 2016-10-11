<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportUploads.aspx.vb" Inherits="WebSysME.ReportUploads" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1" >
    <asp:Panel ID="pnlReport" runat="server">
        <table cellpadding="3" style="width:80%;margin-left:2%">
            <tr>
                <td><h4>Upload Reports</h4><br /></td>
            </tr>
            <tr>
                <td>Report Title</td>
                <td>
                    <asp:Label runat="server" ID="lblTitle" ForeColor="DarkBlue" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Expected Date of Submission
                </td>
                <td><asp:Label runat="server" ID="lblExpectedDate" ForeColor="DarkBlue" Font-Bold="true" ></asp:Label></td>
            </tr>
            <tr>
                <td>Status</td>
                <td>
                    <asp:Label runat="server" ID="lblStatus" Font-Bold="true"></asp:Label><br />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table cellpadding="3" style="margin-left:2%">
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>Upload report</td>
            <td><asp:FileUpload  runat="server" ID="fuUpload" />
            </td>
        </tr>
        <tr> 
		        <td colspan="2"> <br />
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
                </td> 
	        </tr>
        <tr>
            <td><br />
                <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"/>
            </td>
        </tr>
    </table>
</asp:Content>
