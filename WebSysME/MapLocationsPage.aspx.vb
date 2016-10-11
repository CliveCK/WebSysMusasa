Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic
Imports Telerik.Web.UI

Public Class MapLocationsPage
    Inherits System.Web.UI.Page

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

            InitializeObjects()

        End If

    End Sub

    Private Sub LoadGrid()

        Dim sql As String = DertemineObjectTypeSQL(cboObjectType.SelectedItem.Text)
        ViewState("iObjects") = Nothing

        If sql <> "" AndAlso sql <> "Invalid" Then

            If cboObjectType.SelectedIndex > 0 Then

                Dim sql1 As String = "SELECT ObjectID FROM tblLocationObjects O inner join luObjectTypes OT "
                sql1 &= " on OT.ObjectTypeID = O.ObjectTypeID where OT.ObjectTypeID = " & cboObjectType.SelectedValue
                sql1 &= Criteria()

                dsDocuments = db.ExecuteDataSet(CommandType.Text, sql1)

            End If

            With radObjects

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()

                ViewState("iObjects") = .DataSource

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

            Case "Survey"
                sql = "SELECT SurveyID as ObjectID, Name, Description FROM tblOutcomes"

            Case "Evaluation"
                sql = "SELECT OutcomeID as ObjectID, Name, Description FROM tblOutcomes"

            Case Else
                sql = "Invalid"

        End Select

        Return sql

    End Function

    Protected Sub InitializeObjects()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboObjectType

            .DataSource = objLookup.Lookup("luObjectTypes", "ObjectTypeID", "Description").Tables(0)
            .DataValueField = "ObjectTypeID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboCountry

            .DataSource = objLookup.Lookup("luCountries", "CountryID", "CountryName").Tables(0)
            .DataValueField = "CountryID"
            .DataTextField = "CountryName"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

    End Sub

    Protected Function GetSelectedObjectIDs() As String

        Dim ObjectIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radObjects.SelectedItems
            ObjectIDArray.Add(gridRow.Item("ObjectID").Text.ToString())
        Next

        Return String.Join(",", ObjectIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim mObject() As String = GetSelectedObjectIDs.Split(",")

        Try

            If mObject.Length > 0 Then

                For i As Long = 0 To mObject.Length - 1

                    Dim objObjects As New BusinessLogic.LocationObjects(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objObjects

                        .ObjectTypeID = cboObjectType.SelectedValue
                        .ObjectID = mObject(i)

                        Select Case rbLstSaveOption.SelectedItem.Text

                            Case "Country"
                                .CountryID = cboCountry.SelectedValue

                            Case "City"
                                .CityID = cboCity.SelectedValue

                            Case "Section"
                                .SectionID = cboSection.SelectedValue

                            Case "Street"
                                .StreetID = cboStreet.SelectedValue

                            Case "Province"
                                .ProvinceID = cboProvince.SelectedValue

                            Case "Suburb"
                                .SurburbID = cboSuburb.SelectedValue

                            Case "Ward"
                                .WardID = cboWard.SelectedValue

                            Case Else

                                ShowMessage("Invalid Paramaters...", MessageTypeEnum.Error)

                        End Select

                        If Not .Save Then

                            log.Error("Error saving...")
                            Return False

                        End If

                    End With

                Next

            End If

            Return True

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            log.Error(ex)
            Return False

        End Try


    End Function

    Private Sub cboCountry_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCountry.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboCountry.SelectedIndex > 0 Then

            With cboCity

                .DataSource = objLookup.Lookup("tblCities", "CityID", "Name", , "CountryID = " & cboCountry.SelectedValue).Tables(0)
                .DataValueField = "CityID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

        If cboCountry.SelectedIndex > 0 Then

            With cboProvince

                .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name", , "CountryID = " & cboCountry.SelectedValue).Tables(0)
                .DataValueField = "ProvinceID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

        LoadGrid()

    End Sub

    Private Sub cboCity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCity.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboCity.SelectedIndex > 0 Then

            With cboSuburb

                .DataSource = objLookup.Lookup("tblSuburbs", "SuburbID", "Name", , "CityID = " & cboCity.SelectedValue).Tables(0)
                .DataValueField = "SuburbID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If


    End Sub

    Private Sub cboSuburb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSuburb.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboSuburb.SelectedIndex > 0 Then

            With cboSection

                .DataSource = objLookup.Lookup("tblSection", "SectionID", "Name", , "SuburbID = " & cboSuburb.SelectedValue).Tables(0)
                .DataValueField = "SectionID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If


    End Sub

    Private Sub cboSection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSection.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboSection.SelectedIndex > 0 Then

            With cboSection

                .DataSource = objLookup.Lookup("tblStreets", "StreetID", "Name", , "SectionID = " & cboSection.SelectedValue).Tables(0)
                .DataValueField = "StreetID"
                .DataTextField = "Name"
                .DataBind()

            End With

        End If

    End Sub

    Private Sub cboProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProvince.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProvince.SelectedIndex > 0 Then

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name", , "ProvinceID = " & cboProvince.SelectedValue).Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDistrict.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboDistrict.SelectedIndex > 0 Then

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name", , "DistrictID = " & cboDistrict.SelectedValue).Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            LoadGrid()

        End If

    End Sub

    Private Sub cboObjectType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboObjectType.SelectedIndexChanged

        LoadGrid()

    End Sub

    Private Sub radObjects_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radObjects.ItemCommand

        

    End Sub

    Private Sub radObjects_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radObjects.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim gridItem As GridDataItem = e.Item

            Dim btnImage As ImageButton = DirectCast(gridItem.FindControl("imgEdit"), ImageButton)

            If dsDocuments.Tables(0).Select("ObjectID = " & gridItem("ObjectID").Text).Length > 0 Then

                Dim chkbx As CheckBox = DirectCast(gridItem("chkRowSelect").Controls(0), CheckBox)

                chkbx.Enabled = False
                chkbx.ToolTip = "Already mapped..."
                btnImage.Visible = True

            End If

        End If

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Map() Then

            LoadGrid()
            ShowMessage("Mapped successfully...", MessageTypeEnum.Information)

        Else

            ShowMessage("Error saving ...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radObjects_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radObjects.NeedDataSource

        If cboObjectType.SelectedIndex > 0 Then

            Dim sql As String = "SELECT ObjectID FROM tblLocationObjects O inner join luObjectTypes OT "
            sql &= " on OT.ObjectTypeID = O.ObjectTypeID where OT.ObjectTypeID = " & cboObjectType.SelectedValue
            sql &= Criteria()

            dsDocuments = db.ExecuteDataSet(CommandType.Text, sql)

        End If

        radObjects.DataSource = DirectCast(ViewState("iObjects"), DataTable)

    End Sub

    Private Function Criteria() As String

        Dim Crit As String = ""

        Select Case rbLstSaveOption.SelectedItem.Text
            Case "Country"
                Crit = " AND CountryID = " & cboCountry.SelectedValue
            Case "Province"
                Crit = " AND ProvinceID = " & cboProvince.SelectedValue
            Case "District"
                Crit = " AND DistrictID = " & cboDistrict.SelectedValue
            Case "Ward"
                Crit = " AND WardID = " & cboWard.SelectedValue
            Case "City"
                Crit = " AND CityID = " & cboCity.SelectedValue
            Case "Suburb"
                Crit = " AND SurburbID = " & cboSuburb.SelectedValue
            Case "Section"
                Crit = " AND SectionID = " & cboSection.SelectedValue
            Case "Street"
                Crit = " AND StreetID = " & cboStreet.SelectedValue

        End Select

        Return Crit

    End Function

    Private Sub rbLstSaveOption_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbLstSaveOption.SelectedIndexChanged

        LoadGrid()

    End Sub

    Private Sub cboWard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboWard.SelectedIndexChanged

        LoadGrid()

    End Sub
End Class