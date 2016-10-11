Imports Telerik.Web.UI
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class KeyAreaIndicatorControl
    Inherits System.Web.UI.UserControl

    Dim ds As DataSet
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

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

            With cboKeyArea

                .DataSource = objLookup.Lookup("luKeyAreas", "KeyAreaID", "Description").Tables(0)
                .DataValueField = "KeyAreaID"
                .DataTextField = "Description"
                .DataBind()

            End With

        End If

    End Sub

    Protected Function GetSelectedIndicatorIDs() As String

        Dim IndicatorIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radIndicators.SelectedItems
            IndicatorIDArray.Add(gridRow.Item("ObjectID").Text.ToString())
        Next

        Return String.Join(",", IndicatorIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Indicator() As String = GetSelectedIndicatorIDs.Split(",")

        If Indicator.Length > 0 Then

            For i As Long = 0 To Indicator.Length - 1

                Dim objIndicators As New BusinessLogic.KeyAreaIndicators(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objIndicators

                    .KeyAreaID = cboKeyArea.SelectedValue
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

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Mapping successful...", MessageTypeEnum.Information)

        Else

            ShowMessage("Failed to map indicators...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub LoadGrid()

        Dim sql As String = "SELECT * FROM tblKeyAreaIndicators WHERE KeyAreaID =" & cboKeyArea.SelectedValue

        ds = db.ExecuteDataSet(CommandType.Text, sql)

        With radIndicators

            .DataSource = db.ExecuteDataSet(CommandType.Text, "SELECT IndicatorID as ObjectID, Name, Definition, ResponsibleParty FROM tblIndicators").Tables(0)
            .DataBind()

            ViewState("KeyAreaIndicators") = .DataSource

        End With

    End Sub

    Private Sub radIndicators_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radIndicators.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radIndicators.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objIndicators As New BusinessLogic.KeyAreaIndicators(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objIndicators

                        .KeyAreaID = cboKeyArea.SelectedValue
                        .IndicatorID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If


    End Sub

    Private Sub radIndicators_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radIndicators.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

            If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 AndAlso ds.Tables(0).Select("IndicatorID = " & gridItem("ObjectID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already selected..."
                btnImage.Visible = True

            End If

        End If

    End Sub

    Private Sub radIndicators_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radIndicators.NeedDataSource

        radIndicators.DataSource = DirectCast(ViewState("KeyAreaIndicators"), DataTable)

    End Sub

End Class