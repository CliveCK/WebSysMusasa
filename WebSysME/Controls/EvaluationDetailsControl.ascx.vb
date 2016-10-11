Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class EvaluationDetailsControl
    Inherits System.Web.UI.UserControl

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

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboTypeOfEvaluation

                .DataSource = objLookup.Lookup("luTypeOfEvaluation", "TypeOfEvaluationID", "Description").Tables(0)
                .DataValueField = "TypeOfEvaluationID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) AndAlso Not HttpContext.Current.Request.Url.AbsoluteUri.Contains("Project") Then

                LoadEvaluation(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

            If HttpContext.Current.Request.Url.AbsoluteUri.Contains("Project") AndAlso Not IsNothing(Request.QueryString("id")) Then

                Dim sql As String = "SELECT E.* FROM tblEvaluations E INNER JOIN tblProjectObjects PO on PO.ObjectID = E.EvaluationID "
                sql &= " INNER JOIN luObjectTypes O on O.ObjectTypeID = PO.ObjectTypeID WHERE O.Description = 'Evaluation' AND PO.ProjectID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))

                LoadGrid(sql)

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadEvaluation(ByVal EvaluationID As Long) As Boolean

        Try

            Dim objEvaluation As New Evaluation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objEvaluation

                If .Retrieve(EvaluationID) Then

                    txtEvaluationID.Text = .EvaluationID
                    If Not IsNothing(cboTypeOfEvaluation.Items.FindByValue(.TypeOfEvaluationID)) Then cboTypeOfEvaluation.SelectedValue = .TypeOfEvaluationID
                    radFromDate.SelectedDate = .FromDate
                    radToDate.SelectedDate = .ToDate
                    chkStatus.Checked = .Status
                    txtName.Text = .Name
                    txtDescription.Text = .Description
                    txtComments.Text = .Comments

                    ShowMessage("Evaluation loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadEvaluation: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objEvaluation As New Evaluation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objEvaluation

                .EvaluationID = IIf(IsNumeric(txtEvaluationID.Text), txtEvaluationID, 0)
                If cboTypeOfEvaluation.SelectedIndex > -1 Then .TypeOfEvaluationID = cboTypeOfEvaluation.SelectedValue
                .FromDate = radFromDate.SelectedDate
                .ToDate = radToDate.SelectedDate
                .Status = chkStatus.Checked
                .Name = txtName.Text
                .Description = txtDescription.Text
                .Comments = txtComments.Text

                If .Save Then

                    If Not IsNumeric(txtEvaluationID.Text) OrElse Trim(txtEvaluationID.Text) = 0 Then txtEvaluationID.Text = .EvaluationID
                    LoadEvaluation(.EvaluationID)
                    If Not HttpContext.Current.Request.Url.AbsoluteUri.Contains("Project") Then

                        LoadGrid("Select * from tblEvaluations")

                    End If

                    ShowMessage("Evaluation details saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error saving details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtEvaluationID.Text = ""
        If Not IsNothing(cboTypeOfEvaluation.Items.FindByValue("")) Then
            cboTypeOfEvaluation.SelectedValue = ""
        ElseIf Not IsNothing(cboTypeOfEvaluation.Items.FindByValue(0)) Then
            cboTypeOfEvaluation.SelectedValue = 0
        Else
            cboTypeOfEvaluation.SelectedIndex = -1
        End If
        radFromDate.Clear()
        radToDate.Clear()
        chkStatus.Checked = False
        txtName.Text = ""
        txtDescription.Text = ""
        txtComments.Text = ""

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Clear()

    End Sub

    Public Sub LoadGrid(ByVal sql As String)

        ViewState("Evaluation") = Nothing

        Dim objEvaluation As New BusinessLogic.Evaluation(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objEvaluation.GetEvaluation(sql)

        With radEvaluationListing

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("Evaluation") = .DataSource

        End With

    End Sub

    Private Sub radEvaluationListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radEvaluationListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "ViewEvaluationDetails" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radEvaluationListing.Items(index)

                Dim EvaluationID As Long = Server.HtmlDecode(item("EvaluationID").Text)

                LoadEvaluation(EvaluationID)

            End If

        End If

    End Sub

    Private Sub radEvaluationListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radEvaluationListing.NeedDataSource

        radEvaluationListing.DataSource = DirectCast(ViewState("Evaluation"), DataTable)

    End Sub

End Class

