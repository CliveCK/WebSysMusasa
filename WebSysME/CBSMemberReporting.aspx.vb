Public Class CBSMemberReporting
    Inherits System.Web.UI.Page

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

        If Not IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboProvince

                .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name").Tables(0)
                .DataValueField = "ProvinceID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name").Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name").Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboClub

                Dim sql As String = " SELECT * FROM tblGroups G inner join luGroupTypes GT on G.GroupTypeID = GT.GroupTypeID WHERE GT.Description = 'Club'"

                .DataSource = objLookup.SqlExec(sql)
                .DataValueField = "GroupID"
                .DataTextField = "GroupName"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboMonth

                .DataSource = objLookup.Lookup("luMonths", "MonthID", "Description").Tables(0)
                .DataValueField = "MonthID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadCBSMemberReporting(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub lnkCBSMembers_Click(sender As Object, e As EventArgs) Handles lnkCBSMembers.Click

        If IsNumeric(txtCBSMemberReportingID.Text) Then
            Response.Redirect("~/CBSMembers.aspx?id=" & objUrlEncoder.Encrypt(txtCBSMemberReportingID.Text))
        End If

    End Sub
    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub cboProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProvince.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProvince.SelectedIndex > 0 Then

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name", , "ProvinceID = " & cboProvince.SelectedValue).Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
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

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Public Function LoadCBSMemberReporting(ByVal CBSMemberReportingID As Long) As Boolean

        Try

            Dim objCBSMemberReporting As New BusinessLogic.CBSMemberReporting(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCBSMemberReporting

                If .Retrieve(CBSMemberReportingID) Then

                    txtCBSMemberReportingID.Text = .CBSMemberReportingID
                    If Not IsNothing(cboProvince.Items.FindByValue(.ProvinceID)) Then cboProvince.SelectedValue = .ProvinceID
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    If Not IsNothing(cboWard.Items.FindByValue(.Ward)) Then cboWard.SelectedValue = .Ward
                    If Not IsNothing(cboClub.Items.FindByValue(.ClubID)) Then cboClub.SelectedValue = .ClubID
                    txtYear.Text = .Year
                    If Not IsNothing(cboMonth.Items.FindByValue(.Month)) Then cboMonth.SelectedValue = .Month
                    txtChallenges.Text = .Challenges
                    txtRecommendations.Text = .Recommendations

                    ShowMessage("CBSMemberReporting loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadCBSMemberReporting: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objCBSMemberReporting As New BusinessLogic.CBSMemberReporting(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCBSMemberReporting

                .CBSMemberReportingID = IIf(IsNumeric(txtCBSMemberReportingID.Text), txtCBSMemberReportingID.Text, 0)
                If cboProvince.SelectedIndex > 0 Then .ProvinceID = cboProvince.SelectedValue
                If cboDistrict.SelectedIndex > 0 Then .DistrictID = cboDistrict.SelectedValue
                If cboWard.SelectedIndex > 0 Then .Ward = cboWard.SelectedValue
                If cboClub.SelectedIndex > 0 Then .ClubID = cboClub.SelectedValue
                .Year = txtYear.Text
                If cboMonth.SelectedIndex > -1 Then .Month = cboMonth.SelectedValue
                .Challenges = txtChallenges.Text
                .Recommendations = txtRecommendations.Text

                If .Save Then

                    If Not IsNumeric(txtCBSMemberReportingID.Text) OrElse Trim(txtCBSMemberReportingID.Text) = 0 Then txtCBSMemberReportingID.Text = .CBSMemberReportingID
                    ShowMessage("CBSMemberReporting saved successfully...", MessageTypeEnum.Information)

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

        txtCBSMemberReportingID.Text = ""
        If Not IsNothing(cboProvince.Items.FindByValue("")) Then
            cboProvince.SelectedValue = ""
        ElseIf Not IsNothing(cboProvince.Items.FindByValue(0)) Then
            cboProvince.SelectedValue = 0
        Else
            cboProvince.SelectedIndex = -1
        End If
        If Not IsNothing(cboDistrict.Items.FindByValue("")) Then
            cboDistrict.SelectedValue = ""
        ElseIf Not IsNothing(cboDistrict.Items.FindByValue(0)) Then
            cboDistrict.SelectedValue = 0
        Else
            cboDistrict.SelectedIndex = -1
        End If
        If Not IsNothing(cboWard.Items.FindByValue("")) Then
            cboWard.SelectedValue = ""
        ElseIf Not IsNothing(cboWard.Items.FindByValue(0)) Then
            cboWard.SelectedValue = 0
        Else
            cboWard.SelectedIndex = -1
        End If
        If Not IsNothing(cboClub.Items.FindByValue("")) Then
            cboClub.SelectedValue = ""
        ElseIf Not IsNothing(cboClub.Items.FindByValue(0)) Then
            cboClub.SelectedValue = 0
        Else
            cboClub.SelectedIndex = -1
        End If
        txtYear.Text = 0
        cboMonth.SelectedValue = 0
        txtChallenges.Text = ""
        txtRecommendations.Text = ""

    End Sub

End Class