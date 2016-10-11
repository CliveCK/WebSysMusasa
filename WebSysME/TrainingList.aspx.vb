Imports Telerik.Web.UI

Public Class TrainingList
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub


    Private Sub LoadGrid()

        Dim objTraining As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objUserGroup As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Dim sql As String

        If Not objUserGroup.RetrieveUserGroup(CookiesWrapper.thisUserID) Then
            sql = "SELECT TT.Description AS TrainingType ,T.* FROM tblTrainings T inner join luTrainingTypes TT on T.TrainingTypeID = TT.TrainingTypeID "
            sql &= " where TrainingID in (SELECT TrainingID FROM tblStaffTrainingAccess WHERE StaffID = " & CookiesWrapper.StaffID & ")"
        Else
            sql = "SELECT TT.Description AS TrainingType ,T.* FROM tblTrainings T inner join luTrainingTypes TT on T.TrainingTypeID = TT.TrainingTypeID "
        End If

        With radTraining

            .DataSource = objTraining.GetTraining(sql).Tables(0)
            .DataBind()

            ViewState("Trainings") = .DataSource

        End With


    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Response.Redirect("~/TrainingDetails.aspx")

    End Sub

    Private Sub radTraining_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radTraining.ItemCommand

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radTraining.Items(index1)
            Dim TrainingID As Integer

            TrainingID = Server.HtmlDecode(item1("TrainingID").Text)

            Response.Redirect("~/TrainingDetails?id=" & objUrlEncoder.Encrypt(TrainingID))

        End If

    End Sub

    Private Sub radTraining_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radTraining.NeedDataSource

        radTraining.DataSource = DirectCast(ViewState("Trainings"), DataTable)

    End Sub
End Class