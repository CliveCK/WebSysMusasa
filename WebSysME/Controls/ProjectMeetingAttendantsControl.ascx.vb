Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class ProjectMeetingAttendantsControl
    Inherits System.Web.UI.UserControl

    Dim ds As DataSet
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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

    Public Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ProjectMeetingIDText As TextBox = DirectCast(Parent.Parent.FindControl("txtProjectMeetingID"), TextBox)
        txtProjectMeetingID.Text = ProjectMeetingIDText.Text

        If IsNumeric(txtProjectMeetingID.Text) Then
            Dim sql As String = "SELECT * FROM tblProjectMeetingAttendants WHERE ProjectMeetingID = " & txtProjectMeetingID.Text

            ds = db.ExecuteDataSet(CommandType.Text, sql)

            LoadGrid()

        Else : End If

    End Sub

    Protected Function GetSelectedStaffIDs() As String

        Dim StaffIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radStaff1.SelectedItems
            StaffIDArray.Add(gridRow.Item("StaffID").Text.ToString())
        Next

        Return String.Join(",", StaffIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Staff() As String = GetSelectedStaffIDs.Split(",")

        If Staff.Length > 0 Then

            For i As Long = 0 To Staff.Length - 1

                Dim objAttendants As New BusinessLogic.ProjectMeetingAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objAttendants

                    .ProjectMeetingID = txtProjectMeetingID.Text
                    .StaffID = Staff(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If IsNumeric(txtProjectMeetingID.Text) AndAlso txtProjectMeetingID.Text > 0 Then

            If Map() Then

                Dim sql As String = "SELECT * FROM tblProjectMeetingAttendants WHERE ProjectMeetingID = " & txtProjectMeetingID.Text

                ds = db.ExecuteDataSet(CommandType.Text, sql)

                LoadGrid()
                ShowMessage("Attendants saved successfully...", MessageTypeEnum.Information)

            Else

                ShowMessage("Failed to save attendants...", MessageTypeEnum.Error)

            End If

        Else

            ShowMessage("Please save Meeting details first!", MessageTypeEnum.Error)

        End If

    End Sub

    Public Sub LoadGrid()

        Dim sql As String = "Select O.Name as Organization, S.* from tblStaffMembers S inner join tblOrganization O on S.OrganizationID = O.OrganizationID"

        If sql <> "" Then

            With radStaff1

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()

                ViewState("ProjectMeetingAttendants") = .DataSource

            End With

        End If

    End Sub

    Private Sub radStaff1_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radStaff1.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radStaff1.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objAttendants As New BusinessLogic.ProjectMeetingAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objAttendants

                        .ProjectMeetingID = txtProjectMeetingID.Text
                        .StaffID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radStaff1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radStaff1.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

            If ds.Tables(0).Select("StaffID = " & gridItem("StaffID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already selected..."
                btnImage.Visible = True

            End If

        End If

    End Sub

    Private Sub radStaff1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radStaff1.NeedDataSource

        radStaff1.DataSource = DirectCast(ViewState("ProjectMeetingAttendants"), DataTable)

    End Sub
End Class