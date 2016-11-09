<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ShelterClientDetails.aspx.vb" Inherits="WebSysME.ShelterClientDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="2" style="width:80%;margin-left:2%">
        <tr>
            <td colspan="4"><h4>Admission into Shelter</h4></td>
        </tr>
        <tr><td>&nbsp;</td></tr>
         <tr>
            <td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br /><br />
         </td> 
    </tr>
        <tr>
            <td>Name</td><td><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Surname</td><td><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>National ID Number</td><td><asp:textbox id="txtNationalIDNum" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>DOB</td><td>
                <telerik:RadDatePicker ID="radDtDOB" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker>
                        </td>
        </tr>
        <tr>
            <td>Sex</td><td><asp:dropdownlist id="cboSex" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem Value="M">Male</asp:ListItem>
            <asp:ListItem Value="F">Female</asp:ListItem>
            </asp:dropdownlist> </td>
            <td>Contact Number</td><td><asp:textbox id="txtClientContactNumber" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
         <tr>
            <td>District of Residence</td><td><asp:dropdownlist id="cboDistricts" runat="server" CssClass="form-control" AutoPostBack="true">
                                  </asp:dropdownlist></td>
            <td>Ward/Village</td><td><asp:dropdownlist id="cboWards" runat="server" CssClass="form-control">
                                  </asp:dropdownlist></td>
        </tr>
        <tr>
            <td>Arrival Time</td><td><asp:textbox id="txtArrivalTime" runat="server" CssClass="form-control" TextMode="Time"></asp:textbox></td>
            <td>Total # Admitted</td><td><asp:textbox id="txtTotalPeopleAdmitted" runat="server" CssClass="form-control" TextMode="Number"></asp:textbox></td>
        </tr>
        <tr>
            <td>Permanent Address</td><td><asp:textbox id="txtClientPermanentAddress" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Marital Status</td><td><asp:dropdownlist id="cboMaritalStatus" runat="server"  CssClass="form-control" Width="150px"></asp:dropdownlist> </td>
        </tr>

        <tr>
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><h4>Employment Details</h4></td>
        </tr>

        <tr>
            <td>Employment Status</td><td><asp:dropdownlist id="cboEmpoymentStatus" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem Text="Unemployed" Value="Unemployed"></asp:ListItem>
            <asp:ListItem Text="Employed" Value="Employed"></asp:ListItem>
            <asp:ListItem Text="Self-Employed" Value="Self-Employed"></asp:ListItem>
            </asp:dropdownlist> </td>
            <td>Telephone Number</td><td><asp:textbox id="txtEmployerTelNumber" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>

        <tr>
            <td>Physical Address</td><td colspan="3"><asp:textbox id="txtEmployerAddress" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><h4>Referral Details</h4></td>
        </tr>
        <tr>
            <td>Referred By</td><td><asp:textbox id="txtReferredBy" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Referrer's Tel Number</td><td><asp:textbox id="txtReferrerTelNum" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Have you ever been sheltered before</td><td><asp:dropdownlist id="cboEverSheltered" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist> </td>
            
        </tr>
        <tr>
            <td>Do you have any skills you would like to nature</td><td><asp:dropdownlist id="cboSkillsToNature" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist> </td>
            <td>State the skills</td><td><asp:textbox id="txtSkillsToNature" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td colspan="4" style="height: 24px"></td>
        </tr>
        <tr>
            <td colspan="4"><h4>In Case of Emergency</h4></td>
        </tr>
        <tr>
            <td>Name</td><td><asp:textbox id="txtEmergencyContactName" runat="server" CssClass="form-control"></asp:textbox></td>
            <td></td><td></td>
        </tr>
        <tr>
            <td>Relationship</td><td><asp:textbox id="txtRelationToEmergencyContact" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Telephone #</td><td><asp:textbox id="txtEmergencyContactNumber" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Address</td><td><asp:textbox id="txtEmergencyContactAddress" runat="server" CssClass="form-control"></asp:textbox></td>
            
        </tr>
        <tr>
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><h4>Client History</h4></td>
        </tr>
        <tr>
            <td>Injuries sustained in attack</td><td><asp:textbox id="txtInjuries" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Any special medical needs</td><td><asp:dropdownlist id="cboSpecialNeeds" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:dropdownlist> </td>
            <td>If yes, state them</td><td><asp:textbox id="txtSpecialNeeds" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Care Plan</td><td><asp:textbox id="txtCarePlan" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Presenting Problem</td><td><asp:textbox id="txtPresentingProblem" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Have you been tested for HIV?</td> 
            <td><asp:CheckBox ID="chkTestedForHIV" runat="server" /></td>
        </tr>
        <tr>
            <td>If yes, would you like to Diclose your Status?</td> 
            <td><asp:CheckBox ID="chkDiscloseStatus" runat="server" /></td>
        </tr>
        <tr>
            <td>If yes, your HIV Status</td>
            <td><asp:DropDownList ID="cboHIVStatus" runat="server" CssClass="form-control" >
                    <asp:ListItem Text="Undisclosed" Value="Undisclosed"></asp:ListItem>
                    <asp:ListItem Text="Positive" Value="Positive"></asp:ListItem>
                     <asp:ListItem Text="Negative" Value="Negative"></asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Are you on ART</td>
            <td><asp:DropDownList ID="cboOnArt" runat="server" CssClass="form-control" >
                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                    <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                     <asp:ListItem Text="NO" Value="NO"></asp:ListItem>                
                     <asp:ListItem Text="Don't Know" Value="Don't Know"></asp:ListItem>
                </asp:DropDownList></td>
        </tr>        
        <tr>
            <td>Referred To</td>
            <td><asp:DropDownList ID="cboReferredTo" runat="server" CssClass="form-control" ></asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="4"></td>
        </tr>
        <tr>
            <td><asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button>
                <asp:TextBox ID="txtShelterClientDetailID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtAddressID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                          </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
