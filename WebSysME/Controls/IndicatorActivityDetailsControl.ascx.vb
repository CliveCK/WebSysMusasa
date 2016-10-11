Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class IndicatorActivityDetailsControl
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

            LoadGrid2()
            LoadGrid()

        End If

    End Sub



    Private Sub LoadGrid()

        Dim IndicatorTrackingID As String = ""
        For Each gridRow As Telerik.Web.UI.GridDataItem In radIndicatorTracking.SelectedItems
            IndicatorTrackingID = gridRow.Item("IndicatorTrackingID").Text.ToString()
        Next

        Dim sql1 As String = "SELECT A.ID As ActivityID FROM tblIndicatorTracking T inner join tblIndicatorActivities IA on T.IndicatorTrackingID = IA.IndicatorID "
        sql1 &= " inner join Appointments A on IA.ActivityID = A.ID where T.IndicatorTrackingID = " & IIf(String.IsNullOrWhiteSpace(IndicatorTrackingID), 0, IndicatorTrackingID)

        ds = db.ExecuteDataSet(CommandType.Text, sql1)

        Try
            Dim db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim sql As String = "select AP.ID AS ActivityID, A.Description As Activity, CAST([Start] AS Date) [Start], CAST([End] AS Date) [End], "
            sql &= " ISNULL(FirstName, '') + ' ' + ISNULL(Surname, '') As Name ,Completed, AP.Description as [Description] from Appointments AP "
            sql &= " inner join tblActivities A on AP.ActivityID = A.ActivityID inner join tblStaffMembers S on S.StaffID = AP.UserID"

            With radOutputActivity

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

                Session("IndTracking") = ds
                .DataSource = ds

                .DataBind()

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Private Sub LoadGrid2()

        Try
            Dim db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim sql As String = "select IT.IndicatorTrackingID, I.IndicatorID, I.Name, I.BaselineValue, I.DataCollectionMethod "
            sql &= " ,IT.[Year], IT.[Month], IT.[Target], IT.Achievement from tblIndicators I "
            sql &= " inner join tblIndicatorTracking IT on I.IndicatorID = IT.IndicatorID"

            With radIndicatorTracking

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

                Session("IndTracking1") = ds
                .DataSource = ds

                .DataBind()

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedActivityIDs() As String

        Dim ActivityIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radOutputActivity.SelectedItems
            ActivityIDArray.Add(gridRow.Item("ActivityID").Text.ToString())
        Next

        Return String.Join(",", ActivityIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Activity() As String = GetSelectedActivityIDs.Split(",")
        Dim IndicatorTrackingID As Integer

        For Each gridRow As Telerik.Web.UI.GridDataItem In radIndicatorTracking.SelectedItems
            IndicatorTrackingID = gridRow.Item("IndicatorTrackingID").Text.ToString()
        Next

        If Activity.Length > 0 AndAlso IsNumeric(IndicatorTrackingID) Then

            For i As Long = 0 To Activity.Length - 1

                Dim objIndicators As New BusinessLogic.IndicatorActivities(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objIndicators

                    .IndicatorID = IndicatorTrackingID
                    .ActivityID = Activity(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub radOutputActivity_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radOutputActivity.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radOutputActivity.Items(index)
            Dim IndicatorTrackingID As Integer

            Select Case e.CommandName

                Case "Delete"

                    Dim objIndicatorActivity As New BusinessLogic.IndicatorActivities(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    For Each gridRow As Telerik.Web.UI.GridDataItem In radIndicatorTracking.SelectedItems
                        IndicatorTrackingID = gridRow.Item("IndicatorTrackingID").Text.ToString()
                    Next

                    With objIndicatorActivity

                        .IndicatorID = IndicatorTrackingID
                        .ActivityID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radOutputActivity_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radOutputActivity.ItemDataBound

        Dim IndicatorTrackingID As String = "0"
        For Each gridRow As Telerik.Web.UI.GridDataItem In radIndicatorTracking.SelectedItems
            IndicatorTrackingID = gridRow.Item("IndicatorTrackingID").Text.ToString()
        Next

        If IndicatorTrackingID > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

                If ds.Tables(0).Select("ActivityID = " & gridItem("ActivityID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped to selected Output"
                    btnImage.Visible = True

                End If

            End If

        End If

    End Sub

    Private Sub radOutputActivity_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radOutputActivity.NeedDataSource

        Dim IndicatorTrackingID As String = ""
        For Each gridRow As Telerik.Web.UI.GridDataItem In radIndicatorTracking.SelectedItems
            IndicatorTrackingID = gridRow.Item("IndicatorTrackingID").Text.ToString()
        Next

        Dim sql As String = "SELECT A.ID As ActivityID FROM tblIndicatorTracking T inner join tblIndicatorActivities IA on T.IndicatorTrackingID = IA.IndicatorID "
        sql &= " inner join Appointments A on IA.ActivityID = A.ID where T.IndicatorTrackingID = " & IIf(String.IsNullOrWhiteSpace(IndicatorTrackingID), 0, IndicatorTrackingID)

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        radOutputActivity.DataSource = Session("IndTracking")

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Process completed successfully", MessageTypeEnum.Information)

        Else

            ShowMessage("Error while saving...", MessageTypeEnum.Information)

        End If

    End Sub

    Private Sub radIndicatorTracking_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radIndicatorTracking.ItemCommand

        If e.CommandName = "RowClick" Then

            LoadGrid()

        End If

    End Sub

    Private Sub radIndicatorTracking_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radIndicatorTracking.NeedDataSource

        radOutputActivity.DataSource = Session("IndTracking1")

    End Sub

End Class