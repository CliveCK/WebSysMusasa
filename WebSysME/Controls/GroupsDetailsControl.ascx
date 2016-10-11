<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GroupsDetailsControl.ascx.vb" Inherits="WebSysME.GroupsDetailsControl" %>
<table cellpadding="3" cellspacing="0" border="0" style="width:90%;margin-left:2%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Group Details</h4><br /></td> 
	</tr> 
    <tr> 
		<td >Province</td> 
        	<td ><asp:dropdownlist id="cboProvince" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td> 
	</tr> <tr> 
		<td >District</td> 
        	<td ><asp:dropdownlist id="cboDistrict" runat="server" CssClass="form-control" AutoPostBack="true"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Ward</td> 
        	<td ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Group Type</td> 
        	<td ><asp:dropdownlist id="cboGroupType" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td >Description</td> 
        	<td ><asp:textbox id="txtDescription" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Group Size</td> 
        	<td ><asp:textbox id="txtGroupSize" runat="server" CssClass="form-control" onkeypress="return onlyNumbers(event);"></asp:textbox> </td> 
		<td >Group Name</td> 
        	<td ><asp:textbox id="txtGroupName" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
    <tr>
        <td>Males:</td>
            <td ><asp:textbox id="txtMales" runat="server" CssClass="form-control" TextMode="Number" onkeypress="return onlyNumbers(event);"></asp:textbox> </td> 
		<td >Females</td> 
        	<td ><asp:textbox id="txtFemales" runat="server" CssClass="form-control" TextMode="Number" onkeypress="return onlyNumbers(event);"></asp:textbox> </td> 
    </tr>
	<tr> 
		<td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
	<tr> 
		<td colspan="2"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                         <asp:button id="cmdDelete" runat="server" Text="Delete" CssClass="btn btn-default"></asp:button>
        </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
			<asp:TextBox  id="txtGroupID" runat="server" Visible="false"></asp:TextBox> 
		</td> 
	</tr>
    <tr>
        <td><br />Group Membership</td>
    </tr> 
    <tr>
        <td colspan="5">
            <telerik:RadGrid ID="radGroupMembership" runat="server" GridLines="None" Height="100%" AllowMultiRowSelection="True"
                      CellPadding="0" Width="90%">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    <MasterTableView AllowFilteringByColumn="True" AllowMultiColumnSorting="True" AllowPaging="True"
                    AllowSorting="True" CommandItemDisplay="Top" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="5">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="BeneficiaryID" UniqueName="BeneficiaryID" HeaderText="BeneficiaryID"
                                Display="false">                            
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
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
      <tr>
        <td><br />Group Maturity Index</td>
    </tr>
    <tr>
        <td>Month<asp:DropDownList ID="cboMonth" runat="server" CssClass="form-control"></asp:DropDownList></td>
        <td>Year<asp:DropDownList ID="cboYear" runat="server" CssClass="form-control"></asp:DropDownList></td>
        <td>Maturity Area<asp:DropDownList ID="cboKeyArea" runat="server" CssClass="form-control"></asp:DropDownList></td>
        <td>Score<asp:TextBox ID="txtScore" runat="server" CssClass="form-control"></asp:TextBox>        
        </td>
        <td style="text-align:left;padding:0"><asp:Button ID="cmdPlus" runat="server" Text="+" CssClass="btn btn-default" /></td>
    </tr>
    <tr>
        <td colspan="5">
             <telerik:RadGrid ID="radMaturityIndexListing" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="90%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" PageSize="5"
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="GroupMaturityIndexID" UniqueName="Year" HeaderText="Year"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Month" UniqueName="Month" HeaderText="Month"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Year" UniqueName="Year1" HeaderText="Year"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MaturityArea" UniqueName="MaturityArea" HeaderText="MaturityArea">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Score" UniqueName="Score" HeaderText="Score">
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
                        <PagerStyle Position="Bottom"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="false">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid><br />
        </td>
    </tr>
    <tr>
       <td>
           <asp:LinkButton ID="lnkGroupMembership" runat="server" Text="Add Group Members >>"></asp:LinkButton>
       </td>
    </tr>
</table> 
<script type="text/javascript">
   $(document).ready(function () {
       $('#<%=txtMales.ClientID%>').on('input', function (e) {
           if ($('#<%=txtGroupSize.ClientID%>').val() > 0) {
               var Diff = $('#<%=txtGroupSize.ClientID%>').val() - $(this).val();
               $('#<%=txtFemales.ClientID%>').val(Diff);
           }
           else {
               $(this).val(0);
           }
       });

       $('#<%=txtFemales.ClientID%>').on('input', function (e) {
           if ($('#<%=txtGroupSize.ClientID%>').val() > 0) {
               var Diff = $('#<%=txtGroupSize.ClientID%>').val() - $(this).val();
               $('#<%=txtMales.ClientID%>').val(Diff)
           }
           else {
               $(this).val(0);
           }
       });

   });

    function onlyNumbers(e) {
        if (!e) {
            e = window.event;
        }

        var charCode = (e.which) ? e.which : window.event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    }
</script>
