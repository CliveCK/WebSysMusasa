<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GeneralActivityDetailsControl.ascx.vb" Inherits="WebSysME.GeneralActivityDetailsControl" %>
<table cellpadding="3" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>General Activity Details</h4></td> 
	</tr> 
	<tr> 
		<td >Activity</td> 
        	<td ><asp:dropdownlist id="cboActivity" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
    <tr>
        <td >Staff</td> 
        	<td ><asp:dropdownlist id="cboStaff" runat="server" CssClass="form-control"></asp:dropdownlist> </td>
    </tr>
	<tr> 
		<td >Subject</td> 
        	<td ><asp:textbox id="txtSubject" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
    <tr>
        <td>Start Date</td>
        <td>
            <telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker>
        </td>
         <td>End Date</td>
        <td>
            <telerik:RadDatePicker ID="radEnd" runat="server" MinDate="1900-01-01"
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
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="2"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="Clear" CssClass="btn btn-default"></asp:button>
     </td> 
        <td colspan="2">
            <asp:button id="cmdDelete" runat="server" Text="Delete" CssClass="btn btn-default" Width="75px" 
                OnClientClick="javascript:return confirm('Are you sure you want to delete this project?')"></asp:button> 
        </td>
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtGeneralActivityID" runat="server" CssClass="HiddenControl"></asp:TextBox> <br />
		</td> 
	</tr> 
    <tr>
        <td><asp:LinkButton ID="lnkAttendants" runat="server" Text="Attendants"></asp:LinkButton></td>
        <td><asp:LinkButton ID="lnkInputs" runat="server" Text="Inputs" ></asp:LinkButton></td>
        <td><asp:LinkButton ID="lnkFiles" runat="server" Text="File uploads" ></asp:LinkButton></td>
    </tr>
</table> 
