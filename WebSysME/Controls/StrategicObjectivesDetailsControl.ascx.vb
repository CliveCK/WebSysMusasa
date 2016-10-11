Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class StrategicObjectivesDetailsControl
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

            If Not IsNothing(Request.QueryString("id")) Then

                LoadStrategicObjectives(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

            LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub LoadGrid()

        Dim objStrategicObjectives As New StrategicObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radStrategicListing

            .DataSource = objStrategicObjectives.RetrieveAll().Tables(0)
            .DataBind()

            ViewState("StrategicObjectives") = .DataSource

        End With

    End Sub

    Public Function LoadStrategicObjectives(ByVal StrategicObjectiveID As Long) As Boolean

        Try

            Dim objStrategicObjectives As New StrategicObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objStrategicObjectives

                If .Retrieve(StrategicObjectiveID) Then

                    txtStrategicObjectiveID.Text = .StrategicObjectiveID
                    txtFromYear.Text = .FromYear
                    txtToYear.Text = .ToYear
                    txtCode.Text = .Code
                    txtDescription.Text = .Description

                    ShowMessage("Strategic Objective loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Strategic Objective...", MessageTypeEnum.Error)
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

            Dim objStrategicObjectives As New StrategicObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objStrategicObjectives

                .StrategicObjectiveID = IIf(IsNumeric(txtStrategicObjectiveID.Text), txtStrategicObjectiveID.Text, 0)
                .FromYear = txtFromYear.Text
                .ToYear = txtToYear.Text
                .Code = txtCode.Text
                .Description = txtDescription.Text

                If .Save Then

                    If Not IsNumeric(txtStrategicObjectiveID.Text) OrElse Trim(txtStrategicObjectiveID.Text) = 0 Then txtStrategicObjectiveID.Text = .StrategicObjectiveID
                    ShowMessage("Strategic Objective details saved successfully...", MessageTypeEnum.Information)

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

        txtStrategicObjectiveID.Text = ""
        txtFromYear.Text = 0
        txtToYear.Text = 0
        txtCode.Text = ""
        txtDescription.Text = ""

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Clear()

    End Sub

    Private Sub radStrategicListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radStrategicListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radStrategicListing.Items(index)

            Select Case e.CommandName

                Case "View"

                    LoadStrategicObjectives(Server.HtmlDecode(item("StrategicObjectiveID").Text))

                Case "Delete"

                    Dim objStrategicObjective As New BusinessLogic.StrategicObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objStrategicObjective

                        .StrategicObjectiveID = Server.HtmlDecode(e.CommandArgument)

                        If .Delete() Then

                            ShowMessage("Objective deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radStrategicListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radStrategicListing.NeedDataSource

        radStrategicListing.DataSource = DirectCast(ViewState("StrategicObjectives"), DataTable)

    End Sub
End Class

