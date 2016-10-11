Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class TrainingStatus
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboIntake

                .DataSource = objLookup.Lookup("tblIntake", "IntakeID", "Description").Tables(0)
                .DataValueField = "IntakeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboTrainingStatus

                .DataSource = objLookup.Lookup("luTrainingStatus", "TrainingStatusID", "Description").Tables(0)
                .DataValueField = "TrainingStatusID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With
        End If

    End Sub

    Private Sub LoadGrid()

        Dim sql As String = "SELECT O.AttendantID FROM tblIntakeTrainingAttendants O "
        sql &= " inner join tblIntake Ob On Ob.IntakeID = O.IntakeID where O.IntakeID = " & cboIntake.SelectedValue

        ds = db.ExecuteDataSet(CommandType.Text, sql)

        Dim sql1 As String = "Select Distinct BeneficiaryID As AttendantID, HS.FirstName, HS.Surname, S.Description As TrainingStatus from tblTrainingAttendants TA "
        sql1 &= "inner join tblIntakeTrainings I On TA.TrainingID = I.TrainingID "
        sql1 &= "inner join tblHealthCenterStaff HS On HS.HealthCenterStaffID = TA.BeneficiaryID "
        sql1 &= "left outer join tblIntakeTrainingAttendants ITA On ITA.IntakeID = ITA.IntakeID And ITA.AttendantID = TA.BeneficiaryID "
        sql1 &= "left outer join luTrainingStatus S On S.TrainingStatusID = ITA.TrainingStatusID WHERE I.IntakeID=" & cboIntake.SelectedValue

        Try
            Dim db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

            With radAttendants

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql1)

                .DataSource = ds.Tables(0)

                .DataBind()

                ViewState("mtIbntake111") = .DataSource

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedObjectiveIDs() As String

        Dim ObjectiveIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radAttendants.SelectedItems
            ObjectiveIDArray.Add(gridRow.Item("AttendantID").Text.ToString())
        Next

        Return String.Join(",", ObjectiveIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Objectives() As String = GetSelectedObjectiveIDs.Split(",")

        If Objectives.Length > 0 Then

            For i As Long = 0 To Objectives.Length - 1

                Dim objObjectiveOutcomes As New BusinessLogic.IntakeTrainingAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objObjectiveOutcomes

                    .IntakeID = cboIntake.SelectedValue
                    .AttendantID = Objectives(i)
                    .TrainingStatusID = cboTrainingStatus.SelectedValue

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub radTrainings_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radAttendants.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radAttendants.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objObjectiveThemes As New BusinessLogic.IntakeTrainingAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objObjectiveThemes

                        .IntakeID = cboIntake.SelectedValue
                        .AttendantID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radTrainings_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radAttendants.ItemDataBound

        If cboIntake.SelectedIndex > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

                If ds.Tables(0).Select("AttendantID = " & gridItem("AttendantID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped To selected Theme"
                    btnImage.Visible = True

                End If

            End If

        End If

    End Sub

    Private Sub radTrainings_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radAttendants.NeedDataSource

        If cboIntake.SelectedIndex > 0 Then

            Dim sql As String = "SELECT O.AttendantID FROM tblIntakeTrainingAttendants O "
            sql &= " inner join tblIntake Ob On Ob.IntakeID = O.IntakeID where O.IntakeID = " & cboIntake.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        radAttendants.DataSource = DirectCast(ViewState("mtIbntake111"), DataTable)

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If cboTrainingStatus.SelectedIndex > 0 Then

            If Map() Then

                LoadGrid()
                ShowMessage("Process completed successfully", MessageTypeEnum.Information)

            Else

                ShowMessage("Error While saving...", MessageTypeEnum.Information)

            End If

        Else

            ShowMessage("Please select Training Status first before attempting to save!", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub cboIntake_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboIntake.SelectedIndexChanged

        LoadGrid()

    End Sub
End Class