<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ClientDetails.aspx.vb" Inherits="WebSysME.ClientDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding="3" style="width:80%;margin-left:2%">
        <tr><td colspan="4"><h4>Client Details</h4></td></tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
        <tr>
            <td>First Name</td><td><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Surname</td><td><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Date of Birth</td><td>
                <telerik:RadDatePicker ID="radDtDOB" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker>
                </td>
            <td>Sex</td><td>
                <asp:dropdownlist id="cboSex" runat="server" AutoPostBack="true" CssClass="form-control" Width="150px">
                    <asp:ListItem>Male</asp:ListItem>
                    <asp:ListItem>Female</asp:ListItem>
                </asp:dropdownlist>
             </td>
        </tr>
        <tr>            
            <td>Nationality</td><td><asp:textbox id="txtNationality" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>National ID Number</td><td><asp:textbox id="txtNationalIDNum" runat="server" CssClass="form-control" MaxLength="14"></asp:textbox></td>
        </tr>
        <tr>
            <td>No. of Children</td><td><asp:textbox id="txtNumOfChildren" runat="server" TextMode="Number" CssClass="form-control"></asp:textbox></td>
            <td>Level of Education</td> <td><asp:dropdownlist id="cboLevelOfEducation" runat="server" 
                 CssClass="form-control">
                                  </asp:dropdownlist></td>
        </tr>
        <tr>
            <td>Employment Status</td><td><asp:dropdownlist id="cboEmploymentStatus" runat="server"  CssClass="form-control">
                <asp:ListItem Text="Not Employed" Value="Not Employed"></asp:ListItem>
                <asp:ListItem Text="Employed" Value="Employed"></asp:ListItem>
                <asp:ListItem Text="Self Employed" Value="Self Employed"></asp:ListItem>
                               </asp:dropdownlist></td>
            <td>Marriage Type</td><td><asp:dropdownlist id="cboMarriageType" runat="server"  CssClass="form-control">
                                  </asp:dropdownlist></td>
        </tr>
        <tr>
            <td>Phone Number</td><td><asp:textbox id="txtPhoneNum" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Physical Address</td>
            <td><asp:TextBox runat="server" ID="txtAddress" TextMode="MultiLine" Columns="25" Rows="4" CssClass="form-control"></asp:TextBox></td>
        </tr>
        <tr>
            <td>District</td><td><asp:dropdownlist id="cboDistricts" runat="server" CssClass="form-control" AutoPostBack="true">
                                  </asp:dropdownlist></td>
            <td>Ward/Village</td><td><asp:dropdownlist id="cboWards" runat="server" CssClass="form-control">
                                  </asp:dropdownlist></td>
        </tr>
        <tr>
            <td>&nbsp</td>
        </tr>
           <tr>
               <td colspan ="4"><h4> Next of Kin Details</h4></td>
           </tr>
        <tr>
            <td>Name</td><td><asp:textbox id="txtNextOfKin" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Contact Number</td><td><asp:textbox id="txtNxtOfKinConNum" runat="server" CssClass="form-control"></asp:textbox></td>
        </tr>
        <tr>
            <td>Nature of Relationship</td><td><asp:textbox id="txtNatureOfRelationShip" runat="server" CssClass="form-control"></asp:textbox></td>
            <td>Accompanying Children</td><td><asp:textbox id="txtAccompanyingChn" runat="server" CssClass="form-control" TextMode="Number"></asp:textbox></td>
        </tr>
        <tr>
            <td>Accompanying Adult 1</td><td><asp:dropdownlist id="cboAccompanynAdult1" runat="server"  CssClass="form-control">
            <asp:ListItem Text="None" Value="None"></asp:ListItem>
            <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                  </asp:dropdownlist></td>
            <td>Accompanying Adult 2</td><td><asp:dropdownlist id="cboAccompanynAdult2" runat="server"  CssClass="form-control">
            <asp:ListItem Text="None" Value="None"></asp:ListItem>
            <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                  </asp:dropdownlist></td>
        </tr>
        <tr>
            <td>Referred By</td><td><asp:textbox id="txtReferredBy" runat="server" CssClass="form-control"></asp:textbox></td>
            <td></td><td></td>
        </tr>
        <tr>
            <td>Assign Counsellor</td><td><asp:dropdownlist id="cboCounsellor" runat="server"  CssClass="form-control">
                                  </asp:dropdownlist></td><td></td><td></td>
        </tr>
        <tr>
            <td>Assign Lawyer</td><td><asp:dropdownlist id="cboLawyer" runat="server" CssClass="form-control">
                                  </asp:dropdownlist></td><td></td><td></td>
        </tr>
        <tr>
            <td>Refer to Shelter</td><td><asp:dropdownlist id="cboShelter" runat="server"  CssClass="form-control">
                                  </asp:dropdownlist></td>
            <td></td><td></td>
        </tr>
        <tr>
            <td><asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button></td>
            <td><asp:TextBox ID="txtClientDetailID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtAddressID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            </td>
        </tr>
    </table>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
