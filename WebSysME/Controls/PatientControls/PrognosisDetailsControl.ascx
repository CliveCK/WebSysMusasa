<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PrognosisDetailsControl.ascx.vb" Inherits="WebSysME.PrognosisDetailsControl" %>
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Prognosis/Assessment of Child's wellbeing</h4></td> 
	</tr> 
	<tr> 
		<td >Date 1</td> 
        	<td > <telerik:RadDatePicker ID="radDate1" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
        <td >Prognosis 1</td> 
        	<td ><asp:textbox id="txtPrognosis1" runat="server" TextMode="MultiLine" Rows="5" Columns="30" CssClass="form-control"> </asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Date 2</td> 
        	<td > <telerik:RadDatePicker ID="radDate2" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
		<td >Prognosis 2</td> 
        	<td ><asp:textbox id="txtPrognosis2" runat="server" TextMode="MultiLine" Rows="5" Columns="30" CssClass="form-control"></asp:textbox> </td> 
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
			<asp:TextBox id="txtPrognosisID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox id="txtPrognosisPatientID" runat="server" CssClass="HiddenControl"></asp:TextBox>
		</td> 
	</tr> 
</table> 
