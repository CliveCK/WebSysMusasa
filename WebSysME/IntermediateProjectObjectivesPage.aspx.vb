Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class IntermediateProjectObjectivesPage
    Inherits System.Web.UI.Page
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboIntermediateObjectives

                .DataSource = objLookup.Lookup("tblStrategicObjectives", "StrategicObjectiveID", "Description").Tables(0)
                .DataValueField = "StrategicObjectiveID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub LoadGrid()

        Try
            Dim db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim sql As String = "SELECT ObjectiveID, ObjectiveNo, Description FROM tblObjectives "

            With radProjectObjective

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

                .DataSource = ds

                .DataBind()

                Session("IntermediateObjectives") = .DataSource

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedObjectiveIDs() As String

        Dim ObjectiveIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radProjectObjective.SelectedItems
            ObjectiveIDArray.Add(gridRow.Item("ObjectiveID").Text.ToString())
        Next

        Return String.Join(",", ObjectiveIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Objective() As String = GetSelectedObjectiveIDs.Split(",")

        If Objective.Length > 0 Then

            For i As Long = 0 To Objective.Length - 1

                Dim objIntermediateProjectObjective As New BusinessLogic.IntermediateProjectObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objIntermediateProjectObjective

                    .IntermediateObjectiveID = cboIntermediateObjectives.SelectedValue
                    .ProjectObjectiveID = Objective(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

            Return True

        End If

        Return False

    End Function
    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Process completed successfully", MessageTypeEnum.Information)

        Else

            ShowMessage("Error while saving...", MessageTypeEnum.Information)

        End If

    End Sub

    Public Sub Clear()

        txtProjectObjectiveID.Text = ""
        If Not IsNothing(cboIntermediateObjectives.Items.FindByValue("")) Then
            cboIntermediateObjectives.SelectedValue = ""
        ElseIf Not IsNothing(cboIntermediateObjectives.Items.FindByValue(0)) Then
            cboIntermediateObjectives.SelectedValue = 0
        Else
            cboIntermediateObjectives.SelectedIndex = -1
        End If

    End Sub

    Private Sub radProjectObjective_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radProjectObjective.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radProjectObjective.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objIntermediateProjectObjective As New BusinessLogic.IntermediateProjectObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objIntermediateProjectObjective

                        .IntermediateObjectiveID = cboIntermediateObjectives.SelectedValue
                        .ProjectObjectiveID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radProjectObjective_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radProjectObjective.ItemDataBound

        If cboIntermediateObjectives.SelectedIndex > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

                Dim sql As String = "select O.ObjectiveID, O.Description from tblObjectives O inner join tblIntermediateProjectObjectives "
                sql &= " PO on PO.ProjectObjectiveID = O.ObjectiveID inner join tblStrategicObjectives P on P.StrategicObjectiveID = PO.IntermediateObjectiveID where P.StrategicObjectiveID = " & cboIntermediateObjectives.SelectedValue

                ds = db.ExecuteDataSet(CommandType.Text, sql)

                If ds.Tables(0).Select("ObjectiveID = " & gridItem("ObjectiveID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped"
                    btnImage.Visible = True

                End If

            End If

        End If

    End Sub

    Private Sub radProjectObjective_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radProjectObjective.NeedDataSource

        If cboIntermediateObjectives.SelectedIndex > 0 Then

            Dim sql As String = "select O.ObjectiveID, O.Description from tblObjectives O inner join tblIntermediateProjectObjectives "
            sql &= " PO on PO.ProjectObjectiveID = O.ObjectiveID inner join tblStrategicObjectives P on P.StrategicObjectiveID = PO.IntermediateObjectiveID where P.StrategicObjectiveID = " & cboIntermediateObjectives.SelectedValue
            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        radProjectObjective.DataSource = Session("IntermediateObjectives")

    End Sub

    Private Sub cboIntermediateObjectives_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboIntermediateObjectives.SelectedIndexChanged

        If cboIntermediateObjectives.SelectedIndex > 0 Then

            Dim sql As String = "select O.ObjectiveID, O.Description from tblObjectives O inner join tblIntermediateProjectObjectives "
            sql &= " PO on PO.ProjectObjectiveID = O.ObjectiveID inner join tblStrategicObjectives P on P.StrategicObjectiveID = PO.IntermediateObjectiveID where P.StrategicObjectiveID = " & cboIntermediateObjectives.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        LoadGrid()

    End Sub

End Class