Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class KindDonationDetailsControl
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

    Public Function LoadKindDonation(ByVal KindDonationID As Long) As Boolean

        Try

            Dim objKindDonation As New KindDonation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objKindDonation

                If .Retrieve(KindDonationID) Then

                    txtKindDonationID.Text = .KindDonationID
                    radReceivedDate.SelectedDate = .ReceivedDate
                    txtDonation.Text = .Donation
                    txtQuantinty.Text = .Quantinty
                    txtReceivedFrom.Text = .ReceivedFrom
                    txtPurpose.Text = .Purpose
                    txtReceipt.Text = .ReceiptNo

                    ShowMessage("Kind Donation loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load KindDonation...", MessageTypeEnum.Error)
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

            Dim objKindDonation As New KindDonation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objKindDonation

                .KindDonationID = IIf(IsNumeric(txtKindDonationID.Text), txtKindDonationID.Text, 0)
                .ReceivedDate = radReceivedDate.SelectedDate
                .Donation = txtDonation.Text
                .Quantinty = txtQuantinty.Text
                .ReceivedFrom = txtReceivedFrom.Text
                .Purpose = txtPurpose.Text
                .ReceiptNo = txtReceipt.Text

                If .Save Then

                    If Not IsNumeric(txtKindDonationID.Text) OrElse Trim(txtKindDonationID.Text) = 0 Then txtKindDonationID.Text = .KindDonationID
                    LoadKindDonation(.KindDonationID)
                    LoadGrid()
                    ShowMessage("Kind Donation saved successfully...", MessageTypeEnum.Information)

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

    Private Sub LoadGrid()

        Dim objKindDonation As New KindDonation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Session("KindDonation") = Nothing

        With radKindDonation

            .DataSource = objKindDonation.GetKindDonation("SELECT * FROM tblKindDonation")
            .DataBind()

            Session("KindDonation") = .DataSource

        End With

    End Sub

    Public Sub Clear()

        txtKindDonationID.Text = ""
        radReceivedDate.Clear()
        txtDonation.Text = ""
        txtQuantinty.Text = ""
        txtReceivedFrom.Text = ""
        txtPurpose.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub radKindDonation_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radKindDonation.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radKindDonation.Items(index1)
            Dim KindDonationID As Integer

            KindDonationID = Server.HtmlDecode(item1("KindDonationID").Text)

            LoadKindDonation(KindDonationID)

        End If

    End Sub

    Private Sub radKindDonation_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radKindDonation.NeedDataSource

        radKindDonation.DataSource = Session("KindDonation")

    End Sub
End Class

