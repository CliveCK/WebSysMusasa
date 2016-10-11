Partial Public Class AutoComplete
    Inherits System.Web.UI.UserControl

    Public Property Value() As Long
        Get
            Return IIf(IsNumeric(txtValue.Text), txtValue.Text, 0)
        End Get
        Set(ByVal value As Long)
            txtValue.Text = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return txtAuto.Value
        End Get
        Set(ByVal value As String)
            txtAuto.Value = value
        End Set
    End Property


    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If Not Page.IsPostBack Then

        '    txtAuto.Attributes.Add("Purpose", "acomplete")
        '    txtValue.Attributes.Add("Purpose", "acompletevalue")

        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'UserInterfaceHelper.CSSHelper.AddCSSToPage(Me.Page, "Frameworks/jQuery/jquery.autocomplete.css")

        If Not Page.IsPostBack Then

            txtAuto.Attributes.Add("Purpose", "acomplete")
            txtValue.Attributes.Add("Purpose", "acompletevalue")

        End If

        'Page.ClientScript.RegisterClientScriptInclude("jQuery", "Frameworks/jQuery/jquery-1.2.2.pack.js")
        ' Page.ClientScript.RegisterClientScriptInclude("jQueryAuto", "Frameworks/jQuery/jquery.autocomplete.js")

        ' Page.ClientScript.RegisterClientScriptInclude("jQuery", "Frameworks/jQuery/jquery-1.2.2.pack.js")
        ' Page.ClientScript.RegisterClientScriptInclude("mmsCommon", "../Scripts/Common.js")

    End Sub

End Class