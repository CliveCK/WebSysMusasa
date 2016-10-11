<%@ Control Language="vb" AutoEventWireup="false" Codebehind="FindUsers.ascx.vb"
    Inherits="WebSysME.FindUsers" %>
<%@ Register Src="Userpermissions.ascx" TagName="Userpermissions" TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Panel ID="Panel1" runat="server" GroupingText="Search Users" Width="100%">
<table width="100%">
    <tr>
        <td rowspan="1">
            <asp:Panel ID="pnlFindUsers" runat="server" Height="100px" Width="100%">
                <table width="100%">
                    <tr>
                        <td>
                            Username</td>
                        <td>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox></td>
                        <td>
                            Surname</td>
                        <td>
                            <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            Firstname</td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox></td>
                        <td>
                            Email Address</td>
                        <td>
                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="cmdFind" runat="server" Text="Find User(s)" CssClass="btn btn-default" /></td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel><br /><br />
          </td>   
    </tr>
    <tr>
            <td class="PageTitle" style="width: 100%">
               <asp:Label ID="lblCurrentUser" runat="server"></asp:Label></td>
        </tr>
    <tr>
        <td>
            <telerik:RadGrid ID="rdResults" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                AllowMultiRowSelection="True" CellSpacing="0" GridLines="None" >
                <ExportSettings>
                    <Pdf>
                        <PageHeader>
                            <LeftCell Text="" />
                            <MiddleCell Text="" />
                            <RightCell Text="" />
                        </PageHeader>
                        <PageFooter>
                            <LeftCell Text="" />
                            <MiddleCell Text="" />
                            <RightCell Text="" />
                        </PageFooter>
                    </Pdf>
                </ExportSettings>
                <MasterTableView>
                    <EditFormSettings>
                        <EditColumn CancelImageUrl="~/Images/Cancel.gif" EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" UniqueName="EditCommandColumn" UpdateImageUrl="~/Images/Update.gif">
                        </EditColumn>
                    </EditFormSettings>
                    <ExpandCollapseColumn ButtonType="ImageButton" Display="False" UniqueName="ExpandColumn">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                    <RowIndicatorColumn Display="False" UniqueName="RowIndicator">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="UserID" Display="False" HeaderText="UserID" UniqueName="UserID">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Deleted" Display="False" HeaderText="Deleted" UniqueName="Deleted">
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn CommandArgument="UserID" CommandName="Select" DataTextField="Username" HeaderText="Username" UniqueName="Username">
                        </telerik:GridButtonColumn>
                        <telerik:GridBoundColumn DataField="UserFirstname" HeaderText="Firstname" UniqueName="Firstname">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserSurname" HeaderText="Surname" UniqueName="Surname">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="EmailAddress" UniqueName="EmailAddress">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                             <telerik:GridTemplateColumn UniqueName="ActivateUser">
                            <ItemTemplate>
                                <asp:Button ID="btnAct" runat="server" CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Deleted")%>' Text="Activate"
                                    CommandName="ActivateUser" OnClientClick="javascript:return confirm('Are you sure you want to activate user?')"
                                    CssClass="btn btn-default" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                            <telerik:GridTemplateColumn UniqueName="DeactivateUser">
                            <ItemTemplate>
                                <asp:Button ID="btnDeact" runat="server"  CausesValidation="False" Visible="false"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Deleted")%>' Text="Deactivate"
                                    CommandName="DeactivateUser" OnClientClick="javascript:return confirm('Are you sure you want to deactivate user?')"
                                    ToolTip="Click to deactivate " CssClass="btn btn-default"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                    </Columns>
                    <BatchEditingSettings EditType="Cell" />
                    <PagerStyle PageSizeControlType="RadComboBox" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox" />
                <FilterMenu HoverBackColor="LightSteelBlue" HoverBorderColor="Navy" NotSelectedImageUrl="~/Images/NotSelectedMenu.gif"
                    SelectColumnBackColor="Control" SelectedImageUrl="~/Images/SelectedMenu.gif"
                    TextColumnBackColor="Window"></FilterMenu>
            </telerik:RadGrid></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblStatus" runat="server" Height="24px" Width="100%" CssClass="Error"></asp:Label></td>
    </tr>
    <tr>
        <td >
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Button ID="cmdSavePermissions" runat="server" Text="Save Permissions" Visible="False" /></td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblMsg" runat="server" CssClass="Error"></asp:Label></td>
        <td>
        </td>
    </tr>
</table>
</asp:Panel>
