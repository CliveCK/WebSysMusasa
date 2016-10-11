Imports System.Text.RegularExpressions

Public Class WebHelper

    Public Class UrlResolver

        'http://ikhwanhayat.net/2008/01/resolving-paths-into-full-url/

        'Input

        '~/Abc
        '~/Abc/Def.aspx?ghi=jkl&mno=p|q
        '/Abc
        'Abc/Def.aspx
        '~/Abc/Def.aspx?ghi=jkl
        '/123/~/Abc
        '../Def.aspx
        'http://abc/def/ghi.php

        'Output

        'http://localhost/appbase/Abc
        'http://localhost/appbase/Abc/Def.aspx?ghi=jkl&mno=p|q
        'http://localhost/Abc
        'http://localhost/appbase/TestFolder/Abc/Def.aspx
        'http://localhost/appbase/Abc/Def.aspx?ghi=jkl
        'http://localhost/123/~/Abc
        'http://localhost/appbase/Def.aspx
        'http://abc/def/ghi.php

        Public Shared Function ResolveToFullUrl(ByVal url As String) As String

            Return ResolveToFullUrl(url, True)

        End Function

        Public Shared Function GetCurrentPageName() As String

            Return New System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name

        End Function

        ' Starts with protocol
        Protected Shared rgxScheme As New Regex("^\w+://", RegexOptions.Compiled)

        Public Shared Function ResolveToFullUrl(ByVal url As String, ByVal excludePortIf80 As Boolean) As String

            Dim r As Web.HttpRequest = Web.HttpContext.Current.Request

            If r Is Nothing Then
                Throw New InvalidOperationException("This method only works in a web environment.")
            End If

            If rgxScheme.IsMatch(url) Then
                Return url
            End If

            Dim withPort As String = IIf((excludePortIf80 AndAlso r.Url.Port = 80), "", ":" & r.Url.Port)

            Return r.Url.Scheme & "://" & r.Url.Host & withPort & (New System.Web.UI.Control).ResolveUrl(url)

        End Function

    End Class

End Class
