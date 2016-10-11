<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MainOneStopCentreDetails.aspx.vb" Inherits="WebSysME.MainOneStopCentreDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:LinkButton ID="lnkOneStopCentreList" runat="server" Text="Back to List"></asp:LinkButton> <br />
    <table cellpadding="2" style="width:70%;margin-left:2%">

	<tr> 
		<td class="auto-style6" >Centre Name</td> 
        	<td class="auto-style1" ><asp:dropdownlist id="cboClub" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
         <td>&nbsp</td>
          <td>&nbsp</td>
	</tr> 
    <tr>
         <td>Year</td> <td><asp:textbox id="txtYear" runat="server" CssClass="form-control" ></asp:textbox></td>
    </tr>
    <tr>    
       
		<td  >Month</td> 
        <td ><asp:dropdownlist id="cboMonth" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 

	</tr>
     </table>
     <table cellpadding="2" style="width:90%;margin-left:2%">
         <tr>
        
    <td>Survivor's Details
         <hr /><br />
        <table cellpadding="2" style="width:90%;margin-left:2%">
                  <tr> 
		<td  >Province</td> 
        	<td ><asp:dropdownlist id="cboProvince" runat="server" AutoPostBack="true" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
          <td>&nbsp</td>
          <td>&nbsp</td>
	</tr> 
    <tr> 
		<td  >District</td> 
        	<td><asp:dropdownlist id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
         <td>&nbsp</td>
          <td>&nbsp</td>
	</tr> 
        <tr> 
		<td >Ward</td> 
        	<td  ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
         <td>&nbsp</td>
          <td>&nbsp</td>
	</tr> 
            <tr><td>Firstname</td>
                <td><asp:textbox id="txtFirstName" runat="server" CssClass="form-control"></asp:textbox> </td>
                <td>Surname</td>
                <td><asp:textbox id="txtSurname" runat="server" CssClass="form-control"></asp:textbox> </td></tr>
            <tr>
                <td>ID Number</td>
                <td><asp:textbox id="txtIDNumber" runat="server" CssClass="form-control"></asp:textbox> </td>
                <td>Sex</td>
                <td ><asp:dropdownlist id="cboSex" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                    <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                                             </asp:dropdownlist> </td> </tr>
               <tr>
                   <td>DOB</td> 
                   <td><telerik:RadDatePicker ID="radDate" runat="server" MinDate="1900-01-01"
                    Width="150px">
                    <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                        ViewSelectorText="x">
                    </Calendar>
                    <DateInput ID="Dateinput1" runat="server" DateFormat="dd/MMM/yyyy" DisplayDateFormat="dd/MMM/yyyy">
                    </DateInput>
                </telerik:RadDatePicker></td>
            </tr>
            <tr>  
		<td> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="Clear" CssClass="btn btn-default"></asp:button>
                    <asp:button id="cmdDelete" runat="server" Text="Delete" CssClass="btn btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this CBS Entry?')"></asp:button>
            <asp:TextBox ID="txtMainOneStopCenter" runat="server" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox ID="txtBeneficiaryID" runat="server" CssClass="HiddenControl"></asp:TextBox>
            <asp:TextBox ID="txtAddressID" runat="server" CssClass="HiddenControl"></asp:TextBox>
     </td>
       
	</tr>
            <tr>
                <td>&nbsp</td>
            </tr>
            </table>
        <table>
            <tr><td colspan="5">
                <div>
<fieldset style="align-content:flex-start"><h4>Needs and Services Provided</h4>
<table>
    <tr>
        <td colspan="6"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br /><br />
     </td> 
    </tr>
    <tr>
        <td>Type of Violence </td><td>Assistance and Services Provided</td><td>Referred From </td><td>Referred To </td><td>Comments</td><td>&nbsp</td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="cboTypesofViolence" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
        </td>
        <td>
            <asp:DropDownList ID="cboAssistance" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
        </td>
        <td>
            <asp:DropDownList ID="cboReferredfrom" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
        </td>
        <td>
            <asp:DropDownList ID="cboReferredTo" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
        </td>
        <td><asp:textbox id="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Width="200px"></asp:textbox></td>
        <td>
            <asp:Button runat="server" ID="cmdAddNeed" Text="+" CssClass="btn btn-default"/>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadGrid ID="radBenListing" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="231%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                        AlternatingItemStyle-BackColor="#66ccff">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="NeedID" UniqueName="NeedID" HeaderText="NeedID"
                                Display="false">
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="TypeOfViolence" UniqueName="TypeOfViolence" HeaderText="TypeOfViolence">
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="AssistanceProvided" UniqueName="AssistanceProvided" HeaderText="AssistanceProvided">
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="ReferredFrom" UniqueName="ReferredFrom" HeaderText="ReferredFrom">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ReferredTo" UniqueName="ReferredTo" HeaderText="ReferredTo">
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
            </td></tr>
        </table>
    <table cellpadding="2" style="width:70%;margin-left:2%">
        <tr>

            <td></td>
        </tr>   

    <tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtCBSID" runat="server" CssClass="HiddenControl"></asp:TextBox> <br />
		</td> 
	</tr> 
    <tr>
        <td>&nbsp</td>
        <td>
        <asp:LinkButton ID="lnkFiles" runat="server" Text="File uploads" ></asp:LinkButton></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
