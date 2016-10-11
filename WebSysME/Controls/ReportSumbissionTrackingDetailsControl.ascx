<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReportSumbissionTrackingDetailsControl.ascx.vb" Inherits="WebSysME.ReportSumbissionTrackingDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Report Submission Details</h4></td> 
	</tr> 
	<tr> 
		<td >Organization</td> 
        	<td ><asp:dropdownlist id="cboOrganization" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
    </tr>
    <tr>         
		<td >Report Title</td> 
        	<td ><asp:textbox id="txtReportTitle" runat="server" CssClass="form-control"></asp:textbox> </td> 
    </tr>
    <tr>
		<td >Expected Date</td> 
        	<td ><telerik:RadDatePicker ID="radExpectedDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Actual Submission Date</td> 
        	<td > <telerik:RadDatePicker ID="radActualSub" runat="server" MinDate="1900-01-01" Enabled="false"
                    Width="150px">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr>
		<td >Author</td> 
        	<td ><asp:textbox id="txtAuthor" runat="server" CssClass="form-control"></asp:textbox> </td>
		<td >Comments</td>
        	<td ><asp:textbox id="txtComments" runat="server" TextMode="MultiLine" Rows="5" Columns="30" CssClass="form-control"></asp:textbox> </td>
       </tr> 
    <tr>
        <td>
            Notify
        </td>
        <td>
            <asp:ListBox ID="lstUsers" runat="server" Height="150px" Width="250px" SelectionMode ="Multiple" ></asp:ListBox>
        </td>
    </tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="New" CssClass="btn btn-default"></asp:button>
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtReportSubmissionTrackingID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
