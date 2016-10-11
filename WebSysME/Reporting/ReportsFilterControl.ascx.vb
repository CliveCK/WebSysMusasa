Public Partial Class ReportsFilterControl
    Inherits System.Web.UI.UserControl

    Public Event FilterReport(ByVal Criteria As String, ByRef Parameters As Hashtable, ByVal CriteriaSummary As String)
    Public Criteria As String = ""

    Public Sub LoadReportFilter(ByVal ReportID As Long)

        Dim ReportXMLString As String = ""

        Dim ds As DataSet = CacheWrapper.ReportsCache

        If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

            Dim dv As DataView = ds.Tables(0).DefaultView

            dv.RowFilter = "ReportID = " & ReportID

            If dv.Count > 0 Then

                If Not IsDBNull(dv(0)("XMLString")) AndAlso Trim(dv(0)("XMLString")) <> "" Then

                    ReportXMLString = dv(0)("XMLString")

                End If

            End If

            ucReportsFilter.GetControlsTable(ReportXMLString)

        End If

        txtReportID.Text = ReportID
        CookiesWrapper.thisReportID = ReportID

    End Sub


    Protected Sub cmdViewReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewReport.Click

        If Not ucReportsFilter.ControlsLoaded Then

            LoadReportFilter(txtReportID.Text)

        End If

        Criteria = ucReportsFilter.BuildCrystalCriteria
        Dim Parameters As Hashtable = ucReportsFilter.BuildCrystalParameters
        Dim CriteriaSummary As String = ucReportsFilter.BuildCriteriaSummary

        RaiseEvent FilterReport(Criteria, Parameters, CriteriaSummary)

        'Response.Redirect("Reports.aspx?ReportID=" & txtReportID.Text & "&Criteria=" & Criteria & "")

    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If IsNumeric(CookiesWrapper.thisReportID) Then

            LoadReportFilter(CookiesWrapper.thisReportID)

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Page.ClientScript.RegisterClientScriptInclude("radWindowingHelper", "../Scripts/radWindowingHelper.js")

    End Sub

    Public Sub BuildCriteriaSummary()

        ucReportsFilter.BuildCriteriaSummary()

    End Sub

End Class