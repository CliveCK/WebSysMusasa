Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class SchoolStaffDetailsControl
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

            LoadComboBoxes()
            LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadSchoolStaff(ByVal SchoolStaffID As Long) As Boolean

        Try

            Dim objSchoolStaff As New SchoolStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSchoolStaff

                If .Retrieve(SchoolStaffID) Then

                    txtSchoolStaffID.Text = .SchoolStaffID
                    txtSchoolID.Text = .SchoolID
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    If Not IsNothing(cboTitle.Items.FindByValue(.Title)) Then cboTitle.SelectedValue = .Title
                    If Not IsNothing(cboStaffRole.Items.FindByValue(.StaffRoleID)) Then cboStaffRole.SelectedValue = .StaffRoleID
                    If Not .DOB = "" Then radDOB.SelectedDate = .DOB
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname

                    ShowMessage("SchoolStaff loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadSchoolStaff: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objSchoolStaff As New SchoolStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSchoolStaff

                .SchoolStaffID = IIf(IsNumeric(txtSchoolStaffID.Text), txtSchoolStaffID.Text, 0)
                .SchoolID = IIf(IsNumeric(txtSchoolID.Text), txtSchoolID.Text, objUrlEncoder.Decrypt(Request.QueryString("id")))
                If cboSex.SelectedIndex > -1 Then .Sex = cboSex.SelectedValue
                If cboTitle.SelectedIndex > -1 Then .Title = cboTitle.SelectedValue
                If cboStaffRole.SelectedIndex > -1 Then .StaffRoleID = cboStaffRole.SelectedValue
                If radDOB.SelectedDate.HasValue Then .DOB = radDOB.SelectedDate
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text

                If .Save Then

                    If Not IsNumeric(txtSchoolStaffID.Text) OrElse Trim(txtSchoolStaffID.Text) = 0 Then txtSchoolStaffID.Text = .SchoolStaffID
                    LoadGrid()
                    ShowMessage("SchoolStaff saved successfully...", MessageTypeEnum.Information)

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

        If Not IsNothing(cboSex.Items.FindByValue("")) Then
            cboSex.SelectedValue = ""
        ElseIf Not IsNothing(cboSex.Items.FindByValue(0)) Then
            cboSex.SelectedValue = 0
        Else
            cboSex.SelectedIndex = -1
        End If
        If Not IsNothing(cboTitle.Items.FindByValue("")) Then
            cboTitle.SelectedValue = ""
        ElseIf Not IsNothing(cboTitle.Items.FindByValue(0)) Then
            cboTitle.SelectedValue = 0
        Else
            cboTitle.SelectedIndex = -1
        End If
        If Not IsNothing(cboStaffRole.Items.FindByValue("")) Then
            cboStaffRole.SelectedValue = ""
        ElseIf Not IsNothing(cboStaffRole.Items.FindByValue(0)) Then
            cboStaffRole.SelectedValue = 0
        Else
            cboStaffRole.SelectedIndex = -1
        End If
        radDOB.Clear()
        txtFirstName.Text = ""
        txtSurname.Text = ""

    End Sub

    Private Sub LoadComboBoxes()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboStaffRole

            .DataSource = objLookup.Lookup("luStaffRoles", "StaffRoleID", "Description").Tables(0)
            .DataValueField = "StaffRoleID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboTitle

            .DataSource = objLookup.Lookup("luTitles", "TitleID", "TitleName").Tables(0)
            .DataValueField = "TitleID"
            .DataTextField = "TitleName"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Private Sub LoadGrid()

        Dim objStaff As New HealthCenterStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "select S.*, SC.Name As School, SR.Description As StaffRole from tblSchoolStaff S inner join tblSchools SC on S.SchoolID = SC.SchoolID "
        sql &= "left outer join luStaffRoles SR on SR.StaffRoleID = S.StaffRoleID"

        With radStaffListing

            .DataSource = objStaff.GetHealthCenterStaff(sql)
            .DataBind()

            Session("SStaff") = .DataSource

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub radStaffListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radStaffListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radStaffListing.Items(index1)
            Dim SchoolStaffID As Integer

            SchoolStaffID = Server.HtmlDecode(item1("SchoolStaffID").Text)

            LoadSchoolStaff(SchoolStaffID)

        End If

    End Sub

    Private Sub radStaffListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radStaffListing.NeedDataSource

        radStaffListing.DataSource = Session("SStaff")

    End Sub

End Class

