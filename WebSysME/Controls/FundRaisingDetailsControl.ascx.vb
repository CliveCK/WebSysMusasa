Imports BusinessLogic

Partial Class FundRaisingDetailsControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

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

            If Not IsNothing(Request.QueryString("id")) Then

                LoadFundRaising(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

            LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadFundRaising(ByVal FundraisingID As Long) As Boolean

        Try

            Dim objFundRaising As New FundRaising(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFundRaising

                If .Retrieve(FundraisingID) Then

                    txtFundraisingID.Text = .FundraisingID
                    radFundDate.SelectedDate = .FundraisingDate
                    txtAmountRaised.Text = .AmountRaised
                    txtFundraisingEvent.Text = .FundraisingEvent
                    txtLocation.Text = .Location

                    ShowMessage("FundRaising loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load FundRaising...", MessageTypeEnum.Error)
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

            Dim objFundRaising As New FundRaising(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFundRaising

                .FundraisingID = IIf(IsNumeric(txtFundraisingID.Text), txtFundraisingID.Text, 0)
                .FundraisingDate = radFundDate.SelectedDate
                .AmountRaised = txtAmountRaised.Text
                .FundraisingEvent = txtFundraisingEvent.Text
                .Location = txtLocation.Text

                If .Save Then

                    If Not IsNumeric(txtFundraisingID.Text) OrElse Trim(txtFundraisingID.Text) = 0 Then txtFundraisingID.Text = .FundraisingID
                    LoadFundRaising(.FundraisingID)
                    LoadGrid()
                    ShowMessage("FundRaising saved successfully...", MessageTypeEnum.Information)

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

        txtFundraisingID.Text = ""
        radFundDate.Clear()
        txtAmountRaised.Text = 0.0
        txtFundraisingEvent.Text = ""
        txtLocation.Text = ""

    End Sub

    Private Sub LoadGrid()

        Dim objFundRaising As New FundRaising(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Session("Fundraising") = Nothing

        With radFundraising

            .DataSource = objFundRaising.GetFundRaising("SELECT * FROM tblFundRaisings")
            .DataBind()

            Session("Fundraising") = .DataSource

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub radFundraising_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radFundraising.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As Telerik.Web.UI.GridDataItem = radFundraising.Items(index1)
            Dim FundRaisingID As Integer

            FundRaisingID = Server.HtmlDecode(item1("FundRaisingID").Text)

            LoadFundRaising(FundRaisingID)

        End If

    End Sub

    Private Sub radFundraising_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radFundraising.NeedDataSource

        radFundraising.DataSource = Session("Fundraising")

    End Sub

    Private Sub lnkPar_Click(sender As Object, e As EventArgs) Handles lnkPar.Click

        If IsNumeric(txtFundraisingID.Text) AndAlso txtFundraisingID.Text > 0 Then

            Dim EventTypeID = db.ExecuteScalar(CommandType.Text, "SELECT EventTypeID FROM luEventTypes WHERE Description = 'Fundraising'")
            If IsNumeric(EventTypeID) Then
                Response.Redirect("~/EventParticipants.aspx?par=" & objUrlEncoder.Encrypt(HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, IIf(HttpContext.Current.Request.Url.AbsoluteUri.LastIndexOf("?") > 0, HttpContext.Current.Request.Url.AbsoluteUri.LastIndexOf("?"), HttpContext.Current.Request.Url.AbsoluteUri.Length))) & "&ev=" & objUrlEncoder.Encrypt(txtFundraisingID.Text) & "&evid=" & objUrlEncoder.Encrypt(EventTypeID))
            End If

        Else

            ShowMessage("Please save details first before attempting to save Participants...", MessageTypeEnum.Warning)

        End If
    End Sub
End Class

