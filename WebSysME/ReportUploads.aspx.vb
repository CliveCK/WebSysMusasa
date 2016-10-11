Imports System.IO

Public Class ReportUploads
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


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

            If Request.QueryString.Count > 0 Then

                If Not IsNothing(Request.QueryString("ReportID")) Then

                    RetrieveReport()

                Else

                    ShowMessage("Report submission details not found", MessageTypeEnum.Error)

                End If

            End If

        End If

    End Sub

    Private Sub RetrieveReport()

        Dim objReportSubmission As New BusinessLogic.ReportSumbissionTracking(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Try

            With objReportSubmission

                If .Retrieve(objUrlEncoder.Decrypt(Request.QueryString("ReportID"))) Then

                    pnlReport.BackColor = Drawing.Color.LightGray
                    lblTitle.Text = .ReportTitle
                    lblExpectedDate.Text = .ExpectedDate
                    lblStatus.Text = IIf(.SubmissionStatus = True, "Submitted", IIf(.ActualSubmissionDate < Now, "Overdue", "Pending"))
                    lblStatus.ForeColor = IIf(.ActualSubmissionDate < Now AndAlso .SubmissionStatus = False, Drawing.Color.Red, IIf(.SubmissionStatus = False, Drawing.Color.Brown, Drawing.Color.Green))
                    cmdSave.Enabled = IIf(.FileID > 0 AndAlso .SubmissionStatus = True, False, True)

                End If

            End With

        Catch ex As Exception

            ShowMessage("An errr occured. Contact your administrator!", MessageTypeEnum.Error)

        End Try

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If fuUpload.HasFile Then

            Try

                Dim FilePath As String = Path.GetFileName(fuUpload.FileName)
                Dim Ext As String = Path.GetExtension(FilePath)

                fuUpload.SaveAs(Server.MapPath("~/FileUploads/") & FilePath)

                Save(FilePath, Ext)

            Catch ex As Exception

                log.Error(ex)
                ShowMessage("Error while uplading file...", MessageTypeEnum.Error)

            End Try

        Else

            ShowMessage("Please select a file before saving!!", MessageTypeEnum.Error)

        End If

    End Sub

    Private Function Save(ByVal FilePath As String, ByVal FileExt As String) As Boolean

        Try

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objReportSubmission As New BusinessLogic.ReportSumbissionTracking(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim ds As DataSet

            With objFiles

                .FileID = 0
                .FileDate = Now
                ds = objFiles.GetFileTypeByDescription("Report")
                If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    .FileTypeID = ds.Tables(0).Rows(0)("FileTypeID")

                Else

                    ShowMessage("FileType error: Cannot proceed!", MessageTypeEnum.Error)
                    Return False
                    Exit Function

                End If

                objReportSubmission.Retrieve(objUrlEncoder.Decrypt(Request.QueryString("ReportID")))
                .Title = objReportSubmission.ReportTitle
                .Author = objReportSubmission.Author
                .Description = objReportSubmission.Comments
                .FilePath = FilePath
                .FileExtension = FileExt
                .AuthorOrganization = objReportSubmission.OrganizationID

                If .Save Then

                    objReportSubmission.FileID = .FileID
                    objReportSubmission.SubmissionStatus = 1
                    objReportSubmission.ActualSubmissionDate = Now

                    If objReportSubmission.Save Then

                        RetrieveReport()
                        ShowMessage("File uploaded successfully...", MessageTypeEnum.Information)

                        Return True

                    End If

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
End Class