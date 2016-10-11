Imports System.Linq

Partial Class ComplementaryListboxesVertical
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
            Return IIf(IsNothing(ViewState("Tag")), "", ViewState("Tag"))
        End Get
        Set(ByVal Value As String)
            ViewState("Tag") = Value
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

    Public ReadOnly Property SelectedValues() As String()
        Get
            If lstSelectedOptions.Items.Count > 0 Then

                Dim items As New ArrayList
                For i As Integer = 0 To lstSelectedOptions.Items.Count - 1

                    'only get values for items that are not marked as ignore
                    If lstSelectedOptions.Items(i).Value <> "" AndAlso (lstSelectedOptions.Items(i).Attributes("ignore") Is Nothing OrElse lstSelectedOptions.Items(i).Attributes("ignore") <> 1) Then

                        items.Add(lstSelectedOptions.Items(i).Value)

                    End If

                Next

                Return items.ToArray(GetType(System.String))

            Else
                Return New String() {""}
            End If
        End Get
    End Property

    Public ReadOnly Property AvailableValues() As String()
        Get
            If lstAvailableOptions.Items.Count > 0 Then

                Dim items As New ArrayList

                For i As Integer = 0 To lstAvailableOptions.Items.Count - 1

                    'only get values for items that are not marked as ignore
                    If lstAvailableOptions.Items(i).Value <> "" AndAlso (lstAvailableOptions.Items(i).Attributes("ignore") Is Nothing OrElse lstAvailableOptions.Items(i).Attributes("ignore") <> 1) Then

                        items.Add(lstAvailableOptions.Items(i).Value)

                    End If

                Next

                Return items.ToArray(GetType(System.String))

            Else
                Return New String() {""}
            End If
        End Get
    End Property

End Class

Public Class OptionGroup

    Public GroupName As String
    Public Items As ListItemCollection

    Public Sub New(ByVal GroupName As String, ByVal Items As ListItemCollection)

        Me.GroupName = GroupName
        Me.Items = Items

    End Sub

End Class

