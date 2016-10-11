Imports BusinessLogic

Partial Class SchoolsDetailsControl
    Inherits System.Web.UI.UserControl

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboProvince

                .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name").Tables(0)
                .DataValueField = "ProvinceID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboSchoolType

                .DataSource = objLookup.Lookup("luSchoolTypes", "SchoolTypeID", "Description").Tables(0)
                .DataValueField = "SchoolTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With
            If Not IsNothing(Request.QueryString("id")) Then

                LoadSchools(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Private Sub LoadCombo()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboDistrict

            .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name").Tables(0)
            .DataValueField = "DistrictID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboWard

            .DataSource = objLookup.Lookup("tblWards", "WardID", "Name").Tables(0)
            .DataValueField = "WardID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadSchools(ByVal SchoolID As Long) As Boolean

        Try

            Dim objSchools As New Schools(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSchools

                If .Retrieve(SchoolID) Then

                    LoadCombo()

                    txtSchoolID.Text = .SchoolID
                    If Not IsNothing(cboProvince.Items.FindByValue(.ProvinceID)) Then cboProvince.SelectedValue = .ProvinceID
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    If Not IsNothing(cboWard.Items.FindByValue(.WardID)) Then cboWard.SelectedValue = .WardID
                    If Not IsNothing(cboSchoolType.Items.FindByValue(.SchoolTypeID)) Then cboSchoolType.SelectedValue = .SchoolTypeID
                    txtStaffCompliment.Text = .StaffCompliment
                    txtNoOfMaleStudents.Text = .NoOfMaleStudents
                    txtNoOfFemaleStudents.Text = .NoOfFemaleStudents
                    txtTotalEnrollment.Text = .TotalEnrollment
                    txtLongitude.Text = .Longitude
                    txtLatitude.Text = .Latitude
                    txtName.Text = .Name

                    ShowMessage("Schools loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Schools", MessageTypeEnum.Error)
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

            Dim objSchools As New Schools(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSchools

                .SchoolID = IIf(IsNumeric(txtSchoolID.Text), txtSchoolID.Text, 0)
                If cboWard.SelectedIndex > -1 Then .WardID = cboWard.SelectedValue
                If cboSchoolType.SelectedIndex > -1 Then .SchoolTypeID = cboSchoolType.SelectedValue
                .StaffCompliment = IIf(IsNumeric(txtStaffCompliment.Text), txtStaffCompliment.Text, 0)
                .NoOfMaleStudents = IIf(IsNumeric(txtNoOfMaleStudents.Text), txtNoOfMaleStudents.Text, 0)
                .NoOfFemaleStudents = IIf(IsNumeric(txtNoOfFemaleStudents.Text), txtNoOfFemaleStudents.Text, 0)
                .TotalEnrollment = IIf(IsNumeric(txtTotalEnrollment.Text), txtTotalEnrollment.Text, 0)
                .Longitude = IIf(IsNumeric(txtLongitude.Text), txtLongitude.Text, 0)
                .Latitude = IIf(IsNumeric(txtLatitude.Text), txtLatitude.Text, 0)
                .Name = txtName.Text

                If .Save Then

                    If Not IsNumeric(txtSchoolID.Text) OrElse Trim(txtSchoolID.Text) = 0 Then txtSchoolID.Text = .SchoolID
                    ShowMessage("Schools saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error saving details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtSchoolID.Text = ""
        If Not IsNothing(cboWard.Items.FindByValue("")) Then
            cboWard.SelectedValue = ""
        ElseIf Not IsNothing(cboWard.Items.FindByValue(0)) Then
            cboWard.SelectedValue = 0
        Else
            cboWard.SelectedIndex = -1
        End If
        If Not IsNothing(cboSchoolType.Items.FindByValue("")) Then
            cboSchoolType.SelectedValue = ""
        ElseIf Not IsNothing(cboSchoolType.Items.FindByValue(0)) Then
            cboSchoolType.SelectedValue = 0
        Else
            cboSchoolType.SelectedIndex = -1
        End If
        txtStaffCompliment.Text = 0
        txtNoOfMaleStudents.Text = 0
        txtNoOfFemaleStudents.Text = 0
        txtTotalEnrollment.Text = 0
        txtLongitude.Text = 0.0
        txtLatitude.Text = 0.0
        txtName.Text = ""

    End Sub

    Private Sub cboProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProvince.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProvince.SelectedIndex > 0 Then

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name", , "ProvinceID = " & cboProvince.SelectedValue).Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDistrict.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboDistrict.SelectedIndex > 0 Then

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name", , "DistrictID = " & cboDistrict.SelectedValue).Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

End Class

