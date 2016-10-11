Imports Telerik.Web.UI
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class PatientDocumentsDetailsControl
    Inherits System.Web.UI.UserControl

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ObjectTypeID As Integer = db.ExecuteScalar(CommandType.Text, "SELECT TOP 1 ObjectTypeID FROM luObjectTypes WHERE Description = 'Patient'")

        If Not IsNumeric(txtFileID.Text) Then

            If fuFileUpload.HasFile Then

                Try

                    Dim FilePath As String = Path.GetFileName(fuFileUpload.FileName)
                    Dim Ext As String = Path.GetExtension(FilePath)

                    fuFileUpload.SaveAs(Server.MapPath("~/FileUploads/") & FilePath)

                    Save(FilePath, Ext, ObjectTypeID)

                Catch ex As Exception

                    log.Error(ex)
                    ShowMessage("Error while uplading file...", MessageTypeEnum.Error)

                End Try

            Else

                ShowMessage("Please select a file before saving!!", MessageTypeEnum.Error)

            End If

        Else

            Save(txtFilePath.Text, Path.GetExtension(txtFilePath.Text), ObjectTypeID)

        End If

    End Sub

    Public Function LoadFiles(ByVal FileID As Long) As Boolean

        Try

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFiles

                If .Retrieve(FileID) Then

                    txtFileID.Text = .FileID
                    radDate.SelectedDate = .FileDate
                    txtFileType.Text = .FileTypeID
                    txtTitle.Text = .Title
                    txtAuthor.Text = .Author
                    txtDescription.Text = .Description
                    txtFilePath.Text = .FilePath

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

    Public Function Save(ByVal FilePath As String, ByVal FileExt As String, ByVal ObjectTypeID As Long) As Boolean

        Try

            Dim FileType As Long = db.ExecuteScalar(CommandType.Text, "SELECT TOP 1 FileTypeID FROM luFileTypes where Description = 'Patient Document'")
            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFiles

                .FileID = IIf(IsNumeric(txtFileID.Text), txtFileID.Text, 0)
                .FileDate = radDate.SelectedDate
                .FileTypeID = FileType
                .Title = txtTitle.Text
                .Author = txtAuthor.Text
                .Description = txtDescription.Text
                .FilePath = FilePath
                .FileExtension = FileExt

                If CookiesWrapper.PatientID Then

                    If .Save() Then

                        Dim objObjects As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                        With objObjects

                            .ObjectTypeID = ObjectTypeID
                            .ObjectID = CookiesWrapper.PatientID
                            .DocumentID = objFiles.FileID

                            If .Save() Then

                                ShowMessage("Document uploaded successfully...", MessageTypeEnum.Information)
                                LoadGrid()

                            Else

                                ShowMessage("Document upload failed...", MessageTypeEnum.Error)

                            End If

                        End With

                    Else

                        ShowMessage("Operation failed!...", MessageTypeEnum.Error)

                    End If

                Else

                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub LoadGrid()

        Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Dim sql As String = "SELECT F.* FROM tblFiles F inner join tblDocumentObjects O on O.DocumentID  = F.FileID "
        sql &= " inner join tblPatients P on P.PatientID = O.ObjectID "
        sql &= " inner join luObjectTypes OT on OT.ObjectTypeID = O.ObjectTypeID  WHERE OT.Description = 'Patient' "
        sql &= " AND P.PatientID = " & CookiesWrapper.PatientID
        ''sql &= " AND P.ProjectMeetingID = " & IIf(IsNumeric(txtProjectMeetingID.Text), txtProjectMeetingID.Text, 0)

        With radFileListing

            .DataSource = objFiles.GetFiles(sql)
            .DataBind()

            Session("wFiles") = .DataSource

        End With

    End Sub

    Public Sub Clear()

        txtFileID.Text = ""
        radDate.Clear()
        txtFileType.Text = ""
        txtAuthor.Text = ""
        txtDescription.Text = ""
        txtFilePath.Text = ""
        txtTitle.Text = ""

    End Sub

    Private Sub radFileListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radFileListing.ItemCommand

        If e.CommandName = "Download" Then

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

        radFileListing.DataSource = Session("wFiles")

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Clear()

    End Sub

End Class