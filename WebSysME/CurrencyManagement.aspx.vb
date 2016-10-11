Imports Telerik.Web.UI

Public Class CurrencyManagement
    Inherits System.Web.UI.Page

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

            With cboFrom

                .DataSource = objLookup.Lookup("luCurrency", "CurrencyID", "Description")
                .DataValueField = "CurrencyID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboTo

                .DataSource = objLookup.Lookup("luCurrency", "CurrencyID", "Description")
                .DataValueField = "CurrencyID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            LoadGrid()

        End If

    End Sub

    Private Sub LoadGrid()

        Dim objCurrency As New BusinessLogic.CurrencyExchageRates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "Select *, F.Description As FromCurrency, T.Description As ToCurrency from tblCurrencyExchangeRates E inner join "
        sql &= "luCurrency F on E.FromCurrencyID = F.CurrencyID inner join luCurrency T on E.ToCurrencyID = T.CurrencyID"

        With radCurrency

            .DataSource = objCurrency.GetCurrencyExchageRates(sql)
            .DataBind()

            ViewState("CurrencyExchangeRate") = .DataSource

        End With

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If cboFrom.SelectedValue > 0 AndAlso cboTo.SelectedValue > 0 AndAlso txtRate.Text <> "" AndAlso radDateEffective.SelectedDate.HasValue Then

            Dim objCurrency As New BusinessLogic.CurrencyExchageRates(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCurrency

                .CurrencyExchangeRateID = 0
                .FromCurrencyID = cboFrom.SelectedValue
                .ToCurrencyID = cboTo.SelectedValue
                .Rate = txtRate.Text
                .DateEffective = radDateEffective.SelectedDate

                If .Save Then

                    LoadGrid()
                    ShowMessage("Exchange Rate saved successfully...", MessageTypeEnum.Information)

                End If


            End With

        Else

            ShowMessage("Insufficient parameters supplied...please revise!", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radCurrency_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radCurrency.NeedDataSource

        radCurrency.DataSource = DirectCast(ViewState("CurrencyExchangeRate"), DataSet)

    End Sub
End Class