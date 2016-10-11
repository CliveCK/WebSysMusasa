<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MedicalHistoryDetailsControl.ascx.vb" Inherits="WebSysME.MedicalHistoryDetailsControl" %>
<table cellpadding="3" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Medical History</h4></td> 
	</tr> 
	<tr> 
		<td >Condition</td> 
        	<td ><asp:dropdownlist id="cboCondition" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Date Of Diagnosis</td> 
        	<td ><telerik:RadDatePicker ID="radDateOfDiagnosis" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
	</tr> 
	<tr> 
		<td >Diagnosis</td> 
        	<td ><asp:DropDownList id="cboDiagnosis" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Reason For Unstaging</td> 
        	<td ><asp:DropDownList id="cboReasonForUnstaging" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
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
			<asp:TextBox id="txtMedicalHistoryID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
		</td> 
	</tr> 
</table> 
