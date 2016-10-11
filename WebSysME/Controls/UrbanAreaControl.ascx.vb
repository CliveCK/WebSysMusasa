Public Class UrbanAreaControl
    Inherits System.Web.UI.UserControl

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

            cboCity.Enabled = False
            cboSuburb.Enabled = False
            cboSection.Enabled = False
            txtCity.Enabled = False
            txtSuburb.Enabled = False
            txtSection.Enabled = False

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboCountry

                .DataSource = objLookup.Lookup("luCountries", "CountryID", "CountryName").Tables(0)
                .DataValueField = "CountryID"
                .DataTextField = "CountryName"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub rbLstSaveOption_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbLstSaveOption.SelectedIndexChanged

        Select Case rbLstSaveOption.SelectedValue

            Case "Country"
                cboCity.Enabled = False
                cboSuburb.Enabled = False
                cboSection.Enabled = False
                txtCity.Enabled = False
                txtSuburb.Enabled = False
                txtSection.Enabled = False

            Case "City"
                cboCity.Enabled = True
                txtCity.Enabled = True
                cboSection.Enabled = False
                cboSuburb.Enabled = False
                txtSuburb.Enabled = False
                txtSection.Enabled = False

            Case "Suburb"
                cboCity.Enabled = True
                txtCity.Enabled = True
                cboSuburb.Enabled = True
                txtSuburb.Enabled = True
                cboSection.Enabled = False
                txtSection.Enabled = False

            Case "Section"
                cboCity.Enabled = True
                txtCity.Enabled = True
                cboSection.Enabled = True
                txtSection.Enabled = True
                cboSuburb.Enabled = True
                txtSuburb.Enabled = True

        End Select

    End Sub

    Private Sub cboCountry_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCountry.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboCountry.SelectedIndex > 0 Then

            With cboCity

                .DataSource = objLookup.Lookup("tblCities", "CityID", "Name", , "CountryID = " & cboCountry.SelectedValue).Tables(0)
                .DataValueField = "CityID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboCity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCity.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboCity.SelectedIndex > 0 Then

            With cboSuburb

                .DataSource = objLookup.Lookup("tblSuburbs", "SuburbID", "Name", , "CityID = " & cboCity.SelectedValue).Tables(0)
                .DataValueField = "SuburbID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If


    End Sub

    Private Sub cboSuburb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSuburb.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboSuburb.SelectedIndex > 0 Then

            With cboSection

                .DataSource = objLookup.Lookup("tblSection", "SectionID", "Name", , "SuburbID = " & cboSuburb.SelectedValue).Tables(0)
                .DataValueField = "SectionID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If


    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        Dim objUrbanArea As New BusinessLogic.UrbanArea(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objUrbanArea

            Select Case rbLstSaveOption.SelectedValue

                Case "Country"
                    If txtCountry.Text = "" Then Exit Sub
                    .Country = txtCountry.Text

                Case "City"
                    If txtCity.Text = "" Then Exit Sub
                    .City = txtCity.Text
                    .CountryID = cboCountry.SelectedValue

                Case "Suburb"
                    If txtSuburb.Text = "" Then Exit Sub
                    .Suburb = txtSuburb.Text
                    .CityID = cboCity.SelectedValue

                Case "Section"
                    If txtSection.Text = "" Then Exit Sub
                    .Section = txtSection.Text
                    .SuburbID = cboSuburb.SelectedValue

            End Select

            If .Save(rbLstSaveOption.SelectedValue) Then

                ShowMessage(rbLstSaveOption.SelectedValue & " saved successfully...", MessageTypeEnum.Information)

            Else

                ShowMessage("Error: Failed to save " & rbLstSaveOption.SelectedValue, MessageTypeEnum.Error)

            End If

        End With

    End Sub
End Class