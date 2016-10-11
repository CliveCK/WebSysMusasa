<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrganizationalContacts.aspx.vb" Inherits="WebSysME.OrganizationalContacts" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>Organizational Contacts</h4>
    <br />
        <asp:Button ID="cmdAddNew" runat="server" Text="Add New" CssClass="btn btn-default"/>
    <telerik:RadGrid ID="radContacts" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="90%" AutoGenerateColumns="false" >
                    <MasterTableView  AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" DataKeyNames="OrganizationID"
                        HierarchyLoadMode="Client" >
                        <Columns>                            
                            <telerik:GridBoundColumn DataField="OrganizationID" UniqueName="OrganizationID" HeaderText="OrganizationID" Display="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                             <telerik:GridBoundColumn DataField="OrganizationType" UniqueName="OrganizationType" HeaderText="Organization Type"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContactNo" UniqueName="PhoneNo" HeaderText="PhoneNo">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Address" UniqueName="Address" HeaderText="Address">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EmailAddress" UniqueName="Email" HeaderText="Email">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="WebsiteAddress" UniqueName="Website" HeaderText="Website">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <DetailTables>
                            <telerik:GridTableView Name="dsContacts">
                                <Columns>
                                        <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ContactNo" UniqueName="PhoneNo" HeaderText="PhoneNo">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Fax" UniqueName="Fax" HeaderText="Fax">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Address" UniqueName="Address" HeaderText="Address">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Email" UniqueName="Email" HeaderText="Email">
                                        </telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
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
    </div>
</asp:Content>
