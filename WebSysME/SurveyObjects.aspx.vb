Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic
Imports Telerik.Web.UI

Public Class SurveyObjects
    Inherits System.Web.UI.Page

    Private dsDocuments As DataSet
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageTypeEnum)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageTypeEnum)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboSurvey

                .DataSource = objLookup.Lookup("tblSurveys", "SurveyID", "Name").Tables(0)
                .DataValueField = "SurveyID"
                .DataTextField = "Name"
                .DataBind()

            End With

            With cboObjectType

                .DataSource = objLookup.Lookup("luObjectTypes", "ObjectTypeID", "Description").Tables(0)
                .DataValueField = "ObjectTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If Map() Then

            ShowMessage("Survey mapped successfully...", MessageTypeEnum.Information)

        Else

            ShowMessage("Error saving ...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub LoadGrid()

        Dim sql As String = DertemineObjectTypeSQL(cboObjectType.SelectedItem.Text)
        ViewState("tObjects") = Nothing

        If sql <> "" Then

            With radObjects

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()

                ViewState("tObjects") = .DataSource

            End With

        End If

    End Sub

    Public Function DertemineObjectTypeSQL(ByVal Type As String) As String

        Dim sql As String = ""

        Select Case Type

            Case "Staff"
                sql = "SELECT StaffID As ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As StaffName FROM tblStaffMembers"

            Case "Group"
                sql = "SELECT GroupID As ObjectID, GroupName, Description FROM tblGroups"

            Case "Organization"
                sql = "SELECT OrganizationID As ObjectID, Name FROM tblOrganization"

            Case "Project"
                sql = "SELECT Project As ObjectID, Name, Acronym FROM tblProjects"

            Case "School"
                sql = "SELECT SchoolID As ObjectID, Name FROM tblSchools"

            Case "HealthCenter"
                sql = "SELECT HealthCenterID as ObjectID, Name, Description FROM tblHealthCenters"

            Case "Household"
                sql = "SELECT BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries WHERE Suffix = 1"

            Case "Individual"
                sql = "SELECT BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries"

            Case "Activity"
                sql = "SELECT ActivityID as ObjectID, Description FROM tblActivities"

            Case "Document"
                sql = "SELECT FileID as ObjectID, Title, Author, Description FROM tblFiles"

        End Select

        Return sql

    End Function

    'Public Function LoadDocumentObjects(ByVal SurveyObjectID As Long) As Boolean

    '    Try

    '        Dim objDocumentObjects As New DocumentObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    '        With objDocumentObjects

    '            If .Retrieve(SurveyObjectID) Then

    '                txtDocumentObjectID.Text = .DocumentObjectID
    '                'If Not IsNothing(cboDocument.) Then cboDocument.SelectedValue = .DocumentID
    '                If Not IsNothing(cboObjectType.Items.FindByValue(.ObjectTypeID)) Then cboObjectType.SelectedValue = .ObjectTypeID

    '                ShowMessage("DocumentObjects loaded successfully...", MessageTypeEnum.Information)
    '                Return True

    '            Else

    '                ShowMessage("Failed to load DocumentObjects...", MessageTypeEnum.Error)
    '                Return False

    '            End If

    '        End With

    '    Catch ex As Exception

    '        ShowMessage(ex, MessageTypeEnum.Error)
    '        Return False

    '    End Try

    'End Function

    Protected Function GetSelectedObjectIDs() As String

        Dim ObjectIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radObjects.SelectedItems
            ObjectIDArray.Add(gridRow.Item("ObjectID").Text.ToString())
        Next

        Return String.Join(",", ObjectIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim mObject() As String = GetSelectedObjectIDs.Split(",")

        If mObject.Length > 0 Then

            For i As Long = 0 To mObject.Length - 1

                Dim objObjects As New BusinessLogic.SurveyObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objObjects

                    .ObjectTypeID = cboObjectType.SelectedValue
                    .ObjectID = mObject(i)

                    If cboSurvey.Entries.Count > 0 Then

                        .SurveyID = cboSurvey.Entries.Item(0).Value

                    Else

                        Return False

                    End If

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Public Sub Clear()

        txtDocumentObjectID.Text = ""
        If Not IsNothing(cboObjectType.Items.FindByValue("")) Then
            cboObjectType.SelectedValue = ""
        ElseIf Not IsNothing(cboObjectType.Items.FindByValue(0)) Then
            cboObjectType.SelectedValue = 0
        Else
            cboObjectType.SelectedIndex = -1
        End If

    End Sub

    Private Sub radObjects_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radObjects.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            If dsDocuments.Tables(0).Select("ObjectID = " & gridItem("ObjectID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already mapped..."

            End If

        End If

    End Sub

    Private Sub radObjects_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radObjects.NeedDataSource

        radObjects.DataSource = DirectCast(ViewState("tObjects"), DataTable)

    End Sub

    Private Sub cboObjectType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboObjectType.SelectedIndexChanged

        If cboObjectType.SelectedIndex > 0 Then

            Dim sql As String = "SELECT ObjectID FROM tblSurveyObjects O inner join luObjectTypes OT "
            sql &= " on OT.ObjectTypeID = O.ObjectTypeID where O.SurveyID = " & cboSurvey.Entries.Item(0).Value & " AND OT.ObjectType = " & cboObjectType.SelectedValue

            dsDocuments = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        LoadGrid()

    End Sub

End Class