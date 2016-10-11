<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TrainingMarksDetailsControl.ascx.vb" Inherits="WebSysME.TrainingMarksDetailsControl" %>

<table cellpadding="3" cellspacing="0" style="width:100%;margin-left:2%">
    <tr>
        <td>
            <h4>Training Marks</h4>
        </td>
    </tr>
    <tr> 
		<td >Training</td> 
        	<td ><asp:dropdownlist id="cboTraining" runat="server" CssClass="form-control" Enabled="false" ></asp:dropdownlist> </td> 
	</tr> 
<tr> 
		<td >Period</td> 
        	<td ><asp:dropdownlist id="cboPeriod" runat="server" CssClass="form-control" ></asp:dropdownlist> </td> 
    </tr>
    <tr> 
		<td >Block</td> 
        	<td ><asp:dropdownlist id="cboBlock" runat="server" CssClass="form-control" ></asp:dropdownlist> </td> 
    </tr>
    <tr> 
		<td >Paper</td> 
        	<td ><asp:dropdownlist id="cboPaper" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
    </tr>
    <tr> 
		<td >Attendant Type</td> 
        	<td ><asp:dropdownlist id="cboBeneficiaryType" runat="server" CssClass="form-control" ></asp:dropdownlist> </td> 
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td style="color:red;font-size:8pt">Use the search button below to filter results...</td>
    </tr>
    <tr >
        <td>Group Type
            <asp:DropDownList ID="cboHealthGroupType" runat="server" CssClass="form-control"></asp:DropDownList>
        </td>
        <td>
            Province
            <asp:DropDownList ID="cboProvince" runat="server" CssClass="form-control"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="btn btn-default"/>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2" style="color:red;font-size:8pt">Please note in order to add marks, you must process the attendants for the paper, block and period</td>
    </tr>
    <tr>
        <td style="font-family:'Segoe UI';color:darkblue">Unprocessed Attendant(s)</td>
    </tr>
    <tr>
        <td colspan="2">
            <telerik:RadGrid ID="radAttendants" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" AllowMultiRowSelection="true" CellPadding="0" Width="70%">
                    <MasterTableView AutoGenerateColumns="True" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <PagerStyle AlwaysVisible ="true" Position="Top"/>
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="AttendantID" UniqueName="AttendantID" HeaderText="AttendantID" Display="false" >
                            </telerik:GridBoundColumn>                            
                           <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                            UniqueName="chkRowSelect">
                        </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AttendantID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove from Training?')"
                                    ToolTip="Click to remove " />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                                                    
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
                    </MasterTableView>
                    <ClientSettings Selecting-AllowRowSelect ="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
   <tr> 
		<td colspan="2"> 
            		<asp:button id="cmdSave" runat="server" Text="Process" CssClass="btn btn-default"></asp:button> 
     </td> 
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
		<td style="font-family:'Segoe UI';color:darkblue">Attendants Perfomance Scores<br /> <br /></td>
    </tr>
    <tr>
        <td colspan="2">
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" CssClass="div-container no-bg" LoadingPanelID="RadAjaxLoadingPanel1">
 
        <telerik:RadGrid ID="radBeneficiaries" runat="server" AutoGenerateColumns="False"  AllowSorting="true" Width="90%">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <MasterTableView EditMode="InPlace" DataKeyNames="TrainingMarkID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="ObjectID" SortExpression="ObjectID" Display="false">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ObjectID")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="ObjectIDTextBox" runat="server" Width="100px" Text='<%# Bind("ObjectID")%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" SortExpression="Name">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "FirstName")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="NameTextBox" runat="server" Width="100px" Text='<%# Bind("FirstName")%>' Enabled="false">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Surname" SortExpression="Surname">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Surname")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="LastNameTextBox" runat="server" Width="100px" Text='<%# Bind("Surname")%>' Enabled="false">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Description" SortExpression="Description">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Description")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="DescriptionTextBox" runat="server" Width="100px" Text='<%# Bind("Description")%>' Enabled="false">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Mark" SortExpression="Mark">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Mark")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <telerik:RadNumericTextBox Width="60px" ID="MarkTextBox" runat="server" MinValue="0"
                                DbValue='<%# Bind("Mark")%>' Type="Number" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Comments" SortExpression="Comments">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Comments")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <telerik:RadTextBox Width="120px" ID="CommentsTextBox" runat="server" Text='<%# Bind("Comments")%>' >
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UpdateText="Update" CancelText="Cancel"
                        EditText="Edit">
                    </telerik:GridEditCommandColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
        </td> 
	</tr> 
	<tr> 
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	</tr> 
    <tr>
        <td><asp:LinkButton runat="server" ID="lnkBack" Text="Back to Training"></asp:LinkButton></td>
    </tr>
</table>