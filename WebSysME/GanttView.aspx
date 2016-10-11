<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GanttView.aspx.vb" Inherits="WebSysME.GanttView" MasterPageFile="~/Site.Master"%>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2" runat="server" >
    <table>
        <tr>
            <td>
                <telerik:RadGantt runat="server" ID="RadGantt1" DataSourceID="TasksDataSource" Height="600px" ListWidth="350px"
             SelectedView="WeekView" Skin="Silk" AutoGenerateColumns="false">
            <Columns>
                <telerik:GanttBoundColumn DataField="Title" DataType="String" Width="120px"></telerik:GanttBoundColumn>
                <telerik:GanttBoundColumn DataField="Start" DataType="DateTime" DataFormatString="dd/MM/yy" Width="40px"></telerik:GanttBoundColumn>
                <telerik:GanttBoundColumn DataField="End" DataType="DateTime" DataFormatString="dd/MM/yy" Width="40px"></telerik:GanttBoundColumn>
            </Columns>
 
            <DataBindings>
                <TasksDataBindings
                    IdField="ID" ParentIdField="ID"
                    StartField="Start" EndField="End"
                    OrderIdField="UserID"
                    SummaryField="Description"
                    TitleField="Subject" PercentCompleteField="PercentComplete" />
            </DataBindings>
        </telerik:RadGantt> 
    <asp:SqlDataSource  ID="TasksDataSource" runat="server" ProviderName="System.Data.SqlClient" 
        ConnectionString="<%$ ConnectionStrings:Demo %>"
        SelectCommand="SELECT [ID],[Subject],[Start],[End],[UserID],[Description],[Reminder],[LastModified],[Completed],[ActivityID] FROM [Appointments]"
        UpdateCommand="UPDATE [PercentComplete] = @PercentComplete WHERE [ID] = @ID">
        <UpdateParameters>
            <asp:Parameter Name="PercentComplete" Type="Decimal" />
            <asp:Parameter Name="ID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
