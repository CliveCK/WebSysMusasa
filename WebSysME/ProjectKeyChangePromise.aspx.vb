Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic
Imports Telerik.Web.UI

Public Class ProjectKeyChangePromise
    Inherits System.Web.UI.Page

    Private dsDocuments As DataSet
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboProjects

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

        Dim sql As String = "Select S.Description as StrategicObjective, KC.* FROM tblKeyChangePromises KC INNER JOIN tblStrategicObjectives S ON KC.StrategicObjectiveID = S.StrategicObjectiveID"
        ViewState("KeyChangePromiseID") = Nothing

        With radKeyChange

            .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
            .DataBind()

            ViewState("KeyChangePromiseID") = .DataSource

        End With

    End Sub

    Private Sub radKeyChange_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radKeyChange.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            If dsDocuments.Tables(0).Select("KeyChangePromiseID = " & gridItem("KeyChangePromiseID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already mapped..."

            End If

        End If

    End Sub

    Protected Function GetSelectedKeyChangePromiseIDs() As String

        Dim KeyChangePromiseIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radKeyChange.SelectedItems
            KeyChangePromiseIDArray.Add(gridRow.Item("KeyChangePromiseID").Text.ToString())
        Next

        Return String.Join(",", KeyChangePromiseIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim mObject() As String = GetSelectedKeyChangePromiseIDs.Split(",")

        Try

            If mObject.Length > 0 Then

                For i As Long = 0 To mObject.Length - 1

                    Dim objProjectKeyChangePromise As New BusinessLogic.ProjectKeyChangePromise(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objProjectKeyChangePromise

                        .ProjectID = cboProjects.SelectedValue
                        .KeyChangePromiseID = mObject(i)

                        If Not .Save Then

                            log.Error("Error saving...")
                            Return False

                        End If

                    End With

                Next

            End If

            Return True

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            log.Error(ex)
            Return False

        End Try


    End Function

    Private Sub radKeyChange_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radKeyChange.NeedDataSource

        radKeyChange.DataSource = DirectCast(ViewState("KeyChangePromiseID"), DataTable)

    End Sub

    Private Sub cboProjects_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProjects.SelectedIndexChanged

        If cboProjects.SelectedIndex > 0 Then

            Dim sql As String = "SELECT KCP.KeyChangePromiseID FROM tblProjectKeyChangePromise KCP "
            sql &= " WHERE ProjectID = " & cboProjects.SelectedValue

            dsDocuments = db.ExecuteDataSet(CommandType.Text, sql)

        End If

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            ShowMessage("Mapped successfully...", MessageTypeEnum.Information)

        Else

            ShowMessage("Error saving ...", MessageTypeEnum.Error)

        End If

    End Sub
End Class