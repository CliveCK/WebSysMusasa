<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MyInFoControl.ascx.vb"
    Inherits="WebSysME.MyInFoControl" %>

<table border="0" cellpadding="3" cellspacing="2" width="70%" >
    <tr>
        <td class="DetailsSection"><b>Login Details</b>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtUsername" runat="server" ReadOnly="True" Width="250px" Enabled="False" CssClass="form-control"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="DetailsSection" colspan="2"><b>Other details</b>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtFirstname" runat="server" Width="250px" CssClass="form-control"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblSurname" runat="server" Text="Surname"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtSurname" runat="server" Width="250px" CssClass="form-control"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblEmail" runat="server" Text="Email Address" ></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" Width="250px" CssClass="form-control"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="Invalid Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td>Mobile No
        </td>
        <td>
            <asp:TextBox ID="txtMobileNo" runat="server" Width="250px" CssClass="form-control"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblCompany" runat="server" Text="Company" CssClass="HiddenControl" ></asp:Label>
        </td>
        <td >
            <asp:DropDownList ID="cboCompany" runat="server" Width="250px" CssClass="HiddenControl">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="cmdSave" runat="server" Text="Save" Width="103px" CssClass="btn btn-default" />
        </td>
        <td style="height: 22px"></td>
    </tr>

    </table>
<br />
<table border="0" cellpadding="3" cellspacing="3" width="50%" style="border:1px solid darkblue;">
    <tr>
        <td class="DetailsSection"style="height: 22px"><b>Change Password</b>
        </td>
    </tr>
    <tr>
        <td style="width: 160px; height: 22px">Old Password
        </td>
        <td style="height: 22px">
            <asp:TextBox ID="txtOldPassword" runat="server" Width="250px" TextMode="Password" CssClass="form-control"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="height: 22px; width: 160px;">
            New Password
        </td>
        <td style="height: 22px">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"
                Width="250px"></asp:TextBox>
            <asp:Label ID="lblmsg" runat="server" ></asp:Label> 
            <asp:Label ID="lblPasswordPolicyMsg" Width="100%" runat="server" CssClass="HiddenControl"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="height: 22px; width: 160px;">
            <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Label>
        </td>
        <td style="height: 22px">
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Width="250px" CssClass="form-control"></asp:TextBox><asp:CompareValidator
                ID="CompareValidator1" runat="server" ErrorMessage="Passwords Do Not Match!"
                ControlToCompare="txtConfirmPassword" ControlToValidate="txtPassword" ForeColor="#cc0000"></asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="cmdChangePassword" runat="server" Text="Change Password" CssClass="btn btn-default" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblError" runat="server" CssClass="Error" Width="100%"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtMinLength" runat="server" Width="50px" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox ID="txtPasswordStrength" runat="server" Width="50px" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox ID="txtPasswordTemplateID" runat="server" Width="50px" CssClass="HiddenControl"></asp:TextBox>
        </td>
    </tr>
</table>    
 <%--<script src="~/../Scripts/jquery.password-strength.min.js" type="text/javascript"></script>
 <script type="text/javascript">
     $(document).ready(function () {
         var myPSPlugin = $("[id$='txtPassword']").password_strength();

         $("[id$='cmdChangePassword']").click(function () {
             return myPSPlugin.metReq(); //return true or false
         });

         $("[id$='passwordPolicy']").click(function (event) {
             var width = 200, height = 400, left = (screen.width / 2) - (width / 2),
             top = (screen.height / 2) - (height / 2);
             window.open("PasswordPolicy.xml", 'Password_poplicy', 'width=' + width + ',height=' + height + ',left=' + left + ',top=' + top);
             event.preventDefault();
             return false;
         });

     });
</script>--%>

