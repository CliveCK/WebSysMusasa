Imports BusinessLogic
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI
Public Class LawyerClientSessionDetails
    Inherits System.Web.UI.Page

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private ds As DataSet

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
            With cboReferredTo

                .DataSource = objLookup.Lookup("luReferralCentreTypes", "ReferralCentreTypeID", "Description").Tables(0)
                .DataValueField = "ReferralCentreTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboMarriageType

                .DataSource = objLookup.Lookup("luMaritalStatus", "ObjectID", "Description").Tables(0)
                .DataValueField = "ObjectID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            LoadGrid()

            If Not IsNothing(Request.QueryString("id")) Then

                LoadLawyerClientSessionDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub


    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadLawyerClientSessionDetails(ByVal BeneficiaryID As Long) As Boolean

        Try

            Dim objLawyerClientSessionDetails As New BusinessLogic.LawyerClientSessionDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                If .Retrieve(BeneficiaryID) Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    If Not IsNothing(cboMarriageType.Items.FindByValue(.MaritalStatus)) Then cboMarriageType.SelectedValue = .MaritalStatus

                End If

            End With

            With objLawyerClientSessionDetails

                If .Retrieve(BeneficiaryID) Then

                    txtLawyerClientSessionDetailID.Text = .LawyerClientSessionDetailID
                    If Not IsNothing(cboActionToBeTaken.Items.FindByValue(.ActionTobeTakenID)) Then cboActionToBeTaken.SelectedValue = .ActionTobeTakenID
                    If Not IsNothing(cboReferredTo.Items.FindByValue(.ReferralID)) Then cboReferredTo.SelectedValue = .ReferralID
                    If Not .SessionDate = "" Then radSessionDate.SelectedDate = .SessionDate
                    txtCaseNotes.Text = .CaseNotes
                    txtOtherReferDetails.Text = .ReferralOther
                    txtOtherProblem.Text = .OtherProblem

                    LoadGrid()

                    ShowMessage("LawyerClientSessionDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    'ShowMessage("Failed to loadLawyerClientSessionDetails", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function Save() As Boolean

        Try

            Dim objLawyerClientSessionDetails As New BusinessLogic.LawyerClientSessionDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim update As Boolean = False

            'First save the beneficiary details - We will need the BeneficiaryID for the Client details

            With objBeneficiary

                .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                If cboMarriageType.SelectedIndex > 0 Then .MaritalStatus = cboMarriageType.SelectedValue
                .Suffix = 1

                update = .BeneficiaryID > 0

                If .Save Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    If Not update = True Then .MemberNo = .GenerateMemberNo
                    .Save()

                Else

                    ShowMessage("Failed to save beneficiary details...Process aborted!!", MessageTypeEnum.Error)
                    Exit Function

                End If

            End With

            With objLawyerClientSessionDetails

                .LawyerClientSessionDetailID = IIf(IsNumeric(txtLawyerClientSessionDetailID.Text), txtLawyerClientSessionDetailID.Text, 0)
                If IsNumeric(txtBeneficiaryID.Text) Then
                    .BeneficiaryID = txtBeneficiaryID.Text
                Else
                    ShowMessage("Missing beneficiary Information", MessageTypeEnum.Error)
                    Exit Function
                End If
                If cboActionToBeTaken.SelectedIndex > -1 Then .ActionTobeTakenID = cboActionToBeTaken.SelectedValue
                If cboReferredTo.SelectedIndex > 0 Then .ReferralID = cboReferredTo.SelectedValue
                If radSessionDate.SelectedDate.HasValue Then .SessionDate = radSessionDate.SelectedDate
                .CaseNotes = txtCaseNotes.Text
                .ReferralOther = txtOtherReferDetails.Text
                .OtherProblem = txtOtherProblem.Text

                If .Save Then

                    If Not IsNumeric(txtLawyerClientSessionDetailID.Text) OrElse Trim(txtLawyerClientSessionDetailID.Text) = 0 Then txtLawyerClientSessionDetailID.Text = .LawyerClientSessionDetailID
                    Map()
                    ShowMessage("LawyerClientSessionDetails saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtLawyerClientSessionDetailID.Text = ""
        If Not IsNothing(cboReferredTo.Items.FindByValue("")) Then
            cboReferredTo.SelectedValue = ""
        ElseIf Not IsNothing(cboReferredTo.Items.FindByValue(0)) Then
            cboReferredTo.SelectedValue = 0
        Else
            cboReferredTo.SelectedIndex = -1
        End If
        If Not IsNothing(cboActionToBeTaken.Items.FindByValue("")) Then
            cboActionToBeTaken.SelectedValue = ""
        ElseIf Not IsNothing(cboActionToBeTaken.Items.FindByValue(0)) Then
            cboActionToBeTaken.SelectedValue = 0
        Else
            cboActionToBeTaken.SelectedIndex = -1
        End If
        radSessionDate.Clear()
        txtCaseNotes.Text = ""
        txtOtherReferDetails.Text = ""
        txtOtherProblem.Text = ""

    End Sub

    Private Sub radProblems_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radProblems.NeedDataSource

        Dim sql As String = "Select * from tblLaywerClientSessionDetails L inner join tblLawyerSessionProblems P on L.LawyerClientSessionDetailID = P.LawyerClientSessionDetailID "
        sql &= "where P.LawyerClientSessionDetailID = " & IIf(IsNumeric(txtLawyerClientSessionDetailID.Text), txtLawyerClientSessionDetailID.Text, 0)

        ds = db.ExecuteDataSet(CommandType.Text, sql)

        radProblems.DataSource = DirectCast(ViewState("mProblems"), DataSet)

    End Sub

    Protected Function GetProblemIDs() As String

        Dim NatureOfProblemIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radProblems.SelectedItems
            NatureOfProblemIDArray.Add(gridRow.Item("NatureOfProblemID").Text.ToString())
        Next

        Return String.Join(",", NatureOfProblemIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Problems() As String = GetProblemIDs.Split(",")

        If Problems.Length > 0 Then

            For i As Long = 0 To Problems.Length - 1

                Dim objSessionProblems As New BusinessLogic.LawyerSessionProblems(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objSessionProblems

                    If IsNumeric(Problems(i)) Then

                        .LawyerClientSessionDetailID = txtLawyerClientSessionDetailID.Text
                        .ProblemID = Problems(i)

                        If Not .Save Then

                            log.Error("Error saving...")
                            Return False

                        End If

                        LoadGrid()

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Public Sub LoadGrid()

        Dim objLookup As New BusinessLogic.CommonFunctions

        Dim sql As String = "Select * from tblLaywerClientSessionDetails L inner join tblLawyerSessionProblems P on L.LawyerClientSessionDetailID = P.LawyerClientSessionDetailID "
        sql &= "where P.LawyerClientSessionDetailID = " & IIf(IsNumeric(txtLawyerClientSessionDetailID.Text), txtLawyerClientSessionDetailID.Text, 0)

        ds = db.ExecuteDataSet(CommandType.Text, sql)

        With radProblems

            .DataSource = objLookup.SqlExec("Select * from luNatureOfProblems")
            .DataBind()

            ViewState("mProblems") = .DataSource
        End With

    End Sub

    Private Sub radProblems_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radProblems.ItemDataBound

        If IsNumeric(txtLawyerClientSessionDetailID.Text) AndAlso txtLawyerClientSessionDetailID.Text > 0 Then

            If TypeOf e.Item Is GridDataItem Then

                Dim gridItem As GridDataItem = e.Item

                Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

                If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    If ds.Tables(0).Select("ProblemID = " & gridItem("NatureOfProblemID").Text).Length > 0 Then

                        Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                        chkbx.Enabled = False
                        chkbx.ToolTip = "Already mapped..."
                        btnImage.Visible = True

                    End If

                End If

            End If

            End If

    End Sub
End Class