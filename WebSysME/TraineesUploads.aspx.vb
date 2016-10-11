Imports System.Data.OleDb
Imports System.IO
Imports Universal.CommonFunctions

Public Class TraineesUploads
    Inherits System.Web.UI.Page

    Private dsHealth As DataSet
    Private dsProvince As DataSet
    Private dsDistrict As DataSet
    Private dsStaffRoles As DataSet
    Private dsTraining As DataSet
    Private dsBlock As DataSet
    Private dsPeriod As DataSet
    Private dsDepartments As DataSet
    Private dsGroupType As DataSet
    Private dsPaper As DataSet
    Private TrainingID As Long
    Private BlockID As Long
    Private PeriodID As Long

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

            dsTraining = .GetTraining("Select * FROM tblTrainings")
            dsStaffRoles = .GetTraining("SELECT * FROM luStaffPosition")
            dsHealth = .GetTraining("Select * FROM tblHealthCenters")
            dsPeriod = .GetTraining("Select * FROM luPeriod")
            dsBlock = .GetTraining("Select * FROM luBlock")
            dsGroupType = .GetTraining("Select * FROM luHealthGroupTypes")
            dsProvince = .GetTraining("Select * FROM tblProvinces")
            dsDistrict = .GetTraining("Select * FROM tblDistricts")
            dsDepartments = .GetTraining("Select * FROM luDepartments")
            dsPaper = .GetTraining("Select * FROM luPaper")

        End With

    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click

        Try

            Dim dt As DataTable = DirectCast(Session("dt"), DataTable)
            Dim sql As New StringBuilder

            PopulateLookupDatasets()

            TrainingID = Catchnull(dsTraining.Tables(0).Select("Name = '" & DirectCast(Session("dtTraining"), DataTable).Rows(0)(0) & "'")(0).Item("TrainingID"), 0)
            BlockID = Catchnull(dsBlock.Tables(0).Select("Description = '" & DirectCast(Session("dtBlock"), DataTable).Rows(0)(0) & "'")(0).Item("BlockID"), 0)
            PeriodID = Catchnull(dsPeriod.Tables(0).Select("Description = '" & DirectCast(Session("dtPeriod"), DataTable).Rows(0)(0) & "'")(0).Item("PeriodID"), 0)
            Dim count As Long = 0

            Dim objTrainingAttendants As New BusinessLogic.TrainingAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim BeneficiaryTypeID As Long = objTrainingAttendants.GetBeneficiaryTypeIDByName("HealthCenter")

            If TrainingID > 0 Then

                'First add new records into the system
                For Each row As DataRow In dt.Rows

                    If row("RecordStatus") = "New record" Then

                        Dim StaffID = AddNewRecord(row)

                        If StaffID > 0 Then

                            row("HealthCenterStaffID") = StaffID
                            row("RecordStatus") = "OK"

                        End If

                    ElseIf row("RecordStatus") = "Record Exists" Then

                        row("RecordStatus") = "OK"

                    End If

                Next

                dt.AcceptChanges()

                'then add the attendants into training + build inserts for Marks

                With sql

                    .Append("INSERT INTO tblTrainingMarks (TrainingID, BlockID, PeriodID, PaperID, BeneficiaryID, BeneficiaryTypeID, Mark)")
                    .AppendLine("VALUES")

                    For Each record As DataRow In dt.Rows

                        If AddToTraining(record, TrainingID) Then

                            If Not objTrainingAttendants.MarksRecordExists(TrainingID, BlockID, PeriodID, dsPaper.Tables(0).Select("Description = 'Theory Pretest'")(0).Item("PaperID"), record("HealthCenterStaffID"), BeneficiaryTypeID) Then

                                If count <> 0 Then .Append(",")
                                .AppendLine("(" & TrainingID & "," & BlockID & "," & PeriodID & "," & dsPaper.Tables(0).Select("Description = 'Theory Pretest'")(0).Item("PaperID") & "," & record("HealthCenterStaffID") & "," & BeneficiaryTypeID & "," & record("Theory Pretest") & ")")
                                count += 1

                            End If

                            If Not objTrainingAttendants.MarksRecordExists(TrainingID, BlockID, PeriodID, dsPaper.Tables(0).Select("Description = 'Theory Postest'")(0).Item("PaperID"), record("HealthCenterStaffID"), BeneficiaryTypeID) Then

                                If count <> 0 Then .Append(",")
                                .AppendLine("(" & TrainingID & "," & BlockID & "," & PeriodID & "," & dsPaper.Tables(0).Select("Description = 'Theory Postest'")(0).Item("PaperID") & "," & record("HealthCenterStaffID") & "," & BeneficiaryTypeID & "," & record("Theory Postest") & ")")
                                count += 1

                            End If

                            If Not objTrainingAttendants.MarksRecordExists(TrainingID, BlockID, PeriodID, dsPaper.Tables(0).Select("Description = 'Skills Pretest'")(0).Item("PaperID"), record("HealthCenterStaffID"), BeneficiaryTypeID) Then

                                If count <> 0 Then .Append(",")
                                .AppendLine("(" & TrainingID & "," & BlockID & "," & PeriodID & "," & dsPaper.Tables(0).Select("Description = 'Skills Pretest'")(0).Item("PaperID") & "," & record("HealthCenterStaffID") & "," & BeneficiaryTypeID & "," & record("Skills Pretest") & ")")
                                count += 1

                            End If

                            If Not objTrainingAttendants.MarksRecordExists(TrainingID, BlockID, PeriodID, dsPaper.Tables(0).Select("Description = 'Skills Postest'")(0).Item("PaperID"), record("HealthCenterStaffID"), BeneficiaryTypeID) Then

                                If count <> 0 Then .Append(",")
                                .AppendLine("(" & TrainingID & "," & BlockID & "," & PeriodID & "," & dsPaper.Tables(0).Select("Description = 'Skills Postest'")(0).Item("PaperID") & "," & record("HealthCenterStaffID") & "," & BeneficiaryTypeID & "," & record("Theory Postest") & ")")
                                count += 1

                            End If

                            If count = 0 Then record("RecordStatus") = "Some Marks exist" Else record("RecordStatus") = "Success"

                        Else

                            record("RecordStatus") = "Failed"

                        End If

                    Next

                End With

                Dim objTrainingMarks As New BusinessLogic.TrainingMarks(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                If objTrainingMarks.AddTrainingMarksBatch(sql.ToString) Then

                    ShowMessage("Uploaded file processed successfully", MessageTypeEnum.Information)

                End If

                dt.AcceptChanges()

                With radTraining

                    .DataSource = dt
                    .DataBind()

                End With

            Else

                ShowMessage("Please supply Training for listed Attendants!", MessageTypeEnum.Error)

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

        'E5 - Training
        'H5 - Period
        'K5 - Block

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

        'Read Data from First Sheet 
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * From [MySheet$A7:R130] WHERE [Firstname] <> ''"
        oda.SelectCommand = cmdExcel
        oda.Fill(dt)

        cmdExcel.CommandText = "SELECT * From [MySheet$E4:E5]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dtTraining)

        cmdExcel.CommandText = "SELECT * From [MySheet$H4:H5]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dtPeriod)

        cmdExcel.CommandText = "SELECT * From [MySheet$K4:K5]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dtBlock)
        connExcel.Close()

        Session("dtTraining") = dtTraining
        Session("dtPeriod") = dtPeriod
        Session("dtBlock") = dtBlock

        'Sift through the data to dertemine record status

        Dim dtColumn As New DataColumn("RecordStatus", GetType(String))
        Dim dtColumn1 As New DataColumn("HealthCenterStaffID", GetType(Long))
        dt.Columns.Add(dtColumn)
        dt.Columns.Add(dtColumn1)

        For Each row As DataRow In dt.Rows

            If IsNothing(row("IDNumber")) Then

                err = 1
                ShowMessage("IDNumber column does not exist from uploaded file!!", MessageTypeEnum.Error)
                Exit For

            End If

            Dim record As DataSet = IsNewRecord(row("IDNumber"))
            If Not IsNothing(record) AndAlso record.Tables.Count > 0 AndAlso record.Tables(0).Rows.Count > 0 Then

                row("HealthCenterStaffID") = record.Tables(0).Rows(0)("HealthCenterStaffID")
                row("RecordStatus") = "Record Exists"

            Else

                row("RecordStatus") = "New record"

            End If

            dt.AcceptChanges()

        Next

        'Bind Data to GridView 

        If err = 0 Then

            With radTraining

                .DataSource = dt
                .DataBind()

                Session("dt") = .DataSource

                pnlRecords.Visible = True

            End With

        End If

    End Sub

    Private Function IsNewRecord(ByVal IDNumber As String) As DataSet

        Dim objHealthStaff As New BusinessLogic.HealthCenterStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objHealthStaff.RetriveRecordByID(IDNumber)

    End Function

    Private Function AddNewRecord(ByVal row As DataRow) As Long

        Dim objHealthCenter As New BusinessLogic.HealthCenterStaff(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objHealthCenter

            .HealthCenterStaffID = 0
            .HealthCenterID = dsHealth.Tables(0).Select("Name = '" & row("HealthCenter") & "'")(0).Item("HealthCenterID")
            .FirstName = row("Firstname")
            .Surname = row("Surname")
            .Sex = row("Sex")
            .DOB = row("DOB")
            .Title = row("Title")
            .IDNumber = row("IDNumber")
            .ContactNo = row("ContactNo")
            .ProvinceID = dsProvince.Tables(0).Select("Name = '" & row("Province") & "'")(0).Item("ProvinceID")
            .DistrictID = dsDistrict.Tables(0).Select("Name = '" & row("District") & "'")(0).Item("DistrictID")
            .StaffRoleID = dsStaffRoles.Tables(0).Select("Description = '" & row("StaffRole") & "'")(0).Item("PositionID")
            .DepartmentID = dsDepartments.Tables(0).Select("Description = '" & row("Department") & "'")(0).Item("DepartmentID")
            .GroupTypeID = dsGroupType.Tables(0).Select("Description = '" & row("GroupType") & "'")(0).Item("HealthGroupTypeID")

            If .Save Then

                Return .HealthCenterStaffID

            End If

        End With

        Return 0

    End Function

    Private Function AddToTraining(ByVal row As DataRow, ByVal TrainingID As Long) As Boolean

        If row("RecordStatus").ToString = "OK" Then

            Dim objTrainingAttendants As New BusinessLogic.TrainingAttendants(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTrainingAttendants

                Dim BeneficiaryTypeID As Long = .GetBeneficiaryTypeIDByName("HealthCenter")

                If Not .CheckAttendantExistence(row("HealthCenterStaffID"), TrainingID, BeneficiaryTypeID) Then

                    .TrainingID = TrainingID
                    .BeneficiaryTypeID = BeneficiaryTypeID
                    .BeneficiaryID = row("HealthCenterStaffID")

                    If .Save Then

                        Return True

                    End If

                Else

                    Return True

                End If

            End With

        End If

        Return False

    End Function
End Class