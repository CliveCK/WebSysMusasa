<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CBSMemberDetails.aspx.vb" Inherits="WebSysME.CBSMemberDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


        <table style="width:90%;margin-left:2%">
            <tr>
                <td><asp:LinkButton ID="lnkCBSReportingList" runat="server" Text="Back to CBS Members List"></asp:LinkButton> <br /></td>
            </tr>
            <tr>
                <td>Firstname</td><td><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox> </td>
                <td>Surname</td><td><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox> </td>
            </tr>
            <tr>
                <td>ID Number</td><td><asp:textbox id="txtIDNumber" runat="server" CssClass="form-control"></asp:textbox> </td>
                <td>Sex</td><td class="auto-style1" ><asp:dropdownlist id="cboSex" runat="server" CssClass="form-control">
                <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                </asp:dropdownlist> </td> 
            </tr>
            <tr>
                <td>DOB</td><td><telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
            </tr>
            <tr>
                <td>&nbsp</td>
            </tr>
            <tr><td colspan="5"></tr>
        </table>
  <div style="padding-left:10%">
    <fieldset style="align-content:center"><legend style="align-self:center">Need</legend>
<table style="margin-left:10%">
    <tr>
        <td >
           <br />
        </td>
    </tr>
    <tr>
        <td colspan="5"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br /><br />
     </td> 
    </tr>
    <tr>
        <td>Nature of Problem </td><td>Assistance and services Provided</td><td>Referred To </td><td>Comments</td><td>&nbsp</td>
    </tr>
    <tr>
        <td style="height: 38px">
            <asp:DropDownList ID="cboNeeds" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
        </td>
        <td style="height: 38px">
            <asp:DropDownList ID="cboAssistance" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
        </td>
        <td style="height: 38px">
            <asp:DropDownList ID="cboReferredTo" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
        </td>
        <td style="height: 38px"><asp:textbox id="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Width="200px"></asp:textbox></td>
    </tr>
    <tr>
        <td style="height: 38px">
            <asp:Button runat="server" ID="cmdSave" Text="Save" CssClass="btn btn-default"/>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadGrid ID="radBenListing" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="NatureOfProblemID" UniqueName="NatureOfProblemID" HeaderText="NatureOfProblemID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Problem" UniqueName="Problem" HeaderText="Problem">
                            </telerik:GridBoundColumn>   
                              <telerik:GridBoundColumn DataField="AssistanceProvided" UniqueName="AssistanceProvided" HeaderText="AssistanceProvided">
                            </telerik:GridBoundColumn>  
                              <telerik:GridBoundColumn DataField="ReferredTo" UniqueName="ReferredTo" HeaderText="ReferredTo">
                            </telerik:GridBoundColumn>  
                              <telerik:GridBoundColumn DataField="Comments" UniqueName="Comments" HeaderText="Comments">
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
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
</table>
 </fieldset>
</div>
    <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl"></asp:TextBox>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
