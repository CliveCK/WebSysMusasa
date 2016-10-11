<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProjectListing.ascx.vb" Inherits="WebSysME.ProjectListing" %>

<script type="text/javascript">
    function RowContextMenu(sender, eventArgs) {
        var menu = $find("<%=radMContextMenu.ClientID %>");
                    var evt = eventArgs.get_domEvent();

                    var index = eventArgs.get_itemIndexHierarchical();
                    document.getElementById("radGridClickedRowIndex").value = index;

                    sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

                    menu.show(evt);

                    evt.cancelBubble = true;
                    evt.returnValue = false;

                    if (evt.stopPropagation) {
                        evt.stopPropagation();
                        evt.preventDefault();
                    }
                }
            </script>
<div style="padding-left:2%">
<table cellpadding="0" cellspacing="0" border="0" style="width:100%">
    <tr>
        <td>
            <h4>Project Listing</h4><br />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblStatus" runat="server" Font-Bold="true" Font-Names="Sagoe UI" BackColor="Yellow"></asp:Label><br /><br />
        </td>
    </tr>
    <tr>
            <td>
                <input type="text" id="radGridClickedRowIndex" name="radGridClickedRowIndex" class="HiddenControl"/>
            <asp:TextBox ID="txtContext" runat="server" CssClass="HiddenControl" Visible="false">0</asp:TextBox>
            <asp:TextBox ID="txtMenuID" runat="server" Visible="false" >0</asp:TextBox>
                <telerik:RadGrid ID="radProjectListing" runat="server" GridLines="None" Height="100%" 
                    AllowPaging="True" AllowFilteringByColumn="True" CellPadding="0" Width="100%">
                    <MasterTableView AutoGenerateColumns="False"  PagerStyle-Mode="NextPrevNumericAndAdvanced" DataKeyNames="Project">
                        <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="FileID" UniqueName="FileID" HeaderText="FileID"
                                Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ProjectCode" UniqueName="ProjectCode" HeaderText="ProjectCode">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Acronym" UniqueName="Acronym" HeaderText="Acronym">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StartDate" UniqueName="StartDate" HeaderText="StartDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FinalEvlDate" UniqueName="FinalEvlDate" HeaderText="FinalEvaluationDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EndDate" UniqueName="EndDate" HeaderText="EndDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ActualBeneficiaries" UniqueName="ActualBeneficiaries" HeaderText="Number Of Beneficiaries">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BenDescription" UniqueName="BenDescription" HeaderText="Beneficiary Description">
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
                    <ClientSettings>
                         <ClientEvents OnRowContextMenu="RowContextMenu"></ClientEvents>
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>                
            <telerik:RadContextMenu ID="radMContextMenu" runat="server" >
            </telerik:RadContextMenu>
                <script type="text/javascript">
                    function OnClientItemClickedHandler(sender, eventArgs) {
                        document.getElementById('<% Response.Write(txtContext.ClientID) %>').value = document.getElementById("radGridClickedRowIndex").value;
                        document.getElementById('<% Response.Write(txtMenuID.ClientID) %>').value = eventArgs.get_item().get_value()
                        alert(eventArgs.Item.Value);
                    }
            </script>
            </td>
        </tr>
</table>
 </div>