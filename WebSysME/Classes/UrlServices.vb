Imports System.Security.Cryptography
Imports System.IO

Namespace Security.SpecialEncryptionServices

    Public Class UrlServices
        Public Class EncryptDecryptQueryString

            Private key() As Byte = {}
            Private sEncryptionKey As String = "CK0903TY"

            Private IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

            Public Function Decrypt(ByVal stringToDecrypt As String) As String

                stringToDecrypt = stringToDecrypt.Replace(" ", "+")

                Dim inputByteArray() As Byte = New Byte(stringToDecrypt.Length + 1) {}

                Try
                    key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey)

                    Dim des As DESCryptoServiceProvider = New DESCryptoServiceProvider()
                    Dim ms As MemoryStream = New MemoryStream()
                    Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
                    Dim cs As CryptoStream

                    inputByteArray = Convert.FromBase64String(stringToDecrypt)
                    cs = New CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write)
                    cs.Write(inputByteArray, 0, inputByteArray.Length)
                    cs.FlushFinalBlock()

                    Return encoding.GetString(ms.ToArray())

                Catch e As Exception

                    Return e.Message

                End Try

            End Function

            Public Function Encrypt(ByVal stringToEncrypt As String) As String
                Try
                    key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey)

                    Dim des As DESCryptoServiceProvider = New DESCryptoServiceProvider()
                    Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(stringToEncrypt)
                    Dim ms As MemoryStream = New MemoryStream()
                    Dim cs As CryptoStream

                    cs = New CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write)
                    cs.Write(inputByteArray, 0, inputByteArray.Length)
                    cs.FlushFinalBlock()

                    Return Convert.ToBase64String(ms.ToArray())

                Catch e As Exception

                    Return e.Message

                End Try
            End Function

        End Class


    End Class

End Namespace
