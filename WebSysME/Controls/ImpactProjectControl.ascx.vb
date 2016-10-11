Imports Telerik.Web.UI
Imports Microsoft.Practices.EnterpriseLibrary.Data
Public Class ImpactProjectControl
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

            With radProjects

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, "SELECT Project As ProjectID, Name FROM tblObjectives")

                Session("mtProjects") = ds
                .DataSource = ds

                .DataBind()

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedProjectIDs() As String

        Dim ProjectIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radProjects.SelectedItems
            ProjectIDArray.Add(gridRow.Item("ProjectID").Text.ToString())
        Next

        Return String.Join(",", ProjectIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Project() As String = GetSelectedProjectIDs.Split(",")

        If Project.Length > 0 Then

            For i As Long = 0 To Project.Length - 1

                Dim objImpactProjects As New BusinessLogic.ImpactProjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objImpactProjects

                    .ImpactID = cboImpact.SelectedValue
                    .ProjectID = Project(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub radProjects_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radProjects.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radProjects.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objImpactProjects As New BusinessLogic.ImpactProjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objImpactProjects

                        .ImpactID = cboImpact.SelectedValue
                        .ProjectID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radProjects_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radProjects.ItemDataBound

        If cboImpact.SelectedIndex > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

                Dim sql As String = "SELECT O.Project As ProjectID, O.Name FROM tblProjects O inner join tblImpactProjects OO on OO.ProjectID = O.Project "
                sql &= " inner join luImpact Ob on Ob.ImpactID = OO.ImpactID where OO.ImpactID = " & cboImpact.SelectedValue

                ds = db.ExecuteDataSet(CommandType.Text, sql)

                If ds.Tables(0).Select("ProjectID = " & gridItem("ProjectID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped to selected Impact"
                    btnImage.Visible = True

                End If

            End If

        End If

    End Sub

    Private Sub radProjects_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radProjects.NeedDataSource

        If cboImpact.SelectedIndex > 0 Then

            Dim sql As String = "SELECT O.Project As ProjectID, O.Name FROM tblProjects O inner join tblImpactProjects OO on OO.ProjectID = O.Project "
            sql &= " inner join luImpact Ob on Ob.ImpactID = OO.ImpactID where OO.ImpactID = " & cboImpact.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        radProjects.DataSource = Session("mtProjects")

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

            Dim sql As String = "SELECT O.Project As ProjectID, O.Name FROM tblProjects O inner join tblImpactProjects OO on OO.ProjectID = O.Project "
            sql &= " inner join luImpact Ob on Ob.ImpactID = OO.ImpactID where OO.ImpactID = " & cboImpact.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        LoadGrid()

    End Sub
End Class