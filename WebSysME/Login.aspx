<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="WebSysME.Login1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
     <link href="Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Content/Messages.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            background: white url('images/AA.jpg') no-repeat;
            padding: 0px 0px 0px 496px;
            height: 80px;
            width: 467px;
        }
    </style>
</head>
   <body class="LoginBody" style="margin: 0px 0px 0px 0px">
       <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td class="MainHeaderTile" style="width: 100%">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td class="auto-style1">&nbsp;</td>
                            <td class="MainHeaderTile" align="right" valign="top"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
<div style="margin-left:35%;margin-top:10%">
    <form id="form1" runat="server">
        <table width="100%" style="height: 100%;" cellspacing="0" class="LoginTable">
            <tr>
                <td style="height: 203px; width: 29%;">
                    <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse">
                        <tr>
                            <td style="width: 301px;">
                                <asp:Panel ID="pnlLogin" runat="server" GroupingText="Login" Width="382px" CssClass="loginbox">
                                    <table border="0" cellpadding="4" style="background-color: whitesmoke; border-right: gainsboro 1px solid; border-top: gainsboro 1px solid; border-left: gainsboro 1px solid; border-bottom: gainsboro 1px solid;">
                                        <tr>
                                            <td align="left" colspan="3">
                                                <asp:Image ID="Image1" runat="server" AlternateText="WebSysME" ImageUrl="~/images/locked.png"
                                                    Height="64px" Width="64px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">Log In
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="Left">Organisation</td>
                                            <td style="width: 176px;">
                                                <asp:DropDownList ID="cboOrganisation" runat="server" Width="200px" CssClass="form-control"></asp:DropDownList></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td align="Left">Username
                                            </td>
                                            <td style="width: 176px;">
                                                <asp:TextBox ID="txtUserName" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td style="width: 176px; height: 3px">
                                                <asp:RequiredFieldValidator ID="CustomValidator2" runat="server" ControlToValidate="txtPassword"
                                                    ErrorMessage="*" Width="16px"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr style="color: #000000">
                                            <td align="Left">Password
                                            </td>
                                            <td style="width: 176px">
                                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="200px" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td style="width: 176px">
                                                <asp:RequiredFieldValidator ID="CustomValidator3" runat="server" ControlToValidate="txtUserName"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkRememberLogin" runat="server" Text="Keep me logged in on this computer"
                                                    Visible="False" CssClass="form-control"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;
                                            </td>
                                            <td colspan="1"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="font-size: 9px">Please Note: Passwords are case sensitive
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:Panel ID="pnlMessages" runat="server">
                                                    <asp:Label ID="lblMessages" runat="server"></asp:Label>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Button ID="cmdLogin" runat="server" CommandName="Login" OnClick="cmdLogin_Click"
                                                    Text="Log In" ValidationGroup="Login1" CssClass="btn btn-default" />
                                            </td>
                                            <td align="right" colspan="1"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left">
                                                <asp:HyperLink ID="hypLostPassword" runat="server" Text="(Forgot my password)"
                                                    NavigateUrl="~/LostPassword.aspx" CssClass="HiddenControl"></asp:HyperLink></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="pnlResetPassword" runat="server" GroupingText="Reset Password" Width="600px"
                                    CssClass="loginbox" Visible="False">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 382px; height: 230px">
                                        <tr>
                                            <td align="left" class="pagetitle" colspan="3" style="height: 18px"></td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="3" style="">
                                                <asp:Panel ID="pnlResetError" runat="server" Width="381px">
                                                    <asp:Label ID="lblResetError" runat="server" EnableViewState="False" Visible="False"></asp:Label>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td rowspan="3" valign="top">
                                                <table width="100%">
                                                    <tr>
                                                        <td>Old Password
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOldPassword" runat="server" BackColor="#FFFFC0" BorderColor="#CCCCCC"
                                                                BorderStyle="Solid" BorderWidth="1px" Height="18px" TextMode="Password" Width="155px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>New Password
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNewPassword" runat="server" Width="155px" BackColor="#FFFFC0"
                                                                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Height="18px" TextMode="Password"></asp:TextBox>
                                                            <asp:Label ID="lblPasswordPolicyMsg" Width="100%" runat="server" CssClass="HiddenControl"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 155px">Confirm Password
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtConfirmResetPassword" runat="server" Width="155px" BackColor="#FFFFC0"
                                                                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Height="18px" TextMode="Password"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 155px"></td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 155px"></td>
                                                        <td>
                                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPassword"
                                                                ControlToValidate="txtConfirmResetPassword" CssClass="msgError" ErrorMessage="Passwords do not match!"
                                                                ForeColor="Black"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 155px"></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align: left">
                                                            <asp:Button ID="cmdSaveResetPassword" runat="server" Text="RESET PASSWORD" CssClass="submit"
                                                                BorderStyle="None" BorderWidth="0px" />
                                                        </td>                                                    
                                                    </tr>

                                                </table>
                                                &nbsp; &nbsp;&nbsp;
                                            </td>
                                            <td align="left"></td>
                                            <td align="left" rowspan="3" valign="top"></td>
                                        </tr>
                                        <tr>
                                            <td align="left"></td>
                                        </tr>
                                        <tr>
                                            <td align="left"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" rowspan="1" valign="top">Your password has expired. Please reset your password to gain access.
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td >
                </td>
            </tr>
        </table>  
    </form>
    </div>
<<footer style="position:fixed;bottom:0" class="MainHeaderTile">
    <div style="color:antiquewhite;margin-left:37%;font-family:sans-serif">Copyright &copy; 2015 WebSys &ndash; All Rights Reserved</div>
    </footer>
</body>
</html>
