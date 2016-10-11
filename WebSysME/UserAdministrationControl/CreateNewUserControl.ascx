<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CreateNewUserControl.ascx.vb"
    Inherits="WebSysME.CreateNewUserControl" %>
<asp:Panel ID="Panel1" runat="server" GroupingText="   User Details" Width="100%" style="margin-left:2%">

    <table width="100%" cellpadding="3" >
        <tr>
            <td style="height: 21px; width: 131px;">Username
            </td>
            <td style="height: 21px">
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td style="height: 21px">
            <asp:Label ID="lblPasswordPolicyMsg" Width="100%" runat="server" CssClass="Error"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 21px; width: 131px;">Password
            </td>
            <td colspan="2" style="height: 21px" nowrap="nowrap">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Width="287px"></asp:TextBox>
                <asp:CustomValidator ID="cvMinLength" runat="server" ControlToValidate="txtPassword" ValidateEmptyText="True"
                    ClientValidationFunction="clientValidate" Display="Dynamic"></asp:CustomValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="Enter password"></asp:RequiredFieldValidator>
                 <asp:CompareValidator ID="cvPasswordStrength" runat="server" ValueToCompare="3" Type="Integer"
                    ControlToValidate="txtPasswordStrength" Operator = "LessThan" ErrorMessage="Passwords to weak"></asp:CompareValidator><br />
                <label for="chkShowPassword">
                <input type="checkbox" id="chkShowPassword" />
                Show password</label><label style="color:red;font-size:8pt">&nbsp; (please note that this is not recommended)</label>
            </td>
        </tr>
        <tr>
            <td style="height: 21px; width: 131px;">Confirm Password
            </td>
            <td style="height: 21px">
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword"
                    ControlToValidate="txtConfirmPassword" ErrorMessage="Passwords do not match"></asp:CompareValidator>
            </td>
            <td style="height: 21px; color: #000000;"></td>
        </tr>
        <tr style="color: #000000">
            <td style="width: 131px">E-mail Address
            </td>
            <td>
                <asp:TextBox ID="txtEmailAddress" runat="server" Width="250px" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailAddress"
                    ErrorMessage="Invalid Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
            <td></td>
        </tr>
        <tr style="color: #000000">
            <td style="width: 131px">Mobile No
            </td>
            <td>
                <asp:TextBox ID="txtMobileNo" runat="server" Width="250px" CssClass="form-control"></asp:TextBox>
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 131px">User Firstname
            </td>
            <td>
                <asp:TextBox ID="txtFirstname" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="height: 26px; width: 131px;">User Surname
            </td>
            <td style="height: 26px">
                <asp:TextBox ID="txtSurname" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
            </td>
            <td style="height: 26px"></td>
        </tr>
        <tr>
            <td valign="top" style="width: 131px">User Group
            </td>
            <td colspan="2" valign="top">
                <asp:CheckBoxList ID="chkUserGroup" runat="server" RepeatDirection="Horizontal" >
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPasswordExpires" runat="server" Text="Password Expires"></asp:Label>
            </td>
            <td colspan="4">
                <asp:CheckBox ID="chkPasswordExpires" runat="server" onclick="enableTextBox()" /><label>After</label>
                <asp:TextBox ID="txtPasswordValidityPeriod" runat="server" Width="50px" CssClass="form-control" Enabled="false"></asp:TextBox><label>Days</label>
            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top">
                <asp:CheckBox ID="chkIsLockedOut" runat="server" onclick="enableTextBox()" />
                <asp:Label ID="lblIsLockedOut" runat="server" Text="Is Locked Out" Font-Bold="True" Font-Size="10px" ForeColor="Black"></asp:Label>

            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top">
                <asp:CheckBox ID="chkSend" runat="server" Font-Bold="True" Font-Size="10px" ForeColor="Black"
                    Text="Send password email" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtMinLength" runat="server" Width="50px" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtPasswordStrength" runat="server" Width="50px" CssClass="HiddenControl"></asp:TextBox>
                 <asp:TextBox ID="txtPasswordTemplateID" runat="server" Width="50px" CssClass="HiddenControl"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblStatus" Width="100%" runat="server" CssClass="Error"></asp:Label>
            </td>
        </tr>
       
        <tr>
            <td colspan="2">
                <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-default" Width="56px" />
                <asp:Button ID="cmdAddUser" runat="server" CausesValidation="False" Text="Add Another User"
                    Visible="False" Width="25%" CssClass="btn btn-default" />
            </td>
            <td>
                <asp:TextBox ID="txtUserID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel runat="server" ID="pnlLinkAccounts" GroupingText="Link Account to Profile" Font-Size="14px" Visible="false">
        <p style="color:red;font-size:8pt">This sections allows linking of <b>User account</b> to <b>Staff profile</b></p><br />
        <b>Status:</b><asp:Label runat="server" ID="lblLinkStatus" ></asp:Label><br /><br />
        Linked to:<br /> <asp:ListBox runat="server" ID="lstStaffMembers" Height="180px" Width="250px"></asp:ListBox><br />
        <asp:Button runat="server" ID="cmdLink" Text="Link" CssClass="btn btn-default"/>
        <asp:Button runat="server" ID="cmdUnLink" Text="Unlink" CssClass="btn btn-default" Visible="false"/>
    </asp:Panel>
    <script type="text/javascript">
        $(function () {
            $("#chkShowPassword").bind("click", function () {
                var txtPassword = $("[id*=txtPassword]");
                if ($(this).is(":checked")) {
                    txtPassword.after('<input onchange = "PasswordChanged(this);" id = "txt_' + txtPassword.attr("id") + '" width="287px" type = "text" Class="btn btn-default" value = "' + txtPassword.val() + '" /><br />');
                    txtPassword.hide();
                } else {
                    txtPassword.val(txtPassword.next().val());
                    txtPassword.next().remove();
                    txtPassword.show();
                }
            });
        });
        function PasswordChanged(txt) {
            $(txt).prev().val($(txt).val());
        }
</script>
    <script language="javascript" type="text/javascript">
        function enableTextBox() {
            if (document.getElementById('<%= chkPasswordExpires.ClientID %>').checked == false) {
                document.getElementById('<%= txtPasswordValidityPeriod.ClientID %>').disabled = true;
            }
            else {
                document.getElementById('<%= txtPasswordValidityPeriod.ClientID %>').disabled = false;
            }
        }
        document.getElementById("strength").style.display = "none";
    </script>

</asp:Panel>
