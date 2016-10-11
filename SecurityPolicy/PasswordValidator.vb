Public Class PasswordValidator

    Public Shared Function isAphaNumeric(ByVal password As String, ByVal minLength As Integer, ByVal maxLength As Integer) As Boolean

        Dim hasLetter As Boolean = False
        Dim hasDecimalDigit As Boolean = False

        Dim meetsLengthRequirements As Boolean = password.Length >= minLength And password.Length <= maxLength

        If meetsLengthRequirements Then

            For Each c As Char In password
                If Char.IsLetter(c) Then hasLetter = True
                If Char.IsDigit(c) Then hasDecimalDigit = True
            Next


        End If

        Dim isValid As Boolean = hasLetter And hasDecimalDigit
        Return isValid

    End Function

    Public Shared Function isAphaNumericWithSpecialChar(ByVal password As String, ByVal minLength As Integer, ByVal maxLength As Integer) As Boolean

        Dim hasLetter As Boolean = False
        Dim hasDecimalDigit As Boolean = False
        Dim hasSymbol As Boolean = False

        Dim meetsLengthRequirements As Boolean = password.Length >= minLength And password.Length <= maxLength

        If meetsLengthRequirements Then

            For Each c As Char In password
                If Char.IsLetter(c) Then hasLetter = True
                If Char.IsDigit(c) Then hasDecimalDigit = True
                If isSpecialChar(c) Then hasSymbol = True
            Next

        End If

        Dim isValid As Boolean = hasLetter And hasDecimalDigit And hasSymbol
        Return isValid

    End Function


    Public Shared Function isAphaNumericStartEndSpecialChar(ByVal password As String, ByVal minLength As Integer, ByVal maxLength As Integer) As Boolean

        Dim hasLetter As Boolean = False
        Dim hasDecimalDigit As Boolean = False
        Dim hasSymbol As Boolean = False

        Dim meetsLengthRequirements As Boolean = password.Length >= minLength And password.Length <= maxLength


        If meetsLengthRequirements And startsAndEndsWithSymbol(password) Then

            For Each c As Char In password
                If Char.IsLetter(c) Then hasLetter = True
                If Char.IsDigit(c) Then hasDecimalDigit = True
                If isSpecialChar(c) Then hasSymbol = True
            Next

        End If

        Dim isValid As Boolean = hasLetter And hasDecimalDigit And hasSymbol
        Return isValid

    End Function


    Private Shared Function startsAndEndsWithSymbol(ByVal password As String) As Boolean

        Dim pwdLength As Integer = 0
        Dim startChar As Char
        Dim endChar As Char

        pwdLength = password.Length
        startChar = password.Substring(0, 1)
        endChar = password.Substring(pwdLength - 1, 1)

        Dim meetsSymbolRequirements As Boolean = False
        If isSpecialChar(startChar) And isSpecialChar(endChar) Then meetsSymbolRequirements = True

        Return meetsSymbolRequirements

    End Function

    Private Shared Function isSpecialChar(ByVal character As Char) As Boolean

        Dim bASCII As Integer
        bASCII = Asc(character)

        If (bASCII >= 32 And bASCII <= 47) Or (bASCII >= 58 And bASCII <= 64) Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
