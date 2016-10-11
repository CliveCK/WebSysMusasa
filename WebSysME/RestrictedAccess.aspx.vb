Imports SecurityPolicy

Public Class RestrictedAccess
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objUser As New UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim userGroups As DataSet
            Dim thisUserGroups As String = ""

            With objUser

                If objUser.Retrieve(CookiesWrapper.thisUserID) Then

                    lblUserName.Text = .Username
                    lblUserFullName.Text = CookiesWrapper.thisUserFullName
                    userGroups = .GetUserUsergroupsDescriptions(CookiesWrapper.thisUserID)

                    For Each row As DataRow In userGroups.Tables(0).Rows

                        thisUserGroups &= IIf(String.IsNullOrWhiteSpace(thisUserGroups), "", ", ") & row("Description")

                    Next

                    lblUserGroup.Text = thisUserGroups

                End If

            End With

        End If

    End Sub

End Class