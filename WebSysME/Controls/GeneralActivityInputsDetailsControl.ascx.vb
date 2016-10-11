Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class GeneralActivityInputsDetailsControl
    Inherits System.Web.UI.UserControl

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            If Not IsNothing(Request.QueryString("id")) Then

                LoadGrid(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Private Sub LoadGrid(ByVal TrainingID As Long)

        Dim sql As String = "SELECT GeneralActivityInputID, Description, Quantity, Cost FROM tblGeneralActivityInputs WHERE GeneralActivityID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))
        ViewState("TrainingInputs") = Nothing

        If sql <> "" Then

            With radInputs

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()

                ViewState("GeneralActivityInputs") = .DataSource

            End With

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadGeneralActivityInputs(ByVal GeneralActivityInputID As Long) As Boolean

        Try

            Dim objGeneralActivityInputs As New GeneralActivityInputs(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGeneralActivityInputs

                If .Retrieve(GeneralActivityInputID) Then

                    txtGeneralActivityInputID.Text = .GeneralActivityInputID
                    txtCost.Text = .Cost
                    txtDescription.Text = .Description
                    txtQuantity.Text = .Quantity

                    ShowMessage("GeneralActivityInputs loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load GeneralActivityInputs...", MessageTypeEnum.Error)
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

            Dim objGeneralActivityInputs As New GeneralActivityInputs(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGeneralActivityInputs

                .GeneralActivityInputID = IIf(IsNumeric(txtGeneralActivityInputID.Text), txtGeneralActivityInputID.Text, 0)
                .GeneralActivityID = objUrlEncoder.Decrypt(Request.QueryString("id"))
                .Cost = txtCost.Text
                .Description = txtDescription.Text
                .Quantity = txtQuantity.Text

                If .Save Then

                    If Not IsNumeric(txtGeneralActivityInputID.Text) OrElse Trim(txtGeneralActivityInputID.Text) = 0 Then txtGeneralActivityInputID.Text = .GeneralActivityInputID
                    ShowMessage("GeneralActivityInputs saved successfully...", MessageTypeEnum.Information)

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

        txtGeneralActivityInputID.Text = ""
        txtCost.Text = 0.0
        txtDescription.Text = ""
        txtQuantity.Text = ""

    End Sub

    Private Sub radInputs_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radInputs.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radInputs.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objTraininInputs As New BusinessLogic.GeneralActivityInputs(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objTraininInputs

                        .GeneralActivityInputID = Server.HtmlDecode(e.CommandArgument)

                        If .Delete() Then

                            ShowMessage("Entry deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radInputs_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radInputs.NeedDataSource

        radInputs.DataSource = DirectCast(ViewState("GeneralActivityInputs"), DataTable)

    End Sub

    Private Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click

        Response.Redirect("~/GeneralActivityDetails.aspx?id=" & Request.QueryString("id"))

    End Sub

End Class

