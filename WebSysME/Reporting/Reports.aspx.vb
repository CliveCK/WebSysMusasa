Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Reflection
Imports System.Security
Imports System.Xml
Imports System.IO
Imports Universal.CommonFunctions
Imports System.Drawing.Printing
Imports CrystalDecisions.Web
Imports System.Drawing


Partial Public Class Reports
    Inherits System.Web.UI.Page

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public ReportCriteria As String = ""
    Public ReportCriteriaSummary As String = ""

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblMessages.Text = Message.ToString
        pnlMessages.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblMessages.Text = Message.ToString
        If Message.InnerException IsNot Nothing Then lblMessages.Text &= " - " & Message.InnerException.ToString

        If Not LocalOnly Then RaiseEvent Message(Message.ToString, MessageType)

        pnlMessages.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region

    Private Sub Reports_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objReportID As Long = IIf(IsNumeric(Request.QueryString("ReportID")), Request.QueryString("ReportID"), 0)
            Dim objReportCriteria As String = Request.QueryString("Criteria")

            txtUserReportFilter.Text = objReportCriteria
            crvReports1.PrintMode = PrintMode.ActiveX

            'Get the users report permissions here and put them in a sessio

            If objReportID > 0 Then

                Dim dr As DataRow() = CacheWrapper.ReportsCache.Tables(0).Select("ReportID=" & objReportID)

                txtReportName.Text = dr(0)("ReportName")

                crvReports1.ToolPanelView = CBool(dr(0)("DisplayGroupTree"))

                If GetCookieBasedCriteria(objReportID) <> "" Then

                    txtUserReportFilter.Text = GetCookieBasedCriteria(objReportID)

                End If

            End If
            lblReports.Text = "Reports"
            lblReports.ForeColor = Color.Black

            ConfigureCrystalReports()
            LoadTreeViewData()

        End If

        If Page.IsPostBack Then

            crvReports1.ReportSource = CType(Session.Item("REPORT_KEY"), ReportDocument)
            ' ConfigureCrystalReports()

        End If

        Page.ClientScript.RegisterClientScriptInclude("jQuery", My.Settings.jQueryScriptFile)
        ' UserInterfaceHelper.CSSHelper.AddCSSToPage(Page, "ApplicationSkins/default/styles/Messages.css")


    End Sub

    Private Sub LoadNavigationPanel()

        ''we need to add report navigation control and get rid of the Crystal toolbar which is causing probles
        ''we are adding 5 items i.e. print, first, last next and previos page buttons

        radTbarReportOperations.Items.Clear()
        'radTbarReportOperations.Width = 100

        For i As Integer = 0 To 5

            Dim radTlb As New Telerik.Web.UI.RadToolBarButton

            Dim Action As String = ""
            Dim Text As String = ""
            Dim ImageName As String = ""
            Dim ToolTipText = ""


            With radTbarReportOperations

                Select Case i

                    Case 0
                        Action = "First"
                        Text = "First"
                        ImageName = "~/images/icons/first.gif"
                        ToolTipText = "Go To First"
                    Case 1
                        Action = "Previous"
                        Text = "Previous"
                        ImageName = "~/images/icons/previous.gif"
                        ToolTipText = "Go To Previous"
                    Case 2
                        Action = "Next"
                        Text = "Next"
                        ImageName = "~/images/icons/rnext.gif"
                        ToolTipText = "Go To Next"
                    Case 3
                        Action = "Last"
                        Text = "Last"
                        ImageName = "~/images/icons/last.gif"
                        ToolTipText = "Go To Last"
                    Case 4
                        Action = "Print"
                        Text = "Print"
                        ImageName = "~/images/icons/print.gif"
                        ToolTipText = "Print"

                    Case 5
                        Action = "Group"
                        Text = "Display Group Tree"
                        ImageName = "~/images/icons/grouptree.gif"
                        ToolTipText = "Display Group Tree"

                End Select

                With radTlb

                    .CommandName = Action
                    .Text = Text
                    '.DisplayType = Telerik.Web.UI.RadToolbarButton.ButtonDisplayType.ImageOnly
                    .ImageUrl = ImageName
                    .Attributes.Add("runat", "server")
                    .ToolTip = ToolTipText

                End With

                .Items.Add(radTlb)

            End With

        Next

    End Sub

    Private Sub LoadActionsPanel(ByVal XMLString As String)

        Try

            Dim ds As DataSet = CType(CacheWrapper.ReportsCache, DataSet)
            Dim sr As IO.StringReader = New IO.StringReader(XMLString)

            Dim dsActions As New DataSet
            dsActions.ReadXml(sr, XmlReadMode.InferSchema)

            For Each mRow As DataRow In dsActions.Tables(0).Rows

                Dim Action As String = mRow("value")
                Dim Text As String = mRow("Text")

                With radTbarReportOperations

                    Dim radTlb As New Telerik.Web.UI.RadToolBarButton

                    Select Case Action

                        Case "Email"

                            With radTlb

                                .CommandName = Action
                                .Text = Text

                                '.DisplayType = Telerik.Web.UI.RadToolBarButton.ButtonDisplayType.TextImage
                                .ImageUrl = "~/images/icons/e-mail.gif"
                                .Attributes.Add("runat", "server")

                            End With

                        Case "Print"

                            With radTlb
                                .CommandName = Action
                                .Text = Text
                                '.DisplayType = Telerik.Web.UI.RadToolBarButton.ButtonDisplayType.TextImage
                                .ImageUrl = "~/images/icons/Print.gif"
                                .Attributes.Add("runat", "server")
                            End With

                        Case "Save"

                            With radTlb
                                .CommandName = Action
                                .Text = Text
                                '.DisplayType = Telerik.Web.UI.RadToolBarButton.ButtonDisplayType.TextImage
                                .ImageUrl = "~/images/icons/pdficon.gif"
                                .Attributes.Add("runat", "server")
                            End With

                        Case "SaveWord"

                            With radTlb
                                .CommandName = Action
                                .Text = Text
                                '.DisplayType = Telerik.Web.UI.RadToolBarButton.ButtonDisplayType.TextImage
                                .ImageUrl = "~/images/icons/Word2007-Small.jpg"
                                .Attributes.Add("runat", "server")
                            End With

                        Case "SaveExcel"

                            With radTlb
                                .CommandName = Action
                                .Text = Text
                                '.DisplayType = Telerik.Web.UI.RadToolbarButton.ButtonDisplayType.TextImage
                                .ImageUrl = "~/images/icons/Excel2007-Small.jpg"
                                .Attributes.Add("runat", "server")
                            End With

                        Case "SaveCSV"

                            With radTlb
                                .CommandName = Action
                                .Text = Text
                                '.DisplayType = Telerik.WebControls.RadToolbarButton.ButtonDisplayType.TextImage
                                .ImageUrl = "~/images/icons/Office-excel-csv-icon.jpg"
                                .Attributes.Add("runat", "server")
                            End With

                        Case "Fax"

                            With radTlb
                                .CommandName = Action
                                .Text = Text
                                '.DisplayType = Telerik.Web.UI.RadToolBarButton.ButtonDisplayType.TextImage
                                .ImageUrl = "~/images/fax.gif"
                                .Attributes.Add("runat", "server")
                            End With

                    End Select

                    .Items.Add(radTlb)

                End With

            Next

        Catch : End Try

    End Sub

    Public Sub LoadTreeViewData()

        Dim myDS As DataSet = CType(CacheWrapper.ReportsCache, DataSet)

        With tvwReportsCategories

            .DataFieldID = "ReportID"
            .DataFieldParentID = "ParentID"
            .DataTextField = "Description"
            .DataValueField = "ReportID"
            .DataSource = myDS
            .DataBind()
            .ExpandAllNodes()

        End With

    End Sub

    Public Sub tvwReportsCategories_NodeClick(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles tvwReportsCategories.NodeClick

        Try

            txtReportID.Text = e.Node.Value
            CookiesWrapper.thisReportID = e.Node.Value
            txtSaveAs.Text = "-"
            txtSaveAsExcel.Text = "-"
            txtSaveAsWord.Text = "-"
            txtSaveAsCSV.Text = "-"
            txtUserReportFilter.Text = ""
            txtReportName.Text = ""

        Catch ex As Exception
            log.Error(ex)
            ShowMessage(ex.Message, MessageTypeEnum.Error)
        End Try

    End Sub

    Private Sub tvwReportsCategories_NodeBound(ByVal o As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles tvwReportsCategories.NodeDataBound

        Dim row As DataRowView = e.Node.DataItem
        Dim node As Telerik.Web.UI.RadTreeNode = e.Node

        Try

            If Not IsDBNull(row("XMLString")) AndAlso Trim(row("XMLString")) <> "" Then

                Dim XMLDoc As New System.Xml.XmlDocument
                Dim UserInputFields As System.Xml.XmlNode

                XMLDoc.LoadXml(row("XMLString").ToString)

                UserInputFields = XMLDoc.SelectSingleNode("//UserInputFields/UserInputField")

                If Not IsNothing(UserInputFields) AndAlso UserInputFields.HasChildNodes Then

                    node.Category = "HasUserInput=1"

                Else

                    node.Category = "HasUserInput=0"

                End If

            End If

            'apply permissins

        Catch ex As Exception

            node.Text &= "* - Error Loading"

        End Try

    End Sub

    Private Function GetConnectionString() As String

        Return ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString

    End Function

    Private Sub ConfigureCrystalReports(Optional ByVal MemberID As Integer = 0, Optional ByVal rptPath As String = "", Optional ByRef crReportDocument As ReportDocument = Nothing, Optional ByVal Criteria As String = "", Optional ByRef Parameters As Hashtable = Nothing, Optional ByVal CriteriaSummary As String = "")
        '   Try
        '      crReportDocument.Dispose()
        '   Catch ex As Exception

        '  End Try
        crvReports1.Visible = True
        Try

            Dim con As String = ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString
            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim builder As New SqlClient.SqlConnectionStringBuilder(GetConnectionString())

            builder.ConnectionString = con
            builder.AsynchronousProcessing = True

            Dim ReportPath As String = ""
            Dim reportfilename As String = ""

            CacheWrapper.ReportsCache.Tables(
0).DefaultView.RowFilter = "ReportName IS NOT NULL"

            If CacheWrapper.ReportsCache.Tables(0).DefaultView.Count > 0 Then

                ' IMPORTANT!!: make sure you have added the BlankReport.rpt file - flanagan
                'ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Settings\" & "ConnectionString" & "\Reports\BlankReport.rpt"

            End If

            If txtReportName.Text <> "" Then

                ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Settings\" & CookiesWrapper.thisConnectionName & "\" & txtReportName.Text & ".rpt"
                reportfilename = txtReportName.Text

            Else

                'NOTE:we need to check if we are loading a black ReportDoc. if we are proceed and show it. If not exit the sub and display error msg - flanagan

                If Not ReportPath.Contains("\BlankReport.rpt") Then

                    'no report will be displayed so we need to hide the reports filter before we exit the sub here - flanagan
                    ' ucReportsFilterControl.Visible = False
                    radpCriteria.Visible = False
                    reportfilename = ""
                    Exit Sub

                End If

            End If

            'we just need to make sure that the filter is not shown if we are showing BlankReport
            ' ucReportsFilterControl.Visible = Not ReportPath.Contains("\BlankReport.rpt")
            radpCriteria.Visible = Not ReportPath.Contains("\BlankReport.rpt")

            If Not System.IO.File.Exists(ReportPath) Then

                ShowMessage("Report not found: '" & ReportPath & "'", MessageTypeEnum.Error)
                Exit Sub

            End If


            If IsNothing(crReportDocument) Then
                Dim cReportDocument As New ReportDocument
                crReportDocument = cReportDocument
            End If

            crReportDocument.Load(ReportPath)

            'If we have cookie based criteria, we can immediately filter and display
            If IsNumeric(txtReportID.Text) Then

                Dim CookiesCriteria As String = GetCookieBasedCriteria(txtReportID.Text)
                txtUserReportFilter.Text &= CookiesCriteria

            End If

            Dim myConnectionInfo As ConnectionInfo = New ConnectionInfo()

            With myConnectionInfo
                .ServerName = builder.DataSource
                .DatabaseName = builder.InitialCatalog
                .UserID = builder.UserID
                .Password = builder.Password
            End With

            crReportDocument.RecordSelectionFormula = Criteria
            crReportDocument.SummaryInfo.ReportComments = CriteriaSummary
            crReportDocument.SummaryInfo.ReportAuthor = CookiesWrapper.thisUserFullName
            crReportDocument.DataDefinition.RecordSelectionFormula = txtUserReportFilter.Text
            crReportDocument.SummaryInfo.ReportTitle = txtReportTitle.Text

            If Not IsNothing(Parameters) AndAlso Parameters.Count > 0 Then

                For Each de As DictionaryEntry In Parameters

                    crReportDocument.SetParameterValue(de.Key, de.Value)

                Next
            Else
                If Session("Parameters") IsNot Nothing Then

                    Parameters = Session("Parameters")

                    For Each de As DictionaryEntry In Parameters

                        crReportDocument.SetParameterValue(de.Key, de.Value)

                    Next

                End If

            End If

            SetDBLogonForReport(myConnectionInfo, crReportDocument)
            SetDBLogonForSubreports(myConnectionInfo, crReportDocument)

            Session.Add("REPORT_KEY", crReportDocument)
            crvReports1.ReportSource = crReportDocument
            '   crReportDocument.Dispose()
            'If Not IsNothing(crReportDocument) Then
            '    crReportDocument.Dispose()
            'End If
        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            log.Error(ex)

        End Try

    End Sub

    Private Sub LoadMessageReport(Optional ByVal MemberID As Integer = 0, Optional ByVal rptPath As String = "")

        Try


            Dim con As String = ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString
            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim builder As New SqlClient.SqlConnectionStringBuilder(GetConnectionString())

            builder.ConnectionString = con
            builder.AsynchronousProcessing = True

            Dim ReportPath As String = ""

            ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Settings\" & CookiesWrapper.thisConnectionName & "\Reports\MessageReport.rpt"

            If Not System.IO.File.Exists(ReportPath) Then
                ShowMessage("Report not found: '" & ReportPath & "'", MessageTypeEnum.Error)
                Exit Sub
            End If

            Dim crReportDocument As New ReportDocument
            crReportDocument.Load(ReportPath)

            crvReports1.ReportSource = crReportDocument

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)

        End Try

    End Sub

    Protected Sub txtReportID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReportID.TextChanged

        Try
            Dim dr As DataRow() = CacheWrapper.ReportsCache.Tables(0).Select("ReportID = " & txtReportID.Text)

            txtReportName.Text = dr(0)("ReportName")
            txtHasParameters.Text = CBool(dr(0)("HasParameters"))
            Dim newrpt As New ReportDocument
            Dim rpt As ReportDocument = TryCast(crvReports1.ReportSource, ReportDocument)
            If Not IsNothing(rpt) Then
                rpt.Dispose()
                crvReports1.ReportSource = newrpt
            End If
            'clear error messages raised from last report
            ClearDisplayLabel()

            Dim reportnode As Telerik.Web.UI.RadTreeNode = tvwReportsCategories.FindNodeByValue(txtReportID.Text)

            If reportnode.Category = "HasUserInput=1" Then
                'LoadMessageReport()
                crvReports1.Visible = False
                lblReports.Text = "Enter report criteria above to view report"
                lblReports.ForeColor = Color.Blue
                lblReports.Font.Size = 25
            Else
                lblReports.Text = ""
                lblReports.ForeColor = Color.Black
                ConfigureCrystalReports()
            End If

            radpCriteria.Visible = reportnode IsNot Nothing AndAlso reportnode.Category = "HasUserInput=1"
            txtReportTitle.Text = reportnode.Text

            lblReportName.Text = reportnode.GetFullPath(" \ ")

            LoadNavigationPanel()
            LoadPrinters()

            'Load Actions panel: The actions panel is always specific to the current report
            Dim dt As DataTable = CacheWrapper.ReportsCache.Tables(0)
            dt.Select("ReportID = " & txtReportID.Text)

            Dim row As DataRow = dt.Rows(0)

            If Catchnull(row("ReportActionsXML"), "") <> "" Then LoadActionsPanel(row("ReportActionsXML"))

            txtReportID.Text = ""

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)

        End Try

    End Sub

    Private Sub ClearDisplayLabel()

        lblMessages.CssClass = ""
        lblMessages.Text = ""

    End Sub

    Function GetCookieBasedCriteria(ByVal objReportID As Integer)

        Dim myDT As DataTable = CType(CacheWrapper.ReportsCache.Tables(0), DataTable)

        Dim dr As DataRow() = Nothing

        If txtReportID.Text <> "" Then

            dr = myDT.Select("ReportID=" & txtReportID.Text & "")

        End If

        If objReportID >= 0 Then

            dr = myDT.Select("ReportID=" & objReportID & "")

        End If

        Dim XMLDoc As New System.Xml.XmlDocument
        Dim XmlNodeList As System.Xml.XmlNodeList

        Dim crit As String = ""

        XMLDoc.LoadXml(dr(0)("XMLString"))

        XmlNodeList = XMLDoc.SelectNodes("//Cookies/Cookie")

        For Each node As XmlNode In XmlNodeList

            Try

                Dim DBField As String = node("DBField").InnerText()

                Dim Cookie As String = ""

                Cookie = GetObjectValueUsingReflection(New CookiesWrapper, node("Name").InnerText, "")

                If Not IsNumeric(Cookie) Then Cookie = "'" & Cookie.Replace("'", "''") & "'"
                crit &= IIf(crit <> "", " AND ", "") & "" & "" & DBField & "  = " & Cookie & ""

            Catch ex As Exception

                Debug.WriteLine(ex)

            End Try

        Next

        Return crit

    End Function

    Private Function GetObjectValueUsingReflection(ByRef obj As Object, ByVal propName As String, ByVal ValueIfNothing As Object) As Object

        'NOTE: The property name is case sensitive
        Dim objMemberInfo() As MemberInfo = obj.GetType.GetMember(propName)

        If objMemberInfo.Length > 0 AndAlso (objMemberInfo(0).MemberType = MemberTypes.Property Or objMemberInfo(0).MemberType = MemberTypes.Field) Then

            Dim val = CType(objMemberInfo(0), PropertyInfo).GetValue(obj, Nothing)

            If IsNothing(val) Then val = ValueIfNothing

            Return val

        End If

        Return Nothing

    End Function

    Private Sub crvReports_Error(ByVal source As Object, ByVal e As CrystalDecisions.Web.ErrorEventArgs) Handles crvReports1.Error
        e.Handled = True
        Dim objerr As String = ""
        objerr = "Crystal Reports Error. <br />" & vbCrLf
        objerr &= "Stage: " & [Enum].GetName(GetType(CrystalDecisions.Web.EnumAspNetLifeCycleStage), e.AspNetLifeCycleStage) & "<br />"
        If e.AspNetLifeCycleException IsNot Nothing Then objerr &= "Exception: " & e.AspNetLifeCycleException.ToString & "<br />"
        objerr &= "ErrorMessage: " & e.ErrorMessage
        'ShowMessage(objerr, MessageTypeEnum.Error)
        log.Error(objerr)

    End Sub

    Private Sub crvReports_Navigate(ByVal source As Object, ByVal e As CrystalDecisions.Web.NavigateEventArgs) Handles crvReports1.Navigate

        Try
            e.Handled = False

            If Not IsNothing(CType(Session.Item("REPORT_KEY"), ReportDocument)) Then
                crvReports1.ReportSource = CType(Session.Item("REPORT_KEY"), ReportDocument)
                '   ConfigureCrystalReports()
            End If

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)

        End Try

    End Sub

    Private Sub txtEmail_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmail.TextChanged

        txtEmail.Text = ""

        Response.Redirect("~/Distribution/Email.aspx?ReportAttachment=" & txtReportName.Text)

    End Sub

    Private Sub txtSaveAs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSaveAs.TextChanged

        If txtSaveAs.Text <> "savereport" Then
            Exit Sub
        End If
        txtSaveAs.Text = ""
        Try

            Dim oStream As New IO.MemoryStream
            Dim DiskOutPut As New DiskFileDestinationOptions

            Dim con As String = ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString
            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim builder As New SqlClient.SqlConnectionStringBuilder(GetConnectionString())

            builder.ConnectionString = con
            builder.AsynchronousProcessing = True

            Dim ReportPath As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Settings\" & CookiesWrapper.thisConnectionName & "\" & txtReportName.Text & ".rpt"
            Dim crReportDocument As New ReportDocument

            If IsNothing(CType(Session.Item("REPORT_KEY"), ReportDocument)) Then
                crReportDocument.Load(ReportPath)

                Dim myConnectionInfo As ConnectionInfo = New ConnectionInfo()

                With myConnectionInfo
                    .ServerName = builder.DataSource
                    .DatabaseName = builder.InitialCatalog
                    .UserID = builder.UserID
                    .Password = builder.Password
                End With

                SetDBLogonForReport(myConnectionInfo, crReportDocument)
                SetDBLogonForSubreports(myConnectionInfo, crReportDocument)

                crReportDocument.DataDefinition.RecordSelectionFormula = txtUserReportFilter.Text
                ' crvReports1.ReportSource = crReportDocument
            Else
                crReportDocument = CType(Session.Item("REPORT_KEY"), ReportDocument)
            End If
            With Response
                .Clear()
                .ContentType = "application/octet-stream"
            End With

            'txtSaveAs.Text = ""
            crReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, IIf(txtReportName.Text <> "", txtReportName.Text, "Report"))

        Catch ex As Exception

        End Try
    End Sub

    Protected Overrides Sub OnError(ByVal e As System.EventArgs)

        Dim exp As Exception = Server.GetLastError()
        Response.Write("Some Error Occured while executing this page : " & exp.Message & "")

        Server.ClearError()

        Response.End()

    End Sub

    Public Sub ucReportsFilterControl_FilterReport(ByVal Criteria As String, ByRef Parameters As Hashtable, ByVal CriteriaSummary As String)

        txtSaveAs.Text = ""
        txtSaveAsExcel.Text = ""
        txtSaveAsWord.Text = ""
        txtUserReportFilter.Text = Criteria
        lblReports.Text = ""
        Session("Parameters") = Parameters
        ConfigureCrystalReports(, , , Criteria, Parameters, CriteriaSummary)

    End Sub

    Private Sub txtFirst_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFirst.TextChanged
        Me.crvReports1.ShowFirstPage()
    End Sub

    Private Sub txtLast_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLast.TextChanged
        Me.crvReports1.ShowLastPage()
    End Sub

    Private Sub txtNext_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNext.TextChanged
        Me.crvReports1.ShowNextPage()
    End Sub

    Private Sub txtPrevious_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPrevious.TextChanged
        Me.crvReports1.ShowPreviousPage()
    End Sub

    Private Sub txtSaveAsExcel_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSaveAsExcel.TextChanged

        If txtSaveAsExcel.Text <> "saveasexcel" Then
            Exit Sub
        End If
        txtSaveAs.Text = ""
        Try

            Dim oStream As New IO.MemoryStream
            Dim DiskOutPut As New DiskFileDestinationOptions

            Dim con As String = ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString
            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim builder As New SqlClient.SqlConnectionStringBuilder(GetConnectionString())

            Dim ReportPath As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Settings\" & CookiesWrapper.thisConnectionName & "\" & txtReportName.Text & ".rpt"

            Dim crReportDocument As New ReportDocument

            If IsNothing(CType(Session.Item("REPORT_KEY"), ReportDocument)) Then
                crReportDocument.Load(ReportPath)

                Dim myConnectionInfo As ConnectionInfo = New ConnectionInfo()

                With myConnectionInfo
                    .ServerName = builder.DataSource
                    .DatabaseName = builder.InitialCatalog
                    .UserID = builder.UserID
                    .Password = builder.Password
                End With

                SetDBLogonForReport(myConnectionInfo, crReportDocument)
                SetDBLogonForSubreports(myConnectionInfo, crReportDocument)

                crReportDocument.DataDefinition.RecordSelectionFormula = txtUserReportFilter.Text
                ' crvReports1.ReportSource = crReportDocument
            Else
                crReportDocument = CType(Session.Item("REPORT_KEY"), ReportDocument)
            End If

            With Response
                .Clear()
                .ContentType = "application/octet-stream"
            End With

            'txtSaveAs.Text = ""
            crReportDocument.ExportToHttpResponse(ExportFormatType.Excel, Response, True, IIf(txtReportName.Text <> "", txtReportName.Text, "Report"))
            '   crReportDocument.Dispose()

        Catch ex As Exception
            log.Error(ex)
            ShowMessage(ex.Message, MessageTypeEnum.Error)
        End Try

    End Sub

    Private Sub txtSaveAsWord_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSaveAsWord.TextChanged
        If txtSaveAsWord.Text <> "saveasword" Then
            Exit Sub
        End If
        txtSaveAs.Text = ""
        Try

            Dim oStream As New IO.MemoryStream
            Dim DiskOutPut As New DiskFileDestinationOptions

            Dim con As String = ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString
            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim builder As New SqlClient.SqlConnectionStringBuilder(GetConnectionString())

            builder.ConnectionString = con
            builder.AsynchronousProcessing = True

            Dim ReportPath As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Settings\" & CookiesWrapper.thisConnectionName & "\" & txtReportName.Text & ".rpt"

            Dim crReportDocument As New ReportDocument

            If IsNothing(CType(Session.Item("REPORT_KEY"), ReportDocument)) Then
                crReportDocument.Load(ReportPath)

                Dim myConnectionInfo As ConnectionInfo = New ConnectionInfo()

                With myConnectionInfo
                    .ServerName = builder.DataSource
                    .DatabaseName = builder.InitialCatalog
                    .UserID = builder.UserID
                    .Password = builder.Password
                End With

                SetDBLogonForReport(myConnectionInfo, crReportDocument)
                SetDBLogonForSubreports(myConnectionInfo, crReportDocument)

                crReportDocument.DataDefinition.RecordSelectionFormula = txtUserReportFilter.Text
                ' crvReports1.ReportSource = crReportDocument
            Else
                crReportDocument = CType(Session.Item("REPORT_KEY"), ReportDocument)
            End If

            With Response
                .Clear()
                .ContentType = "application/octet-stream"
            End With

            'txtSaveAs.Text = ""
            crReportDocument.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, IIf(txtReportName.Text <> "", txtReportName.Text, "Report"))
            '   crReportDocument.Dispose()

        Catch ex As Exception
            log.Error(ex)
            ShowMessage(ex.Message, MessageTypeEnum.Error)
        End Try
    End Sub

    Private Sub txtSaveAsCSV_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSaveAsCSV.TextChanged

        If txtSaveAsCSV.Text <> "saveascsv" Then
            Exit Sub
        End If
        txtSaveAs.Text = ""

        Try

            Dim oStream As New IO.MemoryStream
            Dim DiskOutPut As New DiskFileDestinationOptions

            Dim con As String = ConfigurationManager.ConnectionStrings(CookiesWrapper.thisConnectionName).ConnectionString
            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = New Microsoft.Practices.EnterpriseLibrary.Data.DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Dim builder As New SqlClient.SqlConnectionStringBuilder(GetConnectionString())

            builder.ConnectionString = con
            builder.AsynchronousProcessing = True

            Dim ReportPath As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Settings\" & CookiesWrapper.thisConnectionName & "\" & txtReportName.Text & ".rpt"

            Dim crReportDocument As New ReportDocument

            If IsNothing(CType(Session.Item("REPORT_KEY"), ReportDocument)) Then
                crReportDocument.Load(ReportPath)

                Dim myConnectionInfo As ConnectionInfo = New ConnectionInfo()

                With myConnectionInfo
                    .ServerName = builder.DataSource
                    .DatabaseName = builder.InitialCatalog
                    .UserID = builder.UserID
                    .Password = builder.Password
                End With

                SetDBLogonForReport(myConnectionInfo, crReportDocument)
                SetDBLogonForSubreports(myConnectionInfo, crReportDocument)

                crReportDocument.DataDefinition.RecordSelectionFormula = txtUserReportFilter.Text
                ' crvReports1.ReportSource = crReportDocument
            Else
                crReportDocument = CType(Session.Item("REPORT_KEY"), ReportDocument)
            End If
            With Response
                .Clear()
                .ContentType = "application/octet-stream"
            End With

            'txtSaveAs.Text = ""
            crReportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, True, IIf(txtReportName.Text <> "", txtReportName.Text, "Report"))
            '   crReportDocument.Dispose()

        Catch ex As Exception
            log.Error(ex)
            ShowMessage(ex, MessageTypeEnum.Error)
        End Try

    End Sub

    Private Sub txtDisplayGroupTree_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDisplayGroupTree.TextChanged
        If crvReports1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None Then
            crvReports1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.GroupTree
        ElseIf crvReports1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.GroupTree Then
            crvReports1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None

        End If
    End Sub

    Private Sub LoadPrinters()

        cboPrinters.Items.Clear()
        cboPrinters.Items.Add("-Select Printer-")
        Dim pkInstalledPrinters As String
        ' Find all printers installed
        For Each pkInstalledPrinters In PrinterSettings.InstalledPrinters
            cboPrinters.Items.Add(pkInstalledPrinters)
        Next pkInstalledPrinters
    End Sub

    Private Sub SetDBLogonForReport(ByVal myConnectionInfo As ConnectionInfo, ByVal myReportDocument As ReportDocument)

        Dim myTables As Tables = myReportDocument.Database.Tables

        For Each myTable As CrystalDecisions.CrystalReports.Engine.Table In myTables
            Dim myTableLogonInfo As TableLogOnInfo = myTable.LogOnInfo
            myTableLogonInfo.ConnectionInfo = myConnectionInfo
            myTable.ApplyLogOnInfo(myTableLogonInfo)
        Next

    End Sub

    Private Sub SetDBLogonForSubreports(ByVal myConnectionInfo As ConnectionInfo, ByVal myReportDocument As ReportDocument)

        Dim mySections As Sections = myReportDocument.ReportDefinition.Sections

        For Each mySection As Section In mySections

            Dim myReportObjects As ReportObjects = mySection.ReportObjects

            For Each myReportObject As ReportObject In myReportObjects
                If myReportObject.Kind = ReportObjectKind.SubreportObject Then
                    Dim mySubreportObject As SubreportObject = CType(myReportObject, SubreportObject)
                    Dim subReportDocument As ReportDocument = mySubreportObject.OpenSubreport(mySubreportObject.SubreportName)
                    SetDBLogonForReport(myConnectionInfo, subReportDocument)
                End If
            Next

        Next


    End Sub

    Private Sub SetCurrentValuesForParameterField(ByVal myParameterFields As ParameterFields, ByVal myArrayList As ArrayList)

        Dim currentParameterValues As ParameterValues = New ParameterValues()
        For Each submittedValue As Object In myArrayList
            Dim myParameterDiscreteValue As ParameterDiscreteValue = New ParameterDiscreteValue()
            myParameterDiscreteValue.Value = submittedValue.ToString()
            currentParameterValues.Add(myParameterDiscreteValue)
        Next

        'Dim myParameterField As ParameterField = myParameterFields(PARAMETER_FIELD_NAME)

    End Sub

End Class
