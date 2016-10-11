Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            lblUser.Text = CookiesWrapper.thisUserFullName
            lblDayDesc.Text = Now.ToString("dddd")
            lblDay.Text = Now.ToString("dd")
            lblMonth.Text = Now.ToString("MMMM")
            lblYear.Text = Now.ToString("yyyy")

            Dim objNotifications As New PrimaSysMessaging.BusinessLogic.MessageHandler()
            Dim count As Long = objNotifications.GetMessageCount(CookiesWrapper.thisUserID, 1)

            hypNotifications.Text = count & " New" 'Get New Messages count
            hypNotifications.ForeColor = IIf(count = 0, Drawing.Color.Red, Drawing.Color.Green)
            hypNotifications.NavigateUrl = "~/Messaging/InAppMail.aspx"

            LoadCompanyLogo()

        End If

    End Sub

    Private Sub LoadCompanyLogo()

        imgCompanyLogo.ImageUrl = "~/Settings/" & CookiesWrapper.thisConnectionName & "/Images/CompanyLogo.jpg"

    End Sub

End Class