<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OutreachNumbers.aspx.vb" Inherits="WebSysME.OutreachNumbers" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>Outreach In Numbers</h4>
    <br />
     <asp:Button ID="cmdNew" runat="server" Text="New" CssClass="btn btn-default"/>
    <telerik:RadGrid ID="radOutreach" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="OutreachID" UniqueName="OutreachID" HeaderText="OutreachID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridButtonColumn Display="True" HeaderText="" Text="View" UniqueName="Procument"
                                    CommandName="View">
                               </telerik:GridButtonColumn> 
                             <telerik:GridBoundColumn DataField="Project" UniqueName="Project" HeaderText="Project"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Partner" UniqueName="Partner" HeaderText="Partner"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="District">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BeneficiaryType" UniqueName="TypeOfBeneficiaries" HeaderText="TypeOfBeneficiaries">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Total" UniqueName="Total" HeaderText="Total">
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


