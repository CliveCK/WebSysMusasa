<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CallCenterDetails.aspx.vb" Inherits="WebSysME.CallCenterDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="2" style="width:70%;margin-left:2%"><tr>
        		<td class="auto-style6" ><h4>Call Details</h4></td> </tr>

    </table>
    <table cellpadding="2" style="width:90%;margin-left:2%">
        <tr><td>Call Date</td><td style="width: 150px"> <telerik:RadDatePicker ID="radCallDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td></tr>
        <tr><td>Call Code</td><td style="width: 498px"><asp:textbox id="txtCallCode" runat="server" CssClass="form-control"></asp:textbox></td>
        <td>Telephone/Cellphone Number</td><td><asp:textbox id="txtCellNumber" runat="server" CssClass="form-control"></asp:textbox></td></tr>
        <tr><td>District</td><td style="width: 498px"><asp:dropdownlist id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px"></asp:dropdownlist> </td>
         <tr><td>Ward</td><td style="width: 498px"><asp:dropdownlist id="cboWard" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px"></asp:dropdownlist> </td>
        <td>First Name</td><td><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox></td></tr>
        <tr><td>Surname</td><td style="width: 498px"><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox></td>
        <td>National ID #</td><td><asp:textbox id="txtNationalIDNum" runat="server" CssClass="form-control"></asp:textbox></td></tr>
        <tr><td>DOB</td><td style="width: 150px"> <telerik:RadDatePicker ID="radDtDOB" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        <td>Sex</td><td><asp:dropdownlist id="cboSex" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
            </asp:dropdownlist> </td></tr>
        <tr>
            <td>Address</td><td style="width: 498px"><asp:textbox id="txtAddress" runat="server" CssClass="form-control"></asp:textbox></td>
        <td>Type of Issue</td><td><asp:dropdownlist id="cboTypeOfIssue" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px"></asp:dropdownlist> </td></tr>
        <tr>
            <td>Details</td><td style="width: 498px"><asp:textbox id="txtDetails" runat="server" CssClass="form-control" Height="88px" TextMode="MultiLine" Width="500px"></asp:textbox></td></tr>
        <tr>
            <td>Referred From</td><td style="width: 498px"><asp:dropdownlist id="cboReferredFrom" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px"></asp:dropdownlist> </td>
        <td>Referred To</td><td><asp:dropdownlist id="cboReferredTo" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px"></asp:dropdownlist> </td></tr>
        <tr><td>Action Taken</td><td style="width: 498px"><asp:textbox id="txtActionTaken" runat="server" CssClass="form-control" Height="62px" TextMode="MultiLine" Width="500px"></asp:textbox></td></tr>
        <tr><td>Notes/Comments</td><td style="width: 498px"><asp:textbox id="txtNotes" runat="server" CssClass="form-control" Height="62px" TextMode="MultiLine" Width="500px"></asp:textbox></td></tr>

    </table>
    <table cellpadding="2" style="width:70%;margin-left:2%">
        <tr>
        <td colspan="8"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> </tr>
         <tr>  
		<td> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="Clear" CssClass="btn btn-default"></asp:button>
                    <asp:button id="cmdDelete" runat="server" Text="Delete" CssClass="btn btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this CBS Entry?')"></asp:button>
            <asp:TextBox ID="txtCallCenterDetailID" runat="server" CssClass="HiddenControl"></asp:TextBox>

     </td>
       
	</tr>

    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
