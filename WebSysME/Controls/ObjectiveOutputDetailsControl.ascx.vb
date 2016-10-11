﻿Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class OutputDetailsControl
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

            With cboObjectives

                .DataSource = objLookup.Lookup("tblObjectives", "ObjectiveID", "Description").Tables(0)
                .DataValueField = "ObjectiveID"
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

            With radOutputs

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, "SELECT OutputID, Description FROM tblOutput")

                Session("mOutputs") = ds
                .DataSource = ds

                .DataBind()

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedOutputIDs() As String

        Dim OutputIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radOutputs.SelectedItems
            OutputIDArray.Add(gridRow.Item("OutputID").Text.ToString())
        Next

        Return String.Join(",", OutputIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Outputs() As String = GetSelectedOutputIDs.Split(",")

        If Outputs.Length > 0 Then

            For i As Long = 0 To Outputs.Length - 1

                Dim objOutput As New BusinessLogic.ObjectiveOutputs(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objOutput

                    .ObjectiveID = cboObjectives.SelectedValue
                    .OutputID = Outputs(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub radOutputs_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radOutputs.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radOutputs.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objObjectiveOutput As New BusinessLogic.ObjectiveOutputs(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objObjectiveOutput

                        .ObjectiveID = cboObjectives.SelectedValue
                        .OutputID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radOutputs_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radOutputs.ItemDataBound

        If cboObjectives.SelectedIndex > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

                Dim sql As String = "SELECT O.OutputID, O.Description FROM tblOutput O inner join tblObjectiveOutputs OO on OO.OutputID = O.OutputID "
                sql &= " inner join tblObjectives Ob on Ob.ObjectiveID = OO.ObjectiveID where OO.ObjectiveID = " & cboObjectives.SelectedValue

                ds = db.ExecuteDataSet(CommandType.Text, sql)

                If ds.Tables(0).Select("OutputID = " & gridItem("OutputID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped to selected Objective"
                    btnImage.Visible = True

                End If

            End If

        End If

    End Sub

    Private Sub radOutputs_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radOutputs.NeedDataSource

        If cboObjectives.SelectedIndex > 0 Then

            Dim sql As String = "SELECT O.OutputID, O.Description FROM tblOutput O inner join tblObjectiveOutputs OO on OO.OutputID = O.OutputID "
            sql &= " inner join tblObjectives Ob on Ob.ObjectiveID = OO.ObjectiveID where OO.ObjectiveID = " & cboObjectives.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        radOutputs.DataSource = Session("mOutputs")

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Process completed successfully", MessageTypeEnum.Information)

        Else

            ShowMessage("Error while saving...", MessageTypeEnum.Information)

        End If

    End Sub

    Private Sub cboObjectives_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboObjectives.SelectedIndexChanged

        If cboObjectives.SelectedIndex > 0 Then

            Dim sql As String = "SELECT O.OutputID, O.Description FROM tblOutput O inner join tblObjectiveOutputs OO on OO.OutputID = O.OutputID "
            sql &= " inner join tblObjectives Ob on Ob.ObjectiveID = OO.ObjectiveID where OO.ObjectiveID = " & cboObjectives.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        LoadGrid()

    End Sub
End Class