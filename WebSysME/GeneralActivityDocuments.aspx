<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GeneralActivityDocuments.aspx.vb" Inherits="WebSysME.GeneralActivityDocuments" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="4" style="margin-left:2%">
        <tr>
            <td>Attach Documents/Files<br /></td>
        </tr>
        <tr> 
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br />
     </td> 
	</tr> 
        <tr>
            <td>File</td>
            <td>
                <asp:FileUpload runat="server" ID="fuTraining" />
            </td>
        </tr>
        <tr>
            <td>Title</td>
            <td><asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Author</td>
            <td><asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Description</td>
            <td><asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Columns="30"></asp:TextBox> </td>
        </tr>
        <tr>
            <td>Organization</td>
            <td><asp:DropDownList ID="cboOrganization" runat="server" CssClass="form-control"></asp:DropDownList><br /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="cmdUpload" Text="Upload" CssClass="btn btn-default"/><br /></td>
        </tr>
        <tr>
            <td colspan="2">
                 <telerik:RadGrid ID="radFileListing" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="900px">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="FileID" UniqueName="FileID" HeaderText="FileID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="FilePath" UniqueName="FilePath" HeaderText="FilePath" Display="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="Title" UniqueName="Title" HeaderText="Title">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FileType" UniqueName="FileType" HeaderText="FileType">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Author" UniqueName="Author" HeaderText="Author">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AuthorOrganization" UniqueName="AuthorOrganization" HeaderText="AuthorOrganization">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Date" UniqueName="DateUploaded" HeaderText="DateUploaded">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="PushButton" Text="Download File" UniqueName="column"
                                CommandName="Download" ButtonCssClass="btn btn-default">
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
        <tr>
        <td>
            <asp:LinkButton runat="server" ID="lnkBack" Text="<< Back to Training Details" Font-Bold="true"></asp:LinkButton>
        </td>
    </tr>
    </table>
</asp:Content>

