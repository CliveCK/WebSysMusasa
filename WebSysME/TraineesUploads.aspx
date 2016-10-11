<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TraineesUploads.aspx.vb" Inherits="WebSysME.TraineesUploads" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:70%;margin-left:2%">
        <tr>
            <td>
                <b>Trainee Uploads Portal</b>
            </td>
        </tr>
        <tr> 
		<td colspan="8"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>Upload File:</td>
            <td>
                <asp:FileUpload runat="server" ID="fUpload" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnUpload" Text="Upload" CssClass="btn btn-default"/>
            </td>
        </tr>
    </table>
  <asp:Panel runat="server" ID="pnlRecords" Visible="false" >
    <table style="width:90%;margin-left:2%">
        <tr>
            <td>&nbsp;</td>
        </tr>
       
        <tr>
            <td>Trainees</td>
        </tr>
        <tr>
            <td colspan="4">
                 <telerik:RadGrid ID="radTraining" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="90%" AllowMultiRowSelection="true" >
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="True" AllowPaging="True" 
                       AllowMultiColumnSorting="true" AllowSorting="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <Columns>                       
                             <telerik:GridBoundColumn DataField="FirstName" UniqueName="FirstName" HeaderText="FirstName"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Surname" UniqueName="Surname" HeaderText="Surname"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Title" UniqueName="Title" HeaderText="Title"  >
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="DOB" UniqueName="DOB" HeaderText="DateOfBirth"  >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Sex" UniqueName="Sex" HeaderText="Sex">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ContactNo" UniqueName="ContactNo" HeaderText="ContactNo">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="IDNumber" UniqueName="IDNumber" HeaderText="IDNumber">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Province" UniqueName="Province" HeaderText="Province">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="District" UniqueName="District" HeaderText="District">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="GroupType" UniqueName="GroupType" HeaderText="GroupType">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Department" UniqueName="Department" HeaderText="Department">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="HealthCenter" UniqueName="HealthCenter" HeaderText="HealthCenter">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RecordStatus" UniqueName="RecordStatus" HeaderText="RecordStatus">
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
                    <ClientSettings EnablePostBackOnRowClick="true" Selecting-AllowRowSelect="true" >
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnProcess" Text="Start Processing" CssClass="btn btn-default"/>
            </td>
        </tr>
    </table>
   </asp:Panel>
</asp:Content>
