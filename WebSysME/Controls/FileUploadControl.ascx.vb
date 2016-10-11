Imports BusinessLogic
Imports System.IO
Imports Telerik.Web.UI

Public Class FileUploadControl
    Inherits System.Web.UI.UserControl

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Private PermissionMsg As String = "?"

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

            Session("Files") = Nothing

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            LoadGrid(objFiles:=objFiles)

            Dim objLookup As New BusinessLogic.CommonFunctions

            With drpFileType

                .DataSource = objLookup.Lookup("luFileTypes", "FileTypeID", "Description").Tables(0)
                .DataValueField = "FileTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboObject

                .DataSource = objLookup.Lookup("luObjectTypes", "ObjectTypeID", "Description").Tables(0)
                .DataValueField = "ObjectTypeID"
                .DataTextField = "Description"
                .DataBind()

            End With

            ucFileMapping.SelectedOptionsCaption = "Mapped"
            ucFileMapping.AvailableOptionsCaption = "Available"

            ucComplementaryListboxes.SelectedOptionsCaption = "Authorized"

            ucComplementaryListboxes.AvailableOptionsCaption = "UnAuthorised"


            If Not IsNothing(Request.QueryString("id")) Then

                LoadFiles(objUrlEncoder.Decrypt(Request.QueryString("id")))

                LoadAvailableUserPermmisions(objUrlEncoder.Decrypt(Request.QueryString("id")), 1)
                LoadAssignedUserPermmisions(objUrlEncoder.Decrypt(Request.QueryString("id")), 1)

                Dim sql As String = DertemineObjectTypeSQL(cboObject.SelectedItem.Text)
                LoadAvailableObjects(sql, objUrlEncoder.Decrypt(Request.QueryString("id")))
                LoadAssignedObjects(sql, objUrlEncoder.Decrypt(Request.QueryString("id")))

            Else

                LoadAvailableUserPermmisions(0, 1)
                LoadAssignedUserPermmisions(0, 1)

                Dim sql As String = DertemineObjectTypeSQL(cboObject.SelectedItem.Text)
                Dim FileID As Long = IIf(IsNumeric(txtFileID.Text), txtFileID.Text, 0)
                LoadAvailableObjects(sql, FileID)
                LoadAssignedObjects(sql, FileID)

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If Not IsNumeric(txtFileID.Text) Then

            If My.Settings.EnforceFileUpload Then

                If ucFileMapping.SelectedOptions.Items.Count > 0 Then

                    SaveFile()

                Else

                    ShowMessage("File mapping is enforced. You must map the file before saving!!", MessageTypeEnum.Error)

                End If

            Else

                SaveFile()

            End If

        Else

            Dim objDoc As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            If My.Settings.EnforceFileUpload Then

                If objDoc.CheckIsMapped(txtFileID.Text) > 0 Or ucFileMapping.SelectedOptions.Items.Count > 0 Then

                    Save(txtFilePath.Text, Path.GetExtension(txtFilePath.Text))

                Else

                    ShowMessage("File mapping is enforced. You must map the file before saving!!", MessageTypeEnum.Error)

                End If

            Else

                If objDoc.CheckIsMapped(txtFileID.Text) > 0 Then

                    Save(txtFilePath.Text, Path.GetExtension(txtFilePath.Text))

                Else

                    ShowMessage("File mapping is enforced. You must map the file before saving!!", MessageTypeEnum.Error)

                End If

            End If

        End If

    End Sub

    Private Sub SaveFile()

        If fuFileUpload.HasFile Then

            Try

                Dim FilePath As String = Path.GetFileName(fuFileUpload.FileName)
                Dim Ext As String = Path.GetExtension(FilePath)

                fuFileUpload.SaveAs(Server.MapPath("~/FileUploads/") & FilePath)

                Save(FilePath, Ext)

            Catch ex As Exception

                log.Error(ex)
                ShowMessage("Error while uplading file...", MessageTypeEnum.Error)

            End Try

        Else

            ShowMessage("Please select a file before saving!!", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadFiles(ByVal FileID As Long) As Boolean

        Try

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFiles

                If .Retrieve(FileID) Then

                    txtFileID.Text = .FileID
                    radDate.SelectedDate = .FileDate
                    If Not IsNothing(drpFileType.Items.FindByValue(.FileTypeID)) Then drpFileType.SelectedValue = .FileTypeID
                    txtTitle.Text = .Title
                    txtAuthor.Text = .Author
                    txtDescription.Text = .Description
                    txtFilePath.Text = .FilePath
                    txtAuthorOrg.Text = .AuthorOrganization
                    cbxApplySecurity.Checked = .ApplySecurity

                    If .ApplySecurity Then

                        pnlPermissions.Visible = True

                    End If

                    ShowMessage("Files details loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load file details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function Save(ByVal FilePath As String, ByVal FileExt As String) As Boolean

        Try

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFiles

                .FileID = IIf(IsNumeric(txtFileID.Text), txtFileID.Text, 0)
                .FileDate = radDate.SelectedDate
                .FileTypeID = drpFileType.SelectedValue
                .Title = txtTitle.Text
                .Author = txtAuthor.Text
                .Description = txtDescription.Text
                .FilePath = FilePath
                .FileExtension = FileExt
                .AuthorOrganization = txtAuthorOrg.Text
                .ApplySecurity = cbxApplySecurity.Checked

                If .Save Then

                    If Not IsNumeric(txtFileID.Text) OrElse Trim(txtFileID.Text) = 0 Then txtFileID.Text = .FileID

                    If ucFileMapping.SelectedOptions.Items.Count > 0 Then SaveMapping(ucFileMapping, cboObject, .FileID)

                    If cbxApplySecurity.Checked Then

                        If SavePermissions(ucComplementaryListboxes, .FileID, cboLevelOfSecurity.SelectedValue) Then

                            PermissionMsg = "?File permissions applied successfully..."

                        Else

                            PermissionMsg = "!However permissions failed to apply..."

                        End If

                    End If

                    LoadGrid(objFiles)
                    ShowMessage("Files uploaded successfully..." & Replace(Replace(PermissionMsg, "?", ""), "!", ""), IIf(PermissionMsg.Contains("?"), MessageTypeEnum.Information, MessageTypeEnum.Warning))

                    Return True

                Else

                    ShowMessage("Error while uploading File to server", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub LoadGrid(ByVal objFiles As BusinessLogic.Files)

        With radFileListing

            .DataSource = objFiles.GetAllFiles()
            .DataBind()

            Session("Files") = .DataSource

        End With

    End Sub

    Public Sub Clear()

        txtFileID.Text = ""
        radDate.Clear()
        If Not IsNothing(drpFileType.Items.FindByValue("")) Then
            drpFileType.SelectedValue = ""
        ElseIf Not IsNothing(drpFileType.Items.FindByValue(0)) Then
            drpFileType.SelectedValue = 0
        Else
            drpFileType.SelectedIndex = -1
        End If
        txtAuthor.Text = ""
        txtDescription.Text = ""
        txtAuthorOrg.Text = ""
        txtFilePath.Text = ""
        txtTitle.Text = ""

    End Sub

    Private Sub radFileListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radFileListing.ItemCommand

        If e.CommandName = "Download" Then

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radFileListing.Items(index)
            Dim FilePath As String

            FilePath = Server.HtmlDecode(item("FilePath").Text)

            With Response

                .Clear()
                .ClearContent()
                .ClearHeaders()
                .BufferOutput = True

            End With

            If File.Exists(Request.PhysicalApplicationPath & FilePath) Or File.Exists(Server.MapPath("~/FileUploads/" & FilePath)) Then

                Dim oFileStream As FileStream
                Dim fileLen As Long

                Try

                    oFileStream = File.Open(Server.MapPath("~/FileUploads/" & FilePath), FileMode.Open, FileAccess.Read, FileShare.None)
                    fileLen = oFileStream.Length

                    Dim ByteFile(fileLen - 1) As Byte

                    If fileLen > 0 Then
                        oFileStream.Read(ByteFile, 0, oFileStream.Length - 1)
                        oFileStream.Close()

                        With Response

                            .AddHeader("Content-Disposition", "attachment; filename=" & FilePath.Replace(" ", "_"))
                            .ContentType = "application/octet-stream"
                            .BinaryWrite(ByteFile)
                            '.WriteFile(Server.MapPath("~/FileUploads/" & FilePath))

                            objFiles.UpdateDownloadCount(Server.HtmlDecode(item("FileID").Text))

                            .End()
                            HttpContext.Current.ApplicationInstance.CompleteRequest()

                        End With

                    Else
                        log.Error("Empty File...")
                    End If

                Catch ex As Exception

                End Try

            Else

                ShowMessage("Error: File not found!!!", MessageTypeEnum.Error)

            End If
        End If

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radFileListing.Items(index1)
            Dim FileID As Integer

            FileID = Server.HtmlDecode(item1("FileID").Text)

            LoadFiles(FileID)

        End If

    End Sub

    Private Sub radFileListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radFileListing.NeedDataSource

        radFileListing.DataSource = Session("Files")

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Clear()

    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click

        Try

            If IsNumeric(txtFileID) Then

                File.Delete(txtFilePath.Text)

                If Not File.Exists(txtFilePath.Text) Then

                    Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objFiles

                        .FileID = txtFileID.Text

                        If .Delete Then

                            ShowMessage("File deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

                End If

            End If

        Catch ex As Exception

            ShowMessage("An error occured! Contact your administrator. Error: " & ex.Message, MessageTypeEnum.Error)

        End Try

    End Sub

    Sub LoadAvailableUserPermmisions(ByVal FileID As Long, ByVal LevelOfSecurity As Long)

        Dim objPermmisions As New Global.SysPermissionsManager.FilePermissions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(FileID) Then

            If FileID >= 0 Then

                Dim ds As DataSet = objPermmisions.GetForbiddenUsersOrOrganizations(FileID, LevelOfSecurity)

                ucComplementaryListboxes.AvailableOptions.DataSource = ds

                ucComplementaryListboxes.AvailableOptions.DataTextField = "Name"

                ucComplementaryListboxes.AvailableOptions.DataValueField = "ObjectID"

                ucComplementaryListboxes.AvailableOptions.DataBind()

            End If

        End If

    End Sub

    Sub LoadAssignedUserPermmisions(ByVal FileID As Long, ByVal LevelOfSecurity As Long)

        Dim objPermmisions As New Global.SysPermissionsManager.FilePermissions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(FileID) Then

            If FileID >= 0 Then

                Dim ds As DataSet = objPermmisions.GetPermittedUsersOrOrganizations(FileID, LevelOfSecurity)

                ucComplementaryListboxes.SelectedOptions.DataSource = ds

                ucComplementaryListboxes.SelectedOptions.DataTextField = "Name"

                ucComplementaryListboxes.SelectedOptions.DataValueField = "ObjectID"

                ucComplementaryListboxes.SelectedOptions.DataBind()

            End If

        End If

    End Sub

    Private Function SavePermissions(ByVal ucComplementaryListBox As ComplementaryListboxes, ByVal FileID As Long, ByVal LevelOfSecurity As Long) As Boolean

        Dim objPermissions As New Global.SysPermissionsManager.FilePermissions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Try
            objPermissions.Revoke(FileID, LevelOfSecurity)

            'save selected
            For i As Integer = 0 To ucComplementaryListBox.SelectedOptions.Items.Count - 1

                ucComplementaryListBox.SelectedOptions.SelectedIndex = i

                If ucComplementaryListBox.SelectedOptions.SelectedValue <> 0 Then

                    objPermissions.SaveDetail(ucComplementaryListBox.SelectedOptions.SelectedValue, FileID, LevelOfSecurity)

                End If

            Next

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub cboLevelOfSecurity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLevelOfSecurity.SelectedIndexChanged

        LoadAssignedUserPermmisions(IIf(IsNumeric(txtFileID.Text), txtFileID.Text, 0), cboLevelOfSecurity.SelectedValue)
        LoadAvailableUserPermmisions(IIf(IsNumeric(txtFileID.Text), txtFileID.Text, 0), cboLevelOfSecurity.SelectedValue)

    End Sub

    Private Sub cbxApplySecurity_CheckedChanged(sender As Object, e As EventArgs) Handles cbxApplySecurity.CheckedChanged

        pnlPermissions.Visible = cbxApplySecurity.Checked

    End Sub

#Region "Mapping"

    Public Function DertemineObjectTypeSQL(ByVal Type As String) As String

        Dim sql As String = ""

        Select Case Type

            Case "Staff"
                sql = "SELECT StaffID As ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblStaffMembers"

            Case "Group"
                sql = "SELECT GroupID As ObjectID, GroupName As Name, Description FROM tblGroups"

            Case "Organization"
                sql = "SELECT OrganizationID As ObjectID, Name FROM tblOrganization"

            Case "Project"
                sql = "SELECT Project As ObjectID, Name, Acronym FROM tblProjects"

            Case "School"
                sql = "SELECT SchoolID As ObjectID, Name FROM tblSchools"

            Case "HealthCenter"
                sql = "SELECT HealthCenterID as ObjectID, Name, Description FROM tblHealthCenters"

            Case "Household"
                sql = "SELECT BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries WHERE Suffix = 1"

            Case "Individual"
                sql = "SELECT BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries"

            Case "Activity"
                sql = "SELECT AP.ID as ObjectID,  A.Description As Name, CAST([Start] AS Date) [Start], CAST([End] AS Date) [End], "
                sql &= " ISNULL(FirstName, '') + ' ' + ISNULL(Surname, '') As FullName ,Completed, AP.Description as [Description] from Appointments AP "
                sql &= " inner join tblActivities A on AP.ActivityID = A.ActivityID inner join tblStaffMembers S on S.StaffID = AP.UserID"

        End Select

        Return sql

    End Function

    Sub LoadAvailableObjects(ByVal sql As String, ByVal FileID As Long)

        Dim objObjects As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Dim ds As DataSet = objObjects.GetAvailableObjects(sql, cboObject.SelectedValue, FileID)

        ucFileMapping.AvailableOptions.DataSource = ds

        ucFileMapping.AvailableOptions.DataTextField = "Name"

        ucFileMapping.AvailableOptions.DataValueField = "ObjectID"

        ucFileMapping.AvailableOptions.DataBind()

    End Sub

    Sub LoadAssignedObjects(ByVal sql As String, ByVal FileID As Long)

        Dim objObjects As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Dim ds As DataSet = objObjects.GetMappedObjects(sql, cboObject.SelectedValue, FileID)

        ucFileMapping.SelectedOptions.DataSource = ds

        ucFileMapping.SelectedOptions.DataTextField = "Name"

        ucFileMapping.SelectedOptions.DataValueField = "ObjectID"

        ucFileMapping.SelectedOptions.DataBind()

    End Sub

    Private Function SaveMapping(ByVal ucComplementaryListBox As ComplementaryListboxes, ByVal cboObject As DropDownList, ByVal FileID As Long) As Boolean

        Dim objObjects As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objObjects

            .DocumentID = FileID
            .ObjectID = ucFileMapping.SelectedOptions.SelectedValue
            .ObjectTypeID = cboObject.SelectedValue

            Try
                .Revoke()

                'save selected
                For i As Integer = 0 To ucFileMapping.SelectedOptions.Items.Count - 1

                    ucFileMapping.SelectedOptions.SelectedIndex = i

                    If ucFileMapping.SelectedOptions.SelectedValue <> 0 Then

                        .SaveDetail()

                    End If

                Next

                Return True

            Catch ex As Exception
                Return False
            End Try

        End With

    End Function

    Private Sub cboObject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboObject.SelectedIndexChanged

        Dim sql As String = DertemineObjectTypeSQL(cboObject.SelectedItem.Text)
        Dim FileID As Long = IIf(IsNumeric(txtFileID.Text), txtFileID.Text, 0)
        LoadAvailableObjects(sql, FileID)
        LoadAssignedObjects(sql, FileID)

    End Sub

#End Region
End Class