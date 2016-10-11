<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LawyerClientSessionDetails.aspx.vb" Inherits="WebSysME.LawyerClientSessionDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="3" style="width:60%;margin-left:2%">
        <tr>
            <td><h4>Lawyer - Client Session Details</h4></td><td style="width: 184px"></td>
        </tr>
         <tr><td>&nbsp;</td></tr>
         <tr>
            <td colspan="5"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br /><br />
         </td> 
    </tr>
        <tr>
            <td colspan="4">&nbsp</td>
        </tr>
        <tr>
            <td>First Name</td><td style="width: 184px"><asp:textbox id="txtFirstName" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Surname</td><td><asp:textbox id="txtSurname" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
       
        <tr>
            <td>Marriage Type</td><td><asp:DropDownList id="cboMarriageType" runat="server" CssClass="form-control"></asp:DropDownList></td>
        </tr>
         <tr>
            <td>&nbsp</td>
        </tr>
        <tr style="display:none">
            <td>Date</td><td style="width: 184px"><telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
            
        </tr>
        <tr><td>&nbsp</td></tr>
        <tr>
            <td>Nature of Problem</td><td colspan="3">
                <telerik:RadGrid ID="radProblems" runat="server" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="98%" AllowMultiRowSelection="True" AutoGenerateColumns="False" >
                     <ClientSettings Selecting-AllowRowSelect ="true">
                         <Selecting AllowRowSelect="True"></Selecting>
                        </ClientSettings>
                    <MasterTableView AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="8">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="NatureOfProblemID" UniqueName="NatureOfProblemID" HeaderText="NatureOfProblemID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridClientSelectColumn DataType="System.Boolean" FilterControlAltText="Filter chkRowSelect column"
                                 UniqueName="chkRowSelect">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NatureOfProblemID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to remove this item?')"
                                    ToolTip="Click to remove " />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                             <telerik:GridBoundColumn DataField="Description" UniqueName="Name" HeaderText="Problem">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
        <tr><td>&nbsp</td></tr>
        <tr>
            <td>Date Of Session</td>
            <td>
                 <telerik:RadDatePicker ID="radSessionDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td>If other, explain</td><td colspan="3" ><asp:textbox id="txtOtherProblem" runat="server" CssClass="form-control" TextMode="MultiLine" ></asp:textbox></td>
        </tr>
        <tr>
            <td>Case Notes</td><td colspan="3"><asp:textbox id="txtCaseNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Height="72px" Width="671px"></asp:textbox></td>
        </tr>
        <tr>
            <td>Action to be taken</td><td style="width: 184px"><asp:dropdownlist id="cboActionToBeTaken" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem Text="Legal Representation" Value="1"></asp:ListItem>
            <asp:ListItem Text="Self Representation" Value="2"></asp:ListItem>
            <asp:ListItem Text="Advice Only" Value="3"></asp:ListItem>
            
            </asp:dropdownlist></td>
        </tr>
        <tr>
            <td>Referred to</td><td ><asp:dropdownlist id="cboReferredTo" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            </asp:dropdownlist></td>
        </tr>
        <tr>
            <td>If other, explain</td>
            <td colspan="3" ><asp:textbox id="txtOtherReferDetails" runat="server" CssClass="form-control" Height="60px" TextMode="MultiLine"></asp:textbox></td>
        </tr>
        <tr>
            <td></td><td ></td>
        </tr>
        <tr>
            <td><asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button>
                <asp:TextBox ID="txtLawyerClientSessionDetailID" runat="server" CssClass="HiddenControl" ></asp:TextBox>
                 <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl" ></asp:TextBox>
            </td><td style="width: 200px"></td>
        </tr>
       
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
