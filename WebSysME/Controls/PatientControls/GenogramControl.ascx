<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GenogramControl.ascx.vb" Inherits="WebSysME.GenogramControl" %>

<div style="margin-left:2%">
    <table>
        <tr>
            <td align="center"><h4>Genogram</h4></td>
        </tr>
        <tr>
           <td>&nbsp</td>

        </tr>
        <tr>
            <td>Upload File:</td>
            <td align="center"><asp:FileUpload ID="fupload" runat="server" /></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="cmdSave" runat="server" Text="Upload" CssClass="btn btn-default" />
            </td>
        </tr>
        <tr>
           <td>&nbsp</td>

        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Image ID="mImage" runat="server" AlternateText="Image Goes here..."/>
            </td>
        </tr>
         <tr> 
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
        <tr>
            <td>
                <asp:TextBox runat="server" ID="txtGenogramID" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtFilePath" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtPatientID" CssClass="HiddenControl"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
