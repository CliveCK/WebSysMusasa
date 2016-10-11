Imports BusinessLogic

Partial Class PatientsDetailsControl
    Inherits System.Web.UI.UserControl

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

            InitializeComponents()

            If Not IsNothing(Request.QueryString("id")) Then

                LoadPatients(objUrlEncoder.Decrypt(Request.QueryString("id")))

            Else

                If CookiesWrapper.PatientID <> 0 Then

                    LoadPatients(CookiesWrapper.PatientID)

                End If

            End If

        End If

    End Sub

    Private Sub InitializeComponents()

        LoadCombo(cboMedicalAidType, "luMedicalAidTypes", "Description", "MedicalAidTypeID")
        LoadCombo(cboReligion, "luReligion", "Description", "ReligionID")
        LoadCombo(cboOrphanhood, "tblOrphanhood", "Description", "ObjectID")
        LoadCombo(cboSchool, "tblSchools", "Name", "SchoolID")
        LoadCombo(cboClosestHealthCenter, "tblHealthCenters", "Name", "HealthCenterID")
        LoadCombo(cboStatus, "luPatientStatus", "Description", "PatientStatusID")

    End Sub

    Public Sub LoadCombo(ByVal cboCombo As DropDownList, ByVal Table As String, ByVal TextField As String, ByVal ValueField As String)

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboCombo

            .DataSource = objLookup.Lookup(Table, ValueField, TextField).Tables(0)
            .DataValueField = ValueField
            .DataTextField = TextField
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadPatients(ByVal PatientID As Long) As Boolean

        Try

            Dim objPatients As New Patients(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objPatients

                If .Retrieve(PatientID) Then

                    txtPatientID.Text = .PatientID
                    txtPatientNumber.Text = .PatientNo
                    If Not IsNothing(cboReligion.Items.FindByValue(.ReligionID)) Then cboReligion.SelectedValue = .ReligionID
                    If Not IsNothing(cboMedicalAidType.Items.FindByValue(.MedicalAidTypeID)) Then cboMedicalAidType.SelectedValue = .MedicalAidTypeID
                    If Not IsNothing(cboOrphanhood.Items.FindByValue(.OrphanhoodID)) Then cboOrphanhood.SelectedValue = .OrphanhoodID
                    If Not IsNothing(cboClosestHealthCenter.Items.FindByValue(.ClosestHealthCenterID)) Then cboClosestHealthCenter.SelectedValue = .ClosestHealthCenterID
                    If Not IsNothing(cboSchool.Items.FindByValue(.SchoolID)) Then cboSchool.SelectedValue = .SchoolID
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    If Not IsNothing(cboStatus.Items.FindByValue(.StatusID)) Then cboStatus.SelectedValue = .StatusID
                    If Not .DOB = "" Then radDateofBirth.SelectedDate = .DOB
                    If Not .DOD = "" Then radDateOfDeath.SelectedDate = .DOD
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    txtTelephone.Text = .Telephone
                    If Not IsNothing(cboClosestHealthCenter.Items.FindByValue(.ClosestHealthCenterID)) Then cboClosestHealthCenter.SelectedValue = .Sex

                    ShowMessage("Patients loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Patients", MessageTypeEnum.Error)
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

            Dim objPatients As New Patients(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objPatients

                .PatientID = IIf(IsNumeric(txtPatientID.Text), txtPatientID.Text, 0)
                .PatientNo = txtPatientNumber.Text
                If cboReligion.SelectedIndex > 0 Then .ReligionID = cboReligion.SelectedValue
                If cboMedicalAidType.SelectedIndex > 0 Then .MedicalAidTypeID = cboMedicalAidType.SelectedValue
                If cboOrphanhood.SelectedIndex > 0 Then .OrphanhoodID = cboOrphanhood.SelectedValue
                If cboClosestHealthCenter.SelectedIndex > 0 Then .ClosestHealthCenterID = cboClosestHealthCenter.SelectedValue
                If cboStatus.SelectedIndex > 0 Then .StatusID = cboStatus.SelectedValue
                If cboSchool.SelectedIndex > 0 Then .SchoolID = cboSchool.SelectedValue
                If radDateofBirth.SelectedDate.HasValue Then .DOB = radDateofBirth.SelectedDate
                If radDateOfDeath.SelectedDate.HasValue Then .DOD = radDateOfDeath.SelectedDate
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .Sex = cboSex.SelectedValue
                .Telephone = txtTelephone.Text

                If .Save Then

                    If Not IsNumeric(txtPatientID.Text) OrElse Trim(txtPatientID.Text) = 0 Then txtPatientID.Text = .PatientID
                    LoadPatients(.PatientID)
                    CookiesWrapper.PatientID = .PatientID
                    ShowMessage("Patients saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtPatientID.Text = ""
        txtPatientNumber.Text = ""
        If Not IsNothing(cboReligion.Items.FindByValue("")) Then
            cboReligion.SelectedValue = ""
        ElseIf Not IsNothing(cboReligion.Items.FindByValue(0)) Then
            cboReligion.SelectedValue = 0
        Else
            cboReligion.SelectedIndex = -1
        End If
        If Not IsNothing(cboMedicalAidType.Items.FindByValue("")) Then
            cboMedicalAidType.SelectedValue = ""
        ElseIf Not IsNothing(cboMedicalAidType.Items.FindByValue(0)) Then
            cboMedicalAidType.SelectedValue = 0
        Else
            cboMedicalAidType.SelectedIndex = -1
        End If
        If Not IsNothing(cboOrphanhood.Items.FindByValue("")) Then
            cboOrphanhood.SelectedValue = ""
        ElseIf Not IsNothing(cboOrphanhood.Items.FindByValue(0)) Then
            cboOrphanhood.SelectedValue = 0
        Else
            cboOrphanhood.SelectedIndex = -1
        End If
        If Not IsNothing(cboStatus.Items.FindByValue("")) Then
            cboStatus.SelectedValue = ""
        ElseIf Not IsNothing(cboStatus.Items.FindByValue(0)) Then
            cboStatus.SelectedValue = 0
        Else
            cboStatus.SelectedIndex = -1
        End If
        If Not IsNothing(cboClosestHealthCenter.Items.FindByValue("")) Then
            cboClosestHealthCenter.SelectedValue = ""
        ElseIf Not IsNothing(cboClosestHealthCenter.Items.FindByValue(0)) Then
            cboClosestHealthCenter.SelectedValue = 0
        Else
            cboClosestHealthCenter.SelectedIndex = -1
        End If
        If Not IsNothing(cboSchool.Items.FindByValue("")) Then
            cboSchool.SelectedValue = ""
        ElseIf Not IsNothing(cboSchool.Items.FindByValue(0)) Then
            cboSchool.SelectedValue = 0
        Else
            cboSchool.SelectedIndex = -1
        End If
        If Not IsNothing(cboSex.Items.FindByValue("")) Then
            cboSex.SelectedValue = ""
        ElseIf Not IsNothing(cboSex.Items.FindByValue(0)) Then
            cboSex.SelectedValue = 0
        Else
            cboSex.SelectedIndex = -1
        End If
        radDateofBirth.Clear()
        radDateOfDeath.Clear()
        txtFirstName.Text = ""
        txtSurname.Text = ""
        txtTelephone.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        CookiesWrapper.PatientID = 0

        Response.Redirect("~/PatientDetailsPage")

    End Sub
End Class

