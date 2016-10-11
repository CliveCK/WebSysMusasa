Public Class MicrofinanceClientDetails
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageTypeEnum)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageTypeEnum)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name").Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadMicroFinanceClientDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

        radDOB.MaxDate = Now

    End Sub
    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub LoadGrid(ByVal MicrofinanceClientDetailID As Long)

        Dim objMicroFinanceRepayments As New BusinessLogic.MicroFinanceRepayments(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radRepayPayments

            .DataSource = objMicroFinanceRepayments.GetMicroFinanceRepayments(MicrofinanceClientDetailID)
            .DataBind()

        End With

    End Sub

    Public Function LoadMicroFinanceClientDetails(ByVal BeneficiaryID As Long) As Boolean

        Try
            Dim objLookup As New BusinessLogic.CommonFunctions
            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name").Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            Dim objMicroFinanceClientDetails As New BusinessLogic.MicroFinanceClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                If .RetrieveWithAddress(BeneficiaryID) Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    If Not .DateOfBirth = "" Then radDOB.SelectedDate = .DateOfBirth
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    txtNationaIDNumber.Text = .NationlIDNo
                    txtContactNumber.Text = .ContactNo
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    If Not IsNothing(cboWard.Items.FindByValue(.WardID)) Then cboWard.SelectedValue = .WardID

                    If objAddress.Retrieve(.BeneficiaryID) Then

                        txtAddressID.Text = objAddress.AddressID
                        txtAddress.Text = objAddress.Address

                    End If

                End If

            End With

            With objMicroFinanceClientDetails

                If .Retrieve(BeneficiaryID) Then

                    txtMicrofinanceClientDetailID.Text = .MicrofinanceClientDetailID
                    txtRepaymentTerm.Text = .RepaymentTerm
                    If Not .DateApproved = "" Then radLoanDate.SelectedDate = .DateApproved
                    txtAmountApproved.Text = .AmountApproved
                    txtMonthlyRepayment.Text = .MonthlyRepayment

                    LoadGrid(.MicrofinanceClientDetailID)

                    ShowMessage("MicroFinanceClientDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load MicroFinanceClientDetails", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function Save() As Boolean

        Try

            Dim objMicroFinanceClientDetails As New BusinessLogic.MicroFinanceClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim update As Boolean = False

            'First save the beneficiary details - We will need the BeneficiaryID for the Client details

            With objBeneficiary

                .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .NationlIDNo = txtNationaIDNumber.Text
                If cboSex.SelectedIndex > -1 Then .Sex = cboSex.SelectedValue
                If radDOB.SelectedDate.HasValue Then .DateOfBirth = radDOB.SelectedDate
                .Suffix = 1
                .ContactNo = txtContactNumber.Text

                update = .BeneficiaryID > 0

                If .Save Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    If update = False Then .MemberNo = .GenerateMemberNo
                    .Save()

                    Dim objAdrress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objAdrress

                        .AddressID = IIf(IsNumeric(txtAddressID.Text), txtAddressID.Text, 0)
                        .OwnerID = objBeneficiary.BeneficiaryID
                        If cboDistrict.SelectedIndex > 0 Then .DistrictID = cboDistrict.SelectedValue
                        If cboWard.SelectedIndex > 0 Then .WardID = cboWard.SelectedValue
                        .Address = txtAddress.Text

                        If .Save() Then

                            txtAddressID.Text = .AddressID

                        End If

                    End With

                Else

                    ShowMessage("Failed to save beneficiary details...Process aborted!!", MessageTypeEnum.Error)
                    Exit Function

                End If

            End With

            With objMicroFinanceClientDetails

                .MicrofinanceClientDetailID = IIf(IsNumeric(txtMicrofinanceClientDetailID.Text), txtMicrofinanceClientDetailID.Text, 0)
                If IsNumeric(txtBeneficiaryID.Text) Then
                    .BeneficiaryID = txtBeneficiaryID.Text
                Else
                    ShowMessage("Missing beneficiary Information", MessageTypeEnum.Error)
                    Exit Function
                End If
                .RepaymentTerm = txtRepaymentTerm.Text
                If radLoanDate.SelectedDate.HasValue Then .DateApproved = radLoanDate.SelectedDate
                .AmountApproved = txtAmountApproved.Text
                .MonthlyRepayment = txtMonthlyRepayment.Text

                If .Save Then

                    LoadGrid(.MicrofinanceClientDetailID)

                    If Not IsNumeric(txtMicrofinanceClientDetailID.Text) OrElse Trim(txtMicrofinanceClientDetailID.Text) = 0 Then txtMicrofinanceClientDetailID.Text = .MicrofinanceClientDetailID
                    ShowMessage("MicroFinanceClientDetails saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtMicrofinanceClientDetailID.Text = ""
        txtBeneficiaryID.Text = ""
        txtRepaymentTerm.Text = 0
        radLoanDate.Clear()
        txtAmountApproved.Text = 0.0
        txtMonthlyRepayment.Text = 0.0

    End Sub

    Private Sub cmdSavePayments_Click(sender As Object, e As EventArgs) Handles cmdSavePayments.Click

        If IsNumeric(txtMicrofinanceClientDetailID.Text) AndAlso txtMicrofinanceClientDetailID.Text > 0 Then

            Dim objPayments As New BusinessLogic.MicroFinanceRepayments(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objPayments

                .MicrofinanceClientDetailID = txtMicrofinanceClientDetailID.Text
                If radRePaymentDateDate.SelectedDate.HasValue Then .DateRepayed = radRePaymentDateDate.SelectedDate
                .AmountRepayed = txtAmtRepayed.Text

                If .Save Then

                    LoadGrid(.MicrofinanceClientDetailID)
                    ShowMessage("Repayment saved successfully", MessageTypeEnum.Information)

                End If

            End With

        End If

    End Sub

    Private Sub cboDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDistrict.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboDistrict.SelectedIndex > 0 Then

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name", , "DistrictID = " & cboDistrict.SelectedValue).Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub
End Class