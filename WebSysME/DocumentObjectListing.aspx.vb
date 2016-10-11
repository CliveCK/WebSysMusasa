Imports Telerik.Web.UI
Imports System.IO

Public Class DocumentObjectListing
    Inherits System.Web.UI.Page

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

        If radObjectAutoComplete.DataSource Is Nothing AndAlso cboObjects.SelectedIndex > -1 Then BindObjectToAuto(cboObject:=cboObjects)

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboObjects

                .DataSource = objLookup.Lookup("luObjectTypes", "ObjectTypeID", "Description").Tables(0)
                .DataValueField = "ObjectTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub radFileListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radFileListing.ItemCommand

        If e.CommandName = "Download" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radFileListing.Items(index)
            Dim FilePath As String

            FilePath = Server.HtmlDecode(item("FilePath").Text)

            With Response

                .Clear()
                .ClearContent()
                .ClearHeaders()
                .BufferOutput = True

            End With

            If File.Exists(Request.PhysicalApplicationPath & FilePath) Or File.Exists(Server.MapPath("~/FileUploads/" & FilePath)) Then

                Dim oFileStream As FileStream
                Dim fileLen As Long

                Try

                    oFileStream = File.Open(Server.MapPath("~/FileUploads/" & FilePath), FileMode.Open, FileAccess.Read, FileShare.None)
                    fileLen = oFileStream.Length

                    Dim ByteFile(fileLen - 1) As Byte

                    If fileLen > 0 Then
                        oFileStream.Read(ByteFile, 0, oFileStream.Length - 1)
                        oFileStream.Close()

                        With Response

                            .AddHeader("Content-Disposition", "attachment; filename=" & FilePath.Replace(" ", "_"))
                            .ContentType = "application/octet-stream"
                            .BinaryWrite(ByteFile)
                            '.WriteFile(Server.MapPath("~/FileUploads/" & FilePath))
                            .End()
                            HttpContext.Current.ApplicationInstance.CompleteRequest()

                        End With

                    Else
                        log.Error("Empty File...")
                    End If

                Catch ex As Exception

                End Try

            Else

                ShowMessage("Error: File not found!!!", MessageTypeEnum.Error)

            End If
        End If

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radFileListing.Items(index1)
            Dim FileID As Integer

            FileID = Server.HtmlDecode(item1("FileID").Text)

            Response.Redirect("~/Files.aspx?id=" & objUrlEncoder.Encrypt(FileID))

        End If

    End Sub

    Private Sub BindObjectToAuto(ByVal cboObject As DropDownList)

        Dim objLookup As New BusinessLogic.CommonFunctions

        Select Case cboObject.SelectedItem.Text

            Case "Staff"
                LoadCombo("tblStaffMembers", "FirstName", "StaffID")

            Case "Group"
                LoadCombo("tblGroups", "GroupName", "GroupID")

            Case "Organization"
                LoadCombo("tblOrganization", "Name", "OrganizationID")

            Case "Project"
                LoadCombo("tblProjects", "Name", "Project")

            Case "School"
                LoadCombo("tblSchools", "Name", "SchoolID")

            Case "HealthCenter"
                LoadCombo("tblHealthCenters", "Name", "HealthCenterID")

            Case "Household"
                LoadCombo("tblBeneficiaries", "FirstName", "BeneficiaryID")

            Case "Individual"
                LoadCombo("tblBeneficiaries", "FirstName", "BeneficiaryID")

            Case "Activity"
                LoadCombo("tblActivities", "Description", "ActivityID")

            Case "Training"
                LoadCombo("tblTrainings", "Name", "TrainingID")

            Case "District"
                LoadCombo("tblDistricts", "Name", "District")

            Case "Province"
                LoadCombo("tblProvinces", "Name", "ProvinceID")

            Case "City"
                LoadCombo("tblCities", "Name", "CityID")

            Case Else
                ShowMessage("Object not recognised. Failed to populate data...", MessageTypeEnum.Error)

        End Select

    End Sub

    Public Sub LoadCombo(ByVal Table As String, ByVal TextField As String, ByVal ValueField As String)

        Dim objLookup As New BusinessLogic.CommonFunctions

        With radObjectAutoComplete

            .DataSource = objLookup.Lookup(Table, ValueField, TextField).Tables(0)
            .DataValueField = ValueField
            .DataTextField = TextField
            .DataBind()

        End With

    End Sub

    Private Sub cboObjects_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboObjects.SelectedIndexChanged

        BindObjectToAuto(cboObject:=cboObjects)
        lblFilter.Text = cboObjects.SelectedItem.Text

    End Sub

    Public Function GenerateSQL(ByVal ObjectText As String) As String

        Dim Sql As String = ""

        Select Case ObjectText

            Case "Staff"
                Sql = "SELECT * FROM tblFiles WHERE CreatedBy = " & IIf(IsNumeric(radObjectAutoComplete.Entries.Item(0).Value), radObjectAutoComplete.Entries.Item(0).Value, 0)



        End Select

        Return Sql

    End Function

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click

        Try

            If cboObjects.SelectedValue > 0 Then

                Dim sql As String = ""

                Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                Dim OtherCriteria As String = IIf(DertemineObjectType(cboObjects.SelectedValue, objFiles),
                    " UNION SELECT * FROM tblFiles WHERE CreatedBy = " & TranslateObjectIDToUserID(cboObjects.SelectedValue, IIf(IsNumeric(radObjectAutoComplete.Entries.Item(0).Value), radObjectAutoComplete.Entries.Item(0).Value, 0), objFiles), "")

                sql = "SELECT F.* FROM tblFiles F left outer join tblDocumentObjects DO on F.FileID = DO.DocumentID "
                sql &= "inner join luObjectTypes O on O.ObjectTypeID = DO.ObjectTypeID WHERE DO.ObjectTypeID = " & cboObjects.SelectedValue
                sql &= " AND DO.ObjectID = " & IIf(IsNumeric(radObjectAutoComplete.Entries.Item(0).Value), radObjectAutoComplete.Entries.Item(0).Value, 0)
                sql &= OtherCriteria


                With radFileListing

                    .DataSource = objFiles.GetFiles(Sql).Tables(0)
                    .DataBind()

                    ViewState("MyFiles") = .DataSource

                End With

            End If

        Catch ex As Exception

            log.Error(ex)

        End Try

    End Sub

    Private Function DertemineObjectType(ByVal ObjectTypeID As Long, ByVal objFiles As BusinessLogic.Files) As Boolean

        Dim oArray As String() = New String() {"Staff", "Organization"}

        Return oArray.Contains(objFiles.GetObjectTypeByID(ObjectTypeID))

    End Function

    Private Function TranslateObjectIDToUserID(ByVal objectTypeID As Long, ByVal ObjectID As Long, ByVal objFiles As BusinessLogic.Files) As Long

        Dim User As New SecurityPolicy.UserAccountProfileLink(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return User.GetUserIDByStaffOrOrganizationID(ObjectID, objFiles.GetObjectTypeByID(objectTypeID))

    End Function

    Private Sub radFileListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radFileListing.NeedDataSource

        radFileListing.DataSource = DirectCast(ViewState("MyFiles"), DataTable)

    End Sub
End Class