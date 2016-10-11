Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic
Imports Telerik.Web.UI

Public Class ProjectOutcomeDetailsControl
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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboProject

                .DataSource = objLookup.Lookup("tblProjects", "Project", "Name").Tables(0)
                .DataValueField = "Project"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub LoadGrid()

        Try
            Dim db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

            With radOutcomes

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, "SELECT OutcomeID, Description FROM tblOutcomes")

                Session("Outcomes") = ds
                .DataSource = ds

                .DataBind()

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedOutcomeIDs() As String

        Dim OutcomeIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radOutcomes.SelectedItems
            OutcomeIDArray.Add(gridRow.Item("OutcomeID").Text.ToString())
        Next

        Return String.Join(",", OutcomeIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Outcome() As String = GetSelectedOutcomeIDs.Split(",")

        If Outcome.Length > 0 Then

            For i As Long = 0 To Outcome.Length - 1

                Dim objOutput As New BusinessLogic.ProjectOutcome(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objOutput

                    .ProjectID = cboProject.SelectedValue
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


    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If Map() Then

            ShowMessage("Process succeeded...", MessageTypeEnum.Information)

        Else

            ShowMessage("Error during save...", MessageTypeEnum.Error)

        End If

    End Sub

    'Public Sub Clear()

    '    txtProjectObjectiveID.Text = ""
    '    If Not IsNothing(cboProject.Items.FindByValue("")) Then
    '        cboProject.SelectedValue = ""
    '    ElseIf Not IsNothing(cboProject.Items.FindByValue(0)) Then
    '        cboProject.SelectedValue = 0
    '    Else
    '        cboProject.SelectedIndex = -1
    '    End If
    '    If Not IsNothing(cboOutcome.Items.FindByValue("")) Then
    '        cboOutcome.SelectedValue = ""
    '    ElseIf Not IsNothing(cboOutcome.Items.FindByValue(0)) Then
    '        cboOutcome.SelectedValue = 0
    '    Else
    '        cboOutcome.SelectedIndex = -1
    '    End If

    'End Sub

    'Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

    '    Clear()

    'End Sub

    Private Sub cboProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProject.SelectedIndexChanged

        If cboProject.SelectedIndex > 0 Then

            Dim sql As String = "SELECT O.OutComeID, O.Description FROM tblOutcomes O inner join tblProjectOutcomes OO on OO.OutcomeID = O.OutcomeID "
            sql &= " inner join tblProjects P on P.Project = OO.ProjectID where OO.ProjectID = " & cboProject.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        LoadGrid()

    End Sub

    Private Sub radOutcomes_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radOutcomes.ItemCommand

        If cboProject.SelectedIndex > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Select("OutcomeID = " & gridItem("OutcomeID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped to selected"

                End If

            End If

        End If

    End Sub
End Class
