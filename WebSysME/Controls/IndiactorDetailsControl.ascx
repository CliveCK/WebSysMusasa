<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="IndiactorDetailsControl.ascx.vb" Inherits="WebSysME.IndiactorDetailsControl" %>

<style type="text/css">
    .auto-style1 {
        width: 73px;
    }
    .auto-style2 {
        width: 225px;
    }
    .auto-style3 {
        width: 458px;
    }
    .auto-style4 {
        width: 239px;
    }
 </style>
<div style="padding-left:2%">
<table cellpadding="4" cellspacing="0" border="0" style="width:100%"> 
	<tr> 
		<td colspan="4" class="PageTitle"><h4>Indicator Details</h4><br /></td> 
	</tr> 
    <tr>
        <td colspan="4"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False">
                        <asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label><br />
            		</asp:Panel></td>
    </tr>
	<tr> 
		<td class="auto-style2" >Indicator Type</td> 
        	<td class="auto-style3" ><asp:DropDownList id="cboIndicatorType" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList> </td> 
	</tr>
    <tr style="visibility:collapse ">
        <td class="auto-style2" >Impact</td> 
        	<td class="auto-style3" ><asp:dropdownlist id="cboImpact" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
    </tr>
    <tr>
        <td class="auto-style2" >Objective</td> 
        	<td class="auto-style3" ><asp:dropdownlist id="cboObjective" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
    </tr>
	<tr>
		<td class="auto-style2" >Output</td> 
        	<td class="auto-style3" ><asp:dropdownlist id="cboOutput" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
		<td class="auto-style4" >Outcome</td> 
        	<td ><asp:dropdownlist id="cboOutcome" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Activity</td> 
        	<td class="auto-style3" ><asp:dropdownlist id="cboActivity" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
		<td class="auto-style4" >Unit Of Measurement</td> 
        	<td ><asp:DropDownList id="cboUnitOfMeasurement" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Baseline Value</td> 
        	<td class="auto-style3" ><asp:textbox id="txtBaselineValue" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td class="auto-style4" >Data Source</td> 
        	<td ><asp:DropDownList id="cboDataSource" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Tool</td> 
        	<td class="auto-style3" ><asp:DropDownList id="cboTool" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
		<td class="auto-style4" >Data Collection Frequency</td> 
        	<td ><asp:DropDownList id="cboDataCollectionFrequency" runat="server" CssClass="form-control"></asp:DropDownList> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Name</td> 
        	<td class="auto-style3" ><asp:textbox id="txtName" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td class="auto-style4" >Definition</td> 
        	<td ><asp:textbox id="txtDefinition" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Description</td> 
        	<td class="auto-style3" ><asp:textbox id="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" Columns="20"></asp:textbox> </td> 
		<td class="auto-style4" >Data Collection Method</td> 
        	<td ><asp:textbox id="txtDataCollectionMethod" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
	<tr> 
		<td class="auto-style2" >Responsible Party</td> 
        	<td class="auto-style3" ><asp:textbox id="txtResponsibleParty" runat="server" CssClass="form-control"></asp:textbox> </td> 
		<td class="auto-style4" >Program Target Value</td> 
        	<td ><asp:textbox id="txtProgramTargetValue" runat="server" CssClass="form-control"></asp:textbox> </td> 
	</tr> 
    <tr>
        <td colspan="4">
            &nbsp;
        </td>
    </tr>
	<tr>
        <td colspan="4"></td>
	</tr> 
	<tr> 
		<td colspan="4"> 
            		<asp:button id="cmdSave" runat="server" Text="Save" class="btn btn-default" />
                    <asp:button id="cmdClear" runat="server" Text="New" class="btn btn-default" />
     </td> 
	</tr> 
	<tr> 
		<td colspan="4"> 
            <asp:TextBox id="txtIndicatorID" runat="server" CssClass="HiddenControl"></asp:TextBox>
		</td> 
	</tr> 
</table>
<asp:Panel ID="pnlIndicatorTracking" runat="server" Visible="false" >
    <hr />
<table>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <h4 style="width: 776px">Indicator Tracking</h4>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>Organization:</td>
        <td><asp:DropDownList runat="server" ID="cboOrganization" CssClass="form-control"></asp:DropDownList></td>
        <td>District:</td>
        <td><asp:DropDownList runat="server" ID="cboDistrict" CssClass="form-control"></asp:DropDownList></td>
        <td>Month:</td>
        <td><asp:DropDownList runat="server" ID="cboMonth" CssClass="form-control"></asp:DropDownList></td>
        <td>Year:</td>
        <td><asp:TextBox runat="server" ID="txtYear" CssClass="form-control"></asp:TextBox></td>        
        <td>Target:</td>
        <td><asp:TextBox runat="server" ID="txtTarget" CssClass="form-control"></asp:TextBox></td>
        <td class="auto-style1"><asp:Button runat="server" ID="cmdSaveTracking" Text="+" class="btn btn-default" Width="38px"/></td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="11">
             <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" CssClass="div-container no-bg" LoadingPanelID="RadAjaxLoadingPanel1">
 
        <telerik:RadGrid ID="radIndicatorTargets" runat="server" AutoGenerateColumns="False"  AllowSorting="true" Width="150%">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <MasterTableView EditMode="InPlace" DataKeyNames="IndicatorTrackingID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Organization" SortExpression="Organization">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Organization")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="OrganizationTextBox" runat="server" Width="100px" Text='<%# Bind("Organization")%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="District" SortExpression="District">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "District")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="DistrictTextBox" runat="server" Width="100px" Text='<%# Bind("District")%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Year" SortExpression="Year">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Year")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="yearTextBox" runat="server" Width="100px" Text='<%# Bind("Year")%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Month" SortExpression="Month">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Month")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="monthTextBox" runat="server" Width="100px" Text='<%# Bind("Month")%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Target" SortExpression="Target">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Target")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <telerik:RadNumericTextBox Width="60px" ID="targetTextBox" runat="server" MinValue="0"
                                DbValue='<%# Bind("Target")%>' Type="Number">
                            </telerik:RadNumericTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Achievement" SortExpression="Achievement">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Achievement")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadNumericTextBox Width="60px" ID="achieveNum" runat="server" MinValue="0"
                                DbValue='<%# Bind("Achievement")%>' Type="Number">
                            </telerik:RadNumericTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Comment" SortExpression="Comment">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Comments")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <telerik:RadTextBox ID="commentsTextBox" runat="server" Width="100px" Text='<%# Bind("Comments")%>' TextMode="MultiLine" Rows="3" Columns="40">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UpdateText="Update" CancelText="Cancel"
                        EditText="Edit">
                    </telerik:GridEditCommandColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
        </td>
    </tr>
</table>
</asp:Panel></div>
