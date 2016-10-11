<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FileUploadControl.ascx.vb" Inherits="WebSysME.FileUploadControl" %>

<%@ Register Src="~/Controls/ComplementaryListboxes.ascx" TagName="ComplementaryListboxes"
    TagPrefix="uc2" %>
<div style="padding-left:2%">
<table width="100%" cellpadding="3">
    <tr>
        <td>
            <h3>File Details</h3>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>Upload File</td>
        <td>
            <asp:FileUpload runat="server" ID="fuFileUpload" />
        </td>
    </tr>
    <tr>
        <td>Project File Type:</td>
        <td><asp:DropDownList runat="server" ID="drpFileType" CssClass="form-control">
            </asp:DropDownList></td>
     </tr>
    <tr>
        <td>Title:</td>
        <td><asp:TextBox runat="server" ID="txtTitle" CssClass="form-control"></asp:TextBox></td>
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
        <td>Author</td>
        <td><asp:TextBox runat="server" ID="txtAuthor" CssClass="form-control"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Author Organisation</td>
        <td><asp:TextBox runat="server" ID="txtAuthorOrg" CssClass="form-control"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Description</td>
        <td><asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5" Columns="40" CssClass="form-control"></asp:TextBox></td>
    </tr>
     <tr>
        <td>Apply Security</td>
        <td><asp:CheckBox  runat="server" ID="cbxApplySecurity" AutoPostBack="true"></asp:CheckBox></td>
    </tr>
    <tr> 
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
    <tr>
        <td colspan="2">
            <asp:Panel ID="pnlMapping" runat="server" GroupingText="File mapping"  >
                <p style="color:red;font-size:8pt">This section is for mapping the uploaded file.</p>
                <br />
                Applies to:&nbsp;<asp:DropDownList ID="cboObject" runat="server" AutoPostBack="true" >
                </asp:DropDownList><br />
                &nbsp;
                <uc2:ComplementaryListboxes ID="ucFileMapping" runat="server" CssClass="form-control"></uc2:ComplementaryListboxes>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel ID="pnlPermissions" runat="server" GroupingText="File Permissions" Visible="false" >
                <p style="color:red;font-size:8pt">This section defines who can have access to the document uploaded.</p>
                <br />
                Applies to:&nbsp;<asp:DropDownList ID="cboLevelOfSecurity" runat="server" AutoPostBack="true" >
                    <asp:ListItem Text ="Users" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text ="Organizations" Value="2"></asp:ListItem>
                </asp:DropDownList><br />
                &nbsp;
                <uc2:ComplementaryListboxes ID="ucComplementaryListboxes" runat="server" CssClass="form-control"></uc2:ComplementaryListboxes>
            </asp:Panel>
        </td>
    </tr>
	<tr> 
		<td> 
            		<asp:button id="cmdSave" runat="server" Text="Save" class="btn btn-default" Width="75px"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="New File" class="btn btn-default" Width="75px"></asp:button> 
     </td> 
        <td>
                    <asp:button id="cmdDelete" runat="server" Text="Delete" class="btn btn-default" Width="75px"></asp:button> 
        </td>
	</tr> 
	<tr> 
		<td colspan="2"> 
			<asp:textbox id="txtFileID" runat="server" Visible="false"></asp:textbox>
            <asp:TextBox ID="txtFilePath" runat="server" Visible="false"></asp:TextBox>
		</td> 
	</tr> 
</table>
<table cellpadding="0" cellspacing="0" border="0" style="width:90%">
    <tr>
        <td>
            <h3>File Listing</h3> 
        </td>
    </tr>
    <tr>
            <td>
                <telerik:RadGrid ID="radFileListing" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
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
                             <telerik:GridBoundColumn DataField="DownloadCount" UniqueName="DownloadCount" HeaderText="DownloadCount">
                            </telerik:GridBoundColumn>
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
                        <PagerStyle Position="Top" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
</table>
</div>