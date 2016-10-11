<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PatientDetailsPage.aspx.vb" Inherits="WebSysME.PatientDetailsPage" MasterPageFile="~/Site.Master"%>
  <%@ Register src="~/Controls/PatientControls/MultiPatientControls.ascx" tagname="PatientPlanDetails" tagprefix="uc1" %>
<%@ Register src="~/Controls/PatientControls/PatientsDetailsControl.ascx" tagname="PatientDetails" tagprefix="uc2" %>
<%@ Register src="~/Controls/PatientControls/GenogramControl.ascx" tagname="Genogram" tagprefix="uc4" %>
<%@ Register src="~/Controls/PatientControls/PatientAddress.ascx" tagname="Address" tagprefix="uc4" %>
<%@ Register src="~/Controls/PatientControls/ReferralsDetailsControl.ascx" tagname="Referrals" tagprefix="uc4" %>
<%@ Register src="~/Controls/PatientControls/CaregiverDetailsControl.ascx" tagname="Caregiver" tagprefix="uc4" %>
<%@ Register src="~/Controls/PatientControls/FollowupsDetailsControl.ascx" tagname="Followups" tagprefix="uc4" %>
<%@ Register src="~/Controls/PatientControls/EligibilityDetailsControl.ascx" tagname="Eligibility" tagprefix="uc4" %>
<%@ Register src="~/Controls/PatientControls/FirstPointOfContactDetailsControl.ascx" tagname="FirstPointOfContact" tagprefix="uc4" %>
<%@ Register src="~/Controls/PatientControls/PatientDocumentsDetailsControl.ascx" tagname="PatientDocuments" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
<div style="margin-left:2%">
    <telerik:RadTabStrip ID="radTab" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Align="Justify" Width="90%">
                <Tabs>
                    <telerik:RadTab Text="Patient Details" Selected="true" >
                    </telerik:RadTab>
                    <telerik:RadTab Text="Address">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Genogram" >
                    </telerik:RadTab>
                    <telerik:RadTab Text="Referrals">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Caregiver">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Followups">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Eligibility">
                    </telerik:RadTab>
                    <telerik:RadTab Text="First Point Of Contact">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Patient Management - Plan">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Documents">
                    </telerik:RadTab>
                </Tabs>
   </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0"
                Width="100%">
                <telerik:RadPageView runat="server" ID="RadPageView1" > 
                    <uc2:PatientDetails runat="server" ID="ucPatientDetails"></uc2:PatientDetails>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView2" > 
                    <uc4:Address runat="server" ID="ucAddress"></uc4:Address>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView3" >                    
                    <uc4:Genogram ID="ucGenogram" runat="server" ></uc4:Genogram>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView4" >                    
                    <uc4:Referrals ID="ucReferrals" runat="server" ></uc4:Referrals>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView5" >                    
                    <uc4:Caregiver ID="ucCaregiver" runat="server" ></uc4:Caregiver>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView6" >                    
                    <uc4:Followups ID="ucFollowups" runat="server" ></uc4:Followups>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView7" >                    
                    <uc4:Eligibility ID="ucEligibility" runat="server" ></uc4:Eligibility>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView8" >                    
                    <uc4:FirstPointOfContact ID="ucFirstPointOfContact" runat="server" ></uc4:FirstPointOfContact>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView9" > 
                    <uc1:PatientPlanDetails runat="server" ID="ucPatientPlanDetails"></uc1:PatientPlanDetails>
                </telerik:RadPageView> 
                <telerik:RadPageView runat="server" ID="RadPageView10" > 
                    <uc4:PatientDocuments runat="server" ID="ucPatientDocuments"></uc4:PatientDocuments>
                </telerik:RadPageView> 
   </telerik:RadMultiPage>
</div>
</asp:Content>
