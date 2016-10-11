<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HouseholdList.aspx.vb" Inherits="WebSysME.HouseholdList" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table style="margin-left:2%;width:80%">
        <tr>
            <td>
                <asp:Label Text="FilterBy:" Font-Bold="true" ForeColor="#000066" ID="lblFilter" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                District: 
            </td>
            <td>
                <asp:DropDownList ID="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
            </td>
            <td>
                 Ward: 
            </td>
            <td>
               <asp:DropDownList ID="cboWard" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><br />
                <asp:Button runat="server" ID="cmdSearch" Text="Search" CssClass="btn btn-default"/>
            </td>
        </tr>
        <tr>
            <td><br /><asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/></td>
        </tr>
        <tr>
            <td colspan="4">
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
                            <telerik:GridBoundColumn DataField="NationalIDNo" UniqueName="NationalIDNo" HeaderText="NationalIDNo">
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

</asp:Content>