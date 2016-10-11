Imports Microsoft.Practices.EnterpriseLibrary.Data

Partial Public Class CustomFieldsBulkAssign
    Inherits System.Web.UI.UserControl

    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database
    Public Event Message(ByVal msg As String)

    Public Property ObjectType() As String
        Get
            Return ViewState("EntiyType")
        End Get
        Set(ByVal value As String)
            ViewState("EntiyType") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        db = New DatabaseProviderFactory().Create("ConnectionString")

        If Not Page.IsPostBack Then

            LoadUsedFields()
            load_templates()

        End If

    End Sub

    Protected Sub cboFieldName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFieldName.SelectedIndexChanged

        If cboFieldName.SelectedIndex > -1 AndAlso cboFieldName.SelectedValue <> "" Then

            LoadFieldValues(cboFieldName.SelectedItem.Text)

        End If

    End Sub

    Public Sub LoadUsedFields()

        db = New DatabaseProviderFactory().Create("ConnectionString")

        Dim sql As String = String.Empty
        sql &= "select distinct isnull(pt.TemplateName + ' > ', '') + pf.FieldName AS Template_FieldName, " & vbCrLf
        sql &= "    isnull(pt.TemplateName,'') + char(177) + isnull(pf.FieldName,'') + char(177) + cast(isnull(pt.DisplayIndex,'') as varchar) + char(177) + isnull(pt.FieldType,'') + char(177) + cast(isnull(pt.CustomFieldID,'') as varchar) as FieldData " & vbCrLf
        sql &= "from CustomField_ObjectData pf  " & vbCrLf
        sql &= "	inner join temp_print p on pf.ObjectID = p.ID and pf.ObjectType = '" & ObjectType & "' " & vbCrLf
        sql &= "	left outer join CustomField_Fields pt on pf.CustomFieldID = pt.CustomFieldID " & vbCrLf
        sql &= "order by Template_FieldName " & vbCrLf

        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then

            With cboFieldName

                .DataTextField = "Template_FieldName"
                .DataValueField = "FieldData"
                .DataSource = ds
                .DataBind()

                .Items.Add(New ListItem("", ""))
                .SelectedIndex = .Items.Count - 1

            End With

        End If

    End Sub

    Private Sub LoadFieldValues(ByVal FieldName As String)

        Dim sql As String = String.Empty
        sql &= "select distinct pf.FieldValue1 " & vbCrLf
        sql &= "from CustomField_ObjectData pf  " & vbCrLf
        sql &= "	inner join temp_print p on pf.ObjectID = p.ID and pf.ObjectType = '" & ObjectType & "' " & vbCrLf
        sql &= "	left outer join CustomField_Fields pt on pf.CustomFieldID = pt.CustomFieldID " & vbCrLf
        sql &= "where isnull(pt.TemplateName + ' > ', '') + pf.FieldName = '" & FieldName.Replace("'", "''") & "' " & vbCrLf
        sql &= "order by FieldValue1 " & vbCrLf

        Dim ds As DataSet = BusinessLogic.CustomFields.DataHelper.ExecuteDataset(db, sql)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then

            chkFilterByValue.Checked = ds.Tables(0).Rows.Count > 0
            cboFieldValues.Enabled = ds.Tables(0).Rows.Count > 0

            With cboFieldValues

                .DataTextField = "FieldValue1"
                .DataValueField = "FieldValue1"
                .DataSource = ds
                .DataBind()

                .Items.Add(New ListItem("", ""))
                .SelectedIndex = .Items.Count - 1

            End With

        End If

    End Sub

    Private Sub load_templates()

        Dim objProject As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)
        Dim templateTable As DataTable = objProject.Get_Templates(True)

        cboTemplates.Items.Clear()

        cboTemplates.Items.Add(New ListItem("", ""))

        For i As Integer = 0 To templateTable.Rows.Count - 1

            Dim dr As DataRow = templateTable.Rows(i)
            cboTemplates.Items.Add(New ListItem(dr(0), dr(0)))

        Next

    End Sub

    Protected Sub chkFilterByValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterByValue.CheckedChanged

        cboFieldValues.Enabled = chkFilterByValue.Checked

    End Sub

    Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

        Dim sql As String = String.Empty
        sql &= "declare @UserID int; " & vbCrLf
        sql &= " " & vbCrLf
        sql &= "select @UserID = " & CookiesWrapper.thisUserID & ";" & vbCrLf
        sql &= " " & vbCrLf

        If ObjectType = "D" Then
            sql &= "with writeaccessdocs as ( " & vbCrLf
            sql &= "	select document_id from documents_users where EditAllowed = 1 and [UserID] = @UserID " & vbCrLf
            sql &= "	union " & vbCrLf
            sql &= "	select document_id from documents_groups  " & vbCrLf
            sql &= "		where EditAllowed = 1 and GroupID in (select b.GroupID from UserGroups b where b.[UserID] = @UserID) " & vbCrLf
            sql &= ") " & vbCrLf
        End If

        sql &= "update pf set FieldValue1 = '" & txtNewValue.Text.Replace("'", "''") & "' " & vbCrLf
        sql &= "from CustomField_ObjectData pf  " & vbCrLf
        sql &= "	inner join temp_print p on pf.Project_ID = p.ID and pf.ObjectType = '" & ObjectType & "' " & vbCrLf
        sql &= "	left outer join CustomField_Fields pt on pf.CustomFieldID = pt.OwnID " & vbCrLf
        sql &= "where isnull(pt.TemplateName + ' > ', '') + pf.FieldName = '" & cboFieldName.SelectedItem.Text.Replace("'", "''") & "' " & vbCrLf
        sql &= "	and p.UserID = @UserID " & vbCrLf

        If ObjectType = "D" Then
            sql &= "	and p.id in (select w.document_id from writeaccessdocs w)"
        End If

        If chkFilterByValue.Checked Then
            sql &= "	and isnull(ltrim(rtrim(pf.FieldValue1)),'') = '" & cboFieldValues.Text.Trim.Replace("'", "''") & "' " & vbCrLf
        End If

        Dim field() As String = cboFieldName.SelectedValue.Split(Chr(177))

        Dim TemplateName As String = field(0).Replace("'", "''")
        Dim FieldName As String = field(1).Replace("'", "''")

        Dim DisplayIndex As String = field(2)
        If Not IsNumeric(DisplayIndex) Then DisplayIndex = 0

        Dim FieldType As String = field(3).Replace("'", "''")

        Dim CustomFieldID As String = field(4)
        If Not IsNumeric(CustomFieldID) Then CustomFieldID = 0

        sql &= vbCrLf & vbCrLf
        If ObjectType = "D" Then
            sql &= "; " & vbCrLf & vbCrLf
            sql &= "with writeaccessdocs as ( " & vbCrLf
            sql &= "	select document_id from documents_users where EditAllowed = 1 and [UserID] = @UserID " & vbCrLf
            sql &= "	union " & vbCrLf
            sql &= "	select document_id from documents_groups  " & vbCrLf
            sql &= "		where EditAllowed = 1 and GroupID in (select b.GroupID from UserGroups b where b.[UserID] = @UserID) " & vbCrLf
            sql &= ") " & vbCrLf
        End If

        sql &= "insert into CustomField_ObjectData (Project_ID, FieldName, FieldValue1, FieldType, DisplayIndex, ObjectType, CustomFieldID) " & vbCrLf
        sql &= "select p.ID, nullif('" & FieldName & "',''), '" & txtNewValue.Text.Replace("'", "''") & "', nullif('" & FieldType & "',''),  " & vbCrLf
        sql &= "	nullif(" & DisplayIndex & ",0), nullif('" & ObjectType & "',''), nullif(" & CustomFieldID & ",0) " & vbCrLf
        sql &= "from temp_print p  " & vbCrLf
        sql &= "where not exists ( " & vbCrLf
        sql &= "	select * from CustomField_ObjectData pf  " & vbCrLf
        sql &= "	where pf.ObjectType='" & ObjectType & "' and pf.Project_ID = p.ID   " & vbCrLf
        sql &= "		and (  " & vbCrLf
        sql &= "			pf.CustomFieldID = nullif(" & CustomFieldID & ",0) " & vbCrLf
        sql &= "			or pf.FieldName = nullif('" & FieldName & "','') and pf.FieldType = nullif('" & FieldType & "','') " & vbCrLf
        sql &= "		) " & vbCrLf
        sql &= ") and p.UserID = @UserID " & vbCrLf

        If ObjectType = "D" Then
            sql &= "	and p.id in (select w.document_id from writeaccessdocs w)"
        End If

        sql &= vbCrLf & vbCrLf
        If ObjectType = "D" Then
            sql &= "; " & vbCrLf
            sql &= "with writeaccessdocs as ( " & vbCrLf
            sql &= "	select document_id from documents_users where EditAllowed = 1 and [UserID] = @UserID " & vbCrLf
            sql &= "	union " & vbCrLf
            sql &= "	select document_id from documents_groups  " & vbCrLf
            sql &= "		where EditAllowed = 1 and GroupID in (select b.GroupID from UserGroups b where b.[UserID] = @UserID) " & vbCrLf
            sql &= ") " & vbCrLf
        End If
        sql &= "insert into [CustomField_ObjectTemplates] (TemplateName, ID, OwnerType)" & vbCrLf
        sql &= "select '" & TemplateName & "', p.ID, '" & ObjectType & "'" & vbCrLf
        sql &= "from temp_print p  " & vbCrLf
        sql &= "where p.UserID = @UserID and not exists (select * from CustomField_ObjectTemplates where TemplateName = '" & TemplateName & "' and id = p.id and OwnerType = '" & ObjectType & "')" & vbCrLf
        If ObjectType = "D" Then
            sql &= "	and p.id in (select w.document_id from writeaccessdocs w)"
        End If

        If BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, sql) Then

            LoadFieldValues(cboFieldName.SelectedItem.Text)
            RaiseEvent Message("Custom field values updated successfully...")

        Else

            RaiseEvent Message("Failed to update custom field values: " & BusinessLogic.CustomFields.DataHelper.ErrorMessage)

        End If

    End Sub

    Private Sub cmdAssignCustomFieldTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAssignCustomFieldTemplate.Click

        If cboTemplates.Text <> "" Then

            Dim sql As String = String.Empty
            sql &= "insert into [CustomField_ObjectTemplates] (TemplateName, ID, OwnerType)" & vbCrLf
            sql &= "select '" & cboTemplates.Text.Replace("'", "''") & "', p.ID, '" & ObjectType & "'" & vbCrLf
            sql &= "from temp_print p  " & vbCrLf
            sql &= "where p.UserID = " & CookiesWrapper.thisUserID & " and not exists (select * from CustomField_ObjectTemplates where TemplateName = '" & cboTemplates.Text.Replace("'", "''") & "' and id = p.id and OwnerType = '" & ObjectType & "')" & vbCrLf

            If BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, sql) Then

                Dim objCF As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)
                objCF.UpdateObjectsUsingTemplate()

                RaiseEvent Message("Custom field values updated successfully...")

            Else

                RaiseEvent Message("Failed to update custom field values: " & BusinessLogic.CustomFields.DataHelper.ErrorMessage)

            End If

        End If

    End Sub

End Class