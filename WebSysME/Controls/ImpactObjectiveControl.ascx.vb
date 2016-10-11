Imports Telerik.Web.UI
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class ImpactObjectiveControl
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

            With cboImpact

                .DataSource = objLookup.Lookup("luImpact", "ImpactID", "Description").Tables(0)
                .DataValueField = "Impact"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub LoadGrid()

        Try
            Dim db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

            With radObjectives

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, "SELECT ObjectiveID, ObjectiveNo, Description FROM tblObjectives")

                Session("mtObjectives") = ds
                .DataSource = ds

                .DataBind()

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedObjectiveIDs() As String

        Dim ObjectiveIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radObjectives.SelectedItems
            ObjectiveIDArray.Add(gridRow.Item("OutcomeID").Text.ToString())
        Next

        Return String.Join(",", ObjectiveIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Objective() As String = GetSelectedObjectiveIDs.Split(",")

        If Objective.Length > 0 Then

            For i As Long = 0 To Objective.Length - 1

                Dim objImpactObjectives As New BusinessLogic.ImpactObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objImpactObjectives

                    .ImpactID = cboImpact.SelectedValue
                    .ObjectiveID = Objective(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub radObjectives_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radObjectives.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radObjectives.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objImpactObjectives As New BusinessLogic.ImpactObjectives(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objImpactObjectives

                        .ImpactID = cboImpact.SelectedValue
                        .ObjectiveID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radObjectives_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radObjectives.ItemDataBound

        If cboImpact.SelectedIndex > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

                Dim sql As String = "SELECT O.ObjectiveID, O.Description FROM tblObjectives O inner join tblImpactObjectives OO on OO.ObjectiveID = O.ObjectiveID "
                sql &= " inner join luImpact Ob on Ob.ImpactID = OO.ImpactID where OO.ImpactID = " & cboImpact.SelectedValue

                ds = db.ExecuteDataSet(CommandType.Text, sql)

                If ds.Tables(0).Select("ObjectiveID = " & gridItem("ObjectiveID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped to selected Impact"
                    btnImage.Visible = True

                End If

            End If

        End If

    End Sub

    Private Sub radOutputs_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radObjectives.NeedDataSource

        If cboImpact.SelectedIndex > 0 Then

            Dim sql As String = "SELECT O.ObjectiveID, O.Description FROM tblObjectives O inner join tblImpactObjectives OO on OO.ObjectiveID = O.ObjectiveID "
            sql &= " inner join luImpact Ob on Ob.ImpactID = OO.ImpactID where OO.ImpactID = " & cboImpact.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        radObjectives.DataSource = Session("mtObjectives")

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Process completed successfully", MessageTypeEnum.Information)

        Else

            ShowMessage("Error while saving...", MessageTypeEnum.Information)

        End If

    End Sub

    Private Sub cboImpact_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboImpact.SelectedIndexChanged

        If cboImpact.SelectedIndex > 0 Then

            Dim sql As String = "SELECT O.ObjectiveID, O.Description FROM tblObjectives O inner join tblImpactObjectives OO on OO.ObjectiveID = O.ObjectiveID "
            sql &= " inner join luImpact Ob on Ob.ImpactID = OO.ImpactID where OO.ImpactID = " & cboImpact.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        LoadGrid()

    End Sub
End Class