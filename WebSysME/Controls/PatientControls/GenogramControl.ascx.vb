Imports System.IO

Public Class GenogramControl
    Inherits System.Web.UI.UserControl

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

            If IsNumeric(CookiesWrapper.PatientID) AndAlso CookiesWrapper.PatientID > 0 Then

                LoadGenogram(CookiesWrapper.PatientID)

            End If

        End If

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If fupload.HasFile Then

            Try

                Dim FilePath As String = Path.GetFileName(fupload.FileName)
                Dim Ext As String = Path.GetExtension(FilePath)

                fupload.SaveAs(Server.MapPath("~/GenogramImages/") & FilePath)

                Save(FilePath)

            Catch ex As Exception

                log.Error(ex)
                ShowMessage("Error while uplading file...", MessageTypeEnum.Error)

            End Try

        Else

            ShowMessage("Please select a file before saving!!", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub LoadGenogram(ByVal PatientID As Long)

        Dim objGenogram As New BusinessLogic.Genogram(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objGenogram

            If .RetrievePatient(PatientID) Then

                txtGenogramID.Text = .GenogramID
                txtFilePath.Text = .FilePath
                txtPatientID.Text = .PatientID

                mImage.ImageUrl = "~/GenogramImages/" & .FilePath

            Else

                ShowMessage("Failed to retrieve Genogram", MessageTypeEnum.Error)

            End If

        End With

    End Sub

    Private Function Save(ByVal FilePath As String) As Boolean

        Try

            Dim objGenogram As New BusinessLogic.Genogram(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGenogram

                .GenogramID = IIf(IsNumeric(txtGenogramID.Text), txtGenogramID.Text, 0)
                .PatientID = IIf(IsNumeric(txtPatientID.Text), txtPatientID.Text, CookiesWrapper.PatientID)
                .FilePath = FilePath

                If .Save Then

                    If Not IsNumeric(txtGenogramID.Text) OrElse Trim(txtGenogramID.Text) = 0 Then txtGenogramID.Text = .GenogramID
                    LoadGenogram(.PatientID)
                    ShowMessage("Files uploaded successfully...", MessageTypeEnum.Information)

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
End Class