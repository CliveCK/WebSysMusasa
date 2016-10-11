Imports Telerik.Web.UI
Imports Universal.CommonFunctions
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class CustomFieldGridEditor
    Inherits System.Web.UI.Page

    Dim CustomFieldTemplateID As Long = 3
    Dim ProjectID As Long = 163, ObjectType As String = "P", UserID As Long = CookiesWrapper.thisUserID

    Private db As Microsoft.Practices.EnterpriseLibrary.Data.Database
    Private objCF As BusinessLogic.CustomFields.CustomFieldsManager

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub CustomFieldGridEditor_Init(sender As Object, e As EventArgs) Handles Me.Init

        If IsNumeric(Request.QueryString("cftid")) Then
            CustomFieldTemplateID = Request.QueryString("cftid")
        End If

        db = New DatabaseProviderFactory().Create("ConnectionString")
        objCF = New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

        'PlaceHolder1.Controls.Add(LoadGridData(CustomFieldTemplateID))
        PlaceHolder1.Controls.Add(objCF.LoadCustomFieldGrid(ObjectID:=0, ObjectType:="D", CFTemplateID:=4, CFTemplateName:="Vehicle Fuel Consumption", DisplayDateFormat:=My.Settings.DisplayDateFormat))

    End Sub

    Private Sub CustomFieldGridEditor_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFContactDetailsRequesting", BusinessLogic.CustomFields.CustomFieldsManager.GetContactsLookupScript, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFProjectDetailsRequesting", BusinessLogic.CustomFields.CustomFieldsManager.GetProjectsLookupScript, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFRequestingOnDemandData", BusinessLogic.CustomFields.CustomFieldsManager.OnCFRequestingOnDemandData, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFLocationRequesting", BusinessLogic.CustomFields.CustomFieldsManager.GetLocationIndexLookupScript, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFEntityRequesting", BusinessLogic.CustomFields.CustomFieldsManager.GetEntityLookupScript, True)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnCFContactDetailsRequesting", BusinessLogic.CustomFields.CustomFieldsManager.GetContactsLookupScript, True)

    End Sub

End Class