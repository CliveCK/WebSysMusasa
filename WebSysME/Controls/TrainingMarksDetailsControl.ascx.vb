Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Telerik.Web.UI
Imports BusinessLogic

Public Class TrainingMarksDetailsControl
    Inherits System.Web.UI.UserControl

    Dim ds As DataSet
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

    Private ReadOnly Property TrainingID As Long
        Get
            Return objUrlEncoder.Decrypt(Request.QueryString("id"))
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

            If Not IsNothing(Request.QueryString("id")) Then

                Dim objLookup As New BusinessLogic.CommonFunctions

                With cboTraining

                    .DataSource = objLookup.Lookup("tblTrainings", "TrainingID", "Name", , "TrainingID = " & objUrlEncoder.Decrypt(Request.QueryString("id"))).Tables(0)
                    .DataValueField = "TrainingID"
                    .DataTextField = "Name"
                    .DataBind()

                End With

                With cboPeriod

                    .DataSource = objLookup.Lookup("luPeriod", "PeriodID", "Description").Tables(0)
                    .DataValueField = "PeriodID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With cboBlock

                    .DataSource = objLookup.Lookup("luBlock", "BlockID", "Description").Tables(0)
                    .DataValueField = "BlockID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With cboPaper

                    .DataSource = objLookup.Lookup("luPaper", "PaperID", "Description").Tables(0)
                    .DataValueField = "PaperID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With cboBeneficiaryType

                    .DataSource = objLookup.Lookup("luBeneficiaryType", "BeneficiaryTypeID", "Description").Tables(0)
                    .DataValueField = "BeneficiaryTypeID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With cboHealthGroupType

                    .DataSource = objLookup.Lookup("luHealthGroupTypes", "HealthGroupTypeID", "Description").Tables(0)
                    .DataValueField = "HealthGroupTypeID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

                With cboProvince

                    .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name").Tables(0)
                    .DataValueField = "ProvinceID"
                    .DataTextField = "Name"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, 0))
                    .SelectedIndex = 0

                End With

            End If

        End If

    End Sub

    Protected Function GetSelectedAttendantIDs() As String

        Dim BeneficiaryIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radAttendants.SelectedItems
            BeneficiaryIDArray.Add(gridRow.Item("AttendantID").Text.ToString())
        Next

        Return String.Join(",", BeneficiaryIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim Beneficiary() As String = GetSelectedAttendantIDs.Split(",")

        If Beneficiary.Length > 0 Then

            For i As Long = 0 To Beneficiary.Length - 1

                Dim objAttendants As New BusinessLogic.TrainingMarks(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objAttendants

                    .BeneficiaryTypeID = cboBeneficiaryType.SelectedValue
                    .TrainingID = cboTraining.SelectedValue
                    .BeneficiaryID = Beneficiary(i)
                    .PeriodID = cboPeriod.SelectedValue
                    .BlockID = cboBlock.SelectedValue
                    .PaperID = cboPaper.SelectedValue

                    If Not .Save Then

                        log.Error("Error saving...")
                        ShowMessage("An error occured during processing....", MessageTypeEnum.Error)
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    Public Function DertemineBeneficiaryTypeSQL(ByVal Type As String, ByVal Criteria As String) As String

        Dim sql As String = ""

        Select Case Type

            Case "Community"
                sql = "SELECT ISNULL(M.TrainingMarkID, 0) As TrainingMarkID,CommunityID As ObjectID, C.Name, C.Description, M.Mark, M.Comments FROM tblCommunities C inner join tblTrainingAttendants TA on TA.BeneficiaryID = C.CommunityID "
                sql &= " left outer join tblTrainingMarks M on M.BeneficiaryID = TA.BeneficiaryID "
                sql &= "left outer join luBeneficiaryType B on B.BeneficiaryTypeID = TA.BeneficiaryTypeID WHERE TA.TrainingID = " & cboTraining.SelectedValue
                sql &= " AND ISNULL(M.PeriodID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPeriod.SelectedValue & " ELSE ISNULL(M.PeriodID, 0) END"
                sql &= " AND ISNULL(M.BlockID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboBlock.SelectedValue & " ELSE ISNULL(M.BlockID, 0) END"
                sql &= " AND ISNULL(M.PaperID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPaper.SelectedValue & " ELSE ISNULL(M.PaperID, 0) END"
            Case "Group"
                sql = "SELECT ISNULL(M.TrainingMarkID, 0) As TrainingMarkID, TA.BeneficiaryID As ObjectID, GroupName As Name, G.Description, M.Mark, M.Comments FROM tblGroups G inner join tblTrainingAttendants TA on TA.BeneficiaryID = G.GroupID "
                sql &= " left outer join tblTrainingMarks M on M.BeneficiaryID = TA.BeneficiaryID "
                sql &= "left outer join luBeneficiaryType B on B.BeneficiaryTypeID = TA.BeneficiaryTypeID WHERE TA.TrainingID = " & cboTraining.SelectedValue
                sql &= " AND ISNULL(M.PeriodID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPeriod.SelectedValue & " ELSE ISNULL(M.PeriodID, 0) END"
                sql &= " AND ISNULL(M.BlockID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboBlock.SelectedValue & " ELSE ISNULL(M.BlockID, 0) END"
                sql &= " AND ISNULL(M.PaperID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPaper.SelectedValue & " ELSE ISNULL(M.PaperID, 0) END"

            Case "Organization"
                sql = "SELECT ISNULL(M.TrainingMarkID, 0) As TrainingMarkID, TA.BeneficiaryID As ObjectID, O.Name, '' As Description, M.Mark, M.Comments FROM tblOrganization O inner join tblTrainingAttendants TA on TA.BeneficiaryID = O.OrganizationID "
                sql &= " left outer join tblTrainingMarks M on M.BeneficiaryID = TA.BeneficiaryID  "
                sql &= "inner join luBeneficiaryType B on B.BeneficiaryTypeID = TA.BeneficiaryTypeID WHERE TA.TrainingID = " & cboTraining.SelectedValue
                sql &= " AND ISNULL(M.PeriodID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPeriod.SelectedValue & " ELSE ISNULL(M.PeriodID, 0) END"
                sql &= " AND ISNULL(M.BlockID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboBlock.SelectedValue & " ELSE ISNULL(M.BlockID, 0) END"
                sql &= " AND ISNULL(M.PaperID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPaper.SelectedValue & " ELSE ISNULL(M.PaperID, 0) END"

            Case "School"
                sql = "SELECT ISNULL(M.TrainingMarkID, 0) As TrainingMarkID, SchoolID As ObjectID, S.Name, '' As Description, M.Mark, M.Comments FROM tblSchools S inner join tblTrainingAttendants M on TA.BeneficiaryID = S.SchoolID "
                sql &= " left outer join tblTrainingMarks M on M.BeneficiaryID = TA.BeneficiaryID  "
                sql &= "inner join luBeneficiaryType B on B.BeneficiaryTypeID = TA.BeneficiaryTypeID WHERE TA.TrainingID = " & cboTraining.SelectedValue
                sql &= " AND ISNULL(M.PeriodID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPeriod.SelectedValue & " ELSE ISNULL(M.PeriodID, 0) END"
                sql &= " AND ISNULL(M.BlockID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboBlock.SelectedValue & " ELSE ISNULL(M.BlockID, 0) END"
                sql &= " AND ISNULL(M.PaperID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPaper.SelectedValue & " ELSE ISNULL(M.PaperID, 0) END"

            Case "SchoolStaff"
                sql = "SELECT M.TrainingMarkID, TA.BeneficiaryID As ObjectID, S.Name, '' As Description, M.Mark, M.Comments  FROM tblSchoolStaff S inner join tblTrainingAttendants M on TA.BeneficiaryID = S.SchoolStaffID "
                sql &= " inner join tblTrainingMarks M on M.BeneficiaryID = TA.BeneficiaryID "
                sql &= " inner join luBeneficiaryType B on B.BeneficiaryTypeID = TA.BeneficiaryTypeID WHERE TA.TrainingID = " & cboTraining.SelectedValue
                sql &= " AND M.PeriodID = " & cboPeriod.SelectedValue & " AND M.BlockID = " & cboBlock.SelectedValue & " AND M.PaperID = " & cboPaper.SelectedValue

            Case "HealthCenter", "Health Center", "HealthCenterStaff", "Health Center Staff"
                sql = "SELECT ISNULL(M.TrainingMarkID, 0) As TrainingMarkID, TA.TrainingAttendantID as ObjectID, H.FirstName, H.Surname, '' as Description, M.Mark, M.Comments FROM tblHealthCenterStaff H "
                sql &= "inner join tblHealthCenters HC on HC.HealthCenterID = H.HealthCenterID "
                sql &= "inner join tblWards W on W.WardID = HC.WardID "
                sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
                sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID inner join tblTrainingAttendants TA on TA.BeneficiaryID = H.HealthCenterStaffID "
                sql &= " inner join tblTrainingMarks M on M.BeneficiaryID = TA.BeneficiaryID "
                sql &= " inner join luBeneficiaryType B on B.BeneficiaryTypeID = TA.BeneficiaryTypeID WHERE TA.TrainingID = " & cboTraining.SelectedValue
                sql &= " AND M.PeriodID = " & cboPeriod.SelectedValue & " AND M.BlockID = " & cboBlock.SelectedValue & " AND M.PaperID = " & cboPaper.SelectedValue
                sql &= " " & Criteria

            Case "Household"
                sql = "SELECT ISNULL(M.TrainingMarkID, 0) As TrainingMarkID, B.BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name, '' as Description, M.Mark, M.Comments FROM tblBeneficiaries B inner join tblTrainingAttendants TA on TA.BeneficiaryID = B.BeneficiaryID "
                sql &= " left outer join tblTrainingMarks M on M.BeneficiaryID = TA.BeneficiaryID "
                sql &= "left outer join luBeneficiaryType BT on BT.BeneficiaryTypeID = TA.BeneficiaryTypeID WHERE TA.TrainingID = " & cboTraining.SelectedValue
                sql &= " AND ISNULL(M.PeriodID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPeriod.SelectedValue & " ELSE ISNULL(M.PeriodID, 0) END"
                sql &= " AND ISNULL(M.BlockID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboBlock.SelectedValue & " ELSE ISNULL(M.BlockID, 0) END"
                sql &= " AND ISNULL(M.PaperID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPaper.SelectedValue & " ELSE ISNULL(M.PaperID, 0) END"

            Case "Individual"
                sql = "SELECT ISNULL(M.TrainingMarkID, 0) As TrainingMarkID, B.BeneficiaryID as ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name, '' As Description, M.Mark, M.Comments FROM tblBeneficiaries B inner join tblTrainingAttendants TA on TA.BeneficiaryID = B.BeneficiaryID "
                sql &= " left outer join tblTrainingMarks M on M.BeneficiaryID = TA.BeneficiaryID  "
                sql &= "left outer join luBeneficiaryType BT on BT.BeneficiaryTypeID = TA.BeneficiaryTypeID WHERE TA.TrainingID = " & cboTraining.SelectedValue
                sql &= " AND ISNULL(M.PeriodID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPeriod.SelectedValue & " ELSE ISNULL(M.PeriodID, 0) END"
                sql &= " AND ISNULL(M.BlockID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboBlock.SelectedValue & " ELSE ISNULL(M.BlockID, 0) END"
                sql &= " AND ISNULL(M.PaperID, 0) = CASE WHEN ISNULL(M.TrainingMarkID, 0) > 0 THEN " & cboPaper.SelectedValue & " ELSE ISNULL(M.PaperID, 0) END"
        End Select

        Return sql

    End Function

    Private Sub radBeneficiaries_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radBeneficiaries.NeedDataSource

        radBeneficiaries.DataSource = DirectCast(ViewState("HealthBeneficiaries"), DataTable)

    End Sub

    Private Sub LoadGridAtt(ByVal Criteria As String)

        Dim sql As String = "SELECT HealthCenterStaffID as AttendantID, FirstName, Surname FROM tblHealthCenterStaff H "
        sql &= "inner join tblHealthCenters HC on HC.HealthCenterID = H.HealthCenterID "
        sql &= "inner join tblWards W on W.WardID = HC.WardID "
        sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
        sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID  "
        sql &= "WHERE H.HealthCenterStaffID IN (Select BeneficiaryID from tblTrainingAttendants WHERE TrainingID = " & cboTraining.SelectedValue & " AND BeneficiaryTypeID = " & cboBeneficiaryType.SelectedValue & ") "
        sql &= "AND HealthCenterStaffID Not IN "
        sql &= "(SELECT BeneficiaryID FROM tblTrainingMarks WHERE BlockID = " & cboBlock.SelectedValue & " AND PaperID = " & cboPaper.SelectedValue & " AND"
        sql &= " PeriodID = " & cboPeriod.SelectedValue & " AND BeneficiaryTypeID = " & cboBeneficiaryType.SelectedValue & " AND TrainingID = " & cboTraining.SelectedValue & ")"
        sql &= " " & Criteria

        With radAttendants

            .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
            .DataBind()

            ViewState("AttendantsExcept") = .DataSource

        End With

    End Sub
    Private Sub LoadGrid(ByVal Criteria As String)

        Dim sql As String = DertemineBeneficiaryTypeSQL(cboBeneficiaryType.SelectedItem.Text, Criteria)

        If sql <> "" Then

            With radBeneficiaries

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()

                ViewState("HealthBeneficiaries") = .DataSource

            End With

        End If

    End Sub

    Private Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click

        Response.Redirect("~/TrainingDetails.aspx?id=" & Server.HtmlEncode(objUrlEncoder.Encrypt(TrainingID)))

    End Sub

    Private Sub radBeneficiaries_UpdateCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radBeneficiaries.UpdateCommand

        Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)

        'Locate the changed row in the DataSource
        Dim changedRows() As DataRow = DirectCast(ViewState("HealthBeneficiaries"), DataTable).Select("TrainingMarkID = " & editedItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("TrainingMarkID"))

        'Update new values
        Dim newValues As Hashtable = New Hashtable
        'The GridTableView will fill the values from all editable columns in the hash
        e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
        changedRows(0).BeginEdit()

        Dim Mark As RadNumericTextBox = DirectCast(editedItem.FindControl("MarkTextBox"), RadNumericTextBox)
        Dim Comments As RadTextBox = DirectCast(editedItem.FindControl("CommentsTextBox"), RadTextBox)

        Try

            Dim objTrainingMarks As New TrainingMarks(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTrainingMarks

                .Mark = Mark.Text
                .TrainingMarkID = editedItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("TrainingMarkID")
                .Comments = Comments.Text

                .SaveTrainingMarks()

            End With


            For Each entry As DictionaryEntry In newValues

                changedRows(0)(CType(entry.Key, String)) = entry.Value

            Next

            changedRows(0).EndEdit()

        Catch ex As Exception

            changedRows(0).CancelEdit()
            radBeneficiaries.Controls.Add(New LiteralControl("Unable to update Marks. Reason: " & ex.Message))
            e.Canceled = True

        End Try

    End Sub

    Private Sub radAttendants_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radAttendants.NeedDataSource

        radAttendants.DataSource = DirectCast(ViewState("AttendantsExcept"), DataTable)

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        Dim Criteria As String = ""

        If cboHealthGroupType.SelectedValue > 0 Then

            Criteria &= IIf(Criteria <> "", " AND H.HealthGroupTypeID = " & cboHealthGroupType.SelectedValue, "")

        End If

        If cboProvince.SelectedValue > 0 Then

            Criteria &= IIf(Criteria <> "", " AND P.ProvinceID = " & cboProvince.SelectedValue, "")

        End If

        If Map() Then

            LoadGridAtt(Criteria)
            LoadGrid(Criteria)
            ShowMessage("Attendants processed successfully...", MessageTypeEnum.Information)

        Else

            ShowMessage("Failed to complete processing. An unexpected error occured...", MessageTypeEnum.Error)

        End If
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click

        Dim Criteria As String = ""

        If IsNumeric(cboHealthGroupType.SelectedValue) AndAlso cboHealthGroupType.SelectedValue > 0 Then

            Criteria &= IIf(Criteria <> "", " AND H.HealthGroupTypeID = " & cboHealthGroupType.SelectedValue, "")

        End If

        If IsNumeric(cboProvince.SelectedValue) AndAlso cboProvince.SelectedValue > 0 Then

            Criteria &= IIf(Criteria <> "", " AND P.ProvinceID = " & cboProvince.SelectedValue, "")

        End If

        If cboPaper.SelectedValue > 0 AndAlso cboBlock.SelectedValue > 0 AndAlso cboPeriod.SelectedValue > 0 And cboBeneficiaryType.SelectedValue > 0 Then

            LoadGridAtt(Criteria)
            LoadGrid(Criteria)

        Else

            ShowMessage("Please select all the required parameters", MessageTypeEnum.Error)

        End If

    End Sub
End Class