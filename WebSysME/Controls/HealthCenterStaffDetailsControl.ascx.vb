Imports BusinessLogic
Imports Telerik.Web.UI
Imports Universal.CommonFunctions

Partial Class HealthCenterStaffDetailsControl
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

        If Not IsNothing(Request.QueryString("id")) AndAlso IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("id"))) Then

            Save()

        Else

            ShowMessage("Please save Health Center Details first...", MessageTypeEnum.Error)

        End If


    End Sub

    Public Function LoadHealthCenterStaff(ByVal HealthCenterStaffID As Long) As Boolean

        Try

            Dim objHealthCenterStaff As New HealthCenterStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHealthCenterStaff

                If .Retrieve(HealthCenterStaffID) Then

                    LoadCombos()

                    txtHealthCenterStaffID.Text = .HealthCenterStaffID
                    txtHealthCenterID.Text = .HealthCenterID
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    If Not IsNothing(cboDepartment.Items.FindByValue(.DepartmentID)) Then cboDepartment.SelectedValue = .DepartmentID
                    If Not IsNothing(cboGroupType.Items.FindByValue(.GroupTypeID)) Then cboGroupType.SelectedValue = .GroupTypeID
                    If Not IsNothing(cboTitle.Items.FindByText(.Title)) Then cboTitle.SelectedItem.Text = .Title
                    If Not IsNothing(cboStaffRole.Items.FindByValue(.StaffRoleID)) Then cboStaffRole.SelectedValue = .StaffRoleID
                    If Not .DOB = "" Then radDOB.SelectedDate = .DOB
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    txtEmail.Text = .Email
                    txtSite.Text = .Site
                    txtContactNo.Text = .ContactNo

                    ShowMessage("HealthCenterStaff loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadHealthCenterStaff: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objHealthCenterStaff As New HealthCenterStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHealthCenterStaff

                .HealthCenterStaffID = IIf(IsNumeric(txtHealthCenterStaffID.Text), txtHealthCenterStaffID.Text, 0)
                .HealthCenterID = IIf(IsNumeric(txtHealthCenterID.Text), txtHealthCenterID.Text, objUrlEncoder.Decrypt(Request.QueryString("id")))
                If cboSex.SelectedIndex > -1 Then .Sex = cboSex.SelectedValue
                If cboTitle.SelectedIndex > -1 Then .Title = cboTitle.SelectedItem.Text
                If cboDepartment.SelectedIndex > -1 Then .DepartmentID = cboDepartment.SelectedValue
                If cboGroupType.SelectedIndex > -1 Then .GroupTypeID = cboGroupType.SelectedValue
                If cboStaffRole.SelectedIndex > -1 Then .StaffRoleID = cboStaffRole.SelectedValue
                If radDOB.SelectedDate.HasValue Then .DOB = radDOB.SelectedDate
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .Site = txtSite.Text
                .Email = txtEmail.Text
                .ContactNo = txtContactNo.Text

                If .Save Then

                    If Not IsNumeric(txtHealthCenterStaffID.Text) OrElse Trim(txtHealthCenterStaffID.Text) = 0 Then txtHealthCenterStaffID.Text = .HealthCenterStaffID
                    LoadGrid()
                    ShowMessage("HealthCenterStaff saved successfully...", MessageTypeEnum.Information)

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

        txtHealthCenterStaffID.Text = ""
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
        If Not IsNothing(cboGroupType.Items.FindByValue("")) Then
            cboGroupType.SelectedValue = ""
        ElseIf Not IsNothing(cboGroupType.Items.FindByValue(0)) Then
            cboGroupType.SelectedValue = 0
        Else
            cboGroupType.SelectedIndex = -1
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

            .DataSource = objLookup.Lookup("luStaffPosition", "PositionID", "Description").Tables(0)
            .DataValueField = "PositionID"
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

        With cboGroupType

            .DataSource = objLookup.Lookup("luHealthGroupTypes", "HealthGroupTypeID", "Description").Tables(0)
            .DataValueField = "HealthGroupTypeID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboDepartment

            .DataSource = objLookup.Lookup("luDepartments", "DepartmentID", "Description").Tables(0)
            .DataValueField = "DepartmentID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))

        End With

    End Sub

    Private Sub LoadGrid()

        Dim objStaff As New HealthCenterStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "select S.*, H.Name As HealthCenter, SR.Description As StaffRole from tblHealthCenterStaff S inner join tblHealthCenters H on S.HealthCenterID = H.HealthCenterID "
        sql &= "left outer join luStaffPosition SR on SR.PositionID = S.StaffRoleID WHERE S.HealthCenterID = " & Catchnull(objUrlEncoder.Decrypt(Request.QueryString("id")), 0)

        With radStaffListing

            .DataSource = objStaff.GetHealthCenterStaff(sql)
            .DataBind()

            Session("HStaff") = .DataSource

        End With

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Private Sub radStaffListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radStaffListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radStaffListing.Items(index1)
            Dim HealthCenterStaffID As Integer

            HealthCenterStaffID = Server.HtmlDecode(item1("HealthCenterStaffID").Text)

            LoadHealthCenterStaff(HealthCenterStaffID)

        End If

    End Sub

    Private Sub radStaffListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radStaffListing.NeedDataSource

        radStaffListing.DataSource = Session("HStaff")

    End Sub

    Private Sub LoadCombos()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboDepartment

            .DataSource = objLookup.Lookup("luDepartments", "DepartmentID", "Description").Tables(0)
            .DataValueField = "DepartmentID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))

        End With

    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click

        If IsNumeric(txtHealthCenterID.Text) Then

            Dim objHealthCenter As New BusinessLogic.HealthCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHealthCenter

                .HealthCenterID = txtHealthCenterID.Text

                If .Delete Then

                    ShowMessage("Health Center deleted successfully", MessageTypeEnum.Information)

                End If

            End With

        Else

            ShowMessage("No information to delete!", MessageTypeEnum.Error)

        End If

    End Sub
End Class

