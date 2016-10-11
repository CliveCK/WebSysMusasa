<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportSubmission.aspx.vb" Inherits="WebSysME.ReportSubmission" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="margin-left:2%">
        <tr>
            <td><h4>Reports by Status</h4></td>
        </tr>
        <tr> 
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr>
        <tr>
            <td>Select Status</td>
            <td>
                <asp:DropDownList runat="server" ID="cboStatus" AutoPostBack="true" CssClass="form-control">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Submitted" Value="Submitted"></asp:ListItem>
                    <asp:ListItem Text="Overdue" Value="Overdue"></asp:ListItem>
                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><br /></td>
        </tr>
        <tr>
            <td><br /><asp:Button ID="cmdNew" runat="server" Text="Add New" CssClass="btn btn-default" /><br /></td>
        </tr>
        <tr>
            <td colspan="2">
                 <telerik:RadGrid ID="radReports" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="ReportSubmissionTrackingID" UniqueName="ReportSubmissionTrackingID" HeaderText="ReportSubmissionTrackingID"
                                Display="false">
                            </telerik:GridBoundColumn>   
                            <telerik:GridBoundColumn DataField="FileID" UniqueName="FileID" HeaderText="FileID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SubmissionStatus" UniqueName="SubmissionStatus" HeaderText="SubmissionStatus"
                                Display="false">
                            </telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="Organization" UniqueName="Organization" HeaderText="Organization"  >
                            </telerik:GridBoundColumn>                       
                             <telerik:GridBoundColumn DataField="ReportTitle" UniqueName="ReportTitle" HeaderText="Report Title"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Author" UniqueName="Author" HeaderText="Author">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ExpectedDate" UniqueName="ExpectedDate" HeaderText="Expected Date">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ActualSubmissionDate" UniqueName="ActualSubmissionDate" HeaderText="Actual Submission Date">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" Text='<%# IIf(Eval("SubmissionStatus") = True, "Uploaded", IIf(Eval("ExpectedDate") < Now, "Overdue", "Pending"))%>' runat="server"
                                        ForeColor='<%# IIf(Eval("SubmissionStatus") = False, Drawing.Color.Red, Drawing.Color.Green)%>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn CommandName="Download" UniqueName="cmdDownload" ButtonCssClass="btn btn-default" Text="Download" >
                            </telerik:GridButtonColumn>
                            <telerik:GridButtonColumn CommandName="Upload" UniqueName="cmdUpload"  ButtonCssClass="btn btn-default" Text="Upload" >      
                            </telerik:GridButtonColumn>
                        </Columns>
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <PagerStyle Position="Top" AlwaysVisible="true"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid><br />
            </td>
        </tr>
    </table>
</asp:Content>
