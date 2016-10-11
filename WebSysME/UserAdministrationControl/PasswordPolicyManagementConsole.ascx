<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PasswordPolicyManagementConsole.ascx.vb" Inherits="WebSysME.PasswordPolicyManagementConsole" %>

<table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
    <tr>
        <td style="width: 20%"></td>
        <td style="width: 80%">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">
            <td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
        </td>
    </tr>
     <tr><td valign="top">
            <asp:ListBox ID="lstPasswordPolicies" runat="server" Rows="10" AutoPostBack="True"
                Width="200px" CssClass="form-control"></asp:ListBox><br />
            <asp:Button ID="cmdAddNewPolicy" runat="server" Text="Add New Password Policy" CssClass="btn btn-default" />
            <asp:Button ID="cmdDelete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this policy?');"
                CssClass="btn btn-default" /></td>
        <td>
            <table width="100%">
                <tr> 
		            <td >Name</td> 
        	            <td ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="*Specify name" ForeColor="#cc0000" ControlToValidate="txtName"></asp:RequiredFieldValidator>  </td>
                </tr>
                <tr> 
		            <td >Description</td> 
        	            <td ><asp:textbox id="txtDescription" runat="server" TextMode="MultiLine" Columns="17" CssClass="form-control"></asp:textbox> </td> 
	            </tr> 
                <tr> 
		            <td >Min Password Length</td> 
        	            <td ><asp:textbox id="txtMinPasswordLength" runat="server" ToolTip="The minimum number of characters that the password should contain" CssClass="form-control"></asp:textbox> </td> 
		            <td >Max Password Length</td> 
        	            <td ><asp:textbox id="txtMaxPasswordLength" runat="server" ToolTip="The minimum number of characters that the password should contain" CssClass="form-control"></asp:textbox> </td> 
	            </tr> 
	            <tr> 
		            <td >Numeric Length</td> 
        	            <td ><asp:textbox id="txtNumericLength" runat="server" ToolTip="The number of numeric characters the password should contain" CssClass="form-control"></asp:textbox> </td> 
		            <td >Upper Case Length</td> 
        	            <td ><asp:textbox id="txtUpperCaseLength" runat="server" ToolTip="The number of Uppercase characters the password should contain" CssClass="form-control"></asp:textbox> </td> 
	            </tr> 
	            <tr> 
		            <td >Special Character Length</td> 
        	            <td ><asp:textbox id="txtSpecialCharacterLength" runat="server" ToolTip="The number of Special characters the password should contain e.g. #$%@^&@*" CssClass="form-control"></asp:textbox> </td> 
		            <td >Password History</td> 
        	            <td ><asp:textbox id="txtPasswordHistory" runat="server" ToolTip="Defines how a user cannot reuse previous passwords. Please specify the history number of previous password..." CssClass="form-control"></asp:textbox> </td> 
	            </tr> 
	            <tr> 
		            <td >Use Dictionary</td> 
        	            <td ><asp:checkbox id="chkUseDictionary" runat="server" ToolTip="Defines if a user's password should or should not contain dictionary words" CssClass="form-control"></asp:checkbox> </td> 
		            <td >Is Active</td> 
        	            <td ><asp:checkbox id="chkIsActive" runat="server"></asp:checkbox> </td> 
	            </tr>
                <tr>
                <td >Password Validity Period (Days)</td> 
        	        <td ><asp:textbox id="txtPasswordValidityPeriod" runat="server"  ToolTip="Defines how long a user's password is valid before a change is needed" CssClass="form-control"></asp:textbox> </td> 
                    <td>PasswordExpires</td>
                    <td ><asp:CheckBox id="chkPasswordExpires" runat="server" ToolTip="Specify if user's passwords expire"></asp:CheckBox> </td> 
                  </tr>	
	            <tr> 
		            <td >Special Characters</td> 
        	            <td ><asp:textbox id="txtSpecialCharacters" runat="server" Text="!@#\\$%*()_+^&amp;}{:;?." ToolTip="Special characters that should be included in the password" CssClass="form-control"></asp:textbox> </td> 
	            <tr> 
		            <td colspan="4"> 
            		            <asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                 </td></tr> 
            </table> 
        </td>
    </tr> 
    <tr>
        <td colspan="2"><asp:TextBox ID="txtPasswordPolicyID" runat="server" CssClass="HiddenControl"></asp:TextBox></td>
    </tr>
</table>