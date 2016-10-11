Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports BusinessLogic

Partial Class BeneficiaryInterventionDetailsControl
    Inherits System.Web.UI.UserControl

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboInterventions

                .DataSource = objLookup.Lookup("tblInterventions", "InterventionID", "Name").Tables(0)
                .DataValueField = "InterventionID"
                .DataTextField = "Name"
                .DataBind()

            End With

            With cboBeneficiaryType

                .DataSource = objLookup.Lookup("luBeneficiaryTypes", "BeneficiaryTypeID", "Description").Tables(0)
                .DataValueField = "BeneficiaryTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            LoadGrid()

        End If

    End Sub

    Public Function DertemineObjectTypeSQL(ByVal Type As String) As String

        Dim sql As String = ""

        Select Case Type

            Case "Community"
                sql = "SELECT CommunityID as Object, Name, Description FROM tblCommunities"

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

    Private Sub LoadGrid()

        Dim sql As String = DertemineObjectTypeSQL(cboBeneficiaryType.SelectedItem.Text)
        ViewState("Objects") = Nothing

        If sql <> "" Then

            With radObjects

                .DataSource = db.ExecuteDataSet(CommandType.Text, sql).Tables(0)
                .DataBind()
                .Rebind()

                ViewState("Objects") = .DataSource

            End With

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Map()

    End Sub

    Protected Function GetSelectedObjectIDs() As String

        Dim ObjectIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radObjects.SelectedItems
            ObjectIDArray.Add(gridRow.Item("OutputID").Text.ToString())
        Next

        Return String.Join(",", ObjectIDArray.ToArray())

    End Function

    Private Function Map() As Boolean

        Dim mObject() As String = GetSelectedObjectIDs.Split(",")

        If mObject.Length > 0 Then

            For i As Long = 0 To mObject.Length - 1

                Dim objObjects As New BusinessLogic.BeneficiaryIntervention(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objObjects

                    .BeneficiaryTypeID = cboBeneficiaryType.SelectedValue
                    .BeneficiaryID = mObject(i)

                    If cboInterventions.Entries.Count > 0 Then

                        .InterventionID = cboInterventions.Entries.Item(0).Value

                    Else

                        Return False

                    End If

                    If Not .Save Then

                        log.Error("Error saving...")
                        Return False

                    End If

                End With

            Next

        End If

        Return True

    End Function

    '   public Function LoadBeneficiaryIntervention(ByVal BeneficiaryInterventionID As Long) As Boolean 

    '       Try 

    '	Dim objBeneficiaryIntervention As New BeneficiaryIntervention(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID) 

    '	With objBeneficiaryIntervention 

    '	    If .Retrieve(BeneficiaryInterventionID) Then 

    '                   txtBeneficiaryInterventionID.Text = .BeneficiaryInterventionID
    '		If Not IsNothing(cboBeneficiary.Items.FindByValue(.BeneficiaryID)) Then cboBeneficiary.SelectedValue = .BeneficiaryID 
    '		If Not IsNothing(cboIntervention.Items.FindByValue(.InterventionID)) Then cboIntervention.SelectedValue = .InterventionID 

    '				ShowMessage("BeneficiaryIntervention loaded successfully...", MessageTypeEnum.Information) 
    '			    Return true 

    '	    Else 

    '				ShowMessage("Failed to loadBeneficiaryIntervention: & .ErrorMessage", MessageTypeEnum.Error) 
    '	    		Return False 

    '	    End If 

    '	End With 

    'Catch ex As Exception 

    '	ShowMessage(ex, MessageTypeEnum.Error) 
    '	Return False 

    'End Try 

    '   End Function 

    Public Sub Clear()

        txtBeneficiaryInterventionID.Text = ""

    End Sub

    Private Sub cboBeneficiaryType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBeneficiaryType.SelectedIndexChanged

        LoadGrid()

    End Sub
End Class

