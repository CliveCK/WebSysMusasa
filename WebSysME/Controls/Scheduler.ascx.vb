Imports System.IO
Imports Telerik.Web.UI
Imports Universal.CommonFunctions

Public Class Scheduler
    Inherits System.Web.UI.UserControl

    Private mUser As List(Of Integer)
    Private mActivity As List(Of Integer)
    Private FileID As Long = 0
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Protected ReadOnly Property Activity() As List(Of Integer)
        Get
            mActivity = New List(Of Integer)

            For Each Actitem As ListItem In lstActivity.Items

                If lstActivity.SelectedIndex > 0 Then

                    If Actitem.Selected Then

                        mActivity.Add(Actitem.Value)

                    End If

                End If

            Next

            Return mActivity

        End Get
    End Property

    Protected ReadOnly Property User() As List(Of Integer)
        Get
            mUser = New List(Of Integer)

            For Each item As ListItem In lstUsers.Items

                If lstUsers.SelectedIndex > 0 Then

                    If item.Selected Then

                        mUser.Add(item.Value)

                    End If

                End If

            Next

            Return mUser
        End Get
    End Property

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

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString
        UsersDataSource.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString
        ActivityDataSource.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim users As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "Select DISTINCT S.StaffID, StaffFullName FROM tblStaffMembers S inner join tblUserAccountProfileLink L on S.StaffID = L.StaffID"

        If Not Page.IsPostBack Then

            If Not IsNothing(Request.QueryString("staffid")) Then

                Dim objLookup As New BusinessLogic.CommonFunctions

                With lstUsers

                    .DataSource = users.GetFiles(sql)
                    .DataTextField = "StaffFullName"
                    .DataValueField = "StaffID"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With cboActivityCategory

                    .DataSource = objLookup.Lookup("tblActivityCategory", "ActivityCategoryID", "Description")
                    .DataTextField = "Description"
                    .DataValueField = "ActivityCategoryID"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With lstActivity

                    .DataSource = objLookup.Lookup("tblActivities", "ActivityID", "Description")
                    .DataTextField = "Description"
                    .DataValueField = "ActivityID"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With drpFileType

                    .DataSource = objLookup.Lookup("luFileTypes", "FileTypeID", "Description")
                    .DataTextField = "Description"
                    .DataValueField = "FileTypeID"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                RadScheduler1.Rebind()

            Else

                Dim objLookup As New BusinessLogic.CommonFunctions

                With lstUsers

                    .DataSource = users.GetFiles(Sql)
                    .DataTextField = "StaffFullName"
                    .DataValueField = "StaffID"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With cboActivityCategory

                    .DataSource = objLookup.Lookup("tblActivityCategory", "ActivityCategoryID", "Description")
                    .DataTextField = "Description"
                    .DataValueField = "ActivityCategoryID"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With lstActivity

                    .DataSource = objLookup.Lookup("tblActivities", "ActivityID", "Description")
                    .DataTextField = "Description"
                    .DataValueField = "ActivityID"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With drpFileType

                    .DataSource = objLookup.Lookup("luFileTypes", "FileTypeID", "Description")
                    .DataTextField = "Description"
                    .DataValueField = "FileTypeID"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

            End If

        End If

    End Sub

    Private Sub SqlDataSource1_Inserting(sender As Object, e As SqlDataSourceCommandEventArgs) Handles SqlDataSource1.Inserting

        Dim subject = e.Command.Parameters(0).Value
        Dim user = e.Command.Parameters(3).Value
        Dim activity = e.Command.Parameters(11).Value

        'Do not save if any of these parameters is null

        If subject Is Nothing OrElse user Is Nothing OrElse activity Is Nothing Then

            ShowMessage("Could not add activity! Please fill in all required information on your activities (Subject, User, Activity)", MessageTypeEnum.Error)
            e.Cancel = True

        End If

        If user.ToString() <> CookiesWrapper.StaffID Then

            ShowMessage("Could not add activity! Please pick your staff name. You cannot add as another person...", MessageTypeEnum.Error)
            e.Cancel = True

        End If
    End Sub

    Protected Sub CompletedStatusCheckBox_CheckedChanged(sender As Object, e As EventArgs)
        Dim CompletedStatusCheckBox As CheckBox = DirectCast(sender, CheckBox)
        'Find the appointment object to directly interact with it
        Dim appContainer As SchedulerAppointmentContainer = DirectCast(CompletedStatusCheckBox.Parent, SchedulerAppointmentContainer)
        Dim appointment As Appointment = appContainer.Appointment
        Dim appointmentToUpdate As Appointment = RadScheduler1.PrepareToEdit(appointment, RadScheduler1.EditingRecurringSeries)

        If appointmentToUpdate.Attributes("UserID") = CookiesWrapper.StaffID Then

            If Not drpFileType.SelectedIndex > 0 Then
                ShowMessage("Please pick file type before this action!", MessageTypeEnum.Error)
                Exit Sub
            End If

            If My.Settings.EnforceFileUpload Then

                If radUpload.UploadedFiles.Count > 0 Then

                    Try

                        For Each file As UploadedFile In radUpload.UploadedFiles

                            Dim TargetFolder As String = Server.MapPath("~/FileUploads/")
                            Dim Ext As String = file.GetExtension

                            file.SaveAs(Path.Combine(TargetFolder, file.FileName))

                            FileID = Save(Path.Combine(TargetFolder, file.FileName), Ext)

                            If Not FileID > 0 Then Throw New Exception("Failure")

                            Dim objObjects As New BusinessLogic.DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                            With objObjects

                                .ObjectTypeID = drpFileType.SelectedValue
                                .ObjectID = appointment.ID
                                .DocumentID = FileID

                                If Not .Save Then

                                    ShowMessage("Failed to complete file upload process...", MessageTypeEnum.Error)
                                    Exit Sub

                                End If

                            End With

                        Next

                    Catch ex As Exception

                        ShowMessage("Error while uplading file...", MessageTypeEnum.Error)
                        Exit Sub

                    End Try

                    If appointmentToUpdate.End <= Now Then

                        appointmentToUpdate.Attributes("Completed") = CompletedStatusCheckBox.Checked.ToString()
                        RadScheduler1.UpdateAppointment(appointmentToUpdate)
                        RadScheduler1.Rebind()

                    Else

                        ShowMessage("Error! You cannot flag an activity as 'Completed' before the end date has passed...", MessageTypeEnum.Error)
                        CompletedStatusCheckBox.Checked = False

                    End If

                Else

                    ShowMessage("Error! You need to upload supporting files...", MessageTypeEnum.Error)
                    CompletedStatusCheckBox.Checked = False

                End If

            Else

                If appointmentToUpdate.End <= Now Then

                    appointmentToUpdate.Attributes("Completed") = CompletedStatusCheckBox.Checked.ToString()
                    RadScheduler1.UpdateAppointment(appointmentToUpdate)
                    RadScheduler1.Rebind()

                Else

                    ShowMessage("Error! You cannot flag an activity as 'Completed' before the end date has passed...", MessageTypeEnum.Error)
                    CompletedStatusCheckBox.Checked = False

                End If

            End If

        Else

            ShowMessage("Oops! You cannot flag this activity as 'Completed' if you did not create it...", MessageTypeEnum.Error)
            CompletedStatusCheckBox.Checked = False

        End If

    End Sub

    Private Sub FilterAppointment(appointment As Appointment, UserList As List(Of Integer), ActivityList As List(Of Integer))

        If lstUsers.SelectedIndex > 0 And lstActivity.SelectedIndex <= 0 Then

            If UserList.Contains(appointment.Attributes("UserID")) AndAlso CheckIfUserAllowed(appointment) Then
                appointment.Visible = True
            End If

        ElseIf lstUsers.SelectedIndex <= 0 And lstActivity.SelectedIndex > 0 Then

            If ActivityList.Contains(appointment.Attributes("ActivityID")) AndAlso CheckIfUserAllowed(appointment) Then
                appointment.Visible = True
            End If

        ElseIf lstUsers.SelectedIndex > 0 And lstActivity.SelectedIndex > 0 Then

            If (UserList.Contains(appointment.Attributes("UserID")) And ActivityList.Contains(appointment.Attributes("ActivityID"))) AndAlso CheckIfUserAllowed(appointment) Then
                appointment.Visible = True
            End If

        End If

    End Sub

    Protected Sub RadScheduler1_AppointmentCreated(sender As Object, e As AppointmentCreatedEventArgs)
        If e.Appointment.RecurrenceState = RecurrenceState.Master OrElse e.Appointment.RecurrenceState = RecurrenceState.Occurrence Then
            Dim recurrenceStatePanel As Panel = TryCast(e.Container.FindControl("RecurrencePanel"), Panel)
            recurrenceStatePanel.Visible = True
        End If
        If e.Appointment.RecurrenceState = RecurrenceState.Exception Then
            Dim recurrenceExceptionPanel As Panel = TryCast(e.Container.FindControl("RecurrenceExceptionPanel"), Panel)
            recurrenceExceptionPanel.Visible = True
        End If
        If e.Appointment.Reminders.Count <> 0 Then
            Dim reminderPanel As Panel = TryCast(e.Container.FindControl("ReminderPanel"), Panel)
            reminderPanel.Visible = True
        End If
    End Sub

    Protected Sub RadScheduler1_AppointmentDataBound(sender As Object, e As SchedulerEventArgs)
        If e.Appointment.Attributes("Completed") = "True" Then
            e.Appointment.BackColor = System.Drawing.Color.CornflowerBlue
        End If
    End Sub

    Private Sub lstUsers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstUsers.SelectedIndexChanged
        RadScheduler1.Rebind()
    End Sub

    Private Sub RadScheduler1_AppointmentDataBound1(sender As Object, e As SchedulerEventArgs) Handles RadScheduler1.AppointmentDataBound

        If lstActivity.SelectedIndex > 0 Or lstUsers.SelectedIndex > 0 Then

            e.Appointment.Visible = False

            FilterAppointment(e.Appointment, User, Activity)

        Else

            e.Appointment.Visible = False

            If CheckIfUserAllowed(e.Appointment) Then

                e.Appointment.Visible = True

            End If

        End If

    End Sub

    Private Sub lstActivity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstActivity.SelectedIndexChanged
        RadScheduler1.Rebind()
    End Sub

    Private Function CheckIfUserAllowed(ByVal Appointment As Appointment) As Boolean

        With Appointment

            If Catchnull(.Attributes("UserID"), -1, True) <> CookiesWrapper.StaffID Then

                If Catchnull(.Attributes("AllowedUserID"), -1, True) = CookiesWrapper.StaffID Then

                    Return True

                Else

                    Return False

                End If

            End If

        End With

        Return True

    End Function

    Private Sub SqlDataSource1_Updating(sender As Object, e As SqlDataSourceCommandEventArgs) Handles SqlDataSource1.Updating

        Dim user = e.Command.Parameters(3).Value

        If user <> CookiesWrapper.StaffID Then

            ShowMessage("Oops! You cannot edit an activity that you have not created...", MessageTypeEnum.Error)
            e.Cancel = True

        End If

    End Sub

    Private Sub SqlDataSource1_Deleting(sender As Object, e As SqlDataSourceCommandEventArgs) Handles SqlDataSource1.Deleting

        'Dim user = e.Command.Parameters(1).Value

        'If user <> CookiesWrapper.StaffID Then

        '    ShowMessage("Oops! You cannot delete an activity that you have not created...", MessageTypeEnum.Error)
        '    e.Cancel = True

        'End If

    End Sub

    Public Function Save(ByVal FilePath As String, ByVal FileExt As String) As Long

        Try

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFiles

                .FileID = 0
                .FileDate = Now
                .FileTypeID = drpFileType.SelectedValue
                .Title = "Activity File"
                .Author = CookiesWrapper.thisUserFullName
                .Description = "Activity files uploaded on completion"
                .FilePath = FilePath
                .FileExtension = FileExt
                .ApplySecurity = 0

                If .Save Then

                    Return .FileID

                Else

                    ShowMessage("Error while uploading File to server", MessageTypeEnum.Error)
                    Return 0

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return 0

        End Try

    End Function

    Private Sub cboActivityCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboActivityCategory.SelectedIndexChanged

        If cboActivityCategory.SelectedIndex > 0 Then

            Dim objSql As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim sql As String = "select * from tblActivities A inner join tblActivityActivityCategory AA on A.ActivityID = AA.ActivityID where AA.ActivityCategoryID = " & cboActivityCategory.SelectedValue

            With lstActivity

                .DataSource = objSql.GetFiles(Sql)
                .DataTextField = "Description"
                .DataValueField = "ActivityID"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub RadScheduler1_AppointmentDelete(sender As Object, e As AppointmentDeleteEventArgs) Handles RadScheduler1.AppointmentDelete

        Dim user = e.Appointment.Attributes("UserID")

        If user <> CookiesWrapper.StaffID Then

            ShowMessage("Oops! You cannot delete an activity that you have not created...", MessageTypeEnum.Error)
            e.Cancel = True

        End If

    End Sub
End Class