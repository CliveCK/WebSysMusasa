<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CounsellingReturningClientDetails.aspx.vb" Inherits="WebSysME.CounsellingReturningClientDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="2" style="width:80%;margin-left:2%">
         <tr>
               <td colspan="4"><h4>Returning Client Counselling Session Details</h4></td>
           </tr>
        <tr>
            <td style="width: 310px">&nbsp;</td>
        </tr>
       <tr>
        <td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br /><br />
     </td> 
    </tr>
        <tr>
                <td style="width: 310px">First Name</td><td><asp:textbox id="txtFirstName" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
                <td>Surname</td><td><asp:textbox id="txtSurname" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
           </tr>
        <tr>
                <td style="width: 310px">DOB</td><td colspan="3"><telerik:RadDatePicker ID="radDateOfBirth" runat="server" MinDate="1900-01-01"
                    >
                    <Calendar ID="Calendar4" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput4" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td></tr>
        <tr>
                <td style="width: 310px">Sex</td><td><asp:dropdownlist id="cboSex" runat="server" AutoPostBack="true" CssClass="form-control" >
                    <asp:ListItem Value="M">Male</asp:ListItem>
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    </asp:dropdownlist></td>
               </tr>
        <tr>
                <td style="width: 310px">National ID Number</td><td><asp:textbox id="txtNationalIDNumber"  runat="server" CssClass="form-control"></asp:textbox></td>
                <td>Level of Education</td><td><asp:DropDownList id="cboLevelOfEducation"  runat="server" CssClass="form-control"></asp:DropDownList></td>
           </tr>
         <tr>
                <td style="width: 310px">Phone Number</td><td><asp:textbox id="txtPhoneNumber" Enabled="false" runat="server" CssClass="form-control"></asp:textbox></td>
                <td>Address</td><td><asp:textbox id="txtAddress"  runat="server" CssClass="form-control"></asp:textbox></td>
           </tr>
        <tr>
                <td style="width: 310px">District</td><td><asp:DropDownList id="cboDistricts"  runat="server" CssClass="form-control"></asp:DropDownList></td>
                <td>Ward</td><td><asp:DropDownList id="cboWards" runat="server" CssClass="form-control"></asp:DropDownList></td>
           </tr>
       
       <tr><td colspan="4">&nbsp;</td></tr>
        <tr>
            <td colspan ="4"><h4>Details</h4></td>
        </tr>
        <tr><td colspan="4">&nbsp;</td></tr>
        <tr>
            <td style="width: 310px" >Date of Session</td><td colspan="3"><telerik:RadDatePicker ID="radSessionDate" runat="server" MinDate="1900-01-01"
                    Enabled="false" >
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td style="width: 310px">Have you been abused after visiting Musasa?</td><td ><asp:dropdownlist id="cboAbusedAfterMusasa" runat="server" AutoPostBack="true" CssClass="form-control" >
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist></td>            
        </tr>
        <tr>
            <td style="width: 310px">How</td><td ><asp:textbox id="txtAbusedHow" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">How many times</td><td ><asp:textbox id="txtNumberOfAbuses" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">Date of last abuse</td><td colspan="3"><telerik:RadDatePicker ID="radDtOfLastAbuse" runat="server" MinDate="1900-01-01"
                    >
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td style="width: 310px">Has the previous abuse continued?</td><td ><asp:dropdownlist id="cboPrevAbuseContinued" runat="server" AutoPostBack="true" CssClass="form-control" >
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist></td>
        </tr>
        <tr>
            <td style="width: 310px">Has a new kind of abuse started?</td><td ><asp:dropdownlist id="cboNewKindOfAbuseStarted" runat="server" AutoPostBack="true" CssClass="form-control" >
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist></td>
       
            <td style="width: 310px">What kind of abuse?</td><td ><asp:textbox id="txtNewAbuse" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">Is the new abuse linked to Musasa visit?</td><td ><asp:dropdownlist id="cboNewAbuseLinkedToMuasasaVisit" runat="server" AutoPostBack="true" CssClass="form-control" >
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist></td>
        
            <td style="width: 310px">Was any report made to the Police?</td><td ><asp:dropdownlist id="cboReportMedToPolice" runat="server" AutoPostBack="true" CssClass="form-control" >
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist></td>
        </tr>
        <tr>
            <td style="width: 310px">Name the Police Station</td><td ><asp:textbox id="txtPoliceStation" runat="server" CssClass="form-control"></asp:textbox></td>
        
            <td style="width: 310px">Name of Officer handling complaint</td><td ><asp:textbox id="txtPoliceOfficer" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">Was Police Officer helpful</td><td ><asp:dropdownlist id="cboWasPolcOfficerHelpful" runat="server" AutoPostBack="true" CssClass="form-control" >
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist></td>
        </tr>
        <tr>
            <td style="width: 310px">If no, why?</td><td ><asp:textbox id="txtHowOfficerWsNotHelpful" runat="server" CssClass="form-control"  TextMode="MultiLine" ></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">Any Medical Report?</td><td ><asp:dropdownlist id="cboMedicalReport" runat="server" AutoPostBack="true" CssClass="form-control" >
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist></td>
        </tr>
        <tr>
            <td style="width: 310px">Report Was issued by</td><td ><asp:textbox id="txtReportIssuedBy" runat="server" CssClass="form-control" ></asp:textbox></td>
        
            <td style="width: 310px">Any permanent injuries sustained?</td><td ><asp:textbox id="txtPermanentInjuries" runat="server" CssClass="form-control"  TextMode="MultiLine" cols="25"></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">Issues from previous session</td><td ><asp:textbox id="txtIssuesFromPrevSession"  runat="server" CssClass="form-control"  TextMode="MultiLine" ></asp:textbox></td>
        
            <td style="width: 310px">Feedback from last session</td><td ><asp:textbox id="txtFeedback"  runat="server" CssClass="form-control" TextMode="MultiLine" ></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">New issues raised</td><td ><asp:textbox id="txtNewIssues"  runat="server" CssClass="form-control" TextMode="MultiLine" ></asp:textbox></td>
        
            <td style="width: 310px">Options available to Client</td><td ><asp:textbox id="txtClientOptions" runat="server" CssClass="form-control"  TextMode="MultiLine" ></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">Action to be taken by client</td><td ><asp:textbox id="txtClientActions" runat="server" CssClass="form-control"  TextMode="MultiLine" ></asp:textbox></td>
       
            <td style="width: 310px">Action to be taken by Counsellor</td><td ><asp:textbox id="txtCounsellorActions"  runat="server" CssClass="form-control" TextMode="MultiLine" ></asp:textbox></td>
        </tr>
        <tr>
            <td style="width: 310px">Next appointment date</td><td colspan="3"><telerik:RadDatePicker ID="radDtOfNxtAppontmt" runat="server" MinDate="1900-01-01"
                    >
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td colspan="4"><asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button>
                <asp:TextBox ID="txtReturningClientDetailID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtAddressID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
