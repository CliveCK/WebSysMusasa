<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ComplementaryListboxesVertical.ascx.vb" Inherits="WebSysME.ComplementaryListboxesVertical" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dw" Assembly="WebSysME" Namespace="WebSysME"%>
<TABLE id="Table4" borderColor="#000000" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD borderColor="#ffffff" colspan="2">
			<P align="left">
				<asp:Label id="lblAvailableOptions" runat="server"></asp:Label></P>
		</TD>
	</TR>
	<TR>
		<TD borderColor="#ffffff" rowSpan="2" width="100%">
			<dw:listboxwithviewstate id="lstAvailableOptions" SelectionMode="Multiple" DataValueField="GroupID" DataTextField="Name"
				Rows="7" Width="100%" runat="server"></dw:listboxwithviewstate></TD>
		<TD borderColor="#ffffff" width="30" align="center">
			<asp:button id="cmdMoveSelected" Width="30px" runat="server" Text="+"></asp:button>
		</TD>
	</TR>
	<TR>
		<TD borderColor="#ffffff" align="center">
			<asp:button id="cmdMoveAll" Width="30px" runat="server" Text="++"></asp:button>
		</TD>
	</TR>
	<TR>
		<TD borderColor="#ffffff" colSpan="2" align="center">
			<P align="left">
				<asp:Label id="lblSelectedOptions" runat="server" style="Z-INDEX: 0"></asp:Label></P>
		</TD>
	</TR>
	<TR>
		<TD borderColor="#ffffff" align="center" rowspan="3">
			<dw:listboxwithviewstate id="lstSelectedOptions" SelectionMode="Multiple" DataValueField="GroupID" DataTextField="Name"
				Rows="7" Width="100%" runat="server">
				<asp:ListItem></asp:ListItem>
			</dw:listboxwithviewstate></TD>
	</TR>
	<TR>
		<TD borderColor="#ffffff" align="center">
			<asp:button id="cmdRemoveSelected" Width="30px" runat="server" Text="-"></asp:button>
		</TD>
	</TR>
	<TR>
		<TD borderColor="#ffffff" align="center">
			<asp:Button id="cmdRemoveAll" Width="30px" runat="server" Text="--"></asp:Button>
		</TD>
	</TR>
</TABLE>
