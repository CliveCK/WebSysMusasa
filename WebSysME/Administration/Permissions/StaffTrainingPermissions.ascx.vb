Imports SysPermissionsManager.Functionality

Public Class StaffTrainingPermissions
    Inherits System.Web.UI.UserControl

    Public Event ReceiveControlMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum)
    Private Shared ReadOnly SecurityLog As log4net.ILog = log4net.LogManager.GetLogger("SecurityLogger")
    Private objUserPermissions As New Global.SysPermissionsManager.Functionality(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.ChangeAccessPermissions) Then


            Dim myEx As New ApplicationException("You are not athorised to view this page details.Contact your administrator")
            Throw myEx

        End If

        ComplementaryListboxes1.SelectedOptionsCaption = "Assigned Staff To Training"

        ComplementaryListboxes1.AvailableOptionsCaption = "Unassigned Staff To Training"

        If Not Page.IsPostBack Then

            LoadTrainings()
            LoadAvailableUserPermmisions(0)
            LoadAssignedUserPermmisions(0)

        End If

    End Sub

    Sub LoadAvailableUserPermmisions(ByVal TrainingID As Long)

        Dim objPermmisions As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(TrainingID) Then

            If TrainingID > 0 Then

                Dim ds As DataSet = objPermmisions.GetAvaliableTrainingStaffMembers(TrainingID)

                ComplementaryListboxes1.AvailableOptions.DataSource = ds

                ComplementaryListboxes1.AvailableOptions.DataTextField = "FullName"

                ComplementaryListboxes1.AvailableOptions.DataValueField = "StaffID"

                ComplementaryListboxes1.AvailableOptions.DataBind()

            End If

        End If

    End Sub

    Sub LoadAssignedUserPermmisions(ByVal TrainingID As Long)

        Dim objPermmisions As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(TrainingID) Then

            If TrainingID > 0 Then

                Dim ds As DataSet = objPermmisions.GetSelectedTrainingStaffMembers(TrainingID)

                ComplementaryListboxes1.SelectedOptions.DataSource = ds

                ComplementaryListboxes1.SelectedOptions.DataTextField = "FullName"

                ComplementaryListboxes1.SelectedOptions.DataValueField = "StaffID"

                ComplementaryListboxes1.SelectedOptions.DataBind()

            End If

        End If

    End Sub

    Sub LoadTrainings()

        Dim DataLookup As New BusinessLogic.CommonFunctions

        With lstTrainings

            '.Items.Clear 'CHECK: Is it necessary to clear items before we load new data?
            .DataTextField = "Name"
            .DataValueField = "TrainingID"
            .DataSource = DataLookup.Lookup("tblTrainings", "TrainingID", "Name", "Name").Tables(0)
            .DataBind()

        End With

    End Sub

    Private Sub lstTrainings_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstTrainings.SelectedIndexChanged

        If IsNumeric(lstTrainings.SelectedValue) Then

            LoadAvailableUserPermmisions(lstTrainings.SelectedValue)

            LoadAssignedUserPermmisions(lstTrainings.SelectedValue)

        End If

    End Sub

    Protected Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click

        Try
            If Save(ComplementaryListboxes1, lstTrainings) Then

                RaiseEvent ReceiveControlMessage("Training Permissions applied..", MessageTypeEnum.Information)
                SecurityLog.Info("Training Permissions applied..")
            End If
        Catch ex As Exception
            RaiseEvent ReceiveControlMessage(ex.Message, MessageTypeEnum.Error)
        End Try

    End Sub

    Private Function Save(ByVal ucComplementaryListBox As ComplementaryListboxes, ByVal ctrListBox As ListBox) As Boolean

        Dim objUserFunctionality As New Global.SysPermissionsManager.UserGroupsManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        'revoke all permissions for this user

        Try
            objUserFunctionality.RevokeTrainingPermissions(ctrListBox.SelectedValue)

            Dim objStaffTrainingAccess As New BusinessLogic.StaffTrainingAccess(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objStaffTrainingAccess

                'save selected
                For i As Integer = 0 To ucComplementaryListBox.SelectedOptions.Items.Count - 1

                    ucComplementaryListBox.SelectedOptions.SelectedIndex = i

                    If ucComplementaryListBox.SelectedOptions.SelectedValue <> 0 Then

                        .SaveDetail(ctrListBox.SelectedValue, ucComplementaryListBox.SelectedOptions.SelectedValue)

                    End If

                Next

            End With

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
End Class