Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class CustomFieldTemplateActivation
    Inherits System.Web.UI.UserControl

    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database
    Private DataLookup As New BusinessLogic.CommonFunctions

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        RaiseEvent Message(Message.ToString, MessageType)

    End Sub

#End Region

    Public Property TemplateID As Long
        Get
            If ViewState("TemplateID") Is Nothing Then
                ViewState("TemplateID") = 0
            End If
            Return ViewState("TemplateID")
        End Get
        Set(value As Long)
            ViewState("TemplateID") = value
        End Set
    End Property

    Public Property TemplateName As String
        Get
            If ViewState("TemplateName") Is Nothing Then
                ViewState("TemplateName") = String.Empty
            End If
            Return ViewState("TemplateName")
        End Get
        Set(value As String)
            ViewState("TemplateName") = value
        End Set
    End Property

    Private Sub InitialisePageobjects()

        db = New DatabaseProviderFactory().Create("ConnectionString")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        InitialisePageobjects()

        If Not Page.IsPostBack Then

            ucAppliesTo.AvailableOptionsCaption = "Template Activated for:"
            ucAppliesTo.SelectedOptionsCaption = "Template Locked for:"

        End If

    End Sub

    Protected Sub cboApplyTo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboApplyTo.SelectedIndexChanged

        If cboApplyTo.SelectedValue <> "" AndAlso TemplateName <> "" Then

            Select Case cboApplyTo.SelectedValue

                Case "PS" 'Project Status

                    LoadProjectStatus()

                Case "CS" 'Client Status

                    LoadClientStatus()

            End Select

        Else

            ucAppliesTo.AvailableOptions.Items.Clear()
            ucAppliesTo.SelectedOptions.Items.Clear()

        End If

    End Sub

    Private Sub LoadProjectStatus()

        With ucAppliesTo

            With .AvailableOptions
                .DataValueField = "ID"
                .DataTextField = "Name"
                .DataSource = DataLookup.Lookup("luProjectStatuses PS LEFT OUTER JOIN luProjectTypes PT ON PS.Project_Type_ID = PT.OwnID", "PS.Project_Status_ID AS [ID]", "PS.Status_Name + ISNULL(' [' + PT.Description + ']','') AS [Name]", "Status_Name", "Project_Status_ID NOT IN (SELECT ActivatorID FROM dbo.CustomField_Template_Activation WHERE ObjectType='P' AND TemplateID = " & TemplateID & ")")
                .DataBind()
            End With

            With .SelectedOptions
                .DataValueField = "ID"
                .DataTextField = "Name"
                .DataSource = DataLookup.Lookup("luProjectStatuses PS LEFT OUTER JOIN luProjectTypes PT ON PS.Project_Type_ID = PT.OwnID", "Project_Status_ID AS [ID]", "PS.Status_Name + ISNULL(' [' + PT.Description + ']','') AS [Name]", "Status_Name", "Project_Status_ID IN (SELECT ActivatorID FROM dbo.CustomField_Template_Activation WHERE ObjectType='P' AND TemplateID = " & TemplateID & ")")
                .DataBind()
            End With

        End With

    End Sub

    Private Sub LoadClientStatus()

        With ucAppliesTo

            With .AvailableOptions
                .DataValueField = "ID"
                .DataTextField = "Name"
                .DataSource = DataLookup.Lookup("luContactStatuses", "StatusID AS [ID]", "luContactStatuses AS [Name]", "luContactStatuses", "StatusID NOT IN (SELECT ActivatorID FROM dbo.CustomField_Template_Activation WHERE ObjectType='C' AND TemplateID = " & TemplateID & ")")
                .DataBind()
            End With

            With .SelectedOptions
                .DataValueField = "ID"
                .DataTextField = "Name"
                .DataSource = DataLookup.Lookup("luContactStatuses", "StatusID AS [ID]", "luContactStatuses AS [Name]", "luContactStatuses", "StatusID IN (SELECT ActivatorID FROM dbo.CustomField_Template_Activation WHERE ObjectType='C' AND TemplateID = " & TemplateID & ")")
                .DataBind()
            End With

        End With

    End Sub

    Public Sub SaveActivationDetails()

        If cboApplyTo.SelectedValue <> "" AndAlso TemplateName <> "" Then

            Dim s As String = Join(ucAppliesTo.SelectedValues, ",").TrimStart(",").TrimEnd(",").Replace(",,", ",")
            Dim a As String = Join(ucAppliesTo.AvailableValues, ",").TrimStart(",").TrimEnd(",").Replace(",,", ",")

            Dim sql As String = ""
            sql &= "DELETE FROM CustomField_Template_Activation WHERE ActivatorID IN (0" & a & ") AND ObjectType = '" & cboApplyTo.SelectedValue & "' AND TemplateID = " & TemplateID & " " & vbCrLf
            sql &= " " & vbCrLf
            sql &= "INSERT INTO CustomField_Template_Activation (ActivatorID, ObjectType, TemplateID) " & vbCrLf
            sql &= "SELECT ID, '" & cboApplyTo.SelectedValue & "', " & TemplateID & " FROM (SELECT {0} AS ID FROM {1}) Data WHERE ID IN (0" & s & ") AND ID NOT IN (SELECT ActivatorID FROM CustomField_Template_Activation WHERE ObjectType = '" & cboApplyTo.SelectedValue & "' AND TemplateID = " & TemplateID & ") " & vbCrLf

            Select Case cboApplyTo.SelectedValue

                Case "PS" 'Project Status

                    sql = String.Format(sql, "StatusID", "luProjectStatuses")
                    LoadProjectStatus()

                Case "CS" 'Client Status

                    sql = String.Format(sql, "StatusID", "luContactStatuses")

            End Select

            If BusinessLogic.CustomFields.DataHelper.ExecuteNonQuery(db, sql) Then

                RaiseEvent Message(cboApplyTo.SelectedItem.Text & " settings updated successfully.", MessageTypeEnum.Information)

            Else

                RaiseEvent Message(BusinessLogic.CustomFields.DataHelper.ErrorMessage, MessageTypeEnum.Error)

            End If

        End If

    End Sub

End Class