<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OurOrganization.aspx.vb" Inherits="WebSysME.OurOrganization" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/OrganizationDetailsControl.ascx" TagName="OrganizationControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <uc1:OrganizationControl ID="ucOrganizationControl" runat="server"  /><br />
    <div style="margin-left:2%">
        <h4>Sub Offices</h4><br />
    <telerik:RadGrid ID="radAddress" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="SubOfficeID" UniqueName="SubOfficeID" HeaderText="SubOfficeID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Name" UniqueName="OfficeName" HeaderText="Office Name"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContactNo" UniqueName="Phone" HeaderText="Phone">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Fax" UniqueName="Fax" HeaderText="Fax">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Email" UniqueName="Email" HeaderText="Email">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PhysicalAddress" UniqueName="Address" HeaderText="Address">
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
                        <PagerStyle Position="Top" AlwaysVisible="true"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid><br />
        <hr />
        <h4>Contacts</h4>
        <br />
         <telerik:RadGrid ID="radStaff" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="StaffID" UniqueName="StaffID" HeaderText="StaffID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="LastName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Position" UniqueName="Position" HeaderText="Functional Title">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContactNo" UniqueName="Address" HeaderText="Phone">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EmailAddress" UniqueName="Email" HeaderText="Email">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Office" UniqueName="Office" HeaderText="Office">
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
                        <PagerStyle Position="Top" AlwaysVisible="true"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>


        </div> 
</asp:Content>