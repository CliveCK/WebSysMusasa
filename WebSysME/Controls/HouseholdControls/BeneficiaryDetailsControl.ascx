<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BeneficiaryDetailsControl.ascx.vb" Inherits="WebSysME.BeneficiaryDetailsControl" %>
<br />
<div style="padding-left:2%">
<fieldset>
    <legend>Beneficiary Details</legend><br />
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"></td> 
	</tr> 
    <tr>
        <asp:CheckBox ID="chkAddDependant" runat="server" Text="Dependant" Enabled="false" ForeColor="Red" BackColor="#00cc00"/>
        <td>
            <asp:Button runat="server" ID="cmdAddDependant" Text="Add Dependant" CssClass="btn btn-default"/><br />
        </td>
    </tr>
    <tr>
        <td>
            <asp:validationsummary id="valSummary" runat="server" headertext="Validation Errors:" cssclass="ValidationSummary" ForeColor="Red"/>
        </td>
    </tr>
	<tr> 
		<td style="width: 15%">Household No</td> 
        	<td style="width: 35%"><asp:TextBox id="txtMemberNo" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox> </td> 
		<td >Suffix</td> 
        	<td ><asp:textbox id="txtSuffix" runat="server" Enabled="false" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
    <tr> 
		<td >First Name</td> 
        	<td ><asp:textbox id="txtFirstName" runat="server"  CssClass="form-control"></asp:textbox>
                <asp:RequiredFieldValidator ID="rfvFirstName" ErrorMessage="Please enter Firstname" runat="server" ControlToValidate="txtFirstName" ForeColor="Red">*</asp:RequiredFieldValidator>
        	</td> 
		<td >Surname</td> 
        	<td ><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox> 
                <asp:RequiredFieldValidator ID="rfvSurname" ErrorMessage="Please enter Surname" runat="server" ControlToValidate="txtSurname" ForeColor="Red">*</asp:RequiredFieldValidator>
        	</td> 
	</tr> 
	<tr> 
		<td >Marital Status</td> 
        	<td ><asp:DropDownList id="cboMaritalStatus" runat="server" CssClass="form-control"></asp:DropDownList> 
                <asp:RequiredFieldValidator ID="rfvMaritalStatus" ErrorMessage="Please select Marital Status" runat="server" ControlToValidate="cboMaritalStatus" ForeColor="Red">*</asp:RequiredFieldValidator>
        	</td> 
		<td >Health Status</td> 
        	<td ><asp:DropDownList id="cboHealthStatus" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td >Disability Status</td> 
        	<td ><asp:DropDownList id="cboDisabilityStatus" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Level Of Education</td> 
        	<td ><asp:DropDownList id="cboLevelOfEducation" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td >Regularity</td> 
        	<td ><asp:DropDownList id="cboRegularity" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
        <td >Opharnhood</td> 
        	<td ><asp:DropDownList id="cboOpharnhood" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr>
	<tr> 
		<td >Major Source Income</td> 
        	<td ><asp:DropDownList id="cboMajorSourceIncome" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Contact No</td> 
        	<td ><asp:textbox id="txtContactNo" runat="server" CssClass="form-control"></asp:textbox>
                <asp:RequiredFieldValidator ID="rfvContactNo" ErrorMessage="Please enter Contact Number" runat="server" ControlToValidate="txtContactNo" ForeColor="Red">*</asp:RequiredFieldValidator>
        	</td> 
	</tr> 
	<tr> 
		<td >Condition</td> 
        	<td ><asp:DropDownList id="cboCondition" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Attendance</td> 
        	<td ><asp:DropDownList id="cboAttendance" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td >Disability</td> 
        	<td ><asp:DropDownList id="cboDisability" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td >Date Of Birth</td> 
        	<td > <telerik:RadDatePicker ID="radDateofBirth" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput3" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker> 
                <asp:RequiredFieldValidator ID="rfvDateOfBirth" ErrorMessage="Please enter Date Of Birth" runat="server" ControlToValidate="radDateofBirth" ForeColor="Red">*</asp:RequiredFieldValidator>
        	</td> 
	</tr> 	
	<tr> 
		<td >Sex</td> 
        	<td ><asp:DropDownList id="cboSex" runat="server" CssClass="form-control">
                       <asp:ListItem Text="" Value=""></asp:ListItem>
                       <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                       <asp:ListItem Text="Female" Value="F"></asp:ListItem>
        	     </asp:DropDownList> 
                <asp:RequiredFieldValidator ID="rfvSex" ErrorMessage="Please select Sex" runat="server" ControlToValidate="cboSex" ForeColor="Red">*</asp:RequiredFieldValidator>
        	</td> 
		<td >Nationl ID No</td> 
        	<td ><asp:textbox id="txtNationlIDNo" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
    <tr>
        <td>Relationship To HouseHold</td>
        <td><asp:DropDownList runat="server" ID="cboRelationships" CssClass="form-control"></asp:DropDownList></td>
    </tr>
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
      <tr>
        <td colspan="4"><asp:PlaceHolder ID="phCustomFields" runat="server"></asp:PlaceHolder></td>
    </tr>
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> &nbsp;
                    <asp:button id="cmdClear" runat="server" Text="New" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtBeneficiaryID1" runat="server" CssClass="HiddenControl"></asp:TextBox> 
            <asp:TextBox id="txtParentID" runat="server" CssClass="HiddenControl"></asp:TextBox>
		</td> 
	</tr> 
</table>
</fieldset> 
<br />
<fieldset><legend>Beneficiary Listing</legend>
<table cellpadding="0" cellspacing="0" border="0" style="width:600px">
    <tr>
        <td>
            <h3></h3> 
        </td>
    </tr>
    <tr>
            <td>
                <telerik:RadGrid ID="radBenListing" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="BeneficiaryID" UniqueName="BeneficiaryID" HeaderText="BeneficiaryID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="MemberNo" UniqueName="MemberNo" HeaderText="MemberNo">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Suffix" UniqueName="Suffix" HeaderText="Suffix">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateOfBirth" UniqueName="DateOfBirth" HeaderText="DateOfBirth">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NationlIDNo" UniqueName="NationalIDNo" HeaderText="NationalIDNo">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MaritalStatus" UniqueName="MaritalStatus" HeaderText="MaritalStatus">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
</table>
</fieldset>
    </div>
