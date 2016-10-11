Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class HouseholdExpenditure
    Inherits System.Web.UI.UserControl

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

    Private ReadOnly Property HouseholdID As Long
        Get
            Dim txtHouseholdID As TextBox = DirectCast(Parent.Parent.FindControl("ucBeneficiaryControl").FindControl("txtBeneficiaryID1"), TextBox)

            Return IIf(IsNumeric(txtHouseholdID.Text), txtHouseholdID.Text, 0)
        End Get
    End Property

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboExpenditureItem

                .DataSource = objLookup.Lookup("luExpenditureItems", "ExpenditureItemID", "Description").Tables(0)
                .DataValueField = "ExpenditureItemID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboDebtItem

                .DataSource = objLookup.Lookup("luDebtItems", "DebtItemID", "Description").Tables(0)
                .DataValueField = "DebtItemID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            LoadGrid(CookiesWrapper.BeneficiaryID)

        End If

    End Sub

    Public Sub LoadGrid(ByVal HouseholdID As Integer)

        ViewState("Expenditure") = Nothing
        ViewState("Debts") = Nothing

        With radExpenditure

            .DataSource = db.ExecuteDataSet(CommandType.Text, "SELECT E.*, I.Description As ExpenditureItem FROM tblExpenditure E inner join luExpenditureItems I on E.ExpenditureItemID = I.ExpenditureItemID WHERE E.HouseholdID = " & CookiesWrapper.BeneficiaryID).Tables(0)
            .DataBind()

            ViewState("Expenditure") = .DataSource

        End With

        With radDebts

            .DataSource = db.ExecuteDataSet(CommandType.Text, "SELECT D.*, I.Description as DebtItem FROM tblDebts D inner join luDebtItems I on D.DebtItemID = I.DebtItemID WHERE D.HouseholdID = " & CookiesWrapper.BeneficiaryID).Tables(0)
            .DataBind()

            ViewState("Debts") = .DataSource

        End With

    End Sub

    Private Sub radDebts_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radDebts.NeedDataSource

        radDebts.DataSource = DirectCast(ViewState("Debts"), DataTable)

    End Sub

    Private Sub radExpenditure_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radExpenditure.NeedDataSource

        radExpenditure.DataSource = DirectCast(ViewState("Expenditure"), DataTable)

    End Sub

    Private Sub cmdSaveDebt_Click(sender As Object, e As EventArgs) Handles cmdSaveDebt.Click

        If CookiesWrapper.BeneficiaryID > 0 Then

            Dim sql As String = ""

            sql = "INSERT INTO tblDebts (HouseholdID,DebtItemID, AmountOwed) VALUES (" & CookiesWrapper.BeneficiaryID & "," & cboDebtItem.SelectedValue & "," & txtAmountOwed.Text & ")"

            If db.ExecuteNonQuery(CommandType.Text, sql) Then

                LoadGrid(CookiesWrapper.BeneficiaryID)
                ShowMessage("Saved successfully...", MessageTypeEnum.Information)

            End If

        Else

            ShowMessage("Please save individual/Household details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub cmdSaveExpenditure_Click(sender As Object, e As EventArgs) Handles cmdSaveExpenditure.Click

        If CookiesWrapper.BeneficiaryID > 0 Then
            Dim sql As String = ""

            sql = "INSERT INTO tblExpenditure (HouseholdID,ExpenditureItemID, AverageExpenditure) VALUES (" & CookiesWrapper.BeneficiaryID & "," & cboExpenditureItem.SelectedValue & "," & txtAverageExpenditure.Text & ")"

            If db.ExecuteNonQuery(CommandType.Text, sql) Then

                LoadGrid(CookiesWrapper.BeneficiaryID)

                ShowMessage("Saved successfully...", MessageTypeEnum.Information)

            End If

        Else

            ShowMessage("Please save Individual/Household details first...", MessageTypeEnum.Error)

        End If

    End Sub
End Class