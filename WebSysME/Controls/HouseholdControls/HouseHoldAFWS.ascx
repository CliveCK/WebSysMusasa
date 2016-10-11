<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HouseHoldAFWS.ascx.vb" Inherits="WebSysME.HouseHoldAFWS" %>
<style type="text/css" >
.leftcolumn {
     width: 350px;
     padding: 0;
     padding-left:2%;
     margin: 2%;
     display: block;
     display: block;
     float: left;}


.rightcolumn {
     width: 600px;
     padding-left:32%;
     border: 1px solid white;
}
    </style> 
     <div><asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </div> <br /><br />
<table cellpadding="4" style="margin-left:2%">
    <tr>
        <td valign="top">
    <table cellpadding="4">
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>  
		<td >Ariable Land Size</td> 
        	<td ><asp:textbox id="txtAriableLandSize" runat="server" CssClass="form-control" Width="200px"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Major Source Of Food</td> 
        	<td ><asp:dropdownlist id="cboMajorSourceOfFood" runat="server" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
    </tr>
        <tr>
		<td >Condition Of House</td> 
        	<td ><asp:dropdownlist id="cboConditionOfHouse" runat="server" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Tenure</td> 
        	<td ><asp:dropdownlist id="cboTenure" runat="server" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
    </tr>
    <tr>
		<td >Wealth Rank</td> 
        	<td ><asp:dropdownlist id="cboWealthRank" runat="server" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Source Of Water</td> 
        	<td ><asp:dropdownlist id="cboSourceOfWater" runat="server" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
    </tr>
    <tr>
		<td >Type Of Toilet</td> 
        	<td ><asp:dropdownlist id="cboTypeOfToilet" runat="server" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td >Health Care Provider</td> 
        	<td ><asp:DropDownList id="txtHealthCareProvider" runat="server" CssClass="form-control" Width="200px"></asp:DropDownList> </td> 
    </tr>
    <tr>
		<td >Household No</td> 
        	<td ><asp:textbox id="txtHouseholdNo" runat="server" CssClass="form-control" Width="200px"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td >Room Occupation Ratio</td> 
        	<td ><asp:textbox id="txtRoomOccupationRatio" runat="server" CssClass="form-control" Width="200px"></asp:textbox> </td> 
	    <tr>
            <td><asp:TextBox ID="txtHouseholdDetailID" runat="server" Visible="false" CssClass="form-control" Width="200px"></asp:TextBox></td>
        </tr>
	<tr> 
		<td> 
            		<asp:button id="cmdSaveA" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
     </td> 
	</tr> 
    </table>
</td>
<td valign="top">
<table style="width: 550px">
    <tr> 
		<td colspan="8" class="PageTitle"><h3>Household Asset Details</h3></td> 
	</tr> 
	<tr> 
		<td >Asset Type</td> 
        	<td ><asp:dropdownlist id="cboAssetType" runat="server" CssClass="form-control" Width="250px"></asp:dropdownlist> </td> 
        <td >Asset</td> 
        	<td ><asp:dropdownlist id="cboAsset" runat="server" CssClass="form-control" Width="250px"></asp:dropdownlist> </td> 
		<td >Quantity</td> 
        	<td ><asp:textbox id="txtQuantity" runat="server" CssClass="form-control" Width="250px"></asp:textbox> </td> 
        <td><asp:TextBox ID="txtHouseholdAssetID" runat="server" Visible="false" CssClass="form-control" Width="250px"></asp:TextBox></td>
        <td><asp:Button ID="cmdSaveAsset" runat="server" Text="+" CssClass="btn btn-default"/></td>
	</tr> 
	<tr> 
		<td colspan="8"></td>
	</tr> 
	<tr> 
        <td colspan="8">
		<telerik:RadGrid ID="radAsset" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" >
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" >
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="HouseHoldAssetDetailsID" UniqueName="HouseHoldAssetDetailsID" HeaderText="HouseHoldAssetDetailsID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Type" UniqueName="Type" HeaderText="Type">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Asset" UniqueName="Asset" HeaderText="Asset" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Quantity" UniqueName="Quantity" HeaderText="Quantity">
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
                    <ClientSettings>
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid></td>		
	</tr>
</table>
</td>
</tr>
</table>