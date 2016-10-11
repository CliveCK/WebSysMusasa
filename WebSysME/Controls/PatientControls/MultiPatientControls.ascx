<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MultiPatientControls.ascx.vb" Inherits="WebSysME.PatientControls" %>

<%@ Register src="~/Controls/PatientControls/PatientPlanDetailsControl.ascx" tagname="PatientPlanDetails" tagprefix="uc1" %>
<%@ Register src="~/Controls/PatientControls/PrognosisDetailsControl.ascx" tagname="PrognosisDetails" tagprefix="uc1" %>
<%@ Register src="~/Controls/PatientControls/DischargeSummaryDetailsControl.ascx" tagname="DischargeSummary" tagprefix="uc1" %>

  <uc1:PatientPlanDetails runat="server" ID="ucPatientPlanDetails"></uc1:PatientPlanDetails>
    
   <hr /><br />
    <uc1:PrognosisDetails runat="server" ID="ucPrognosisDetails"></uc1:PrognosisDetails>
 <hr /><br />
        <uc1:DischargeSummary runat="server" ID="ucDischargeSummary"></uc1:DischargeSummary>