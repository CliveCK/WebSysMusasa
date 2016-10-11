Imports Telerik.Web.UI
Imports BusinessLogic

Partial Public Class ReportPermissionsControl
    Inherits System.Web.UI.UserControl

    Public Event DisplayMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadUsers()
            LoadRoles()
            LoadReports()
            pnlCodes.Enabled = False

        End If

    End Sub

    Sub LoadUsers()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With lstUsers

            '.Items.Clear 'CHECK: Is it necessary to clear items before we load new data?
            .DataTextField = "Username"
            .DataValueField = "UserID"
            .DataSource = objLookup.Lookup("tblUsers", "UserID", "Username", "Username", "Deleted=0 AND [Type] = 'SystemUser'").Tables(0)
            .DataBind()

        End With

    End Sub

    Sub LoadRoles()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With lstRoles

            '.Items.Clear 'CHECK: Is it necessary to clear items before we load new data?
            .DataTextField = "Description"
            .DataValueField = "UserGroupID"
            .DataSource = objLookup.Lookup("luUserGroups", "UserGroupID", "Description", "Description").Tables(0)
            .DataBind()

        End With

    End Sub

    Private Sub LoadReports()

        With radTreeCategories


            Dim objReport As New Report(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            Dim ds As DataSet = objReport.GetReports()

            .DataTextField = "Description"
            .DataValueField = "ReportID"
            .DataFieldID = "ReportID"
            .DataFieldParentID = "ParentID"
            .DataSource = ds
            .DataBind()

            .Visible = True

            .ExpandAllNodes()

        End With

    End Sub

    Private Sub radTreeCategories_NodeBound(ByVal o As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles radTreeCategories.NodeDataBound

        Try

            Dim dr As DataRowView = e.Node.DataItem

            If Not dr Is Nothing Then

                'e.Node.Attributes.Remove("IsContainerOnly")
                'e.Node.Attributes.Add("IsContainerOnly", IIf(IsDBNull(dr("IsContainerOnly")), "", dr("IsContainerOnly")))

                'e.Node.Attributes.Remove("IsSubscriptionBased")
                'e.Node.Attributes.Add("IsSubscriptionBased", IIf(IsDBNull(dr("IsSubscriptionBased")), "", dr("IsSubscriptionBased")))

                'e.Node.Enabled = CBool(dr("IsSubscriptionBased"))
                If e.Node.Enabled Then e.Node.CollapseParentNodes() 'just to make sure we can reach all subscription based nodes

                If txtUserID.Text > 0 Then

                    Dim ds As DataSet = Session("UserReportRights")

                    If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                        For Each row As DataRow In ds.Tables(0).Rows

                            If e.Node.Value = row("ReportID") Then
                                e.Node.Checked = True
                            End If

                        Next

                    End If

                End If

            End If

        Catch : End Try

    End Sub

    Protected Function SaveUsercheckedNodes(ByVal UserID As Long) As Boolean

        Dim NodeFullPath As String = ""
        Dim CategoryID As Integer
        ' Dim nodeCollection As Telerik.Web.UI.RadTreeNodeCollection = radTreeCategories.CheckedNodes

        'Dim node As RadTreeNode

        Try

            Dim objUserReportAccessRight As New Global.SysPermissionsManager.UserReportAccessRight(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim lstUserType As WebControls.ListBox = lstUsers

            If txtUserType.Text = "UserGroup" Then lstUserType = lstRoles

            If radTreeCategories.CheckedNodes.Count > 0 Then
                'revoke existing transaction
                objUserReportAccessRight.Revoke(UserID, txtUserType.Text)

                Dim dsReport As DataSet = CreateErrorDataset()

                For Each node As RadTreeNode In radTreeCategories.CheckedNodes


                    NodeFullPath = node.FullPath
                    CategoryID = node.Value
                    objUserReportAccessRight.SaveDetail(lstUserType.SelectedValue, CategoryID, txtUserType.Text) ' save the code!

                Next node
                DisplayErrors(dsReport)
                Return True

            Else

                Return False

            End If

        Catch ex As Exception
            Return False
        End Try

        'nodes.Text = message

    End Function

    'Private Function SaveTransaction(ByVal CategoryID As Long) As Boolean

    '    Try

    '        Dim objPayeeTransaction As New Pensions.PayrollAdministration.PayeeTransaction(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
    '        Dim objCategory As New DataManagement.SQLHeirarchies.Category(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
    '        Dim objCategoryOptions As New Pensions.PayrollAdministration.CategoryOptions(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
    '        Dim objPayee As New Pensions.PayrollAdministration.Payee(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    '        With objPayeeTransaction

    '            .PayTransactionID = 0 ' we are inserting
    '            .PayrollID = objPayee.PayInformation.PayrollID
    '            .PayeeID = objPayee.PayeeID
    '            .PeriodID = cboPeriod.SelectedValue
    '            .Reference = ""

    '            If CategoryID = Options.BasicCodeCategoryID Then
    '                the category is  basic. Basic is the core af any payroll system 
    '                we get the rate from Payee Master, NEC Grade, Internal Grade
    '                Select Case objPayee.PayInformation.BaseRateOn

    '                    Case BasicRate.PayeeMaster
    '                        .UnitValue = objPayee.PayInformation.RateOfPay
    '                    Case BasicRate.InternalGrade
    '                        Dim objInternalGrade As New Pensions.PayrollAdministration.InternalGrades(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
    '                        If objInternalGrade.Retrieve(objPayee.WorkInformation.GradeID) Then
    '                            might need to get houly rate, daily etc depending on payroll type
    '                            .UnitValue = objInternalGrade.PeriodRate

    '                        End If

    '                    Case BasicRate.NECGrade

    '                        Dim objNECGrade As New Pensions.PayrollAdministration.NECGrade(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
    '                        If objNECGrade.Retrieve(objPayee.WorkInformation.NECGradeID) Then
    '                            might need to get houly rate, daily etc depending on payroll type
    '                            .UnitValue = objNECGrade.PeriodRate

    '                        End If

    '                End Select

    '            Else
    '                .UnitValue = objCategoryOptions.DefaultUnits
    '            End If

    '            .UnitTypeID = objCategoryOptions.DefaultUnitTypeID

    '            we need to calculate amount here. This will depend on the default units, unit type and unit value

    '            .Amount = txtAmount.Text
    '            .InputMethodTypeID = 1
    '            .Message = chkMessage.Checked
    '            .TransactionStatusID = 1 '1: input, 2: Processed
    '            .ProcessingDate = txtProcessingDate.Text
    '            .EDCode = objCategory.Code

    '            .CurrencyTypeID = objPayee.PayInformation.de

    '            .CategoryID = CategoryID
    '            .DurationID = objCategoryOptions.DurationID

    '            If .Save Then

    '                If Not IsNumeric(txtPayTransactionID.Text) OrElse Trim(txtPayTransactionID.Text) = 0 Then txtPayTransactionID.Text = .PayTransactionID

    '                Return True

    '            Else

    '                lblError.Text = .ErrorMessage
    '                Return False

    '            End If

    '        End With


    '    Catch ex As Exception

    '        RaiseEvent DisplayMessage(ex.ToString, MessageTypeEnum.Error)
    '        Return False

    '    End Try

    'End Function

    Private Function DisplayErrors(ByVal dsErrors As DataSet) As Boolean

        If dsErrors.Tables(0).Rows.Count > 0 Then

            Dim msg As String = ""

            For Each row As DataRow In dsErrors.Tables(0).Rows

                msg = "<span class = 'Error'>" & row("Details") & " </span> <br/>"

            Next

            'lblReport.text = msg

        End If

    End Function

    Private Function AddToErrorDataset(ByVal dsErrors As DataSet, ByVal [Error] As String, ByVal Details As String) As Boolean

        Try

            If dsErrors.Tables.Contains("Errors") Then

                Dim row As DataRow = dsErrors.Tables("Errors").NewRow

                row("Error") = [Error]
                row("Details") = Details

                dsErrors.Tables("Errors").Rows.Add(row)

            End If

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Private Function CreateErrorDataset() As DataSet

        Dim ds As New DataSet
        ds.Tables.Add("Errors")

        With ds.Tables("Errors").Columns

            .Add(New DataColumn("Error", GetType(String)))
            .Add(New DataColumn("Details", GetType(String)))

        End With

        Return ds

    End Function

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If txtUserID.Text > 0 AndAlso txtUserType.Text <> "" Then

            If SaveUsercheckedNodes(txtUserID.Text) Then
                RaiseEvent DisplayMessage("Details saved!", MessageTypeEnum.Information)
            Else
                'RaiseEvent DisplayMessage("Details saved!", MessageTypeEnum.Information)
            End If
        Else
            RaiseEvent DisplayMessage("Select User or UserGroup!", MessageTypeEnum.Error)
        End If

    End Sub

    Private Sub lstUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstUsers.SelectedIndexChanged

        txtUserID.Text = lstUsers.SelectedValue
        txtUserType.Text = "User"
        Dim objUserReportAccessRight As New Global.SysPermissionsManager.UserReportAccessRight(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objUserReportAccessRight.GetSelectedReportRights(txtUserID.Text, txtUserType.Text)
        Session("UserReportRights") = ds

        LoadReports()
        pnlCodes.Enabled = True

    End Sub

    Private Sub lstRoles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstRoles.SelectedIndexChanged

        txtUserID.Text = lstRoles.SelectedValue
        txtUserType.Text = "UserGroup"

        Dim objUserReportAccessRight As New Global.SysPermissionsManager.UserReportAccessRight(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds As DataSet = objUserReportAccessRight.GetSelectedReportRights(txtUserID.Text, txtUserType.Text)
        Session("UserReportRights") = ds

        LoadReports()
        pnlCodes.Enabled = True

    End Sub

End Class