Public Class HouseholdNeeds
    Inherits System.Web.UI.UserControl

    Private ReadOnly Property HouseholdID As Long
        Get
            Dim txtHouseholdID As TextBox = DirectCast(Parent.Parent.FindControl("ucBeneficiaryControl").FindControl("txtBeneficiaryID1"), TextBox)

            Return IIf(IsNumeric(txtHouseholdID.Text), txtHouseholdID.Text, 0)
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboNeeds

                .DataSource = objLookup.Lookup("luNeeds", "NeedID", "Description").Tables(0)
                .DataValueField = "NeedID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            Dim objNeeds As New BusinessLogic.HouseHoldNeeds(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            If CookiesWrapper.BeneficiaryID > 0 Then

                LoadGrid(objNeeds, CookiesWrapper.BeneficiaryID)

            End If

        End If

    End Sub

    Public Function LoadHouseHoldNeeds(ByVal HouseHoldNeedID As Long) As Boolean

        Try

            Dim objHouseHoldNeeds As New BusinessLogic.HouseHoldNeeds(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHouseHoldNeeds

                If .Retrieve(HouseHoldNeedID) Then

                    If Not IsNothing(cboNeeds.Items.FindByValue(.NeedID)) Then cboNeeds.SelectedValue = .NeedID

                    Return True

                Else

                    Return False

                End If

            End With

        Catch ex As Exception

            'ShowMessage(ex, MessageType.Error)
            Return False

        End Try

    End Function

    Public Function Save() As Boolean

        Try

            Dim objHouseHoldNeeds As New BusinessLogic.HouseHoldNeeds(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHouseHoldNeeds

                .NeedID = cboNeeds.SelectedValue
                .BeneficiaryID = CookiesWrapper.BeneficiaryID

                If .Save Then

                    ShowMessage("HouseHold Needs saved successfully...", MessageTypeEnum.Information)
                    LoadGrid(objHouseHoldNeeds, .BeneficiaryID)

                    Return True

                Else

                    ShowMessage("Error saving needs...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        If Not IsNothing(cboNeeds.Items.FindByValue("")) Then
            cboNeeds.SelectedValue = ""
        ElseIf Not IsNothing(cboNeeds.Items.FindByValue(0)) Then
            cboNeeds.SelectedValue = 0
        Else
            cboNeeds.SelectedIndex = -1
        End If

    End Sub

    Public Sub LoadGrid(ByVal objNeeds As BusinessLogic.HouseHoldNeeds, ByVal BeneficiaryID As Long)

        Session("Needs") = Nothing

        With radBenListing

            .DataSource = objNeeds.GetNeeds(CookiesWrapper.BeneficiaryID)

            Session("Needs") = .DataSource

        End With

    End Sub

    Private Sub radBenListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radBenListing.NeedDataSource

        radBenListing.DataSource = Session("Needs")

    End Sub

    Private Sub cmdAddNeed_Click(sender As Object, e As EventArgs) Handles cmdAddNeed.Click

        If CookiesWrapper.BeneficiaryID > 0 Then

            Save()

        Else

            ShowMessage("Please save Individual/Household details first", MessageTypeEnum.Error)

        End If

    End Sub
End Class