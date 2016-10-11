Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class SurveyDetailsControl
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

            

        End If

        If Not IsNothing(Request.QueryString("id")) AndAlso Not HttpContext.Current.Request.Url.AbsoluteUri.Contains("Project") Then

            LoadSurvey(IIf(IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("id"))), objUrlEncoder.Decrypt(Request.QueryString("id")), 0))

        End If

        If HttpContext.Current.Request.Url.AbsoluteUri.Contains("Project") AndAlso Not IsNothing(Request.QueryString("id")) Then

            Dim sql As String = "SELECT * FROM tblSurveys E INNER JOIN tblProjectObjects PO on PO.ObjectID = E.SurveyID "
            sql &= " INNER JOIN luObjectTypes O on O.ObjectTypeID = PO.ObjectTypeID WHERE O.Description = 'Survey' AND PO.ProjectID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))

            LoadGrid(sql)

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadSurvey(ByVal SurveyID As Long) As Boolean

        Try

            Dim objSurvey As New Survey(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSurvey

                If .Retrieve(SurveyID) Then

                    txtSurveyID.Text = .SurveyID
                    radFromDate.SelectedDate = .FromDate
                    radToDate.SelectedDate = .ToDate
                    chkStatus.Checked = .Status
                    txtName.Text = .Name
                    txtDescription.Text = .Description
                    txtComments.Text = .Comments

                    ShowMessage("Survey loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Survey", MessageTypeEnum.Error)
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

            Dim objSurvey As New Survey(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSurvey

                .SurveyID = IIf(IsNumeric(txtSurveyID.Text), txtSurveyID.Text, 0)
                .FromDate = radFromDate.SelectedDate
                .ToDate = radToDate.SelectedDate
                .Status = chkStatus.Checked
                .Name = txtName.Text
                .Description = txtDescription.Text
                .Comments = txtComments.Text

                If .Save Then

                    If Not IsNumeric(txtSurveyID.Text) OrElse Trim(txtSurveyID.Text) = 0 Then txtSurveyID.Text = .SurveyID
                    LoadSurvey(.SurveyID)

                    If Not HttpContext.Current.Request.Url.AbsoluteUri.Contains("Project") Then

                        LoadGrid("Select * from tblSurveys")

                    End If

                    ShowMessage("Survey saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error during save...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtSurveyID.Text = ""
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

        Dim objSurvey As New BusinessLogic.Survey(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objSurvey.GetSurvey(sql)

        With radSurveyListing

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("SurveyList") = .DataSource

        End With

    End Sub

    Private Sub radSurveyListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radSurveyListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "ViewEvaluationDetails" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radSurveyListing.Items(index)

                Dim SurveyID As Long = Server.HtmlDecode(item("SurveyID").Text)

                LoadSurvey(SurveyID)

            End If

        End If

    End Sub

    Private Sub radEvaluationListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radSurveyListing.NeedDataSource

        radSurveyListing.DataSource = DirectCast(ViewState("SurveyList"), DataTable)

    End Sub
End Class

