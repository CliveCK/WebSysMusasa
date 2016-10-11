Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class CustomFieldTemplateDetails
    Inherits System.Web.UI.UserControl

    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database
    Public Property TemplateID As Long

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        TemplateID = Request.QueryString("id")
        db = New DatabaseProviderFactory().Create("ConnectionString")

        If Not Page.IsPostBack Then

            LoadCFTemplate()
            LoadCFSecurity(BusinessLogic.CustomFields.CustomFieldSecurityType.Read)
            Dim s As New StringCollection

        End If

    End Sub

    Private Sub LoadCFTemplate()

        Dim objCFTemplate As New BusinessLogic.CustomFields.CustomFieldTemplate("ConnectionString", CookiesWrapper.thisUserID)

        If objCFTemplate.Retrieve(TemplateID) Then

            lblTemplateName.Text = objCFTemplate.TemplateName
            txtComment.Content = objCFTemplate.Comments

            ucCustomFieldTemplateActivation.TemplateID = TemplateID
            ucCustomFieldTemplateActivation.TemplateName = objCFTemplate.TemplateName

        End If

    End Sub

    Private Sub SaveCFTemplate()

        Dim objCFTemplate As New BusinessLogic.CustomFields.CustomFieldTemplate("ConnectionString", CookiesWrapper.thisUserID)

        objCFTemplate.TemplateID = TemplateID
        objCFTemplate.TemplateName = lblTemplateName.Text
        objCFTemplate.Comments = txtComment.Content

        If objCFTemplate.Save Then

        End If

    End Sub

    Private Sub cboSecurityType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboSecurityType.SelectedIndexChanged

        LoadCFTemplateSecurity(cboSecurityType.SelectedValue)

    End Sub

    Private Sub LoadCFTemplateSecurity(ByVal SecurityType As String)

        Select Case SecurityType.ToLower

            Case "read"

                LoadCFSecurity(BusinessLogic.CustomFields.CustomFieldSecurityType.Read)

            Case "write"

                LoadCFSecurity(BusinessLogic.CustomFields.CustomFieldSecurityType.Write)

        End Select

    End Sub

    Private Sub LoadCFSecurity(SecurityType As BusinessLogic.CustomFields.CustomFieldSecurityType)

        Dim objSec As New BusinessLogic.CustomFields.CustomFieldTemplateSecurity("ConnectionString", CookiesWrapper.thisUserID)

        With (ucUserSecurity)

            With .AvailableOptions

                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = objSec.GetDeniedUsers(TemplateID, SecurityType)
                .DataBind()

            End With

            With .SelectedOptions

                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = objSec.GetPermittedUsers(TemplateID, SecurityType)
                .DataBind()

            End With

        End With

        With (ucGroupSecurity)

            With .AvailableOptions

                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = objSec.GetDeniedGroups(TemplateID, SecurityType)
                .DataBind()

            End With

            With .SelectedOptions

                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = objSec.GetPermittedGroups(TemplateID, SecurityType)
                .DataBind()

            End With

        End With

        lblError.Text = "Successfully loaded " & [Enum].GetName(GetType(BusinessLogic.CustomFields.CustomFieldSecurityType), SecurityType) & " security details..."

    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        SaveCFTemplate()

        Dim objSec As New BusinessLogic.CustomFields.CustomFieldTemplateSecurity("ConnectionString", CookiesWrapper.thisUserID)

        Dim SecrityType As BusinessLogic.CustomFields.CustomFieldSecurityType = IIf(cboSecurityType.SelectedValue.ToLower = "read", BusinessLogic.CustomFields.CustomFieldSecurityType.Read, BusinessLogic.CustomFields.CustomFieldSecurityType.Write)

        objSec.RevokeBulkUserSecurity(TemplateID, Join(ucUserSecurity.AvailableValues, ","), SecrityType)
        objSec.RevokeBulkGroupSecurity(TemplateID, Join(ucGroupSecurity.AvailableValues, ","), SecrityType)

        objSec.AssignBulkUserSecurity(TemplateID, Join(ucUserSecurity.SelectedValues, ","), SecrityType)
        objSec.AssignBulkGroupSecurity(TemplateID, Join(ucGroupSecurity.SelectedValues, ","), SecrityType)

        ucCustomFieldTemplateActivation.SaveActivationDetails()

        lblError.Text = "Security details saved..."

    End Sub

    Private Sub ucCustomFieldTemplateActivation_Message(Message As String, MessageType As MessageTypeEnum) Handles ucCustomFieldTemplateActivation.Message

        lblError.Text = Message

    End Sub

End Class