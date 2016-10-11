Imports BusinessLogic

Partial Class FirstPointOfContactDetailsControl
    Inherits System.Web.UI.UserControl

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

            With cboTypeOfContact

                .DataSource = objLookup.Lookup("luContactTypes", "ContactTypeID", "Description")
                .DataValueField = "ContactTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With txtAdmittedTo

                .DataSource = objLookup.Lookup("tblHealthCenters", "HealthCenterID", "Name")
                .DataValueField = "HealthCenterID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If CookiesWrapper.PatientID > 0 Then

                LoadFirstPointOfContact(CookiesWrapper.PatientID)

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If CookiesWrapper.PatientID > 0 Then

            Save()

        Else

            ShowMessage("Please save Patient details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadFirstPointOfContact(ByVal PatientID As Long) As Boolean

        Try

            Dim objFirstPointOfContact As New FirstPointOfContact(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFirstPointOfContact

                If .Retrieve(PatientID) Then

                    txtFirstPointOfContactID.Text = .FirstPointOfContactID
                    If Not IsNothing(cboTypeOfContact.Items.FindByValue(.TypeOfContactID)) Then cboTypeOfContact.SelectedValue = .TypeOfContactID
                    If Not IsNothing(txtAdmittedTo.Items.FindByValue(.AdmittedTo)) Then txtAdmittedTo.SelectedValue = .AdmittedTo
                    If Not IsNothing(.DateOfFirstContact) Then radFirstContact.SelectedDate = .DateOfFirstContact
                    If Not IsNothing(.DateOfFirstReferral) Then radFirstRefferal.SelectedDate = .DateOfFirstReferral
                    If Not IsNothing(.DateFirstAdmitted) Then radFirstAdmitted.SelectedDate = .DateFirstAdmitted
                    txtContactName.Text = .ContactName
                    txtReferralHospital.Text = .ReferralHospital

                    ShowMessage("FirstPointOfContact loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load FirstPointOfContact...", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function Save() As Boolean

        Try

            Dim objFirstPointOfContact As New FirstPointOfContact(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objFirstPointOfContact

                .FirstPointOfContactID = IIf(IsNumeric(txtFirstPointOfContactID.Text), txtFirstPointOfContactID.Text, 0)
                .PatientID = CookiesWrapper.PatientID
                If cboTypeOfContact.SelectedIndex > -1 Then .TypeOfContactID = cboTypeOfContact.SelectedValue
                If txtAdmittedTo.SelectedIndex > -1 Then .AdmittedTo = txtAdmittedTo.SelectedValue
                .DateOfFirstContact = radFirstContact.SelectedDate
                .DateOfFirstReferral = radFirstRefferal.SelectedDate
                .DateFirstAdmitted = radFirstAdmitted.SelectedDate
                .ContactName = txtContactName.Text
                .ReferralHospital = txtReferralHospital.Text

                If .Save Then

                    If Not IsNumeric(txtFirstPointOfContactID.Text) OrElse Trim(txtFirstPointOfContactID.Text) = 0 Then txtFirstPointOfContactID.Text = .FirstPointOfContactID
                    LoadFirstPointOfContact(.PatientID)
                    ShowMessage("FirstPointOfContact saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        If Not IsNothing(txtAdmittedTo.Items.FindByValue("")) Then
            txtAdmittedTo.SelectedValue = ""
        ElseIf Not IsNothing(txtAdmittedTo.Items.FindByValue(0)) Then
            txtAdmittedTo.SelectedValue = 0
        Else
            txtAdmittedTo.SelectedIndex = -1
        End If
        txtFirstPointOfContactID.Text = ""
        If Not IsNothing(cboTypeOfContact.Items.FindByValue("")) Then
            cboTypeOfContact.SelectedValue = ""
        ElseIf Not IsNothing(cboTypeOfContact.Items.FindByValue(0)) Then
            cboTypeOfContact.SelectedValue = 0
        Else
            cboTypeOfContact.SelectedIndex = -1
        End If
        radFirstAdmitted.Clear()
        radFirstContact.Clear()
        radFirstRefferal.Clear()
        txtContactName.Text = ""
        txtReferralHospital.Text = ""

    End Sub

End Class

