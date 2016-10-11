﻿Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class ProcumentDetailsControl
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

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboCommodity

                .DataSource = objLookup.Lookup("luCommodities", "CommodityID", "Description").Tables(0)
                .DataValueField = "CommodityID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("Procid")) Then

                LoadProcument(objUrlEncoder.Decrypt(Request.QueryString("Procid")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadProcument(ByVal ProcumentID As Long) As Boolean

        Try

            Dim objProcument As New Procument(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProcument

                If .Retrieve(ProcumentID) Then

                    txtProcumentID.Text = .ProcumentID
                    If Not IsNothing(cboCommodity.Items.FindByValue(.CommodityID)) Then cboCommodity.SelectedValue = .CommodityID
                    txtQuantityRequired.Text = .QuantityRequired
                    radDateRequired.SelectedDate = .DateRequired
                    radDateOrdered.SelectedDate = .DateOrdered
                    radDateRequested.SelectedDate = .DateRequested
                    radDateSupplied.SelectedDate = .DateSupplied
                    txtRequestedBy.Text = .RequestedBy
                    txtOrderedBy.Text = .OrderedBy

                    ShowMessage("Procument loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadProcument: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objProcument As New Procument(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objProcument

                .ProcumentID = IIf(IsNumeric(txtProcumentID.Text), txtProcumentID.Text, 0)
                If IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("id"))) Then .ProjectID = objUrlEncoder.Decrypt(Request.QueryString("id")) Else Return False
                If cboCommodity.SelectedIndex > -1 Then .CommodityID = cboCommodity.SelectedValue
                .QuantityRequired = txtQuantityRequired.Text
                .DateRequired = radDateRequired.SelectedDate
                .DateOrdered = radDateOrdered.SelectedDate
                .DateRequested = radDateRequested.SelectedDate
                .DateSupplied = radDateSupplied.SelectedDate
                .RequestedBy = txtRequestedBy.Text
                .OrderedBy = txtOrderedBy.Text

                If .Save Then

                    If Not IsNumeric(txtProcumentID.Text) OrElse Trim(txtProcumentID.Text) = 0 Then txtProcumentID.Text = .ProcumentID
                    LoadProcument(.ProcumentID)
                    LoadGrid()
                    ShowMessage("Procument details saved successfully...", MessageTypeEnum.Information)

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

        txtProcumentID.Text = ""
        If Not IsNothing(cboCommodity.Items.FindByValue("")) Then
            cboCommodity.SelectedValue = ""
        ElseIf Not IsNothing(cboCommodity.Items.FindByValue(0)) Then
            cboCommodity.SelectedValue = 0
        Else
            cboCommodity.SelectedIndex = -1
        End If
        txtQuantityRequired.Text = 0
        radDateRequired.Clear()
        radDateOrdered.Clear()
        radDateRequired.Clear()
        radDateSupplied.Clear()
        txtRequestedBy.Text = ""
        txtOrderedBy.Text = ""

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub

    Public Sub LoadGrid()

        Dim objSurvey As New BusinessLogic.Survey(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objSurvey.GetSurvey("SELECT * FROM tblProcument where ProjectID = " & objUrlEncoder.Decrypt(Request.QueryString("id")))

        With radProcumentListing

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("Procument") = .DataSource

        End With

    End Sub

    Private Sub radProcumentListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radProcumentListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            If e.CommandName = "View" Then

                Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
                Dim item As GridDataItem = radProcumentListing.Items(index)

                Dim ProcumentID As Long = Server.HtmlDecode(item("ProcumentID").Text)

                LoadProcument(ProcumentID)

            End If

        End If

    End Sub

    Private Sub radProcumentListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radProcumentListing.NeedDataSource

        radProcumentListing.DataSource = DirectCast(ViewState("Procument"), DataTable)

    End Sub
End Class