Public Class ListboxWithViewState
    Inherits System.Web.UI.WebControls.ListBox

    Private Function FindOptionGroup(ByVal GroupName As String) As ListItem

        For Each itm As ListItem In Me.Items

            If itm.Value = "$$GROUPHEADER$$GROUPHEADER$$" AndAlso itm.Text = GroupName Then

                Return itm

            End If

        Next

        Dim optGrpTitle As New ListItem(GroupName, "$$GROUPHEADER$$GROUPHEADER$$")
        Me.Items.Add(optGrpTitle)

        Return optGrpTitle

    End Function

    Function CloneListItem(ByVal itm As ListItem) As ListItem

        Dim clone As New ListItem(itm.Text, itm.Value)

        Dim keys As IEnumerator = itm.Attributes.Keys.GetEnumerator()
        Dim i As Integer = 1

        While keys.MoveNext()
            Dim key As String = CType(keys.Current, String)
            clone.Attributes.Add(key, itm.Attributes(key))
            i += 1
        End While

        Return clone

    End Function

    Public Sub AddOptionGroup(ByVal og As OptionGroup)

        Dim optGrpTitle As ListItem = FindOptionGroup(og.GroupName)

        Dim location As Long = Me.Items.IndexOf(optGrpTitle) + 1

        For Each li As ListItem In og.Items

            Me.Items.Insert(location, CloneListItem(li))

        Next

    End Sub

    Protected Overloads Overrides Sub RenderContents(ByVal writer As System.Web.UI.HtmlTextWriter)

        If Me.Items.Count > 0 Then
            Dim selected As Boolean = False
            Dim optGroupStarted As Boolean = False

            For i As Integer = 0 To Me.Items.Count - 1
                Dim item As ListItem = Me.Items(i)

                If item.Enabled Then

                    If item.Value = "$$GROUPHEADER$$GROUPHEADER$$" Then

                        If optGroupStarted Then
                            writer.WriteEndTag("optgroup")
                        End If

                        writer.WriteBeginTag("optgroup")
                        writer.WriteAttribute("label", item.Text)
                        writer.Write(">"c)
                        writer.WriteLine()
                        optGroupStarted = True

                    Else

                        writer.WriteBeginTag("option")

                        If item.Selected Then

                            If selected Then
                                Me.VerifyMultiSelect()
                            End If

                            selected = True
                            writer.WriteAttribute("selected", "selected")

                        End If

                        writer.WriteAttribute("value", item.Value, True)

                        item.Attributes.Render(writer)

                        If Me.Page IsNot Nothing Then
                            Me.Page.ClientScript.RegisterForEventValidation(Me.UniqueID, item.Value)
                        End If

                        writer.Write(">"c)
                        HttpUtility.HtmlEncode(item.Text, writer)
                        writer.WriteEndTag("option")
                        writer.WriteLine()

                    End If

                End If

            Next

            If optGroupStarted Then
                writer.WriteEndTag("optgroup")
            End If

        End If

    End Sub

    Protected Overloads Overrides Function SaveViewState() As Object
        ' Create an object array with one element for the CheckBoxList's
        ' ViewState contents, and one element for each ListItem in skmCheckBoxList
        Dim state As Object() = New Object(Me.Items.Count) {}

        Dim baseState As Object = MyBase.SaveViewState()
        state(0) = baseState

        ' Now, see if we even need to save the view state
        Dim itemHasAttributes As Boolean = False
        For i As Integer = 0 To Me.Items.Count - 1
            If Me.Items(i).Attributes.Count > 0 Then
                itemHasAttributes = True

                ' Create an array of the item's Attribute's keys and values
                Dim attribKV As Object() = New Object((Me.Items(i).Attributes.Count * 2) - 1) {}
                Dim k As Integer = 0
                For Each key As String In Me.Items(i).Attributes.Keys
                    attribKV(k) = key
                    k += 1
                    attribKV(k) = Me.Items(i).Attributes(key)
                    k += 1
                Next

                state(i + 1) = attribKV
            End If
        Next

        ' return either baseState or state, depending on whether or not
        ' any ListItems had attributes
        If itemHasAttributes Then
            Return state
        Else
            Return baseState
        End If
    End Function

    Protected Overloads Overrides Sub LoadViewState(ByVal savedState As Object)
        If savedState Is Nothing Then
            Exit Sub
        End If

        ' see if savedState is an object or object array
        If TypeOf savedState Is Object() Then
            ' we have an array of items with attributes
            Dim state As Object() = DirectCast(savedState, Object())
            MyBase.LoadViewState(state(0))
            ' load the base state
            For i As Integer = 1 To state.Length - 1
                If Not state(i) Is Nothing Then
                    ' Load back in the attributes
                    Dim attribKV As Object() = DirectCast(state(i), Object())
                    For k As Integer = 0 To attribKV.Length - 1 Step 2
                        Me.Items(i - 1).Attributes.Add(attribKV(k).ToString(), attribKV(k + 1).ToString())
                    Next
                End If
            Next
        Else
            ' we have just the base state
            MyBase.LoadViewState(savedState)
        End If
    End Sub

End Class

