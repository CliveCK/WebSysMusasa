Imports BusinessLogic
Imports Microsoft.Practices.EnterpriseLibrary.Data

Partial Class TripCostsDetailsControl
    Inherits System.Web.UI.UserControl

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)

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


    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim TripIDText As TextBox = DirectCast(Parent.Parent.FindControl("txtTripID"), TextBox)
        txtTripID.Text = TripIDText.Text

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboUnit

                .DataSource = objLookup.Lookup("luUnitOfMeasurement", "UnitOfMeasurementID", "Description").Tables(0)
                .DataValueField = "UnitOfMeasurementID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            LoadGrid()

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadTripCosts(ByVal TripCostID As Long) As Boolean

        Try

            Dim objTripCosts As New TripCosts(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTripCosts

                If .Retrieve(TripCostID) Then

                    txtTripCostID.Text = .TripCostID
                    txtTripID.Text = .TripID
                    If Not IsNothing(cboUnit.Items.FindByValue(.UnitID)) Then cboUnit.SelectedValue = .UnitID
                    txtQuantity.Text = .Quantity
                    txtCost.Text = .Cost
                    txtItem.Text = .Item

                    ShowMessage("Trip Cost details loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Trip Cost details...", MessageTypeEnum.Error)
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

            Dim objTripCosts As New TripCosts(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTripCosts

                .TripCostID = IIf(IsNumeric(txtTripCostID.Text), txtTripCostID.Text, 0)
                If IsNumeric(txtTripID.Text) Then .TripID = txtTripID.Text Else ShowMessage("Save/Load Trip details first!", MessageTypeEnum.Error) : Return False
                If cboUnit.SelectedIndex > -1 Then .UnitID = cboUnit.SelectedValue
                .Quantity = txtQuantity.Text
                .Cost = txtCost.Text
                .Item = txtItem.Text

                If .Save Then

                    If Not IsNumeric(txtTripCostID.Text) OrElse Trim(txtTripCostID.Text) = 0 Then txtTripCostID.Text = .TripCostID
                    LoadTripCosts(.TripCostID)
                    LoadGrid()
                    ShowMessage("Trip Cost details saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save Trip Cost details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtTripCostID.Text = ""
        If Not IsNothing(cboUnit.Items.FindByValue("")) Then
            cboUnit.SelectedValue = ""
        ElseIf Not IsNothing(cboUnit.Items.FindByValue(0)) Then
            cboUnit.SelectedValue = 0
        Else
            cboUnit.SelectedIndex = -1
        End If
        txtQuantity.Text = 0.0
        txtCost.Text = 0.0
        txtItem.Text = ""

    End Sub

    Private Sub LoadGrid()

        If IsNumeric(txtTripID.Text) Then

            Dim sql As String = "SELECT C.*, U.Description as Unit FROM tblTripCosts C inner  join luUnitOfMeasurement U on C.UnitID = U.UnitOfMeasurementID WHERE TripID = " & txtTripID.Text

            If sql <> "" Then

                With radTripCostListing

                    .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                    .DataBind()

                    ViewState("TripCosts") = .DataSource

                End With

            End If

        End If

    End Sub

    Private Sub radTripCostListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radTripCostListing.ItemCommand

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            If e.CommandName = "Delete" Then

                Dim objTripCosts As New TripCosts(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objTripCosts

                    .TripCostID = e.CommandArgument

                    If .Delete Then

                        ShowMessage("Entry deleted successfully", MessageTypeEnum.Information)

                    End If

                End With

            End If

        End If

    End Sub

    Private Sub radTripCostListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radTripCostListing.NeedDataSource

        radTripCostListing.DataSource = DirectCast(ViewState("TripCosts"), DataTable)

    End Sub
End Class

