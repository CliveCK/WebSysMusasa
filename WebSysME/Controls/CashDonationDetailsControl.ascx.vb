Imports BusinessLogic

Partial Class CashDonationDetailsControl
    Inherits System.Web.UI.UserControl

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadCashDonation(ByVal CashDonationID As Long) As Boolean

        Try

            Dim objCashDonation As New CashDonation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCashDonation

                If .Retrieve(CashDonationID) Then

                    txtCashDonationID.Text = .CashDonationID
                    radFundDate.SelectedDate = .FundraisingDate
                    txtAmountReceived.Text = .AmountReceived
                    txtReceivedFrom.Text = .ReceivedFrom
                    txtPurpose.Text = .Purpose
                    txtReceipt.Text = .ReceiptNo

                    ShowMessage("CashDonation loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadCashDonation: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub LoadGrid()

        Dim objCashDonation As New CashDonation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Session("CashDonation") = Nothing

        With radCashDonation

            .DataSource = objCashDonation.GetCashDonation("SELECT * FROM tblCashDonations")
            .DataBind()

            Session("CashDonation") = .DataSource

        End With

    End Sub

    Public Function Save() As Boolean

        Try

            Dim objCashDonation As New CashDonation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCashDonation

                .CashDonationID = IIf(IsNumeric(txtCashDonationID.Text), txtCashDonationID.Text, 0)
                .FundraisingDate = radFundDate.SelectedDate
                .AmountReceived = txtAmountReceived.Text
                .ReceivedFrom = txtReceivedFrom.Text
                .Purpose = txtPurpose.Text
                .ReceiptNo = txtReceipt.Text

                If .Save Then

                    If Not IsNumeric(txtCashDonationID.Text) OrElse Trim(txtCashDonationID.Text) = 0 Then txtCashDonationID.Text = .CashDonationID
                    LoadCashDonation(.CashDonationID)
                    LoadGrid()
                    ShowMessage("CashDonation saved successfully...", MessageTypeEnum.Information)

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

        txtCashDonationID.Text = ""
        radFundDate.Clear()
        txtAmountReceived.Text = 0.0
        txtReceivedFrom.Text = ""
        txtPurpose.Text = ""
        txtReceipt.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub radCashDonation_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radCashDonation.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As Telerik.Web.UI.GridDataItem = radCashDonation.Items(index1)
            Dim CashDonationID As Integer

            CashDonationID = Server.HtmlDecode(item1("CashDonationID").Text)

            LoadCashDonation(CashDonationID)

        End If

    End Sub

    Private Sub radCashDonation_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radCashDonation.NeedDataSource

        radCashDonation.DataSource = Session("CashDonation")

    End Sub
End Class

