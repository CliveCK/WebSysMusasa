Imports Telerik.Web.UI
Imports System.IO

Public Class ReportSubmission
    Inherits System.Web.UI.Page

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

            LoadGrid("All")

        End If

    End Sub

    Private Sub LoadGrid(ByVal Status As String)

        Dim objReportSubmission As New BusinessLogic.ReportSumbissionTracking(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = ""

        Select Case Status

            Case "All"
                sql = "select O.Name As Organization, R.* from tblReportSubmissionTracking R inner join tblOrganization O on R.OrganizationID = O.OrganizationID"

            Case "Submitted"
                sql = "select O.Name As Organization, R.* from tblReportSubmissionTracking R inner join tblOrganization O on R.OrganizationID = O.OrganizationID WHERE SubmissionStatus = 1 AND FileID IS NOT NULL"

            Case "Overdue"
                sql = "select O.Name As Organization, R.* from tblReportSubmissionTracking R inner join tblOrganization O on R.OrganizationID = O.OrganizationID WHERE ActualSubmissionDate < getdate() AND SubmissionStatus = 0"

            Case "Pending"
                sql = "select O.Name As Organization, R.* from tblReportSubmissionTracking R inner join tblOrganization O on R.OrganizationID = O.OrganizationID WHERE SubmissionStatus = 0"

        End Select

        With radReports

            .DataSource = objReportSubmission.GetReportSumbissionTracking(sql).Tables(0)
            .DataBind()

            ViewState("SubmissionReports") = .DataSource

        End With

    End Sub

    Private Sub cboStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboStatus.SelectedIndexChanged

        LoadGrid(cboStatus.SelectedValue)

    End Sub

    Private Sub DownloadFile(ByVal FilePath As String)

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

    End Sub

    Private Sub radReports_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radReports.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radReports.Items(index)

            Dim FileID As Long = item("FileID").Text
            Dim ReportTrackingSubmissionID As Long = item("ReportSubmissionTrackingID").Text

            Select Case e.CommandName

                Case "Download"

                    Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    If objFiles.Retrieve(FileID) Then

                        DownloadFile(objFiles.FilePath)

                    End If

                Case "Upload"

                    Response.Redirect("~/ReportUploads.aspx?ReportID=" & objUrlEncoder.Encrypt(ReportTrackingSubmissionID))

            End Select

        End If

    End Sub

    Private Sub radReports_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radReports.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item
            Dim cmdDownload As LinkButton = DirectCast(gridItem("cmdDownload").Controls(0), LinkButton)
            Dim cmdUpload As LinkButton = DirectCast(gridItem("cmdUpload").Controls(0), LinkButton)

            If gridItem("SubmissionStatus").Text = True AndAlso Not IsNothing(gridItem("FileID")) Then
                cmdDownload.Visible = True
                cmdUpload.Visible = False
            Else
                cmdDownload.Visible = False
                cmdUpload.Visible = True
            End If

        End If

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/ReportTracking.aspx")

    End Sub
End Class