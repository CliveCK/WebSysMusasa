<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TripDocuments.ascx.vb" Inherits="WebSysME.TripDocuments" %>

<table>
    <tr>
        <td><br /></td>
    </tr>
    <tr>
        <td>Upload File</td>
        <td>
            <asp:FileUpload runat="server" ID="fuFileUpload" />
        </td>
    </tr>
    <tr>
        <td>Title:</td>
        <td><asp:TextBox runat="server" ID="txtTitle" CssClass="form-control"></asp:TextBox></td>
    </tr>
     <tr>
        <td>Author</td>
        <td><asp:TextBox runat="server" ID="txtAuthor" CssClass="form-control"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Date:</td>
        <td><telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
    </tr>
    <tr>
        <td>Description</td>
        <td><asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5" Columns="40" CssClass="form-control"></asp:TextBox></td>
    </tr>
    <tr> 
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="2"> 
            		<asp:button id="cmdSave" runat="server" Text="Upload File" class="btn btn-default" Width="75px"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="New File" class="btn btn-default" Width="75px"></asp:button> 
     </td> 
	</tr>
    <tr>
        <td><asp:textbox id="txtFileID" runat="server" Visible="false"></asp:textbox>
            <asp:textbox id="txtTripID" runat="server" Visible="false"></asp:textbox>
            <asp:TextBox ID="txtFilePath" runat="server" Visible="false"></asp:TextBox>
            <asp:textbox id="txtFileType" runat="server" Visible="false"></asp:textbox>
        </td>
    </tr> 
    <tr>
            <td colspan="2">
                <telerik:RadGrid ID="radFileListing" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
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
                </telerik:RadGrid>
            </td>
        </tr>
</table>