Imports System.IO
Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class GrantProposalDetailsControl
    Inherits System.Web.UI.UserControl

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboAction

                .DataSource = objLookup.Lookup("luGrantActions", "GrantActionID", "Description")
                .DataTextField = "Description"
                .DataValueField = "GrantActionID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboStatus

                .DataSource = objLookup.Lookup("luGrantProposalStatus", "StatusID", "Description")
                .DataTextField = "Description"
                .DataValueField = "StatusID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadGrantProposal(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadGrantProposal(ByVal GrantProposalID As Long) As Boolean

        Try

            Dim objGrantProposal As New GrantProposal(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGrantProposal

                If .Retrieve(GrantProposalID) Then

                    txtGrantProposalID.Text = .GrantProposalID
                    If Not IsNothing(cboAction.Items.FindByValue(.ActionID)) Then cboAction.SelectedValue = .ActionID
                    txtDonorName.Text = .DonorName
                    txtProposalTitle.Text = .ProposalTitle
                    txtProposedProjectName.Text = .ProposedProjectName
                    txtComments.Text = .Comments

                    'Load Grant Statuses
                    LoadGrantStatus(.GrantProposalID)

                    'Load any file attachments
                    LoadFiles(.GrantProposalID)

                    ShowMessage("GrantProposal loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load GrantProposal", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub LoadGrantStatus(ByVal GrantProposalID As Long)

        Dim objGrantStatus As New GrantProposalStatus(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radStatus

            .DataSource = objGrantStatus.GetGrantProposalStatus(GrantProposalID)
            .DataBind()

            ViewState("GrantStatus") = .DataSource

        End With

    End Sub

    Private Sub LoadFiles(ByVal GrantProposalID As Long)

        Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "Select FileID, Title, FilePath from tblFiles F inner join tblDocumentObjects D on D.DocumentID = F.FileID "
        sql &= "inner join luObjectTypes O on O.ObjectTypeID = D.ObjectTypeID  where O.Description = 'Grants' "
        sql &= " And ObjectID = " & GrantProposalID

        With radAttachments

            .DataSource = objFiles.GetFiles(sql)
            .DataBind()

            ViewState("GrantAttachments") = .DataSource

        End With

    End Sub

    Public Function Save() As Boolean

        Try

            Dim objGrantProposal As New GrantProposal(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGrantProposal

                .GrantProposalID = IIf(IsNumeric(txtGrantProposalID.Text), txtGrantProposalID.Text, 0)
                If cboAction.SelectedIndex > -1 Then .ActionID = cboAction.SelectedValue
                .DonorName = txtDonorName.Text
                .ProposalTitle = txtProposalTitle.Text
                .ProposedProjectName = txtProposedProjectName.Text
                .Comments = txtComments.Text

                If .Save Then

                    'Save the Grant proposal status
                    If radDate.SelectedDate.HasValue AndAlso cboStatus.SelectedValue > 0 Then

                        Dim objGrantStatus As New GrantProposalStatus(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                        With objGrantStatus

                            .GrantProposalID = objGrantProposal.GrantProposalID
                            .StatusDate = radDate.SelectedDate
                            .StatusID = cboStatus.SelectedValue

                            If Not .CheckGrantStatus(.GrantProposalID) Then
                                .Save()
                            End If

                        End With

                    End If

                    'Save any file that might have been attached
                    SaveFile(.GrantProposalID)

                    If Not IsNumeric(txtGrantProposalID.Text) OrElse Trim(txtGrantProposalID.Text) = 0 Then txtGrantProposalID.Text = .GrantProposalID
                    LoadGrantProposal(.GrantProposalID)
                    ShowMessage("GrantProposal saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Could not save details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub SaveFile(ByVal ObjectID As Long)

        If fuUpload.HasFile Then

            Try

                Dim FilePath As String = Path.GetFileName(fuUpload.FileName)
                Dim Ext As String = Path.GetExtension(FilePath)

                fuUpload.SaveAs(Server.MapPath("~/FileUploads/") & FilePath)

                Save(FilePath, Ext, ObjectID)

            Catch ex As Exception

                log.Error(ex)

            End Try

        End If

    End Sub

    Public Function Save(ByVal FilePath As String, ByVal FileExt As String, ByVal ObjectID As Long) As Boolean

        Try

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFiles

                .FileID = 0
                .FileDate = Now
                .FileTypeID = 0
                .Title = fuUpload.FileName
                .Author = CookiesWrapper.thisUserFullName
                .Description = "Proposal file"
                .FilePath = FilePath
                .FileExtension = FileExt
                .AuthorOrganization = ""
                .ApplySecurity = True

                If .Save() Then

                    Dim objObjects As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objObjects

                        .DocumentObjectID = 0
                        .DocumentID = objFiles.FileID
                        .ObjectID = ObjectID
                        .ObjectTypeID = .GetObjectTypeIDByDescription("Grants")

                        .Save()

                    End With

                End If

            End With


        Catch ex As Exception
            log.Error(ex)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtGrantProposalID.Text = ""
        If Not IsNothing(cboAction.Items.FindByValue("")) Then
            cboAction.SelectedValue = ""
        ElseIf Not IsNothing(cboAction.Items.FindByValue(0)) Then
            cboAction.SelectedValue = 0
        Else
            cboAction.SelectedIndex = -1
        End If
        txtDonorName.Text = ""
        txtProposalTitle.Text = ""
        txtProposedProjectName.Text = ""
        txtComments.Text = ""

    End Sub

    Private Sub radAttachments_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radAttachments.NeedDataSource

        radAttachments.DataSource = DirectCast(ViewState("GrantAttachments"), DataSet)

    End Sub

    Private Sub radStatus_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radStatus.NeedDataSource

        radStatus.DataSource = DirectCast(ViewState("GrantStatus"), DataSet)

    End Sub

    Private Sub radStatus_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radStatus.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radStatus.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objHealthCenter As New BusinessLogic.GrantProposalStatus(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objHealthCenter

                        .GrantProposalStatusID = Server.HtmlDecode(e.CommandArgument)

                        If .Delete() Then

                            ShowMessage("Status deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radAttachments_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radAttachments.ItemCommand

        If e.CommandName = "Download" Then

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radAttachments.Items(index)
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

    End Sub
End Class

