<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CBSMemberReporting.aspx.vb" Inherits="WebSysME.CBSMemberReporting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left:2%">

<table cellpadding="3" style="width:100%;margin-left:2%">
    <tr>
        <td><h4>CBS Member Reporting</h4><br /></td>
    </tr>
     <tr>
        <td colspan="5"> 
            		<asp:Panel id="pnlError" width="95%" runat="server" EnableViewState="False"><asp:label id="lblError" Width="100%" runat="server" CssClass="Error" EnableViewState="False"></asp:label></asp:Panel> <br /><br />
     </td> 
    </tr>
      <tr> 
		<td class="auto-style5" >Province</td> 
        	<td class="auto-style1" ><asp:dropdownlist id="cboProvince" runat="server" AutoPostBack="true" CssClass="form-control" Width="200px"></asp:dropdownlist> </td> 
          <td>&nbsp</td>
          <td>&nbsp</td>
	</tr> 
    <tr> 
		<td class="auto-style6" >District</td> 
        	<td class="auto-style1" ><asp:dropdownlist id="cboDistrict" runat="server" AutoPostBack="true" CssClass="form-control"></asp:dropdownlist> </td> 
         <td>&nbsp</td>
          <td>&nbsp</td>
	</tr> 
	<tr> 
		<td class="auto-style6" >Ward</td> 
        	<td class="auto-style1" ><asp:dropdownlist id="cboWard" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
         <td>&nbsp</td>
          <td>&nbsp</td>
	</tr> 
    <tr> 
		<td class="auto-style6" >Club</td> 
        	<td class="auto-style1" ><asp:dropdownlist id="cboClub" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
         <td>&nbsp</td>
          <td>&nbsp</td>
	</tr> 
    <tr>
         <td>Year</td> <td><asp:textbox id="txtYear" runat="server" CssClass="form-control" TextMode="Number"></asp:textbox></td>
    </tr>
    <tr>    
       
		<td class="auto-style6" >Month</td> 
        <td class="auto-style1" ><asp:dropdownlist id="cboMonth" runat="server" CssClass="form-control"></asp:dropdownlist> </td> 
	</tr>

    <tr><td>Challenges</td><td><asp:textbox id="txtChallenges" runat="server" CssClass="form-control" Height="71px" TextMode="MultiLine" Width="767px"></asp:textbox></td></tr>
    <tr><td>Recommendations</td><td><asp:textbox id="txtRecommendations" runat="server" CssClass="form-control" Height="71px" TextMode="MultiLine" Width="767px"></asp:textbox></td></tr>
    <tr><td colspan="2">&nbsp</td></tr>
    <tr>  <td>&nbsp</td>
		<td> 
            		<asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-default"></asp:button> 
                    <asp:button id="cmdClear" runat="server" Text="Clear" CssClass="btn btn-default"></asp:button>
                    <asp:button id="cmdDelete" runat="server" Text="Delete" CssClass="btn btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this CBS Entry?')"></asp:button>

     </td>
       
	</tr>

    <tr> 
		<td colspan="4"> 
			<asp:TextBox id="txtCBSMemberReportingID" runat="server" CssClass="HiddenControl"></asp:TextBox> <br />
		</td> 
	</tr> 
    <tr>
        <td>&nbsp</td>
        <td>
        <asp:LinkButton ID="lnkCBSMembers" runat="server" Text="CBS Members"></asp:LinkButton> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|
        <asp:LinkButton ID="lnkInputs" runat="server" Text="Inputs" ></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|
        <asp:LinkButton ID="lnkFiles" runat="server" Text="File uploads" ></asp:LinkButton></td>
    </tr>
</table>


</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
