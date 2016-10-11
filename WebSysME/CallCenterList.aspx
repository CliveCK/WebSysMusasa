<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CallCenterList.aspx.vb" Inherits="WebSysME.CallCenterList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h4>Call Center</h4>
    <br />
        <asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/> &nbsp;&nbsp;&nbsp;&nbsp
        Choose File:<asp:FileUpload ID="fuUpload" runat="server" /><br />
        <asp:Button runat="server" ID="cmdUpload" Text="Upload From MS Excel Template" CssClass="btn btn-default"/>
    <table style="width:70%;margin-left:2%">
        <tr>
            <td>
                &nbsp</td>
        </tr>
        <tr> 
		<td colspan="8"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr><td colspan="4">
            <table><tr><td>

    <telerik:RadGrid ID="radCallCenter" runat="server" Height="80%" 
                    CellPadding="0" Width="100%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="CallCenterDetailID" UniqueName="CallCenterDetailID" HeaderText="CallCenterDetailID"
                                Display="false">
                            </telerik:GridBoundColumn>
                             
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="View/Edit Details" UniqueName="column"
                                CommandName="View"> 
                             </telerik:GridButtonColumn>                          
                            <telerik:GridBoundColumn DataField="CallDate" UniqueName="column5" HeaderText="Call Date">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="CallCode" UniqueName="CallCode" HeaderText="Caller Number"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="CellNumber" UniqueName="TelNumber" HeaderText="Tel-Number"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="District">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="First Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Surname" HeaderText="Surname" UniqueName="Surname">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NationalIDNum" HeaderText="NationalIDNum" UniqueName="NationalIDNum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DOB"  HeaderText="DateOfBirth" UniqueName="DateOfBirth">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Sex"  HeaderText="Sex" UniqueName="Sex">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Address" HeaderText="Address" UniqueName="Address">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TypeOfIssue" HeaderText="TypeOfIssue" UniqueName="TypeOfIssue">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ReferredFrom" HeaderText="ReferredFrom" UniqueName="ReferredFrom">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ReferredTo" HeaderText="ReferredTo" UniqueName="ReferredTo">
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

               </td></tr></table>
            </td></tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
