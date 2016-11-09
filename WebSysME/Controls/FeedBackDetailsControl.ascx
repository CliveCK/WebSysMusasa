<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FeedBackDetailsControl.ascx.vb" Inherits="WebSysME.FeedBackDetailsControl" %>
<table cellpadding="3" cellspacing="0" border="0" style="width:100%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>FeedBack</h4></td> 
	</tr>
	<tr> 		
		<td >Office Name</td> 
        	<td ><asp:textbox id="txtOfficeName" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 	 
	<tr>         
		<td >Sex</td> 
        	<td ><asp:DropDownList id="cboSex" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
        	     </asp:DropDownList> </td> 
    </tr>
    <tr>
		<td >Age</td> 
        	<td ><asp:textbox id="txtAge" runat="server" TextMode="Number" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
    <tr>
        <td >Date Completed</td> 
        	<td ><telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td> 
    </tr>
	<tr> 
		<td >What was the nature of your case?</td> 
        	<td ><asp:dropdownlist id="cboNatureOfProblem" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:dropdownlist> </td> 
     </tr>
    <tr> 
		<td >Others (Please specify)</td> 
        	<td ><asp:textbox id="txtOtherProblem" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
	</tr> 
    <tr>
		<td >Which services did you receive at Musasa?</td> 
        	<td ><asp:dropdownlist id="cboAssistance" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:dropdownlist> </td> 
	</tr> 
    <tr>
        <td >Others (Please Specify)</td> 
        	<td ><asp:textbox id="txtOtherAssistance" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
    </tr>
	<tr> 		
		<td >How satisfied were you with the outcome of your case? </td> 
        	<td ><asp:DropDownList id="cboSatisfiedOutcomeOfCase" runat="server" CssClass="form-control">
                         <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Not Satisfied" Value="Not Satisfied"></asp:ListItem>
                        <asp:ListItem Text="Somewhat Satisfied" Value="Somewhat Satisfied"></asp:ListItem>
                        <asp:ListItem Text="Satisfied" Value="Satisfied"></asp:ListItem>
                        <asp:ListItem Text="Very Satisfied" Value="Very Satisfied"></asp:ListItem>
                        <asp:ListItem Text="Not Applicable" Value="Not Applicable"></asp:ListItem>
        	     </asp:DropDownList> </td> 
	</tr> 
    <tr>
        <td >Explain?</td> 
        	<td ><asp:textbox id="txtExplainWhyCase" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
    </tr> 
    <tr>
        <td >How satisfied were you with service provided by Musasa?</td> 
        	<td ><asp:DropDownList id="cboSatisfiedWithService" runat="server" CssClass="form-control">
                             <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Not Satisfied" Value="Not Satisfied"></asp:ListItem>
                        <asp:ListItem Text="Somewhat Satisfied" Value="Somewhat Satisfied"></asp:ListItem>
                        <asp:ListItem Text="Satisfied" Value="Satisfied"></asp:ListItem>
                        <asp:ListItem Text="Very Satisfied" Value="Very Satisfied"></asp:ListItem>
                        <asp:ListItem Text="Not Applicable" Value="Not Applicable"></asp:ListItem>
        	     </asp:DropDownList> </td> 
    </tr>
    <tr>
        <td >Explain Why?</td> 
        	<td ><asp:textbox id="txtExplaiWhyService" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
    </tr>
    <tr>		
		<td >Besides helping in solving your matter, did the support from Musasa help you in other areas of your life?</td> 
        	<td ><asp:DropDownList id="cboHelpInOtherAreas" runat="server" CssClass="form-control">
                         <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                        <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
        	     </asp:DropDownList> </td> 
	</tr> 
    <tr>
		<td >If yes, how was it helpful? </td> 
        	<td ><asp:DropDownList id="cboHowHelpful" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:DropDownList> </td>
    </tr>
    <tr> 
		<td >Challenges with organisations that I was referred to. </td> 
        	<td ><asp:textbox id="txtChallengesWithOrganization" runat="server"  CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
	</tr> 	
	<tr> 
		<td >Challenges with service delivery at Musasa:</td> 
        	<td ><asp:textbox id="txtChallengesWithServiceDelivery" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
     </tr> 
    <tr>
		<td >Other challenges (Please specify)</td> 
        	<td ><asp:textbox id="txtOtherChallenges" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td>
    </tr>
    <tr> 
		<td >What did you expect from Musasa? </td> 
        	<td ><asp:textbox id="txtExpectation" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
    </tr>
    <tr>
		<td >Were your expectations fulfilled?</td> 
        	<td ><asp:DropDownList id="cboExpectationFulfilled" runat="server" CssClass="form-control">
                 <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Fully" Value="Fully"></asp:ListItem>
                        <asp:ListItem Text="Partly" Value="Partly"></asp:ListItem>
                        <asp:ListItem Text="No at all" Value="No at all"></asp:ListItem>
        	     </asp:DropDownList> </td> 
	</tr> 
    <tr>
		<td >Please explain, why. </td> 
        	<td ><asp:textbox id="txtExplainExpectation" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 		
	</tr> 
    <tr>
        <td >After having received services from Musasa, do you now feel safe?</td> 
        	<td ><asp:DropDownList id="cboFeelSafe" runat="server" CssClass="form-control">
                     <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Fully" Value="Fully"></asp:ListItem>
                        <asp:ListItem Text="Partly" Value="Partly"></asp:ListItem>
                        <asp:ListItem Text="No at all" Value="No at all"></asp:ListItem>
        	     </asp:DropDownList> </td> 
    </tr> 	
	<tr> 
		<td >Please explain, why.</td> 
        	<td ><asp:textbox id="txtExplainFeelSafe" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
		</tr> 	
	<tr> 		
		<td >Do you have any recommendations for future improvement of services? </td> 
        	<td ><asp:textbox id="txtRecommendations" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox> </td> 
	</tr>
	<tr> 
		<td >Would you recommend Musasa services to other people? </td> 
        	<td ><asp:DropDownList id="cboWouldRecommend" runat="server"  CssClass="form-control">
                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                        <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
        	     </asp:DropDownList> </td> 
	</tr>
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> &nbsp;
            <asp:button id="cmdNew" runat="server" Text="New" CssClass="btn btn-default"></asp:button>
            <asp:TextBox ID="txtFeedbackID" runat="server" CssClass="HiddenControl"></asp:TextBox>
     </td> 
	</tr> 
	
</table> 
