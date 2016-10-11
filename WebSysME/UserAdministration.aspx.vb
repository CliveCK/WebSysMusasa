Public Class UserAdministration

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then


        End If


        If Request.QueryString("op") = "eu" Then

            'we are editing details
            CreateNewUserControl1.Visible = True
            FindUsers1.Visible = False

        ElseIf Request.QueryString("op") = "vw" Then

            CreateNewUserControl1.Visible = False
            FindUsers1.Visible = True

        Else

            CreateNewUserControl1.Visible = True
            FindUsers1.Visible = False

        End If

    End Sub

End Class