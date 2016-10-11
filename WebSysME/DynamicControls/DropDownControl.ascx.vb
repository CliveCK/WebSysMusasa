Imports Telerik.Web.UI
Enum ItemsOptionListTypes As Long
    Checklist = 1
    [Default] = 2
    None = 3
    OptionList = 4
End Enum

Partial Public Class DropDownControl
    Inherits System.Web.UI.UserControl

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private m_delimiter As String = ","

    Public Property Delimiter() As String
        Get
            Return m_delimiter
        End Get
        Set(ByVal value As String)
            m_delimiter = value
        End Set
    End Property

    Public ReadOnly Property GetSelectedValue(ByVal UseCheckBoxes As Boolean) As String
        Get
            Return GetSelectedIds(UseCheckBoxes)
        End Get
    End Property

    Public ReadOnly Property GetSelectedSingleSelectValues() As String
        Get
            Return GetSelectedSingleTreeValues()
        End Get
    End Property

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        ' Create the javascript functions with the client id appended to them.

    End Sub

    Public Property Enabled() As Boolean

        Get
            Return rcbTreeview.Enabled
        End Get
        Set(value As Boolean)
            rcbTreeview.Enabled = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Create the javascript functions.
        If Not Me.IsPostBack Then
            ' Add the events
        End If
    End Sub

    Public Sub PopulateTree(ByVal dsRecords As DataSet, ByVal DataFieldID As String, ByVal DataFieldParentID As String, ByVal DataTextField As String, ByVal DataValueField As String, Optional ByVal UseCheckBoxes As Boolean = True, Optional ByVal Multipleselect As Boolean = False, Optional ByVal TriStateCheckBoxes As Boolean = False, Optional ByVal requireAtLeastOnNode As Boolean = True, Optional ByVal ExpandAllNodes As Boolean = True)
        Try

            Dim RequiredFieldValidator1 As RequiredFieldValidator = TryCast(rcbTreeview.Items(0).FindControl("RequiredFieldValidator1"), RequiredFieldValidator)

            RequiredFieldValidator1.Enabled = False

            If Not IsNothing(dsRecords) AndAlso dsRecords.Tables(0) IsNot Nothing AndAlso dsRecords.Tables.Count > 0 Then

                Dim recordsTable As DataTable = dsRecords.Tables(0)
                Dim cboTree As RadTreeView = TryCast(rcbTreeview.Items(0).FindControl("rcbTree"), RadTreeView)


                If cboTree.Nodes.Count <= 0 Then


                    cboTree.CheckBoxes = UseCheckBoxes
                    cboTree.MultipleSelect = Multipleselect
                    cboTree.TriStateCheckBoxes = TriStateCheckBoxes
                    cboTree.ExpandAllNodes()

                    cboTree.DataFieldID = DataFieldID
                    cboTree.DataFieldParentID = DataFieldParentID
                    cboTree.DataTextField = DataTextField
                    cboTree.DataValueField = DataValueField
                    cboTree.AppendDataBoundItems = True
                    cboTree.DataSource = recordsTable
                    cboTree.DataBind()

                End If

                If ExpandAllNodes Then

                    For Each node As Telerik.Web.UI.RadTreeNode In cboTree.Nodes

                        node.Expanded = True
                        node.ExpandChildNodes()

                        If node.Nodes.Count > 0 Then node.ExpandChildNodes()

                    Next

                End If

            End If

        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub RcbTree_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim prjTree As RadTreeView = TryCast(rcbTreeview.Items(0).FindControl("rcbTree"), RadTreeView)

        If Not Me.IsPostBack Then
            For Each itm As RadTreeNode In prjTree.Nodes
                If itm.Nodes.Count > 0 Then
                    ' perform this check on child nodes
                    itm.Font.Bold = True
                End If
            Next


        End If
    End Sub

    Private Function GetSelectedIds(ByVal UseCheckBoxes As Boolean) As String
        Try

            Dim theTree As RadTreeView = TryCast(rcbTreeview.Items(0).FindControl("rcbTree"), RadTreeView)
            Dim strBuilder As New StringBuilder


            If UseCheckBoxes Then
                Dim CheckedNodes As List(Of Telerik.Web.UI.RadTreeNode) = theTree.CheckedNodes.ToList()

                For Each nd As RadTreeNode In CheckedNodes
                    strBuilder.Append(nd.Value.ToString())
                    strBuilder.Append(",")
                Next
                Dim value As String = strBuilder.ToString
                Return If(value.Length > 0, value.Substring(0, value.Length - 1), String.Empty)

            Else
                Dim SelectedNodes As List(Of Telerik.Web.UI.RadTreeNode) = theTree.SelectedNodes.ToList()

                For Each nd As RadTreeNode In SelectedNodes
                    strBuilder.Append(nd.Value.ToString())
                    strBuilder.Append(",")
                Next

                Dim value As String = strBuilder.ToString
                Return If(value.Length > 0, value.Substring(0, value.Length - 1), String.Empty)

            End If


        Catch ex As Exception
            Log.Error(ex)
            Return String.Empty
        End Try
    End Function

    Public Sub SetCheckBoxValues(ByVal drRecords As DataSet, ByVal DataValueField As String)
        ' First clear all checks.  
        Try

            Dim text As String = String.Empty
            Dim theTree As RadTreeView = TryCast(rcbTreeview.Items(0).FindControl("rcbTree"), RadTreeView)

            'For Each item As RadComboBoxItem In rcbTreeview.Items
            '    Dim chkbox As CheckBox = DirectCast(item.FindControl("rcbTree"), CheckBox)
            '    If chkbox IsNot Nothing Then
            '        chkbox.Checked = False
            '    End If
            'Next

            If Not IsNothing(drRecords) AndAlso drRecords.Tables.Count > 0 AndAlso drRecords.Tables(0).Rows.Count Then
                ' Find each item in the list and set the check and combo text value.
                For Each dr As DataRow In drRecords.Tables(0).Rows
                    'Dim item As RadComboBoxItem = rcbTreeview.FindItemByValue(dr(DataValueField))

                    Dim node As Telerik.Web.UI.RadTreeNode = theTree.Nodes.FindNodeByValue(dr(DataValueField))

                    For Each tNode As Telerik.Web.UI.RadTreeNode In theTree.Nodes

                        If tNode.Nodes.Count > 0 Then ' Determine if the  the node has child nodes as well

                            tNode.Expanded = True

                            For Each cNode As Telerik.Web.UI.RadTreeNode In tNode.Nodes

                                If cNode.Value = dr(DataValueField) Then

                                    If text.Equals(String.Empty) Then
                                        text = String.Format("{0}", cNode.Text)
                                    Else
                                        text = String.Format("{0}{1} {2}", text, Delimiter, cNode.Text)
                                    End If

                                    cNode.Checked = True

                                End If
                            Next
                        End If

                        If tNode.Value = dr(DataValueField) Then

                            If text.Equals(String.Empty) Then
                                text = String.Format("{0}", tNode.Text)
                            Else
                                text = String.Format("{0}{1} {2}", text, Delimiter, tNode.Text)
                            End If

                            tNode.Checked = True

                        End If
                    Next

                Next
            End If

            rcbTreeview.Text = text

        Catch ex As Exception

            Log.Error(ex)

        End Try
    End Sub

    Private Function GetSelectedSingleTreeValues() As String
        Dim t As String = rcbTreeview.Text
        Return t
    End Function


End Class