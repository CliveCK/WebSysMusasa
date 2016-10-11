Public Class AnnualPlansPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadGrid()

        End If

    End Sub

    Public Sub LoadGrid()

        Dim objSurvey As New BusinessLogic.Survey(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim sql As String = "select O.Name as Organization, P.PlanYear, P.PlanQuarter, P.PlanMonth, A.Description As Activity "
        sql &= "from tblStrategicPlans S inner join tblStrategicPlanDetails P on S.StrategicPlanID = P.StrategicPlanID "
        sql &= "inner join tblOrganization O on O.OrganizationID = S.OrganizationID "
        sql &= "inner join tblActivities A on A.ActivityID = P.ActivityID "

        Dim ds As DataSet = objSurvey.GetSurvey(sql)

        With radAnnualPlan

            .DataSource = ds.Tables(0)
            .DataBind()

            ViewState("AnnualPlan") = .DataSource

        End With

    End Sub

    Private Sub radAnnualPlan_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radAnnualPlan.NeedDataSource

        radAnnualPlan.DataSource = DirectCast(ViewState("ÄnnualPlan"), DataTable)

    End Sub
End Class