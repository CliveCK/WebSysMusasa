
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

        ' Starts with protocol
        Protected Shared rgxScheme As New Regex("^\w+://", RegexOptions.Compiled)

        Public Shared Function ResolveToFullUrl(ByVal url As String, ByVal excludePortIf80 As Boolean) As String

            Dim r As HttpRequest = HttpContext.Current.Request

            If r Is Nothing Then
                Throw New InvalidOperationException("This method only works in a web environment.")
            End If

            If rgxScheme.IsMatch(url) Then
                Return url
            End If

            Dim withPort As String = ""
            If excludePortIf80 AndAlso r.Url.Port = 80 Then
                withPort = ""
            Else
                withPort = ":" & r.Url.Port.ToString
            End If

            Return r.Url.Scheme & "://" & r.Url.Host & withPort & (New System.Web.UI.Control()).ResolveUrl(url)

        End Function

    End Class

    Public Shared Function IsLoginPage() As Boolean

        Return System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath).ToLower().Contains("login.aspx")

    End Function

    Public Shared Function IsASPXPageRequest() As Boolean

        Return HttpContext.Current.Request.Url.AbsoluteUri.ToLower().IndexOf(".aspx") > -1

    End Function

    Public Shared Function GetUserIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function

End Class
