<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="InitialCounsellingSession.aspx.vb" Inherits="WebSysME.InitialCounsellingSession" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="3" style="width:80%;margin-left:2%">
         <tr>
               <td colspan="3"><h4>Initial Client Counselling Session Details</h4></td>
           </tr>
         <tr><td>&nbsp;</td></tr>
         <tr>
            <td colspan="5"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br /><br />
         </td>
         </tr>
        <tr>
            <td colspan="4">&nbsp</td>
        </tr>
        <tr>
                <td>First Name</td><td><asp:textbox id="txtFirstName"  runat="server" CssClass="form-control"></asp:textbox></td>
                <td>Surname</td><td><asp:textbox id="txtSurname"  runat="server" CssClass="form-control"></asp:textbox></td>
           </tr>
        <tr>
                <td>DOB</td><td>  <telerik:RadDatePicker ID="radDOB" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
                <td>Sex</td><td><asp:dropdownlist id="cboSex" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem Value="M">Male</asp:ListItem>
            <asp:ListItem Value="F">Female</asp:ListItem>
            </asp:dropdownlist></td>
           </tr>
        <tr>
                <td>National ID Number</td><td><asp:textbox id="txtNationalIDNumber"  runat="server" CssClass="form-control" MaxLength="15"></asp:textbox></td>
                <td>Level of Education</td><td><asp:DropDownList id="cboLevelOfEducation"  runat="server" CssClass="form-control"></asp:DropDownList></td>
           </tr>
        <tr>
                <td>Employment Status</td><td><asp:textbox id="txtEmploymentStatus"  runat="server" CssClass="form-control"></asp:textbox></td>
                <td>Marriage Type</td><td><asp:DropDownList id="cboMaritalStatus" runat="server" CssClass="form-control"></asp:DropDownList></td>
           </tr>
        <tr>
                <td>Phone Number</td><td><asp:textbox id="txtPhoneNumber" runat="server" CssClass="form-control"></asp:textbox></td>
                <td>Address</td><td><asp:textbox id="txtAddress"  runat="server" CssClass="form-control"></asp:textbox></td>
           </tr>
        <tr>
                <td>District</td><td><asp:DropDownList id="cboDistricts"  runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList></td>
                <td>Ward</td><td><asp:DropDownList id="cboWards" runat="server" CssClass="form-control"></asp:DropDownList></td>
           </tr>
         <tr>
                <td>Next of Kin</td><td><asp:textbox id="txtNextOfKin"  runat="server" CssClass="form-control"></asp:textbox></td>           
                <td>Referred By</td><td><asp:textbox id="txtReferredBy"  runat="server" CssClass="form-control"></asp:textbox></td>
                <td></td><td></td>
           </tr>     
         <tr>
               <td colspan="3"><h4>Initial Session Details</h4></td>
           </tr>            
        <tr>
                <td>Presenting Problem</td><td colspan="3"><asp:textbox id="txtPresentingProblem" runat="server" CssClass="form-control" Height="110px" TextMode="MultiLine" Width="200px"></asp:textbox></td>
                
           </tr>        
        <tr>             
                <td>Specify Item Above</td><td><asp:textbox id="txtPropSpecification" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox></td>
                <td>Problem Category</td><td><asp:ListBox id="lstProblemCategory" runat="server" CssClass="form-control" Height="100px" SelectionMode="Multiple"></asp:ListBox></td>
           </tr>
        <tr>
                <td>Case was reported</td><td><asp:dropdownlist id="cboCaseWasReported" runat="server" AutoPostBack="true" CssClass="form-control" >
                <asp:ListItem>Yes</asp:ListItem>
                <asp:ListItem>No</asp:ListItem>
                </asp:dropdownlist></td>
                <td>How many times</td><td><asp:textbox id="txtTimesRpted" runat="server" CssClass="form-control"></asp:textbox></td>
           </tr>
        <tr>
                <td>Where was problem reported</td><td><asp:textbox id="txtPlaceWhereReported" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox></td>
                <td>Challenges faced</td><td><asp:textbox id="txtChallengesFaced" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox></td>
           </tr>
        <tr>
                <td>Medical Report</td><td><asp:dropdownlist id="cboMedicalReport" runat="server" AutoPostBack="true" CssClass="form-control" >
                <asp:ListItem>Yes</asp:ListItem>
                <asp:ListItem>No</asp:ListItem>
                </asp:dropdownlist></td>
                <td>Issued By</td><td><asp:textbox id="txtIssuedBy" runat="server" CssClass="form-control"></asp:textbox></td>
           </tr>
        <tr>
                <td>Problems Experienced</td><td><asp:textbox id="txtProblemsFaced" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox></td>
                <td>Client Expectations</td><td><asp:textbox id="txtClientExpectations" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox></td>
           </tr>
           <tr>
                <td>Other Option Available</td><td><asp:textbox id="txtOtherOptionAvailable" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox></td>
                <td>Referral if any and for what</td><td><asp:textbox id="txtReferral" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox></td>
           </tr>
           <tr>
                <td>Support given to client</td><td ><asp:TextBox ID="txtSupport" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox></td>
           </tr>
           <tr>
                <td>Refer to Lawyer</td><td><asp:dropdownlist id="cboLawyer" runat="server"  CssClass="form-control" ></asp:dropdownlist></td>
                <td>Refer to Shelter</td><td><asp:dropdownlist id="cboshelter" runat="server" CssClass="form-control"></asp:dropdownlist></td>
           </tr>
           <tr>
                <td>Care Plan</td><td><asp:textbox id="txtCarePlan" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:textbox></td>
                <td></td><td></td></tr>
        <tr>
            <td>Date Of Session</td><td>
                    <telerik:RadDatePicker ID="radSessionDate" runat="server" MinDate="1900-01-01" Enabled="false">
                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput2" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker>
                                              </td>
        </tr>
               <tr>
                <td>Next Appointment Date</td><td>
                    <telerik:RadDatePicker ID="radDtNext" runat="server" MinDate="1900-01-01">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker>
                                              </td>
                <td>Duration of Session (Minutes)</td><td><asp:textbox id="txtSessionDuration" runat="server" CssClass="form-control"></asp:textbox></td>
           </tr>
               <tr>
                   <td colspan="4"><hr /></td>
               </tr>
           <tr>
               <td>&nbsp</td>
               <td>
                   <asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                   <asp:TextBox ID="txtInitialCounsellingSessionID" runat="server" CssClass="HiddenControl"></asp:TextBox> 
                   <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl"></asp:TextBox>  
                   <asp:TextBox ID="txtAddressID" runat="server" CssClass="HiddenControl"></asp:TextBox>                  
               </td>
               <td>
                  <asp:button id="cmdClear" runat="server" Text="Clear" CssClass="btn btn-default"></asp:button>
               </td>
           </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
