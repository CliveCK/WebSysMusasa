<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProjectView.ascx.vb" Inherits="WebSysME.ProjectView" %>
<%@ Register Src="~/Controls/Scheduler.ascx" TagName="SchedulerControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ProjectMeetingDetailsControl.ascx" TagName="ProjectMeetingControl" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ProcumentDetailsControl.ascx" TagName="ProcumentControl" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/EvaluationDetailsControl.ascx" TagName="EvaluationControl" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/TripsDetailsControl.ascx" TagName="TripDetailsControl" TagPrefix="uc5" %>
<style type="text/css" >
.leftcolumn {
     width: 250px;
     padding: 0;
     margin: 0;
     display: block;
     position: fixed;
}

.rightcolumn {
     width: 750px;
     padding-left:265px;
     display: block;
     float: left;
     border: 1px solid white;
}
    </style>
<table style="margin-left:2%">
    <tr>
        <td><h4 style="font-family:sans-serif">Project View</h4></td>
        <td><asp:TextBox ID="txtProjectID" runat="server" Visible="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblProject" runat="server"  Font-Size="11pt" Font-Bold="true"></asp:Label>
        </td>
    </tr>
</table>
<table cellspacing="4" border="1" style="margin-left:2%"> 
    <tr>
        <td valign="top"> 
    <telerik:RadTreeView runat="Server" ID="radProjectTree" BackColor="#cccccc">
                    <Nodes>
                        <telerik:RadTreeNode runat="server" Text="Project Data Portal" AllowDrag="false"
                            AllowDrop="false" Expanded="true">
                            <Nodes>
                                <telerik:RadTreeNode runat="server" Text="Needs Assessment" AllowDrop="false" Value="Assessement">
                                        </telerik:RadTreeNode>
                                <telerik:RadTreeNode runat="server" Text="Project Contracts" AllowDrag="false" Expanded="true" >
                                    <Nodes>
                                        <telerik:RadTreeNode runat="server" Text="Donor Contracts" AllowDrop="false" Value="DonorContract">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Partner Contracts" AllowDrop="false" Value="PartnerContract">
                                        </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                                <telerik:RadTreeNode runat="server" Text="Project Planning" AllowDrag="false" Expanded ="true">
                                    <Nodes>
                                        <telerik:RadTreeNode runat="server" Text="Project Proposal" AllowDrop="false" Value="ProjectProposal">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Project Goal" AllowDrop="false" Value="Goal">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Intermediate Objectives" AllowDrop="false" Value="Objectives">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Project Baseline" AllowDrop="false" Value="ProjectBaseline">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Project Plans" AllowDrop="false" Value="1999" Expanded="true">
                                            <Nodes>
                                                <telerik:RadTreeNode runat="server" Text="Service Contracts" AllowDrop="false" Value="Service Contracts">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Project Logframe" AllowDrop="false" Value="2999" Expanded="true">
                                                     <Nodes>
                                                        <telerik:RadTreeNode runat="server" Text="Documents" AllowDrop="false" Value="LogFrame Documents">
                                                        </telerik:RadTreeNode>
                                                        <telerik:RadTreeNode runat="server" Text="Objectives" AllowDrop="false" Value="Objectives">
                                                        </telerik:RadTreeNode>
                                                        <telerik:RadTreeNode runat="server" Text="Outcomes" AllowDrop="false" Value="Outcomes">
                                                        </telerik:RadTreeNode>
                                                         <telerik:RadTreeNode runat="server" Text="Outputs" AllowDrop="false" Value="Outputs">
                                                        </telerik:RadTreeNode>
                                                         <telerik:RadTreeNode runat="server" Text="Activities" AllowDrop="false" Value="Activities">
                                                        </telerik:RadTreeNode>
                                                         <telerik:RadTreeNode runat="server" Text="Inputs" AllowDrop="false" Value="Inputs">
                                                        </telerik:RadTreeNode>
                                                    </Nodes>
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="M&E Framework" AllowDrop="false" Value="5999">
                                                    <Nodes>
                                                        <telerik:RadTreeNode runat="server" Text="Documentation" AllowDrop="false" Value="M&E Plan Documents">
                                                        </telerik:RadTreeNode>
                                                        <telerik:RadTreeNode runat="server" Text="Indicators" AllowDrop="false" Value="5999">
                                                            <Nodes>
                                                                <telerik:RadTreeNode runat="server" Text="Inventory" AllowDrop="false" Value="Inventory">
                                                                </telerik:RadTreeNode>
                                                                 <telerik:RadTreeNode runat="server" Text="Targets" AllowDrop="false" Value="Target">
                                                                </telerik:RadTreeNode>
                                                                 <telerik:RadTreeNode runat="server" Text="Progress Tracking" AllowDrop="false" Value="Tracking">
                                                                </telerik:RadTreeNode>
                                                            </Nodes>
                                                        </telerik:RadTreeNode>
                                                    </Nodes>
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Project Schedule" AllowDrop="false" Value="Schedule">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Project Meetings" AllowDrop="false" Value="Project Meetings">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Trips" AllowDrop="false" Value="Project Trips">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Tools" AllowDrop="false" Value="Project Tools">
                                                </telerik:RadTreeNode>
                                            </Nodes>
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Project Staff" AllowDrop="false" Value="5999">
                                            <Nodes>
                                                <telerik:RadTreeNode runat="server" Text="Contacts" AllowDrop="false" Value="Contacts">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Activity Assignments" AllowDrop="false" Value="Assignments">
                                                </telerik:RadTreeNode>
                                            </Nodes>
                                        </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                                <telerik:RadTreeNode runat="server" Text="Project Execution" Expanded="true" AllowDrag="false">
                                    <Nodes>
                                        <telerik:RadTreeNode runat="server" Text="Project Implementation" Expanded="true" AllowDrag="false">
                                            <Nodes>
                                                <telerik:RadTreeNode runat="server" Text="Indicator Tracking" Expanded="true" AllowDrag="false" Value="Indicator Tracking">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Dashboard" Expanded="true" AllowDrag="false" Value="Dashboard">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Progress reports" Expanded="true" AllowDrag="false" Value="Progress Reports">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Monitoring Surveys" Expanded="true" AllowDrag="false">
                                                    <Nodes>
                                                        <telerik:RadTreeNode runat="server" Text="Rapid Assessments" Expanded="true" AllowDrag="false" Value="Rapid Assessments">
                                                        </telerik:RadTreeNode>
                                                         <telerik:RadTreeNode runat="server" Text="Key Informant Interviews" Expanded="true" AllowDrag="false" Value="Key Informant Interviews">
                                                        </telerik:RadTreeNode>
                                                         <telerik:RadTreeNode runat="server" Text="Focus Group Discussions" Expanded="true" AllowDrag="false" Value="Focus Group Discussions">
                                                        </telerik:RadTreeNode>
                                                         <telerik:RadTreeNode runat="server" Text="Case Studies" Expanded="true" AllowDrag="false" Value="Case Studies">
                                                        </telerik:RadTreeNode>
                                                         <telerik:RadTreeNode runat="server" Text="Success Stories" Expanded="true" AllowDrag="false" Value="Success Stories">
                                                        </telerik:RadTreeNode>
                                                         <telerik:RadTreeNode runat="server" Text="Most Significant Change" Expanded="true" AllowDrag="false" Value="Most Significant Change">
                                                        </telerik:RadTreeNode>
                                                    </Nodes>
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Trips" Expanded="true" AllowDrag="false" Value="Project Trips">
                                                </telerik:RadTreeNode>
                                            </Nodes>
                                         </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                                <telerik:RadTreeNode runat="server" Text="Project Stakeholders" Expanded="true" AllowDrag="false">
                                    <Nodes>
                                         <telerik:RadTreeNode runat="server" Text="Donors" Expanded="true" AllowDrag="false" Value="Donors">
                                         </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Partners" Expanded="true" AllowDrag="false" Value="Partners">
                                         </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Beneficiaries" Expanded="true" AllowDrag="false">
                                            <Nodes>
                                                <telerik:RadTreeNode runat="server" Text="Institutions/Organizations" Expanded="true" AllowDrag="false" Value="Organizations">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Schools" Expanded="true" AllowDrag="false" Value="Schools">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Health Centers" Expanded="true" AllowDrag="false" Value="HealthCenters">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Community Level" Expanded="true" AllowDrag="false" Value="Community">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Groups" Expanded="true" AllowDrag="false" Value="Groups">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Households" Expanded="true" AllowDrag="false" Value="HouseHolds">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Individual" Expanded="true" AllowDrag="false" Value="Individuals">
                                                </telerik:RadTreeNode>
                                            </Nodes>
                                         </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                                <telerik:RadTreeNode runat="server" Text="Project Control" Expanded="true" AllowDrag="false">
                                    <Nodes>
                                        <telerik:RadTreeNode runat="server" Text="Procument" Expanded="true" AllowDrag="false" Value="Procument">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Budgets" Expanded="true" AllowDrag="false">
                                            <Nodes>
                                                <telerik:RadTreeNode runat="server" Text="Budget Contracts" Expanded="false" AllowDrag="false" Value="Budget Contracts">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Detailed Budget" Expanded="true" AllowDrag="false" Value="Detailed Budget">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Finacial Reports" Expanded="true" AllowDrag="false" Value="Financial Reports">
                                                </telerik:RadTreeNode>
                                            </Nodes>
                                        </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                                <telerik:RadTreeNode runat="server" Text="Project Close Out" Expanded="true" AllowDrag="false">
                                    <Nodes>
                                        <telerik:RadTreeNode runat="server" Text="Assessments" Expanded="true" AllowDrag="false" Value="Evaluation">
                                            <Nodes>
                                                <telerik:RadTreeNode runat="server" Text="MidTerm Evaluation" Expanded="true" AllowDrag="false" Value="MidTermEvaluation">
                                                </telerik:RadTreeNode>
                                                <telerik:RadTreeNode runat="server" Text="Final Evaluation" Expanded="true" AllowDrag="false" Value="FinalEvaluation">
                                                </telerik:RadTreeNode>
                                            </Nodes>
                                        </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                                <telerik:RadTreeNode runat="server" Text="Project Media" Expanded="false" AllowDrag="false">
                                    <Nodes>
                                        <telerik:RadTreeNode runat="server" Text="Files Repository" Expanded="true" AllowDrag="false" >
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Pictures" Expanded="true" AllowDrag="false" Value="Pictures">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Audio" Expanded="true" AllowDrag="false" Value="Audio">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Video" Expanded="true" AllowDrag="false" Value="Video">
                                        </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                                <telerik:RadTreeNode runat="server" Text="GIS" Expanded="true" AllowDrag="false">
                                    <Nodes>
                                        <telerik:RadTreeNode runat="server" Text="Outreach Mapping" Expanded="true" AllowDrag="false">
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode runat="server" Text="Outputs Mapping" Expanded="true" AllowDrag="false">
                                        </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                            </Nodes>
                        </telerik:RadTreeNode>
                    </Nodes>
                </telerik:RadTreeView>
            </td>
            <td valign="top" style="margin-left:2%">
                 <telerik:RadGrid ID="radFileListing" runat="server" GridLines="None" Height="100%" 
                     AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False" AllowPaging="True" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="FileID" UniqueName="FileID" HeaderText="FileID"
                                Display="false">                            
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="FilePath" UniqueName="FilePath" HeaderText="FilePath" Display="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="Edit Details" UniqueName="column"
                                CommandName="View">
                            </telerik:GridButtonColumn>                            
                            <telerik:GridBoundColumn DataField="Title" UniqueName="Title" HeaderText="Title">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FileType" UniqueName="FileType" HeaderText="FileType">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Author" UniqueName="Author" HeaderText="Author">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AuthorOrganization" UniqueName="AuthorOrganization" HeaderText="AuthorOrganization">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Date" UniqueName="DateUploaded" HeaderText="DateUploaded">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="PushButton" Text="Download" UniqueName="column"
                                CommandName="Download" ButtonCssClass="btn btn-default">
                            </telerik:GridButtonColumn>
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
                    <ClientSettings >
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
                <telerik:RadGrid ID="radListing" runat="server" GridLines="None" Height="100%" 
                     AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="True" AllowPaging="True" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
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
                    <ClientSettings >
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
                <table style="width:1300px"><tr><td>                    
               <uc1:SchedulerControl ID="ucSchedulerControl" runat="server"  Visible="false" />
                <uc2:ProjectMeetingControl ID="ucProjectMeetingControl" runat="server"  Visible="false"/>
                <uc3:ProcumentControl ID="ucProcumentControl" runat="server"  Visible="false"/>
                <uc4:EvaluationControl ID="ucEvaluationControl" runat="server"  Visible="false"/>
                <uc5:TripDetailsControl ID="ucTripDetailsControl" runat="server"  Visible="false"/>
                           </td></tr></table>
            </td>
        </tr>
    </table>
