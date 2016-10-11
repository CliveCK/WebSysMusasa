<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CBSMembers.aspx.vb" Inherits="WebSysME.CBSMembers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <div style="margin-left:2%">
    <h4>CBS Details</h4>  &nbsp;&nbsp;&nbsp;&nbsp; <asp:LinkButton ID="lnkCBSReporting" runat="server" Text="Back to CBS Reporting Details"></asp:LinkButton>
    <br />

<table style="width:40%">
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr> 
		<td >District</td> <td><asp:textbox id="txtDistrict" runat="server" CssClass="form-control" Enabled="false"></asp:textbox></td>
	</tr> 
	<tr> 
		<td >Ward</td> <td><asp:textbox id="txtWard" runat="server" CssClass="form-control" Enabled="false"></asp:textbox></td>
	</tr> 
    <tr> 
		<td >Club</td> <td><asp:textbox id="txtClub" runat="server" CssClass="form-control" Enabled="false"></asp:textbox></td>
	</tr> 
    <tr>
         <td >Year</td> <td><asp:textbox id="txtYear" runat="server" CssClass="form-control" Enabled="false"></asp:textbox></td>
    </tr>
    <tr>    
   	<td >Month</td>  <td><asp:textbox id="txtMonth" runat="server" CssClass="form-control" Enabled="false"></asp:textbox></td>
	</tr>
</table>
       <br />
       <h4>CBS Members</h4>
       <hr />
       <asp:Button runat="server" ID="cmdNew" Text="Add New" CssClass="btn btn-default"/>

    <telerik:RadGrid ID="radCBS" runat="server" Height="80%" 
                    CellPadding="0" Width="90%" AutoGenerateColumns="False">
                    <MasterTableView AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>
                            <telerik:GridBoundColumn DataField="BeneficiaryID" UniqueName="BeneficiaryID" HeaderText="BeneficiaryID"
                                Display="false">
                            </telerik:GridBoundColumn>   
                             <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>                         
                             <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName"  >

                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname">

                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NationlIDNo" UniqueName="NationalIDNum" HeaderText="NationalIDNum">

                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="DateOfBirth" UniqueName="DOB" HeaderText="DOB"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Problem" UniqueName="Problem" HeaderText="Problem"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AssistanceProvided" UniqueName="AssistanceProvided" HeaderText="AssistanceProvided"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ReferredTo" UniqueName="ReferredTo" HeaderText="ReferredTo"  >
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
