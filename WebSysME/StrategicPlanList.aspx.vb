﻿Imports Telerik.Web.UI

Public Class StrategicPlanList
    Inherits System.Web.UI.Page
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Private Sub LoadGrid()

        Dim objStrategicPlan As New BusinessLogic.StrategicPlans(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "SELECT S.*, S.FromYear + '-' + S.ToYear As Period, O.Name as Organization FROM tblStrategicPlans S "
        sql &= "inner join tblOrganization O on S.OrganizationID = O.OrganizationID"

        With radStrategicListing

            .DataSource = objStrategicPlan.GetStrategicPlans(sql).Tables(0)
            .DataBind()

            ViewState("StratgeicPlan") = .DataSource

        End With

    End Sub

    Private Sub radStrategicListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radStrategicListing.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radStrategicListing.Items(index)

            Select Case e.CommandName

                Case "View"

                    Response.Redirect("~/StrategicPlansPage.aspx?id=" & objUrlEncoder.Encrypt(Server.HtmlDecode(item("StrategicPlanID").Text)))

                Case "Delete"

                    Dim objStrategicPlan As New BusinessLogic.StrategicPlans(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objStrategicPlan

                        .StrategicPlanID = Server.HtmlDecode(e.CommandArgument)

                        If .Delete() Then

                            ShowMessage("Strategic Plan deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub

    Private Sub radStrategicListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radStrategicListing.NeedDataSource

        radStrategicListing.DataSource = DirectCast(ViewState("StratgeicPlan"), DataTable)

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/StrategicPlansPage.aspx")

    End Sub
End Class