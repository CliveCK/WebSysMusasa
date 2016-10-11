Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.Odbc
Imports CrystalDecisions.Web
Imports Telerik.Web.UI

Public Class ReportsTest
    Inherits System.Web.UI.Page
    Public reportName As String
    Public dtData As DataTable
    Public report As ReportDocument
    Public printer_name As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim RadPanelBar1 As RadPanelBar = DirectCast(Master.FindControl("RadPanelBar1"), RadPanelBar)

        RadPanelBar1.Visible = False

        ' ConfigureCrystalReports()

    End Sub

    Private Sub ConfigureCrystalReports()

        'Try
        '    Report = New ReportDocument()
        '    Dim reportPath As String = "C:\Users\Leslie\Desktop\CCPT\Custom Fields Appv2\TelerikWebApp1\TelerikWebApp1\SampleApp\Reports\trainingDetails.rpt"
        '    Report.Load(reportPath)
        '    Report.SetDataSource(dtData)
        '    crReports.ReportSource = report
        'Catch ex As Exception
        '    ''  MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        'End Try



        ' ReportDocument rptDoc = new ReportDocument();
        'dsSample ds = new dsSample(); // .xsd file name
        'DataTable dt = new DataTable();

        '// Just set the name of data table
        'dt.TableName = "Crystal Report Example";
        'dt = getAllOrders(); //This function is located below this function
        'ds.Tables[0].Merge(dt);

        '// Your .rpt file path will be below
        'rptDoc.Load(Server.MapPath("../Reports/SimpleReports.rpt"));

        '//set dataset to the report viewer.
        'rptDoc.SetDataSource(ds);
        'CrystalReportViewer1.ReportSource = rptDoc;


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' ConfigureCrystalReports()
        ' TextBox1.Text = DropDownList2.SelectedValue.ToString
    End Sub

    Private Sub Button1_Load(sender As Object, e As EventArgs) Handles Button1.Load

    End Sub

    Private Sub Button1_PreRender(sender As Object, e As EventArgs) Handles Button1.PreRender

    End Sub

    Private Sub DropDownList2_PreRender(sender As Object, e As EventArgs) Handles DropDownList2.PreRender
        ' TextBox1.Text = DropDownList2.SelectedValue.ToString
    End Sub

    Protected Sub DropDownList2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList2.SelectedIndexChanged
        ' TextBox1.Text = DropDownList2.SelectedValue.ToString
    End Sub
End Class
