<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ClientCounsellingSessionActivityDetails.aspx.vb" Inherits="WebSysME.ClientCounsellingSessionActivityDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="2" style="width:70%;margin-left:2%">
        <tr>
            <td colspan="4"><h4>Client Counselling Session Activity Details</h4></td>
        </tr>
        <tr>
            <td>&nbsp</td>
        </tr>
          <tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
        <tr>
            <td>Firstname</td> <td><asp:textbox id="txtFirstName" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Surname</td><td><asp:textbox id="txtSurname" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Sex</td><td> <asp:dropdownlist id="cboSex" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
                    <asp:ListItem>Male</asp:ListItem>
                    <asp:ListItem>Female</asp:ListItem>
                </asp:dropdownlist></td>
            <td>DOB</td><td><telerik:RadDatePicker ID="radDOB" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td>National ID Number</td><td><asp:textbox id="txtNationalIDNumber" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Contact Number</td><td><asp:textbox id="txtContactNumber" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>&nbsp</td>
        </tr>
        <tr>
            <td colspan="4"><h4>Details</h4> </td>
        </tr>
        <tr>
            <td>Date</td>
            <td colspan="3"> <telerik:RadDatePicker ID="radActivityDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td>Activity</td><td colspan="3"><asp:textbox id="txtActivity" runat="server" CssClass="form-control" Height="76px" TextMode="MultiLine" Width="691px"></asp:textbox></td>
        </tr>
        <tr>
            <td>Description</td><td colspan="3"><asp:textbox id="txtDescription" runat="server" CssClass="form-control" Height="85px" TextMode="MultiLine" Width="694px"></asp:textbox></td>
        </tr>
        <tr>
            <td>Outcome</td><td colspan="3"><asp:textbox id="txtOutcome" runat="server" CssClass="form-control" Height="81px" TextMode="MultiLine" Width="692px"></asp:textbox></td>
        </tr>
        <tr>
            <td>Remarks</td><td colspan="3"><asp:textbox id="txtRemarks" runat="server" CssClass="form-control" Height="91px" TextMode="MultiLine" Width="690px"></asp:textbox></td>
        </tr>
        <tr>
            <td>&nbsp</td>
        </tr>

        <tr>
            <td>&nbsp</td><td><asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button></td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtClientSessionActivityID" runat="server" CssClass="HiddenControl" ></asp:TextBox>
                <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            </td>
        </tr>        
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
