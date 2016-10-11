Imports Telerik.Web.UI
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class TripTravellers
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

        Dim TripIDText As TextBox = DirectCast(Parent.Parent.FindControl("txtTripID"), TextBox)
        txtTripID.Text = TripIDText.Text

        If IsNumeric(txtTripID.Text) Then

            Dim sql As String = "SELECT * FROM tblTripTravellers WHERE TripID = " & txtTripID.Text

            ds = db.ExecuteDataSet(CommandType.Text, sql)

            LoadGrid()

        Else : End If

    End Sub

    Protected Function GetSelectedStaffIDs() As String

        Dim StaffIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radStaff.SelectedItems
            StaffIDArray.Add(gridRow.Item("StaffID").Text.ToString())
        Next

        Return String.Join(",", StaffIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Staff() As String = GetSelectedStaffIDs.Split(",")

        If Staff.Length > 0 Then

            For i As Long = 0 To Staff.Length - 1

                Dim objAttendants As New BusinessLogic.TripTravellers(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objAttendants

                    .TripID = txtTripID.Text
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

        If IsNumeric(txtTripID.Text) AndAlso txtTripID.Text > 0 Then

            If Map() Then

                Dim sql As String = "SELECT * FROM tblTripTravellers WHERE TripID = " & txtTripID.Text

                ds = db.ExecuteDataSet(CommandType.Text, sql)

                LoadGrid()
                ShowMessage("Travellers saved successfully...", MessageTypeEnum.Information)

            Else

                ShowMessage("Failed to save attendants...", MessageTypeEnum.Error)

            End If

        Else

            ShowMessage("Please save Meeting details first!", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub LoadGrid()

        Dim sql As String = "Select O.Name as Organization, S.* from tblStaffMembers S inner join tblOrganization O on S.OrganizationID = O.OrganizationID"

        If sql <> "" Then

            With radStaff

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()

                ViewState("TripAttendants") = .DataSource

            End With

        End If

    End Sub

    Private Sub radStaff_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radStaff.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radStaff.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objAttendants As New BusinessLogic.TripTravellers(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objAttendants

                        .TripID = txtTripID.Text
                        .StaffID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radStaff_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radStaff.ItemDataBound

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

    Private Sub radStaff_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radStaff.NeedDataSource

        radStaff.DataSource = DirectCast(ViewState("TripAttendants"), DataTable)

    End Sub
End Class