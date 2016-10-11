<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DischargeSummaryDetailsControl.ascx.vb" Inherits="WebSysME.DischargeSummaryDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Discharge Summary</h4></td> 
	</tr> 
	<tr> 
		<td >Hospital</td> 
        	<td ><asp:dropdownlist id="cboHospital" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
        <td >Ward</td> 
        	<td ><asp:textbox id="txtWard" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Discharged by</td> 
        	<td ><asp:textbox id="txtDischarged" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Discharged To</td> 
        	<td ><asp:textbox id="txtDischargedTo" runat="server" CssClass="form-control" ></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Discharged Date</td> 
        	<td ><telerik:RadDatePicker ID="radDischargedDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        <td >Review Date</td> 
        	<td ><telerik:RadDatePicker ID="radReviewDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
    </tr> 
    <tr>
        <td >Reason For Discharge</td> 
        	<td ><asp:textbox id="txtReasonForDischarge" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Columns="30"></asp:textbox> </td> 
    </tr>
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtDischargeSummaryID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
            <asp:TextBox id="txtDischargePatientID" runat="server" CssClass="HiddenControl"></asp:TextBox>
		</td> 
	</tr> 
</table> 
