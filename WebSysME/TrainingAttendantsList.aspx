<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TrainingAttendantsList.aspx.vb" Inherits="WebSysME.TrainingAttendantsList" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="margin-left:2%">
    <h4>Training Attendants</h4>
    <br />
        <asp:Button ID="cmdNew" runat="server" Text="Add New"/>
    <telerik:RadGrid ID="radTraining" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="TrainingID" UniqueName="FileID" HeaderText="FileID"
                                Display="false">
                            </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContactNo" UniqueName="ContactNo" HeaderText="ContactNo">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex">
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
                        <PagerStyle Position="Top" AlwaysVisible="true"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid><br />
    </div>
</asp:Content>


