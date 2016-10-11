Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class OutputOutcomeDetailsControl
    Inherits System.Web.UI.UserControl

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private ds As DataSet

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

            With cboOutputs

                .DataSource = objLookup.Lookup("tblOutput", "OutputID", "Description").Tables(0)
                .DataValueField = "OutputID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub LoadGrid()

        If cboOutputs.SelectedIndex > 0 Then

            Dim sql As String = "SELECT A.OutcomeID, A.Description FROM tblOutput O inner join tblOutcomeOutputs OA on OA.OutputID = O.OutputID "
            sql &= " inner join tblOutcomes A on OA.OutcomeID = A.OutcomeID where O.OutputID = " & cboOutputs.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        Try
            Dim db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim sql As String = "select OutcomeID, Description As Outcome FROM"
            sql &= " tblOutcomes "

            With radOutputOutcomes

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

                Session("OutputsOutcomes1") = ds
                .DataSource = ds

                .DataBind()

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedOutcomeIDs() As String

        Dim OutcomeIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radOutputOutcomes.SelectedItems
            OutcomeIDArray.Add(gridRow.Item("OutcomeID").Text.ToString())
        Next

        Return String.Join(",", OutcomeIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Outcome() As String = GetSelectedOutcomeIDs.Split(",")

        If Outcome.Length > 0 Then

            For i As Long = 0 To Outcome.Length - 1

                Dim objOutput As New BusinessLogic.OutcomeOutputs(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objOutput

                    .OutputID = cboOutputs.SelectedValue
                    .OutcomeID = Outcome(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub radOutputOutcomes_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radOutputOutcomes.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radOutputOutcomes.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objOutputOutcomes As New BusinessLogic.OutcomeOutputs(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objOutputOutcomes

                        .OutputID = cboOutputs.SelectedValue
                        .OutcomeID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radOutputActivity_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radOutputOutcomes.ItemDataBound

        If cboOutputs.SelectedIndex > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

                If ds.Tables(0).Select("OutcomeID = " & gridItem("OutcomeID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped to selected Output"
                    btnImage.Visible = True

                End If

            End If

        End If

    End Sub

    Private Sub radOutputOutcomes_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radOutputOutcomes.NeedDataSource

        If cboOutputs.SelectedIndex > 0 Then

            Dim sql As String = "SELECT A.OutcomeID, A.Description FROM tblOutput O inner join tblOutcomeOutputs OA on OA.OutputID = O.OutputID "
            sql &= " inner join tblOutcomes A on OA.OutcomeID = A.OutcomeID where O.OutputID = " & cboOutputs.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        radOutputOutcomes.DataSource = Session("OutputsOutcomes1")

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Process completed successfully", MessageTypeEnum.Information)

        Else

            ShowMessage("Error while saving...", MessageTypeEnum.Information)

        End If

    End Sub

    Private Sub cboOutputs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOutputs.SelectedIndexChanged

        LoadGrid()

    End Sub
End Class