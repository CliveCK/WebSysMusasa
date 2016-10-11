Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI
Imports System.IO

Public Class TrainingDocuments
    Inherits System.Web.UI.Page

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

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

            If Not IsNothing(Request.QueryString("id")) Then

                Dim objLookup As New BusinessLogic.CommonFunctions

                With cboOrganization

                    .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name").Tables(0)
                    .DataValueField = "OrganizationID"
                    .DataTextField = "Name"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                    .SelectedIndex = 0

                End With

            End If

            LoadGrid()

        End If

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

            LoadFile(FileID)

        End If

    End Sub

    Private Sub LoadFile(ByVal FileID As Long)

        Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objFiles

            If .Retrieve(FileID) Then

                txtAuthor.Text = .Author
                txtDescription.Text = .Description
                txtTitle.Text = .Title
                If Not IsNothing(cboOrganization.Items.FindByText(.AuthorOrganization)) Then cboOrganization.SelectedItem.Text = .AuthorOrganization

            End If

        End With


    End Sub

    Private Sub LoadGrid()

        Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "SELECT * FROM tblFiles F inner join tblDocumentObjects DO on F.FileID = DO.DocumentID inner join luObjectTypes O on"
        sql &= " O.ObjectTypeID = DO.ObjectTypeID WHERE O.Description = 'Training' AND DO.ObjectID = " & IIf(IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("id"))), objUrlEncoder.Decrypt(Request.QueryString("id")), 0)

        With radFileListing

            .DataSource = objFiles.GetFiles(sql)
            .DataBind()

            ViewState("mTrainings") = .DataSource

        End With

    End Sub

    Private Sub cmdUpload_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click

        If IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("id"))) Then

            If fuTraining.HasFile Then

                Dim ObjectTypeID As Integer = db.ExecuteScalar(CommandType.Text, "SELECT TOP 1 ObjectTypeID FROM luObjectTypes WHERE Description = 'Training'")

                Try

                    Dim FilePath As String = IO.Path.GetFileName(fuTraining.FileName)
                    Dim Ext As String = IO.Path.GetExtension(FilePath)

                    fuTraining.SaveAs(Server.MapPath("~/FileUploads/") & FilePath)

                    Save(FilePath, Ext, ObjectTypeID)

                Catch ex As Exception

                    log.Error(ex)
                    ShowMessage("Error while uplading file...", MessageTypeEnum.Error)

                End Try

            Else

                ShowMessage("No file selected...", MessageTypeEnum.Error)

            End If

            LoadGrid()

        Else

            ShowMessage("No staff member has been selected/saved yet!...", MessageTypeEnum.Warning)

        End If

    End Sub

    Private Sub Save(ByVal FilePath As String, ByVal Ext As String, ByVal ObjectTypeID As Long)

        Dim FileType As Long = db.ExecuteScalar(CommandType.Text, "SELECT TOP 1 FileTypeID FROM luFileTypes where Description = 'TrainingUploads'")

        Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objFiles

            .FileID = 0
            .FileDate = Now
            .FileTypeID = FileType
            .Title = txtTitle.Text
            .Author = txtAuthor.Text
            .Description = txtDescription.Text
            .FilePath = FilePath
            .FileExtension = Ext
            .AuthorOrganization = cboOrganization.SelectedItem.Text

            If .Save() Then

                Dim objObjects As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objObjects

                    .ObjectTypeID = ObjectTypeID
                    .ObjectID = objUrlEncoder.Decrypt(Request.QueryString("id"))
                    .DocumentID = objFiles.FileID

                    If .Save() Then

                        ShowMessage("File uploaded successfully...", MessageTypeEnum.Information)

                    Else

                        ShowMessage("File upload failed...", MessageTypeEnum.Error)

                    End If

                End With

            Else

                ShowMessage("Operation failed!...", MessageTypeEnum.Error)

            End If

        End With

    End Sub

    Private Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click

        Response.Redirect("~/TrainingDetails.aspx?id=" & Request.QueryString("id"))

    End Sub

    Private Sub radFileListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radFileListing.NeedDataSource

        radFileListing.DataSource = DirectCast(ViewState("mTrainings"), DataTable)

    End Sub

End Class