Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class IndicatorCategoryPage
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

            With cboIndicatorCategory

                .DataSource = objLookup.Lookup("luIndicatorTypes", "IndicatorTypeID", "Description").Tables(0)
                .DataValueField = "IndicatorTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            LoadGrid()

        End If

    End Sub

    Private Sub LoadGrid()

        Try
            Dim db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

            With radIndicators

                Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblIndicators")

                Session("Indicatorsl") = ds
                .DataSource = ds

                .DataBind()

            End With

        Catch ex As Exception
            log.Error(ex)
        End Try

    End Sub

    Protected Function GetSelectedIndicatorIDs() As String

        Dim IndicatorIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radIndicators.SelectedItems
            IndicatorIDArray.Add(gridRow.Item("IndicatorID").Text.ToString())
        Next

        Return String.Join(",", IndicatorIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Indicator() As String = GetSelectedIndicatorIDs.Split(",")

        If Indicator.Length > 0 Then

            For i As Long = 0 To Indicator.Length - 1

                Dim objOutput As New BusinessLogic.IndicatorCategory(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objOutput

                    .IndicatorTypeID = cboIndicatorCategory.SelectedValue
                    .IndicatorID = Indicator(i)

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Private Sub radIndicators_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radIndicators.ItemDataBound

        If cboIndicatorCategory.SelectedIndex > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                If ds.Tables(0).Select("IndicatorID = " & gridItem("IndicatorID").Text).Length > 0 Then

                    Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                    chkbx.Enabled = False
                    chkbx.ToolTip = "Already mapped to selected Type"

                End If

            End If

        End If

    End Sub

    Private Sub radIndicators_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radIndicators.NeedDataSource

        radIndicators.DataSource = Session("Indicatorsl")

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            ShowMessage("Process completed successfully", MessageTypeEnum.Information)

        Else

            ShowMessage("Error while saving...", MessageTypeEnum.Information)

        End If

    End Sub

    Private Sub cboIndicatorCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboIndicatorCategory.SelectedIndexChanged

        If cboIndicatorCategory.SelectedIndex > 0 Then

            Dim sql As String = "SELECT * FROM tblIndicatorIndicatorType WHERE IndicatorTypeID = " & cboIndicatorCategory.SelectedValue

            ds = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        LoadGrid()

    End Sub
End Class