Imports System.Data.OleDb
Imports System.IO
Imports Universal.CommonFunctions

Public Class IndividualInforUpload
    Inherits System.Web.UI.Page

    Private dsMaritalStatus As DataSet

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

        End With

    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click

        Try

            Dim dt As DataTable = DirectCast(Session("dt"), DataTable)

            PopulateLookupDatasets()
            Dim count As Long = 0
            Dim errCount As Long = 0

            'First add new records into the system
            For Each row As DataRow In dt.Rows

                If row("RecordStatus") = "New record" Then

                    If Catchnull(row("FirstName"), "", True) = "" Or Catchnull(row("Surname"), "", True) = "" Or row("MaritalStatus") = "" Or Not IsDate(row("DateOfBirth")) Or row("Sex") = "" Or Catchnull(row("CellphoneNo"), 0) = 0 Or Catchnull(row("IDnumber"), 0) = 0 Then

                        row("RecordStatus") = "Missing critical field(s). Please revise record!"
                        errCount += 1

                    Else

                        Dim StaffID = AddNewRecord(row)

                        If StaffID > 0 Then

                            row("BeneficiaryID") = StaffID
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

            With radIndividuals

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

        'Read Data from First Sheet 
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * From [Records$A3:J200] WHERE [Firstname] <> ''"
        oda.SelectCommand = cmdExcel
        oda.Fill(dt)

        'Sift through the data to dertemine record status

        Dim dtColumn As New DataColumn("RecordStatus", GetType(String))
        Dim dtColumn1 As New DataColumn("BeneficiaryID", GetType(Long))
        dt.Columns.Add(dtColumn)
        dt.Columns.Add(dtColumn1)

        For Each row As DataRow In dt.Rows

            If IsNothing(row("IDNumber")) Then

                err = 1
                ShowMessage("IDNumber column does not exist from uploaded file!!", MessageTypeEnum.Error)
                Exit For

            End If

            Dim record As DataSet = IsNewRecord(Catchnull(row("IDNumber"), 0, True))
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

            With radIndividuals

                .DataSource = dt
                .DataBind()

                Session("dt") = .DataSource

                pnlRecords.Visible = True

            End With

        End If

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
        If dsVillages.Tables(0).Select("Name = '" & row("Village") & "'").Length > 0 Then
            Village = Catchnull(dsVillages.Tables(0).Select("Name = '" & row("Village") & "'")(0).Item("VillageID"), 0)
        End If

        With objBeneficiary

            .BeneficiaryID = 0
            .FirstName = row("Firstname")
            .Surname = row("Surname")
            .Sex = row("Sex")
            .DateOfBirth = row("DateOfBirth")
            .NationlIDNo = row("IDNumber")
            .ContactNo = row("CellphoneNo")
            .MaritalStatus = dsMaritalStatus.Tables(0).Select("Description = '" & row("MaritalStatus") & "'")(0).Item("ObjectID")
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
                    .VillageID = Village

                    .Save()

                End With

                Return .BeneficiaryID

            End If

        End With

        Return 0

    End Function
End Class