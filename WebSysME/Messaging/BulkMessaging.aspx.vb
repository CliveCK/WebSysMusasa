Imports Universal.CommonFunctions

Public Class BulkMessaging
    Inherits System.Web.UI.Page

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

    End Sub

    Sub LoadAvailableObjects(ByVal sql As String)

        If Not sql = "" Then

            Dim objLookup As New BusinessLogic.CommonFunctions

            Dim ds As DataSet = objLookup.SqlExec(sql)

            ucRecipients.AvailableOptions.DataSource = ds

            ucRecipients.AvailableOptions.DataTextField = "Name"

            ucRecipients.AvailableOptions.DataValueField = "RecipientID"

            ucRecipients.AvailableOptions.DataBind()

        End If

    End Sub

    Sub LoadAssignedObjects()

        Dim lkUp As New BusinessLogic.CommonFunctions
        Dim dt As New DataTable

        dt.Columns.Add(New DataColumn("Name"))
        dt.Columns.Add(New DataColumn("RecipientID"))

        ucRecipients.SelectedOptions.DataSource = dt

        ucRecipients.SelectedOptions.DataTextField = "Name"

        ucRecipients.SelectedOptions.DataValueField = "ObjectID"

        ucRecipients.SelectedOptions.DataBind()

    End Sub

    Public Function DertemineObjectTypeSQL(ByVal Type As String) As String

        Dim sql As String = ""

        Select Case Type

            Case "Staff"
                sql = "SELECT StaffID As RecipientID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblStaffMembers"

            Case "User"
                sql = "SELECT UserID As RecipientID, ISNULL(UserFirstName, '') + ' ' + ISNULL(UserSurname, '') As Name FROM tblUsers WHERE Deleted = 0"

            Case "Beneficiary"
                sql = "SELECT BeneficiaryID as RecipientID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name FROM tblBeneficiaries WHERE Suffix = 1"

        End Select

        Return sql

    End Function

    Private Sub cboRecipientsType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRecipientsType.SelectedIndexChanged

        LoadAvailableObjects(DertemineObjectTypeSQL(cboRecipientsType.SelectedValue))
        LoadAssignedObjects()

    End Sub

    Private Sub btnCampaign_Click(sender As Object, e As EventArgs) Handles btnCampaign.Click

        If ucRecipients.SelectedOptions.Items.Count > 0 Then

            Dim objComms As New PrimaSysMessaging.BusinessLogic.MessageHandler

            Select Case cboMessageType.SelectedValue

                Case "EMail"

                    Dim Email As New List(Of String)

                    For i As Integer = 0 To ucRecipients.SelectedOptions.Items.Count - 1

                        ucRecipients.SelectedOptions.SelectedIndex = i

                        Email.Add(GetEmail(cboRecipientsType.SelectedItem.Text, ucRecipients.SelectedOptions.SelectedValue))

                    Next

                    If objComms.SendEmail_SMS(New String() {}, Email.ToArray, New String() {}, txtMessage.Text, txtSubject.Text, 1) Then

                        ShowMessage("Message has been queued successfully...", MessageTypeEnum.Information)

                    Else

                        ShowMessage("Message queueing failed!", MessageTypeEnum.Error)

                    End If

        Case "SMS"

                    Dim Cell As New List(Of String)

                    For i As Integer = 0 To ucRecipients.SelectedOptions.Items.Count - 1

                        ucRecipients.SelectedOptions.SelectedIndex = i

                        Cell.Add(GetSMS(cboRecipientsType.SelectedItem.Text, ucRecipients.SelectedOptions.SelectedValue))

                    Next

                    If objComms.SendEmail_SMS(New String() {}, New String() {}, Cell.ToArray, txtMessage.Text, txtSubject.Text, 2) Then

                        ShowMessage("Message has been queued successfully...", MessageTypeEnum.Information)

                    Else

                        ShowMessage("Message queueing failed!", MessageTypeEnum.Error)

                    End If

                Case "Both"

                    Dim Email As New List(Of String)
                    Dim Cell As New List(Of String)

                    For i As Integer = 0 To ucRecipients.SelectedOptions.Items.Count - 1

                        ucRecipients.SelectedOptions.SelectedIndex = i

                        Email.Add(GetEmail(cboRecipientsType.SelectedItem.Text, ucRecipients.SelectedOptions.SelectedValue))

                    Next

                    For i As Integer = 0 To ucRecipients.SelectedOptions.Items.Count - 1

                        ucRecipients.SelectedOptions.SelectedIndex = i

                        Cell.Add(GetSMS(cboRecipientsType.SelectedItem.Text, ucRecipients.SelectedOptions.SelectedValue))

                    Next

                    If objComms.SendEmail_SMS(New String() {}, Email.ToArray, Cell.ToArray, txtMessage.Text, txtSubject.Text, 3) Then

                        ShowMessage("Message has been queued successfully...", MessageTypeEnum.Information)

                    Else

                        ShowMessage("Message queueing failed!", MessageTypeEnum.Error)

                    End If

            End Select


        End If

    End Sub

    Private Function GetEmail(ByVal Type As String, ByVal RecipientID As Long) As String

        Dim ds As DataSet
        Dim objStaff As New BusinessLogic.StaffMember(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Select Case Type
            Case "StaffMembers"
                ds = objStaff.GetStaffMember("SELECT EmailAddress FROM tblStaffMembers WHERE StaffID = " & RecipientID)

            Case "System Users"
                ds = objStaff.GetStaffMember("SELECT EmailAddress FROM tblUsers WHERE UserID = " & RecipientID)

        End Select

        If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

            Return Catchnull(ds.Tables(0).Rows(0)(0), "")

        End If

        Return ""

    End Function

    Private Function GetSMS(ByVal Type As String, ByVal RecipientID As Long) As String

        Dim ds As DataSet
        Dim objStaff As New BusinessLogic.StaffMember(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Select Case Type
            Case "StaffMembers"
                ds = objStaff.GetStaffMember("SELECT ContactNo FROM tblStaffMembers WHERE StaffID = " & RecipientID)

            Case "System Users"
                ds = objStaff.GetStaffMember("SELECT MobileNo FROM tblUsers WHERE UserID = " & RecipientID)

        End Select

        If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

            Return Catchnull(ds.Tables(0).Rows(0)(0), "")

        End If

        Return ""

    End Function
End Class