Public Class DropDownWithViewState
    Inherits System.Web.UI.WebControls.DropDownList

    Protected Overloads Overrides Function SaveViewState() As Object
        ' Create an object array with one element for the CheckBoxList's
        ' ViewState contents, and one element for each ListItem in skmCheckBoxList
        Dim state As Object() = New Object(Me.Items.Count) {}

        Dim baseState As Object = MyBase.SaveViewState()
        state(0) = baseState

        ' Now, see if we even need to save the view state
        Dim itemHasAttributes As Boolean = False
        For i As Integer = 0 To Me.Items.Count - 1
            If Me.Items(i).Attributes.Count > 0 Then
                itemHasAttributes = True

                ' Create an array of the item's Attribute's keys and values
                Dim attribKV As Object() = New Object((Me.Items(i).Attributes.Count * 2) - 1) {}
                Dim k As Integer = 0
                For Each key As String In Me.Items(i).Attributes.Keys
                    attribKV(k) = key
                    k += 1
                    attribKV(k) = Me.Items(i).Attributes(key)
                    k += 1
                Next

                state(i + 1) = attribKV
            End If
        Next

        ' return either baseState or state, depending on whether or not
        ' any ListItems had attributes
        If itemHasAttributes Then
            Return state
        Else
            Return baseState
        End If
    End Function

    Protected Overloads Overrides Sub LoadViewState(ByVal savedState As Object)
        If savedState Is Nothing Then
            Exit Sub
        End If

        ' see if savedState is an object or object array
        If TypeOf savedState Is Object() Then
            ' we have an array of items with attributes
            Dim state As Object() = DirectCast(savedState, Object())
            MyBase.LoadViewState(state(0))
            ' load the base state
            For i As Integer = 1 To state.Length - 1
                If Not state(i) Is Nothing Then
                    ' Load back in the attributes
                    Dim attribKV As Object() = DirectCast(state(i), Object())
                    For k As Integer = 0 To attribKV.Length - 1 Step 2
                        Me.Items(i - 1).Attributes.Add(attribKV(k).ToString(), attribKV(k + 1).ToString())
                    Next
                End If
            Next
        Else
            ' we have just the base state
            MyBase.LoadViewState(savedState)
        End If
    End Sub

End Class

Public Class GroupedDropDownList
    Inherits DropDownList
    Public Property DataGroupField() As [String]
        Get
            Return m_DataGroupField
        End Get
        Set(value As [String])
            m_DataGroupField = Value
        End Set
    End Property
    Private m_DataGroupField As [String]

    Protected Overrides Sub PerformDataBinding(dataSource As IEnumerable)
        MyBase.PerformDataBinding(dataSource)

        If ([String].IsNullOrWhiteSpace(Me.DataGroupField) = False) AndAlso (dataSource IsNot Nothing) Then
            Dim items As ListItemCollection = Me.Items
            Dim data As IEnumerable(Of [Object]) = dataSource.OfType(Of [Object])()
            Dim count As Int32 = data.Count()

            For i As Int32 = 0 To count - 1
                Dim group As [String] = If(TryCast(DataBinder.Eval(data.ElementAt(i), Me.DataGroupField), [String]), [String].Empty)

                If [String].IsNullOrWhiteSpace(group) = False Then
                    items(i).Attributes("Group") = group
                End If
            Next
        End If
    End Sub

    Protected Overrides Sub RenderContents(writer As HtmlTextWriter)
        Dim items As ListItemCollection = Me.Items
        Dim count As Int32 = items.Count
        Dim groupedItems = items.OfType(Of ListItem)().GroupBy(Function(x) If(x.Attributes("Group"), [String].Empty)).[Select](Function(x) New With { _
            Key .Group = x.Key, _
            Key .Items = x.ToList() _
        })

        If count > 0 Then
            Dim flag As [Boolean] = False

            For Each groupedItem As Object In groupedItems
                If [String].IsNullOrWhiteSpace(groupedItem.Group) = False Then
                    writer.WriteBeginTag("optgroup")
                    writer.WriteAttribute("label", groupedItem.Group)
                    writer.Write(">"c)
                End If

                For i As Int32 = 0 To groupedItem.Items.Count - 1
                    Dim item As ListItem = groupedItem.Items(i)

                    If item.Enabled = True Then
                        writer.WriteBeginTag("option")

                        If item.Selected = True Then
                            If flag = True Then
                                Me.VerifyMultiSelect()
                            End If

                            flag = True

                            writer.WriteAttribute("selected", "selected")
                        End If

                        writer.WriteAttribute("value", item.Value, True)

                        If item.Attributes.Count <> 0 Then
                            item.Attributes.Render(writer)
                        End If

                        If Me.Page IsNot Nothing Then
                            Me.Page.ClientScript.RegisterForEventValidation(Me.UniqueID, item.Value)
                        End If

                        writer.Write(">"c)
                        HttpUtility.HtmlEncode(item.Text, writer)
                        writer.WriteEndTag("option")
                        writer.WriteLine()
                    End If
                Next

                If [String].IsNullOrWhiteSpace(groupedItem.Group) = False Then
                    writer.WriteEndTag("optgroup")
                End If
            Next
        End If
    End Sub
End Class