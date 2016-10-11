Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class KeyChangePromisesDetailsControl
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

            With cboStrategicObjective

                .DataSource = objLookup.Lookup("tblStrategicObjectives", "StrategicObjectiveID", "Code").Tables(0)
                .DataValueField = "StrategicObjectiveID"
                .DataTextField = "Code"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadKeyChangePromises(objUrlEncoder.Decrypt(Request.QueryString("id")))
                LoadGrid(objUrlEncoder.Decrypt(Request.QueryString("id")))
                pnlDescription.Visible = True

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadKeyChangePromises(ByVal KeyChangePromiseID As Long) As Boolean

        Try

            Dim objKeyChangePromises As New KeyChangePromises(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objKeyChangePromises

                If .Retrieve(KeyChangePromiseID) Then

                    txtKeyChangePromiseID.Text = .KeyChangePromiseID
                    If Not IsNothing(cboStrategicObjective.Items.FindByValue(.StrategicObjectiveID)) Then cboStrategicObjective.SelectedValue = .StrategicObjectiveID
                    txtKeyChangePromiseNo.Text = .KeyChangePromiseNo

                    ShowMessage("KeyChange Promise loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load details", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Function LoadKeyChangePromiseDescription(ByVal KeyChangePromiseDescription As Long) As Boolean

        Try
            Dim objKeyChangeDescription As New KeyChangePromiseDescription(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objKeyChangeDescription

                If .Retrieve(KeyChangePromiseDescription) Then

                    txtKeyChangePromiseDescriptionID.Text = .KeyChangePromiseDescriptionID
                    txtDescription.Text = .Description

                    ShowMessage("Description loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load description...", MessageTypeEnum.Information)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage("Error loading Description", MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function Save() As Boolean

        Try

            Dim objKeyChangePromises As New KeyChangePromises(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objKeyChangePromises

                .KeyChangePromiseID = IIf(IsNumeric(txtKeyChangePromiseID.Text), txtKeyChangePromiseID.Text, 0)
                If cboStrategicObjective.SelectedIndex > -1 Then .StrategicObjectiveID = cboStrategicObjective.SelectedValue
                .KeyChangePromiseNo = txtKeyChangePromiseNo.Text

                If .Save Then

                    If Not IsNumeric(txtKeyChangePromiseID.Text) OrElse Trim(txtKeyChangePromiseID.Text) = 0 Then txtKeyChangePromiseID.Text = .KeyChangePromiseID
                    pnlDescription.Visible = True
                    ShowMessage("KeyChangePromises saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtKeyChangePromiseID.Text = ""
        If Not IsNothing(cboStrategicObjective.Items.FindByValue("")) Then
            cboStrategicObjective.SelectedValue = ""
        ElseIf Not IsNothing(cboStrategicObjective.Items.FindByValue(0)) Then
            cboStrategicObjective.SelectedValue = 0
        Else
            cboStrategicObjective.SelectedIndex = -1
        End If
        txtKeyChangePromiseNo.Text = ""

    End Sub

    Private Sub LoadGrid(ByVal KeyChangePromiseID As Long)

        Dim objKeyChangeDescription As New KeyChangePromiseDescription(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radKeyChangeDescription

            .DataSource = objKeyChangeDescription.GetDescriptionsByPromiseID(KeyChangePromiseID).Tables(0)
            .DataBind()

            ViewState("PromiseDescription") = .DataSource

        End With

    End Sub

    Private Sub cmdAddDescription_Click(sender As Object, e As EventArgs) Handles cmdAddDescription.Click

        If IsNumeric(txtKeyChangePromiseID.Text) AndAlso txtKeyChangePromiseID.Text > 0 Then

            Dim objKeyChangeDescription As New KeyChangePromiseDescription(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objKeyChangeDescription

                .KeyChangePromiseID = txtKeyChangePromiseID.Text
                .Description = txtDescription.Text

                If .Save Then

                    LoadGrid(.KeyChangePromiseID)
                    ShowMessage("Description added successfully...", MessageTypeEnum.Information)

                Else

                    ShowMessage("Error saving description...", MessageTypeEnum.Error)

                End If

            End With

        End If

    End Sub

    Private Sub radKeyChangeDescription_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radKeyChangeDescription.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radKeyChangeDescription.Items(index)

            Select Case e.CommandName

                Case "View"

                    LoadKeyChangePromiseDescription(Server.HtmlDecode(item("KeyChangePromiseDescriptionID").Text))

                Case "Delete"

                    Dim objKeyPromiseDescription As New BusinessLogic.KeyChangePromiseDescription(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objKeyPromiseDescription

                        .KeyChangePromiseDescriptionID = Server.HtmlDecode(e.CommandArgument)

                        If .Delete() Then

                            ShowMessage("Description deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If


    End Sub

    Private Sub radKeyChangeDescription_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radKeyChangeDescription.NeedDataSource

        radKeyChangeDescription.DataSource = DirectCast(ViewState("PromiseDescription"), DataTable)

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Clear()

    End Sub
End Class

