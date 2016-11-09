Imports System.Data.OleDb
Imports System.IO
Imports Telerik.Web.UI
Imports Universal.CommonFunctions
Public Class ExcelUploadConsole
    Inherits System.Web.UI.Page

    Private dsMaritalStatus As DataSet
    Private dsLevelOfEducation As DataSet

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

        If Not IsPostBack Then

        End If

        'PopulateLookupDatasets()

    End Sub

    Private Sub PopulateLookupDatasets()

        Dim objTraining As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objTraining

            dsMaritalStatus = .GetTraining("Select * FROM luMaritalStatus")
            dsLevelOfEducation = .GetTraining("Select * from tblLevelOfEducation")

        End With

    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click

        Try

            Dim dt As DataTable = DirectCast(Session("dtExcel"), DataTable)

            If IsNothing(Session("Origin")) Then

                ShowMessage("Missing critical Information...process aborted!! Contact Admin...", MessageTypeEnum.Error)
                Exit Sub

            Else

                If Replace(Session("Origin"), "$", "") <> rdoType.SelectedValue Then

                    ShowMessage("Signature mismatch!...The file uploaded has a different signature than the specified upload type.", MessageTypeEnum.Error)
                    Exit Sub

                End If

            End If

            PopulateLookupDatasets()
            Dim count As Long = 0
            Dim errCount As Long = 0

            'First add new records into the system
            For Each row As DataRow In dt.Rows

                If row("RecordStatus") = "New record" Then

                    If Catchnull(row("FirstName"), "") = "" Or Catchnull(row("Surname"), "") = "" Or Not IsDate(row("DateOfBirth")) Or Catchnull(row("NationalIDNo"), "") = "" Then

                        row("RecordStatus") = "Missing critical field(s). Please revise record!"
                        errCount += 1

                    Else

                        Dim BeneficiaryID = AddNewRecord(row)

                        If BeneficiaryID > 0 Then

                            SaveDetails(row, BeneficiaryID)
                            row("BeneficiaryID") = BeneficiaryID
                            row("RecordStatus") = "Success"

                            count += 1

                        Else

                            row("RecordStatus") = "Not saved"
                            errCount += 1

                        End If

                    End If

                ElseIf row("RecordStatus") = "Record Exists" Or row("RecordStatus") = "Not added - Already exists in system" Or row("RecordStatus") = "Success" Then

                    row("RecordStatus") = "Not added - Already exists in system"

                End If

            Next

            dt.AcceptChanges()

            With radRecords

                .DataSource = dt
                .DataBind()

            End With

            If count > 0 And errCount = 0 Then
                ShowMessage("Uploaded file processed successfully: " & count & " records added", MessageTypeEnum.Information)

            ElseIf count > 0 And errCount > 0 Then
                ShowMessage("Uploaded file processed successfully: " & count & " records added...But some records could not be added", MessageTypeEnum.Information)
            ElseIf count = 0 And errCount > 0 Then
                ShowMessage("Could not save. An error occured...", MessageTypeEnum.Error)
            End If

        Catch ex As Exception

            ShowMessage("A fatal error occured! Please contact your administrator...", MessageTypeEnum.Error)

        End Try

    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click

        If fUpload.HasFile Then
            Dim FileName As String = Path.GetFileName(fUpload.PostedFile.FileName)
            Dim Extension As String = Path.GetExtension(fUpload.PostedFile.FileName)
            Dim FolderPath As String = "Temp/"

            Dim FilePath As String = Server.MapPath(FolderPath + FileName)
            fUpload.SaveAs(FilePath)
            ImportExcelToGrid(FilePath, Extension)
        End If

    End Sub

    Private Sub ImportExcelToGrid(ByVal FilePath As String, ByVal Extension As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03 
                conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
                Exit Select
            Case ".xlsx"
                'Excel 07 
                conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select

        conStr = String.Format(conStr, FilePath)

        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        Dim dt As New DataTable()
        Dim dtTraining As New DataTable()
        Dim dtPeriod As New DataTable
        Dim dtBlock As New DataTable
        Dim err As Int16 = 0

        cmdExcel.Connection = connExcel

        'Get the name of First Sheet 
        connExcel.Open()

        Dim dtExcelSchema As DataTable
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
        connExcel.Close()
        Dim Address As String = ""

        If SheetName = "Reception$" Then
            Address = "U200"
        ElseIf SheetName = "Sheet2$" Then
            SheetName = "Shelter$"
            Address = "AG200"
        ElseIf SheetName = "Shelter$" Then
            Address = "AG200"
        ElseIf SheetName = "Counsellor$" Then
            Address = "AF200"
        ElseIf SheetName = "Lawyer" Then
            Address = "S200"
        End If

        Session("Origin") = SheetName

        'Read Data from First Sheet 
        connExcel.Open()

        Try
            cmdExcel.CommandText = "SELECT * From [" & SheetName & "A1:" & Address & "] WHERE [Firstname] <> ''"
            oda.SelectCommand = cmdExcel
            oda.Fill(dt)
            connExcel.Close()
            'Sift through the data to dertemine record status

            Dim dtColumn As New DataColumn("RecordStatus", GetType(String))
            Dim dtColumn1 As New DataColumn("BeneficiaryID", GetType(Long))
            dt.Columns.Add(dtColumn)
            dt.Columns.Add(dtColumn1)

            For Each row As DataRow In dt.Rows

                If IsNothing(row("NationalIDNo")) Then

                    err = 1
                    ShowMessage("NationalIDNo column does not exist from uploaded file!!", MessageTypeEnum.Error)
                    Exit For

                End If

                Dim record As DataSet = IsNewRecord(Catchnull(row("NationalIDNo"), 0, True))
                If Not IsNothing(record) AndAlso record.Tables.Count > 0 AndAlso record.Tables(0).Rows.Count > 0 Then

                    row("BeneficiaryID") = record.Tables(0).Rows(0)("BeneficiaryID")
                    row("RecordStatus") = "Record Exists"

                Else

                    row("RecordStatus") = "New record"

                End If

                dt.AcceptChanges()

            Next

            'Bind Data to GridView 

            If err = 0 Then

                With radRecords

                    .DataSource = dt
                    .DataBind()

                    Session("dtExcel") = .DataSource

                    pnlRecords.Visible = True

                End With

            End If

        Catch ex As Exception

            connExcel.Close()

        End Try

    End Sub

    Private Function IsNewRecord(ByVal IDNumber As String) As DataSet

        Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objBeneficiary.RetriveRecordByID(IDNumber)

    End Function

    Private Function AddNewRecord(ByVal row As DataRow) As Long

        Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Dim dsDistrict As DataSet = objBeneficiary.GetBeneficiary("SELECT * FROM tblDistricts")
        Dim dsWards As DataSet = objBeneficiary.GetBeneficiary("SELECT *, D.Name as District FROM tblWards W inner join tblDistricts D on D.DistrictID = W.DistrictID")
        Dim dsVillages As DataSet = objBeneficiary.GetBeneficiary("SELECT * FROM tblVillages")

        Dim District As Long = 0
        Dim Ward As Long = 0
        Dim Village As Long = 0

        If dsDistrict.Tables(0).Select("Name = '" & row("District") & "'").Length > 0 Then
            District = Catchnull(dsDistrict.Tables(0).Select("Name = '" & row("District") & "'")(0).Item("DistrictID"), 0)
        End If
        If dsWards.Tables(0).Select("Name = '" & row("Ward") & "' AND District = '" & row("District") & "'").Length > 0 Then
            Ward = Catchnull(dsWards.Tables(0).Select("Name = '" & row("Ward") & "' AND District = '" & row("District") & "'")(0).Item("WardID"), 0)
        End If

        With objBeneficiary

            .BeneficiaryID = 0
            .FirstName = row("Firstname")
            .Surname = row("Surname")
            .Sex = row("Sex")
            .DateOfBirth = row("DateOfBirth")
            .NationlIDNo = row("NationalIDNo")
            .ContactNo = row("PhoneNumber")
            .MaritalStatus = dsMaritalStatus.Tables(0).Select("Description = '" & row("MarriageType") & "'")(0).Item("ObjectID")
            Try
                If Not IsNothing(row("LevelOfEducation")) Then .LevelOfEducation = dsLevelOfEducation.Tables(0).Select("Description = '" & row("LevelOfEducation") & "'")(0).Item("ObjectID")
            Catch ex As Exception : End Try
            .Suffix = 1
            .IsDependant = 0

            If .Save Then

                .MemberNo = .GenerateMemberNo()

                If .MemberNo <> "" Then .Save()

                With objAddress

                    .IsUrban = 0
                    .OwnerID = objBeneficiary.BeneficiaryID
                    .DistrictID = District
                    .WardID = Ward
                    '.VillageID = Village

                    .Save()

                End With

                Return .BeneficiaryID

            End If

        End With

        Return 0

    End Function

    Private Sub radRecords_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radRecords.NeedDataSource

        radRecords.DataSource = DirectCast(ViewState("dtExcel"), DataTable)

    End Sub

    Private Function SaveDetails(ByVal row As DataRow, ByVal BeneficiaryID As Long) As Boolean

        If Not IsNothing(Session("Origin")) Then

            Select Case rdoType.SelectedValue

                Case "Reception"
                    Return SaveReceptionDetails(row, BeneficiaryID)

                Case "Counsellor"
                    Return SaveCounsellorDetails(row, BeneficiaryID)

                Case "Lawyer"
                    Return SaveLawyerDetails(row, BeneficiaryID)

                Case "Shelter"
                    Return SaveShelterDetails(row, BeneficiaryID)

            End Select

        End If

        Return False

    End Function

    Private Function SaveReceptionDetails(ByVal row As DataRow, ByVal BeneficiaryID As Long) As Boolean

        Dim objReception As New BusinessLogic.ClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objReception

            .ClientDetailID = 0
            .NoOfChildren = Catchnull(row("NoOfChildren"), 0)
            .BeneficiaryID = BeneficiaryID
            .EmploymentStatusID = Catchnull(row("EmploymentStatus"), "")
            .AccompanyingChildren = Catchnull(row("AccompanyingChildren"), 0)
            .CapturedFromID = 1
            .NextOfKin = Catchnull(row("NextOfKin"), "")
            .ContactNo = Catchnull(row("ContactNo"), "")
            .NatureOfRelationship = Catchnull(row("NatureOfRelationship"), "")
            .AccompanyingAdult1 = Catchnull(row("AccompanyingAdult1"), "")
            .AccompanyingAdult2 = Catchnull(row("AccompanyingAdult2"), "")
            .ReferredBy = Catchnull(row("ReferredBy"), "")

            If .Save Then

                Return True

            End If

        End With

        Return False

    End Function

    Private Function SaveCounsellorDetails(ByVal row As DataRow, ByVal BeneficiaryID As Long)

        Dim objInitialSession As New BusinessLogic.InitialCounsellingSession(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objInitialSession

            .InitialCounsellingSessionID = 0
            .BeneficiaryID = BeneficiaryID
            .HowManyTimes = Catchnull(row("HowManyTimes"), 0, True)
            .SessionDate = Now
            If IsDate(row("NextAppointmentDate")) Then .NextAppointmentDate = row("NextAppointmentDate")
            .PresentingProblem = Catchnull(row("PresentingProblem"), "")
            .Other = Catchnull(row("OtherOptionAvailable"), "")
            .CaseReported = Catchnull(row("CaseReported?"), "")
            .WhereWasProblemReported = Catchnull(row("WhereProblemReported"), "")
            .ChallengesFaced = Catchnull(row("ChallengesFaced"), "")
            .MedicalReport = Catchnull(row("MedicalReport?"), "")
            .DurationOfSession = Catchnull(row("DurationInMinutes"), 0, True)
            .IssuedBy = Catchnull(row("IssuedBy"), "")
            .ProblemsExperienced = Catchnull(row("ProblemsExperienced"), "")
            .ClientExpectations = Catchnull(row("ClientExpectations"), "")
            .OtherOptionAvailable = Catchnull(row("OtherOptionAvailable"), "")
            .Referral = Catchnull(row("ReferalIfAny"), "")
            .CarePlan = Catchnull(row("CarePlan"), "")
            .EmploymentStatus = Catchnull(row("EmploymentStatus"), "")
            .NextOfkin = Catchnull(row("NextOfKin"), "")
            .Support = Catchnull(row("SupportToClient"), "")

            If .Save Then

                Return True

            End If

        End With

        Return False

    End Function

    Private Function SaveLawyerDetails(ByVal row As DataRow, ByVal BeneficiaryID As Long) As Boolean

        Dim objLawyer As New BusinessLogic.LawyerClientSessionDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objLawyer

            '.LawyerClientSessionDetailID = 0
            '.BeneficiaryID = BeneficiaryID
            'If cboActionToBeTaken.SelectedIndex > -1 Then .ActionTobeTakenID = cboActionToBeTaken.SelectedValue
            'If cboReferredTo.SelectedIndex > 0 Then .ReferralID = cboReferredTo.SelectedValue
            'If radSessionDate.SelectedDate.HasValue Then .SessionDate = radSessionDate.SelectedDate
            '.CaseNotes = txtCaseNotes.Text
            '.ReferralOther = txtOtherReferDetails.Text
            '.OtherProblem = txtOtherProblem.Text

        End With

        Return False

    End Function

    Private Function SaveShelterDetails(ByVal row As DataRow, ByVal BeneficiaryID As Long)

        Dim objShelter As New BusinessLogic.ShelterClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objShelter

            .BeneficiaryID = BeneficiaryID
            .EmployerTelNo = Catchnull(row("TelephoneNo"), "")
            .EmployerAddress = Catchnull(row("PhysicalAddress"), "")
            .ReferredBy = Catchnull(row("ReferredBy"), "")
            .ReferrerTelNo = Catchnull(row("ReferralTelNo"), "")
            .SheltedBefore = Catchnull(row("HaveShelteredBefore"), "")
            .InjuriesSustained = Catchnull(row("InjuriesInAttack"), "")
            .AnySpecialMedicalNeeds = Catchnull(row("SpecialMedicalNeeds"), "")
            .MedicalNeeds = Catchnull(row("IfYesState"), "")
            .CarePlan = Catchnull(row("CarePlan"), "")
            .PresentingProblem = Catchnull(row("PresentingProblem"), "")
            .SkillsToNature = Catchnull(row("SkillsToNature"), "")
            .Skills = Catchnull(row("Skills"), "")
            .Name = Catchnull(row("EmergencyName"), "")
            .Relationship = Catchnull(row("Relationship"), "")
            .ContactNo = Catchnull(row("TelNo"), "")
            .ContactAddress = Catchnull(row("Address"), "")
            .ArrivalTime = Catchnull(row("ArrivalTime"), "")
            .TotalAdmitted = Catchnull(row("TotalAdmitted"), "")
            .HIVStatus = Catchnull(row("HIVStatus"), "")
            .DiscloseStatus = Catchnull(row("DiscloseStatus"), False)
            .TestedForHIV = Catchnull(row("TestedForHIV"), False)
            .OnART = Catchnull(row("onART"), "")

            If .Save Then

                Return True

            End If

        End With

        Return False

    End Function

    Private Sub radRecords_PreRender(sender As Object, e As EventArgs) Handles radRecords.PreRender

        radRecords.MasterTableView.GetColumn("BeneficiaryID").Display = False

    End Sub
End Class