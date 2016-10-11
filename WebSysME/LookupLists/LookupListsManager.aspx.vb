Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class LookupListsManager
    Inherits System.Web.UI.Page

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        log.Error(Message)
        lblError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        log.Error(Message)
        lblError.Text = Message.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        lblError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            LoadLookupLists()

        End If

    End Sub

    Private Function LoadLookupListXml() As DataSet

        Dim ds As New DataSet

        Try

            ds.ReadXml(Server.MapPath("LookupLists.xml"))

            If System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory() & "LookupLists\LookupLists.xml") Then

                Dim ApplicationSpecificLookupList As String = System.AppDomain.CurrentDomain.BaseDirectory() & "LookupLists\LookupLists.xml"

                If My.Computer.FileSystem.FileExists(ApplicationSpecificLookupList) Then

                    Dim dsApplicationLists As New DataSet

                    dsApplicationLists.ReadXml(ApplicationSpecificLookupList)

                    ds.Merge(dsApplicationLists)

                End If

            End If

            Dim CompanySpecificLookupLists As String = Server.MapPath("~/LookupLists/LookupLists.xml")

            If My.Computer.FileSystem.FileExists(CompanySpecificLookupLists) Then

                Dim dsCompanyLists As New DataSet

                dsCompanyLists.ReadXml(CompanySpecificLookupLists)

                ds.Merge(dsCompanyLists)

            End If


        Catch ex As Exception

            ShowMessage(New Exception("Error loading configuration data.", ex), MessageTypeEnum.Error)

        End Try

        Return ds

    End Function

    Private Function GetLookupListTable() As DataSet

        Dim ds As DataSet = LoadLookupListXml()

        ViewState("LookupListTable") = ds

        Return ds

    End Function

    Private Sub LoadLookupLists()

        Dim ds As DataSet = GetLookupListTable()

        Dim dv As DataView = ds.Tables(0).DefaultView

        dv.RowFilter = "Display = 1"
        dv.Sort = "Editable DESC, ListName ASC"
        Dim dt As DataTable = dv.ToTable(True, "ListName", "Listdescription", "Display", "Editable")

        With dgLookupLists
            .DataSource = dt
            .DataBind()
        End With

    End Sub

    Private Sub dgLookupLists_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLookupLists.ItemCommand

        Select Case e.CommandName

            Case "Select"

                If e.Item.Cells(2).Text = 1 Then
                    cmdAdd.Enabled = True
                    cmdAdd.Visible = True
                Else
                    cmdAdd.Visible = False
                End If

                txtListName.Text = CType(e.Item.Cells(1).Controls(1), Label).Text

                PopulateGrid(txtListName.Text)

        End Select

    End Sub

    Sub PopulateGrid(ByVal TemplateName As String, Optional ByVal EditItem As Integer = -1, Optional ByVal WithInsert As Boolean = False)

        Dim ds As DataSet = CType(ViewState("LookupListTable"), DataSet)

        Dim dtLookup As DataTable = ds.Tables("LookupList")
        Dim dv As DataView = dtLookup.DefaultView

        dv.Sort = "ListName"

        Dim i As Integer = dv.Find(TemplateName)

        If i < 0 Then

            ShowMessage("Unable to locate configuration data for '" & TemplateName & "'. Operation aborted!!!", MessageTypeEnum.Error)
            Exit Sub

        End If

        Dim drv As DataRowView = dv(i)

        If Not drv Is Nothing Then

            With dgData

                .AutoGenerateColumns = False

                Dim HiddenColumns() As String = CStr(drv.Item("HiddenColumns")).Split(",")
                Dim NonEditableColumns() As String = CStr(drv.Item("NonEditableColumns")).Split(",")
                Dim RequiredFields() As String

                If drv.Row.Table.Columns.Contains("RequiredFields") Then

                    RequiredFields = CStr(drv.Item("RequiredFields")).Split(",")

                End If

                Dim lookupData As DataSet = db.ExecuteDataSet(CommandType.Text, drv.Item("SelectQuery"))
                Dim dt As DataTable = lookupData.Tables(0)

                If dtLookup.Columns.Contains("Filter") AndAlso drv.Item("Filter") = 1 Then
                    pnlFilter.Visible = True

                    With lstColumns
                        .DataTextField = "ColumnName"
                        .DataValueField = "ColumnName"
                        .DataSource = dt.Columns
                        .DataBind()
                    End With
                Else
                    pnlFilter.Visible = False
                End If

                If WithInsert Then

                    Dim dr As DataRow = dt.NewRow
                    dt.Rows.Add(dr)

                End If

                For index As Integer = 0 To dt.Columns.Count - 1
                    Dim dCol As DataColumn = dt.Columns(index)

                    .Columns.Add(CreateBoundColumn(dCol))

                    If Array.IndexOf(HiddenColumns, dCol.ColumnName) > -1 Then
                        .Columns(Array.IndexOf(HiddenColumns, dCol.ColumnName) + 2).ItemStyle.Width = New System.Web.UI.WebControls.Unit(0)
                        CType(.Columns(Array.IndexOf(HiddenColumns, dCol.ColumnName) + 2), BoundColumn).Visible = True
                    End If

                    If Array.IndexOf(NonEditableColumns, dCol.ColumnName) > -1 Then
                        CType(.Columns(Array.IndexOf(NonEditableColumns, dCol.ColumnName) + 2), BoundColumn).ReadOnly = True
                    End If

                    If WithInsert Then
                        .EditItemIndex = .Items.Count
                    Else
                        .EditItemIndex = EditItem
                    End If

                Next

                If drv.Item("Editable") = 0 Then
                    .Columns(0).Visible = False
                End If

                If drv.Item("DeleteAllowed") = 0 Then
                    .Columns(1).Visible = False
                End If

                .DataSource = lookupData
                .DataBind()

            End With

        End If

    End Sub

    Function CreateBoundColumn(ByRef c As DataColumn) As DataGridColumn

        Dim column As New BoundColumn

        column.DataField = c.ColumnName
        column.HeaderText = c.ColumnName.Replace("_", " ")

        Return column

    End Function

    Private Sub dgData_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgData.EditCommand

        PopulateGrid(txtListName.Text, e.Item.ItemIndex)

    End Sub

    Private Sub dgData_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgData.CancelCommand

        PopulateGrid(txtListName.Text)

    End Sub

    Private Sub dgData_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgData.DeleteCommand

        Dim ds As DataSet = LoadLookupListXml()

        Dim dv As DataView = ds.Tables("LookupList").DefaultView

        dv.Sort = "ListName"

        Dim i As Integer = dv.Find(txtListName.Text)

        If i < 0 Then

            ShowMessage("Unable to locate configuration data for '" & txtListName.Text & "'. Operation aborted!!!", MessageTypeEnum.Error)
            Exit Sub

        End If

        Dim drv As DataRowView = dv(i)

        If Not drv Is Nothing Then

            'Dim UpdateQuery As String = drv.Item("UpdateQuery")
            'Dim InsertQuery As String = drv.Item("InsertQuery")
            Dim DeleteQuery As String = drv.Item("DeleteQuery")

            Dim HiddenColumns() As String = CStr(drv.Item("HiddenColumns")).Split(",")
            Dim NonEditableColumns() As String = CStr(drv.Item("NonEditableColumns")).Split(",")

            Dim lookupData As DataSet = db.ExecuteDataSet(CommandType.Text, drv.Item("SelectQuery"))

            For iCol As Integer = 0 To lookupData.Tables(0).Columns.Count - 1
                Dim dCol As DataColumn = lookupData.Tables(0).Columns(iCol)

                'editable columns will have a control in them
                'noneditable columns will have no control in them

                'dgData:_ctl4:_ctl2
                If (Array.IndexOf(NonEditableColumns, dCol.ColumnName) > -1) Then
                    'this is not an editable column
                    If lookupData.Tables(0).Rows.Count - 1 < e.Item.DataSetIndex Then
                        If DeleteQuery.IndexOf("%" & (iCol + 2) & "%") > -1 Then DeleteQuery = DeleteQuery.Replace("%" & (iCol + 2) & "%", "'" & lookupData.Tables(0).Rows(e.Item.DataSetIndex)(dCol.ColumnName) & "'")
                    Else
                        If DeleteQuery.IndexOf("%" & (iCol + 2) & "%") > -1 Then DeleteQuery = DeleteQuery.Replace("%" & (iCol + 2) & "%", "'" & lookupData.Tables(0).Rows(e.Item.DataSetIndex)(dCol.ColumnName) & "'")
                    End If
                Else
                    'this is an editable column
                    Dim value1 As String = Request.Form.Item(dgData.UniqueID & ":_ctl" & e.Item.DataSetIndex + 2 & ":_ctl" & (iCol + 2))
                    Dim value2 As String = Request.Form.Item(dgData.UniqueID & "$ctl" & CInt(e.Item.DataSetIndex + 2).ToString("#00") & "$ctl" & CInt(iCol + 2).ToString("#00"))

                    If value1 = "" AndAlso value2 <> "" Then value1 = value2

                    DeleteQuery = DeleteQuery.Replace("%" & (iCol + 2) & "%", "'" & value1 & "'")

                    If lookupData.Tables(0).Rows.Count - 1 < e.Item.DataSetIndex Then

                        If DeleteQuery.IndexOf("%" & (iCol + 2) & "%") > -1 Then DeleteQuery = DeleteQuery.Replace("%" & (iCol + 2) & "%", "'" & value1 & "'")

                    End If
                End If

                If lookupData.Tables(0).Rows.Count - 1 < e.Item.DataSetIndex Then
                    'delete
                    Try
                        db.ExecuteNonQuery(CommandType.Text, DeleteQuery)
                        ShowMessage("Data deleted successfully...", MessageTypeEnum.Information)
                    Catch ex As Exception
                        ShowMessage(ex, MessageTypeEnum.Error)
                    End Try
                Else
                    'delete
                    Try
                        db.ExecuteNonQuery(CommandType.Text, DeleteQuery)
                        ShowMessage("Data deleted successfully...", MessageTypeEnum.Information)
                    Catch ex As Exception
                        ShowMessage(ex, MessageTypeEnum.Error)
                    End Try
                End If

            Next

        End If

        PopulateGrid(txtListName.Text)

    End Sub

    Private Sub dgData_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgData.ItemCreated

        ' process data rows only (skip the header, footer etc.)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            ' get a reference to the Button of this row,
            '  and add the javascript confirmation
            Dim btnDelete As Button = CType(e.Item.Cells(1).Controls(0), Button)
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this record?');")
        End If

    End Sub

    Private Sub dgData_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgData.UpdateCommand

        'now need to generate an update command

        Dim ds As DataSet = LoadLookupListXml()

        Dim dv As DataView = ds.Tables("LookupList").DefaultView

        dv.Sort = "ListName"

        Dim i As Integer = dv.Find(txtListName.Text)

        If i < 0 Then

            ShowMessage("Failed to get configuration details for '" & txtListName.Text & "'. Operation aborted!!!", MessageTypeEnum.Error)
            Exit Sub

        End If

        Dim drv As DataRowView = dv(i)

        If Not drv Is Nothing Then

            Dim UpdateQuery As String = drv.Item("UpdateQuery")
            Dim InsertQuery As String = drv.Item("InsertQuery")

            Dim HiddenColumns() As String = CStr(drv.Item("HiddenColumns")).Split(",")
            Dim NonEditableColumns() As String = CStr(drv.Item("NonEditableColumns")).Split(",")

            Dim lookupData As DataSet = db.ExecuteDataSet(CommandType.Text, drv.Item("SelectQuery"))

            For iCol As Integer = 0 To lookupData.Tables(0).Columns.Count - 1

                Dim dCol As DataColumn = lookupData.Tables(0).Columns(iCol)

                'editable columns will have a control in them
                'noneditable columns will have no control in them

                'dgData:_ctl4:_ctl2
                If (Array.IndexOf(NonEditableColumns, dCol.ColumnName) > -1) Then

                    'this is not an editable column
                    'If lookupData.Tables(0).Rows.Count - 1 <= e.Item.DataSetIndex Then

                    If lookupData.Tables(0).Rows.Count - 1 < e.Item.DataSetIndex Then
                        If InsertQuery.IndexOf("%" & (iCol + 2) & "%") > -1 Then InsertQuery = InsertQuery.Replace("%" & (iCol + 2) & "%", "'" & lookupData.Tables(0).Rows(e.Item.DataSetIndex)(dCol.ColumnName) & "'")
                    Else
                        If UpdateQuery.IndexOf("%" & (iCol + 2) & "%") > -1 Then UpdateQuery = UpdateQuery.Replace("%" & (iCol + 2) & "%", "'" & lookupData.Tables(0).Rows(e.Item.DataSetIndex)(dCol.ColumnName) & "'")
                    End If

                Else

                    'this is an editable column
                    'If lookupData.Tables(0).Rows.Count - 1 <= e.Item.DataSetIndex Then
                    If lookupData.Tables(0).Rows.Count - 1 < e.Item.DataSetIndex Then

                        If InsertQuery.IndexOf("%" & (iCol + 2) & "%") > -1 Then

                            Dim value1 As String = Request.Form.Item(dgData.UniqueID & ":_ctl" & e.Item.DataSetIndex + 2 & ":_ctl" & (iCol + 2))
                            Dim value2 As String = Request.Form.Item(dgData.UniqueID & "$ctl" & CInt(e.Item.DataSetIndex + 2).ToString("#00") & "$ctl" & CInt(iCol + 2).ToString("#00"))

                            If value1 = "" AndAlso value2 <> "" Then value1 = value2

                            InsertQuery = InsertQuery.Replace("%" & (iCol + 2) & "%", "'" & value1 & "'")

                            'End If

                        End If

                    Else

                        If UpdateQuery.IndexOf("%" & (iCol + 2) & "%") > -1 Then

                            'If Request.Form.Item(dgData.UniqueID & ":_ctl" & e.Item.DataSetIndex + 2 & ":_ctl" & (iCol + 2)) = "" Then

                            '    UpdateQuery = ""

                            'Else

                            Dim value1 As String = Request.Form.Item(dgData.UniqueID & ":_ctl" & e.Item.DataSetIndex + 2 & ":_ctl" & (iCol + 2))
                            Dim value2 As String = Request.Form.Item(dgData.UniqueID & "$ctl" & CInt(e.Item.DataSetIndex + 2).ToString("#00") & "$ctl" & CInt(iCol + 2).ToString("#00"))

                            If value1 = "" AndAlso value2 <> "" Then value1 = value2

                            UpdateQuery = UpdateQuery.Replace("%" & (iCol + 2) & "%", "'" & value1 & "'")

                            'End If

                        End If

                    End If

                End If

                'If lookupData.Tables(0).Rows.Count - 1 <= e.Item.DataSetIndex Then

            Next

            If lookupData.Tables(0).Rows.Count - 1 < e.Item.DataSetIndex Then

                'insert
                'Response.Write(InsertQuery)

                Try

                    If InsertQuery <> "" Then

                        db.ExecuteNonQuery(CommandType.Text, InsertQuery)
                        ShowMessage("Data saved successfully...", MessageTypeEnum.Information)

                    Else

                        ShowMessage("System failed to insert data into the database. Malformed query.", MessageTypeEnum.Error)

                    End If

                Catch ex As Exception

                    ShowMessage(ex, MessageTypeEnum.Error)

                End Try

            Else

                'update
                'Response.Write(UpdateQuery)

                Try

                    If UpdateQuery <> "" Then

                        db.ExecuteNonQuery(CommandType.Text, UpdateQuery)
                        ShowMessage("Data saved successfully...", MessageTypeEnum.Information)

                    Else

                        ShowMessage("System failed to insert data into the database. Malformed query.", MessageTypeEnum.Error)

                    End If

                Catch ex As Exception

                    ShowMessage(ex, MessageTypeEnum.Error)

                End Try

            End If

        End If

        PopulateGrid(txtListName.Text)

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        PopulateGrid(txtListName.Text, , True)

    End Sub

    Private Sub dgLookupLists_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgLookupLists.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem _
        Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.EditItem Then
            If CType(e.Item.DataItem, DataRowView).Row.Item("Editable") = 0 Then
                With CType(e.Item.Cells(1).FindControl("lblListName"), Label)
                    .Font.Bold = False
                End With
            End If
        End If
    End Sub

    Private Sub cmdAddFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddFilter.Click
        Dim Filter As String = ""

        Select Case cboFilterOperation.SelectedValue
            Case "="
                Filter = txtFilterColumn.Text & " = '" & txtFilterValue.Text & "'"
            Case ">"
                Filter = txtFilterColumn.Text & " > '" & txtFilterValue.Text & "'"
            Case ">="
                Filter = txtFilterColumn.Text & " >= '" & txtFilterValue.Text & "'"
            Case "<"
                Filter = txtFilterColumn.Text & " < '" & txtFilterValue.Text & "'"
            Case "<="
                Filter = txtFilterColumn.Text & " <= '" & txtFilterValue.Text & "'"
                'Case "INSTR"
        End Select

        lstFilters.Items.Add(New ListItem(Filter, Filter))

        txtFilter.Text &= IIf(txtFilter.Text = "", "", " AND ") & Filter

    End Sub

    Private Sub lstColumns_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstColumns.SelectedIndexChanged
        If lstColumns.SelectedIndex > -1 Then
            txtFilterColumn.Text = lstColumns.SelectedValue
        End If
    End Sub

End Class