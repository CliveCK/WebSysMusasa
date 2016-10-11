Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class TripsDetailsControl
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

        If Not IsNothing(Request.QueryString("id")) Then

            txtProjectID.Text = objUrlEncoder.Decrypt(Request.QueryString("id"))

        End If

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadTrips(ByVal TripID As Long) As Boolean

        Try

            Dim objTrips As New Trips(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTrips

                If .Retrieve(TripID) Then

                    txtTripID.Text = .TripID
                    txtProjectID.Text = .ProjectID
                    radFromDate.SelectedDate = .FromDate
                    radToDate.SelectedDate = .ToDateDate
                    txtPurpose.Text = .Purpose
                    txtTravellingFrom.Text = .TravellingFrom
                    txtTravellingTo.Text = .TravellingTo

                    ucTripCostsDetails.Page_Load(Me, New System.EventArgs)
                    ucTripDocuments.Page_Load(Me, New System.EventArgs)
                    ucTripTravellersDetails.Page_Load(Me, New System.EventArgs)

                    ShowMessage("Trip details loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Trip details...", MessageTypeEnum.Error)
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

            Dim objTrips As New Trips(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTrips

                .TripID = IIf(IsNumeric(txtTripID.Text), txtTripID.Text, 0)
                If IsNumeric(txtProjectID.Text) Then .ProjectID = txtProjectID.Text Else Return False
                .FromDate = radFromDate.SelectedDate
                .ToDateDate = radToDate.SelectedDate
                .Purpose = txtPurpose.Text
                .TravellingFrom = txtTravellingFrom.Text
                .TravellingTo = txtTravellingTo.Text

                If .Save Then

                    If Not IsNumeric(txtTripID.Text) OrElse Trim(txtTripID.Text) = 0 Then txtTripID.Text = .TripID
                    LoadTrips(.TripID)
                    LoadGrid()
                    ShowMessage("Trip details saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save Trip details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtTripID.Text = ""
        radFromDate.Clear()
        radToDate.Clear()
        txtPurpose.Text = ""
        txtTravellingFrom.Text = ""
        txtTravellingTo.Text = ""

    End Sub

    Public Sub LoadGrid()

        If IsNumeric(txtProjectID.Text) Then

            Dim objTrip As New BusinessLogic.Trips(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim ds As DataSet = objTrip.GetTrips("SELECT * FROM tblTrips WHERE ProjectID = " & txtProjectID.Text)

            With radTripListing

                .DataSource = ds.Tables(0)
                .DataBind()

                ViewState("ProjectTrip") = .DataSource

            End With

        Else

            ShowMessage("Failed to load grid: Missing parameter", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radTripListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radTripListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "ViewTripDetails" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radTripListing.Items(index)

                Dim TripID As Long = Server.HtmlDecode(item("TripID").Text)

                LoadTrips(TripID)

            End If

        End If

    End Sub

    Private Sub radTripListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radTripListing.NeedDataSource

        radTripListing.DataSource = DirectCast(ViewState("ProjectTrip"), DataTable)

    End Sub

End Class

