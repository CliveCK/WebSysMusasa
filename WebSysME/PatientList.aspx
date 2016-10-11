<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PatientList.aspx.vb" Inherits="WebSysME.PatientList" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table style="margin-left:2%">
        <tr>
            <td><br /><asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/></td>
        </tr>
        <tr>
            <td>
        <telerik:RadGrid ID="radPatientListing" runat="server" GridLines="None" Height="100%" 
                    AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True"  PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="PatientID" UniqueName="PatientID" HeaderText="PatientID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="PatientNo" UniqueName="PatientNo" HeaderText="KidzCan Number">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DOB" UniqueName="DateOfBirth" HeaderText="DateOfBirth" DataFormatString="{0:dd/MM/yyyy}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PatientStatus" UniqueName="PatientStatus" HeaderText="PatientStatus">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Priority" UniqueName="Priority" HeaderText="Priority">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Diagnosis" UniqueName="Diagnosis" HeaderText="Diagnosis">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Orphanhood" UniqueName="Orphanhood" HeaderText="Orphanhood">
                            </telerik:GridBoundColumn>                            
                            <telerik:GridBoundColumn DataField="ClosestHealthCenter" UniqueName="ClosestHealthCenter" HeaderText="ClosestHealthCenter">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Religion" UniqueName="Religion" HeaderText="Religion">
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

</asp:Content>
