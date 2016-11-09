<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExcelUploadConsole.aspx.vb" Inherits="WebSysME.ExcelUploadConsole" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <table style="width:70%;margin-left:2%">
        <tr>
            <td>
                <b>Excel Uploads Portal</b>
            </td>
        </tr>
        <tr> 
		<td colspan="2"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="90%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> 
     </td> 
	</tr> 
        <tr>
            <td>Please specify the type of details to Upload</td>
        </tr>
        <tr>
            <td><asp:RadioButtonList ID="rdoType" runat="server" >
                <asp:ListItem Text="Reception" Value="Reception" Selected="true"></asp:ListItem>
                <asp:ListItem Text="Counsellor" Value="Counsellor"></asp:ListItem>
                <asp:ListItem Text="Lawyer" Value="Lawyer"></asp:ListItem>
                <asp:ListItem Text="Shelter" Value="Shelter"></asp:ListItem>
            </asp:RadioButtonList></td>
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
            <td>Records</td>
        </tr>
        <tr>
            <td colspan="4">
                 <telerik:RadGrid ID="radRecords" runat="server" GridLines="None" Height="80%" 
                    CellPadding="0" Width="90%" AllowMultiRowSelection="true" >
                    <MasterTableView AutoGenerateColumns="True" AllowFilteringByColumn="False" AllowPaging="True" PagerStyle-Mode="NextPrevNumericAndAdvanced">
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
                        <PagerStyle Position="Top" AlwaysVisible="true"/>
                    </MasterTableView>
                    <ClientSettings EnablePostBackOnRowClick="false" Selecting-AllowRowSelect="true" >
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
