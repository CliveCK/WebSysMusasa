Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic
Imports Telerik.Web.UI

Public Class InterventionObjectsDetailsControl
    Inherits System.Web.UI.UserControl

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboInterventions

                .DataSource = objLookup.Lookup("tblInterventions", "InterventionID", "Name").Tables(0)
                .DataValueField = "InterventionID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

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

    Private Sub LoadGrid()

        Dim sql As String = DertemineObjectTypeSQL(cboObjectType.SelectedItem.Text)
        ViewState("tObjects") = Nothing

        If sql <> "" AndAlso sql <> "Invalid" Then

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

            Case "Intervention"
                sql = "SELECT InterventionID as ObjectID, Description, Name, Description, StartDate, EndDate  FROM tblInterventions"

            Case "Outcome"
                sql = "SELECT OutcomeID as ObjectID, Description FROM tblOutcomes"

            Case Else
                sql = "Invalid"

        End Select

        Return sql

    End Function

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

                Dim objObjects As New BusinessLogic.InterventionObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objObjects

                    .ObjectTypeID = cboObjectType.SelectedValue
                    .ObjectID = mObject(i)

                    If cboInterventions.SelectedIndex > 0 Then

                        .InterventionID = cboInterventions.SelectedValue

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

    Private Sub radObjects_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radObjects.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            If dsDocuments.Tables(0).Select("ObjectID = " & gridItem("ObjectID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already mapped..."

            End If

        End If

    End Sub

    Private Sub cboObjectType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboObjectType.SelectedIndexChanged

        If cboInterventions.SelectedIndex > 0 Then

            If cboObjectType.SelectedIndex > 0 Then

                Dim sql As String = "SELECT ObjectID FROM tblInterventions I inner join tblInterventionObjects O on I.InterventionID = O.InterventionID inner join luObjectTypes OT "
                sql &= " on OT.ObjectTypeID = O.ObjectTypeID where O.InterventionID = " & cboInterventions.SelectedValue & " AND OT.Description = '" & cboObjectType.SelectedValue & "'"

                dsDocuments = db.ExecuteDataSet(CommandType.Text, sql)

            End If

            LoadGrid()

        Else

            ShowMessage("Please select intervention first...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            ShowMessage("Intervention mapped successfully...", MessageTypeEnum.Information)

        Else

            ShowMessage("Error saving ...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radObjects_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radObjects.NeedDataSource

        radObjects.DataSource = DirectCast(ViewState("tObjects"), DataTable)

    End Sub

End Class