<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CommunityListing.aspx.vb" Inherits="WebSysME.CommunityListing" Title="Community Listing"
    MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="margin-left:2%;width:80%">
        <tr>
            <td colspan="4">
                <h4>Communities</h4><br />
            </td> 
        </tr>
        <tr> 
		<td colspan="4">
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br />
     </td> 
	</tr> 
        <tr>
            <td>District</td>
            <td><asp:DropDownList ID="cboDistrict" runat="server" CssClass="form-control"></asp:DropDownList></td>
             <td>Ward</td>
            <td><asp:DropDownList ID="cboWard" runat="server" CssClass="form-control"></asp:DropDownList></td><br />
        </tr>
        <tr>
            <td colspan="4" style="text-align:right">
                <asp:Button runat="server" ID="cmdSearch" Text="Search" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="4" >
                <asp:Button ID="cmNew" runat="server" Text="Add New"  CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <telerik:RadGrid ID="radCommunityListing" runat="server" GridLines="None" Height="100%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <PagerStyle AlwaysVisible="true" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="CommunityID" UniqueName="CommunityID" HeaderText="CommunityID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NoOfHouseholds" UniqueName="NoOfHouseholds" HeaderText="NoOfHouseholds">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NoOfIndividualAdultMales" UniqueName="NoOfIndividualAdultMales" HeaderText="NoOfIndividualAdultMales">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NoOfIndividualAdultFemales" UniqueName="NoOfIndividualAdultFemales" HeaderText="NoOfIndividualAdultFemales">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NoOfMaleYouths" UniqueName="NoOfMaleYouths" HeaderText="NoOfMaleYouths">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="NoOfFemaleYouth" UniqueName="NoOfFemaleYouth" HeaderText="NoOfFemaleYouth">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NoOfChildren" UniqueName="NoOfChildren" HeaderText="NoOfChildren">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Cancel" CausesValidation="False"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CommunityID")%>'
                                    CommandName="Delete" ImageUrl="~/images/Delete.png" OnClientClick="javascript:return confirm('Are you sure you want to delete?')"
                                    ToolTip="Click to delete " />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
                        <PagerStyle Position="TopAndBottom" />
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
</asp:Content>