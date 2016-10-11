Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class GroupAssociationsMapping
    Inherits System.Web.UI.Page

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

            With cboGroupAssociation

                .DataSource = objLookup.Lookup("tblGroupAssociations", "GroupAssociationID", "Association").Tables(0)
                .DataValueField = "GroupAssociationID"
                .DataTextField = "Association"
                .DataBind()

            End With

            With cboGroupType

                .DataSource = objLookup.Lookup("luGroupTypes", "GroupTypeID", "Description").Tables(0)
                .DataValueField = "GroupTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Protected Function GetSelectedGroupIDs() As String

        Dim GroupIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radGroups.SelectedItems
            GroupIDArray.Add(gridRow.Item("ObjectID").Text.ToString())
        Next

        Return String.Join(",", GroupIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Groups() As String = GetSelectedGroupIDs.Split(",")

        If Groups.Length > 0 Then

            For i As Long = 0 To Groups.Length - 1

                Dim objGroups As New BusinessLogic.GroupAssociationsGroups(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objGroups

                    .GroupAssociationID = cboGroupAssociation.SelectedValue
                    .GroupID = Groups(i)

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
            ShowMessage("Attendants saved successfully...", MessageTypeEnum.Information)

        Else

            ShowMessage("Failed to save attendants...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub LoadGrid()

        Dim sql As String = "SELECT * FROM tblGroupAssociationGroups WHERE GroupAssociationID = " & cboGroupAssociation.SelectedValue

        ds = db.ExecuteDataSet(CommandType.Text, sql)

        Dim sql1 As String = "SELECT GroupID as ObjectID, GroupName, GT.Description as GroupType, G.Description, GroupSize "
        sql1 &= "from tblGroups G inner join luGroupTypes GT On GT.GroupTypeID = G.GroupTypeID WHERE G.GroupTypeID = " & cboGroupType.SelectedValue

        With radGroups

            .DataSource = db.ExecuteDataSet(CommandType.Text, sql1).Tables(0)
            .DataBind()

            ViewState("tGroups1") = .DataSource

        End With

    End Sub

    Private Sub radGroups_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radGroups.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radGroups.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objGroups As New BusinessLogic.GroupAssociationsGroups(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objGroups

                        .GroupAssociationID = cboGroupAssociation.SelectedValue
                        .GroupID = Server.HtmlDecode(e.CommandArgument)

                        If .DeleteEntries() Then

                            LoadGrid()
                            ShowMessage("entry deselected successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If


    End Sub

    Private Sub radBeneficiaries_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radGroups.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

            If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 AndAlso ds.Tables(0).Select("GroupID = " & gridItem("ObjectID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already selected..."
                btnImage.Visible = True

            End If

        End If

    End Sub

    Private Sub radGroups_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radGroups.NeedDataSource

        radGroups.DataSource = DirectCast(ViewState("tGroups1"), DataTable)

    End Sub

    Private Sub cboGroupType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGroupType.SelectedIndexChanged

        LoadGrid()

    End Sub

End Class
