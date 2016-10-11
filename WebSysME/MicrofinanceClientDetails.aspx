<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MicrofinanceClientDetails.aspx.vb" Inherits="WebSysME.MicrofinanceClientDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="2" style="width:70%;margin-left:2%">
        <tr><td colspan ="4"><h4>Microfinance Client Details</h4></td></tr>
        <tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
        <tr>
            <td>District</td><td><asp:dropdownlist id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px"></asp:dropdownlist></td>
            <td>Ward</td><td><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control" Width="150px"></asp:dropdownlist></td>
        </tr>
        <tr><td>First Name</td><td><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Surname</td><td><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr><td>Sex</td><td><asp:dropdownlist id="cboSex" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
            <asp:ListItem Value="M">Male</asp:ListItem>
            <asp:ListItem Value="F">Female</asp:ListItem>
            </asp:dropdownlist></td>
            <td>DOB</td><td><telerik:RadDatePicker ID="radDOB" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td></tr>
        <tr><td>National ID Number</td><td><asp:textbox id="txtNationaIDNumber"  runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Contact Number</td><td><asp:textbox id="txtContactNumber"  runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr><td style="height: 34px">Address</td><td colspan="3" style="height: 34px"><asp:textbox id="txtAddress"  runat="server" CssClass="form-control" Width="697px"></asp:textbox></td></tr>

        <tr><td colspan="4">&nbsp</td></tr>
        <tr><td colspan ="4"><h4>Loan Details</h4></td></tr>



        <tr><td>Amount Approved</td><td><asp:textbox id="txtAmountApproved"  runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Date Approved</td><td><telerik:RadDatePicker ID="radLoanDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td></tr>
        <tr><td>Monthly Repayment</td><td><asp:textbox id="txtMonthlyRepayment" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Repayment Term (months)</td><td><asp:textbox id="txtRepaymentTerm" runat="server" CssClass="form-control"></asp:textbox></td></tr>
       
        <tr><td colspan="4">&nbsp</td></tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="cmdSave" Text="Save" CssClass="btn btn-default" />
                <asp:TextBox ID="txtMicrofinanceClientDetailID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtAddressID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td colspan="4"><h4>Repayments</h4></td>
        </tr>
         <tr>
            <td>Date</td><td><telerik:RadDatePicker ID="radRePaymentDateDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
            <td>Amount Repayed</td><td><asp:textbox id="txtAmtRepayed" runat="server" CssClass="form-control"></asp:textbox></td>
               <td><asp:button id="cmdSavePayments" runat="server" Text="+" CssClass="btn btn-default"></asp:button> </td>
        </tr>

        <tr><td colspan="4">
    <telerik:RadGrid ID="radRepayPayments" runat="server" Height="80%" 
                    CellPadding="0" Width="100%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="CBSID" UniqueName="CBSID" HeaderText="PaymentID"
                                Display="false">
                             </telerik:GridBoundColumn>   
                            <telerik:GridBoundColumn DataField="DateRepayed" UniqueName="PayBackDate" HeaderText="Date">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AmountRepayed" UniqueName="AmountPaid" HeaderText="Amount Paid Back">
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
                        <PagerStyle Position="Top" AlwaysVisible="true"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
