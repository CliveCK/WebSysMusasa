<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StrategicPlanMilestones.aspx.vb" Inherits="WebSysME.StrategicPlanMilestones" MasterPageFile="~/Site.Master"%>

<%@ Register Src="~/Controls/StrategicPlanDetailsDetailsControl.ascx" TagName="MilestonesControl" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <uc1:MilestonesControl ID="ucMilestonesControl" runat="server"  />
</asp:Content>