<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile= "~/Sample.Master" CodeBehind="ReportsTest.aspx.vb" Inherits="SampleApp.ReportsTest" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
             <tr>
                        <td valign="top" align="center">
                            <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="SqlDataSource1" DataTextField="ProjectName" DataValueField="ProjectID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CustomFields %>" SelectCommand="SELECT [ProjectName], [ProjectID] FROM [Projects]"></asp:SqlDataSource>
                            <asp:DropDownList ID="DropDownList1" runat="server">
                                <asp:ListItem>zdbsBb</asp:ListItem>
                                <asp:ListItem>bdfbdb</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="TextBox1" runat="server" Width="71px"></asp:TextBox>
                            <asp:Button ID="Button1" runat="server" Text="Button" />
                            <%-- <CR:CrystalReportSource ID="crsreport" runat="server" CacheDuration ="180" >
                            </CR:CrystalReportSource>--%>

                            <%--<CR:CrystalReportViewer ID="crReports" runat="server" AutoDataBind="true" />--%>

                            <asp:Label ID="lblReports" runat="server" Text="REPORTS" Font-Size="45"> </asp:Label>

                            <CR:CrystalReportViewer ID="crReports" runat="server" AutoDataBind="True"
                                HasCrystalLogo="False" ReportSourceID="CrystalReportSource" Width="100%" Height="100%" HasSearchButton="True" HasZoomFactorList="True" HasPrintButton="True" HasExportButton="True" > </CR:CrystalReportViewer>
                            
                            <CR:CrystalReportSource ID="CrystalReportSource" runat="server">
                                <report filename="C:\Users\Leslie\Desktop\CCPT\Custom Fields Appv2\TelerikWebApp1\TelerikWebApp1\SampleApp\Reports\trainingDetails.rpt">
                                </report>
                            </CR:CrystalReportSource>
                               
                        </td>
                    </tr>
    </table>
</asp:Content>
