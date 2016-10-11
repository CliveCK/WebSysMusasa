Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class HouseholdIncomeSources
    Inherits System.Web.UI.UserControl

    Private db As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
    Private dsIncome As DataSet
    Private ReadOnly Property HouseholdID As Long
        Get
            Dim txtHouseholdID As TextBox = DirectCast(Parent.Parent.FindControl("ucBeneficiaryControl").FindControl("txtBeneficiaryID1"), TextBox)

            Return IIf(IsNumeric(txtHouseholdID.Text), txtHouseholdID.Text, 0)
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

            LoadGrid()

        End If

        dsIncome = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblIncomeSources I inner join tblHouseholdIncome HI on I.IncomeSourceID = HI.IncomeSourceID WHERE HouseholdID = " & CookiesWrapper.BeneficiaryID)

    End Sub

    Public Sub LoadGrid()

        ViewState("Income") = Nothing

        With radIncomeSource

            .DataSource = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblIncomeSources")
            .DataBind()

            ViewState("Income") = .DataSource

        End With

    End Sub

    Protected Function GetSelectedIncomeIDs() As String

        Dim IncomeIDArray As New List(Of String)

        For Each gridRow As Telerik.Web.UI.GridDataItem In radIncomeSource.SelectedItems
            IncomeIDArray.Add(gridRow.Item("IncomeSourceID").Text.ToString())
        Next

        Return String.Join(",", IncomeIDArray.ToArray())

    End Function

    Private Function MapIncome() As Boolean

        Dim Income() As String = GetSelectedIncomeIDs.Split(",")

        If Income.Length > 0 Then

            For i As Long = 0 To Income.Length - 1

                If dsIncome.Tables(0).Select("IncomeSourceID = " & Income(i)).Length > 0 Then

                    Return True

                ElseIf Not db.ExecuteNonQuery(CommandType.Text, "INSERT INTO tblHouseholdIncome (HouseholdID, IncomeSourceID) VALUES (" & CookiesWrapper.BeneficiaryID & "," & Income(i) & ")") > 0 Then

                    Return False

                End If

            Next

        End If

        Return True

    End Function

    Private Sub cmdMapIncome_Click(sender As Object, e As EventArgs) Handles cmdMapIncome.Click

        If CookiesWrapper.BeneficiaryID > 0 Then

            If MapIncome() Then

                ShowMessage("Saved successfully...", MessageTypeEnum.Information)

            End If

        Else

            ShowMessage("Please save Individual/Household details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radIncomeSource_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radIncomeSource.NeedDataSource

        radIncomeSource.DataSource = DirectCast(ViewState("Income"), DataTable)

    End Sub
End Class