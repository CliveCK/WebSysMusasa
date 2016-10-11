Partial Class ComplementaryListboxes
    Inherits System.Web.UI.UserControl

    Public Event Selected(ByVal SelectedItems As String)
    Public Event Removed(ByVal SelectedItems As String)

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Public Sub Clear()

        lstAvailableOptions.ClearSelection()
        lstAvailableOptions.Items.Clear()

        lstSelectedOptions.ClearSelection()
        lstSelectedOptions.Items.Clear()

    End Sub

    Public Property Tag() As String
        Get
            Return IIf(IsNothing(viewstate("Tag")), "", viewstate("Tag"))
        End Get
        Set(ByVal Value As String)
            viewstate("Tag") = Value
        End Set
    End Property

    Public ReadOnly Property SelectedOptions() As ListBox
        Get
            Return lstSelectedOptions
        End Get
    End Property

    Public ReadOnly Property AvailableOptions() As ListBox
        Get
            Return lstAvailableOptions
        End Get
    End Property

    Public Property SelectedOptionsCaption() As String
        Get
            Return lblSelectedOptions.Text
        End Get
        Set(ByVal Value As String)
            lblSelectedOptions.Text = Value
        End Set
    End Property

    Public Property AvailableOptionsCaption() As String
        Get
            Return lblAvailableOptions.Text
        End Get
        Set(ByVal Value As String)
            lblAvailableOptions.Text = Value
        End Set
    End Property

    Private Sub cmdMoveSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMoveSelected.Click
        Dim item As ListItem
        Dim items As String = ""
        Dim list As New ListItemCollection

        For Each item In lstAvailableOptions.Items
            If item.Selected AndAlso (item.Attributes("fixed") Is Nothing OrElse item.Attributes("fixed") <> 1) Then
                items &= IIf(items = "", "", ",") & item.Value
                lstSelectedOptions.Items.Add(item)
                list.Add(item)
            End If
        Next

        For Each item In list
            lstAvailableOptions.Items.Remove(item)
        Next

        RaiseEvent Selected(items)

    End Sub

    Private Sub cmdRemoveSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveSelected.Click
        Dim item As ListItem
        Dim items As String = ""
        Dim list As New ListItemCollection

        For Each item In lstSelectedOptions.Items
            If item.Selected AndAlso (item.Attributes("fixed") Is Nothing OrElse item.Attributes("fixed") <> 1) Then
                items &= IIf(items = "", "", ",") & item.Value
                lstAvailableOptions.Items.Add(item)
                list.Add(item)
            End If
        Next

        For Each item In list
            lstSelectedOptions.Items.Remove(item)
        Next

        RaiseEvent Removed(items)

    End Sub

    Private Sub cmdMoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMoveAll.Click
        Dim item As ListItem
        Dim items As String = ""
        Dim list As New ListItemCollection

        For Each item In lstAvailableOptions.Items
            If (item.Attributes("fixed") Is Nothing OrElse item.Attributes("fixed") <> 1) Then
                items &= IIf(items = "", "", ",") & item.Value
                lstSelectedOptions.Items.Add(item)
                list.Add(item)
            End If
        Next

        For Each item In list
            lstAvailableOptions.Items.Remove(item)
        Next

        RaiseEvent Selected(items)

    End Sub

    Private Sub cmdRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveAll.Click
        Dim item As ListItem
        Dim items As String = ""
        Dim list As New ListItemCollection

        For Each item In lstSelectedOptions.Items
            If (item.Attributes("fixed") Is Nothing OrElse item.Attributes("fixed") <> 1) Then
                items &= IIf(items = "", "", ",") & item.Value
                lstAvailableOptions.Items.Add(item)
                list.Add(item)
            End If
        Next

        For Each item In list
            lstSelectedOptions.Items.Remove(item)
        Next

        RaiseEvent Removed(items)

    End Sub

    Public ReadOnly Property SelectedTexts(Optional ByVal Quote As String = "") As String()
        Get
            If lstSelectedOptions.Items.Count > 0 Then

                Dim items As New ArrayList
                For i As Integer = 0 To lstSelectedOptions.Items.Count - 1

                    'only get values for items that are not marked as ignore
                    If lstSelectedOptions.Items(i).Text <> "" AndAlso (lstSelectedOptions.Items(i).Attributes("ignore") Is Nothing OrElse lstSelectedOptions.Items(i).Attributes("ignore") <> 1) Then

                        items.Add(Quote & lstSelectedOptions.Items(i).Text & Quote)

                    End If

                Next

                Return items.ToArray(GetType(System.String))

            Else
                Return New String() {""}
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedValues(Optional ByVal Quote As String = "") As String()
        Get
            If lstSelectedOptions.Items.Count > 0 Then

                Dim items As New ArrayList
                For i As Integer = 0 To lstSelectedOptions.Items.Count - 1

                    'only get values for items that are not marked as ignore
                    If lstSelectedOptions.Items(i).Value <> "" AndAlso (lstSelectedOptions.Items(i).Attributes("ignore") Is Nothing OrElse lstSelectedOptions.Items(i).Attributes("ignore") <> 1) Then

                        items.Add(Quote & lstSelectedOptions.Items(i).Value & Quote)

                    End If

                Next

                Return items.ToArray(GetType(System.String))

            Else
                Return New String() {""}
            End If
        End Get
    End Property

    Public ReadOnly Property AvailableValues(Optional ByVal Quote As String = "") As String()
        Get
            If lstAvailableOptions.Items.Count > 0 Then

                Dim items As New ArrayList
                For i As Integer = 0 To lstAvailableOptions.Items.Count - 1

                    'only get values for items that are not marked as ignore
                    If lstAvailableOptions.Items(i).Value <> "" AndAlso (lstAvailableOptions.Items(i).Attributes("ignore") Is Nothing OrElse lstAvailableOptions.Items(i).Attributes("ignore") <> 1) Then

                        items.Add(Quote & lstAvailableOptions.Items(i).Value & Quote)

                    End If

                Next

                Return items.ToArray(GetType(System.String))

            Else
                Return New String() {""}
            End If
        End Get
    End Property

End Class
