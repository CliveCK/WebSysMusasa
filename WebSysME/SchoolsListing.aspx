<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SchoolsListing.aspx.vb" Inherits="WebSysME.SchoolsListing" MasterPageFile="~/Site.Master"%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>Schools</h4>
        <asp:Button ID="cmdNew" runat="server" CssClass="btn btn-default" Text="AddNew" />
    <br />
    <telerik:RadGrid ID="radSchoolListing" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="SchoolID" UniqueName="SchoolID" HeaderText="SchoolID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="SchoolType" UniqueName="SchoolType" HeaderText="SchoolType">
                            </telerik:GridBoundColumn> 
                             <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StaffCompliment" UniqueName="StaffCompliment" HeaderText="StaffCompliment">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NoOfMaleStudents" UniqueName="NoOfMaleStudents" HeaderText="NoOfMaleStudents">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NoOfFemaleStudents" UniqueName="NoOfFemaleStudents" HeaderText="NoOfFemaleStudents">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TotalEnrollment" UniqueName="TotalEnrollment" HeaderText="TotalEnrollment">
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
    </div>
</asp:Content>

