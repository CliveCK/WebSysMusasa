Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class ParticipantsControl
    Inherits System.Web.UI.UserControl

    Private dsParticipants As DataSet
    Private ReturnURL As String
    Private EventID As Long
    Private EventTypeID As Long
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboObjectType

                .DataSource = objLookup.Lookup("luObjectTypes", "ObjectTypeID", "Description", , "Description IN ('Staff','HealthCenter', 'Orgnaization', 'Household', 'Individual', 'School', 'Group')").Tables(0)
                .DataValueField = "ObjectTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("ev")) AndAlso IsNumeric(objUrlEncoder.Decrypt(Request.QueryString("ev"))) _
                        AndAlso Not IsNothing(Request.QueryString("par")) AndAlso Not IsNothing(Request.QueryString("evid")) Then

                EventID = objUrlEncoder.Decrypt(Request.QueryString("ev"))
                EventTypeID = objUrlEncoder.Decrypt(Request.QueryString("evid"))

            End If

        End If

    End Sub

    Private Sub LoadGrid()

        Dim sql As String = DertemineObjectTypeSQL(cboObjectType.SelectedItem.Text)
        ViewState("Objects") = Nothing

        Dim sql1 As String = "select P.ObjectID from tblParticipants P inner join luObjectTypes O on P.ObjectTypeID = O.ObjectTypeID "
        sql1 &= "inner join luEventTypes E on E.EventTypeID = P.EventTypeID where P.EventID = " & objUrlEncoder.Decrypt(Request.QueryString("ev"))
        sql1 &= " AND P.ObjectTypeID = " & IIf(cboObjectType.SelectedIndex > 0, cboObjectType.SelectedValue, 0) & " AND P.EventTypeID = " & _
        objUrlEncoder.Decrypt(Request.QueryString("evid"))

        dsParticipants = db.ExecuteDataSet(CommandType.Text, sql1)

        If sql <> "" Then

            With radObjects

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()

                ViewState("Objects") = .DataSource

            End With

        End If

    End Sub

    Private Function Map() As Boolean

        Dim mObject() As String = GetSelectedObjectIDs.Split(",")

        If mObject.Length > 0 Then

            For i As Long = 0 To mObject.Length - 1

                Dim objObjects As New BusinessLogic.Participants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objObjects

                    .ObjectTypeID = cboObjectType.SelectedValue
                    .ObjectID = mObject(i)
                    .EventID = objUrlEncoder.Decrypt(Request.QueryString("ev"))
                    .EventTypeID = objUrlEncoder.Decrypt(Request.QueryString("evid"))

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Protected Function GetSelectedObjectIDs() As String

        Dim ObjectIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radObjects.SelectedItems
            ObjectIDArray.Add(gridRow.Item("ObjectID").Text.ToString())
        Next

        Return String.Join(",", ObjectIDArray.ToArray())

    End Function

    Private Sub radObjects_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radObjects.NeedDataSource

        Dim sql As String = "select P.ObjectID from tblParticipants P inner join luObjectTypes O on P.ObjectTypeID = O.ObjectTypeID "
        sql &= "inner join luEventTypes E on E.EventTypeID = P.EventTypeID where P.EventID = " & EventID & " AND P.ObjectTypeID = " & IIf(cboObjectType.SelectedIndex > 0, cboObjectType.SelectedValue, 0)

        dsParticipants = db.ExecuteDataSet(CommandType.Text, sql)

        radObjects.DataSource = DirectCast(ViewState("Objects"), DataTable)

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

            Case "School"
                sql = "SELECT SchoolID As ObjectID, Name FROM tblSchools"

            Case "HealthCenter"
                sql = "SELECT HealthCenterID as ObjectID, Name, Description FROM tblHealthCenters"

            Case "Household"
                sql = "SELECT BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries WHERE Suffix = 1"

            Case "Individual"
                sql = "SELECT BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries"


        End Select

        Return sql

    End Function

    Private Sub cboObjectType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboObjectType.SelectedIndexChanged

        LoadGrid()

    End Sub

    Private Sub radObjects_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radObjects.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            If dsParticipants.Tables(0).Select("ObjectID = " & gridItem("ObjectID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already mapped..."

            End If

        End If

    End Sub

    Private Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click

        Response.Redirect(objUrlEncoder.Decrypt(Request.QueryString("par")) & "?id=" & Request.QueryString("ev"))

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Process completed successfully", MessageTypeEnum.Information)

        Else

            ShowMessage("Error while saving...", MessageTypeEnum.Information)

        End If

    End Sub
End Class