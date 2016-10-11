<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BeneficiaryControl.ascx.vb" Inherits="WebSysME.BeneficiaryControl" %>
<%@ Register src="~/Controls/HouseholdControls/BeneficiaryDetailsControl.ascx" tagname="BeneficiaryControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/HouseholdControls/HouseholdAddress.ascx" tagname="AddressControl" tagprefix="uc2" %>
<%@ Register src="~/Controls/HouseholdControls/HouseholdIncomeSources.ascx" tagname="IncomeControl" tagprefix="uc4" %>
<%@ Register src="~/Controls/HouseholdControls/HouseholdExpenditure.ascx" tagname="ExpenditureControl" tagprefix="uc5" %>
<%@ Register src="~/Controls/HouseholdControls/HouseholdParticipation.ascx" tagname="GroupControl" tagprefix="uc6" %>
<%@ Register src="~/Controls/HouseholdControls/HouseHoldAFWS.ascx" tagname="AssetsControl" tagprefix="uc7" %>
<%@ Register src="~/Controls/HouseholdControls/HouseholdNeeds.ascx" tagname="NeedsControl" tagprefix="uc3" %>
<div>
    <telerik:RadTabStrip ID="radTab" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="3" Align="Justify" Width="70%">
                <Tabs>
                    <telerik:RadTab Text="Income Sources">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Expenditure and Debts">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Priority Needs">
                    </telerik:RadTab>
                    <telerik:RadTab Text="HouseholdMembers" IsBreak="True">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Address">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Project and Group Participation">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Assets, FoodSource, WASH">
                    </telerik:RadTab>
                </Tabs>
   </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="3"
                Width="100%" CssClass="multiPage">
                <telerik:RadPageView runat="server" ID="RadPageView1" > 
                    <uc4:IncomeControl runat="server" ID="ucIncomeControl"></uc4:IncomeControl>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView2" > 
                    <uc5:ExpenditureControl runat="server" ID="ucExpenditure"></uc5:ExpenditureControl>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView3" >                    
                    <uc3:NeedsControl ID="ucNeedsControl" runat="server" ></uc3:NeedsControl>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView4" > 
                    <uc1:BeneficiaryControl runat="server" ID="ucBeneficiaryControl"></uc1:BeneficiaryControl>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView5" >                    
                    <uc2:AddressControl ID="ucAddresControl" runat="server" ></uc2:AddressControl>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView6" > 
                    <uc6:GroupControl runat="server" ID="ucGroup"></uc6:GroupControl>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView7" > 
                    <uc7:AssetsControl runat="server" ID="ucAssets"></uc7:AssetsControl>
                </telerik:RadPageView>
   </telerik:RadMultiPage>
</div>