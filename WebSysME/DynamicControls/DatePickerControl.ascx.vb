Partial Public Class DatePickerControl
    Inherits System.Web.UI.UserControl

    Private Sub DatePickerControl1_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'If (cboDateRange.Attributes("onchange") = "") Then

        '    cboDateRange.Attributes.Add("onchange", "javascript:" & ClientID & "_setDateCollector();")

        'End If

    End Sub

    Public ReadOnly Property ReportCriteria(ByVal DBField As String) As String
        Get

            Return GenerateCrysalDate(DBField)

        End Get

    End Property

    Protected Sub Page_LoadDatePicker(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Write(GenerateScript)

        If Not Page.IsPostBack Then

            'UserInterfaceHelper.SetDateFormat(radStartDate)
            'UserInterfaceHelper.SetDateFormat(radEndDate)

        End If

        If Not Page.ClientScript.IsStartupScriptRegistered(Me.GetType, ClientID & "_clearDateCollector") Then

            'radStartDate.SelectedDate = Today
            radStartDate.MinDate = CDate("1/1/1900") ' Date.MinValue
            Dim maxdate As Date = DateAdd(DateInterval.Year, 20, Today)
            radStartDate.MaxDate = maxdate 'WHY!!!

            'radEndDate.SelectedDate = Today
            radEndDate.MinDate = CDate("1/1/1900") ' Date.MinValue
            radEndDate.MaxDate = Today

            Page.ClientScript.RegisterStartupScript(Me.GetType, ClientID & "_clearDateCollector", ClientID & "_clearDateCollector();", True)
            Page.ClientScript.RegisterStartupScript(Me.GetType, ClientID & "_setDateCollector", ClientID & "_setDateCollector();", True)
            If (cboDateRange.Attributes("onchange") = "") Then

                cboDateRange.Attributes.Add("onchange", "javascript:" & ClientID & "_setDateCollector();")

            End If

        End If

    End Sub

    Public Function SetSelectedValues(ByVal Values As String)

        'TODO: Flanagan do your thing here!!!
        'this function sets selected values for these controls
        '1 - Remove the items from the AvailableOptions
        '2 - Add the items to the SelectedOptions (unless they already exist, in which case, Select them)

        Return "" 'Should return any values that it failed to assign i.e It failed to find them in either the Available of Selected items lists

    End Function

    Public Function GenerateCrysalDate(ByVal DBField As String) As String

        Dim DateValue As String = ""
        Dim Theday As String = Date.Today.AddDays(1).ToString("dd/MMM/yyyy")

        Select Case cboDateRange.SelectedValue

            Case "On"

                If IsDate(radStartDate.SelectedDate) Then

                    DateValue = DBField & " = Date('" & String.Format("{0:d/MMM/yy}", radStartDate.SelectedDate) & "')"

                End If

            Case "Between"

                If IsDate(radStartDate.SelectedDate) Then
                    If IsDate(radEndDate.SelectedDate) Then
                        DateValue = DBField & " IN  Date('" & String.Format("{0:d/MMM/yy}", radStartDate.SelectedDate) & "') TO Date('" & String.Format("{0:d/MMM/yy}", radEndDate.SelectedDate) & "')"
                    End If
                End If

            Case "After"

                If IsDate(radStartDate.SelectedDate) Then
                    DateValue = DBField & " > #" & String.Format("{0:d/MMM/yy}", radStartDate.SelectedDate) & "#"
                End If

            Case "Before"

                If IsDate(radStartDate.SelectedDate) Then
                    DateValue = DBField & " < #" & String.Format("{0:d/MMM/yy}", radStartDate.SelectedDate) & "#"
                End If

            Case "Last"

                If IsNumeric(txtInthe.Text) Then

                    Select Case cboInthe.SelectedValue
                        Case "dd"

                            Dim days As Double = -1 * txtInthe.Text

                            DateValue = DBField & "> #" & Date.Today.AddDays(days) & "# AND " & DBField & " < #" & Theday & "#"

                        Case "ww"

                            Dim Weeks As Double = -1 * txtInthe.Text * 7

                            DateValue = DBField & "> #" & Date.Today.AddDays(Weeks) & "# AND " & DBField & " < #" & Theday & "#"

                        Case "mm"

                            Dim Months As Long = -1 * txtInthe.Text

                            DateValue = DBField & "> #" & Date.Today.AddMonths(Months) & "# AND " & DBField & " < #" & Theday & "#"

                        Case "yy"
                            Dim Years As Long = -1 * txtInthe.Text

                            DateValue = DBField & "> #" & Date.Today.AddYears(Years) & "# AND " & DBField & " < #" & Theday & "#"
                    End Select

                End If


        End Select

        Return DateValue

    End Function

    Public Function GenerateDatabaseDate(ByVal DBField As String) As String

        Dim DateValue As String = ""
        Dim Theday As String = Date.Today.AddDays(1).ToString("dd/MMM/yyyy")

        Select Case cboDateRange.SelectedValue

            Case "On"

                If radStartDate.SelectedDate.HasValue Then

                    DateValue = DBField & " = '" & radStartDate.SelectedDate.Value.ToString("dd/MMM/yyyy") & "' "

                End If

            Case "Between"

                If radStartDate.SelectedDate.HasValue Then
                    If radEndDate.SelectedDate.HasValue Then
                        DateValue = "(" & DBField & " BETWEEN '" & radStartDate.SelectedDate.Value.ToString("dd/MMM/yyyy") & "' AND '" & radEndDate.SelectedDate.Value.ToString("dd/MMM/yyyy") & "') "
                    End If
                End If

            Case "After"

                If radStartDate.SelectedDate.HasValue Then
                    DateValue = DBField & " > '" & radStartDate.SelectedDate.Value.ToString("dd/MMM/yyyy") & "' "
                End If

            Case "Before"

                If radStartDate.SelectedDate.HasValue Then
                    DateValue = DBField & " < '" & radStartDate.SelectedDate.Value.ToString("dd/MMM/yyyy") & "' "
                End If

            Case "Last"

                If IsNumeric(txtInthe.Text) Then

                    Select Case cboInthe.SelectedValue
                        Case "dd"

                            Dim days As Double = -1 * txtInthe.Text

                            DateValue = DBField & " > '" & Date.Today.AddDays(days) & "' AND " & DBField & " < '" & Theday & "'"

                        Case "ww"

                            Dim Weeks As Double = -1 * txtInthe.Text * 7

                            DateValue = DBField & " > '" & Date.Today.AddDays(Weeks) & "' AND " & DBField & " < '" & Theday & "'"

                        Case "mm"

                            Dim Months As Long = -1 * txtInthe.Text

                            DateValue = DBField & " > '" & Date.Today.AddMonths(Months) & "' AND " & DBField & " < '" & Theday & "'"

                        Case "yy"
                            Dim Years As Long = -1 * txtInthe.Text

                            DateValue = DBField & " > '" & Date.Today.AddYears(Years) & "' AND " & DBField & " < '" & Theday & "'"

                    End Select

                    DateValue = "(" & DateValue & ")"

                End If

        End Select

        Return DateValue

    End Function

    Public Function GenerateScript() As String

        Dim script As String = ""

        script &= "<script type='text/javascript'>" & vbCrLf
        script &= "" & vbCrLf
        script &= "    function " & ClientID & "_setDateCollectorOptions (lblAND, radStartDate, radEndDate, cboInThe, txtInThe)" & vbCrLf
        script &= "    {" & vbCrLf
        script &= "        if ( document.getElementById('" & lblAnd.ClientID & "') )  {document.getElementById('" & lblAnd.ClientID & "').style.display=lblAND;}" & vbCrLf
        script &= "        if ( document.getElementById('" & radStartDate.ClientID & "_wrapper') ) {document.getElementById('" & radStartDate.ClientID & "_wrapper').style.display=radStartDate;}" & vbCrLf
        script &= "        if ( document.getElementById('" & radEndDate.ClientID & "_wrapper') ) { document.getElementById('" & radEndDate.ClientID & "_wrapper').style.display=radEndDate;}	" & vbCrLf
        script &= "        if ( document.getElementById('" & cboInthe.ClientID & "') ) { document.getElementById('" & cboInthe.ClientID & "').style.display=cboInThe;}" & vbCrLf
        script &= "        if ( document.getElementById('" & txtInthe.ClientID & "') ) {document.getElementById('" & txtInthe.ClientID & "').style.display=txtInThe;}" & vbCrLf
        script &= "    }" & vbCrLf
        script &= "" & vbCrLf
        script &= "    function " & ClientID & "_setDateCollector()" & vbCrLf
        script &= "    {if(document.getElementById('" & cboDateRange.ClientID & "'))" & vbCrLf
        script &= "      {" & vbCrLf
        script &= "        switch(document.getElementById('" & cboDateRange.ClientID & "').value)" & vbCrLf
        script &= "        {" & vbCrLf
        script &= "            case 'On':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','inline','none','none','none');" & vbCrLf
        script &= "            break;" & vbCrLf
        script &= "            case 'Before':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','inline','none','none','none');" & vbCrLf
        script &= "            break; " & vbCrLf
        script &= "            case 'After':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','inline','none','none','none');" & vbCrLf
        script &= "            break; " & vbCrLf
        script &= "            case 'Between':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('inline','inline','inline','none','none');" & vbCrLf
        script &= "            break; " & vbCrLf
        script &= "            case 'Last':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','none','none','inline','inline');" & vbCrLf
        script &= "            break;" & vbCrLf
        script &= "            default:" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','none','none','none','none');" & vbCrLf
        script &= "            break;" & vbCrLf
        script &= "        }" & vbCrLf
        script &= "     }" & vbCrLf
        script &= "    }" & vbCrLf
        script &= "" & vbCrLf
        script &= "    function " & ClientID & "_clearDateCollector()" & vbCrLf
        script &= "    {" & vbCrLf
        script &= "         if ( document.getElementById('" & lblAnd.ClientID & "') ) {document.getElementById('" & lblAnd.ClientID & "').style.display='none';}" & vbCrLf
        script &= "        if ( document.getElementById('" & radStartDate.ClientID & "_wrapper') ) { document.getElementById('" & radStartDate.ClientID & "_wrapper').style.display='none';}" & vbCrLf
        script &= "        if ( document.getElementById('" & radEndDate.ClientID & "_wrapper') ) { document.getElementById('" & radEndDate.ClientID & "_wrapper').style.display='none';	}" & vbCrLf
        script &= "        if ( document.getElementById('" & cboInthe.ClientID & "') ) { document.getElementById('" & cboInthe.ClientID & "').style.display='none';}" & vbCrLf
        script &= "       if ( document.getElementById('" & txtInthe.ClientID & "') ) { document.getElementById('" & txtInthe.ClientID & "').style.display='none';}" & vbCrLf
        script &= "    }" & vbCrLf
        script &= "" & vbCrLf
        script &= "</script>" & vbCrLf
        script &= "" & vbCrLf

        Return script

    End Function

    Private Function ControlDisplayScript() As String

        Dim script As String = ""

        script &= "<script type='text/javascript'>" & vbCrLf
        script &= "" & vbCrLf
        script &= "        switch(document.getElementById('" & cboDateRange.ClientID & "').value)" & vbCrLf
        script &= "        {" & vbCrLf
        script &= "            case 'On':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','inline','none','none','none');" & vbCrLf
        script &= "            break;" & vbCrLf
        script &= "            case 'Before':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','inline','none','none','none');" & vbCrLf
        script &= "            break; " & vbCrLf
        script &= "            case 'After':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','inline','none','none','none');" & vbCrLf
        script &= "            break; " & vbCrLf
        script &= "            case 'Between':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('inline','inline','inline','none','none');" & vbCrLf
        script &= "            break; " & vbCrLf
        script &= "            case 'Last':" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','none','none','inline','inline');" & vbCrLf
        script &= "            break;" & vbCrLf
        script &= "            default:" & vbCrLf
        script &= "            " & ClientID & "_setDateCollectorOptions('none','none','none','none','none');" & vbCrLf
        script &= "            break;" & vbCrLf
        script &= "        }" & vbCrLf
        script &= "</script>" & vbCrLf

        Return script

    End Function

End Class