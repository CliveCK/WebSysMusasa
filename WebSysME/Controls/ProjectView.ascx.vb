Imports Telerik.Web.UI
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class ProjectView
    Inherits System.Web.UI.UserControl

    Private dsFileTypes As DataSet
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)


    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        If Not Page.IsPostBack Then

            CookiesWrapper.RadTreeSelectedNode = ""

        End If

    End Sub

    Private Sub LoadProjectDetails(ByVal nodeText As String)

        Dim objProject As New BusinessLogic.Projects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If objProject.Retrieve(objUrlEncoder.Decrypt(Request.QueryString("id"))) Then

            lblProject.Text = objProject.Name & " - " & nodeText
            lblProject.ForeColor = Drawing.Color.DarkBlue

        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadProjectDetails("")

        End If

        If Not IsNothing(Request.QueryString("id")) Then

            txtProjectID.Text = objUrlEncoder.Decrypt(Request.QueryString("id"))

        End If

    End Sub

    Private Sub radProjectTree_NodeClick(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles radProjectTree.NodeClick

        EnableControls(Nothing)
        radFileListing.Visible = False
        radListing.Visible = False

        LoadProjectDetails(radProjectTree.SelectedNode.Text)

        If CheckFileType(radProjectTree.SelectedNode.Value) Then

            Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With radFileListing

                .DataSource = objFiles.GetFilesByFileType(radProjectTree.SelectedNode.Value, "Project", IIf(IsNumeric(txtProjectID.Text), txtProjectID.Text, 0))
                .DataBind()
                .Rebind()

            End With

            radFileListing.Visible = True

        End If

        If radProjectTree.SelectedNode.Value = "Project Meetings" Then

            CookiesWrapper.RadTreeSelectedNode = "Project Meetings"

            EnableControls(ucProjectMeetingControl)

            radFileListing.Visible = False

        End If

        If radProjectTree.SelectedNode.Value = "Schedule" Then

            CookiesWrapper.RadTreeSelectedNode = "Schedule"

            EnableControls(ucSchedulerControl)

        End If

        If radProjectTree.SelectedNode.Value = "Procument" Then

            CookiesWrapper.RadTreeSelectedNode = "Procument"

            EnableControls(ucProcumentControl)

        End If

        If radProjectTree.SelectedNode.Value = "Evaluation" Then

            CookiesWrapper.RadTreeSelectedNode = "Evaluation"

            EnableControls(ucEvaluationControl)

        End If

        If radProjectTree.SelectedNode.Value = "Project Trips" Then

            CookiesWrapper.RadTreeSelectedNode = "Project Trips"

            EnableControls(ucTripDetailsControl)

            radFileListing.Visible = False

        End If

        If radProjectTree.SelectedNode.Value = "Organizations" Then

            Dim sql As String = "SELECT Name, O.Description, ContactName ,ContactNo FROM tblOrganization O inner join tblProjectObjects PO "
            sql &= " on O.OrganizationID = PO.ObjectID INNER JOIN luObjectTypes OT on OT.ObjectTypeID = PO.ObjectTypeID "
            sql &= " WHERE PO.ProjectID = " & txtProjectID.Text & " AND OT.Description = 'Organization'"

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "Schools" Then

            Dim sql As String = "SELECT Name, StaffCompliment, NoOfMaleStudents ,NoOfFemaleStudents FROM tblSchools S inner join tblProjectObjects PO "
            sql &= " on S.SchoolID = PO.ObjectID INNER JOIN luObjectTypes OT on OT.ObjectTypeID = PO.ObjectTypeID "
            sql &= " WHERE PO.ProjectID = " & txtProjectID.Text & " AND OT.Description = 'School'"

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "HealthCenters" Then

            Dim sql As String = "SELECT Name, H.Description, NoOfDoctors ,NoOfNurses, NoOfBeds FROM tblHealthCenters H inner join tblProjectObjects PO "
            sql &= " on H.HealthCenterID = PO.ObjectID INNER JOIN luObjectTypes OT on OT.ObjectTypeID = PO.ObjectTypeID "
            sql &= " WHERE PO.ProjectID = " & txtProjectID.Text & " AND OT.Description = 'HealthCenter'"

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "Groups" Then

            Dim sql As String = "SELECT GroupName, G.Description, GroupSize  FROM tblGroups G inner join tblProjectObjects PO "
            sql &= " on G.GroupID = PO.ObjectID INNER JOIN luObjectTypes OT on OT.ObjectTypeID = PO.ObjectTypeID "
            sql &= " WHERE PO.ProjectID = " & txtProjectID.Text & " AND OT.Description = 'Group'"

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "Community" Then

            Dim sql As String = "SELECT Name, C.Description, NoOfHouseholds ,NoOfIndividualAdultMales, NoOfIndividualAdultFemales, NoOfMaleYouths "
            sql &= " ,NoOfFemaleYouth, NoOfChildren FROM tblCommunities C inner join tblProjectObjects PO "
            sql &= " on C.CommunityID = PO.ObjectID INNER JOIN luObjectTypes OT on OT.ObjectTypeID = PO.ObjectTypeID "
            sql &= " WHERE PO.ProjectID = " & txtProjectID.Text & " AND OT.Description = 'Community'"

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "Households" Then

            Dim sql As String = "SELECT MemberNo, FirstName ,Surname, Sex, DateOfBirth, NationalID FROM tblBeneficiaries B inner join tblProjectObjects PO "
            sql &= " on B.BenficiaryID = PO.ObjectID INNER JOIN luObjectTypes OT on OT.ObjectTypeID = PO.ObjectTypeID "
            sql &= " WHERE PO.ProjectID = " & txtProjectID.Text & " AND OT.Description = 'Household' AND B.Suffx = 1"

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "Individuals" Then

            Dim sql As String = "SELECT MemberNo, FirstName ,Surname, Sex, DateOfBirth, NationalID FROM tblBeneficiaries B inner join tblProjectObjects PO "
            sql &= " on B.BenficiaryID = PO.ObjectID INNER JOIN luObjectTypes OT on OT.ObjectTypeID = PO.ObjectTypeID "
            sql &= " WHERE PO.ProjectID = " & txtProjectID.Text & " AND OT.Description = 'Individual'"

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "Objectives" Then

            Dim sql As String = "SELECT O.ObjectiveNo, O.Description FROM tblObjectives O inner join tblProjectObjectives PO on O.ObjectiveID = PO.ObjectiveID WHERE PO.ProjectID = " & txtProjectID.Text

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "Outcomes" Then

            Dim sql As String = "SELECT O.OutcomeID as OutcomeNo, O.Description FROM tblOutcomes O inner join tblProjectOutComes PO on O.OutcomeID = PO.OutcomeID WHERE PO.ProjectID = " & txtProjectID.Text

            LoadRadListing(sql)

        End If

        If radProjectTree.SelectedNode.Value = "Outputs" Then

            Dim sql As String = "SELECT O.OutputID as OutputNo, O.Description FROM tblOutput O inner join tblObjectiveOutputs OO on O.OutputID = OO.OutputID "
            sql &= " inner join tblProjectObjectives PO on PO.ObjectiveID = OO.ObjectiveID where PO.ProjectID   = " & txtProjectID.Text

            LoadRadListing(sql)

        End If

    End Sub

    Private Sub LoadRadListing(ByVal sql As String)

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        With radListing

            .DataSource = ds.Tables(0)
            .DataBind()
            .Rebind()

            ViewState("MyRadList") = .DataSource

        End With

        radListing.Visible = True

    End Sub

    Private Sub EnableControls(ByVal Control As UserControl)

        ucEvaluationControl.Visible = False
        ucProcumentControl.Visible = False
        ucProjectMeetingControl.Visible = False
        ucSchedulerControl.Visible = False
        ucTripDetailsControl.Visible = False

        If Not IsNothing(Control) Then

            Control.Visible = True

        End If

    End Sub

    Public Function CheckFileType(ByVal FileType As String) As Boolean

        Dim objFiles As New BusinessLogic.Files(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        dsFileTypes = objFiles.GetFileTypes()

        If dsFileTypes.Tables(0).Select("Description = '" & FileType & "'").Length > 0 Then

            Return True

        End If

        Return False

    End Function

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
            End If

        End If

        If e.CommandName = "View" Then

            Dim index1 As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item1 As GridDataItem = radFileListing.Items(index1)
            Dim FileID As Integer

            FileID = Server.HtmlDecode(item1("FileID").Text)

            Response.Redirect("~/Files.aspx?id=" & FileID)

        End If

    End Sub

    Private Sub radListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radListing.NeedDataSource

        radListing.DataSource = DirectCast(ViewState("MyRadList"), DataTable)

    End Sub
End Class