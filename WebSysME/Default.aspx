<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WebSysME._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <table Cellpadding="4"style="width:100%">
            <tr>
                <td style="width:60%">                    
                   <center>
                    <table>
                        <tr>
                           <td>&nbsp</td>

                        </tr>
                        <tr>
                           <td align="center"><h4 style="font-family:'Segoe UI';font-size:2em;font-weight:bold;color:#1B335D"> Projects Information Management System</h4></td>

                        </tr>
                         <tr>
                           <td>&nbsp</td>

                        </tr>
                         <tr>
                           <td align="center">Licensed to:</td>

                        </tr>
                         <tr>
                           <td>&nbsp</td>

                        </tr>
                         <tr>
                           <td align="center"><asp:Image runat="server" AlternateText="CompanyLogo" ID="imgCompanyLogo"/>></td>

                        </tr>
                         <tr>
                           <td>&nbsp</td>

                        </tr>
                         <tr>
                           <td  align="center"><img src="images/zimbabwe.jpg" /></td>

                        </tr>
                    </table>
                    </center> 
                </td>
               <td valign="top">
                   <center>
                       <table Cellpadding="4">
                           <tr>
                                <td style="font-size:13px">&nbsp;<br />
                                     Welcome, <asp:Label ID=lblUser runat="server" Font-Bold="true" ForeColor="OliveDrab"></asp:Label><br />
                                            Today is <asp:Label runat="server" ID="lblDayDesc" Font-Bold="true"></asp:Label>, <asp:Label runat="server" ID="lblDay" Font-Bold="true"></asp:Label>
                                            <asp:Label runat="server" ID="lblMonth" Font-Bold="true"></asp:Label> <asp:Label runat="server" ID="lblYear" Font-Bold="true"></asp:Label>.
                                    <fieldset>
                                        <legend>
                                        </legend>
                                            <asp:Panel ID="pnlNotifications" runat="server" Width="400px" Height="150px">
                                                            &nbsp;
                                                You have <asp:HyperLink runat="server" ID="hypNotifications" Text="0 New" ></asp:HyperLink> notifications.
                                            </asp:Panel>
                                    </fieldset>
                                </td>
                           </tr>
                       </table>
                </center>
               </td>
            </tr>
        </table>
</asp:Content>
