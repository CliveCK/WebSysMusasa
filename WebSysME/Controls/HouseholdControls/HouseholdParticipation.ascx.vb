Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI

Public Class HouseholdParticipation
    Inherits System.Web.UI.UserControl

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private dsGroups As DataSet
    Private dsInterventions As DataSet

    Private ReadOnly Property HouseholdID As Long
        Get
            Dim txtHouseholdID As TextBox = DirectCast(Parent.Parent.FindControl("ucBeneficiaryControl").FindControl("txtBeneficiaryID1"), TextBox)

            Return IIf(IsNumeric(txtHouseholdID.Text), txtHouseholdID.Text, 0)
        End Get
    End Property

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

            If CookiesWrapper.BeneficiaryID > 0 Then
                LoadGrid()
            End If

        End If

        dsGroups = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblGroups G inner join tblHouseHoldGroups HG on G.GroupID = HG.GroupID WHERE HouseholdID = " & CookiesWrapper.BeneficiaryID)
        dsInterventions = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblInterventions I inner join tblBeneficiaryIntervention BI on I.InterventionID = BI.InterventionID WHERE BeneficiaryID = " & CookiesWrapper.BeneficiaryID)

    End Sub

    Public Sub LoadGrid()

        dsGroups = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblGroups G inner join tblHouseHoldGroups HG on G.GroupID = HG.GroupID WHERE HouseholdID = " & CookiesWrapper.BeneficiaryID)
        dsInterventions = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblInterventions I inner join tblBeneficiaryIntervention BI on I.InterventionID = BI.InterventionID WHERE BeneficiaryID = " & CookiesWrapper.BeneficiaryID)

        With radInterventions

            .DataSource = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblInterventions")
            .DataBind()

            ViewState("Interventions") = .DataSource

        End With

        With radGroups

            .DataSource = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblGroups")
            .DataBind()

            ViewState("Groups") = .DataSource

        End With

    End Sub

    Private Sub radGroups_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles radGroups.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            If dsGroups.Tables(0).Select("GroupID = " & gridItem("GroupID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Checked = True

            End If

        End If

    End Sub

    Private Sub radInterventions_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radInterventions.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            If dsInterventions.Tables(0).Select("InterventionID = " & gridItem("InterventionID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Checked = True

            End If

        End If

    End Sub

    Protected Function GetSelectedGroupIDs() As String

        Dim GroupIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radGroups.SelectedItems
            GroupIDArray.Add(gridRow.Item("GroupID").Text.ToString())
        Next

        Return String.Join(",", GroupIDArray.ToArray())

    End Function

    Protected Function GetSelectedInterventionIDs() As String

        Dim InterventionIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radGroups.SelectedItems
            InterventionIDArray.Add(gridRow.Item("InterventionID").Text.ToString())
        Next

        Return String.Join(",", InterventionIDArray.ToArray())

    End Function

    Private Function MapGroups() As Boolean

        Dim Group() As String = GetSelectedGroupIDs.Split(",")

        If Group.Length > 0 Then

            For i As Long = 0 To Group.Length - 1

                If dsGroups.Tables(0).Select("GroupID = " & Group(i)).Length > 0 Then

                    Return True

                ElseIf Not db.ExecuteNonQuery(CommandType.Text, "INSERT INTO tblHouseHoldGroups (HouseholdID, GroupID) VALUES (" & CookiesWrapper.BeneficiaryID & "," & Group(i) & ")") > 0 Then

                    Return False

                End If

            Next

        End If

        Return True

    End Function

    Private Function MapInterventions() As Boolean

        Dim Intervention() As String = GetSelectedInterventionIDs.Split(",")

        If Intervention.Length > 0 Then

            For i As Long = 0 To Intervention.Length - 1

                If dsInterventions.Tables(0).Select("InterventionID = " & Intervention(i)).Length > 0 Then

                    Continue For

                ElseIf Not db.ExecuteNonQuery(CommandType.Text, "INSERT INTO tblBeneficiaryInterventions (BeneficiaryID, InterventionID) VALUES (" & CookiesWrapper.BeneficiaryID & "," & Intervention(i) & ")") > 0 Then

                    Return False

                End If

            Next

        End If

        Return True

    End Function

    Private Sub cmdMapIntervention_Click(sender As Object, e As EventArgs) Handles cmdMapIntervention.Click

        If CookiesWrapper.BeneficiaryID > 0 Then

            If MapInterventions() Then

                ShowMessage("Mapped to interventions successfully", MessageTypeEnum.Information)

            End If

        Else

            ShowMessage("Please save Individual/Household details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub cmdSaveGroups_Click(sender As Object, e As EventArgs) Handles cmdSaveGroups.Click

        If CookiesWrapper.BeneficiaryID > 0 Then

            If MapGroups() Then

                ShowMessage("Mapped to groups successfully...", MessageTypeEnum.Information)

            End If

        Else

            ShowMessage("Please save Individual/Household details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radGroups_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radGroups.NeedDataSource

        radGroups.DataSource = DirectCast(ViewState("Groups"), DataTable)

    End Sub

    Private Sub radInterventions_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radInterventions.NeedDataSource

        radInterventions.DataSource = DirectCast(ViewState("Interventions"), DataTable)

    End Sub
End Class